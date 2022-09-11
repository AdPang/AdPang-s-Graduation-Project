using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace AdPang.FileManager.Application_WPF.Common.Converters.FileSharedViewConverters
{
    public class MultiNameToSingleNameConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                if (value.GetType().Equals(typeof(string)))
                    if (value != null) return value.ToString();
            }
            return values.FirstOrDefault() ?? "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
