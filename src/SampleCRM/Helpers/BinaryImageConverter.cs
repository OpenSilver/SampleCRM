using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SampleCRM.Web.Views
{

    /// <summary>
    /// Converts byte array to image using.
    /// </summary>
    public class BinaryImageConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// BitmapImage.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var toArrayMethod = value.GetType().GetMethod("ToArray",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                if (toArrayMethod != null)
                {
                    value = toArrayMethod.Invoke(value, new object[] { });
                }

                var bytes = value as byte[];

                if (bytes != null)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(new MemoryStream(bytes));
                    return bitmapImage;
                }
            }

            return value;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}