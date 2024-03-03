using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SampleCRM.Web.Views
{
    public partial class Products : BasePage
    {
        #region Contexts
        private SampleCRMContext _productsContext => productsDataSource.DomainContext as SampleCRMContext;
        private SampleCRMContext _categoryContext = new SampleCRMContext();
        #endregion

        #region Properties
        public Models.Product SelectedProduct
        {
            get { return (Models.Product)GetValue(SelectedProductProperty); }
            set { SetValue(SelectedProductProperty, value); }
        }
        public static readonly DependencyProperty SelectedProductProperty =
            DependencyProperty.Register("SelectedProduct", typeof(Models.Product), typeof(Products),
                new PropertyMetadata(
                    new PropertyChangedCallback((s, t) =>
                    {
                        var value = t.NewValue as Models.Product;
                        //var page = (Products)s;
                        if (value != null)
                        {
#if DEBUG
                            Console.WriteLine($"Products, Product: {value.ProductID} {value.Name} selected");
#endif
                        }

                    })));

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }
        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(Products),
                new PropertyMetadata(
                    new PropertyChangedCallback((s, t) =>
                    {
                        var value = t.NewValue as string;
                        var page = s as Products;
                        var searchParam = page.productsDataSource.QueryParameters.FirstOrDefault(x => x.ParameterName == "search");
                        searchParam.Value = value;
                        page.productsDataSource.Load();
#if DEBUG
                        Console.WriteLine($"SearchText Changed {value}");
#endif
                    })));

        public IEnumerable<Models.Category> CategoriesCombo
        {
            get { return (IEnumerable<Models.Category>)GetValue(CategoriesComboProperty); }
            set { SetValue(CategoriesComboProperty, value); }
        }
        public static readonly DependencyProperty CategoriesComboProperty =
            DependencyProperty.Register("CategoriesCombo", typeof(IEnumerable<Models.Category>), typeof(Products), new PropertyMetadata(null));


        #endregion

        public Products()
        {
            InitializeComponent();
            DataContext = this;
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
            }
            else
            {
                grdHead.ColumnDefinitions[2].Width = new GridLength(405, GridUnitType.Pixel);

                Grid.SetColumn(grdSearch, 2);
                Grid.SetRow(grdSearch, 0);
                grdSearch.Margin = new Thickness();
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await AsyncHelper.RunAsync(LoadCategoriesCombo);
            productsDataSource.Load();
        }

        private void productsDataSource_LoadingData(object sender, OpenRiaServices.Controls.LoadingDataEventArgs e)
        {
            lstProducts.SelectionChanged -= lstProducts_SelectionChanged;
        }
        private void productsDataSource_LoadedData(object sender, OpenRiaServices.Controls.LoadedDataEventArgs e)
        {
            if (e.HasError)
                return;

            var products = e.Entities.Cast<Models.Product>();
            foreach (var product in products)
                product.CategoriesCombo = CategoriesCombo;
            
            lstProducts.SelectionChanged += lstProducts_SelectionChanged;
        }

        private async Task LoadCategoriesCombo() => CategoriesCombo = (await _categoryContext.LoadAsync(_categoryContext.GetCategoriesQuery())).Entities;

        private async void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var result = await ProductsAddEditWindow.Show(new Models.Product
            {
                CategoriesCombo = CategoriesCombo,
                CreatedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LastModifiedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }, _productsContext);

            if (result)
            {
                NavigationService.Refresh();
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e) { }

        private void btnSearchCancel_Click(object sender, RoutedEventArgs e)
        {
            SearchText = string.Empty;
        }

        private async void lstProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("lstProducts_SelectionChanged, {0} Items Added, {1} Items Removed", e.AddedItems.Count, e.RemovedItems.Count);
#endif

            if (e.RemovedItems.Count < 1)
            {
                SelectedProduct = null;
                return;
            }

            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Models.Product)
            {
                SelectedProduct = e.AddedItems[0] as Models.Product;
                if (SelectedProduct != null)
                {
                    if (SelectedProduct.CategoriesCombo == null)
                        SelectedProduct.CategoriesCombo = CategoriesCombo;

                    if (SelectedProduct.Picture == null || SelectedProduct.Picture.Length < 2)
                        _productsContext.GetProductPicture(SelectedProduct.ProductID, GetProductPicture_Completed, null);
                    else
                        await showEditProdocutWindow();
                }
            }
        }

        private async Task showEditProdocutWindow()
        {
            var result = await ProductsAddEditWindow.Show(SelectedProduct, _productsContext);
            if (result)
            {
                NavigationService.Refresh();
            }
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

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSearch.Focus();
            }
        }
    }
}