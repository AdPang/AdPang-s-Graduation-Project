using AdPang.FileManager.Application_WPF.Common;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace AdPang.FileManager.Application_WPF.ViewModels.Dialogs
{
    public class DeleteFileDialogViewModel : BindableBase, IDialogHostAware
    {
        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        private string content;
        private bool isDeleteLocalFile = false;

        public bool IsDeleteLocalFile
        {
            get { return isDeleteLocalFile; }
            set { isDeleteLocalFile = value; RaisePropertyChanged(); }
        }


        public string Content
        {
            get { return string.Format("确认移除{0}数据吗？", content); }
            set { content = value; RaisePropertyChanged(); }
        }

        public DeleteFileDialogViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
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

            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                //确定时，把编辑的实体返回并返回Ok
                DialogParameters param = new();
                param.Add("Value", IsDeleteLocalFile);
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
            }
        }


        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Content = parameters.GetValue<string>("Value");
            }
            else
            {
                Content = "该";
            }
        }
    }
}
