using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SampleCRM.Web.Views
{
    public partial class Customers : BasePage
    {
        #region Contexts
        private CustomersContext _customersContext => customersDataSource.DomainContext as CustomersContext;
        private OrderContext _orderContext => ordersDataSource.DomainContext as OrderContext;

        private CountryCodesContext _countryCodesContext = new CountryCodesContext();
        private OrderStatusContext _orderStatusContext = new OrderStatusContext();
        private ShippersContext _shippersContext = new ShippersContext();
        private PaymentTypeContext _paymentTypesContext = new PaymentTypeContext();
        #endregion

        private bool _ordersTabSelected;

        #region DataContext Properties
        public IEnumerable<Models.CountryCodes> CountryCodes
        {
            get { return (IEnumerable<Models.CountryCodes>)GetValue(CountryCodesProperty); }
            set { SetValue(CountryCodesProperty, value); }
        }
        public static readonly DependencyProperty CountryCodesProperty =
            DependencyProperty.Register("CountryCodes", typeof(IEnumerable<Models.CountryCodes>), typeof(Customers), new PropertyMetadata(null));

        public Models.Customers SelectedCustomer
        {
            get { return (Models.Customers)GetValue(SelectedCustomerProperty); }
            set { SetValue(SelectedCustomerProperty, value); }
        }
        public static readonly DependencyProperty SelectedCustomerProperty =
            DependencyProperty.Register("SelectedCustomer", typeof(Models.Customers), typeof(Customers),
                new PropertyMetadata(
                    new PropertyChangedCallback((s, t) =>
                    {
                        var value = t.NewValue as Models.Customers;
                        var page = (Customers)s;

                        page.AnySelectedCustomer = value != null;

                        if (value != null)
                        {
                            if (page._ordersTabSelected)
                            {
                                var customerId = value.CustomerID;
                                var customerParam = page.ordersDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "customerId");
                                customerParam.Value = customerId;
                                page.ordersDataSource.Load();
                            }
#if DEBUG
                            Console.WriteLine($"Customers, Customer: {value.FullName} selected");
#endif
                        }

                    })));

        public bool AnySelectedCustomer
        {
            get { return (bool)GetValue(AnySelectedCustomerProperty); }
            set { SetValue(AnySelectedCustomerProperty, value); }
        }
        private static readonly DependencyProperty AnySelectedCustomerProperty =
            DependencyProperty.Register("AnySelectedCustomer", typeof(bool), typeof(Customers),
                new PropertyMetadata(false));

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(Customers),
                new PropertyMetadata(
                    new PropertyChangedCallback((s, t) =>
                    {
                        var value = t.NewValue as string;
                        var page = s as Customers;
                        var searchParam = page.customersDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "search");
                        searchParam.Value = value;
                        page.customersDataSource.Load();
                    })));


        public Models.Orders SelectedOrder
        {
            get { return (Models.Orders)GetValue(SelectedOrderProperty); }
            set { SetValue(SelectedOrderProperty, value); }
        }
        public static readonly DependencyProperty SelectedOrderProperty =
            DependencyProperty.Register("SelectedOrder", typeof(Models.Orders), typeof(Customers),
                new PropertyMetadata(
                    new PropertyChangedCallback((s, t) =>
                    {
                        var value = t.NewValue as Models.Orders;
                        //var page = (Customers)s;
                        if (value != null)
                        {
#if DEBUG
                            Console.WriteLine($"Orders, Order: {value.OrderID} selected");
#endif
                        }
                    })));

        public string SearchOrderText
        {
            get { return (string)GetValue(SearchOrderTextProperty); }
            set { SetValue(SearchOrderTextProperty, value); }
        }
        public static readonly DependencyProperty SearchOrderTextProperty =
            DependencyProperty.Register("SearchOrderText", typeof(string), typeof(Customers),
                new PropertyMetadata(
                    new PropertyChangedCallback((s, t) =>
                    {
                        var value = t.NewValue as string;
                        var page = s as Customers;
                        var searchParam = page.ordersDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "search");
                        searchParam.Value = value;
                        page.ordersDataSource.Load();
                    })));

        #endregion

        public Customers()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void customersDataSource_LoadedData(object sender, OpenRiaServices.Controls.LoadedDataEventArgs e)
        {
            var customers = e.Entities.Cast<Models.Customers>();
            foreach (var customer in customers)
                customer.CountryCodes = CountryCodes;
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

                Grid.SetRow(customerCard, 0);
                Grid.SetColumn(customerCard, 0);

                Grid.SetColumn(grdCustomerDetails, 0);
                Grid.SetRow(grdCustomerDetails, 1);

                grdTbCustomer.RowDefinitions[0].Height = GridLength.Auto;
                grdTbCustomer.RowDefinitions[1].Height = GridLength.Auto;

                grdTbCustomer.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
                grdTbCustomer.ColumnDefinitions[1].Width = GridLength.Auto;

                formCustomer.EditTemplate = Resources["dtNarrowCustomers"] as DataTemplate;


                grdTbOrders.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);

                Grid.SetColumn(grdOrderSearch, 0);
                Grid.SetRow(grdOrderSearch, 2);
                grdOrderSearch.Margin = new Thickness(0, 0, 0, 10);
            }
            else
            {
                grdHead.ColumnDefinitions[2].Width = new GridLength(405, GridUnitType.Pixel);

                Grid.SetColumn(grdSearch, 2);
                Grid.SetRow(grdSearch, 0);
                grdSearch.Margin = new Thickness();

                Grid.SetRow(customerCard, 0);
                Grid.SetColumn(customerCard, 0);

                Grid.SetRow(grdCustomerDetails, 0);
                Grid.SetColumn(grdCustomerDetails, 1);

                grdTbCustomer.RowDefinitions[0].Height = new GridLength(2, GridUnitType.Star);
                grdTbCustomer.RowDefinitions[1].Height = GridLength.Auto;

                grdTbCustomer.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
                grdTbCustomer.ColumnDefinitions[1].Width = new GridLength(4, GridUnitType.Star);

                formCustomer.EditTemplate = Resources["dtWideCustomers"] as DataTemplate;


                grdTbOrders.ColumnDefinitions[2].Width = new GridLength(405, GridUnitType.Pixel);

                Grid.SetColumn(grdOrderSearch, 2);
                Grid.SetRow(grdOrderSearch, 0);
                grdOrderSearch.Margin = new Thickness();
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await AsyncHelper.RunAsync(LoadCountryCodes);
            customersDataSource.Load();
        }

        private async Task LoadCountryCodes() => CountryCodes = (await _countryCodesContext.LoadAsync(_countryCodesContext.GetCountriesQuery())).Entities;

        private void btnSearchCancel_Click(object sender, RoutedEventArgs e)
        {
            SearchText = string.Empty;
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void grdCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Models.Customers)
            {
                SelectedCustomer = e.AddedItems[0] as Models.Customers;
            }
            else
            {
                SelectedCustomer = null;
            }

#if DEBUG
            Console.WriteLine("grdCustomers_SelectionChanged, {0} Items Added", e.AddedItems.Count);
            if (e.AddedItems.Count > 0)
                Console.WriteLine(e.AddedItems[0]);
#endif
        }

        private void formCustomer_EditEnded(object sender, DataFormEditEndedEventArgs e)
        {
            if (e.EditAction == DataFormEditAction.Commit)
            {
                _customersContext.SubmitChanges(OnFormCustomerSubmitCompleted, null);
            }
            else if (e.EditAction == DataFormEditAction.Cancel)
            {
                _customersContext.RejectChanges();
            }
        }
        private void OnFormCustomerSubmitCompleted(SubmitOperation so)
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
                //SelectedCustomer.CountryCodes = CountryCodes;
                SelectedCustomer.CountryName = CountryCodes.FirstOrDefault(x => x.CountryCodeID == SelectedCustomer.CountryCode)?.Name;
            }
        }

        private void btnOrderSearchCancel_Click(object sender, RoutedEventArgs e)
        {
            SearchOrderText = string.Empty;
        }

        private void tcDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _ordersTabSelected = e.AddedItems.Count > 0 && e.AddedItems.Contains(tbOrders);
            if (_ordersTabSelected && SelectedCustomer != null)
            {
                var customerId = SelectedCustomer.CustomerID;
                var customerParam = ordersDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "customerId");
                customerParam.Value = customerId;
                ordersDataSource.Load();
            }
        }

        private void grdOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("grdOrders_SelectionChanged, {0} Items Added", e.AddedItems.Count);
#endif
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Models.Orders)
            {
                SelectedOrder = e.AddedItems[0] as Models.Orders;
            }
            else
            {
                SelectedOrder = null;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete the customer and all orders belong that customer?", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
                return;

            if (_customersContext.Customers.CanRemove)
            {
                _customersContext.Customers.Remove(SelectedCustomer);
                _customersContext.SubmitChanges(OnDeleteSubmitCompleted, null);
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

        private async void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            var result = await CustomerAddEditWindow.Show(new Models.Customers
            {
                IsEditMode = true,
                CountryCodes = CountryCodes,
                CountryCode = CountryCodes.FirstOrDefault().CountryCodeID,
                BirthDateUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }, _customersContext);

            if (result)
            {
                NavigationService.Refresh();
            }
        }

        private async void btnShowOrder_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOrder == null)
                return;

            SelectedOrder.CountryCodes = CountryCodes;
            SelectedOrder.CustomersCombo = (await _customersContext.LoadAsync(_customersContext.GetCustomersComboQuery())).Entities;
            SelectedOrder.Statuses = (await _orderStatusContext.LoadAsync(_orderStatusContext.GetOrderStatusQuery())).Entities;
            SelectedOrder.Shippers = (await _shippersContext.LoadAsync(_shippersContext.GetShippersQuery())).Entities;
            SelectedOrder.PaymentTypes = (await _paymentTypesContext.LoadAsync(_paymentTypesContext.GetPaymentTypesQuery())).Entities;

            await OrderAddEditWindow.Show(SelectedOrder, _orderContext);
        }
        private async void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            await AsyncHelper.RunAsync(ArrangeOrderAddEditWindow);
        }
        private async Task ArrangeOrderAddEditWindow()
        {
            if (SelectedCustomer == null)
                return;

            var order = new Models.Orders
            {
                IsEditMode = true,
                CountryCodes = CountryCodes,
                CustomersCombo = (await _customersContext.LoadAsync(_customersContext.GetCustomersComboQuery())).Entities,
                CustomerID = SelectedCustomer.CustomerID,
                Statuses = (await _orderStatusContext.LoadAsync(_orderStatusContext.GetOrderStatusQuery())).Entities,
                Shippers = (await _shippersContext.LoadAsync(_shippersContext.GetShippersQuery())).Entities,
                PaymentTypes = (await _paymentTypesContext.LoadAsync(_paymentTypesContext.GetPaymentTypesQuery())).Entities
            };

            var result = await OrderAddEditWindow.Show(order, _orderContext);
            if (result)
            {
                NavigationService.Refresh();
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSearch.Focus();
            }
        }
        private void txtOrderSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnOrderSearch.Focus();
            }
        }
    }
}