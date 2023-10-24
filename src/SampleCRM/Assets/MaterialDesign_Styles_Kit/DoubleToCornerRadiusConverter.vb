Imports System.Windows

Namespace MaterialDesign_Styles_Kit
    Public Class DoubleToCornerRadiusConverter
#If OPENSILVER Then
        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object
#Else
        Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object
#End If
            If TypeOf value Is Double Then
                Return New CornerRadius(CDbl(value))
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
