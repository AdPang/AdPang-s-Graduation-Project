using AdPang.FileManager.Application_WPF.Common;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace AdPang.FileManager.Application_WPF.ViewModels.Dialogs
{
    public class OperaDirInfoViewModel : BindableBase, IDialogHostAware
    {
        public OperaDirInfoViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private DirInfoDetailDto model;
        private string title = "新建文件夹";

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 新增或编辑的实体
        /// </summary>
        public DirInfoDetailDto Model
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
            if (string.IsNullOrEmpty(Model.DirName))
                return;

            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                //确定时，把编辑的实体返回并返回Ok
                DialogParameters param = new DialogParameters();
                param.Add("Value", Model);
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
            }
        }
        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }


        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Title = "重命名";
                Model = parameters.GetValue<DirInfoDetailDto>("Value");
            }
            else
                Model = new DirInfoDetailDto();
        }
    }
}
