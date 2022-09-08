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
        #region field & prop
        private static List<FileDownloadInfo> localFileDownloadInfos = new();
        private static List<FileUploadInfo> localFileUploadInfos = new();

        private ObservableCollection<FileDownloadInfo> downloadFiles = new();
        public ObservableCollection<FileDownloadInfo> DownloadFiles
        {
            get { return downloadFiles; }
            set { downloadFiles = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<FileUploadInfo> UploadFiles
        {
            get { return uploadFiles; }
            set { uploadFiles = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<FileUploadInfo> uploadFiles = new();

        public DelegateCommand<object> OpenFileDirCommand { get; set; }
        public DelegateCommand<object> OpenFileCommand { get; set; }

        public DelegateCommand<FileDownloadInfo> DeleteDownloadedFileCommand { get; set; }

        public DelegateCommand<FileUploadInfo> DeleteUploadedFileCommand { get; set; }

        #endregion

        #region readonly field
        private readonly IFileRequestService fileRequestService;
        private readonly IDialogHostService dialogHostService;
        #endregion

        #region ctor
        static FileTransferListViewModel()
        {
            try
            {
                string downLoadList = System.Windows.Forms.Application.StartupPath + "\\data\\DownLoadList.json";
                string upLoadList = System.Windows.Forms.Application.StartupPath + "\\data\\UpLoadList.json";
                var tempDownloadFile = JsonConvert.DeserializeObject<List<FileDownloadInfo>>(File.ReadAllText(downLoadList));
                if (tempDownloadFile != null) localFileDownloadInfos.AddRange(tempDownloadFile);

                var tempUploadFile = JsonConvert.DeserializeObject<List<FileUploadInfo>>(File.ReadAllText(upLoadList));
                if (tempUploadFile != null) localFileUploadInfos.AddRange(tempUploadFile);

            }
            catch (Exception)
            {

            }


        }

        public FileTransferListViewModel(IContainerProvider containerProvider, IEventAggregator eventAggregator, IFileRequestService fileRequestService, IDialogHostService dialogHostService) : base(containerProvider)
        {
            DownloadFiles.AddRange(localFileDownloadInfos);
            UploadFiles.AddRange(localFileUploadInfos);

            eventAggregator.ResgiterFileTransferMessage(arg =>
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
                    }, SystemSettingsModel.GetDownloadPath() + arg.DownloadDir);
                    downloadInfo.DownloadedPath = result;
                });
                task.Start();

            }, "Download");

            eventAggregator.ResgiterFileTransferMessage(arg =>
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

            OpenFileDirCommand = new DelegateCommand<object>(obj =>
            {
                if (obj.GetType().Equals(typeof(FileDownloadInfo)))
                {
                    if (obj is not FileDownloadInfo fileInfo || string.IsNullOrEmpty(fileInfo.DownloadedPath))
                    {
                        _aggregator.SendMessage("发生错误！", "Main");
                        return;
                    }
                    FileHelper.OpenFileDir(fileInfo.DownloadedPath);
                }
                else if(obj.GetType().Equals(typeof(FileUploadInfo)))
                {
                    if (obj is not FileUploadInfo fileInfo || string.IsNullOrEmpty(fileInfo.UploadFilePath))
                    {
                        _aggregator.SendMessage("发生错误！", "Main");
                        return;
                    }
                    FileHelper.OpenFileDir(fileInfo.UploadFilePath);
                }
            });

            OpenFileCommand = new DelegateCommand<object>(obj =>
            {
                if (obj.GetType().Equals(typeof(FileDownloadInfo)))
                {
                    if (obj is not FileDownloadInfo fileInfo || string.IsNullOrEmpty(fileInfo.DownloadedPath))
                    {
                        _aggregator.SendMessage("发生错误！", "Main");
                        return;
                    }
                    FileHelper.OpenFile(fileInfo.DownloadedPath);
                }
                else if (obj.GetType().Equals(typeof(FileUploadInfo)))
                {
                    if (obj is not FileUploadInfo fileInfo || string.IsNullOrEmpty(fileInfo.UploadFilePath))
                    {
                        _aggregator.SendMessage("发生错误！", "Main");
                        return;
                    }
                    FileHelper.OpenFile(fileInfo.UploadFilePath);
                }
                
            });

            DeleteDownloadedFileCommand = new DelegateCommand<FileDownloadInfo>(async fileInfo =>
            {
                DialogParameters param = new();
                param.Add("Value", fileInfo.UserPrivateFileInfo.FileName);
                var dialogResult = await dialogHostService.ShowDialog("DeleteFileDialogView", param);
                if (dialogResult.Result != ButtonResult.OK) return;
                try
                {
                    UpdateLoading(true);
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

            DeleteUploadedFileCommand = new DelegateCommand<FileUploadInfo>(async fileInfo =>
            {
                var dialogResult = await dialogHostService.Question("温馨提示",$"是否删除{fileInfo.FileName}？");
                if (dialogResult.Result != ButtonResult.OK) return;
                try
                {
                    UpdateLoading(true);
                    UploadFiles.Remove(fileInfo);
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
