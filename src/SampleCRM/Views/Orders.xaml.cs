using OpenRiaServices.DomainServices.Client;
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
        private CustomersContext _customersContext = new CustomersContext();
        //private ProductsContext _productsContext = new ProductsContext();

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
                    LoadCustomer();
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
            await LoadStatuses();
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
            var op = await _orderStatusContext.LoadAsync(query);
            Statuses = op.Entities;

            foreach (var o in OrdersCollection)
            {
                o.Statuses = Statuses;
                o.StatusDesc = Statuses.FirstOrDefault(x => x.Status == o.Status)?.Name;
            }
        }

        private async void LoadCustomer()
        {
            if (SelectedOrder == null)
                return;

            if (SelectedOrder.Customer == null)
            {
                var query = _customersContext.GetCustomersQuery().Where(x => x.CustomerID == SelectedOrder.CustomerID);
                var op = await _customersContext.LoadAsync(query);
                var customer = op.Entities.FirstOrDefault();
                SelectedOrder.Customer = customer;
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

#if DEBUG
            Console.WriteLine("Order Items Collection:" + OrderItemsCollection.Count());
            foreach (var item in OrderItemsCollection)
            {
                Console.WriteLine("Order Item Id:" + item.OrderLine);
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

        private void tcDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var anySelected = e.AddedItems.Count > 0;
            if (anySelected)
            {
                _orderItemsTabSelected = e.AddedItems.Contains(tbOrderItems);
                if (_orderItemsTabSelected)
                {
                    LoadOrderItemsOfOrder();
                }
            }
            else
            {
                _orderItemsTabSelected = false;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete the order and all items belong that order?", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
                return;

            if (_orderContext.Orders.CanRemove)
            {
                _orderContext.Orders.Remove(SelectedOrder);
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
                MessageBox.Show(string.Format("Submit Failed: {0}", so.Error.Message));
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
                _customersContext.SubmitChanges(OnFormOrderSubmitCompleted, null);
            }
            else if (e.EditAction == DataFormEditAction.Cancel)
            {
                SelectedOrder.IsEditMode = false;
            }
        }

        private void OnFormOrderSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                MessageBox.Show(string.Format("Submit Failed: {0}", so.Error.Message));
                so.MarkErrorAsHandled();
            }
            else
            {
                OnPropertyChanged("SelectedOrder");
                OnPropertyChanged("OrdersCollection");
                OnPropertyChanged("FilteredOrdersCollection");
                grdOrders.UpdateLayout();
                grdOrders.InvalidateArrange();
                grdOrders.InvalidateMeasure();
            }
        }
    }
}
