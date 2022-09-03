using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdPang.FileManager.Application_WPF.Common.Helper;
using AdPang.FileManager.Application_WPF.Common.Models;
using AdPang.FileManager.Application_WPF.Extensions;
using AdPang.FileManager.Application_WPF.Services.IServices;
using AdPang.FileManager.Application_WPF.Services.Services;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using HttpRequestClient.Services.IRequestServices;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Services.Dialogs;

namespace AdPang.FileManager.Application_WPF.ViewModels
{
    public class FileTransferListViewModel : NavigationViewModel
    {
        private static List<FileDownloadInfo> localFileDownloadInfos = new();
        private static List<FileUploadInfo> localFileUploadInfos= new();

        static FileTransferListViewModel()
        {
            try
            {
                string downLoadList = System.Windows.Forms.Application.StartupPath + "\\data\\DownLoadList.json";
                string upLoadList = System.Windows.Forms.Application.StartupPath + "\\data\\UpLoadList.json";
                var tempDownloadFile = JsonConvert.DeserializeObject<List<FileDownloadInfo>>(File.ReadAllText(downLoadList));
                if(tempDownloadFile != null) localFileDownloadInfos.AddRange(tempDownloadFile);

                var tempUploadFile = JsonConvert.DeserializeObject<List<FileUploadInfo>>(File.ReadAllText(upLoadList));
                if (tempUploadFile != null) localFileUploadInfos.AddRange(tempUploadFile);

            }
            catch (Exception)
            {

            }


        }

        #region field & prop

        private ObservableCollection<FileDownloadInfo> downloadFiles = new();
        public ObservableCollection<FileDownloadInfo> DownloadFiles
        {
            get { return downloadFiles; }
            set { downloadFiles = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<FileUploadInfo> uploadFiles = new();

        public DelegateCommand<FileDownloadInfo> OpenFileDirCommand { get; set; }
        public DelegateCommand<FileDownloadInfo> OpenFileCommand { get; set; }

        public DelegateCommand<FileDownloadInfo> DeleteDownloadedFileCommand { get; set; }
        #endregion

        #region readonly field
        private readonly IFileRequestService fileRequestService;
        private readonly IDialogHostService dialogHostService;
        #endregion


        public ObservableCollection<FileUploadInfo> UploadFiles
        {
            get { return uploadFiles; }
            set { uploadFiles = value; RaisePropertyChanged(); }
        }
        #region ctor

        public FileTransferListViewModel(IContainerProvider containerProvider, IEventAggregator eventAggregator, IFileRequestService fileRequestService, IDialogHostService dialogHostService) : base(containerProvider)
        {
            DownloadFiles.AddRange(localFileDownloadInfos);
            UploadFiles.AddRange(localFileUploadInfos);

            eventAggregator.ResgiterPersonMessage(arg =>
            {
                var downloadInfo = new FileDownloadInfo
                {
                    UserPrivateFileInfo = arg.UserPrivateFileInfo
                };
                DownloadFiles.Add(downloadInfo);


                Task task = new(async () =>
                {
                    var result = await fileRequestService.DownloadFile(arg.UserPrivateFileInfo, (sender, e) =>
                    {
                        downloadInfo.DownloadProgress = e.ProgressPercentage;
                    }, SystemSettingsModel.GetDownloadPath());
                    downloadInfo.DownloadedPath = result;
                });
                task.Start();

            }, "Download");

            eventAggregator.ResgiterPersonMessage(arg =>
            {
                var file = new FileInfo(arg.FilePath);

                var uploadInfo = new FileUploadInfo
                {
                    CurrentDirId = arg.CurrentDirId,
                    UploadFilePath = arg.FilePath,
                    FileName = arg.FilePath.Split('\\')[^1],
                    FileLength = file.Length
                };

                UploadFiles.Add(uploadInfo);

                Task task = new(async () =>
                {
                    var uploadFileResponse = await fileRequestService.UploadFile(file, (sender, e) =>
                    {
                        uploadInfo.UploadProgress = e.ProgressPercentage;
                    });
                    if (!uploadFileResponse.Status) return;
                    foreach (var item in uploadFileResponse.Result)
                    {
                        var addToCloudFile = await fileRequestService.AddFileToCloud(new Guid(item.Key), uploadInfo.CurrentDirId, new Shared.Dtos.CloudSavedDto.UserPrivateFileInfo.UserPrivateFileInfoDto
                        {
                            FileName = item.Value
                        });
                        if (!addToCloudFile.Status)
                        {

                        }
                    }
                });
                task.Start();

            }, "Upload");

            OpenFileDirCommand = new DelegateCommand<FileDownloadInfo>(fileInfo =>
            {
                if (fileInfo == null || string.IsNullOrEmpty(fileInfo.DownloadedPath))
                {
                    _aggregator.SendMessage("发生错误！", "Main");
                    return;
                }
                FileHelper.OpenFileDir(fileInfo.DownloadedPath);

            });

            OpenFileCommand = new DelegateCommand<FileDownloadInfo>(fileInfo =>
            {
                if (fileInfo == null || string.IsNullOrEmpty(fileInfo.DownloadedPath))
                {
                    _aggregator.SendMessage("发生错误！", "Main");
                    return;
                }
                FileHelper.OpenFile(fileInfo.DownloadedPath);
            });

            DeleteDownloadedFileCommand = new DelegateCommand<FileDownloadInfo>(async fileInfo =>
            {
                DialogParameters param = new();
                param.Add("Value", fileInfo.UserPrivateFileInfo.FileName);
                var dialogResult = await dialogHostService.ShowDialog("DeleteFileDialogView", param);
                if (dialogResult.Result != ButtonResult.OK) return;
                UpdateLoading(true);
                try
                {
                    var isDeleteLocalFile = dialogResult.Parameters.GetValue<bool>("Value");
                    FileInfo file = new(fileInfo.DownloadedPath);
                    if (file.Exists && isDeleteLocalFile)
                    {
                        file.Delete();
                    }
                    DownloadFiles.Remove(fileInfo);
                }
                catch (Exception e)
                {
                    _aggregator.SendMessage("发生错误：" + e.Message);
                }
                finally
                {
                    UpdateLoading(false);
                }
            });
            this.fileRequestService = fileRequestService;
            this.dialogHostService = dialogHostService;
        }
        #endregion
    }
}
