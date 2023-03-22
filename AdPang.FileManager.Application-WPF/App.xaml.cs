using System;
using System.Configuration;
using System.Windows;
using AdPang.FileManager.Application_WPF.Common;
using AdPang.FileManager.Application_WPF.Services.IServices;
using AdPang.FileManager.Application_WPF.Services.Services;
using AdPang.FileManager.Application_WPF.ViewModels;
using AdPang.FileManager.Application_WPF.ViewModels.Dialogs;
using AdPang.FileManager.Application_WPF.ViewModels.LocalPrivateManage;
using AdPang.FileManager.Application_WPF.ViewModels.Settings;
using AdPang.FileManager.Application_WPF.Views;
using AdPang.FileManager.Application_WPF.Views.Dialogs;
using AdPang.FileManager.Application_WPF.Views.LocalPrivateManage;
using AdPang.FileManager.Application_WPF.Views.Settings;
using DryIoc;
using HttpRequestClient.Services;
using HttpRequestClient.Services.IRequestServices;
using HttpRequestClient.Services.RequestServices;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;

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

        public static void LoginOut(IContainerProvider container)
        {
            Current.MainWindow.Hide();
            var dialog = container.Resolve<IDialogService>();

            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }
                Current.MainWindow.Show();
            });
        }

        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();

            dialog.ShowDialog(nameof(LoginView), callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }
                if (App.Current.MainWindow.DataContext is IConfigureService service)
                    service.Configure();
                base.OnInitialized();

            });


        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            #region HttpRequestDI
            containerRegistry.GetContainer()
                .Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
            var hostUrl = ConfigurationManager.AppSettings["HostUrl"];
            containerRegistry.GetContainer().RegisterInstance(hostUrl, serviceKey: "webUrl");


            containerRegistry.Register<IUserCloudDirInfoRequestService, UserCloudDirInfoRequestService>();
            containerRegistry.Register<IAuthRequestService, AuthRequestService>();
            containerRegistry.Register<IFileRequestService, FileRequestService>();
            containerRegistry.Register<ISharedInfoRequestService, SharedInfoRequestService>();
            containerRegistry.Register<IPrivateDiskRequestService, PrivateDiskRequestService>();
            containerRegistry.Register<IPrivateFileInfoRequestService, PrivateFileInfoRequestService>();


            containerRegistry.RegisterSingleton<IAuthModel, AuthModel>();

            #endregion

            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();

            containerRegistry.Register<IDialogHostService, DialogHostService>();
            containerRegistry.RegisterSingleton<FileTransferListViewModel>();
            containerRegistry.RegisterSingleton<FileSharedViewModel>();
            containerRegistry.RegisterSingleton<PrivateFileInfoViewModel>();

            containerRegistry.RegisterSingleton<ILocalInfoService, LocalInfoService>();
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();

            containerRegistry.RegisterForNavigation<CloudFileManagerView, CloudFileManagerViewModel>();
            containerRegistry.RegisterForNavigation<OperaFileInfoView, OperaFileInfoViewModel>();
            containerRegistry.RegisterForNavigation<OperaDirInfoView, OperaDirInfoViewModel>();
            containerRegistry.RegisterForNavigation<OperaSharedInfoDialogView, OperaSharedInfoDialogViewModel>();
            containerRegistry.RegisterForNavigation<DiskInfoDialogView, DiskInfoDialogViewModel>();
            containerRegistry.RegisterForNavigation<LocalManageView, LocalManageViewModel>();
            containerRegistry.RegisterForNavigation<LocalFileManagerView, LocalFileManagerViewModel>();
            containerRegistry.RegisterForNavigation<PrivateDiskView, PrivateDiskViewModel>();
            containerRegistry.RegisterForNavigation<PrivateFileInfoView, PrivateFileInfoViewModel>();
            containerRegistry.RegisterForNavigation<FileInfosDetailDialogView, FileInfosDetailDialogViewModel>();

            containerRegistry.RegisterForNavigation<DeleteFileDialogView, DeleteFileDialogViewModel>();



            containerRegistry.RegisterForNavigation<FileTransferListView, FileTransferListViewModel>();
            containerRegistry.RegisterForNavigation<FileSharedView, FileSharedViewModel>();


            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<SystemSettingsView, SystemSettingsViewModel>();
            containerRegistry.RegisterForNavigation<AboutView>();
            containerRegistry.RegisterForNavigation<IndexView>();
        }
    }
}
