Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports SampleCRM.Web.Attributes
Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Migrations
Imports System.Linq

Namespace SampleCRM.Web
    <EnableClientAccess>
    Public Class ProductsService
        Inherits SampleCRMService

        <Query>
        Public Function GetProducts() As IQueryable(Of Products)
            Return _context.Products
        End Function

        <Query>
        Public Function GetTopSaleProducts(ByVal limit As Integer) As IQueryable(Of Products)
            Dim topProducts = _context.Products _
                .Join(_context.OrderItems, Function(p) p.ProductID, Function(oi) oi.ProductID, Function(p, oi) New With {.Product = p, .OrderItem = oi}) _
                .Join(_context.Orders, Function(oi) oi.OrderItem.OrderID, Function(o) o.OrderID, Function(oi, o) New With {.Product = oi.Product, .OrderID = o.OrderID}) _
                .GroupBy(Function(po) po.Product) _
                .OrderByDescending(Function(group) group.Count()) _
                .Take(5) _
                .Select(Function(group) group.Key)
            Return topProducts
        End Function

        Public Function GetProductById(ByVal productId As String) As Products
            Return _context.Products.SingleOrDefault(Function(x) x.ProductID = productId)
        End Function

        <Delete>
        <RestrictAccessDeveloperMode>
        Public Sub DeleteProduct(ByVal product As Products)
            _context.Products.Remove(product)
            Dim dProduct = _context.Products.FirstOrDefault(Function(x) x.ProductID = product.ProductID)
            If dProduct Is Nothing Then Return
            _context.Products.Remove(dProduct)
            _context.SaveChanges()
        End Sub

        <Insert>
        <RestrictAccessDeveloperMode>
        Public Sub InsertProduct(ByVal product As Products)
            product.ProductID = New Random().[Next](CInt(Math.Pow(10, 12)), CInt(Math.Pow(10, 13)) - 1).ToString()
            product.CreatedOn = Date.Now.ToString()
            product.LastModifiedOn = Date.Now.ToString()
            _context.Products.Add(product)
            _context.SaveChanges()
        End Sub

        <Update>
        <RestrictAccessDeveloperMode>
        Public Sub UpdateProduct(ByVal product As Products)
            product.LastModifiedOn = Date.Now.ToString()
            _context.Products.AddOrUpdate(product)
            _context.SaveChanges()
        End Sub
    End Class
End Namespace
