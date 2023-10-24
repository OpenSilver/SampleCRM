Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports SampleCRM.Web.Attributes
Imports System.Data.Entity.Migrations
Imports System.Linq

Namespace SampleCRM.Web
    <EnableClientAccess>
    Public Class ShippersService
        Inherits SampleCRMService

        <Query>
        Public Function GetShippers() As IQueryable(Of Shippers)
            Return _context.Shippers
        End Function

        Public Function GetShipperById(ByVal shipperId As Long) As Shippers
            Return _context.Shippers.SingleOrDefault(Function(x) x.ShipperID = shipperId)
        End Function

        <Delete>
        <RestrictAccessDeveloperMode>
        Public Sub DeleteShippers(ByVal shipper As Shippers)
            _context.Shippers.Remove(shipper)
        End Sub

        <Insert>
        <RestrictAccessDeveloperMode>
        Public Sub InsertShippers(ByVal shipper As Shippers)
            _context.Shippers.AddOrUpdate(shipper)
        End Sub

        <Update>
        <RestrictAccessDeveloperMode>
        Public Sub UpdateShippers(ByVal shipper As Shippers)
            _context.Shippers.AddOrUpdate(shipper)
        End Sub
    End Class
End Namespace
