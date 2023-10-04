using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public partial class CustomerAddEditWindow : ChildWindow
    {
        private const double windowSizeMult = .85;

        public static async Task<bool> Show(Models.Customers customer)
        {
            var window = new CustomerAddEditWindow(customer);
            window.Width = Application.Current.MainWindow.ActualWidth * windowSizeMult;
            window.Height = Application.Current.MainWindow.ActualHeight * windowSizeMult;
            await window.ShowAndWait();
            return window.DialogResult.GetValueOrDefault(false);
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
            customerAddEditView.Save();
            //DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void customerAddEditView_CustomerDeleted(object sender, System.EventArgs e)
        {
            DialogResult = true;
        }

        private void customerAddEditView_CustomerAdded(object sender, System.EventArgs e)
        {
            DialogResult = true;
        }
    }
}