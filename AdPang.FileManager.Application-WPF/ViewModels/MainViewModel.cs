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

        #region 构造
        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerProvider"></param>
        /// <param name="regionManager"></param>
        public MainViewModel(IContainerProvider containerProvider,
            IRegionManager regionManager)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            GoBackCommand = new DelegateCommand(() =>
            {
                if (_journal != null && _journal.CanGoBack)
                    _journal.GoBack();
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (_journal != null && _journal.CanGoForward)
                    _journal.GoForward();
            });

            _containerProvider = containerProvider;
            _regionManager = regionManager;
        }
        #endregion

        #region 字段
        private readonly IContainerProvider _containerProvider;
        private readonly IRegionManager _regionManager;
        private IRegionNavigationJournal _journal;
        #endregion

        #region 操作属性
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
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
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "硬盘管理", NameSpace = "DiskManagerView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title = "文件管理-本地", NameSpace = "FileManagerView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "爬虫模式", NameSpace = "CrawlerView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "文件管理-数据库", NameSpace = "FileInfoDbView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "设置", NameSpace = "SettingsView" });
        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;

            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                _journal = back.Context.NavigationService.Journal;
            });
        }

        public void Configure()
        {
            CreateMenuBar();
        }
    }
}
