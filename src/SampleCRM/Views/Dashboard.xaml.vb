Imports SampleCRM.Web.SampleCRM.Web
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Navigation

Namespace Views
    Partial Public Class Dashboard
        Inherits BasePage

        Private _customersContext As CustomersContext = New CustomersContext()
        Private _productsContext As ProductsContext = New ProductsContext()
        Private _orderContext As OrderContext = New OrderContext()

        Public Const ROW_LIMIT As Integer = 5

        Private _customers As IEnumerable(Of Web.Customers)
        Public Property Customers As IEnumerable(Of Web.Customers)
            Get
                Return _customers
            End Get
            Set(value As IEnumerable(Of Web.Customers))
                If Not Object.Equals(_customers, value) Then
                    _customers = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _orders As IEnumerable(Of Web.Orders)
        Public Property Orders As IEnumerable(Of Web.Orders)
            Get
                Return _orders
            End Get
            Set(value As IEnumerable(Of Web.Orders))
                If Not Object.Equals(_orders, value) Then
                    _orders = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _products As IEnumerable(Of Web.Products)
        Public Property Products As IEnumerable(Of Web.Products)
            Get
                Return _products
            End Get
            Set(value As IEnumerable(Of Web.Products))
                If Not Object.Equals(_products, value) Then
                    _products = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

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
                grdDashboard.ColumnDefinitions.Clear()
                grdDashboard.ColumnDefinitions.Add(New ColumnDefinition())

                Grid.SetColumn(cntCustomers, 0)
                Grid.SetColumn(cntOrders, 0)
                Grid.SetColumn(cntProducts, 0)

                grdDashboard.RowDefinitions.Clear()
                For i As Integer = 0 To 2
                    grdDashboard.RowDefinitions.Add(New RowDefinition() With {
                        .Height = GridLength.Auto
                    })
                Next

                Grid.SetRow(cntCustomers, 0)
                Grid.SetRow(cntOrders, 1)
                Grid.SetRow(cntProducts, 2)
            Else
                grdDashboard.ColumnDefinitions.Clear()
                grdDashboard.ColumnDefinitions.Add(New ColumnDefinition())
                grdDashboard.ColumnDefinitions.Add(New ColumnDefinition() With {
                    .Width = New GridLength(20, GridUnitType.Pixel)
                })
                grdDashboard.ColumnDefinitions.Add(New ColumnDefinition())

                Grid.SetColumn(cntCustomers, 0)
                Grid.SetColumn(cntOrders, 0)
                Grid.SetColumn(cntProducts, 2)

                grdDashboard.RowDefinitions.Clear()
                grdDashboard.RowDefinitions.Add(New RowDefinition() With {
                    .MinHeight = 300
                })
                grdDashboard.RowDefinitions.Add(New RowDefinition() With {
                    .MinHeight = 400
                })
                grdDashboard.RowDefinitions.Add(New RowDefinition() With {
                    .Height = GridLength.Auto
                })

                Grid.SetRow(cntCustomers, 0)
                Grid.SetRow(cntOrders, 1)
                Grid.SetRow(cntProducts, 1)
            End If
        End Sub

        Private Async Sub LoadElements()
            Await LoadCustomers()
            Await LoadOrders()
            Await LoadProducts()
        End Sub

        Private Async Function LoadCustomers() As Task
            Dim q = _customersContext.GetLatestCustomersQuery(ROW_LIMIT)
            Dim o = Await _customersContext.LoadAsync(q)
            Customers = o.Entities
        End Function

        Private Async Function LoadOrders() As Task
            Dim q = _orderContext.GetLatestOrdersQuery(ROW_LIMIT)
            Dim o = Await _orderContext.LoadAsync(q)
            Orders = o.Entities
        End Function

        Private Async Function LoadProducts() As Task
            Dim q = _productsContext.GetTopSaleProductsQuery(ROW_LIMIT)
            Dim o = Await _productsContext.LoadAsync(q)
            Products = o.Entities
        End Function
    End Class
End Namespace
