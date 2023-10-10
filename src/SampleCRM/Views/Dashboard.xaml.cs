using OpenRiaServices.DomainServices.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SampleCRM.Web.Views
{
    public partial class Dashboard : BasePage
    {
        private CustomersContext _customersContext = new CustomersContext();
        private ProductsContext _productsContext = new ProductsContext();
        private OrderContext _orderContext = new OrderContext();

        public const int ROW_LIMIT = 5;

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

        public Dashboard()
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
            await LoadCustomers();
            await LoadOrders();
            await LoadProducts();
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
