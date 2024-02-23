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
        #region Constructor
        public CustomersPageVM()
        {
            PropertyChanged += customersPageVM_PropertyChanged;

            countryCodesContext = new CountryCodesContext();
            orderStatusContext = new OrderStatusContext();
            shippersContext = new ShippersContext();
            paymentTypesContext = new PaymentTypeContext();
        }

        [RelayCommand]
        public async Task Initialize()
        {
            await AsyncHelper.RunAsync(LoadCountryCodes);
            CustomersDataSource.Load();
        }

        private async Task LoadCountryCodes() => CountryCodes = (await CountryCodesContext.LoadAsync(CountryCodesContext.GetCountriesQuery())).Entities;
        #endregion

        #region Events
        private void customersPageVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedDetailsTabIndex):
                    OrdersTabSelected = SelectedDetailsTabIndex == 1;
                    if (OrdersTabSelected && SelectedCustomer != null)
                    {
                        var customerId = SelectedCustomer.CustomerID;
                        var customerParam = OrdersDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "customerId");
                        customerParam.Value = customerId;
                        OrdersDataSource.Load();
                    }
                    break;
                case nameof(SelectedCustomer):
                    {
                        var value = SelectedCustomer;
                        AnySelectedCustomer = value != null;
                        if (value != null)
                        {
                            if (OrdersTabSelected)
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
                    break;
                case nameof(SelectedOrder):
                    {
                        var value = SelectedOrder;
                        if (value != null)
                        {
#if DEBUG
                            Console.WriteLine($"Orders, Order: {value.OrderID} selected");
#endif
                        }
                    }
                    break;
                case nameof(CustomersDataSource):
                    {
                        CustomersDataSource.LoadedData += customersDataSource_LoadedData;
                    }
                    break;
                case nameof(OrdersDataSource):
                    {
                        OrdersDataSource.LoadedData += ordersDataSource_LoadedData;
                    }
                    break;
                default:
                    break;
            }
        }

        private void ordersDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            var orders = e.Entities.Cast<Orders>();
            foreach (var order in orders)
            {
                order.CountryCodes = CountryCodes;
                order.OrderShown += async (s, e) => await ShowOrder(s as Orders);
            }
        }
        private void customersDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            var customers = e.Entities.Cast<Customers>();
            foreach (var customer in customers)
            {
                customer.CountryCodes = CountryCodes;
            }
        }
        #endregion

        #region Commands
        [RelayCommand]
        public void CustomerFormEditEnded(DataFormEditAction e)
        {
            if (e == DataFormEditAction.Commit)
            {
                CustomersContext.SubmitChanges(OnFormCustomerSubmitCompleted, null);
            }
            else if (e == DataFormEditAction.Cancel)
            {
                CustomersContext.RejectChanges();
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
        public Task NewOrder()
        {
            return AsyncHelper.RunAsync(ArrangeOrderAddEditWindow);
        }
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
                Statuses = (await OrderStatusContext.LoadAsync(OrderStatusContext.GetOrderStatusQuery())).Entities,
                Shippers = (await ShippersContext.LoadAsync(ShippersContext.GetShippersQuery())).Entities,
                PaymentTypes = (await PaymentTypesContext.LoadAsync(PaymentTypesContext.GetPaymentTypesQuery())).Entities
            };

            var result = await OrderAddEditWindow.Show(order, OrderContext);
            if (result)
            {
                OrdersDataSource.Load();
            }
        }

        [RelayCommand]
        public async Task NewCustomer()
        {
            var result = await CustomerAddEditWindow.Show(new Customers
            {
                IsEditMode = true,
                CountryCodes = CountryCodes,
                CountryCode = CountryCodes.FirstOrDefault().CountryCodeID,
                BirthDateUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }, CustomersContext);

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
            SelectedOrder.Statuses = (await OrderStatusContext.LoadAsync(OrderStatusContext.GetOrderStatusQuery())).Entities;
            SelectedOrder.Shippers = (await ShippersContext.LoadAsync(ShippersContext.GetShippersQuery())).Entities;
            SelectedOrder.PaymentTypes = (await PaymentTypesContext.LoadAsync(PaymentTypesContext.GetPaymentTypesQuery())).Entities;

            await OrderAddEditWindow.Show(SelectedOrder, OrderContext);
        }

        [RelayCommand]
        public void Delete(Customers customer)
        {
            if (customer == null)
                return;

            if (CustomersContext.Customers.CanRemove)
            {
                var result = MessageBox.Show("Are you sure to delete the customer and all orders belong that customer?", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK)
                    return;

                CustomersContext.Customers.Remove(customer);
                CustomersContext.SubmitChanges(OnDeleteSubmitCompleted, null);
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

        #region Properties
        [ObservableProperty]
        public bool ordersTabSelected;
        [ObservableProperty]
        public int selectedDetailsTabIndex;

        [ObservableProperty]
        public DomainDataSource ordersDataSource;
        [ObservableProperty]
        public OrderContext orderContext;

        [ObservableProperty]
        public DomainDataSource customersDataSource;
        [ObservableProperty]
        public CustomersContext customersContext;

        [ObservableProperty]
        public CountryCodesContext countryCodesContext;
        [ObservableProperty]
        public OrderStatusContext orderStatusContext;
        [ObservableProperty]
        public ShippersContext shippersContext;
        [ObservableProperty]
        public PaymentTypeContext paymentTypesContext;

        [ObservableProperty]
        public IEnumerable<CountryCodes> countryCodes;

        [ObservableProperty]
        public Customers selectedCustomer;
        [ObservableProperty]
        public bool anySelectedCustomer;
        [ObservableProperty]
        public string searchText;

        [ObservableProperty]
        public Orders selectedOrder;
        [ObservableProperty]
        public string searchOrderText;
        #endregion
    }
}