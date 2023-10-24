Imports System.Windows
Imports System.Windows.Data


Namespace Views
    Public Class InverseBoolToVisibilityConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            Dim boolValue As Boolean = TypeOf value Is Boolean AndAlso CBool(value)

            If Not boolValue Then
                Return Visibility.Visible
            Else
                Return Visibility.Collapsed
            End If
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            If TypeOf value Is Visibility Then
                Return CType(value, Visibility) <> Visibility.Visible
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
