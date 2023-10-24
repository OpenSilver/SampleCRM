Imports OpenRiaServices.DomainServices.Client
Imports SampleCRM.Web.SampleCRM.Web
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Navigation

Namespace Views
    Partial Public Class Customers
        Inherits BasePage

        Private _customersContext As New CustomersContext()
        Private _countryCodesContext As New CountryCodesContext()
        Private _orderContext As New OrderContext()
        Private _orderStatusContext As New OrderStatusContext()
        Private _shippersContext As New ShippersContext()
        Private _paymentTypesContext As New PaymentTypeContext()
        Private _ordersTabSelected As Boolean

        Private _customersCollection As IEnumerable(Of Web.Customers)
        Public Property CustomersCollection As IEnumerable(Of Web.Customers)
            Get
                Return _customersCollection
            End Get
            Set(value As IEnumerable(Of Web.Customers))
                If Not Object.Equals(_customersCollection, value) Then
                    _customersCollection = value
                    OnPropertyChanged()
                    OnPropertyChanged("FilteredCustomersCollection")
                    SelectedCustomer = FilteredCustomersCollection.FirstOrDefault()
                End If
            End Set
        End Property

        Public ReadOnly Property FilteredCustomersCollection As IEnumerable(Of Web.Customers)
            Get
                If String.IsNullOrWhiteSpace(_searchText) Then
                    Return _customersCollection
                Else
                    Return _customersCollection.Where(Function(x) x.FullName.ToLowerInvariant().Contains(_searchText.ToLowerInvariant()))
                End If
            End Get
        End Property

        Private _countryCodes As IEnumerable(Of Web.CountryCodes)
        Public Property CountryCodes As IEnumerable(Of Web.CountryCodes)
            Get
                Return _countryCodes
            End Get
            Set(value As IEnumerable(Of Web.CountryCodes))
                If Not Object.Equals(_countryCodes, value) Then
                    _countryCodes = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _selectedCustomer As Web.Customers
        Public Property SelectedCustomer As Web.Customers
            Get
                Return _selectedCustomer
            End Get
            Set(value As Web.Customers)
                If Not Object.Equals(_selectedCustomer, value) Then
                    _selectedCustomer = value
                    OnPropertyChanged()
                    OnPropertyChanged("AnySelectedCustomer")
                    LoadOrdersOfCustomer()
#If DEBUG Then
                    Console.WriteLine($"Customers, Customer: {value.FirstName} selected")
#End If
                End If
            End Set
        End Property

        Public ReadOnly Property AnySelectedCustomer As Boolean
            Get
                Return _selectedCustomer IsNot Nothing
            End Get
        End Property

        Private _searchText As String
        Public Property SearchText As String
            Get
                Return _searchText
            End Get
            Set(value As String)
                If Not Object.Equals(_searchText, value) Then
                    _searchText = value
                    OnPropertyChanged()
                    OnPropertyChanged("FilteredCustomersCollection")
                    SelectedCustomer = FilteredCustomersCollection.FirstOrDefault()
                End If
            End Set
        End Property

        Private _ordersCollection As IEnumerable(Of Web.Orders)
        Public Property OrdersCollection As IEnumerable(Of Web.Orders)
            Get
                Return _ordersCollection
            End Get
            Set(value As IEnumerable(Of Web.Orders))
                If Not Object.Equals(_ordersCollection, value) Then
                    _ordersCollection = value
                    OnPropertyChanged()
                    OnPropertyChanged("FilteredOrdersCollection")
                    SelectedOrder = FilteredOrdersCollection.FirstOrDefault()
                End If
            End Set
        End Property

        Public ReadOnly Property FilteredOrdersCollection As IEnumerable(Of Web.Orders)
            Get
                If String.IsNullOrWhiteSpace(_searchOrderText) Then
                    Return _ordersCollection
                Else
                    Return _ordersCollection.Where(Function(x) x.OrderID.ToString().Contains(_searchOrderText))
                End If
            End Get
        End Property

        Private _selectedOrder As Web.Orders
        Public Property SelectedOrder As Web.Orders
            Get
                Return _selectedOrder
            End Get
            Set(value As Web.Orders)
                If Not Object.Equals(_selectedOrder, value) Then
                    _selectedOrder = value
                    OnPropertyChanged()
#If DEBUG Then
                    Console.WriteLine($"Orders, Order: {value.OrderID} selected")
#End If
                End If
            End Set
        End Property

        Private _searchOrderText As String
        Public Property SearchOrderText As String
            Get
                Return _searchOrderText
            End Get
            Set(value As String)
                If Not Object.Equals(_searchOrderText, value) Then
                    _searchOrderText = value
                    OnPropertyChanged()
                    OnPropertyChanged("FilteredOrdersCollection")
                    SelectedOrder = FilteredOrdersCollection.FirstOrDefault()
                End If
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
            DataContext = Me
        End Sub

        Protected Overrides Sub OnSizeChanged(sender As Object, e As SizeChangedEventArgs)
            MyBase.OnSizeChanged(sender, e)

            If IsMobileUI Then
                grdHead.ColumnDefinitions(2).Width = New GridLength(1, GridUnitType.Star)

                Grid.SetColumn(grdSearch, 0)
                Grid.SetRow(grdSearch, 2)
                grdSearch.Margin = New Thickness(0, 0, 0, 10)

                Grid.SetRow(customerCard, 0)
                Grid.SetColumn(customerCard, 0)

                Grid.SetColumn(grdCustomerDetails, 0)
                Grid.SetRow(grdCustomerDetails, 1)

                grdTbCustomer.RowDefinitions(0).Height = GridLength.Auto
                grdTbCustomer.RowDefinitions(1).Height = GridLength.Auto

                grdTbCustomer.ColumnDefinitions(0).Width = New GridLength(2, GridUnitType.Star)
                grdTbCustomer.ColumnDefinitions(1).Width = GridLength.Auto

                formCustomer.EditTemplate = TryCast(Resources("dtNarrowCustomers"), DataTemplate)

                grdTbOrders.ColumnDefinitions(2).Width = New GridLength(1, GridUnitType.Star)

                Grid.SetColumn(grdOrderSearch, 0)
                Grid.SetRow(grdOrderSearch, 2)
                grdOrderSearch.Margin = New Thickness(0, 0, 0, 10)
            Else
                grdHead.ColumnDefinitions(2).Width = New GridLength(405, GridUnitType.Pixel)

                Grid.SetColumn(grdSearch, 2)
                Grid.SetRow(grdSearch, 0)
                grdSearch.Margin = New Thickness()

                Grid.SetRow(customerCard, 0)
                Grid.SetColumn(customerCard, 0)

                Grid.SetRow(grdCustomerDetails, 0)
                Grid.SetColumn(grdCustomerDetails, 1)

                grdTbCustomer.RowDefinitions(0).Height = New GridLength(2, GridUnitType.Star)
                grdTbCustomer.RowDefinitions(1).Height = GridLength.Auto

                grdTbCustomer.ColumnDefinitions(0).Width = New GridLength(2, GridUnitType.Star)
                grdTbCustomer.ColumnDefinitions(1).Width = New GridLength(4, GridUnitType.Star)

                formCustomer.EditTemplate = TryCast(Resources("dtWideCustomers"), DataTemplate)

                grdTbOrders.ColumnDefinitions(2).Width = New GridLength(405, GridUnitType.Pixel)

                Grid.SetColumn(grdOrderSearch, 2)
                Grid.SetRow(grdOrderSearch, 0)
                grdOrderSearch.Margin = New Thickness()
            End If
        End Sub

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            LoadElements()
        End Sub

        Private Async Sub LoadElements()
            Dim customersQuery = _customersContext.GetCustomersQuery()
            Dim customersOp = Await _customersContext.LoadAsync(customersQuery)
            CustomersCollection = customersOp.Entities

            Await LoadCountryCodes()
#If DEBUG Then
            Console.WriteLine("Customers Collection:" & CustomersCollection.Count())
            For Each item In CustomersCollection
                Console.WriteLine("Customer Name:" & item.FirstName)
                Console.WriteLine("Customer Picture Bytes:" & item.Picture.Length)
            Next
#End If
        End Sub

        Private Async Function LoadCountryCodes() As Task
            Dim countryCodesquery = _countryCodesContext.GetCountriesQuery()
            Dim countriesOp = Await _countryCodesContext.LoadAsync(countryCodesquery)
            CountryCodes = countriesOp.Entities

            For Each c In CustomersCollection
                c.CountryCodes = CountryCodes
                c.CountryName = CountryCodes.SingleOrDefault(Function(x) x.CountryCodeID = c.CountryCode).Name
            Next
        End Function

        Private Async Sub LoadOrdersOfCustomer()
            If SelectedCustomer Is Nothing Then
                Return
            End If

            If Not _ordersTabSelected Then
                Return
            End If

            Dim customerId = SelectedCustomer.CustomerID
            Dim ordersQuery = _orderContext.GetOrdersOfCustomerQuery(customerId)
            Dim ordersOp = Await _orderContext.LoadAsync(ordersQuery)
            OrdersCollection = ordersOp.Entities

            For Each o In OrdersCollection
                o.ShipCountryName = CountryCodes.SingleOrDefault(Function(x) x.CountryCodeID = o.ShipCountryCode)?.Name
            Next

#If DEBUG Then
            Console.WriteLine("Orders Collection:" & OrdersCollection.Count())
            For Each item In OrdersCollection
                Console.WriteLine("Order Id:" & item.OrderID)
            Next
#End If
        End Sub

        Private Sub btnSearchCancel_Click(sender As Object, e As RoutedEventArgs)
            SearchText = String.Empty
        End Sub

        Private Sub btnSearch_Click(sender As Object, e As RoutedEventArgs)
            ' Handle search
        End Sub

        Private Sub grdCustomers_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
#If DEBUG Then
            Console.WriteLine($"grdCustomers_SelectionChanged, {e.AddedItems.Count} Items Added")
#End If
        End Sub

        Private Sub formCustomer_EditEnded(sender As Object, e As DataFormEditEndedEventArgs)
            If e.EditAction = DataFormEditAction.Commit Then
                _customersContext.SubmitChanges(AddressOf OnFormCustomerSubmitCompleted, Nothing)
            ElseIf e.EditAction = DataFormEditAction.Cancel Then
                SelectedCustomer.IsEditMode = False
            End If
        End Sub

        Private Sub OnFormCustomerSubmitCompleted(so As SubmitOperation)
            If so.HasError Then
                If so.Error.Message.StartsWith("Submit operation failed. Access to operation") Then
                    ErrorWindow.Show("Access Denied", "Insufficient User Role", so.Error.Message)
                Else
                    ErrorWindow.Show("Access Denied", so.Error.Message, "")
                End If

                so.MarkErrorAsHandled()
            Else
                OnPropertyChanged("SelectedCustomer")
                OnPropertyChanged("CustomersCollection")
                OnPropertyChanged("FilteredCustomersCollection")
            End If
        End Sub

        Private Sub btnOrderSearchCancel_Click(sender As Object, e As RoutedEventArgs)
            SearchOrderText = String.Empty
        End Sub

        Private Sub tcDetails_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
            Dim anySelected = e.AddedItems.Count > 0
            If anySelected Then
                _ordersTabSelected = e.AddedItems.Contains(tbOrders)
                If _ordersTabSelected Then
                    LoadOrdersOfCustomer()
                End If
            Else
                _ordersTabSelected = False
            End If
        End Sub

        Private Sub grdOrders_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
#If DEBUG Then
            Console.WriteLine($"grdOrders_SelectionChanged, {e.AddedItems.Count} Items Added")
#End If
        End Sub

        Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs)
            Dim result = MessageBox.Show("Are you sure to delete the customer and all orders belong to that customer?", MessageBoxButton.OKCancel)
            If result <> MessageBoxResult.OK Then
                Return
            End If

            If _customersContext.Customers.CanRemove Then
                _customersContext.Customers.Remove(SelectedCustomer)
                _customersContext.SubmitChanges(AddressOf OnDeleteSubmitCompleted, Nothing)
            Else
                Throw New AccessViolationException("RIA Service Delete Entity for Customer Context is denied")
            End If
        End Sub

        Private Sub OnDeleteSubmitCompleted(so As SubmitOperation)
            If so.HasError Then
                If so.Error.Message.StartsWith("Submit operation failed. Access to operation") Then
                    ErrorWindow.Show("Access Denied", "Insufficient User Role", so.Error.Message)
                Else
                    ErrorWindow.Show("Access Denied", so.Error.Message, "")
                End If

                so.MarkErrorAsHandled()
            Else
                NavigationService.Refresh()
            End If
        End Sub

        Private Async Sub btnNewCustomer_Click(sender As Object, e As RoutedEventArgs)
            Dim result = Await CustomerAddEditWindow.Show(New Web.Customers() With {
                .IsEditMode = True,
                .CountryCodes = CountryCodes,
                .CountryCode = CountryCodes.FirstOrDefault().CountryCodeID,
                .BirthDate = DateTime.Now.ToShortDateString()
            }, _customersContext)

            If result Then
                NavigationService.Refresh()
            End If
        End Sub

        Private Async Sub btnShowOrder_Click(sender As Object, e As RoutedEventArgs)
            If SelectedOrder Is Nothing Then
                Return
            End If

            Await OrderAddEditWindow.Show(SelectedOrder, _orderContext)
        End Sub

        Private Async Sub btnNewOrder_Click(sender As Object, e As RoutedEventArgs)
            Dim result = Await OrderAddEditWindow.Show(New Web.Orders() With {
                .IsEditMode = True,
                .CustomersCombo = (Await _customersContext.LoadAsync(_customersContext.GetCustomersQuery())).Entities,
                .Statuses = (Await _orderStatusContext.LoadAsync(_orderStatusContext.GetOrderStatusQuery())).Entities,
                .CountryCodes = (Await _countryCodesContext.LoadAsync(_countryCodesContext.GetCountriesQuery())).Entities,
                .Shippers = (Await _shippersContext.LoadAsync(_shippersContext.GetShippersQuery())).Entities,
                .PaymentTypes = (Await _paymentTypesContext.LoadAsync(_paymentTypesContext.GetPaymentTypesQuery())).Entities
            }, _orderContext)

            If result Then
                NavigationService.Refresh()
            End If
        End Sub
    End Class
End Namespace