using SampleCRM.Web;
using SampleCRM.Web.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SampleCRM
{
    public partial class MainPage : BaseUserControl, IBusyCapablePage
    {
        private CountContext _countContext = new CountContext();

        private int _categoriesCount;
        public int CategoriesCount
        {
            get { return _categoriesCount; }
            set
            {
                if (_categoriesCount != value)
                {
                    _categoriesCount = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _customersCount;
        public int CustomersCount
        {
            get { return _customersCount; }
            set
            {
                if (_customersCount != value)
                {
                    _customersCount = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _ordersCount;
        public int OrdersCount
        {
            get { return _ordersCount; }
            set
            {
                if (_ordersCount != value)
                {
                    _ordersCount = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _productsCount;
        public int ProductsCount
        {
            get { return _productsCount; }
            set
            {
                if (_productsCount != value)
                {
                    _productsCount = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainPage()
        {
            RetryOnExceptionHelper.ContentPage = this;
            InitializeComponent();
            DataContext = this;
            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await RetryOnExceptionHelper.RetryAsync(LoadCounts);
        }

        private async Task LoadCounts()
        {
            var query = _countContext.GetAllCountsQuery();
            var op = await _countContext.LoadAsync(query);
            var counts = op.Entities.FirstOrDefault();
            CategoriesCount = counts.CategoryCount;
            CustomersCount = counts.CustomerCount;
            OrdersCount = counts.OrderCount;
            ProductsCount = counts.ProductCount;
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            highlightLinks(e, LinksStackPanel.Children);
            highlightLinks(e, MobileLinksStackPanel.Children);
        }

        private void highlightLinks(NavigationEventArgs e, UIElementCollection links)
        {
            foreach (var child in links)
            {
                var hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    else
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                }
            }
        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ErrorWindow.Show(e.Uri);
        }

        public void MakeBusy(bool isBusy)
        {
            IsBusy = isBusy;
        }
    }
}