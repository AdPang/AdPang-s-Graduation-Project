using System.Windows.Controls;
using AdPang.FileManager.Application_WPF.Common.Events;
using AdPang.FileManager.Application_WPF.Extensions;
using Prism.Events;

namespace AdPang.FileManager.Application_WPF.Views
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            eventAggregator.ResgiterMessage(arg =>
            {
                LoginSnackBar.MessageQueue.Enqueue(arg.Message);
            }, "Login");
        }
    }
}
