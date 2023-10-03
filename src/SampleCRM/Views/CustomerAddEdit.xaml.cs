using OpenSilver.Controls;
using System;
using System.Windows;

namespace SampleCRM.Web.Views
{
    public partial class CustomerAddEdit : BaseUserControl
    {
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

        private void formCustomer_EditEnded(object sender, System.Windows.Controls.DataFormEditEndedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEdit_Checked(object sender, RoutedEventArgs e)
        {
            formCustomer.BeginEdit();
        }

        private void btnEdit_Unchecked(object sender, RoutedEventArgs e)
        {
            formCustomer.CancelEdit();
        }
    }
}