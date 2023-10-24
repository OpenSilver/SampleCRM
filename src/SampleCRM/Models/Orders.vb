Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel

Namespace Web
    Partial Public Class Orders
        Inherits Entity

        Protected Overrides Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
            If e.PropertyName = NameOf(CustomerID) Then

                If CustomersCombo IsNot Nothing AndAlso CustomersCombo.Any() Then
                    Customer = CustomersCombo.FirstOrDefault(Function(x) x.CustomerID = CustomerID)
                Else
                    Customer = New Customers()
                End If
            ElseIf e.PropertyName = NameOf(Status) Then
                PaymentTypesVisible = Status > 0
                ShippedViaVisible = Status > 1
                ShippedDateVisible = ShippedViaVisible
                DeliveredDateVisible = Status > 2
            End If

            MyBase.OnPropertyChanged(e)
        End Sub

        Public ReadOnly Property IsNew As Boolean
            Get
                Return OrderID <= 0
            End Get
        End Property

        Private _paymentTypesVisible As Boolean

        Public Property PaymentTypesVisible As Boolean
            Get
                Return _paymentTypesVisible
            End Get
            Set(ByVal value As Boolean)

                If _paymentTypesVisible <> value Then
                    _paymentTypesVisible = value
                    OnPropertyChanged(New PropertyChangedEventArgs("PaymentTypesVisible"))
                End If
            End Set
        End Property

        Private _shippedDateVisible As Boolean

        Public Property ShippedDateVisible As Boolean
            Get
                Return _shippedDateVisible
            End Get
            Set(ByVal value As Boolean)

                If _shippedDateVisible <> value Then
                    _shippedDateVisible = value
                    OnPropertyChanged(New PropertyChangedEventArgs("ShippedDateVisible"))
                End If
            End Set
        End Property

        Private _shippedViaVisible As Boolean

        Public Property ShippedViaVisible As Boolean
            Get
                Return _shippedViaVisible
            End Get
            Set(ByVal value As Boolean)

                If _shippedViaVisible <> value Then
                    _shippedViaVisible = value
                    OnPropertyChanged(New PropertyChangedEventArgs("ShippedViaVisible"))
                End If
            End Set
        End Property

        Private _deliveredDateVisible As Boolean

        Public Property DeliveredDateVisible As Boolean
            Get
                Return _deliveredDateVisible
            End Get
            Set(ByVal value As Boolean)

                If _deliveredDateVisible <> value Then
                    _deliveredDateVisible = value
                    OnPropertyChanged(New PropertyChangedEventArgs("DeliveredDateVisible"))
                End If
            End Set
        End Property

        Private _shippers As IEnumerable(Of Shippers)

        Public Property Shippers As IEnumerable(Of Shippers)
            Get
                Return _shippers
            End Get
            Set(ByVal value As IEnumerable(Of Shippers))

                If Not _shippers.Equals(value) Then
                    _shippers = value
                    OnPropertyChanged(New PropertyChangedEventArgs("Shippers"))
                End If
            End Set
        End Property

        Private _paymentTypes As IEnumerable(Of PaymentTypes)

        Public Property PaymentTypes As IEnumerable(Of PaymentTypes)
            Get
                Return _paymentTypes
            End Get
            Set(ByVal value As IEnumerable(Of PaymentTypes))

                If Not _paymentTypes.Equals(value) Then
                    _paymentTypes = value
                    OnPropertyChanged(New PropertyChangedEventArgs("PaymentTypes"))
                End If
            End Set
        End Property

        Private _countryCodes As IEnumerable(Of CountryCodes)

        Public Property CountryCodes As IEnumerable(Of CountryCodes)
            Get
                Return _countryCodes
            End Get
            Set(ByVal value As IEnumerable(Of CountryCodes))

                If Not _countryCodes.Equals(value) Then
                    _countryCodes = value
                    OnPropertyChanged(New PropertyChangedEventArgs("CountryCodes"))
                End If
            End Set
        End Property

        Private _statuses As IEnumerable(Of OrderStatus)

        Public Property Statuses As IEnumerable(Of OrderStatus)
            Get
                Return _statuses
            End Get
            Set(ByVal value As IEnumerable(Of OrderStatus))

                If Not _statuses.Equals(value) Then
                    _statuses = value
                    OnPropertyChanged(New PropertyChangedEventArgs("Statuses"))
                End If
            End Set
        End Property

        Private _shipCountryName As String

        Public Property ShipCountryName As String
            Get
                Return _shipCountryName
            End Get
            Set(ByVal value As String)

                If _shipCountryName <> value Then
                    _shipCountryName = value
                    OnPropertyChanged(New PropertyChangedEventArgs("ShipCountryName"))
                End If
            End Set
        End Property

        Private _statusDesc As String

        Public Property StatusDesc As String
            Get
                Return _statusDesc
            End Get
            Set(ByVal value As String)

                If _statusDesc <> value Then
                    _statusDesc = value
                    OnPropertyChanged(New PropertyChangedEventArgs("StatusDesc"))
                End If
            End Set
        End Property

        Private _customersCombo As IEnumerable(Of Customers)

        Public Property CustomersCombo As IEnumerable(Of Customers)
            Get
                Return _customersCombo
            End Get
            Set(ByVal value As IEnumerable(Of Customers))

                If Not _customersCombo.Equals(value) Then
                    _customersCombo = value
                    OnPropertyChanged(New PropertyChangedEventArgs("CustomersCombo"))
                End If
            End Set
        End Property

        Private _customer As Customers = New Customers()

        Public Property Customer As Customers
            Get
                Return _customer
            End Get
            Set(ByVal value As Customers)

                If Not _customer.Equals(value) Then
                    _customer = value

                    If _customer IsNot Nothing Then
                        ShipAddress = _customer.AddressLine1
                        ShipCity = _customer.City
                        ShipRegion = _customer.Region
                        ShipCountryCode = _customer.CountryCode
                        ShipPostalCode = _customer.PostalCode
                    End If

                    OnPropertyChanged(New PropertyChangedEventArgs("Customer"))
                End If
            End Set
        End Property

        Private _isEditMode As Boolean

        Public Property IsEditMode As Boolean
            Get
                Return _isEditMode
            End Get
            Set(ByVal value As Boolean)

                If _isEditMode <> value Then
                    _isEditMode = value
                    OnPropertyChanged(New PropertyChangedEventArgs("IsEditMode"))
                End If
            End Set
        End Property

        Private Class CSharpImpl
            <Obsolete("Please refactor calling code to use normal Visual Basic assignment")>
            Shared Function __Assign(Of T)(ByRef target As T, value As T) As T
                target = value
                Return value
            End Function
        End Class
    End Class
End Namespace
