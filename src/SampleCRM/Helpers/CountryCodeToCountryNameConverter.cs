using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace SampleCRM.Web.Views
{
    public class CountryCodeToCountryNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var countryCode = value as string;
                if (!string.IsNullOrEmpty(countryCode))
                {
                    var countryCodesContext = new CountryCodesContext();
                    var query = countryCodesContext.GetCountryByIdQuery(countryCode);
                    var entityObj = countryCodesContext.Load(query);
                    value = entityObj.Entities.FirstOrDefault();
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}