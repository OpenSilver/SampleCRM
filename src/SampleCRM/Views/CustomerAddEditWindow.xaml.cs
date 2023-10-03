using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public partial class CustomerAddEditWindow : ChildWindow
    {
        public static void Show(Models.Customers customer)
        {
            var window = new CustomerAddEditWindow(customer);
            window.Show();
        }

        public CustomerAddEditWindow()
        {
            InitializeComponent();
        }

        public CustomerAddEditWindow(Models.Customers customer)
            : this()
        {
            customerAddEditView.CustomerViewModel = customer;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}