using OpenRiaServices.DomainServices.Client;
using OpenSilver.Controls;
using System;
using System.Windows;

namespace SampleCRM.Web.Views
{
    public partial class CustomerAddEdit : BaseUserControl
    {
        public event EventHandler CustomerDeleted;
        public event EventHandler CustomerAdded;

        private Models.Customers _customerViewModel = new Models.Customers();
        public Models.Customers CustomerViewModel
        {
            get { return _customerViewModel; }
            set
            {
                if (_customerViewModel != value)
                {
                    _customerViewModel = value;
                    OnPropertyChanged();
#if DEBUG
                    Console.WriteLine($"CustomerAddEdit, Customer: {value.FirstName} selected");
#endif
                }
            }
        }

        public CustomerAddEdit()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void btnEditPicture_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "All Images Files (*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif)|*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif" +
            "|PNG Portable Network Graphics (*.png)|*.png" +
            "|JPEG File Interchange Format (*.jpg *.jpeg *jfif)|*.jpg;*.jpeg;*.jfif" +
            "|BMP Windows Bitmap (*.bmp)|*.bmp" +
            "|TIF Tagged Imaged File Format (*.tif *.tiff)|*.tif;*.tiff" +
            "|GIF Graphics Interchange Format (*.gif)|*.gif";

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

        private void formPersonalInfo_EditEnded(object sender, System.Windows.Controls.DataFormEditEndedEventArgs e)
        {

        }

        private void formAddress_EditEnded(object sender, System.Windows.Controls.DataFormEditEndedEventArgs e)
        {

        }

        private void formDemographic_EditEnded(object sender, System.Windows.Controls.DataFormEditEndedEventArgs e)
        {

        }

        public void Save(CustomersContext customersContext)
        {
            if (customersContext.Customers.CanAdd)
            {
                if (!formPersonalInfo.CommitEdit())
                {
                    ErrorWindow.Show("Invalid Personal Info");
                    return;
                }

                if (!formAddress.CommitEdit())
                {
                    ErrorWindow.Show("Invalid Address Info");
                    return;
                }

                if (!formDemographic.CommitEdit())
                {
                    ErrorWindow.Show("Invalid Demographic");
                    return;
                }

                customersContext.Customers.Add(CustomerViewModel);
                customersContext.SubmitChanges(OnAddSubmitCompleted, null);
            }
            else
            {
                throw new AccessViolationException("RIA Service Add Entity for Customer Context is denied");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var customersContext = new CustomersContext();
            if (customersContext.Customers.CanRemove)
            {
                customersContext.Customers.Remove(CustomerViewModel);
                customersContext.SubmitChanges(OnDeleteSubmitCompleted, null);
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
                MessageBox.Show(string.Format("Submit Failed: {0}", so.Error.Message));
                Console.WriteLine(string.Format("Submit Failed: {0}", so.Error.StackTrace));
                so.MarkErrorAsHandled();
            }
            else
            {
                if (CustomerDeleted != null)
                    CustomerDeleted(this, new EventArgs());
            }
        }

        private void OnAddSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                MessageBox.Show(string.Format("Submit Failed: {0}", so.Error.Message));
                Console.WriteLine(string.Format("Submit Failed: {0}", so.Error.StackTrace));
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