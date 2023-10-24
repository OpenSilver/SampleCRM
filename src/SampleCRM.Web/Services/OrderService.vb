Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports SampleCRM.Web.Attributes
Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Migrations
Imports System.Linq

Namespace SampleCRM.Web
    <EnableClientAccess>
    Public Class OrderService
        Inherits SampleCRMService

        <Query>
        Public Function GetOrders() As IQueryable(Of Orders)
            Return _context.Orders
        End Function

        <Query>
        Public Function GetLatestOrders(ByVal limit As Integer) As IQueryable(Of Orders)
            Return _context.Orders.OrderByDescending(Function(x) x.OrderDate).Take(limit)
        End Function

        <Query>
        Public Function GetOrdersOfCustomer(ByVal customerId As Long) As IQueryable(Of Orders)
            Return _context.Orders.Where(Function(x) x.CustomerID = customerId)
        End Function

        <Delete>
        <RestrictAccessDeveloperMode>
        Public Sub DeleteOrder(ByVal order As Orders)
            Dim dorder = _context.Orders.FirstOrDefault(Function(x) x.OrderID = order.OrderID)
            If dorder Is Nothing Then Return
            _context.Orders.Remove(dorder)
            _context.SaveChanges()
        End Sub

        <Insert>
        <RestrictAccessDeveloperMode>
        Public Sub InsertOrder(ByVal order As Orders)
            order.OrderID = New Random().[Next](CInt(Math.Pow(10, 12)), CInt(Math.Pow(10, 13)) - 1)
            If order.OrderID < 0 Then order.OrderID = order.OrderID * -1
            order.LastModifiedOn = Date.Now.ToString()
            order.OrderDate = order.LastModifiedOn
            _context.Orders.Add(order)
            _context.SaveChanges()
        End Sub

        <Update>
        <RestrictAccessDeveloperMode>
        Public Sub UpdateOrder(ByVal order As Orders)
            order.LastModifiedOn = Date.Now.ToString()
            _context.Orders.AddOrUpdate(order)
            _context.SaveChanges()
        End Sub
    End Class
End Namespace
