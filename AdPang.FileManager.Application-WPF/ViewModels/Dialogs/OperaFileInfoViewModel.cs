using AdPang.FileManager.Application_WPF.Common;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace AdPang.FileManager.Application_WPF.ViewModels.Dialogs
{
    public class OperaFileInfoViewModel : BindableBase, IDialogHostAware
    {
        public OperaFileInfoViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }
        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public void OnDialogOpend(IDialogParameters parameters)
        {

        }

        private UserPrivateFileInfoDetailDto model;
        /// <summary>
        /// 新增或编辑的实体
        /// </summary>
        public UserPrivateFileInfoDetailDto Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));//取消返回NO告诉操作结束
        }
        /// <summary>
        /// 确定
        /// </summary>
        private void Save()
        {
            if (string.IsNullOrEmpty(Model.FileName))
                return;

            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                //确定时，把编辑的实体返回并返回Ok
                DialogParameters param = new();
                param.Add("Value", Model);
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
            }
        }
    }
}
