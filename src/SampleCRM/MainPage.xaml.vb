Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Navigation
Imports SampleCRM.Web
Imports SampleCRM.Web.SampleCRM.Web

Namespace Views

    Partial Public Class MainPage
        Inherits BaseUserControl

        Private _countContext As New CountContext()

        Private _categoriesCount As Integer
        Public Property CategoriesCount As Integer
            Get
                Return _categoriesCount
            End Get
            Set(value As Integer)
                If _categoriesCount <> value Then
                    _categoriesCount = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _customersCount As Integer
        Public Property CustomersCount As Integer
            Get
                Return _customersCount
            End Get
            Set(value As Integer)
                If _customersCount <> value Then
                    _customersCount = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _ordersCount As Integer
        Public Property OrdersCount As Integer
            Get
                Return _ordersCount
            End Get
            Set(value As Integer)
                If _ordersCount <> value Then
                    _ordersCount = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _productsCount As Integer
        Public Property ProductsCount As Integer
            Get
                Return _productsCount
            End Get
            Set(value As Integer)
                If _productsCount <> value Then
                    _productsCount = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
            DataContext = Me
            LoadCounts()
        End Sub

        Private Async Sub LoadCounts()
            Dim query = _countContext.GetAllCountsQuery()
            Dim op = Await _countContext.LoadAsync(query)
            Dim counts = op.Entities.FirstOrDefault()
            CategoriesCount = counts.CategoryCount
            CustomersCount = counts.CustomerCount
            OrdersCount = counts.OrderCount
            ProductsCount = counts.ProductCount
        End Sub

        Private Sub ContentFrame_Navigated(sender As Object, e As NavigationEventArgs)
            HighlightLinks(e, LinksStackPanel.Children)
            HighlightLinks(e, MobileLinksStackPanel.Children)
        End Sub

        Private Sub HighlightLinks(e As NavigationEventArgs, links As UIElementCollection)
            For Each child As UIElement In links
                Dim hb As HyperlinkButton = TryCast(child, HyperlinkButton)
                If hb IsNot Nothing AndAlso hb.NavigateUri IsNot Nothing Then
                    If hb.NavigateUri.ToString().Equals(e.Uri.ToString()) Then
                        VisualStateManager.GoToState(hb, "ActiveLink", True)
                    Else
                        VisualStateManager.GoToState(hb, "InactiveLink", True)
                    End If
                End If
            Next
        End Sub

        Private Sub ContentFrame_NavigationFailed(sender As Object, e As NavigationFailedEventArgs)
            e.Handled = True
            ErrorWindow.Show(e.Uri)
        End Sub
    End Class
End Namespace