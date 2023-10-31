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
        private CustomersContext _customersContext = new CustomersContext();
        private CountryCodesContext _countryCodesContext = new CountryCodesContext();
        private OrderContext _orderContext = new OrderContext();
        private OrderStatusContext _orderStatusContext = new OrderStatusContext();
        private ShippersContext _shippersContext = new ShippersContext();
        private PaymentTypeContext _paymentTypesContext = new PaymentTypeContext();
        #endregion

        private bool _ordersTabSelected;

        #region DataContext Properties
        private IEnumerable<Models.Customers> _customersCollection;
        public IEnumerable<Models.Customers> CustomersCollection
        {
            get { return _customersCollection; }
            set
            {
                if (_customersCollection != value)
                {
                    _customersCollection = value;
                    OnPropertyChanged();
                    OnPropertyChanged("FilteredCustomersCollection");
                    SelectedCustomer = FilteredCustomersCollection.FirstOrDefault();
                }
            }
        }

        public IEnumerable<Models.Customers> FilteredCustomersCollection
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_searchText))
                {
                    return _customersCollection;
                }
                else
                {
                    return _customersCollection.Where(x => x.FullName.ToLowerInvariant().Contains(_searchText.ToLowerInvariant()));
                }
            }
        }

        private IEnumerable<Models.CountryCodes> _countryCodes;
        public IEnumerable<Models.CountryCodes> CountryCodes
        {
            get { return _countryCodes; }
            set
            {
                if (_countryCodes != value)
                {
                    _countryCodes = value;
                    OnPropertyChanged();
                }
            }
        }

        private Models.Customers _selectedCustomer;
        public Models.Customers SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (_selectedCustomer != value)
                {
                    _selectedCustomer = value;
                    OnPropertyChanged();
                    OnPropertyChanged("AnySelectedCustomer");
                    LoadOrdersOfCustomer();
#if DEBUG
                    Console.WriteLine($"Customers, Customer: {value.FirstName} selected");
#endif
                }
            }
        }

        public bool AnySelectedCustomer => _selectedCustomer != null;

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    OnPropertyChanged("FilteredCustomersCollection");
                    SelectedCustomer = FilteredCustomersCollection.FirstOrDefault();
                    //OnPropertyChanged("SelectedCustomer");
                    //OnPropertyChanged("AnySelectedCustomer");
                }
            }
        }


        private IEnumerable<Models.Orders> _ordersCollection;
        public IEnumerable<Models.Orders> OrdersCollection
        {
            get { return _ordersCollection; }
            set
            {
                if (_ordersCollection != value)
                {
                    _ordersCollection = value;
                    OnPropertyChanged();
                    OnPropertyChanged("FilteredOrdersCollection");
                    SelectedOrder = FilteredOrdersCollection.FirstOrDefault();
                }
            }
        }

        public IEnumerable<Models.Orders> FilteredOrdersCollection
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_searchOrderText))
                {
                    return _ordersCollection;
                }
                else
                {
                    return _ordersCollection.Where(x => x.OrderID.ToString().Contains(_searchOrderText));
                }
            }
        }

        private Models.Orders _selectedOrder;
        public Models.Orders SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                if (_selectedOrder != value)
                {
                    _selectedOrder = value;
                    OnPropertyChanged();
#if DEBUG
                    Console.WriteLine($"Orders, Order: {value.OrderID} selected");
#endif
                }
            }
        }

        private string _searchOrderText;
        public string SearchOrderText
        {
            get { return _searchOrderText; }
            set
            {
                if (_searchOrderText != value)
                {
                    _searchOrderText = value;
                    OnPropertyChanged();
                    OnPropertyChanged("FilteredOrdersCollection");
                    SelectedOrder = FilteredOrdersCollection.FirstOrDefault();
                    //OnPropertyChanged("SelectedOrder");
                }
            }
        }
        #endregion 

        public Customers()
        {
            InitializeComponent();
            DataContext = this;
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
            await RetryOnExceptionHelper.RetryAsync(LoadElements);
        }

        private async Task LoadElements()
        {
            var customersQuery = _customersContext.GetCustomersQuery();
            var customersOp = await _customersContext.LoadAsync(customersQuery);
            CustomersCollection = customersOp.Entities;

            await RetryOnExceptionHelper.RetryAsync(LoadCountryCodes);
#if DEBUG
            Console.WriteLine("Customers Collection:" + CustomersCollection.Count());
            foreach (var item in CustomersCollection)
            {
                Console.WriteLine("Customer Name:" + item.FirstName);
                Console.WriteLine("Customer Picture Bytes:" + item.Picture.Length);
            }
#endif
        }
        private async Task LoadCountryCodes()
        {
            var countryCodesquery = _countryCodesContext.GetCountriesQuery();
            var countriesOp = await _countryCodesContext.LoadAsync(countryCodesquery);
            CountryCodes = countriesOp.Entities;

            foreach (var c in CustomersCollection)
            {
                c.CountryCodes = CountryCodes;
                c.CountryName = CountryCodes.SingleOrDefault(x => x.CountryCodeID == c.CountryCode).Name;
            }
        }
        private async Task LoadOrdersOfCustomer()
        {
            if (SelectedCustomer == null)
                return;

            if (!_ordersTabSelected)
                return;

            var customerId = SelectedCustomer.CustomerID;
            var ordersQuery = _orderContext.GetOrdersOfCustomerQuery(customerId);
            var ordersOp = await _orderContext.LoadAsync(ordersQuery);
            OrdersCollection = ordersOp.Entities;

            foreach (var o in OrdersCollection)
            {
                o.ShipCountryName = CountryCodes.SingleOrDefault(x => x.CountryCodeID == o.ShipCountryCode)?.Name;
            }

#if DEBUG
            Console.WriteLine("Orders Collection:" + OrdersCollection.Count());
            foreach (var item in OrdersCollection)
            {
                Console.WriteLine("Order Id:" + item.OrderID);
            }
#endif
        }

        private void btnSearchCancel_Click(object sender, RoutedEventArgs e)
        {
            SearchText = string.Empty;
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void grdCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("grdCustomers_SelectionChanged, {0} Items Added", e.AddedItems.Count);
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
                SelectedCustomer.IsEditMode = false;
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
                OnPropertyChanged("SelectedCustomer");
                OnPropertyChanged("CustomersCollection");
                OnPropertyChanged("FilteredCustomersCollection");
            }
        }

        private void btnOrderSearchCancel_Click(object sender, RoutedEventArgs e)
        {
            SearchOrderText = string.Empty;
        }

        private async void tcDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var anySelected = e.AddedItems.Count > 0;
            if (anySelected)
            {
                _ordersTabSelected = e.AddedItems.Contains(tbOrders);
                if (_ordersTabSelected)
                {
                    await RetryOnExceptionHelper.RetryAsync(LoadOrdersOfCustomer);
                }
            }
            else
            {
                _ordersTabSelected = false;
            }
        }

        private void grdOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("grdOrders_SelectionChanged, {0} Items Added", e.AddedItems.Count);
#endif
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
                BirthDate = DateTime.Now.ToShortDateString()
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

            await OrderAddEditWindow.Show(SelectedOrder, _orderContext);
        }
        private async void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            await RetryOnExceptionHelper.RetryAsync(ArrangeOrderAddEditWindow, 15);
        }
        private async Task ArrangeOrderAddEditWindow()
        {
            var result = await OrderAddEditWindow.Show(new Models.Orders
            {
                IsEditMode = true,
                CustomersCombo = (await _customersContext.LoadAsync(_customersContext.GetCustomersQuery())).Entities,
                Statuses = (await _orderStatusContext.LoadAsync(_orderStatusContext.GetOrderStatusQuery())).Entities,
                CountryCodes = (await _countryCodesContext.LoadAsync(_countryCodesContext.GetCountriesQuery())).Entities,
                Shippers = (await _shippersContext.LoadAsync(_shippersContext.GetShippersQuery())).Entities,
                PaymentTypes = (await _paymentTypesContext.LoadAsync(_paymentTypesContext.GetPaymentTypesQuery())).Entities
            }, _orderContext);

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