using OpenRiaServices.DomainServices.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SampleCRM.Web.Views
{
    public partial class Dashboard : BasePage
    {
        #region Contexts
        private CustomersContext _customersContext = new CustomersContext();
        private ProductsContext _productsContext = new ProductsContext();
        private OrderContext _orderContext = new OrderContext();
        #endregion

        public const int ROW_LIMIT = 5;

        #region DataContext Properties
        private IEnumerable<Models.Customers> _customers;
        public IEnumerable<Models.Customers> Customers
        {
            get { return _customers; }
            set
            {
                if (_customers != value)
                {
                    _customers = value;
                    OnPropertyChanged();
                }
            }
        }

        private IEnumerable<Models.Orders> _orders;
        public IEnumerable<Models.Orders> Orders
        {
            get { return _orders; }
            set
            {
                if (_orders != value)
                {
                    _orders = value;
                    OnPropertyChanged();
                }
            }
        }

        private IEnumerable<Models.Products> _products;
        public IEnumerable<Models.Products> Products
        {
            get { return _products; }
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        public Dashboard()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await AsyncHelper.RunAsync(LoadElements);
        }

        protected override void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            base.OnSizeChanged(sender, e);

            if (IsMobileUI)
            {
                grdDashboard.ColumnDefinitions.Clear();
                grdDashboard.ColumnDefinitions.Add(new ColumnDefinition());

                Grid.SetColumn(cntCustomers, 0);
                Grid.SetColumn(cntOrders, 0);
                Grid.SetColumn(cntProducts, 0);

                grdDashboard.RowDefinitions.Clear();
                for (int i = 0; i < 3; i++)
                    grdDashboard.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                Grid.SetRow(cntCustomers, 0);
                Grid.SetRow(cntOrders, 1);
                Grid.SetRow(cntProducts, 2);
            }
            else
            {
                grdDashboard.ColumnDefinitions.Clear();
                grdDashboard.ColumnDefinitions.Add(new ColumnDefinition());
                grdDashboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Pixel) });
                grdDashboard.ColumnDefinitions.Add(new ColumnDefinition());

                Grid.SetColumn(cntCustomers, 0);
                Grid.SetColumn(cntOrders, 0);
                Grid.SetColumn(cntProducts, 2);

                grdDashboard.RowDefinitions.Clear();
                grdDashboard.RowDefinitions.Add(new RowDefinition { MinHeight = 300 });
                grdDashboard.RowDefinitions.Add(new RowDefinition { MinHeight = 400 });
                grdDashboard.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                Grid.SetRow(cntCustomers, 0);
                Grid.SetRow(cntOrders, 1);
                Grid.SetRow(cntProducts, 1);
            }
        }

        private async Task LoadElements()
        {
            await AsyncHelper.RunAsync(LoadCustomers);
            await AsyncHelper.RunAsync(LoadOrders);
            await AsyncHelper.RunAsync(LoadProducts);
        }

        private async Task LoadCustomers()
        {
            var q = _customersContext.GetLatestCustomersQuery(ROW_LIMIT);
            var o = await _customersContext.LoadAsync(q);
            Customers = o.Entities;
        }

        private async Task LoadOrders()
        {
            var q = _orderContext.GetLatestOrdersQuery(ROW_LIMIT);
            var o = await _orderContext.LoadAsync(q);
            Orders = o.Entities;
        }

        private async Task LoadProducts()
        {
            var q = _productsContext.GetTopSaleProductsQuery(ROW_LIMIT);
            //var q = _productsContext.GetProductsQuery().Take(ROW_LIMIT);
            var o = await _productsContext.LoadAsync(q);
            Products = o.Entities;
        }
    }
}
