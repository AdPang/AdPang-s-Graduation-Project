using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AdPang.FileManager.Application_WPF.Common.Converters
{
    public class IntByteLengthToOtherLengthConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((!value.GetType().Equals(typeof(int)) && (!value.GetType().Equals(typeof(long))))||parameter==null)
            {
                return value;
            }
            long length = (long)value;

            string mode = parameter.ToString();
            double result;
            switch (mode)
            {
                case "KB":
                    result = length / 1024.0;
                    break;
                case "MB":
                    result = length / 1024.0 / 1024.0;
                    break;
                case "GB":
                    result = length / 1024.0 / 1024.0 / 1024.0;
                    break;
                default:
                    return value;
            }
            return result.ToString("0.0") + mode;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
