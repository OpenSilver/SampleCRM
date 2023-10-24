Imports System.Globalization
Imports System.Windows
Imports System.Windows.Data

Namespace Views
    Public Class BoolToVisibilityConverter
        Implements IValueConverter
        Public Shared Function Convert(value As Boolean) As Visibility
            Return If(value, Visibility.Visible, Visibility.Collapsed)
        End Function

        Public Shared Function ConvertBack(visibility As Visibility) As Boolean
            Return visibility = Visibility.Visible
        End Function

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim boolValue As Boolean = If(TypeOf value Is Boolean AndAlso DirectCast(value, Boolean), True, False)
            If boolValue Then
                Return Visibility.Visible
            Else
                Return Visibility.Collapsed
            End If
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            If TypeOf value Is Visibility Then
                Return DirectCast(value, Visibility) = Visibility.Visible
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
