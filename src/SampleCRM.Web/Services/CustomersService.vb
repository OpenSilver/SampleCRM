Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports SampleCRM.Web.Attributes
Imports System
Imports System.Data.Entity.Migrations
Imports System.Linq

Namespace SampleCRM.Web
    <EnableClientAccess>
    Public Class CustomersService
        Inherits SampleCRMService

        <Query>
        Public Function GetCustomers() As IQueryable(Of Customers)
            Return _context.Customers
        End Function

        <Query>
        Public Function GetLatestCustomers(ByVal limit As Integer) As IQueryable(Of Customers)
            Return _context.Customers.OrderByDescending(Function(x) x.CreatedOn).Take(limit)
        End Function

        <Delete>
        <RestrictAccessDeveloperMode>
        Public Sub DeleteCustomer(ByVal customer As Customers)
            Dim dCustomer = _context.Customers.FirstOrDefault(Function(x) x.CustomerID = customer.CustomerID)
            If dCustomer Is Nothing Then Return
            _context.Customers.Remove(dCustomer)
            _context.SaveChanges()
        End Sub

        <Insert>
        <RestrictAccessDeveloperMode>
        Public Sub InsertCustomer(ByVal customer As Customers)
            customer.CustomerID = New Random().[Next](CInt(Math.Pow(10, 12)), CInt(Math.Pow(10, 13)) - 1)
            If customer.CustomerID < 0 Then customer.CustomerID = customer.CustomerID * -1
            customer.CreatedOn = Date.Now.ToString()
            customer.LastModifiedOn = customer.CreatedOn
            _context.Customers.Add(customer)
            _context.SaveChanges()
        End Sub

        <Update>
        <RestrictAccessDeveloperMode>
        Public Sub UpdateCustomer(ByVal customer As Customers)
            customer.LastModifiedOn = Date.Now.ToString()
            _context.Customers.AddOrUpdate(customer)
            _context.SaveChanges()
        End Sub

        Private Class CSharpImpl
            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
    End Class
End Namespace
