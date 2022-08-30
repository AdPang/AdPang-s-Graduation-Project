using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo;
using Prism.Mvvm;

namespace AdPang.FileManager.Application_WPF.Common.Models
{
    public enum TransferStatus
    {
        WaitForStart,
        Loading,
        Success,
    }
    public class FileDownloadInfo : BindableBase
    {
        private UserPrivateFileInfoDto userPrivateFileInfo;
        private string downloadPath;

        public string DownloadedPath
        {
            get { return downloadPath; }
            set { downloadPath = value; RaisePropertyChanged(); }
        }

        private double downloadProgress = 0;

        private TransferStatus loadingStatus = TransferStatus.WaitForStart;
        public TransferStatus LoadingStatus
        {
            get { return loadingStatus; }
            set { loadingStatus = value; RaisePropertyChanged(); }
        }

        public double DownloadProgress
        {
            get { return downloadProgress; }
            set 
            {
                downloadProgress = value; 
                if (value > 0 && value < 100) 
                    LoadingStatus = TransferStatus.Loading;
                else 
                    LoadingStatus = TransferStatus.Success;
                RaisePropertyChanged(); 
            }
        }
        public UserPrivateFileInfoDto UserPrivateFileInfo
        {
            get { return userPrivateFileInfo; }
            set
            {
                userPrivateFileInfo = value; RaisePropertyChanged();
            }
        }
    }


    public class FileUploadInfo : BindableBase
    {
        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; RaisePropertyChanged(); }
        }


        private double uploadProgress = 0;
        public double UploadProgress
        {
            get { return uploadProgress; }
            set { uploadProgress = value; RaisePropertyChanged(); }
        }

        private string uploadFilePath;

        public string UploadFilePath
        {
            get { return uploadFilePath; }
            set { uploadFilePath = value;RaisePropertyChanged(); }
        }

        private Guid currentDirId;

        public Guid CurrentDirId
        {
            get { return currentDirId; }
            set { currentDirId = value;RaisePropertyChanged(); }
        }



        //private UserPrivateFileInfoDetailDto userPrivateFileInfoDetailDto;
        //public UserPrivateFileInfoDetailDto UserPrivateFileInfoDetailDto
        //{
        //    get { return userPrivateFileInfoDetailDto; }
        //    set
        //    {
        //        userPrivateFileInfoDetailDto = value; RaisePropertyChanged();
        //    }
        //}
    }
}
