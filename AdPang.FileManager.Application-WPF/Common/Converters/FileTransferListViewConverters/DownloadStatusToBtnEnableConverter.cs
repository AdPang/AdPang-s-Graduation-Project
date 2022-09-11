﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace AdPang.FileManager.Application_WPF.Common.Converters.FileTransferListViewConverters
{
    public class DownloadStatusToBtnEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return false;
            double p = (double)value;
            return p >= 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
