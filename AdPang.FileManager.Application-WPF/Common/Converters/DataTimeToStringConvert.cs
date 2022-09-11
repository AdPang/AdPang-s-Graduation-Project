using System;
using System.Globalization;
using System.Windows.Data;

namespace AdPang.FileManager.Application_WPF.Common.Converters
{
    public class DataTimeToStringConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.GetType().Equals(typeof(DateTime))) return value;
            DateTime date = (DateTime)value;
            return date.ToString(@"yyyy/MM/dd HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
