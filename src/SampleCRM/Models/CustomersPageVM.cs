using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenRiaServices.Controls;
using OpenRiaServices.DomainServices.Client;
using SampleCRM.Web.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Models
{
    public partial class CustomersPageVM : ObservableObject
    {
        #region Properties
        public static Dictionary<string, string> Countries { get; private set; }

        private bool _ordersTabSelected;

        private readonly CustomersContext _customersContext = new();
        private readonly OrderContext _orderContext = new();
        private readonly CountryCodesContext _countryCodesContext = new();
        private readonly OrderStatusContext _orderStatusContext = new();
        private readonly ShippersContext _shippersContext = new();
        private readonly PaymentTypeContext _paymentTypesContext = new();

        public DomainDataSource OrdersDataSource { get; }
        public DomainDataSource CustomersDataSource { get; }

        [ObservableProperty]
        private IEnumerable<CountryCodes> countryCodes;

        [ObservableProperty]
        private int selectedDetailsTabIndex;

        [ObservableProperty]
        private Customers selectedCustomer;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private Orders selectedOrder;

        [ObservableProperty]
        private string searchOrderText;
        #endregion

        public CustomersPageVM()
        {
            CustomersDataSource = new DomainDataSource
            {
                QueryName = "GetCustomers",
                PageSize = 5,
                LoadSize = 5,
                DomainContext = _customersContext,
            };
            CustomersDataSource.QueryParameters.Add(new Parameter { ParameterName = "search", Value = "" });
            CustomersDataSource.SortDescriptors.Add(new SortDescriptor(nameof(Customers.FirstName), ListSortDirection.Ascending));

            OrdersDataSource = new DomainDataSource
            {
                QueryName = "GetOrdersOfCustomer",
                PageSize = 10,
                LoadSize = 10,
                DomainContext = _orderContext,
            };
            OrdersDataSource.QueryParameters.Add(new Parameter { ParameterName = "search", Value = "" });
            OrdersDataSource.QueryParameters.Add(new Parameter { ParameterName = "customerId", Value = "" });
            OrdersDataSource.SortDescriptors.Add(new SortDescriptor(nameof(Orders.OrderDateUTC), ListSortDirection.Descending));
            OrdersDataSource.LoadedData += ordersDataSource_LoadedData;
        }

        #region Handle changing properties
        partial void OnSelectedDetailsTabIndexChanged(int value)
        {
            _ordersTabSelected = SelectedDetailsTabIndex == 1;
            if (_ordersTabSelected && SelectedCustomer != null)
            {
                var customerId = SelectedCustomer.CustomerID;
                var customerParam = OrdersDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "customerId");
                customerParam.Value = customerId;
                OrdersDataSource.Load();
            }
        }

        partial void OnSelectedCustomerChanged(Customers value)
        {
            if (value != null)
            {
                if (_ordersTabSelected)
                {
                    var customerId = value.CustomerID;
                    var customerParam = OrdersDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "customerId");
                    customerParam.Value = customerId;
                    OrdersDataSource.Load();
                }
#if DEBUG
                Console.WriteLine($"Customers, Customer: {value.FullName} selected");
#endif
            }
        }

#if DEBUG
        partial void OnSelectedOrderChanged(Orders value)
        {
            if (value != null)
            {
                Console.WriteLine($"Orders, Order: {value.OrderID} selected");
            }
        }
#endif

        private void ordersDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            var orders = e.Entities.Cast<Orders>();
            foreach (var order in orders)
            {
                order.CountryCodes = CountryCodes;
                order.OrderShown += async (s, e) => await ShowOrder(s as Orders);
            }
        }
        #endregion

        #region Commands
        [RelayCommand]
        public async Task Initialize()
        {
            await AsyncHelper.RunAsync(LoadCountryCodes);
            CustomersDataSource.Load();
        }

        private async Task LoadCountryCodes()
        {
            CountryCodes = (await _countryCodesContext.LoadAsync(_countryCodesContext.GetCountriesQuery())).Entities;
            Countries = CountryCodes.ToDictionary(x => x.CountryCodeID, x => x.Name);
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
        public void Search()
        {
            var value = SearchText;
            var searchParam = CustomersDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "search");
            searchParam.Value = value;
            CustomersDataSource.Load();
        }

        [RelayCommand]
        public void SearchCancel()
        {
            SearchText = string.Empty;
            Search();
        }

        [RelayCommand]
        public void SearchOrder()
        {
            var value = SearchOrderText;
            var searchParam = OrdersDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "search");
            searchParam.Value = value;
            OrdersDataSource.Load();
        }

        [RelayCommand]
        public void SearchOrderCancel()
        {
            SearchOrderText = string.Empty;
            SearchOrder();
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
                OrdersDataSource.Load();
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
                CustomersDataSource.Load();
            }
        }

        [RelayCommand]
        public async Task ShowOrder(Orders order)
        {
            if (order == null)
                order = SelectedOrder;
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
        public void Delete(Customers customer)
        {
            if (customer == null)
                return;

            if (_customersContext.Customers.CanRemove)
            {
                var result = MessageBox.Show("Are you sure to delete the customer and all orders belong that customer?", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK)
                    return;

                _customersContext.Customers.Remove(customer);
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
                CustomersDataSource.Load();
            }
        }
        #endregion
    }
}