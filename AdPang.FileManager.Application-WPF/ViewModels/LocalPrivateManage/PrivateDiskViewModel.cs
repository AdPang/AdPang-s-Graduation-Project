using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AdPang.FileManager.Application_WPF.Common.Models;
using AdPang.FileManager.Application_WPF.Extensions;
using AdPang.FileManager.Application_WPF.Services.IServices;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk;
using HttpRequestClient.Services.IRequestServices;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AdPang.FileManager.Application_WPF.ViewModels.LocalPrivateManage
{
    public class PrivateDiskViewModel : NavigationViewModel
    {
        #region 构造
        public PrivateDiskViewModel(IContainerProvider containerProvider, ILocalInfoService localInfoService, IPrivateDiskRequestService privateDiskRequestService, IRegionManager regionManager, PrivateFileInfoViewModel privateFileInfoViewModel) : base(containerProvider)
        {
            dialogHost = containerProvider.Resolve<IDialogHostService>();
            this.localInfoService = localInfoService;
            this.privateDiskRequestService = privateDiskRequestService;
            RefreshModelCommand = new DelegateCommand(RefreshModels);
            AddDiskInfoCommand = new DelegateCommand(AddDisk);
            DeleteDiskCommand = new DelegateCommand<PrivateDiskInfoDto>(DeleteDisk);
            EditDiskInfoCommand = new DelegateCommand<PrivateDiskInfoDto>(EditDisk);
            ToFileInfosViewCommand = new DelegateCommand<PrivateDiskInfoDto>(diskInfo =>
            {
                if (diskInfo.Id == null) return;
                _aggregator.SendFileInfosMessage(new Common.Events.FileInfosMessage
                {
                    DiskId = (Guid)diskInfo.Id
                });
                regionManager.Regions[PrismManager.LocalManageViewRegionName].RequestNavigate("PrivateFileInfoView");
            });

            RefreshModels();
        }

        #endregion
        #region 字段、属性


        public DelegateCommand<PrivateDiskInfoDto> DeleteDiskCommand { get; set; }
        public DelegateCommand<PrivateDiskInfoDto> EditDiskInfoCommand { get; set; }
        public DelegateCommand<PrivateDiskInfoDto> ToFileInfosViewCommand { get; set; }
        public DelegateCommand AddDiskInfoCommand { get; set; }
        public DelegateCommand RefreshModelCommand { get; set; }


        private ObservableCollection<PrivateDiskInfoDto> diskInfos = new();
        private ObservableCollection<LocalDiskInfo> localDiskInfos = new();
        private readonly ILocalInfoService localInfoService;
        private readonly IPrivateDiskRequestService privateDiskRequestService;
        private readonly IDialogHostService dialogHost;

        public ObservableCollection<LocalDiskInfo> LocalDiskInfos
        {
            get { return localDiskInfos; }
            set { localDiskInfos = value; RaisePropertyChanged(); }
        }


        public ObservableCollection<PrivateDiskInfoDto> DiskInfos
        {
            get { return diskInfos; }
            set { diskInfos = value; RaisePropertyChanged(); }
        }

        #endregion


        #region 方法
        private async void AddDisk()
        {
            await ShowDiskInfoDialog(null);
        }
        private async void DeleteDisk(PrivateDiskInfoDto diskInfo)
        {
            var questResult = await dialogHost.Question("温馨提示", $"是否删除硬盘：{diskInfo.DiskName}？");
            if (questResult.Result != ButtonResult.OK) return;
            if (diskInfo == null || diskInfo.Id == null)
            {
                _aggregator.SendMessage("发生错误,请刷新重试！");
                return;
            }
            var deleteRequestResult = await privateDiskRequestService.DeleteAsync((Guid)diskInfo.Id);
            if (!deleteRequestResult.Status)
            {
                _aggregator.SendMessage("发送请求错误：" + deleteRequestResult.Message);
                return;
            }
            _aggregator.SendMessage("删除成功！");
            DiskInfos.Remove(diskInfo);

        }
        private async void EditDisk(PrivateDiskInfoDto diskInfo)
        {
            await ShowDiskInfoDialog(diskInfo);
        }

        private async Task ShowDiskInfoDialog(PrivateDiskInfoDto? model)
        {
            DialogParameters param = new();
            if (model != null)
                param.Add("Value", model);
            var dialogResult = await dialogHost.ShowDialog("DiskInfoDialogView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var diskInfo = dialogResult.Parameters.GetValue<PrivateDiskInfoDto>("Value");
                if (diskInfo == null) return;
                try
                {
                    UpdateLoading(true);
                    if (diskInfo.Id != null)
                    {
                        //修改
                        var updateResult = await privateDiskRequestService.UpdateAsync(diskInfo);
                        if (!updateResult.Status)
                        {
                            _aggregator.SendMessage("更新失败：" + updateResult.Message);
                            return;
                        }
                        _aggregator.SendMessage("更新成功");
                        var disk = DiskInfos.Where(x => x.Id.Equals(updateResult.Result.Id)).FirstOrDefault();
                        if (disk == null) return;
                        disk.DiskName = updateResult.Result.DiskName;
                        disk.DiskSN = updateResult.Result.DiskSN;
                    }
                    else
                    {
                        var insertResult = await privateDiskRequestService.AddAsync(diskInfo);
                        if (!insertResult.Status)
                        {
                            _aggregator.SendMessage("新增失败：" + insertResult.Message);
                            return;
                        }
                        _aggregator.SendMessage("新增成功");
                        DiskInfos.Add(insertResult.Result);
                        //新增
                    }
                }
                catch (Exception e)
                {
                    _aggregator.SendMessage("发生错误：" + e.Message);
                }
                finally
                {
                    UpdateLoading(false);
                }
            }
        }



        private async void RefreshModels()
        {
            try
            {
                UpdateLoading(true);

                LocalDiskInfos.Clear();
                LocalDiskInfos.AddRange(localInfoService.GetLocalDiskInfos());

                var getsResult = await privateDiskRequestService.GetAllAsync(new Shared.Paremeters.QueryParameter
                {
                    PageIndex = 0,
                    PageSize = int.MaxValue,

                });
                if (!getsResult.Status)
                {
                    _aggregator.SendMessage("请求出错：" + getsResult.Message);
                    return;
                }
                DiskInfos.Clear();
                DiskInfos.AddRange(getsResult.Result.Items);

            }
            catch (Exception e)
            {
                _aggregator.SendMessage("发生错误：" + e.Message);
            }
            finally
            {
                UpdateLoading(false);
            }
        }
        #endregion
    }
}
