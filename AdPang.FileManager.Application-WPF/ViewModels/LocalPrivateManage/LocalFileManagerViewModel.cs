using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using AdPang.FileManager.Application_WPF.Common.Helper;
using AdPang.FileManager.Application_WPF.Common.Models;
using AdPang.FileManager.Application_WPF.Extensions;
using AdPang.FileManager.Application_WPF.Services.IServices;
using AdPang.FileManager.Shared.Common.Helper;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateFile;
using HttpRequestClient.Services.IRequestServices;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;

namespace AdPang.FileManager.Application_WPF.ViewModels.LocalPrivateManage
{
    public class LocalFileManagerViewModel : NavigationViewModel
    {
        public LocalFileManagerViewModel(IContainerProvider containerProvider, ILocalInfoService localInfoService, IPrivateDiskRequestService diskRequestService, IPrivateFileInfoRequestService privateFileInfoRequestService) : base(containerProvider)
        {
            dialogHost = containerProvider.Resolve<IDialogHostService>();
            localDiskInfos = localInfoService.GetLocalDiskInfos();
            this.diskRequestService = diskRequestService;
            this.privateFileInfoRequestService = privateFileInfoRequestService;
            InitCommand();
        }

        #region 字段、属性
        private readonly IDialogHostService dialogHost;
        private readonly ObservableCollection<LocalDiskInfo> localDiskInfos;
        private readonly IPrivateDiskRequestService diskRequestService;
        private readonly IPrivateFileInfoRequestService privateFileInfoRequestService;
        private bool diskIsRegister = false;
        public DelegateCommand<string> OperateViewCommand { get; private set; }
        public DelegateCommand<string> ShowDirBrowser { get; private set; }
        public DelegateCommand<string> OperateFileCommand { get; set; }

        public List<FileInfo> FileList { get; set; } = new();
        private PrivateDiskInfoDto diskInfo = new();

        private string rootPath;

        private string destPath;

        private bool rootPathEnable = false;

        private bool destRootPathEnable = false;

        private ObservableCollection<LocalFilesInfo> fileInfos = new();


        private LocalFilesInfo selectedFileInfo = new();

        public LocalFilesInfo SelectedFileInfo
        {
            get { return selectedFileInfo; }
            set { selectedFileInfo = value; RaisePropertyChanged(); }
        }


        public ObservableCollection<LocalFilesInfo> FileInfos
        {
            get { return fileInfos; }
            set { fileInfos = value; RaisePropertyChanged(); }
        }


        /// <summary>
        /// 目标目录是否可用
        /// </summary>
        public bool DestRootPathEnable
        {
            get { return destRootPathEnable; }
            set { destRootPathEnable = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 根目录是否可用
        /// </summary>
        public bool RootPathEnable
        {
            get { return rootPathEnable; }
            set { rootPathEnable = value; RaisePropertyChanged(); GetDiskInfo(); }
        }


        /// <summary>
        /// 目标目录路径
        /// </summary>
        public string DestPath
        {
            get { return destPath; }
            set
            {
                destPath = value;
                if (string.IsNullOrEmpty(value))
                {
                    DestRootPathEnable = false;
                }
                else
                {
                    DestRootPathEnable = true;
                }
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 根目录路径
        /// </summary>
        public string RootPath
        {
            get { return rootPath; }
            set
            {
                UpdateLoading(true);
                rootPath = value;
                if (string.IsNullOrEmpty(value))
                {
                    RootPathEnable = false;
                    DestPath = "";
                    ClearFileInfos();
                }
                else
                {
                    RootPathEnable = true;
                    GetFileInfos();
                }
                RaisePropertyChanged();
                UpdateLoading(false);
            }
        }

        public bool DiskIsRegister { get => diskIsRegister; set => diskIsRegister = value; }

        private void ClearFileInfos()
        {
            FileInfos.Clear();
        }

        #endregion

        #region 方法
        private async void GetDiskInfo()
        {

            try
            {
                UpdateLoading(true);
                var getDiskInfos = await diskRequestService.GetAllAsync(new Shared.Paremeters.QueryParameter
                {
                    PageIndex = 0,
                    PageSize = int.MaxValue
                });
                if (!getDiskInfos.Status)
                {
                    _aggregator.SendMessage("请求错误：" + getDiskInfos.Message);
                    return;
                }
                DiskIsRegister = false;
                var diskSn = DiskHelper.GetDiskVolume(RootPath[0].ToString());
                foreach (var item in getDiskInfos.Result.Items)
                {
                    if (item.DiskSN.Trim() == diskSn)
                    {
                        this.diskInfo = item;
                        DiskIsRegister = true;
                        return;
                    }
                }
                _aggregator.SendMessage("硬盘未注册");

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

        private void GetFileInfos()
        {
            FileList.Clear();
            FileHelper.GetAllFileFromDir(new DirectoryInfo(RootPath), FileList);
            Reload();
        }


        private void Reload()
        {
            int id = 1;
            FileInfos.Clear();
            FileList.ForEach(f =>
            {
                FileInfos.Add(new LocalFilesInfo
                {
                    Id = id++,
                    File = f,
                    FileName = f.Name,
                    FullName = f.FullName,
                });
            });
        }

        private void InitCommand()
        {

            ShowDirBrowser = new DelegateCommand<string>(commandStr =>
            {
                string path = string.Empty;
                switch (commandStr)
                {
                    case "SelectDescDir":
                        path = DirSelectorDialogHelper.GetPathByFolderBrowserDialog();
                        if (string.IsNullOrEmpty(path)) return;
                        DestPath = path;
                        break;
                    case "SelectRootDir":
                        path = DirSelectorDialogHelper.GetPathByFolderBrowserDialog();
                        if (string.IsNullOrEmpty(path)) return;
                        RootPath = path;
                        break;
                    default:
                        break;
                }
            });

            OperateViewCommand = new DelegateCommand<string>(commandStr =>
            {
                //UpdateLoading(true);
                switch (commandStr)
                {
                    case "Refresh":
                        Reload();
                        break;
                    case "FindRepateFileName":
                        FileInfos = FileHelper.FindRepateFileName(FileInfos);
                        break;
                    //case "FindViolationFileName":
                    //    FileInfos = FileHelper.FindViolationFileName(FileInfos);
                    //    break;
                    case "MakeFileNameToUpper":
                        FileHelper.FileRename("Upper", FileInfos);
                        Refresh();
                        break;
                    case "MakeFileNameToLower":
                        FileHelper.FileRename("Lower", FileInfos);
                        Refresh();
                        break;
                    case "ShowFileDetailViewDialog":
                        ShowFileDetailView();
                        break;
                    default:
                        break;
                }
                //UpdateLoading(false);
            });

            OperateFileCommand = new DelegateCommand<string>(commandStr =>
            {
                try
                {
                    UpdateLoading(true);
                    switch (commandStr)
                    {
                        case "OpenFile":
                            FileHelper.OpenFile(SelectedFileInfo.File.FullName);
                            break;
                        case "OpenFileDir":
                            FileHelper.OpenFileDir(SelectedFileInfo.File.FullName);
                            break;
                        case "MoveFile":
                            FileHelper.FileMove(SelectedFileInfo.File, DestPath);
                            GetFileInfos();
                            break;
                        case "ReNameFile":
                            FileHelper.FileRename(RootPath, SelectedFileInfo);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    _aggregator.SendMessage("发生错误！");
                }
                finally
                {
                    Refresh();
                    UpdateLoading(false);
                }


            });
        }

        private async void ShowFileDetailView()
        {
            DialogParameters param = new();
            param.Add("Value", FileInfos);
            var dialogResult = await dialogHost.ShowDialog("FileInfosDetailDialogView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                var fileInfos = dialogResult.Parameters.GetValue<ObservableCollection<LocalFilesInfo>>("Value");
                List<PrivateFileInfoDto> privateFiles = new();
                foreach (var file in fileInfos)
                {
                    privateFiles.Add(new PrivateFileInfoDto
                    {
                        FileMD5Str = MD5Helper.GetMD5HashCodeFromFile(file.FullName),
                        FileLength = file.File.Length,
                        FileName = file.FileName,
                        FileType = file.File.Extension.TrimStart('.'),
                        DiskId = (Guid)diskInfo.Id,
                        FilePath = file.File.FullName,

                    });
                }
                var fileAddsResult = await privateFileInfoRequestService.AddsAsync(privateFiles);
                if (!fileAddsResult.Status)
                {
                    _aggregator.SendMessage("请求错误：" + fileAddsResult.Message);
                    return;
                }
                _aggregator.SendMessage("添加成功！");
            }

        }

        private void Refresh()
        {
            var tempSelected = SelectedFileInfo;
            var tempFileInfos = FileInfos;
            if ((tempFileInfos is null) || (tempSelected is null))
            {
                return;
            }
            SelectedFileInfo = new();
            FileInfos = new();
            FileInfos.AddRange(tempFileInfos);
            SelectedFileInfo = FileInfos.FirstOrDefault(x => x.File.Name == tempSelected.FileName);

        }
        #endregion
    }
}
