Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports SampleCRM.Web.Models
Imports System.Linq

Namespace SampleCRM.Web
    <EnableClientAccess>
    Public Class CountService
        Inherits SampleCRMService

        <Query>
        Public Function GetAllCounts() As IQueryable(Of CountModel)
            Dim countModel = New CountModel With {
            .OrderCount = _context.Orders.Count(),
            .CustomerCount = _context.Customers.Count(),
            .CategoryCount = _context.Categories.Count(),
            .ProductCount = _context.Products.Count()
        }
            Return New CountModel() {countModel}.AsQueryable()
        End Function
    End Class
End Namespace
