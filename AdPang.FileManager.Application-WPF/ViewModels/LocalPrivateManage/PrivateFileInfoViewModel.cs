using System;
using System.Collections.ObjectModel;
using System.Linq;
using AdPang.FileManager.Application_WPF.Extensions;
using AdPang.FileManager.Application_WPF.Services.IServices;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateFile;
using DryIoc;
using HttpRequestClient.Services.IRequestServices;
using Prism.Commands;
using Prism.Ioc;

namespace AdPang.FileManager.Application_WPF.ViewModels.LocalPrivateManage
{
    public class PrivateFileInfoViewModel : NavigationViewModel
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="containerProvider"></param>
        public PrivateFileInfoViewModel(IContainerProvider containerProvider, IPrivateFileInfoRequestService privateFileInfoRequestService, IPrivateDiskRequestService privateDiskRequestService) : base(containerProvider)
        {
            dialogHostService = containerProvider.Resolve<IDialogHostService>();
            this.privateFileInfoRequestService = privateFileInfoRequestService;
            this.privateDiskRequestService = privateDiskRequestService;
            _aggregator.ResgiterFileInfoMessage(message =>
            {
                var diskId = message.DiskId;
                var disk = DiskInfoDtos.Where(x => x.Id.Equals(diskId)).FirstOrDefault();
                if (disk != null)
                    DiskSeletedItem = disk;
                else
                {
                    RefreshDiskListModel();
                }
            });
            //PageSize改变事件
            PageSizeChangedCommand = new DelegateCommand(() =>
            {
                CurrentPage = 1;
                RefreshFileListModel();
            });
            //上一页
            PrePageCommand = new DelegateCommand(() =>
            {
                if (CurrentPage <= 1) return;
                CurrentPage--;
                RefreshFileListModel();
            });
            //下一页
            NextPageCommand = new DelegateCommand(() =>
            {
                if (CurrentPage >= PageCount) return;
                CurrentPage++;
                RefreshFileListModel();
            });
            //第一页
            FirstPageCommand = new DelegateCommand(() =>
            {
                CurrentPage = 1;
                RefreshFileListModel();
            });
            //最后一页
            LastPageCommand = new DelegateCommand(() =>
            {
                CurrentPage = PageCount;
                RefreshFileListModel();
            });
            //
            DiskSelectedChanged = new DelegateCommand(() =>
            {
                RefreshFileListModel();
            });
            RefreshDiskListModel();
            RefreshFileListModel();
        }
        #endregion

        #region 字段、属性

        private readonly IDialogHostService dialogHostService;
        private readonly IPrivateFileInfoRequestService privateFileInfoRequestService;
        private readonly IPrivateDiskRequestService privateDiskRequestService;
        private ObservableCollection<PrivateFileInfoDto> fileInfoDtos = new();
        private ObservableCollection<PrivateDiskInfoDto> diskInfoDtos = new();
        private PrivateDiskInfoDto diskSeletedItem;
        private bool isFindDuplicate = false;

        public bool IsFindDuplicate
        {
            get { return isFindDuplicate; }
            set
            {
                isFindDuplicate = value;
                requestMode = value ? Shared.Paremeters.RequestMode.Distinct : Shared.Paremeters.RequestMode.Default;
                RefreshFileListModel();
                RaisePropertyChanged();
            }
        }


        public PrivateDiskInfoDto DiskSeletedItem
        {
            get { return diskSeletedItem; }
            set { diskSeletedItem = value; RaisePropertyChanged(); }
        }


        public ObservableCollection<PrivateDiskInfoDto> DiskInfoDtos
        {
            get { return diskInfoDtos; }
            set { diskInfoDtos = value; RaisePropertyChanged(); }
        }

        private int pageSize = 10;
        private int pageCount = 1;
        private int currentPage = 1;
        private Shared.Paremeters.RequestMode requestMode = Shared.Paremeters.RequestMode.Default;

        public int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; RaisePropertyChanged(nameof(CurrentPage)); }
        }


        public int PageCount
        {
            get { return pageCount; }
            set { pageCount = value; RaisePropertyChanged(nameof(PageCount)); }
        }


        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; RaisePropertyChanged(nameof(PageSize)); }
        }


        public ObservableCollection<PrivateFileInfoDto> FileInfoDtos
        {
            get { return fileInfoDtos; }
            set { fileInfoDtos = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// PageSize改变事件
        /// </summary>
        public DelegateCommand PageSizeChangedCommand { get; private set; }
        /// <summary>
        /// 第一页
        /// </summary>
        public DelegateCommand FirstPageCommand { get; private set; }
        /// <summary>
        /// 最后一页
        /// </summary>
        public DelegateCommand LastPageCommand { get; private set; }
        /// <summary>
        /// 上一页
        /// </summary>
        public DelegateCommand PrePageCommand { get; private set; }
        /// <summary>
        /// 跳转指定页
        /// </summary>
        public DelegateCommand GotoPageCommand { get; private set; }
        /// <summary>
        /// 下一页
        /// </summary>
        public DelegateCommand NextPageCommand { get; private set; }
        /// <summary>
        /// 硬盘选中改变事件
        /// </summary>
        public DelegateCommand DiskSelectedChanged { get; set; }
        #endregion

        #region 方法
        private async void RefreshFileListModel()
        {
            try
            {
                UpdateLoading(true);
                var getRequestResult = await privateFileInfoRequestService.GetAllFileInfoAsync(new Shared.Paremeters.PrivateFileInfoQueryParameter
                {
                    PageIndex = CurrentPage - 1,
                    PageSize = PageSize,
                    DiskId = DiskSeletedItem == null || DiskSeletedItem.Id == null ? null : DiskSeletedItem.Id.ToString(),
                    RequestMode = requestMode
                });
                if (!getRequestResult.Status)
                {
                    _aggregator.SendMessage("获取数据错误：" + getRequestResult.Message);
                    return;
                }
                PageCount = getRequestResult.Result.TotalPages;
                CurrentPage = getRequestResult.Result.PageIndex + 1;

                FileInfoDtos.Clear();
                FileInfoDtos.AddRange(getRequestResult.Result.Items);

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

        private async void RefreshDiskListModel()
        {
            try
            {
                UpdateLoading(true);
                var getDisksRequestResult = await privateDiskRequestService.GetAllAsync(new Shared.Paremeters.QueryParameter
                {
                    PageIndex = 0,
                    PageSize = int.MaxValue,
                });
                if (!getDisksRequestResult.Status)
                {
                    _aggregator.SendMessage("请求错误：" + getDisksRequestResult.Message);
                    return;
                }
                DiskInfoDtos.Clear();
                DiskInfoDtos.Add(new PrivateDiskInfoDto
                {
                    Id = null,
                    DiskName = "全部",
                    DiskSN = "全部"
                });
                DiskInfoDtos.AddRange(getDisksRequestResult.Result.Items);
                DiskSeletedItem = DiskInfoDtos.First();
            }
            catch (Exception e)
            {
                _aggregator.SendMessage("发生错误：" + e.Message);
                return;
            }
            finally
            {
                UpdateLoading(false);
            }
        }
        #endregion
    }
}
