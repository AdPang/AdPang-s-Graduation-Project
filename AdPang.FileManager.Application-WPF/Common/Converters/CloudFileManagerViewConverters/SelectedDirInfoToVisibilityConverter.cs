using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;

namespace AdPang.FileManager.Application_WPF.Common.Converters.CloudFileManagerViewConverters
{
    public class SelectedDirInfoToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value is DirInfoDetailDto dir && dir.ChildrenDirInfo.Count + dir.ChildrenFileInfo.Count == 0)
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}