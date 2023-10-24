Imports System.Windows.Data


Namespace Views
    Public NotInheritable Class DateTimeFormatConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            Try
                If TypeOf value Is String Then
                    Return Date.Parse(value.ToString()).ToShortDateString()
                ElseIf TypeOf value Is Date Then
                    If CType(value, Date) = Date.MinValue Then
                        Return "N/A"
                    Else
                        Return CType(value, Date).ToShortDateString()
                    End If
                Else
                    Return "N/A"
                End If
            Catch
                Return "N/A"
            End Try
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace
