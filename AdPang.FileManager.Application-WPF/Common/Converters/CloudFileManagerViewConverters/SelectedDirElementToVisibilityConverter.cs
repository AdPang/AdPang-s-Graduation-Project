using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AdPang.FileManager.Application_WPF.Common.Converters.CloudFileManagerViewConverters
{
    public class SelectedDirElementToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int sum = 0;
            foreach (var value in values)
            {
                if (value.GetType().Equals(typeof(Int32)))
                {
                    int num = (int)value;
                    sum += num;
                }

            }

            return sum > 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
