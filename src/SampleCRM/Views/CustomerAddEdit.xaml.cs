using System;
using System.Windows;

namespace SampleCRM.Web.Views
{
    public partial class CustomerAddEdit : BaseUserControl
    {
        private Models.Customers _customerViewModel=new Models.Customers();
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

        private void btnEditPicture_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}