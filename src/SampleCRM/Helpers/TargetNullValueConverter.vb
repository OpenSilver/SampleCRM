Imports System.Globalization
Imports System.Windows.Data

''' <summary>
''' Two way IValueConverter that lets you bind a property on a bindable object that can be an empty string value to a dependency property that should be set to null in that case.
''' </summary>
Public Class TargetNullValueConverter
    Implements IValueConverter

    ''' <summary>
    ''' Converts <c>null</c> or empty strings to <c>null</c>.
    ''' </summary>
    ''' <param name="value">The value to convert.</param>
    ''' <param name="targetType">The expected type of the result (ignored).</param>
    ''' <param name="parameter">Optional parameter (ignored).</param>
    ''' <param name="culture">The culture for the conversion (ignored).</param>
    ''' <returns>If the <paramref name="value"/>is <c>null</c> or empty, this method returns <c>null</c> otherwise it returns the <paramref name="value"/>.</returns>
    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

        Dim strValue As String = value

        Return If(String.IsNullOrEmpty(strValue), Nothing, value)

    End Function

    ''' <summary>
    ''' Converts <c>null</c> back to <see cref="String.Empty"/>.
    ''' </summary>
    ''' <param name="value">The value to convert.</param>
    ''' <param name="targetType">The expected type of the result (ignored).</param>
    ''' <param name="parameter">Optional parameter (ignored).</param>
    ''' <param name="culture">The culture for the conversion (ignored).</param>
    ''' <returns>If <paramref name="value"/> is <c>null</c>, it returns <see cref="String.Empty"/> otherwise <paramref name="value"/>.</returns>
    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return If(value IsNot Nothing, value, String.Empty)
    End Function

End Class