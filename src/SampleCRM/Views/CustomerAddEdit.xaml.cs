using OpenRiaServices.DomainServices.Client;
using OpenSilver.Controls;
using System;
using System.Windows;

namespace SampleCRM.Web.Views
{
    public partial class CustomerAddEdit : BaseUserControl
    {
        public event EventHandler CustomerAdded;

        public Models.Customers CustomerViewModel
        {
            get { return (Models.Customers)GetValue(CustomerViewModelProperty); }
            set { SetValue(CustomerViewModelProperty, value); }
        }
        public static readonly DependencyProperty CustomerViewModelProperty =
            DependencyProperty.Register("CustomerViewModel", typeof(Models.Customers), typeof(CustomerAddEdit),
                new PropertyMetadata(new PropertyChangedCallback((s, t) =>
                {
                    var page = s as CustomerAddEdit;
                    var value = t.NewValue as Models.Customers;
#if DEBUG
                    Console.WriteLine($"CustomerAddEdit, Customer: {value.FirstName} selected");
#endif
                })));

        public CustomerAddEdit()
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
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = _imageFileExtFilter;

            var result = await fileDialog.ShowDialogAsync();
            if (!result.GetValueOrDefault())
                return;

            var imageFile = fileDialog.File;
            if (imageFile.Length < 1)
                return;

            using (var fileStream = imageFile.OpenRead())
            {
                byte[] buffer = new byte[fileStream.Length];
                await fileStream.ReadAsync(buffer, 0, buffer.Length);
                CustomerViewModel.Picture = buffer;
            }
        }

        public void Save(CustomersContext customersContext)
        {
            if (customersContext.Customers.CanAdd)
            {
                customersContext.Customers.Add(CustomerViewModel);
                customersContext.SubmitChanges(OnAddSubmitCompleted, null);
            }
            else
            {
                throw new AccessViolationException("RIA Service Add Entity for Customer Context is denied");
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
                if (CustomerAdded != null)
                    CustomerAdded(this, new EventArgs());
            }
        }
    }
}