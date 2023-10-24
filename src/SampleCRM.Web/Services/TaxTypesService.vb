Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports SampleCRM.Web.Attributes
Imports System.Data.Entity.Migrations
Imports System.Linq

Namespace SampleCRM.Web
    <EnableClientAccess>
    Public Class TaxTypesService
        Inherits SampleCRMService

        <Query>
        Public Function GetTaxTypes() As IQueryable(Of TaxTypes)
            Return _context.TaxTypes
        End Function

        Public Function GetTaxTypeById(ByVal taxTypeId As Long) As TaxTypes
            Return _context.TaxTypes.SingleOrDefault(Function(x) x.TaxTypeID = taxTypeId)
        End Function

        <Delete>
        <RestrictAccessDeveloperMode>
        Public Sub DeleteTaxTypes(ByVal taxType As TaxTypes)
            _context.TaxTypes.Remove(taxType)
        End Sub

        <Insert>
        <RestrictAccessDeveloperMode>
        Public Sub InsertTaxTypes(ByVal taxType As TaxTypes)
            _context.TaxTypes.AddOrUpdate(taxType)
        End Sub

        <Update>
        <RestrictAccessDeveloperMode>
        Public Sub UpdateTaxTypes(ByVal taxType As TaxTypes)
            _context.TaxTypes.AddOrUpdate(taxType)
        End Sub
    End Class
End Namespace
