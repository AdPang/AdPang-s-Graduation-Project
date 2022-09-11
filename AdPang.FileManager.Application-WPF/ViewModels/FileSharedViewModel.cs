using System;
using System.Collections.ObjectModel;
using System.Linq;
using AdPang.FileManager.Application_WPF.Extensions;
using AdPang.FileManager.Application_WPF.Services.IServices;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.SharedFileInfo;
using HttpRequestClient.Services.IRequestServices;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;

namespace AdPang.FileManager.Application_WPF.ViewModels
{
    public class FileSharedViewModel : NavigationViewModel
    {
        #region 字段、属性
        private readonly ISharedInfoRequestService sharedInfoRequestService;
        private readonly IDialogHostService dialogHostService;
        private ObservableCollection<SharedFileInfoDetailDto> sharedFiles = new();
        private SharedFileInfoDetailDto currentSelectShareInfo;

        public SharedFileInfoDetailDto CurrentSelectShareInfo
        {
            get { return currentSelectShareInfo; }
            set { currentSelectShareInfo = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<string> ExecuteCommand { get; set; }

        public ObservableCollection<SharedFileInfoDetailDto> SharedFiles
        {
            get { return sharedFiles; }
            set { sharedFiles = value; RaisePropertyChanged(); }
        }


        #endregion

        #region 构造
        public FileSharedViewModel(IContainerProvider containerProvider, ISharedInfoRequestService sharedInfoRequestService, IDialogHostService dialogHostService) : base(containerProvider)
        {
            //注册消息通知
            _aggregator.ResgiterFileSharedMessage(async sharedInfo =>
            {
                //发送添加请求
                var addRequsetResult = await sharedInfoRequestService.AddAsync(sharedInfo);
                if (!addRequsetResult.Status)
                {
                    _aggregator.SendMessage("请求出错：" + addRequsetResult.Message);
                    return;
                }
                _aggregator.SendMessage("添加分享成功！", "DirView");
                //成功，添加数据到集合
                SharedFiles.Add(addRequsetResult.Result);
            });
            //获取文件分享数据
            ExecuteCommand = new DelegateCommand<string>(Execute);

            this.sharedInfoRequestService = sharedInfoRequestService;
            this.dialogHostService = dialogHostService;
            InitSharedInfoList();
        }


        #endregion

        #region 方法
        private void Execute(string commStr)
        {
            switch (commStr)
            {
                case "ShareDetail":
                    ShowShareInfo(false);
                    break;
                case "ShareEdit":
                    ShowShareInfo(true);
                    break;
                case "ShareDelete":
                    ShareDelete();
                    break;
                case "Refresh":
                    Refresh();
                    break;
                default:
                    break;
            }
        }

        private void Refresh()
        {
            try
            {
                UpdateLoading(true);
                SharedFiles.Clear();
                InitSharedInfoList();
            }
            catch (Exception e)
            {

                _aggregator.SendMessage(e.Message);
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private async void ShareDelete()
        {
            var dialogResult = await dialogHostService.Question("温馨提示", "是否删除该分享?");
            if (dialogResult.Result != ButtonResult.OK || CurrentSelectShareInfo.Id == null) return;


            var deleteResult = await sharedInfoRequestService.DeleteAsync((Guid)CurrentSelectShareInfo.Id);
            if (!deleteResult.Status)
            {
                _aggregator.SendMessage("发生错误" + deleteResult.Message);
                return;
            }
            _aggregator.SendMessage("删除成功");
            SharedFiles.Remove(CurrentSelectShareInfo);
        }

        private async void ShowShareInfo(bool isEdit)
        {
            try
            {
                DialogParameters param = new();
                param.Add("Value", CurrentSelectShareInfo);
                param.Add("IsEdit", isEdit);
                var dialogResult = await dialogHostService.ShowDialog("OperaSharedInfoDialogView", param);
                if (!isEdit) return;

                if (dialogResult.Result != ButtonResult.OK) return;
                var editShare = dialogResult.Parameters.GetValue<SharedFileInfoDetailDto>("Value");

                var editRequestResult = await sharedInfoRequestService.UpdateAsync(editShare);
                if (!editRequestResult.Status)
                {
                    _aggregator.SendMessage("修改失败：" + editRequestResult.Message);
                    return;
                }
                _aggregator.SendMessage("修改成功");
                var share = SharedFiles.Where(x => x.Id.Equals(CurrentSelectShareInfo.Id)).FirstOrDefault();
                share = editRequestResult.Result;
            }
            catch (Exception e)
            {
                _aggregator.SendMessage("发生错误：" + e.Message);
            }
        }


        private async void InitSharedInfoList()
        {
            //发送获取我的文件分享列表请求
            var getAllRequestResult = await sharedInfoRequestService.GetAllAsync(new Shared.Paremeters.QueryParameter
            {
                PageIndex = 0,
                PageSize = int.MaxValue
            });

            if (!getAllRequestResult.Status)
            {
                _aggregator.SendMessage("请求出错：" + getAllRequestResult.Message);
                return;
            }
            //将数据添加到集合
            SharedFiles.AddRange(getAllRequestResult.Result.Items);

        }
        #endregion

    }
}
