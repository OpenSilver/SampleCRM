Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports SampleCRM.Web.Attributes
Imports System.Data.Entity.Migrations
Imports System.Linq

Namespace SampleCRM.Web
    <EnableClientAccess>
    Public Class OrderStatusService
        Inherits SampleCRMService

        <Query>
        Public Function GetOrderStatus() As IQueryable(Of OrderStatus)
            Return _context.OrderStatus
        End Function

        Public Function GetOrderStatusById(ByVal statusId As Long) As OrderStatus
            Return _context.OrderStatus.SingleOrDefault(Function(x) x.Status = statusId)
        End Function

        <Delete>
        <RestrictAccessDeveloperMode>
        Public Sub DeleteStatus(ByVal status As OrderStatus)
            _context.OrderStatus.Remove(status)
        End Sub

        <Insert>
        <RestrictAccessDeveloperMode>
        Public Sub InsertStatus(ByVal status As OrderStatus)
            _context.OrderStatus.AddOrUpdate(status)
        End Sub

        <Update>
        <RestrictAccessDeveloperMode>
        Public Sub UpdateStatus(ByVal status As OrderStatus)
            _context.OrderStatus.AddOrUpdate(status)
        End Sub
    End Class
End Namespace
