Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports SampleCRM.Web.Attributes
Imports System.Data.Entity.Migrations
Imports System.Linq

Namespace SampleCRM.Web
    <EnableClientAccess>
    Public Class CategoryService
        Inherits SampleCRMService

        <Query>
        Public Function GetCategories() As IQueryable(Of Categories)
            Return _context.Categories
        End Function

        <Delete>
        <RestrictAccessDeveloperMode>
        Public Sub DeleteCategory(ByVal category As Categories)
            _context.Categories.Remove(category)
            _context.SaveChanges()
        End Sub

        <Insert>
        <RestrictAccessDeveloperMode>
        Public Sub InsertCategory(ByVal category As Categories)
            _context.Categories.AddOrUpdate(category)
            _context.SaveChanges()
        End Sub

        <Update>
        <RestrictAccessDeveloperMode>
        Public Sub UpdateCategory(ByVal category As Categories)
            _context.Categories.AddOrUpdate(category)
            _context.SaveChanges()
        End Sub
    End Class
End Namespace
