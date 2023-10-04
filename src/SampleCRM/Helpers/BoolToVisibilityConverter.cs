using System;
using System.Windows;
using System.Windows.Data;

namespace SampleCRM.Web.Views
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool boolValue = value is bool && (bool)value;
            if (boolValue)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility)
                return (Visibility)value == Visibility.Visible;
            else
                return false;
        }
    }
}