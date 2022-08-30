using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Application_WPF.Common.Events;
using AdPang.FileManager.Application_WPF.Common.Helper;
using AdPang.FileManager.Application_WPF.Common.Models;
using AdPang.FileManager.Application_WPF.Extensions;
using HttpRequestClient.Services.IRequestServices;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;

namespace AdPang.FileManager.Application_WPF.ViewModels
{
    public class FileTransferListViewModel : NavigationViewModel
    {
        #region field & prop

        private string fileDownloadPath = Environment.CurrentDirectory;

        public string FileDownloadPath
        {
            get { return fileDownloadPath; }
            set { fileDownloadPath = value; RaisePropertyChanged(); }
        }


        private ObservableCollection<FileDownloadInfo> downloadFiles = new ObservableCollection<FileDownloadInfo>();
        public ObservableCollection<FileDownloadInfo> DownloadFiles
        {
            get { return downloadFiles; }
            set { downloadFiles = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<FileUploadInfo> uploadFiles = new ObservableCollection<FileUploadInfo>();

        public DelegateCommand<FileDownloadInfo> OpenFileDirCommand { get; set; }
        public DelegateCommand<FileDownloadInfo> OpenFileCommand { get; set; }
        #endregion

        #region readonly field
        private readonly IFileRequestService fileRequestService;
        #endregion


        public ObservableCollection<FileUploadInfo> UploadFiles
        {
            get { return uploadFiles; }
            set { uploadFiles = value; RaisePropertyChanged(); }
        }
        #region ctor

        public FileTransferListViewModel(IContainerProvider containerProvider, IEventAggregator eventAggregator, IFileRequestService fileRequestService) : base(containerProvider)
        {
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
                    }, FileDownloadPath);
                    downloadInfo.DownloadedPath = result;
                });
                task.Start();

            }, "Download");

            eventAggregator.ResgiterPersonMessage(arg =>
            {
                var uploadInfo = new FileUploadInfo
                {
                    CurrentDirId = arg.CurrentDirId,
                    UploadFilePath = arg.FilePath,
                    FileName = arg.FilePath.Split('\\')[^1]
                };

                UploadFiles.Add(uploadInfo);

                Task task = new(async () =>
                {
                    var uploadFileResponse = await fileRequestService.UploadFile(new System.IO.FileInfo(uploadInfo.UploadFilePath), (sender, e) =>
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


            this.fileRequestService = fileRequestService;
        }
        #endregion
    }
}
