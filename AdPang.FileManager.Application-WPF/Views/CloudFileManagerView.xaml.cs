using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AdPang.FileManager.Application_WPF.Extensions;
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

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = e.Source;

            ScrollViewer scv = (ScrollViewer)sender;
            scv.RaiseEvent(eventArg);
            e.Handled = true;
        }

        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

            ScrollViewer_PreviewMouseWheel(scrollViewer, e);
        }
    }
}
