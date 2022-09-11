using System;
using System.Globalization;
using System.Windows.Data;

namespace AdPang.FileManager.Application_WPF.Common.Converters.FileSharedViewConverters
{
    public class DatetimeToIsExpiredConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "无期限限制";
            if (!value.GetType().Equals(typeof(DateTime)))
            {
                return value;
            }
            DateTime dateTime = (DateTime)value;
            if (dateTime - DateTime.Now < TimeSpan.Zero)
            {
                return "该分享已过期";
            }
            return dateTime.ToString(@"yyyy/MM/dd");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
