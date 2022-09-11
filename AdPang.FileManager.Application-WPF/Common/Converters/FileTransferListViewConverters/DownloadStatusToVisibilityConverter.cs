using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AdPang.FileManager.Application_WPF.Common.Converters.FileTransferListViewConverters
{
    public class DownloadStatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return Visibility.Collapsed;
            double p = (double)value;
            if (parameter.ToString().Equals("Finished"))
            {
                return p >= 100 ? Visibility.Visible : Visibility.Collapsed;

            }
            else if (parameter.ToString().Equals("Loading"))
            {
                return p >= 100 ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
