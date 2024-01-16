using System;
using System.Windows.Data;

namespace SampleCRM.Web.Views
{
    public sealed class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is null)
                return null;

            try
            {
                var unixTimeInSeconds = (long)value;
                return DateTimeOffset.FromUnixTimeSeconds(unixTimeInSeconds).ToLocalTime().DateTime;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex);
#endif
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is null)
                return null;
            try
            {
                var dateTime = (DateTime)value;
                var dateTimeOffset = new DateTimeOffset(dateTime.ToUniversalTime());
                var unixTimeInSeconds = dateTimeOffset.ToUnixTimeSeconds();
                return unixTimeInSeconds;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex);
#endif
                return null;
            }

        }
    }
}