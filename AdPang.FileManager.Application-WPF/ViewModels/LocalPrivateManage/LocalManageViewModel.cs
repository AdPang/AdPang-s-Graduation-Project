using System.Collections.ObjectModel;
using AdPang.FileManager.Application_WPF.Common.Models;
using AdPang.FileManager.Application_WPF.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace AdPang.FileManager.Application_WPF.ViewModels.LocalPrivateManage
{
    public class LocalManageViewModel : BindableBase
    {
        public LocalManageViewModel(IRegionManager regionManager)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            this.regionManager = regionManager;
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            CreatedMenuBar();
        }
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }

        private ObservableCollection<MenuBar> menuBars;
        private readonly IRegionManager regionManager;
        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;
            regionManager.Regions[PrismManager.LocalManageViewRegionName].RequestNavigate(obj.NameSpace);
        }

        void CreatedMenuBar()
        {
            MenuBars.Add(new MenuBar { Icon = "FolderWrench", Title = "本地文件管理", NameSpace = "LocalFileManagerView" });
            MenuBars.Add(new MenuBar { Icon = "WashingMachine", Title = "私有硬盘管理", NameSpace = "PrivateDiskView" });
            MenuBars.Add(new MenuBar { Icon = "FileMultiple", Title = "私有文件列表", NameSpace = "PrivateFileInfoView" });
        }
    }
}
