using System.Windows;
using System.Windows.Controls;

namespace AdPang.FileManager.Application_WPF.Views.LocalPrivateManage
{
    /// <summary>
    /// PrivateDiskView.xaml 的交互逻辑
    /// </summary>
    public partial class PrivateDiskView : UserControl
    {
        public PrivateDiskView()
        {
            InitializeComponent();
        }
        private void BtnChangeMode_Click(object sender, RoutedEventArgs e)
        {
            if (GridDbView.Visibility == Visibility.Visible)
            {
                GridDbView.Visibility = Visibility.Hidden;
                GridLocalView.Visibility = Visibility.Visible;
                BtnChangeMode.Content = "数据库硬盘数据";
            }
            else
            {
                GridDbView.Visibility = Visibility.Visible;
                GridLocalView.Visibility = Visibility.Hidden;
                BtnChangeMode.Content = "本地硬盘数据";
            }
        }
    }
}
