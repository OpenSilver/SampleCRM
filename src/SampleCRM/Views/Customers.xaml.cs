using SampleCRM.Web.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SampleCRM.Web.Views
{
    public partial class Customers : BasePage
    {
        private readonly CustomersPageVM _vm = new();

        public Customers()
        {
            InitializeComponent();

            DataContext = _vm;

            _vm.OrdersDataSource = ordersDataSource;
            _vm.CustomersDataSource = customersDataSource;
            _vm.OrderContext = ordersDataSource.DomainContext as OrderContext;
            _vm.CustomersContext = customersDataSource.DomainContext as CustomersContext;
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
            await _vm.Initialize();
            base.OnNavigatedTo(e);
        }
    }
}