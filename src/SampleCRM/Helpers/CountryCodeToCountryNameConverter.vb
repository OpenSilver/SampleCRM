Imports System.Globalization
Imports System.Windows.Data
Imports SampleCRM.Web.SampleCRM.Web

Namespace Views
    Public Class CountryCodeToCountryNameConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            If value IsNot Nothing Then
                Dim countryCode As String = TryCast(value, String)
                If Not String.IsNullOrEmpty(countryCode) Then
                    Dim countryCodesContext As New CountryCodesContext()
                    Dim query = countryCodesContext.GetCountryByIdQuery(countryCode)
                    Dim entityObj = countryCodesContext.Load(query)
                    value = entityObj.Entities.FirstOrDefault()
                End If
            End If

            Return value
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Return value
        End Function
    End Class
End Namespace
