using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;

namespace SampleCRM.Web.Views
{
    public partial class Customers : BasePage
    {
        private CustomersContext _customersContext = new CustomersContext();
        private CountryCodesContext _countryCodesContext = new CountryCodesContext();
        private OrderContext _orderContext = new OrderContext();


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

        public bool AnySelectedCustomer
        {
            get
            {
                return _selectedCustomer != null;
            }
        }

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
                    OnPropertyChanged("SelectedCustomer");
                    OnPropertyChanged("AnySelectedCustomer");
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
        private bool _ordersTabSelected;

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
                    OnPropertyChanged("SelectedOrder");
                }
            }
        }


        public Customers()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadElements();
        }

        private async void LoadElements()
        {
            var customersQuery = _customersContext.GetCustomersQuery();
            var categoriesOp = await _customersContext.LoadAsync(customersQuery);
            CustomersCollection = categoriesOp.Entities;

            var countryCodesquery = _countryCodesContext.GetCountriesQuery();
            var countriesOp = await _countryCodesContext.LoadAsync(countryCodesquery);
            CountryCodes = countriesOp.Entities;

            foreach (var c in CustomersCollection)
            {
                c.CountryCodes = CountryCodes;
                c.CountryName = CountryCodes.SingleOrDefault(x => x.CountryCodeID == c.CountryCode).Name;
            }
#if DEBUG
            Console.WriteLine("Customers Collection:" + CustomersCollection.Count());
            foreach (var item in CustomersCollection)
            {
                Console.WriteLine("Customer Name:" + item.FirstName);
                Console.WriteLine("Customer Picture Bytes:" + item.Picture.Length);
            }
#endif
        }

        private async void LoadOrdersOfCustomer()
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

        private void grdCustomers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("grdCustomers_SelectionChanged, {0} Items Added", e.AddedItems.Count);
#endif
        }

        private void formCustomer_EditEnded(object sender, System.Windows.Controls.DataFormEditEndedEventArgs e)
        {
            if (e.EditAction == System.Windows.Controls.DataFormEditAction.Commit)
            {
                _customersContext.SubmitChanges(OnFormCustomerSubmitCompleted, null);
            }
        }

        private void OnFormCustomerSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                MessageBox.Show(string.Format("Submit Failed: {0}", so.Error.Message));
                so.MarkErrorAsHandled();
            }
            else
            {
                OnPropertyChanged("SelectedCustomer");
                OnPropertyChanged("CustomersCollection");
                OnPropertyChanged("FilteredCustomersCollection");
                grdCustomers.UpdateLayout();
                grdCustomers.InvalidateArrange();
                grdCustomers.InvalidateMeasure();
            }
        }


        private void btnOrderSearchCancel_Click(object sender, RoutedEventArgs e)
        {
            SearchOrderText = string.Empty;
        }

        private void tcDetails_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var anySelected = e.AddedItems.Count > 0;
            if (anySelected)
            {
                _ordersTabSelected = e.AddedItems.Contains(tbOrders);
                if (_ordersTabSelected)
                {
                    LoadOrdersOfCustomer();
                }
            }
            else
            {
                _ordersTabSelected = false;
            }
        }

        private void grdOrders_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("grdOrders_SelectionChanged, {0} Items Added", e.AddedItems.Count);
#endif
        }
    }
}