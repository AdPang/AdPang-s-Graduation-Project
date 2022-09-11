using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdPang.FileManager.Application_WPF.Views.Pagination
{
    /// <summary>
    /// Pagination.xaml 的交互逻辑
    /// </summary>
    public partial class Pagination : UserControl
    {
        public Pagination()
        {
            InitializeComponent();
            PageSizeCmb.SelectedIndex = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand FirstPage
        {
            get { return (ICommand)GetValue(FirstPageProperty); }
            set { SetValue(FirstPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstPageProperty =
            DependencyProperty.Register("FirstPage", typeof(ICommand), typeof(Pagination), new PropertyMetadata(null));




        public ICommand PrePage
        {
            get { return (ICommand)GetValue(PrePageProperty); }
            set { SetValue(PrePageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PrePage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PrePageProperty =
            DependencyProperty.Register("PrePage", typeof(ICommand), typeof(Pagination), new PropertyMetadata(null));




        public ICommand NextPage
        {
            get { return (ICommand)GetValue(NextPageProperty); }
            set { SetValue(NextPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NextPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NextPageProperty =
            DependencyProperty.Register("NextPage", typeof(ICommand), typeof(Pagination), new PropertyMetadata(null));




        public ICommand LastPage
        {
            get { return (ICommand)GetValue(LastPageProperty); }
            set { SetValue(LastPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LastPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LastPageProperty =
            DependencyProperty.Register("LastPage", typeof(ICommand), typeof(Pagination), new PropertyMetadata(null));



        public ICommand GoTo
        {
            get { return (ICommand)GetValue(GoToProperty); }
            set { SetValue(GoToProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GoTo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GoToProperty =
            DependencyProperty.Register("GoTo", typeof(ICommand), typeof(Pagination), new PropertyMetadata(null));




        public ICommand PageSizeChange
        {
            get { return (ICommand)GetValue(PageSizeChangeProperty); }
            set { SetValue(PageSizeChangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSizeChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeChangeProperty =
            DependencyProperty.Register("PageSizeChange", typeof(ICommand), typeof(Pagination), new PropertyMetadata(null));





        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(Pagination), new PropertyMetadata(0));




        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(Pagination), new PropertyMetadata(0));



        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(Pagination), new PropertyMetadata(0));

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageSize = int.Parse(((PageSizeCmb.SelectedValue as ComboBoxItem).Content).ToString());
            PageSizeChange?.Execute(0);
        }

        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            FirstPage?.Execute(0);
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            PrePage?.Execute(0);
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            NextPage?.Execute(0);
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            LastPage?.Execute(0);
        }

        private void GoTo_Click(object sender, RoutedEventArgs e)
        {
            int pageNum = 0;
            //if (int.TryParse(GoToPagetb.Text, out pageNum))
            //{
            //    GoTo?.Execute(pageNum);
            //}
        }

        private void GoToPagetb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }
    }
}
