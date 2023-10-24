Imports OpenRiaServices.DomainServices.Client
Imports SampleCRM.Web.SampleCRM.Web
Imports System.Globalization
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Navigation

Namespace Views
    Partial Public Class Orders
        Inherits BasePage

#Region "Contexts"
        Private _orderStatusContext As New OrderStatusContext()
        Private _countryCodesContext As New CountryCodesContext()
        Private _orderContext As New OrderContext()
        Private _orderItemsContext As New OrderItemsContext()
        Private _customersContext As New CustomersContext()
        Private _productsContext As New ProductsContext()
        Private _taxTypesContext As New TaxTypesContext()
        Private _shippersContext As New ShippersContext()
        Private _paymentTypesContext As New PaymentTypeContext()
#End Region

        Private _orderItemsTabSelected As Boolean

#Region "DataContext Properties"
        Private _ordersCollection As IEnumerable(Of Web.Orders)
        Public Property OrdersCollection As IEnumerable(Of Web.Orders)
            Get
                Return _ordersCollection
            End Get
            Set(value As IEnumerable(Of Web.Orders))
                If _ordersCollection IsNot value Then
                    _ordersCollection = value
                    OnPropertyChanged()
                    OnPropertyChanged("FilteredOrdersCollection")
                    SelectedOrder = FilteredOrdersCollection.FirstOrDefault()
                End If
            End Set
        End Property

        Public ReadOnly Property FilteredOrdersCollection As IEnumerable(Of Web.Orders)
            Get
                If String.IsNullOrWhiteSpace(_searchText) Then
                    Return _ordersCollection
                Else
                    Return _ordersCollection.Where(Function(x) x.OrderID.ToString().Contains(_searchText.ToLowerInvariant()))
                End If
            End Get
        End Property

        Private _countryCodes As IEnumerable(Of Web.CountryCodes)
        Public Property CountryCodes As IEnumerable(Of Web.CountryCodes)
            Get
                Return _countryCodes
            End Get
            Set(value As IEnumerable(Of Web.CountryCodes))
                If _countryCodes IsNot value Then
                    _countryCodes = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _statuses As IEnumerable(Of Web.OrderStatus)
        Public Property Statuses As IEnumerable(Of Web.OrderStatus)
            Get
                Return _statuses
            End Get
            Set(value As IEnumerable(Of Web.OrderStatus))
                If _statuses IsNot value Then
                    _statuses = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _selectedOrder As New Web.Orders()
        Public Property SelectedOrder As Web.Orders
            Get
                Return _selectedOrder
            End Get
            Set(value As Web.Orders)
                If _selectedOrder IsNot value Then
                    _selectedOrder = value
                    OnPropertyChanged()
                    OnPropertyChanged("AnySelectedOrder")
                    LoadCustomer()
                    LoadOrderItemsOfOrder()
#If DEBUG Then
                    Console.WriteLine($"Orders, Order: {value.OrderID} selected")
#End If
                End If
            End Set
        End Property

        Public ReadOnly Property AnySelectedOrder As Boolean
            Get
                Return _selectedOrder IsNot Nothing
            End Get
        End Property

        Private _searchText As String
        Public Property SearchText As String
            Get
                Return _searchText
            End Get
            Set(value As String)
                If _searchText IsNot value Then
                    _searchText = value
                    OnPropertyChanged()
                    OnPropertyChanged("FilteredOrdersCollection")
                    SelectedOrder = FilteredOrdersCollection.FirstOrDefault()
                End If
            End Set
        End Property

        Private _orderItemsCollection As IEnumerable(Of Web.OrderItems)
        Public Property OrderItemsCollection As IEnumerable(Of Web.OrderItems)
            Get
                Return _orderItemsCollection
            End Get
            Set(value As IEnumerable(Of Web.OrderItems))
                If _orderItemsCollection IsNot value Then
                    _orderItemsCollection = value
                    OnPropertyChanged()
                    OnPropertyChanged("FilteredOrderItemsCollection")
                    SelectedOrderItem = FilteredOrderItemsCollection.FirstOrDefault()
                End If
            End Set
        End Property

        Public ReadOnly Property FilteredOrderItemsCollection As IEnumerable(Of Web.OrderItems)
            Get
                If String.IsNullOrWhiteSpace(_searchOrderItemText) Then
                    Return _orderItemsCollection
                Else
                    Return _orderItemsCollection.Where(Function(x) x.OrderID.ToString().Contains(_searchOrderItemText))
                End If
            End Get
        End Property

        Private _selectedOrderItem As Web.OrderItems
        Public Property SelectedOrderItem As Web.OrderItems
            Get
                Return _selectedOrderItem
            End Get
            Set(value As Web.OrderItems)
                If _selectedOrderItem IsNot value Then
                    _selectedOrderItem = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _searchOrderItemText As String
        Public Property SearchOrderItemText As String
            Get
                Return _searchOrderItemText
            End Get
            Set(value As String)
                If _searchOrderItemText IsNot value Then
                    _searchOrderItemText = value
                    OnPropertyChanged()
                    OnPropertyChanged("FilteredOrderItemsCollection")
                    SelectedOrderItem = FilteredOrderItemsCollection.FirstOrDefault()
                End If
            End Set
        End Property
#End Region

        Public Sub New()
            InitializeComponent()
            DataContext = Me
        End Sub

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            LoadElements()
        End Sub

        Protected Overrides Sub OnSizeChanged(sender As Object, e As SizeChangedEventArgs)
            MyBase.OnSizeChanged(sender, e)

            If IsMobileUI Then
                grdHead.ColumnDefinitions(2).Width = New GridLength(1, GridUnitType.Star)

                Grid.SetColumn(grdSearch, 0)
                Grid.SetRow(grdSearch, 2)
                grdSearch.Margin = New Thickness(0, 0, 0, 10)

                Grid.SetRow(orderCard, 0)
                Grid.SetColumn(orderCard, 0)

                Grid.SetColumn(grdOrderDetails, 0)
                Grid.SetRow(grdOrderDetails, 1)

                grdTbOrder.RowDefinitions(0).Height = GridLength.Auto
                grdTbOrder.RowDefinitions(1).Height = GridLength.Auto

                grdTbOrder.ColumnDefinitions(0).Width = New GridLength(2, GridUnitType.Star)
                grdTbOrder.ColumnDefinitions(1).Width = GridLength.Auto

                formOrder.EditTemplate = TryCast(Resources("dtNarrowOrders"), DataTemplate)

                grdTbOrderItems.ColumnDefinitions(2).Width = New GridLength(1, GridUnitType.Star)

                Grid.SetColumn(grdOrderItemSearch, 0)
                Grid.SetRow(grdOrderItemSearch, 2)
                grdOrderItemSearch.Margin = New Thickness(0, 0, 0, 10)
            Else
                grdHead.ColumnDefinitions(2).Width = New GridLength(405, GridUnitType.Pixel)

                Grid.SetColumn(grdSearch, 2)
                Grid.SetRow(grdSearch, 0)
                grdSearch.Margin = New Thickness()

                Grid.SetRow(orderCard, 0)
                Grid.SetColumn(orderCard, 0)

                Grid.SetRow(grdOrderDetails, 0)
                Grid.SetColumn(grdOrderDetails, 1)

                grdTbOrder.RowDefinitions(0).Height = New GridLength(2, GridUnitType.Star)
                grdTbOrder.RowDefinitions(1).Height = GridLength.Auto

                grdTbOrder.ColumnDefinitions(0).Width = New GridLength(2, GridUnitType.Star)
                grdTbOrder.ColumnDefinitions(1).Width = New GridLength(4, GridUnitType.Star)

                formOrder.EditTemplate = TryCast(Resources("dtWideOrders"), DataTemplate)

                grdTbOrderItems.ColumnDefinitions(2).Width = New GridLength(405, GridUnitType.Pixel)

                Grid.SetColumn(grdOrderItemSearch, 2)
                Grid.SetRow(grdOrderItemSearch, 0)
                grdOrderItemSearch.Margin = New Thickness()
            End If
        End Sub

        Private Async Sub LoadElements()
            Dim query = _orderContext.GetOrdersQuery()
            Dim op = Await _orderContext.LoadAsync(query)
            OrdersCollection = op.Entities

            Await LoadCountryCodes()
            Await LoadStatuses()
#If DEBUG Then
            Console.WriteLine("Orders Collection:" & OrdersCollection.Count())
            For Each item In OrdersCollection
                Console.WriteLine("Order Id:" & item.OrderID)
                Console.WriteLine("Customer Id:" & item.CustomerID)
            Next
#End If
        End Sub

        Private Async Function LoadCountryCodes() As Task
            Dim query = _countryCodesContext.GetCountriesQuery()
            Dim op = Await _countryCodesContext.LoadAsync(query)
            CountryCodes = op.Entities

            For Each o In OrdersCollection
                o.CountryCodes = CountryCodes
                o.ShipCountryName = CountryCodes.SingleOrDefault(Function(x) x.CountryCodeID = o.ShipCountryCode).Name
            Next
        End Function

        Private Async Function LoadStatuses() As Task
            Dim query = _orderStatusContext.GetOrderStatusQuery()
            Dim op = Await _orderStatusContext.LoadAsync(query)
            Statuses = op.Entities

            For Each o In OrdersCollection
                o.Statuses = Statuses
                o.StatusDesc = Statuses.FirstOrDefault(Function(x) x.Status = o.Status)?.Name
            Next
        End Function

        Private Async Sub LoadCustomer()
            If SelectedOrder Is Nothing Then
                Return
            End If

            If SelectedOrder.CustomersCombo IsNot Nothing AndAlso SelectedOrder.CustomersCombo.Any() Then
                Return
            End If

            If SelectedOrder.Customer Is Nothing OrElse SelectedOrder.Customer.CustomerID <> SelectedOrder.CustomerID Then
                Dim query = _customersContext.GetCustomersQuery().Where(Function(x) x.CustomerID = SelectedOrder.CustomerID)
                Dim op = Await _customersContext.LoadAsync(query)
                Dim customer = op.Entities.FirstOrDefault()
                SelectedOrder.Customer = customer
            End If
        End Sub

        Private Async Sub LoadProduct(orderItem As Web.OrderItems)
            If orderItem Is Nothing Then
                Return
            End If

            If orderItem.Product Is Nothing Then
                Dim query = _productsContext.GetProductsQuery().Where(Function(x) x.ProductID = orderItem.ProductID)
                Dim op = Await _productsContext.LoadAsync(query)
                Dim product = op.Entities.FirstOrDefault()
                orderItem.Product = product
            End If
        End Sub

        Private Async Sub LoadTaxRate(orderItem As Web.OrderItems)
            If orderItem Is Nothing Then
                Return
            End If

            If orderItem.TaxRate = 0 Then
                Dim query = _taxTypesContext.GetTaxTypesQuery().Where(Function(x) x.TaxTypeID = orderItem.TaxType)
                Dim op = Await _taxTypesContext.LoadAsync(query)
                Dim taxRate = Convert.ToDecimal(op.Entities.FirstOrDefault()?.Rate, CultureInfo.InvariantCulture)
                orderItem.TaxRate = taxRate
            End If
        End Sub

        Private Async Sub LoadOrderItemsOfOrder()
            If SelectedOrder Is Nothing Then
                Return
            End If

            If Not _orderItemsTabSelected Then
                Return
            End If

            Dim orderId = SelectedOrder.OrderID
            Dim query = _orderItemsContext.GetOrderItemsOfOrderQuery(SelectedOrder.OrderID)
            Dim op = Await _orderItemsContext.LoadAsync(query)
            OrderItemsCollection = op.Entities

            For Each orderItem In OrderItemsCollection
                LoadProduct(orderItem)
                LoadTaxRate(orderItem)
            Next
#If DEBUG Then
            Console.WriteLine("Order Items Collection:" & OrderItemsCollection.Count())
            For Each item In OrderItemsCollection
                Console.WriteLine("Order Item Id:" & item.OrderLine)
            Next
#End If
        End Sub

        Private Sub btnSearchCancel_Click(sender As Object, e As RoutedEventArgs)
            SearchText = String.Empty
        End Sub

        Private Sub btnSearch_Click(sender As Object, e As RoutedEventArgs)

        End Sub

        Private Sub grdOrders_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
#If DEBUG Then
            Console.WriteLine("grdOrders_SelectionChanged, {0} Items Added", e.AddedItems.Count)
#End If
        End Sub

        Private Async Sub btnNew_Click(sender As Object, e As RoutedEventArgs)
            Dim result = Await OrderAddEditWindow.Show(New Web.Orders With {
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

        Private Sub tcDetails_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
            Dim anySelected = e.AddedItems.Count > 0
            If anySelected Then
                _orderItemsTabSelected = e.AddedItems.Contains(tbOrderItems)
                If _orderItemsTabSelected Then
                    LoadOrderItemsOfOrder()
                End If
            Else
                _orderItemsTabSelected = False
            End If
        End Sub

        Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs)
            Dim result = MessageBox.Show("Are you sure to delete the order and all items belong to that order?", MessageBoxButton.OKCancel)
            If result <> MessageBoxResult.OK Then
                Return
            End If

            If _orderContext.Orders.CanRemove Then
                _orderContext.Orders.Remove(SelectedOrder)
                _orderContext.SubmitChanges(AddressOf OnDeleteSubmitCompleted, Nothing)
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

        Private Sub formOrder_EditEnded(sender As Object, e As DataFormEditEndedEventArgs)
            If e.EditAction = DataFormEditAction.Commit Then
                _customersContext.SubmitChanges(AddressOf OnFormOrderSubmitCompleted, Nothing)
            ElseIf e.EditAction = DataFormEditAction.Cancel Then
                SelectedOrder.IsEditMode = False
            End If
        End Sub

        Private Sub OnFormOrderSubmitCompleted(so As SubmitOperation)
            If so.HasError Then
                If so.Error.Message.StartsWith("Submit operation failed. Access to operation") Then
                    ErrorWindow.Show("Access Denied", "Insufficient User Role", so.Error.Message)
                Else
                    ErrorWindow.Show("Access Denied", so.Error.Message, "")
                End If
                so.MarkErrorAsHandled()
            Else
                OnPropertyChanged("SelectedOrder")
                OnPropertyChanged("OrdersCollection")
                OnPropertyChanged("FilteredOrdersCollection")
            End If
        End Sub

        Private Sub btnOrderItemSearchCancel_Click(sender As Object, e As RoutedEventArgs)
            SearchOrderItemText = String.Empty
        End Sub

        Private Sub grdOrderItems_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
#If DEBUG Then
            Console.WriteLine("grdOrderItems_SelectionChanged, {0} Items Added", e.AddedItems.Count)
#End If
        End Sub

        Private Async Sub btnShowOrderItem_Click(sender As Object, e As RoutedEventArgs)
            If SelectedOrderItem Is Nothing Then
                Throw New ArgumentNullException("Selected Order Item can't be null")
            End If

            If SelectedOrderItem.ProductsCombo Is Nothing Then
                SelectedOrderItem.ProductsCombo = (Await _productsContext.LoadAsync(_productsContext.GetProductsQuery())).Entities
            End If

            If SelectedOrderItem.TaxTypes Is Nothing Then
                SelectedOrderItem.TaxTypes = (Await _taxTypesContext.LoadAsync(_taxTypesContext.GetTaxTypesQuery())).Entities
            End If

            Dim result = Await OrderItemAddEditWindow.Show(SelectedOrderItem, _orderItemsContext)
            If result Then
                NavigationService.Refresh()
            End If
        End Sub

        Private Async Sub btnNewOrderItem_Click(sender As Object, e As RoutedEventArgs)
            If SelectedOrder Is Nothing Then
                Throw New ArgumentNullException("Selected Order can't be null")
            End If

            Dim result = Await OrderItemAddEditWindow.Show(New Web.OrderItems With {
                .IsEditMode = True,
                .OrderID = SelectedOrder.OrderID,
                .OrderLine = 0,
                .ProductsCombo = (Await _productsContext.LoadAsync(_productsContext.GetProductsQuery())).Entities,
                .TaxTypes = (Await _taxTypesContext.LoadAsync(_taxTypesContext.GetTaxTypesQuery())).Entities
            }, _orderItemsContext)

            If result Then
                NavigationService.Refresh()
            End If
        End Sub
    End Class
End Namespace
