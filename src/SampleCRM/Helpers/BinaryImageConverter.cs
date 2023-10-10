using System;
using System.IO;
using System.Text;
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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Convert start");
            if (value != null)
            {
                sb.AppendLine("Value not null");
                var toArrayMethod = value.GetType().GetMethod("ToArray",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                if (toArrayMethod != null)
                {
                    sb.AppendLine("toArrayMethod not null");
                    value = toArrayMethod.Invoke(value, new object[] { });
                }

                var bytes = value as byte[];

                if (bytes != null)
                {
                    sb.AppendLine("bytes not null");
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(new MemoryStream(bytes));
                    sb.AppendLine("New bitmap image set");
                    Console.WriteLine(sb.ToString());
                    return bitmapImage;
                }
            }

            sb.AppendLine("Convert end");
            Console.WriteLine(sb.ToString());
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