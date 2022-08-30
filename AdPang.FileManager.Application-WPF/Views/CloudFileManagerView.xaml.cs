using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AdPang.FileManager.Application_WPF.Common.Events;
using AdPang.FileManager.Application_WPF.Extensions;
using MaterialDesignThemes.Wpf;
using Prism.Events;

namespace AdPang.FileManager.Application_WPF.Views
{
    /// <summary>
    /// CloudFileManagerView.xaml 的交互逻辑
    /// </summary>
    public partial class CloudFileManagerView : UserControl
    {
        public CloudFileManagerView(IEventAggregator aggregator)
        {
            InitializeComponent();
            aggregator.ResgiterMessage(arg =>
            {
                DirViewSnackBar.MessageQueue.Enqueue(arg.Message);
            }, "DirView");
        }
    }
}
