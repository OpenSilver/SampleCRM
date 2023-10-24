Imports System.Windows.Media

Namespace MaterialDesign_Styles_Kit
    Public Class AccentColorConverter
#If OPENSILVER Then
        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object
#Else
        Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object
#End If
            If TypeOf value Is SolidColorBrush Then
                Dim oldColor As Color = DirectCast(value, SolidColorBrush).Color
                Dim newR As Byte = If(oldColor.R > 40, CByte(oldColor.R - 40), CByte(0))
                Dim newG As Byte = If(oldColor.G > 40, CByte(oldColor.G - 40), CByte(0))
                Dim newB As Byte = If(oldColor.B > 40, CByte(oldColor.B - 40), CByte(0))
                Dim newColor As Color = Color.FromArgb(oldColor.A, newR, newG, newB)
                Return New SolidColorBrush(newColor)
            End If
            Return value
        End Function

#If OPENSILVER Then
        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object
#Else
        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object
#End If
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace
