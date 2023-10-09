using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;

namespace SampleCRM.Web.Views
{
    public partial class Products : BasePage
    {
        private ProductsContext _productsContext = new ProductsContext();

        private IEnumerable<Models.Products> _productsCollection;
        public IEnumerable<Models.Products> ProductsCollection
        {
            get { return _productsCollection; }
            set
            {
                if (_productsCollection != value)
                {
                    _productsCollection = value;
                    OnPropertyChanged();
                    OnPropertyChanged("FilteredProductsCollection");
                    SelectedProduct = FilteredProductsCollection.FirstOrDefault();

                }
            }
        }

        public IEnumerable<Models.Products> FilteredProductsCollection
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_searchText))
                {
                    return _productsCollection;
                }
                else
                {
                    return _productsCollection.Where(x => x.Name.Contains(_searchText.ToLowerInvariant()));
                }
            }
        }

        private Models.Products _selectedProduct;
        public Models.Products SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged();
#if DEBUG
                    Console.WriteLine($"Products, Product: {value.ProductID} {value.Name} selected");
#endif
                }
            }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    OnPropertyChanged("FilteredProductsCollection");
                }
            }
        }

        public Products()
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
            var query = _productsContext.GetProductsQuery();
            var op = await _productsContext.LoadAsync(query);
            ProductsCollection = op.Entities;
#if DEBUG
            Console.WriteLine("Products Collection:" + ProductsCollection.Count());
            foreach (var item in ProductsCollection)
            {
                Console.WriteLine("Product Id:" + item.ProductID);
                Console.WriteLine("Product Name:" + item.Name);
            }
#endif
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e) { }

        private void btnSearchCancel_Click(object sender, RoutedEventArgs e)
        {
            SearchText = string.Empty;
        }
    }
}
