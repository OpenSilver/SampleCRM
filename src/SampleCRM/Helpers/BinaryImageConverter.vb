Imports System.IO
Imports System.Windows.Data
Imports System.Windows.Media.Imaging


Namespace Views
    ''' <summary>
    ''' Converts byte array to image using.
    ''' </summary>
    Public Class BinaryImageConverter
        Implements IValueConverter
        ''' <summary>
        ''' Converts a value.
        ''' </summary>
        ''' <param name="value">The value produced by the binding source.</param>
        ''' <param name="targetType">The type of the binding target property.</param>
        ''' <param name="parameter">The converter parameter to use.</param>
        ''' <param name="culture">The culture to use in the converter.</param>
        ''' <returns>
        ''' BitmapImage.
        ''' </returns>
        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            If value IsNot Nothing Then
                Dim toArrayMethod = value.GetType().GetMethod("ToArray", System.Reflection.BindingFlags.Public Or System.Reflection.BindingFlags.Instance)

                If toArrayMethod IsNot Nothing Then
                    value = toArrayMethod.Invoke(value, New Object() {})
                End If
                Dim bytes = TryCast(value, Byte())
                If bytes IsNot Nothing Then
                    Dim bitmapImage As New BitmapImage()
                    bitmapImage.SetSource(New MemoryStream(bytes))
                    Return bitmapImage
                End If
            End If
            Return value
        End Function

        ''' <summary>
        ''' Converts a value.
        ''' </summary>
        ''' <param name="value">The value that is produced by the binding target.</param>
        ''' <param name="targetType">The type to convert to.</param>
        ''' <param name="parameter">The converter parameter to use.</param>
        ''' <param name="culture">The culture to use in the converter.</param>
        ''' <returns>
        ''' A converted value. If the method returns null, the valid null value is used.
        ''' </returns>
        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            Return value
        End Function
    End Class
End Namespace
