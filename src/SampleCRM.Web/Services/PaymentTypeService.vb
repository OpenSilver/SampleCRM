Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports SampleCRM.Web.Attributes
Imports System.Data.Entity.Migrations
Imports System.Linq

Namespace SampleCRM.Web
    <EnableClientAccess>
    Public Class PaymentTypeService
        Inherits SampleCRMService

        <Query>
        Public Function GetPaymentTypes() As IQueryable(Of PaymentTypes)
            Return _context.PaymentTypes
        End Function

        Public Function GetPaymentTypeById(ByVal paymentTypeId As Long) As PaymentTypes
            Return _context.PaymentTypes.SingleOrDefault(Function(x) x.PaymentTypeID = paymentTypeId)
        End Function

        <Delete>
        <RestrictAccessDeveloperMode>
        Public Sub DeletePaymentType(ByVal paymentType As PaymentTypes)
            _context.PaymentTypes.Remove(paymentType)
        End Sub

        <Insert>
        <RestrictAccessDeveloperMode>
        Public Sub InsertPaymentType(ByVal paymentType As PaymentTypes)
            _context.PaymentTypes.AddOrUpdate(paymentType)
        End Sub

        <Update>
        <RestrictAccessDeveloperMode>
        Public Sub UpdatePaymentType(ByVal paymentType As PaymentTypes)
            _context.PaymentTypes.AddOrUpdate(paymentType)
        End Sub
    End Class
End Namespace
