using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Application_WPF.Common;
using AdPang.FileManager.Application_WPF.Common.Models;
using AdPang.FileManager.Application_WPF.Services.IServices;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace AdPang.FileManager.Application_WPF.ViewModels.Dialogs
{
    public class DiskInfoDialogViewModel : BindableBase, IDialogHostAware
    {
        public DiskInfoDialogViewModel(ILocalInfoService localInfoService)
        {
            localDiskInfos = localInfoService.GetLocalDiskInfos();
            SaveCommand = new DelegateCommand(() =>
            {
                if (string.IsNullOrEmpty(Model.DiskName))
                    return;
                if (DialogHost.IsDialogOpen(DialogHostName))
                {
                    //确定时，把编辑的实体返回并返回Ok
                    DialogParameters param = new();
                    param.Add("Value", Model);
                    DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
                }
            });
            CancelCommand = new DelegateCommand(() =>
            {
                if (DialogHost.IsDialogOpen(DialogHostName))
                    DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));//取消返回NO告诉操作结束
            });
        }

        private ObservableCollection<LocalDiskInfo> localDiskInfos;
        private int cbSelectedIndex = 0;
        private PrivateDiskInfoDto model;
        private string dialogTitle;
        private ObservableCollection<string> localDiskNames = new();

        public ObservableCollection<string> LocalDiskNames
        {
            get { return localDiskNames; }
            set { localDiskNames = value; }
        }
        public string DialogTitle
        {
            get { return dialogTitle; }
            set { dialogTitle = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 新增或编辑的实体
        /// </summary>
        public PrivateDiskInfoDto Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }

        public int CbSelectedIndex
        {
            get { return cbSelectedIndex; }
            set { cbSelectedIndex = value; RaisePropertyChanged(); Model.DiskSN = localDiskInfos[cbSelectedIndex].DriveSN; }
        }

        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                DialogTitle = "修改硬盘信息";
                Model = parameters.GetValue<PrivateDiskInfoDto>("Value");
                LocalDiskNames.Add(model.DiskName + "\t" + model.DiskSN);
            }
            else
            {
                DialogTitle = "添加硬盘信息";
                Model = new PrivateDiskInfoDto() { DiskSN = localDiskInfos[CbSelectedIndex].DriveSN };

                foreach (var item in localDiskInfos)
                {
                    LocalDiskNames.Add(item.Drive + "\t" + item.DriveSN);
                };
            }
        }

        
    }
}
