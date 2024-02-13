using SampleCRM.Web.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SampleCRM.Web.Views
{
    public partial class Customers : BasePage
    {
        public Customers()
        {
            InitializeComponent();

            var vm = (LayoutRoot.DataContext as CustomersPageVM);
            vm.OrdersDataSource = ordersDataSource;
            vm.CustomersDataSource = customersDataSource;
            vm.OrderContext = ordersDataSource.DomainContext as OrderContext;
            vm.CustomersContext = customersDataSource.DomainContext as CustomersContext;
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
            if (LayoutRoot == null)
                return;
            if (LayoutRoot.DataContext == null)
                return;
            if (LayoutRoot.DataContext is not CustomersPageVM)
                return;

            await (LayoutRoot.DataContext as CustomersPageVM).Navigated();

        }

        private void btnSearchCancel_Click(object sender, RoutedEventArgs e)
        {
            if (LayoutRoot == null)
                return;
            if (LayoutRoot.DataContext == null)
                return;
            if (LayoutRoot.DataContext is not CustomersPageVM)
                return;

            (LayoutRoot.DataContext as CustomersPageVM).SearchText = string.Empty;
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void grdCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LayoutRoot == null)
                return;
            if (LayoutRoot.DataContext == null)
                return;
            if (LayoutRoot.DataContext is not CustomersPageVM)
                return;

            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Models.Customers)
            {
                (LayoutRoot.DataContext as CustomersPageVM).SelectedCustomer = e.AddedItems[0] as Models.Customers;
            }
            else
            {
                (LayoutRoot.DataContext as CustomersPageVM).SelectedCustomer = null;
            }

#if DEBUG
            Console.WriteLine("grdCustomers_SelectionChanged, {0} Items Added", e.AddedItems.Count);
            if (e.AddedItems.Count > 0)
                Console.WriteLine(e.AddedItems[0]);
#endif
        }

        private void formCustomer_EditEnded(object sender, DataFormEditEndedEventArgs e)
        {
            if (LayoutRoot == null)
                return;
            if (LayoutRoot.DataContext == null)
                return;
            if (LayoutRoot.DataContext is not CustomersPageVM)
                return;
            (LayoutRoot.DataContext as CustomersPageVM).formCustomer_EditEnded(sender, e);
        }

        private void btnOrderSearchCancel_Click(object sender, RoutedEventArgs e)
        {
            if (LayoutRoot == null)
                return;
            if (LayoutRoot.DataContext == null)
                return;
            if (LayoutRoot.DataContext is not CustomersPageVM)
                return;
            (LayoutRoot.DataContext as CustomersPageVM).SearchOrderText = string.Empty;
        }

        private void tcDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LayoutRoot == null)
                return;
            if (LayoutRoot.DataContext == null)
                return;
            if (LayoutRoot.DataContext is not CustomersPageVM)
                return;
            (LayoutRoot.DataContext as CustomersPageVM).OrdersTabSelected = e.AddedItems.Count > 0 && e.AddedItems.Contains(tbOrders);
            (LayoutRoot.DataContext as CustomersPageVM).tcDetails_SelectionChanged(sender, e);
        }

        private void grdOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LayoutRoot == null)
                return;
            if (LayoutRoot.DataContext == null)
                return;
            if (LayoutRoot.DataContext is not CustomersPageVM)
                return;
            (LayoutRoot.DataContext as CustomersPageVM).grdOrders_SelectionChanged(sender, e);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LayoutRoot == null)
                return;
            if (LayoutRoot.DataContext == null)
                return;
            if (LayoutRoot.DataContext is not CustomersPageVM)
                return;
            (LayoutRoot.DataContext as CustomersPageVM).btnDelete_Click(sender, e);
        }

        private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (LayoutRoot == null)
                return;
            if (LayoutRoot.DataContext == null)
                return;
            if (LayoutRoot.DataContext is not CustomersPageVM)
                return;
            (LayoutRoot.DataContext as CustomersPageVM).btnNewCustomer_Click(sender, e);
        }

        private void btnShowOrder_Click(object sender, RoutedEventArgs e)
        {
            if (LayoutRoot == null)
                return;
            if (LayoutRoot.DataContext == null)
                return;
            if (LayoutRoot.DataContext is not CustomersPageVM)
                return;
            (LayoutRoot.DataContext as CustomersPageVM).btnShowOrder_Click(sender, e);
        }
        private async void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            if (LayoutRoot == null)
                return;
            if (LayoutRoot.DataContext == null)
                return;
            if (LayoutRoot.DataContext is not CustomersPageVM)
                return;
            await (LayoutRoot.DataContext as CustomersPageVM).btnNewOrder_Click(sender, e);
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