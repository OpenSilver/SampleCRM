using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenRiaServices.DomainServices.Client;
using SampleCRM.Web.Views;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SampleCRM.Web.Models
{
    public partial class ProductsPageVM : ObservableObject
    {
        #region Properties
        private readonly ProductsContext _productsContext = new();

        private ICollectionView _productsView;
        public ICollectionView ProdcutsView
        {
            get => _productsView;
            private set => SetProperty(ref _productsView, value);
        }

        [ObservableProperty]
        private Product selectedProduct;

        [ObservableProperty]
        private string searchText = string.Empty;

        #endregion

        #region Handle changing properties

        private bool isUserSelectedProduct = false;

        partial void OnSelectedProductChanging(Product oldValue, Product newValue)
        {
            isUserSelectedProduct = oldValue != newValue && oldValue != null;
        }

        partial void OnSelectedProductChanged(Product value)
        {
#if DEBUG
            Console.WriteLine($"Products, Product: {value.ProductID} {value.Name} selected");
#endif
            if (SelectedProduct != null && isUserSelectedProduct)
            {
                if (SelectedProduct.Picture == null || SelectedProduct.Picture.Length < 2)
                    _productsContext.GetProductPicture(SelectedProduct.ProductID, GetProductPicture_Completed, null);
                else
                    Task.Run(showEditProdocutWindow);
            }
        }

        private async Task showEditProdocutWindow()
        {
            var result = await ProductsAddEditWindow.Show(SelectedProduct, _productsContext);
            if (result)
                LoadProducts();
        }

        private async void GetProductPicture_Completed(InvokeOperation<byte[]> operation)
        {
            if (operation.IsComplete && !operation.HasError)
            {
                SelectedProduct.Picture = operation.Value;
                await showEditProdocutWindow();
            }
            else
            {
                ErrorWindow.Show(operation.Error);
            }
        }

        #endregion

        public ProductsPageVM()
        {
            LoadProducts();
        }

        #region Commands
        [RelayCommand]
        public void LoadProducts()
        {
            var query = _productsContext.GetProductsQuery(SearchText).OrderBy(c => c.Name);
            _productsContext.Load(query, result =>
            {
                ProdcutsView = new PagedCollectionView(result.Entities);
            });
        }

        [RelayCommand]
        public async Task NewProduct()
        {
            var result = await ProductsAddEditWindow.Show(new Product
            {
                CreatedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LastModifiedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }, _productsContext);

            if (result)
                LoadProducts();
        }

        [RelayCommand]
        public void SearchCancel()
        {
            SearchText = string.Empty;
            LoadProducts();
        }

        #endregion
    }
}