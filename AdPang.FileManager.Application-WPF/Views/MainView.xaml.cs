using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using AdPang.FileManager.Application_WPF.Extensions;
using AdPang.FileManager.Application_WPF.Services.IServices;
using AdPang.FileManager.Application_WPF.ViewModels;
using AdPang.FileManager.Application_WPF.Views.Notifys;
using Newtonsoft.Json;
using Prism.Events;

namespace AdPang.FileManager.Application_WPF.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private readonly IDialogHostService dialogHostService;
        private readonly FileTransferListViewModel fileTransferListViewModel;

        public MainView(IEventAggregator aggregator, IDialogHostService dialogHostService, FileTransferListViewModel fileTransferListViewModel)
        {
            InitializeComponent();
            this.dialogHostService = dialogHostService;
            this.fileTransferListViewModel = fileTransferListViewModel;


            //注册提示消息
            aggregator.ResgiterMessage(arg =>
            {
                Snackbar.MessageQueue.Enqueue(arg.Message);
            }, "Main");

            //注册等待消息窗口
            aggregator.Resgiter(arg =>
            {
                DialogHost.IsOpen = arg.IsOpen;

                if (DialogHost.IsOpen)
                    DialogHost.DialogContent = new ProgressView();
            });
            //窗口操作事件添加
            #region 窗口操作

            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };
            btnClose.Click += async (s, e) =>
            {
                var dialogResult = await dialogHostService.Question("温馨提示", "确认退出系统?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;

                this.Close();
            };
            ColorZone.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };

            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };

            menuBar.SelectionChanged += (s, e) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };
            #endregion
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var downloadData = fileTransferListViewModel.DownloadFiles;
            var uploadData = fileTransferListViewModel.UploadFiles;

            if (!Directory.Exists(Environment.CurrentDirectory + "\\data"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\data");
            }
            string downLoadList = Environment.CurrentDirectory + "\\data\\DownLoadList.json";
            string upLoadList = Environment.CurrentDirectory + "\\data\\UpLoadList.json";
            if (!File.Exists(downLoadList))  // 判断是否已有相同文件 
            {


                FileStream fs1 = new(downLoadList, FileMode.Create, FileAccess.ReadWrite);
                FileStream fs2 = new(upLoadList, FileMode.Create, FileAccess.ReadWrite);
                fs1.Close();
                fs2.Close();
            }

            await File.WriteAllTextAsync(downLoadList, JsonConvert.SerializeObject(downloadData));
            await File.WriteAllTextAsync(upLoadList, JsonConvert.SerializeObject(uploadData));

        }
    }
}
