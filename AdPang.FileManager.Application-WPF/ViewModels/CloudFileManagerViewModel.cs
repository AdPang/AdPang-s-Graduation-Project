using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Application_WPF.Common.Models;
using System.Windows.Input;
using AdPang.FileManager.Application_WPF.Extensions;
using HttpRequestClient.Services.IRequestServices;
using Prism.Ioc;
using System.Xml.Linq;
using System.Windows;
using Prism.Commands;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using ImTools;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo;
using AdPang.FileManager.Application_WPF.Services.Services;
using AdPang.FileManager.Application_WPF.Services.IServices;
using Prism.Services.Dialogs;
using Prism.Regions;
using AdPang.FileManager.Application_WPF.Views.Dialogs;
using AdPang.FileManager.Application_WPF.Views;
using Prism.Mvvm;
using AdPang.FileManager.Application_WPF.Common.Events;
using AdPang.FileManager.Application_WPF.Common.Helper;

namespace AdPang.FileManager.Application_WPF.ViewModels
{
    public class CloudFileManagerViewModel : NavigationViewModel
    {
        private readonly IUserCloudDirInfoRequestService userCloudDirInfoRequestService;
        private readonly IDialogHostService dialogHostService;

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
            set { selectedDirIndex = value;RaisePropertyChanged(); }
        }
        public int SelectedFileIndex
        {
            get { return selectedFileIndex; }
            set { selectedFileIndex = value;RaisePropertyChanged(); }
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


        public CloudFileManagerViewModel(IContainerProvider containerProvider, IUserCloudDirInfoRequestService userCloudDirInfoRequestService, IDialogHostService dialogHostService, FileTransferListViewModel fileTransferListViewModel) : base(containerProvider)
        {
            //ViewModelLocationProvider.Register<FileTransferListView, FileTransferListViewModel>();

            this.userCloudDirInfoRequestService = userCloudDirInfoRequestService;
            this.dialogHostService = dialogHostService;
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

        private void Execute(string para)
        {
            switch (para)
            {
                case "AddFile"://添加文件
                    AddFile();
                    break;
                case "EditFile"://编辑文件
                    EditFile();
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
                case "DeleteDir":
                    DeleteDirInfo();
                    break;
                case "GotoUpDir"://返回上一级
                    GotoUpDir();
                    break;
                case "DownloadFile":
                    DownloadFile();
                    break;
                default:
                    break;
            }
        }

        private void DownloadFile()
        {
            _aggregator.SendPersonMessage(new Common.Events.FileTransferMessage
            {
                UserPrivateFileInfo = CurrentSelectedItem.ChildrenFileInfo[SelectedFileIndex]
            }, "Download") ;
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
                if (SelectedDirIndex == -1 ) { _aggregator.SendMessage("未选中文件夹", "DirView"); return; }
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
                    if(editDir !=null && editDir.Id != null)
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

        private void AddFile()
        {
            var filePath = DirSelectorDialogHelper.GetPathByFileBrowserDialog();
            if (string.IsNullOrEmpty(filePath)) return;

            if(CurrentSelectedItem.ParentDirInfoId == null)
            {
                _aggregator.SendMessage("无法再根目录下添加文件！", "DirView");
            }

            _aggregator.SendPersonMessage(new FileTransferMessage
            {
                CurrentDirId = (Guid)CurrentSelectedItem.Id,
                FilePath = filePath

            }, "Upload");
        }

        private void EditFile()
        {
        }

        private void DeleteFile()
        {
        }

        private void GotoUpDir()
        {
            if (CurrentSelectedItem.ParentDirInfoId != null)
            {
                var parentDir = FindChildren(DirInfoDetailDtos.FirstOrDefault(), (Guid)CurrentSelectedItem.ParentDirInfoId);
                if (parentDir == null)
                {
                    _aggregator.SendMessage("发生错误请刷新页面", "DirView");
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
                CurrentSelectedItem = DirInfoDetailDtos.FirstOrDefault();
                UpdateNavDirInfo();
                OperaVisibility = Visibility.Visible;
            }
        }

    }
}
