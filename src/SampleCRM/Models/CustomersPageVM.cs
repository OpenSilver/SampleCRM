using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenRiaServices.DomainServices.Client;
using SampleCRM.Web.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SampleCRM.Web.Models
{
    public partial class CustomersPageVM : ObservableObject
    {
        #region Properties
        public static Dictionary<string, string> Countries { get; private set; } = new();

        private readonly CustomersContext _customersContext = new();
        private readonly OrderContext _orderContext = new();
        private readonly CountryCodesContext _countryCodesContext = new();
        private readonly OrderStatusContext _orderStatusContext = new();
        private readonly ShippersContext _shippersContext = new();
        private readonly PaymentTypeContext _paymentTypesContext = new();

        private ICollectionView _customersView;
        public ICollectionView CustomersView
        {
            get => _customersView;
            private set => SetProperty(ref _customersView, value);
        }

        private ICollectionView _ordersView;
        public ICollectionView OrdersView
        {
            get => _ordersView;
            private set => SetProperty(ref _ordersView, value);
        }

        // todo: remove this property after applying similar changes to Orders
        [ObservableProperty]
        private IEnumerable<CountryCodes> countryCodes;

        [ObservableProperty]
        private bool isOrdersTabSelected;

        [ObservableProperty]
        private Customers selectedCustomer;

        [ObservableProperty]
        private string searchText = "";

        [ObservableProperty]
        private Orders selectedOrder;

        [ObservableProperty]
        private string searchOrderText = "";
        #endregion

        #region Handle changing properties
        partial void OnIsOrdersTabSelectedChanged(bool value)
        {
            Task.Run(async () => await LoadOrdersForSelectedCustomer());
        }

        partial void OnSelectedCustomerChanged(Customers value)
        {
            Task.Run(async () => await LoadOrdersForSelectedCustomer());
        }

#if DEBUG
        partial void OnSelectedOrderChanged(Orders value)
        {
            Console.WriteLine($"Orders, Order: {value?.OrderID} selected");
        }
#endif
        #endregion

        #region Commands
        [RelayCommand]
        public async Task Initialize()
        {
            await AsyncHelper.RunAsync(LoadCountryCodes);
            await LoadCustomers();
        }

        private async Task LoadCountryCodes()
        {
            CountryCodes = (await _countryCodesContext.LoadAsync(_countryCodesContext.GetCountriesQuery())).Entities;
            Countries = CountryCodes.ToDictionary(x => x.CountryCodeID, x => x.Name);
        }

        [RelayCommand]
        public async Task LoadCustomers()
        {
            var query = _customersContext.GetCustomersQuery(SearchText).OrderBy(c => c.FirstName);
            var result = await _customersContext.LoadAsync(query);

            CustomersView = new PagedCollectionView(result.Entities);
        }

        [RelayCommand]
        public void CustomerFormEditEnded(DataFormEditAction e)
        {
            if (e == DataFormEditAction.Commit)
            {
                _customersContext.SubmitChanges(OnFormCustomerSubmitCompleted, null);
            }
            else if (e == DataFormEditAction.Cancel)
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
        }

        [RelayCommand]
        public async Task SearchCancel()
        {
            SearchText = string.Empty;
            await LoadCustomers();
        }

        [RelayCommand]
        public async Task LoadOrdersForSelectedCustomer()
        {
            if (IsOrdersTabSelected && SelectedCustomer != null)
            {
                var query = _orderContext.GetOrdersOfCustomerQuery(SelectedCustomer.CustomerID, SearchOrderText).OrderByDescending(o => o.OrderDateUTC);
                var result = await _orderContext.LoadAsync(query);

                foreach (var order in result.Entities)
                {
                    order.CountryCodes = CountryCodes;
                }

                OrdersView = new PagedCollectionView(result.Entities);
            }
        }

        [RelayCommand]
        public async Task SearchOrderCancel()
        {
            SearchOrderText = string.Empty;
            await LoadOrdersForSelectedCustomer();
        }

        [RelayCommand]
        public Task NewOrder() => AsyncHelper.RunAsync(ArrangeOrderAddEditWindow);

        private async Task ArrangeOrderAddEditWindow()
        {
            if (SelectedCustomer == null)
                return;

            var order = new Orders
            {
                IsEditMode = true,
                CountryCodes = CountryCodes,
                CustomerID = SelectedCustomer.CustomerID,
                Customer = SelectedCustomer,
                Statuses = (await _orderStatusContext.LoadAsync(_orderStatusContext.GetOrderStatusQuery())).Entities,
                Shippers = (await _shippersContext.LoadAsync(_shippersContext.GetShippersQuery())).Entities,
                PaymentTypes = (await _paymentTypesContext.LoadAsync(_paymentTypesContext.GetPaymentTypesQuery())).Entities
            };

            var result = await OrderAddEditWindow.Show(order, _orderContext);
            if (result)
            {
                await LoadOrdersForSelectedCustomer();
            }
        }

        [RelayCommand]
        public async Task NewCustomer()
        {
            var countryCode = CountryCodes.FirstOrDefault();
            var result = await CustomerAddEditWindow.Show(new Customers
            {
                IsEditMode = true,
                CountryCode = countryCode?.CountryCodeID,
                BirthDateUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }, _customersContext);

            if (result)
            {
                await LoadCustomers();
            }
        }

        [RelayCommand]
        public async Task ShowSelectedOrder()
        {
            if (SelectedOrder == null)
                return;

            SelectedOrder.CountryCodes = CountryCodes;
            SelectedOrder.Customer = SelectedCustomer;
            SelectedOrder.Statuses = (await _orderStatusContext.LoadAsync(_orderStatusContext.GetOrderStatusQuery())).Entities;
            SelectedOrder.Shippers = (await _shippersContext.LoadAsync(_shippersContext.GetShippersQuery())).Entities;
            SelectedOrder.PaymentTypes = (await _paymentTypesContext.LoadAsync(_paymentTypesContext.GetPaymentTypesQuery())).Entities;

            await OrderAddEditWindow.Show(SelectedOrder, _orderContext);
        }

        [RelayCommand]
        public void DeleteSelectedCustomer()
        {
            if (SelectedCustomer == null)
                return;

            if (_customersContext.Customers.CanRemove)
            {
                var result = MessageBox.Show("Are you sure to delete the customer and all orders belong that customer?", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK)
                    return;

                _customersContext.Customers.Remove(SelectedCustomer);
                _customersContext.SubmitChanges(OnDeleteSubmitCompleted, null);
            }
            else
            {
                throw new AccessViolationException("RIA Service Delete Entity for Customer Context is denied");
            }
        }

        private async void OnDeleteSubmitCompleted(SubmitOperation so)
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
                await LoadCustomers();
            }
        }
        #endregion
    }
}