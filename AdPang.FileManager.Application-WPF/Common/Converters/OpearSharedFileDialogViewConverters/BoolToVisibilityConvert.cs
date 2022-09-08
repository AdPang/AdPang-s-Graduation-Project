using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AdPang.FileManager.Application_WPF.Common.Converters.OpearSharedFileDialogViewConverters
{
    public class BoolToVisibilityConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType()== typeof(bool))
            {
                var b = (bool)value;
                if(parameter !=null && parameter.ToString().Equals("Normal"))
                {
                    return b ? Visibility.Visible : Visibility.Collapsed;

                }
                else if (parameter != null && parameter.ToString().Equals("Reverse"))
                {
                    return !b ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
