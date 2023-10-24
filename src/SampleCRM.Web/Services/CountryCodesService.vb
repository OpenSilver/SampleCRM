Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports SampleCRM.Web.Attributes
Imports System.Data.Entity.Migrations
Imports System.Linq

Namespace SampleCRM.Web
    <EnableClientAccess>
    Public Class CountryCodesService
        Inherits SampleCRMService

        <Query>
        Public Function GetCountries() As IQueryable(Of CountryCodes)
            Return _context.CountryCodes
        End Function

        Public Function GetCountryById(ByVal countryCodeID As String) As CountryCodes
            Return _context.CountryCodes.SingleOrDefault(Function(x) x.CountryCodeID = countryCodeID)
        End Function

        <Delete>
        <RestrictAccessDeveloperMode>
        Public Sub DeleteCountry(ByVal country As CountryCodes)
            _context.CountryCodes.Remove(country)
        End Sub

        <Insert>
        <RestrictAccessDeveloperMode>
        Public Sub InsertCountry(ByVal country As CountryCodes)
            _context.CountryCodes.AddOrUpdate(country)
        End Sub

        <Update>
        <RestrictAccessDeveloperMode>
        Public Sub UpdateCountry(ByVal country As CountryCodes)
            _context.CountryCodes.AddOrUpdate(country)
        End Sub
    End Class
End Namespace
