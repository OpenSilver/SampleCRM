using CommunityToolkit.Mvvm.ComponentModel;
using OpenRiaServices.Controls;
using OpenRiaServices.DomainServices.Client;
using SampleCRM.Web.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Navigation;

namespace SampleCRM.Web.Models
{
    public partial class CustomersPageVM : ObservableObject
    {
        public CustomersPageVM()
        {
            PropertyChanged += CustomersPageVM_PropertyChanged;

            countryCodesContext = new CountryCodesContext();
            orderStatusContext = new OrderStatusContext();
            shippersContext = new ShippersContext();
            paymentTypesContext = new PaymentTypeContext();
        }

        private void CustomersPageVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
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
                case nameof(SearchText):
                    {
                        var value = SearchText;
                        var searchParam = CustomersDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "search");
                        searchParam.Value = value;
                        CustomersDataSource.Load();
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
                case nameof(SearchOrderText):
                    {
                        var value = SearchOrderText;
                        var searchParam = OrdersDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "search");
                        searchParam.Value = value;
                        OrdersDataSource.Load();
                    }
                    break;
                case nameof(CustomersDataSource):
                    {
                        CustomersDataSource.LoadedData += customersDataSource_LoadedData;
                    }
                    break;
                default:
                    break;
            }
        }

        private void customersDataSource_LoadedData(object sender, OpenRiaServices.Controls.LoadedDataEventArgs e)
        {
            var customers = e.Entities.Cast<Models.Customers>();
            foreach (var customer in customers)
                customer.CountryCodes = CountryCodes;
        }

        [ObservableProperty]
        public bool ordersTabSelected;

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
        public IEnumerable<Models.CountryCodes> countryCodes;

        [ObservableProperty]
        public Models.Customers selectedCustomer;
        [ObservableProperty]
        public bool anySelectedCustomer;
        [ObservableProperty]
        public string searchText;

        [ObservableProperty]
        public Models.Orders selectedOrder;
        [ObservableProperty]
        public string searchOrderText;

        public async Task Navigated()
        {
            await AsyncHelper.RunAsync(LoadCountryCodes);
            CustomersDataSource.Load();
        }

        private async Task LoadCountryCodes() => CountryCodes = (await CountryCodesContext.LoadAsync(CountryCodesContext.GetCountriesQuery())).Entities;

        public void formCustomer_EditEnded(object sender, DataFormEditEndedEventArgs e)
        {
            if (e.EditAction == DataFormEditAction.Commit)
            {
                CustomersContext.SubmitChanges(OnFormCustomerSubmitCompleted, null);
            }
            else if (e.EditAction == DataFormEditAction.Cancel)
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

        public void tcDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //_ordersTabSelected = e.AddedItems.Count > 0 && e.AddedItems.Contains(tbOrders);
            if (OrdersTabSelected && SelectedCustomer != null)
            {
                var customerId = SelectedCustomer.CustomerID;
                var customerParam = OrdersDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "customerId");
                customerParam.Value = customerId;
                OrdersDataSource.Load();
            }
        }

        public void grdOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        public void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure to delete the customer and all orders belong that customer?", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK)
                return;

            if (CustomersContext.Customers.CanRemove)
            {
                CustomersContext.Customers.Remove(SelectedCustomer);
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

        public async void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            var result = await CustomerAddEditWindow.Show(new Models.Customers
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

        public async void btnShowOrder_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOrder == null)
                return;

            SelectedOrder.CountryCodes = CountryCodes;
            SelectedOrder.Customer = SelectedCustomer;
            SelectedOrder.Statuses = (await OrderStatusContext.LoadAsync(OrderStatusContext.GetOrderStatusQuery())).Entities;
            SelectedOrder.Shippers = (await ShippersContext.LoadAsync(ShippersContext.GetShippersQuery())).Entities;
            SelectedOrder.PaymentTypes = (await PaymentTypesContext.LoadAsync(PaymentTypesContext.GetPaymentTypesQuery())).Entities;

            await OrderAddEditWindow.Show(SelectedOrder, OrderContext);
        }

        public async void btnNewOrder_Click(object sender, RoutedEventArgs e)
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
    }
}