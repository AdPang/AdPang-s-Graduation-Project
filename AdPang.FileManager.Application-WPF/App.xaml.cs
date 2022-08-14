using System.Windows;
using Prism.DryIoc;
using DryIoc;
using Prism.Ioc;
using AdPang.FileManager.Application_WPF.Views;
using AdPang.FileManager.Application_WPF.Services.IServices;
using AdPang.FileManager.Application_WPF.Services.Services;
using AdPang.FileManager.Application_WPF.Common;

namespace AdPang.FileManager.Application_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void OnInitialized()
        {
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
                service.Configure();
            base.OnInitialized();

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.Register<IDialogHostService, DialogHostService>();
        }
    }
}
