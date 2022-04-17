using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Aggregator.services
{
    class PrecisionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
                return null;

           return String.Format("{0:0.00}", value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
