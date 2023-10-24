Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports SampleCRM.Web.Attributes
Imports System.Data.Entity.Migrations
Imports System.Linq

Namespace SampleCRM.Web
    <EnableClientAccess>
    Public Class OrderItemsService
        Inherits SampleCRMService

        <Query>
        Public Function GetOrderItems() As IQueryable(Of OrderItems)
            Return _context.OrderItems
        End Function

        Public Function GetOrderItemById(ByVal orderId As Integer) As OrderItems
            Return _context.OrderItems.SingleOrDefault(Function(x) x.OrderID = orderId)
        End Function

        <Query>
        Public Function GetOrderItemsOfOrder(ByVal orderId As Long) As IQueryable(Of OrderItems)
            Dim itemsQuery = _context.OrderItems.Where(Function(x) x.OrderID = orderId)
            Return itemsQuery
        End Function

        <Query>
        Public Function GetOrderItemsOfProduct(ByVal productId As String) As IQueryable(Of OrderItems)
            Return _context.OrderItems.Where(Function(x) x.ProductID = productId)
        End Function

        <Delete>
        <RestrictAccessDeveloperMode>
        Public Sub DeleteOrderItem(ByVal orderItem As OrderItems)
            Dim dOrderItem = _context.OrderItems.FirstOrDefault(Function(x) x.OrderID = orderItem.OrderID AndAlso x.OrderLine = orderItem.OrderLine)
            If dOrderItem Is Nothing Then Return
            _context.OrderItems.Remove(dOrderItem)
            _context.SaveChanges()
        End Sub

        <Insert>
        <RestrictAccessDeveloperMode>
        Public Sub InsertOrderItem(ByVal orderItem As OrderItems)
            Dim lastItem = _context.OrderItems.Where(Function(x) x.OrderID = orderItem.OrderID).OrderByDescending(Function(x) x.OrderLine).FirstOrDefault()

            If lastItem Is Nothing Then
                orderItem.OrderLine = 1
            Else
                orderItem.OrderLine = lastItem.OrderLine + 1
            End If

            _context.OrderItems.Add(orderItem)
            _context.SaveChanges()
        End Sub

        <Update>
        <RestrictAccessDeveloperMode>
        Public Sub UpdateOrderItem(ByVal orderItem As OrderItems)
            _context.OrderItems.AddOrUpdate(orderItem)
            _context.SaveChanges()
        End Sub
    End Class
End Namespace
