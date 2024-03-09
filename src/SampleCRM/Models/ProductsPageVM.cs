using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenRiaServices.DomainServices.Client;
using SampleCRM.Web.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SampleCRM.Web.Models
{
    public partial class ProductsPageVM : ObservableObject
    {
        #region Properties
        private readonly ProductsContext _productsContext = new();
        private readonly CategoryContext _categoryContext = new();

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

        [ObservableProperty]
        private IEnumerable<Models.Category> categoriesCombo;

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
                if (SelectedProduct.CategoriesCombo == null)
                    SelectedProduct.CategoriesCombo = CategoriesCombo;

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
                await AsyncHelper.RunAsync(LoadProducts);
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

        #region Commands
        [RelayCommand]
        public async Task Initialize()
        {
            await AsyncHelper.RunAsync(LoadCategoriesCombo);
            await AsyncHelper.RunAsync(LoadProducts);
        }

        [RelayCommand]
        public async Task LoadProducts()
        {
            var query = _productsContext.GetProductsQuery(SearchText).OrderBy(c => c.Name);
            var result = await _productsContext.LoadAsync(query);

            ProdcutsView = new PagedCollectionView(result.Entities);

            foreach (var product in result.Entities)
                product.CategoriesCombo = CategoriesCombo;
        }

        private async Task LoadCategoriesCombo() => CategoriesCombo = (await _categoryContext.LoadAsync(_categoryContext.GetCategoriesQuery())).Entities;

        [RelayCommand]
        public async Task NewProduct()
        {
            var result = await ProductsAddEditWindow.Show(new Product
            {
                CategoriesCombo = CategoriesCombo,
                CreatedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LastModifiedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }, _productsContext);

            if (result)
                await AsyncHelper.RunAsync(LoadProducts);
        }

        [RelayCommand]
        public async Task SearchCancel()
        {
            SearchText = string.Empty;
            await AsyncHelper.RunAsync(LoadProducts);
        }

        #endregion
    }
}