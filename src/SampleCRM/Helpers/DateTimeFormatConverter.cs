using System;
using System.Windows.Data;

namespace SampleCRM.Web.Views
{
    public sealed class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value is string str)
                {
                    if (DateTime.TryParse(value.ToString(), out var datetime))
                    {
                        return datetime.ToShortDateString();
                    }
                    else
                    {
                        return value.ToString();
                    }
                }
                else if (value is DateTime dateTime)
                {
                    if (dateTime == DateTime.MinValue)

                        return "N/A";
                    else
                        return dateTime.ToShortDateString();
                }
                else
                {
                    return "N/A";
                }
            }
            catch
            {
                return "N/A";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}