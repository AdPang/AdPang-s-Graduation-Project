using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Application_WPF.Common.Helper;
using AdPang.FileManager.Application_WPF.Common.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace AdPang.FileManager.Application_WPF.ViewModels.Settings
{
    public class SystemSettingsViewModel : BindableBase
    {
        private string downloadPath;

        public string DownloadPath
        {
            get { return downloadPath; }
            set { downloadPath = value; RaisePropertyChanged(); }
        }

        public SystemSettingsViewModel()
        {
            DownloadPath = SystemSettingsModel.GetDownloadPath();
            ChangeDownloadPathCommand = new DelegateCommand(() =>
            {
                var path = DirSelectorDialogHelper.GetPathByFolderBrowserDialog(SystemSettingsModel.GetDownloadPath());
                if (string.IsNullOrEmpty(path)) return;
                SystemSettingsModel.SetDownloadPath(path);
                DownloadPath = path;
            });
        }

        public DelegateCommand ChangeDownloadPathCommand { get; set; }


    }
}
