using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SampleCRM.Web.Views
{
    public partial class Orders : BasePage
    {
        #region Contexts
        private OrderContext _orderContext => ordersDataSource.DomainContext as OrderContext;
        private OrderItemsContext _orderItemsContext => orderItemsDataSource.DomainContext as OrderItemsContext;

        private CountryCodesContext _countryCodesContext = new CountryCodesContext();
        private CustomersContext _customersContext = new CustomersContext();
        private OrderStatusContext _orderStatusContext = new OrderStatusContext();
        private ProductsContext _productsContext = new ProductsContext();
        private TaxTypesContext _taxTypesContext = new TaxTypesContext();
        private ShippersContext _shippersContext = new ShippersContext();
        private PaymentTypeContext _paymentTypesContext = new PaymentTypeContext();
        #endregion

        private bool _orderItemsTabSelected;

        #region DataContext Properties

        public IEnumerable<Models.CountryCode> CountryCodes
        {
            get { return (IEnumerable<Models.CountryCode>)GetValue(CountryCodesProperty); }
            set { SetValue(CountryCodesProperty, value); }
        }
        public static readonly DependencyProperty CountryCodesProperty =
            DependencyProperty.Register("CountryCodes", typeof(IEnumerable<Models.CountryCode>), typeof(Orders), new PropertyMetadata(null));

        public IEnumerable<Models.Shipper> Shippers
        {
            get { return (IEnumerable<Models.Shipper>)GetValue(ShippersProperty); }
            set { SetValue(ShippersProperty, value); }
        }
        public static readonly DependencyProperty ShippersProperty =
            DependencyProperty.Register("Shippers", typeof(IEnumerable<Models.Shipper>), typeof(Orders), new PropertyMetadata(null));

        public IEnumerable<Models.PaymentType> PaymentTypes
        {
            get { return (IEnumerable<Models.PaymentType>)GetValue(PaymentTypesProperty); }
            set { SetValue(PaymentTypesProperty, value); }
        }
        public static readonly DependencyProperty PaymentTypesProperty =
            DependencyProperty.Register("PaymentTypes", typeof(IEnumerable<Models.PaymentType>), typeof(Orders), new PropertyMetadata(null));

        public IEnumerable<Models.OrderStatu> Statuses
        {
            get { return (IEnumerable<Models.OrderStatu>)GetValue(StatusesProperty); }
            set { SetValue(StatusesProperty, value); }
        }
        public static readonly DependencyProperty StatusesProperty =
            DependencyProperty.Register("Statuses", typeof(IEnumerable<Models.OrderStatu>), typeof(Orders), new PropertyMetadata(null));

        public Models.Order SelectedOrder
        {
            get { return (Models.Order)GetValue(SelectedOrderProperty); }
            set { SetValue(SelectedOrderProperty, value); }
        }
        public static readonly DependencyProperty SelectedOrderProperty =
            DependencyProperty.Register("SelectedOrder", typeof(Models.Order), typeof(Orders),
                new PropertyMetadata(
                    new PropertyChangedCallback(async (s, t) =>
                    {
                        var value = t.NewValue as Models.Order;
                        var page = (Orders)s;

                        page.AnySelectedOrder = value != null;

                        if (value != null)
                        {
                            await AsyncHelper.RunAsync(async () => await page.LoadCustomer(value));
                            if (page._orderItemsTabSelected)
                            {
                                page.orderItemsDataSource.Clear();
                                var orderId = value.OrderID;
                                var orderParam = page.orderItemsDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "orderId");
                                orderParam.Value = orderId;
                                page.orderItemsDataSource.Load();
                            }
#if DEBUG
                            Console.WriteLine($"Orders, Order: {value.OrderID} selected");
#endif
                        }

                    })));

        public bool AnySelectedOrder
        {
            get { return (bool)GetValue(AnySelectedOrderProperty); }
            set { SetValue(AnySelectedOrderProperty, value); }
        }
        private static readonly DependencyProperty AnySelectedOrderProperty =
            DependencyProperty.Register("AnySelectedOrder", typeof(bool), typeof(Orders),
                new PropertyMetadata(false));


        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(Orders),
                new PropertyMetadata(
                    new PropertyChangedCallback((s, t) =>
                    {
                        var value = t.NewValue as string;
                        var page = s as Orders;
                        var searchParam = page.ordersDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "search");
                        searchParam.Value = value;
                        page.ordersDataSource.Load();
                    })));


        public Models.OrderItem SelectedOrderItem
        {
            get { return (Models.OrderItem)GetValue(SelectedOrderItemProperty); }
            set { SetValue(SelectedOrderItemProperty, value); }
        }
        public static readonly DependencyProperty SelectedOrderItemProperty =
            DependencyProperty.Register("SelectedOrderItem", typeof(Models.OrderItem), typeof(Customers),
                new PropertyMetadata(
                    new PropertyChangedCallback((s, t) =>
                    {
                        var value = t.NewValue as Models.OrderItem;
                        //var page = (Customers)s;
                        if (value != null)
                        {
#if DEBUG
                            Console.WriteLine($"OrderItems, OrderItem: {value.OrderID}, {value.OrderLine} selected");
#endif
                        }
                    })));

        public string SearchOrderItemText
        {
            get { return (string)GetValue(SearchOrderItemTextProperty); }
            set { SetValue(SearchOrderItemTextProperty, value); }
        }
        public static readonly DependencyProperty SearchOrderItemTextProperty =
            DependencyProperty.Register("SearchOrderItemText", typeof(string), typeof(Orders),
                new PropertyMetadata(
                    new PropertyChangedCallback((s, t) =>
                    {
                        var value = t.NewValue as string;
                        var page = s as Orders;
                        page.orderItemsDataSource.Clear();
                        var searchParam = page.orderItemsDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "search");
                        searchParam.Value = value;
                        page.orderItemsDataSource.Load();
                    })));

        #endregion

        public Orders()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await AsyncHelper.RunAsync(LoadPaymentTypes);
            await AsyncHelper.RunAsync(LoadShippers);
            await AsyncHelper.RunAsync(LoadCountryCodes);
            await AsyncHelper.RunAsync(LoadStatuses);
            ordersDataSource.Load();
        }

        protected override void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            base.OnSizeChanged(sender, e);

            if (IsMobileUI)
            {
                grdHead.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);

                Grid.SetColumn(grdSearch, 0);
                Grid.SetRow(grdSearch, 2);
                grdSearch.Margin = new Thickness(0, 0, 0, 10);

                Grid.SetRow(orderCard, 0);
                Grid.SetColumn(orderCard, 0);

                Grid.SetColumn(grdOrderDetails, 0);
                Grid.SetRow(grdOrderDetails, 1);

                grdTbOrder.RowDefinitions[0].Height = GridLength.Auto;
                grdTbOrder.RowDefinitions[1].Height = GridLength.Auto;

                grdTbOrder.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
                grdTbOrder.ColumnDefinitions[1].Width = GridLength.Auto;

                formOrder.EditTemplate = Resources["dtNarrowOrders"] as DataTemplate;


                grdTbOrderItems.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);

                Grid.SetColumn(grdOrderItemSearch, 0);
                Grid.SetRow(grdOrderItemSearch, 2);
                grdOrderItemSearch.Margin = new Thickness(0, 0, 0, 10);
            }
            else
            {
                grdHead.ColumnDefinitions[2].Width = new GridLength(405, GridUnitType.Pixel);

                Grid.SetColumn(grdSearch, 2);
                Grid.SetRow(grdSearch, 0);
                grdSearch.Margin = new Thickness();

                Grid.SetRow(orderCard, 0);
                Grid.SetColumn(orderCard, 0);

                Grid.SetRow(grdOrderDetails, 0);
                Grid.SetColumn(grdOrderDetails, 1);

                grdTbOrder.RowDefinitions[0].Height = new GridLength(2, GridUnitType.Star);
                grdTbOrder.RowDefinitions[1].Height = GridLength.Auto;

                grdTbOrder.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
                grdTbOrder.ColumnDefinitions[1].Width = new GridLength(4, GridUnitType.Star);

                formOrder.EditTemplate = Resources["dtWideOrders"] as DataTemplate;


                grdTbOrderItems.ColumnDefinitions[2].Width = new GridLength(405, GridUnitType.Pixel);

                Grid.SetColumn(grdOrderItemSearch, 2);
                Grid.SetRow(grdOrderItemSearch, 0);
                grdOrderItemSearch.Margin = new Thickness();
            }
        }

        private void ordersDataSource_LoadedData(object sender, OpenRiaServices.Controls.LoadedDataEventArgs e)
        {
            if (e.HasError)
                return;

            var orders = e.Entities.Cast<Models.Order>();
            foreach (var order in orders)
            {
                order.CountryCodes = CountryCodes;
                order.Shippers = Shippers;
                order.PaymentTypes = PaymentTypes;
                order.Statuses = Statuses;
            }
        }

        private async Task LoadShippers() => Shippers = (await _shippersContext.LoadAsync(_shippersContext.GetShippersQuery())).Entities;
        private async Task LoadPaymentTypes() => PaymentTypes = (await _paymentTypesContext.LoadAsync(_paymentTypesContext.GetPaymentTypesQuery())).Entities;
        private async Task LoadCountryCodes() => CountryCodes = (await _countryCodesContext.LoadAsync(_countryCodesContext.GetCountriesQuery())).Entities;
        private async Task LoadStatuses() => Statuses = (await _orderStatusContext.LoadAsync(_orderStatusContext.GetOrderStatusQuery())).Entities;

        private async Task LoadCustomer(Models.Order order)
        {
            if (order == null)
                return;

            if (order.Customer == null || order.Customer.CustomerID != order.CustomerID)
                order.Customer = (await _customersContext.LoadAsync(_customersContext.GetCustomerByIdQuery(SelectedOrder.CustomerID))).Entities.FirstOrDefault();
        }
        private async Task LoadProduct(Models.OrderItem orderItem)
        {
            if (orderItem == null)
                return;

            if (orderItem.Product == null)
            {
                orderItem.Product = (await _productsContext.LoadAsync(_productsContext.GetProductByIdQuery(orderItem.ProductID))).Entities.FirstOrDefault();
            }
        }
        private async Task LoadTaxRate(Models.OrderItem orderItem)
        {
            if (orderItem == null)
                return;

            if (orderItem.TaxRate == 0)
            {
                var query = _taxTypesContext.GetTaxTypeByIdQuery(orderItem.TaxType);
                var op = await _taxTypesContext.LoadAsync(query);
                var taxRate = Convert.ToDecimal(op.Entities.FirstOrDefault()?.Rate, CultureInfo.InvariantCulture);
                orderItem.TaxRate = taxRate;
            }
        }

        private void btnSearchCancel_Click(object sender, RoutedEventArgs e)
        {
            SearchText = string.Empty;
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void grdOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Models.Order)
            {
                SelectedOrder = e.AddedItems[0] as Models.Order;
            }
            else
            {
                SelectedOrder = null;
            }
#if DEBUG
            Console.WriteLine("grdOrders_SelectionChanged, {0} Items Added", e.AddedItems.Count);
#endif
        }

        private async void btnNew_Click(object sender, RoutedEventArgs e)
        {
            await AsyncHelper.RunAsync(ArrangeOrderAddEditWindow);
        }
        private async Task ArrangeOrderAddEditWindow()
        {
            var newOrder = new Models.Order
            {
                IsEditMode = true
            };

            UpdateComboDataForOrder(newOrder);
            var result = await OrderAddEditWindow.Show(newOrder, _orderContext);
            if (result)
            {
                NavigationService.Refresh();
            }
        }

        private void tcDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _orderItemsTabSelected = e.AddedItems.Count > 0 && e.AddedItems.Contains(tbOrderItems);
            if (_orderItemsTabSelected && SelectedOrder != null)
            {
                orderItemsDataSource.Clear();
                var orderId = SelectedOrder.OrderID;
                var orderParam = orderItemsDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "orderId");
                orderParam.Value = orderId;
                orderItemsDataSource.Load();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete the order and all items belong that order?", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
                return;

            if (_orderContext.Orders.CanRemove)
            {
                _orderContext.Orders.Remove((Entity)SelectedOrder);
                _orderContext.SubmitChanges(OnDeleteSubmitCompleted, null);
            }
            else
            {
                throw new AccessViolationException("RIA Service Delete Entity for Customer Context is denied");
            }
        }

        private void OnDeleteSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                if (so.Error.Message.StartsWith("Submit operation failed. Access to operation"))
                {
                    ErrorWindow.Show("Access Denied", "Insuficient User Role", so.Error.Message);
                }
                else
                {
                    ErrorWindow.Show("Access Denied", so.Error.Message, "");
                }
                so.MarkErrorAsHandled();
            }
            else
            {
                NavigationService.Refresh();
            }
        }

        private void formOrder_EditEnded(object sender, DataFormEditEndedEventArgs e)
        {
            if (e.EditAction == DataFormEditAction.Commit)
            {
                _orderContext.SubmitChanges(OnFormOrderSubmitCompleted, null);
            }
            else if (e.EditAction == DataFormEditAction.Cancel)
            {
                _orderContext.RejectChanges();
            }
        }

        private void OnFormOrderSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                if (so.Error.Message.StartsWith("Submit operation failed. Access to operation"))
                {
                    ErrorWindow.Show("Access Denied", "Insuficient User Role", so.Error.Message);
                }
                else
                {
                    ErrorWindow.Show("Access Denied", so.Error.Message, "");
                }
                so.MarkErrorAsHandled();
            }
            else
            {
                UpdateComboDataForOrder(SelectedOrder);
            }
        }

        private void UpdateComboDataForOrder(Models.Order order)
        {
            order.CountryCodes = CountryCodes;
            order.Shippers = Shippers;
            order.PaymentTypes = PaymentTypes;
            order.Statuses = Statuses;
        }

        private void btnOrderItemSearchCancel_Click(object sender, RoutedEventArgs e)
        {
            SearchOrderItemText = string.Empty;
        }

        private void grdOrderItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Models.OrderItem)
            {
                SelectedOrderItem = e.AddedItems[0] as Models.OrderItem;
            }
            else
            {
                SelectedOrderItem = null;
            }
#if DEBUG
            Console.WriteLine("grdOrderItems_SelectionChanged, {0} Items Added", e.AddedItems.Count);
#endif
        }

        private async void btnShowOrderItem_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOrderItem == null)
                throw (new ArgumentNullException("Selected Order Item can't be null"));


            if (SelectedOrderItem.TaxTypes == null)
                SelectedOrderItem.TaxTypes = (await _taxTypesContext.LoadAsync(_taxTypesContext.GetTaxTypesQuery())).Entities;

            var result = await OrderItemAddEditWindow.Show(SelectedOrderItem, _orderItemsContext);
            if (result)
                orderItemsDataSource.Load();
            
        }
        private async void btnNewOrderItem_Click(object sender, RoutedEventArgs e)
        {
            await AsyncHelper.RunAsync(ArrangeOrderItemWindow);
        }
        private async Task ArrangeOrderItemWindow()
        {
            if (SelectedOrder == null)
                throw (new ArgumentNullException("Selected Order can't be null"));

            var newOrderItem = new Models.OrderItem
            {
                IsEditMode = true,
                OrderID = SelectedOrder.OrderID,
                OrderLine = 0,
                TaxTypes = (await _taxTypesContext.LoadAsync(_taxTypesContext.GetTaxTypesQuery())).Entities
            };

            var result = await OrderItemAddEditWindow.Show(newOrderItem, _orderItemsContext);
            if (result)
            {
                orderItemsDataSource.Load();
            }
        }

        private void txtOrderItemSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnOrderItemSearch.Focus();
        }
        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSearch.Focus();
        }

        private async void orderItemsDataSource_LoadedData(object sender, OpenRiaServices.Controls.LoadedDataEventArgs e)
        {
            if (e.HasError)
                return;

            var orderItems = e.Entities.Cast<Models.OrderItem>();

            foreach (var item in orderItems)
            {
                await LoadProduct(item);
                await LoadTaxRate(item);
            }
        }
    }
}