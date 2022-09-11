using System;
using System.Globalization;
using System.Windows.Data;

namespace AdPang.FileManager.Application_WPF.Common.Converters.FileSharedViewConverters
{
    public class BoolToShareTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType().Equals((typeof(bool))))
                return (bool)value ? "文件分享" : "文件夹分享";
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
