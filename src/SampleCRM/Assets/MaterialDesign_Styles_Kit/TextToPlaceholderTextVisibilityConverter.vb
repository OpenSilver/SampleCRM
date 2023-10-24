Imports System
Imports System.Windows
Imports System.Windows.Data
Imports System.Globalization

Namespace MaterialDesign_Styles_Kit
    Public Class TextToPlaceholderTextVisibilityConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object

            If TypeOf value Is String Then
                If String.IsNullOrEmpty(CStr(value)) Then
                    Return Visibility.Visible
                End If
                Return Visibility.Collapsed
            End If
            Throw New InvalidOperationException("The IValueConverter expects a value of type String.")
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object

            Throw New NotImplementedException()
        End Function
    End Class
End Namespace
