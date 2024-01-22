using OpenRiaServices.DomainServices.Client;
using OpenSilver.Controls;
using System;
using System.Windows;

namespace SampleCRM.Web.Views
{
    public partial class ProductAddEdit : BaseUserControl
    {
        public event EventHandler ProductDeleted;
        public event EventHandler ProductAdded;
        public event EventHandler ProductUpdated;

        public Models.Products ProductViewModel
        {
            get { return (Models.Products)GetValue(ProductViewModelProperty); }
            set { SetValue(ProductViewModelProperty, value); }
        }
        public static readonly DependencyProperty ProductViewModelProperty =
            DependencyProperty.Register("ProductViewModel", typeof(Models.Products), typeof(ProductAddEdit), 
                new PropertyMetadata(new PropertyChangedCallback((s,t) =>
                {
                    var page = s as ProductAddEdit;
                    var value = t.NewValue as Models.Products;
#if DEBUG
                    if (value.IsNew)
                        Console.WriteLine("ProductAddEdit, New Product Generated");
                    else
                        Console.WriteLine($"ProductAddEdit, ProductViewModel: {value.ProductID} {value.Name} selected");
#endif
                })));

        public ProductAddEdit()
        {
            InitializeComponent();
            DataContext = this;
        }

        public override void ArrangeLayout()
        {
            base.ArrangeLayout();
            grdNarrow.Visibility = BoolToVisibilityConverter.Convert(IsMobileUI);
            grdWide.Visibility = BoolToVisibilityConverter.Convert(!IsMobileUI);
        }

        private async void btnEditPicture_Click(object sender, RoutedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("btnEditPicture_Click");
#endif
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = _imageFileExtFilter;

            var result = await fileDialog.ShowDialogAsync();
            if (!result.GetValueOrDefault())
                return;

            var imageFile = fileDialog.File;
            if (imageFile.Length < 1)
                return;

#if DEBUG
            Console.WriteLine($"imageFile: {imageFile.Name}, {imageFile.Length} bytes");
#endif

            using (var fileStream = imageFile.OpenRead())
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                //await fileStream.ReadAsync(buffer, 0, buffer.Length);
                ProductViewModel.Picture = buffer;
#if DEBUG
                Console.WriteLine($"Byte buffer set to ProductViewModel.Picture");
#endif
            }
        }

        public void Save(ProductsContext context)
        {
            if ((ProductViewModel.IsNew && context.Products.CanAdd) || context.Products.CanEdit)
            {
#if DEBUG
                if (ProductViewModel.IsNew)
                    Console.WriteLine("ProductAddEdit, Save, New Product Submiting Changes");
                else
                    Console.WriteLine($"ProductAddEdit, Update, Product Id: {ProductViewModel.ProductID}");
#endif

                if (ProductViewModel.IsNew)
                {
                    context.Products.Add(ProductViewModel);
                }

                context.SubmitChanges(OnAddSubmitCompleted, null);
            }
            else
            {
                throw new AccessViolationException("RIA Service Add / Edit Entity for Product Context is denied");
            }
        }

        public void Delete(ProductsContext context)
        {
            if (context.Products.CanRemove)
            {
#if DEBUG
                Console.WriteLine($"ProductAddEdit, Delete, Product Id: {ProductViewModel.ProductID}");
#endif
                context.Products.Remove(ProductViewModel);
                context.SubmitChanges(OnDeleteSubmitCompleted, null);
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
                ErrorWindow.Show(string.Format("Submit Failed: {0}", so.Error.Message));
#if DEBUG
                Console.WriteLine(string.Format("Submit Failed: {0}", so.Error.StackTrace));
#endif
                so.MarkErrorAsHandled();
            }
            else
            {
#if DEBUG
                Console.WriteLine($"ProductAddEdit, OnDeleteSubmitCompleted, Product Id: {ProductViewModel.ProductID}");
#endif
                if (ProductDeleted != null)
                    ProductDeleted(this, new EventArgs());
            }
        }

        private void OnAddSubmitCompleted(SubmitOperation so)
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
#if DEBUG
                if (ProductViewModel.IsNew)
                    Console.WriteLine("ProductAddEdit, OnAddSubmitCompleted, New Product");
                else
                    Console.WriteLine($"ProductAddEdit, OnAddSubmitCompleted, Product Id: {ProductViewModel.ProductID}");
#endif
                if (ProductViewModel.IsNew)
                {
                    if (ProductAdded != null)
                        ProductAdded(this, new EventArgs());
                }
                else
                {
                    if (ProductUpdated != null)
                        ProductUpdated(this, new EventArgs());
                }
            }
        }
    }
}
