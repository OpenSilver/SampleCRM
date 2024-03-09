using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenRiaServices.DomainServices.Client;
using SampleCRM.Web.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SampleCRM.Web.Models
{
    public partial class OrdersPageVM : ObservableObject
    {
        #region Properties
        private readonly OrderContext _orderContext = new();
        private readonly OrderItemsContext _orderItemsContext = new();
        private readonly CountryCodesContext _countryCodesContext = new();
        private readonly CustomersContext _customersContext = new();
        private readonly OrderStatusContext _orderStatusContext = new();
        private readonly ProductsContext _productsContext = new();
        private readonly TaxTypesContext _taxTypesContext = new();
        private readonly ShippersContext _shippersContext = new();
        private readonly PaymentTypeContext _paymentTypesContext = new();

        private ICollectionView _ordersView;
        public ICollectionView OrdersView
        {
            get => _ordersView;
            private set => SetProperty(ref _ordersView, value);
        }

        private ICollectionView _orderItemsView;
        public ICollectionView OrderItemsView
        {
            get => _orderItemsView;
            private set => SetProperty(ref _orderItemsView, value);
        }

        [ObservableProperty]
        private bool isOrderItemsTabSelected;

        [ObservableProperty]
        private IEnumerable<CountryCode> countryCodes;

        [ObservableProperty]
        private IEnumerable<Shipper> shippers;

        [ObservableProperty]
        private IEnumerable<PaymentType> paymentTypes;

        [ObservableProperty]
        private IEnumerable<OrderStatu> statuses;

        [ObservableProperty]
        private Order selectedOrder;

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private OrderItem selectedOrderItem;

        [ObservableProperty]
        private string searchOrderItemText = string.Empty;

        #endregion

        #region Handle changing properties
        partial void OnSelectedOrderChanged(Order value)
        {
            Task.Run(async () =>
            {
                await AsyncHelper.RunAsync(LoadCustomer, SelectedOrder);
                await AsyncHelper.RunAsync(LoadOrderItems);
            });
#if DEBUG
            Console.WriteLine($"Orders, Order: {value?.OrderID} selected");
#endif
        }

        partial void OnSelectedOrderItemChanged(OrderItem value)
        {
#if DEBUG
            Console.WriteLine($"OrderItems, OrderItem: {value.OrderID}, {value.OrderLine} selected");
#endif
        }

        partial void OnIsOrderItemsTabSelectedChanged(bool value)
        {
            Task.Run(async () => await AsyncHelper.RunAsync(LoadOrderItems));
        }
        #endregion

        #region Commands
        [RelayCommand]
        public async Task Initialize()
        {
            await AsyncHelper.RunAsync(LoadPaymentTypes);
            await AsyncHelper.RunAsync(LoadShippers);
            await AsyncHelper.RunAsync(LoadCountryCodes);
            await AsyncHelper.RunAsync(LoadStatuses);
            await AsyncHelper.RunAsync(LoadOrders);
        }
        private async Task LoadShippers() => Shippers = (await _shippersContext.LoadAsync(_shippersContext.GetShippersQuery())).Entities;
        private async Task LoadPaymentTypes() => PaymentTypes = (await _paymentTypesContext.LoadAsync(_paymentTypesContext.GetPaymentTypesQuery())).Entities;
        private async Task LoadCountryCodes() => CountryCodes = (await _countryCodesContext.LoadAsync(_countryCodesContext.GetCountriesQuery())).Entities;
        private async Task LoadStatuses() => Statuses = (await _orderStatusContext.LoadAsync(_orderStatusContext.GetOrderStatusQuery())).Entities;

        private async Task LoadCustomer(Order order)
        {
            if (order == null)
                return;

            if (order.Customer == null || order.Customer.CustomerID != order.CustomerID)
                order.Customer = (await _customersContext.LoadAsync(_customersContext.GetCustomerByIdQuery(SelectedOrder.CustomerID))).Entities.FirstOrDefault();
        }
        private async Task LoadProduct(OrderItem orderItem)
        {
            if (orderItem == null)
                return;

            if (orderItem.Product == null)
            {
                orderItem.Product = (await _productsContext.LoadAsync(_productsContext.GetProductByIdQuery(orderItem.ProductID))).Entities.FirstOrDefault();
            }
        }
        private async Task LoadTaxRate(OrderItem orderItem)
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

        [RelayCommand]
        public async Task LoadOrders()
        {
            var query = _orderContext.GetOrdersQuery(SearchText).OrderByDescending(c => c.OrderDateUTC);
            var result = await _orderContext.LoadAsync(query);

            OrdersView = new PagedCollectionView(result.Entities);

            foreach (var order in result.Entities)
            {
                order.CountryCodes = CountryCodes;
                order.Shippers = Shippers;
                order.PaymentTypes = PaymentTypes;
                order.Statuses = Statuses;
            }
        }

        [RelayCommand]
        public async Task LoadOrderItems()
        {
            if (SelectedOrder == null)
                return;

            if (IsOrderItemsTabSelected)
            {
                var query = _orderItemsContext.GetOrderItemsOfOrderQuery(SelectedOrder.OrderID, SearchOrderItemText).OrderBy(o => o.OrderLine);
                var result = await _orderItemsContext.LoadAsync(query);
                OrderItemsView = new PagedCollectionView(result.Entities);

                foreach (var orderItem in result.Entities)
                {
                    await LoadProduct(orderItem);
                    await LoadTaxRate(orderItem);
                }
            }
        }

        [RelayCommand]
        public async Task SearchCancel()
        {
            SearchText = string.Empty;
            await AsyncHelper.RunAsync(LoadOrders);
        }

        [RelayCommand]
        public async Task SearchOrderItemsCancel()
        {
            SearchOrderItemText = string.Empty;
            await AsyncHelper.RunAsync(LoadOrderItems);
        }

        [RelayCommand]
        public async Task NewOrder() => await AsyncHelper.RunAsync(ArrangeOrderAddEditWindow);

        private async Task ArrangeOrderAddEditWindow()
        {
            var newOrder = new Order
            {
                IsEditMode = true
            };

            UpdateComboDataForOrder(newOrder);
            var result = await OrderAddEditWindow.Show(newOrder, _orderContext);
            if (result)
                await LoadOrders();
        }

        private void UpdateComboDataForOrder(Order order)
        {
            order.CountryCodes = CountryCodes;
            order.Shippers = Shippers;
            order.PaymentTypes = PaymentTypes;
            order.Statuses = Statuses;
        }

        [RelayCommand]
        public void DeleteSelectedOrder()
        {
            var result = MessageBox.Show("Are you sure to delete the order and all items belong that order?", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
                return;

            if (_orderContext.Orders.CanRemove)
            {
                _orderContext.Orders.Remove(SelectedOrder);
                _orderContext.SubmitChanges(OnDeleteSubmitCompletedAsync, null);
            }
            else
            {
                throw new AccessViolationException("RIA Service Delete Entity for Order Context is denied");
            }
        }

        private async void OnDeleteSubmitCompletedAsync(SubmitOperation so)
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
                await AsyncHelper.RunAsync(LoadOrders);
            }
        }

        [RelayCommand]
        private void OrderFormEditEnded(DataFormEditAction e)
        {
            if (e == DataFormEditAction.Commit)
            {
                _orderContext.SubmitChanges(OnFormOrderSubmitCompleted, null);
            }
            else if (e == DataFormEditAction.Cancel)
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

        [RelayCommand]
        public async Task NewOrderItem() => await AsyncHelper.RunAsync(ArrangeOrderItemWindow);

        private async Task ArrangeOrderItemWindow()
        {
            if (SelectedOrder == null)
                throw (new ArgumentNullException("Selected Order can't be null"));

            var newOrderItem = new OrderItem
            {
                IsEditMode = true,
                OrderID = SelectedOrder.OrderID,
                OrderLine = 0,
                TaxTypes = (await _taxTypesContext.LoadAsync(_taxTypesContext.GetTaxTypesQuery())).Entities
            };

            var result = await OrderItemAddEditWindow.Show(newOrderItem, _orderItemsContext);
            if (result)
                await LoadOrderItems();
        }

        [RelayCommand]
        public async Task ShowOrderItem()
        {
            if (SelectedOrderItem == null)
                throw (new ArgumentNullException("Selected Order Item can't be null"));

            if (SelectedOrderItem.TaxTypes == null)
                SelectedOrderItem.TaxTypes = (await _taxTypesContext.LoadAsync(_taxTypesContext.GetTaxTypesQuery())).Entities;

            var result = await OrderItemAddEditWindow.Show(SelectedOrderItem, _orderItemsContext);
            if (result)
                await AsyncHelper.RunAsync(LoadOrderItems);
        }

        #endregion
    }
}