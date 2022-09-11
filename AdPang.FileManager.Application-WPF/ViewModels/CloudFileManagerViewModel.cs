using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AdPang.FileManager.Application_WPF.Extensions;
using HttpRequestClient.Services.IRequestServices;
using Prism.Ioc;
using System.Windows;
using Prism.Commands;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo;
using AdPang.FileManager.Application_WPF.Services.IServices;
using Prism.Services.Dialogs;
using Prism.Regions;
using AdPang.FileManager.Application_WPF.Common.Events;
using AdPang.FileManager.Application_WPF.Common.Helper;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.SharedFileInfo;
using System.IO;

namespace AdPang.FileManager.Application_WPF.ViewModels
{
    public class CloudFileManagerViewModel : NavigationViewModel
    {
        private readonly IUserCloudDirInfoRequestService userCloudDirInfoRequestService;
        private readonly IDialogHostService dialogHostService;
        private readonly IFileRequestService fileRequestService;
        private readonly IRegionManager regionManager;

        #region 指令
        public DelegateCommand<string> CommandExecute { get; set; }
        public DelegateCommand<DirInfoDetailDto> SelectedItemChangedCommand { get; set; }
        public DelegateCommand NavClickCommand { get; set; }

        #endregion

        #region 字段、属性
        private ObservableCollection<DirInfoDetailDto> navDirInfos = new();
        private DirInfoDetailDto currentSelectedItem;
        private ObservableCollection<DirInfoDetailDto> dirInfoDetailDtos = new();
        private Visibility operaVisibility = Visibility.Hidden;
        private DirInfoDetailDto navSelectedItem;
        private int selectedFileIndex = -1;
        private int selectedDirIndex = -1;

        public int SelectedDirIndex
        {
            get { return selectedDirIndex; }
            set { selectedDirIndex = value; RaisePropertyChanged(); }
        }
        public int SelectedFileIndex
        {
            get { return selectedFileIndex; }
            set { selectedFileIndex = value; RaisePropertyChanged(); }
        }

        public DirInfoDetailDto NavSelectedItem
        {
            get { return navSelectedItem; }
            set { navSelectedItem = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<DirInfoDetailDto> NavDirInfos
        {
            get { return navDirInfos; }
            set { navDirInfos = value; RaisePropertyChanged(); }
        }
        public Visibility OperaVisibility { get { return operaVisibility; } set { operaVisibility = value; RaisePropertyChanged(); } }

        public DirInfoDetailDto CurrentSelectedItem
        {
            get { return currentSelectedItem; }
            set { currentSelectedItem = value; UpdateNavDirInfo(); RaisePropertyChanged(); }
        }

        private void UpdateNavDirInfo()
        {
            Stack<DirInfoDetailDto> stackDetailDtos = new Stack<DirInfoDetailDto>();
            stackDetailDtos.Push(currentSelectedItem);
            var parentId = CurrentSelectedItem.ParentDirInfoId;
            while (parentId != null)
            {
                var tempDir = FindChildren(DirInfoDetailDtos.FirstOrDefault(), (Guid)parentId);
                if (tempDir == null) break;
                stackDetailDtos.Push(tempDir);
                parentId = tempDir.ParentDirInfoId;
            }
            NavDirInfos.Clear();
            var count = stackDetailDtos.Count;
            for (int i = 0; i < count; i++)
            {
                NavDirInfos.Add(stackDetailDtos.Pop());
            }
        }

        public ObservableCollection<DirInfoDetailDto> DirInfoDetailDtos
        {
            get { return dirInfoDetailDtos; }
            set { dirInfoDetailDtos = value; RaisePropertyChanged(); }
        }
        #endregion


        public CloudFileManagerViewModel(IContainerProvider containerProvider, IUserCloudDirInfoRequestService userCloudDirInfoRequestService, IDialogHostService dialogHostService, IFileRequestService fileRequestService, IRegionManager regionManager, FileSharedViewModel fileSharedViewModel) : base(containerProvider)
        {
            //ViewModelLocationProvider.Register<FileTransferListView, FileTransferListViewModel>();

            this.userCloudDirInfoRequestService = userCloudDirInfoRequestService;
            this.dialogHostService = dialogHostService;
            this.fileRequestService = fileRequestService;
            this.regionManager = regionManager;
            //regionManager.RegisterViewWithRegion("FileTransferListView", typeof(FileTransferListView));
            //var a = regionManager.RegisterViewWithRegion<FileTransferListView>("");
            CommandExecute = new DelegateCommand<string>(Execute);
            SelectedItemChangedCommand = new DelegateCommand<DirInfoDetailDto>(ChangeHandler);
            NavClickCommand = new DelegateCommand(NavClickMethod);

            GetDirInfo();


        }

        private void NavClickMethod()
        {
            if (NavSelectedItem != null)
                CurrentSelectedItem = NavSelectedItem;
        }

        private void ChangeHandler(DirInfoDetailDto dirTree)
        {
            if (dirTree != null)
                CurrentSelectedItem = dirTree;
        }

        private async void Execute(string para)
        {
            switch (para)
            {
                case "AddFile"://添加文件
                    AddFile();
                    break;
                case "EditFile"://编辑文件
                    await EditFile();
                    break;
                case "DeleteFile"://删除文件
                    DeleteFile();
                    break;
                case "AddDir"://添加文件夹
                    AddNewDirInfo();
                    break;
                case "EditDir"://编辑文件夹
                    EditDirInfo();
                    break;
                case "DownloadDir"://下载文件夹
                    DownloadDir();
                    break;
                case "DeleteDir"://删除文件夹
                    DeleteDirInfo();
                    break;
                case "GotoUpDir"://返回上一级
                    GotoUpDir();
                    break;
                case "DownloadFile"://下载文件
                    DownloadFile();
                    break;
                case "Refresh"://刷新
                    RefreshModel();
                    break;
                case "GotoTransferView"://去传输中心界面
                    regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("FileTransferListView");
                    break;
                case "ShareDir"://分享文件夹
                    ShareDir();
                    break;
                case "ShareFile"://分享文件
                    ShareFile();
                    break;
                case "GotoShareCenter"://分享中心
                    regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("FileSharedView");
                    break;
                case "UploadDir":
                    UploadDir();//上传文件夹
                    break;
                default:
                    break;
            }
        }

        private async void UploadDir()
        {
            if (CurrentSelectedItem.ParentDirInfoId == null)
            {
                _aggregator.SendMessage("无法在根目录下添加文件！", "DirView");
                return;
            }
            try
            {
                var dirPaht = DirSelectorDialogHelper.GetPathByFolderBrowserDialog();
                if (string.IsNullOrEmpty(dirPaht)) return;
                DirectoryInfo dirInfo = new(dirPaht);
                if (!dirInfo.Exists) return;
                UpdateLoading(true);

                var newDirResult = await userCloudDirInfoRequestService.AddAsync(new DirInfoDetailDto
                {
                    ParentDirInfoId = CurrentSelectedItem.Id,
                    DirName = dirInfo.Name
                });
                if (!newDirResult.Status)
                {
                    _aggregator.SendMessage("发生错误：" + newDirResult.Message, "DirView");
                    return;
                }
                CurrentSelectedItem.ChildrenDirInfo.Add(newDirResult.Result);
                CreatDir(dirInfo, (Guid)newDirResult.Result.Id);
            }
            catch (Exception e)
            {
                _aggregator.SendMessage("发生错误：" + e.Message,"DirView");
            }
            finally
            {
                UpdateLoading(false);
            }

        }


        private async void CreatDir(DirectoryInfo root,Guid parentDirId)
        {
            var parentDir = FindChildren(DirInfoDetailDtos.FirstOrDefault(), parentDirId);
            if (parentDir == null)
            {
                _aggregator.SendMessage("未找到父目录！", "DirView");
                return;
            }

            foreach (var file in root.GetFiles())
            {
                _aggregator.SendFileTransferMessage(new FileTransferMessage
                {
                    CurrentDirId = parentDirId,
                    FilePath = file.FullName,
                }, "Upload");

                parentDir.ChildrenFileInfo.Add(new UserPrivateFileInfoDto
                {
                    FileName = file.Name,
                    RealFileInfo = new Shared.Dtos.CloudSavedDto.CloudFileInfo.CloudFileInfoDto
                    {
                        FileType = file.Extension,
                        FileLength = file.Length,
                        UpdateTime = DateTime.Now,
                    }
                });
            }

            foreach (var dir in root.GetDirectories())
            {
                var newDirResult = await userCloudDirInfoRequestService.AddAsync(new DirInfoDetailDto
                {
                    ParentDirInfoId = parentDirId,
                    DirName = dir.Name
                });
                if (!newDirResult.Status)
                {
                    _aggregator.SendMessage("发生错误：" + newDirResult.Message, "DirView");
                    return;
                }
                if(newDirResult.Result.Id == null)
                {
                    _aggregator.SendMessage("发生错误：服务器发生错误，发生此问题请联系管理员！", "DirView");
                    return;
                }
                
                
                
                parentDir.ChildrenDirInfo.Add(newDirResult.Result);
                CreatDir(dir, (Guid)newDirResult.Result.Id);
            }
        }


        private void ShareDir()
        {
            if (CurrentSelectedItem.ParentDirInfoId == null)
            {
                _aggregator.SendMessage("无法在根目录下添加文件！", "DirView");
                return;
            }
            var dir = CurrentSelectedItem.ChildrenDirInfo[SelectedDirIndex];
            SharedFileInfoDetailDto sharedFileInfo = new()
            {
                IsSingleFile = false,
                DirId = dir.Id,
                DirInfo = dir,
            };
            SendAddShareFileInfoMessage(sharedFileInfo);

        }

        private void ShareFile()
        {
            var file = CurrentSelectedItem.ChildrenFileInfo[SelectedFileIndex];
            if (file.Id == null)
            {
                _aggregator.SendMessage("请刷新后重试！", "DirView");
            }
            SharedFileInfoDetailDto sharedFileInfo = new()
            {
                IsSingleFile = true,
                SingleFileId = file.Id,
                SingleFileInfo = file,
                HasExpired = false,
            };
            SendAddShareFileInfoMessage(sharedFileInfo);
        }

        private async void SendAddShareFileInfoMessage(SharedFileInfoDetailDto sharedFileInfoDetailDto)
        {
            DialogParameters param = new();
            param.Add("Value", sharedFileInfoDetailDto);
            param.Add("IsEdit", false);
            var dialogResult =  await dialogHostService.ShowDialog("OperaSharedInfoDialogView", param);
            if (dialogResult.Result != ButtonResult.OK) return;
            var shareFileInfo = dialogResult.Parameters.GetValue<SharedFileInfoDetailDto>("Value");
            _aggregator.SendFileSharedMessage(shareFileInfo);
        }

        private void DownloadDir()
        {
            var dir = CurrentSelectedItem.ChildrenDirInfo[SelectedDirIndex];
            var fileList = new List<FileTransferMessage>();
            GetAllFiles(dir, "\\"+ dir.DirName, fileList);
            foreach (var file in fileList)
            {
                _aggregator.SendFileTransferMessage(file, "Download");
            }
            _aggregator.SendMessage($"文件夹{dir.DirName}开始下载", "DirView");
        }

        private void GetAllFiles(DirInfoDetailDto rootDir,string dirName,List<FileTransferMessage> fileList)
        {
            foreach (var item in rootDir.ChildrenFileInfo)
            {
                fileList.Add(new FileTransferMessage
                {
                    UserPrivateFileInfo = item,
                    DownloadDir = dirName,
                });  
            }
            foreach (var item in rootDir.ChildrenDirInfo)
            {
                GetAllFiles(item, dirName + "\\" + item.DirName, fileList);
            }
        }


        private Guid? lastDirId = null;
        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshModel()
        {
            try
            {
                UpdateLoading(true);
                lastDirId = CurrentSelectedItem.Id;
                if (lastDirId == null) return;
                GetDirInfo();
            }
            catch (Exception e)
            {
                _aggregator.SendMessage("发生错误："+e.Message, "DirView");
            }
            finally
            {
                UpdateLoading(false);
            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        private void DownloadFile()
        {
            var file = CurrentSelectedItem.ChildrenFileInfo[SelectedFileIndex];
            if (file == null || file.Id == null)
            {
                _aggregator.SendMessage("发生错误，请刷新后再试试！", "DirView");
                return;
            }
            _aggregator.SendFileTransferMessage(new Common.Events.FileTransferMessage
            {
                UserPrivateFileInfo = CurrentSelectedItem.ChildrenFileInfo[SelectedFileIndex]
            }, "Download");
            _aggregator.SendMessage("文件开始下载", "DirView");
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        private async void DeleteFile()
        {
            try
            {
                var dialogResult = await dialogHostService.Question("温馨提示", "确认删除文件?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                UpdateLoading(true);
                if (SelectedFileIndex == -1) { _aggregator.SendMessage("未选中文件夹", "DirView"); return; }
                var deleteFile = CurrentSelectedItem.ChildrenFileInfo[SelectedFileIndex];
                var result = await fileRequestService.DeleteAsync((Guid)deleteFile.Id);
                if (!result.Status) { _aggregator.SendMessage(result.Message, "DirView"); return; }
                else
                {
                    var parentDir = FindChildren(DirInfoDetailDtos.FirstOrDefault(), (Guid)CurrentSelectedItem.Id);
                    parentDir.ChildrenFileInfo.Remove(deleteFile);
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
        /// <summary>
        /// 删除文件夹
        /// </summary>
        private async void DeleteDirInfo()
        {
            try
            {
                var dialogResult = await dialogHostService.Question("温馨提示", "确认删除文件夹?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                UpdateLoading(true);
                if (SelectedDirIndex == -1) { _aggregator.SendMessage("未选中文件夹", "DirView"); return; }
                var deleteDir = CurrentSelectedItem.ChildrenDirInfo[SelectedDirIndex];
                var result = await userCloudDirInfoRequestService.DeleteAsync((Guid)deleteDir.Id);
                if (!result.Status) { _aggregator.SendMessage(result.Message, "DirView"); return; }
                else
                {
                    var parentDir = FindChildren(DirInfoDetailDtos.FirstOrDefault(), (Guid)deleteDir.ParentDirInfoId);
                    parentDir.ChildrenDirInfo.Remove(deleteDir);
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
        /// <summary>
        /// 重命名文件夹
        /// </summary>
        private async void EditDirInfo()
        {
            //_aggregator.SendMessage("Edit:" + CurrentSelectedItem.DirName);
            DialogParameters param = new DialogParameters();
            bool isRoot = false;
            if (SelectedDirIndex >= 0)
            {
                var dir = CurrentSelectedItem.ChildrenDirInfo[SelectedDirIndex];
                if (dir.ParentDirInfoId == DirInfoDetailDtos[0].Id)
                {
                    isRoot = true;
                    dir.ParentDirInfoId = null;
                }
                param.Add("Value", dir);
            }
            else
            {
                _aggregator.SendMessage("请选择文件夹", "DirView");
                return;
            }
            var dialogResult = await dialogHostService.ShowDialog("OperaDirInfoView", param);
            try
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    var editDir = dialogResult.Parameters.GetValue<DirInfoDetailDto>("Value");
                    if (editDir != null && editDir.Id != null)
                    {
                        var updateResult = await userCloudDirInfoRequestService.UpdateAsync(editDir);
                        if (!updateResult.Status)
                        {
                            _aggregator.SendMessage($"更新失败:{updateResult.Message}", "DirView");
                            return;
                        }
                        _aggregator.SendMessage("重命名成功", "DirView");
                        if (isRoot)
                        {
                            editDir.ParentDirInfoId = DirInfoDetailDtos[0].Id;
                        }
                        CurrentSelectedItem.ChildrenDirInfo[SelectedDirIndex] = editDir;
                    }
                }
            }
            catch (Exception ex)
            {
                _aggregator.SendMessage($"发生异常：{ex.Message}");
            }


        }
        /// <summary>
        /// 添加文件夹
        /// </summary>
        private async void AddNewDirInfo()
        {
            //_aggregator.SendMessage("Add:" + CurrentSelectedItem.DirName);
            try
            {
                DialogParameters param = new DialogParameters();
                var dialogResult = await dialogHostService.ShowDialog("OperaDirInfoView", param);
                if (dialogResult.Result == ButtonResult.OK)
                {
                    var newDir = dialogResult.Parameters.GetValue<DirInfoDetailDto>("Value");

                    if (newDir != null)
                    {
                        bool isRoot = false;
                        //是否是根目录
                        if (CurrentSelectedItem.Id == DirInfoDetailDtos[0].Id)
                        {
                            newDir.ParentDirInfoId = null;
                            isRoot = true;
                        }
                        else
                            newDir.ParentDirInfoId = CurrentSelectedItem.Id;

                        var updateResult = await userCloudDirInfoRequestService.AddAsync(newDir);
                        if (!updateResult.Status)
                        {
                            _aggregator.SendMessage($"新建失败:{updateResult.Message}", "DirView");
                            return;
                        }
                        newDir = updateResult.Result;
                        _aggregator.SendMessage("新建文件夹成功", "DirView");
                        if (isRoot) newDir.ParentDirInfoId = DirInfoDetailDtos[0].Id;

                        CurrentSelectedItem.ChildrenDirInfo.Add(newDir);
                    }
                }
            }
            catch (Exception ex)
            {
                _aggregator.SendMessage("发生错误：" + ex.Message, "DirView");
            }
        }
        /// <summary>
        /// 添加文件
        /// </summary>
        private void AddFile()
        {
            if (CurrentSelectedItem.ParentDirInfoId == null)
            {
                _aggregator.SendMessage("无法在根目录下添加文件！", "DirView");
                return;
            }
            var filePath = DirSelectorDialogHelper.GetPathByFileBrowserDialog();
            if (string.IsNullOrEmpty(filePath)) return;



            _aggregator.SendFileTransferMessage(new FileTransferMessage
            {
                CurrentDirId = (Guid)CurrentSelectedItem.Id,
                FilePath = filePath
            }, "Upload");
            var file = new System.IO.FileInfo(filePath);
            CurrentSelectedItem.ChildrenFileInfo.Add(new UserPrivateFileInfoDto
            {
                FileName = file.Name,
                RealFileInfo = new Shared.Dtos.CloudSavedDto.CloudFileInfo.CloudFileInfoDto
                {
                    FileType = file.Extension,
                    FileLength = file.Length,
                    UpdateTime = DateTime.Now,
                }
            });
        }
        /// <summary>
        /// 编辑文件
        /// </summary>
        private async Task EditFile()
        {
            if (CurrentSelectedItem.ParentDirInfoId == null || CurrentSelectedItem.Id == null) return;
            var file = CurrentSelectedItem.ChildrenFileInfo[SelectedFileIndex];
            if (file == null || file.Id == null)
            {
                _aggregator.SendMessage("请等待文件上传完毕后刷新后刷新！", "DirView");
                return;
            }

            DialogParameters param = new();
            param.Add("Value", file);
            var dialogResult = await dialogHostService.ShowDialog("OperaFileInfoView", param);
            try
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    var editFile = dialogResult.Parameters.GetValue<UserPrivateFileInfoDto>("Value");

                    var updateResult = await fileRequestService.UpdateFileInfo((Guid)CurrentSelectedItem.Id, editFile);
                    if (!updateResult.Status)
                    {
                        _aggregator.SendMessage("更新失败：" + updateResult.Message, "DirView");
                        return;
                    }
                    CurrentSelectedItem.ChildrenFileInfo[SelectedFileIndex].FileName = updateResult.Result.FileName;
                }
            }
            catch (Exception e)
            {
                _aggregator.SendMessage("发生错误：" + e.Message, "DirView");
            }

        }



        private void GotoUpDir()
        {
            if (CurrentSelectedItem.ParentDirInfoId != null)
            {
                var parentDir = FindChildren(DirInfoDetailDtos.FirstOrDefault(), (Guid)CurrentSelectedItem.ParentDirInfoId);
                if (parentDir == null)
                {
                    _aggregator.SendMessage("请等待上传完毕后在操作！", "DirView");
                    RefreshModel();
                    return;
                }
                CurrentSelectedItem = parentDir;
            }
            else
            {
                _aggregator.SendMessage("没有父文件夹", "DirView");
                return;
            }
        }
        /// <summary>
        /// 找到指定节点
        /// </summary>
        /// <param name="root"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private DirInfoDetailDto? FindChildren(DirInfoDetailDto? root, Guid id)
        {
            if (root == null) return null;
            if (root.Id == id)
            {
                return root;
            }
            else if (root.ChildrenDirInfo != null && root.ChildrenDirInfo.Count > 0)
            {
                DirInfoDetailDto? result = null;
                for (int i = 0; result == null && i < root.ChildrenDirInfo.Count; i++)
                {
                    result = FindChildren(root.ChildrenDirInfo[i], id);
                }
                return result;
            }
            return null;
        }



        private async void GetDirInfo()
        {
            var requsetResult = await userCloudDirInfoRequestService.GetDirInfoAsync(new Shared.Paremeters.QueryParameter
            {
                PageIndex = 0,
                PageSize = ushort.MaxValue,
            });
            if (!requsetResult.Status)
            {
                _aggregator.SendMessage("获取信息失败:" + requsetResult.Message);
                OperaVisibility = Visibility.Hidden;
            }
            else
            {
                DirInfoDetailDtos.Clear();
                DirInfoDetailDtos.Add(requsetResult.Result);
                if (lastDirId != null)
                {
                    var lastDir = FindChildren(DirInfoDetailDtos.FirstOrDefault(), (Guid)lastDirId);
                    if (lastDir != null)
                        CurrentSelectedItem = lastDir;
                    else
                        CurrentSelectedItem = DirInfoDetailDtos.FirstOrDefault();
                }
                else
                {
                    CurrentSelectedItem = DirInfoDetailDtos.FirstOrDefault();
                }
                UpdateNavDirInfo();
                OperaVisibility = Visibility.Visible;
            }
        }
    }
}
