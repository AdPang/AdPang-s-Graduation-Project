﻿using System.Collections.ObjectModel;
using AdPang.FileManager.Application_WPF.Common.Models;
using AdPang.FileManager.Application_WPF.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace AdPang.FileManager.Application_WPF.ViewModels.Settings
{
    public class SettingsViewModel : BindableBase
    {
        public SettingsViewModel(IRegionManager regionManager)
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
            regionManager.Regions[PrismManager.SettingsViewRegionName].RequestNavigate(obj.NameSpace);
        }

        void CreatedMenuBar()
        {
            MenuBars.Add(new MenuBar { Icon = "Palette", Title = "个性化", NameSpace = "SkinView" });
            MenuBars.Add(new MenuBar { Icon = "Cog", Title = "系统设置", NameSpace = "SystemSettingsView" });
            MenuBars.Add(new MenuBar { Icon = "Information", Title = "关于更多", NameSpace = "AboutView" });
        }
    }
}
