using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using AdPang.FileManager.Application_WPF.Common;
using AdPang.FileManager.Application_WPF.Common.Models;
using AdPang.FileManager.Application_WPF.Extensions;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace AdPang.FileManager.Application_WPF.ViewModels
{
    public class MainViewModel : BindableBase, IConfigureService
    {

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="containerProvider"></param>
        /// <param name="regionManager"></param>
        public MainViewModel(IRegionManager regionManager, IContainerProvider containerProvider)
        {
            this.regionManager = regionManager;
            this.containerProvider = containerProvider;
            MenuBars = new ObservableCollection<MenuBar>();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            LoginOutCommand = new DelegateCommand(() =>
            {
                App.LoginOut(containerProvider);
            });
            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoBack)
                    journal.GoBack();
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoForward)
                    journal.GoForward();
            });
        }

        #region 字段
        private readonly IContainerProvider containerProvider;
        private readonly IRegionManager regionManager;
        private IRegionNavigationJournal journal;
        #endregion

        #region 操作属性
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
        public DelegateCommand LoginOutCommand { get; set; }
        #endregion

        #region 绑定属性
        private ObservableCollection<MenuBar> menuBars;
        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }
        #endregion
        void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            //MenuBars.Add(new MenuBar() { Icon = "ContentSave", Title = "私有硬盘信息", NameSpace = "PrivateDiskView" });
            //MenuBars.Add(new MenuBar() { Icon = "FileMultiple", Title = "私有文件列表", NameSpace = "PrivateFileInfoView" });
            MenuBars.Add(new MenuBar() { Icon = "Application", Title = "本地管理", NameSpace = "LocalManageView" });
            MenuBars.Add(new MenuBar() { Icon = "Nas", Title = "我的网盘", NameSpace = "CloudFileManagerView" });
            MenuBars.Add(new MenuBar() { Icon = "FileArrowUpDown", Title = "文件传输列表", NameSpace = "FileTransferListView" });
            MenuBars.Add(new MenuBar() { Icon = "ShareAll", Title = "分享列表", NameSpace = "FileSharedView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "设置", NameSpace = "SettingsView" });
        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;

            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }

        public void Configure()
        {
            CreateMenuBar();
            //regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("FileTransferListView");
        }
    }
}
