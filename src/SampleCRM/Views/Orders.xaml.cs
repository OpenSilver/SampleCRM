using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SampleCRM.Web.Views
{
    public partial class Orders : BasePage
    {
        private OrderStatusContext _orderStatusContext = new OrderStatusContext();
        private CountryCodesContext _countryCodesContext = new CountryCodesContext();
        private OrderContext _orderContext = new OrderContext();
        private OrderItemsContext _orderItemsContext = new OrderItemsContext();

        private bool _orderItemsTabSelected;

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
                if (string.IsNullOrWhiteSpace(_searchText))
                {
                    return _ordersCollection;
                }
                else
                {
                    return _ordersCollection.Where(x => x.OrderID.ToString().Contains(_searchText.ToLowerInvariant()));
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

        private IEnumerable<Models.OrderStatus> _statuses;
        public IEnumerable<Models.OrderStatus> Statuses
        {
            get { return _statuses; }
            set
            {
                if (_statuses != value)
                {
                    _statuses = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged("AnySelectedOrder");
                    LoadOrderItemsOfOrder();
#if DEBUG
                    Console.WriteLine($"Orders, Order: {value.OrderID} selected");
#endif
                }
            }
        }

        public bool AnySelectedOrder => _selectedOrder != null;

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
                    OnPropertyChanged("FilteredOrdersCollection");
                    //OnPropertyChanged("SelectedOrder");
                    //OnPropertyChanged("AnySelectedOrder");
                }
            }
        }


        private IEnumerable<Models.OrderItems> _orderItemsCollection;
        public IEnumerable<Models.OrderItems> OrderItemsCollection
        {
            get { return _orderItemsCollection; }
            set
            {
                if (_orderItemsCollection != value)
                {
                    _orderItemsCollection = value;
                    OnPropertyChanged();
                    OnPropertyChanged("FilteredOrderItemsCollection");
                }
            }
        }

        public IEnumerable<Models.OrderItems> FilteredOrderItemsCollection
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_searchOrderItemText))
                {
                    return _orderItemsCollection;
                }
                else
                {
                    return _orderItemsCollection.Where(x => x.OrderID.ToString().Contains(_searchOrderItemText));
                }
            }
        }

        private Models.OrderItems _selectedOrderItem;
        public Models.OrderItems SelectedOrderItem
        {
            get { return _selectedOrderItem; }
            set
            {
                if (_selectedOrderItem != value)
                {
                    _selectedOrderItem = value;
                    OnPropertyChanged();
#if DEBUG
                    Console.WriteLine($"OrderItems, OrderItem: {value.OrderLine} selected");
#endif
                }
            }
        }

        private string _searchOrderItemText;
        public string SearchOrderItemText
        {
            get { return _searchOrderItemText; }
            set
            {
                if (_searchOrderItemText != value)
                {
                    _searchOrderItemText = value;
                    OnPropertyChanged();
                    OnPropertyChanged("FilteredOrderItemsCollection");
                    //OnPropertyChanged("SelectedOrder");
                }
            }
        }


        public Orders()
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
            var query = _orderContext.GetOrdersQuery();
            var op = await _orderContext.LoadAsync(query);
            OrdersCollection = op.Entities;

            await LoadCountryCodes();
#if DEBUG
            Console.WriteLine("Orders Collection:" + OrdersCollection.Count());
            foreach (var item in OrdersCollection)
            {
                Console.WriteLine("Order Id:" + item.OrderID);
                Console.WriteLine("Customer Id:" + item.CustomerID);
            }
#endif
        }

        private async Task LoadCountryCodes()
        {
            var query = _countryCodesContext.GetCountriesQuery();
            var op = await _countryCodesContext.LoadAsync(query);
            CountryCodes = op.Entities;

            foreach (var o in OrdersCollection)
            {
                o.CountryCodes = CountryCodes;
                o.ShipCountryName = CountryCodes.SingleOrDefault(x => x.CountryCodeID == o.ShipCountryCode).Name;
            }
        }

        private async Task LoadStatuses()
        {
            var query = _orderStatusContext.GetOrderStatusQuery();
            var op = await _countryCodesContext.LoadAsync(query);
            Statuses = op.Entities;

            foreach (var o in OrdersCollection)
            {
                o.Statuses = Statuses;
                o.StatusDesc = Statuses.SingleOrDefault(x => x.Status == o.Status).Name;
            }
        }

        private async void LoadOrderItemsOfOrder()
        {
            if (SelectedOrder == null)
                return;

            if (!_orderItemsTabSelected)
                return;

            var orderId = SelectedOrder.OrderID;
            var query = _orderItemsContext.GetOrderItemsOfOrderQuery(orderId);
            var op = await _orderContext.LoadAsync(query);
            OrderItemsCollection = op.Entities;

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

        private void grdOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("grdOrders_SelectionChanged, {0} Items Added", e.AddedItems.Count);
#endif
        }

        private async void btnNew_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
            //var result = await CustomerAddEditWindow.Show(new Models.Customers
            //{
            //    IsEditMode = true,
            //    CountryCodes = CountryCodes,
            //    CountryCode = CountryCodes.FirstOrDefault().CountryCodeID,
            //    BirthDate = DateTime.Now.ToShortDateString()
            //});

            //if (result)
            //{
            //    NavigationService.Refresh();
            //}
        }
    }
}
