using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AdPang.FileManager.Application_WPF.Common.Converters
{
    public class DataTimeToStringConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.GetType().Equals(typeof(DateTime))) return value;
            DateTime date = (DateTime)value;
            return date.ToString(@"yyyy/MM/dd hh:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
