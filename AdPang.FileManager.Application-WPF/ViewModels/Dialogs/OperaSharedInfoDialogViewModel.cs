using System;
using AdPang.FileManager.Application_WPF.Common;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.SharedFileInfo;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace AdPang.FileManager.Application_WPF.ViewModels.Dialogs
{
    public class OperaSharedInfoDialogViewModel : BindableBase, IDialogHostAware
    {
        public OperaSharedInfoDialogViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }
        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        private bool editable = false;

        public bool Editable
        {
            get { return editable; }
            set { editable = value; RaisePropertyChanged(); }
        }


        private string title;
        private string shareName;

        public string ShareName
        {
            get { return shareName; }
            set { shareName = value; RaisePropertyChanged(); }
        }

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

        private SharedFileInfoDetailDto model;

        public SharedFileInfoDetailDto Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }


        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value") && parameters.ContainsKey("IsEdit"))
            {
                Model = parameters.GetValue<SharedFileInfoDetailDto>("Value");
                var isEdit = parameters.GetValue<bool>("IsEdit");
                ShareName = Model.IsSingleFile ? Model.SingleFileInfo.FileName : Model.DirInfo.DirName;
                Title = isEdit ? "编辑分享" : Model.Id == null ? "新建分享信息" : "分享详情";
                Editable = isEdit || Model.Id == null;
            }
            else
            {
                throw new ArgumentException("参数不正确！");
            }
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
            if (Model.HasExpired && Model.ExpiredTime == null) return;
            if (!Model.HasExpired)
                Model.ExpiredTime = null;
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