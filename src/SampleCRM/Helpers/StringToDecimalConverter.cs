using System;
using System.Globalization;
using System.Windows.Data;

namespace SampleCRM.Web.Views
{
    public class StringToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
#if DEBUG
            Console.WriteLine($"StringToDecimalConverter, Convert {value}");
#endif
            decimal.TryParse((value ?? "0").ToString(), out decimal dValue);
            return dValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
#if DEBUG
            Console.WriteLine($"StringToDecimalConverter, ConvertBack {value}");
#endif
            var dValue = value.ToString();
            return dValue;
        }
    }
}