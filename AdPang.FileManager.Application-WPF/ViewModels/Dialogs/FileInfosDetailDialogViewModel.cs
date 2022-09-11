using System.Collections.ObjectModel;
using AdPang.FileManager.Application_WPF.Common;
using AdPang.FileManager.Application_WPF.Common.Models;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace AdPang.FileManager.Application_WPF.ViewModels.Dialogs
{
    public class FileInfosDetailDialogViewModel : BindableBase, IDialogHostAware
    {
        public FileInfosDetailDialogViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        #region 属性、字段
        private ObservableCollection<LocalFilesInfo> filesDetailInfo = new();

        private string title = "文件详情";

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }


        public ObservableCollection<LocalFilesInfo> FilesDetailInfo
        {
            get { return filesDetailInfo; }
            set { filesDetailInfo = value; RaisePropertyChanged(); }
        }

        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        #endregion



        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                var tempFileInfos = parameters.GetValue<ObservableCollection<LocalFilesInfo>>("Value");
                FilesDetailInfo.AddRange(tempFileInfos);
            }
        }


        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));//取消返回NO告诉操作结束
        }

        private void Save()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                //确定时，把编辑的实体返回并返回Ok
                DialogParameters param = new();
                param.Add("Value", FilesDetailInfo);
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
            }
        }
    }
}
