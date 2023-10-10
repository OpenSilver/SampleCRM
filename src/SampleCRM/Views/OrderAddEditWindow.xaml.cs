using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public partial class OrderAddEditWindow : ChildWindow
    {
        private OrderContext _context;

        private const double windowSizeMult = .85;

        public static async Task<bool> Show(Models.Orders order, OrderContext context)
        {
            var window = new OrderAddEditWindow(order, context);
            window.Width = Application.Current.MainWindow.ActualWidth * windowSizeMult;
            window.Height = Application.Current.MainWindow.ActualHeight * windowSizeMult;
            await window.ShowAndWait();
            return window.DialogResult.GetValueOrDefault(false);
        }

        public OrderAddEditWindow()
        {
            InitializeComponent();
        }

        public OrderAddEditWindow(Models.Orders orderItem, OrderContext context)
            : this()
        {
            _context = context;
            orderAddEditView.Order = orderItem;
            Title = orderItem.IsNew ? "Add Order" : "Edit Order";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            orderAddEditView.Save(_context);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void orderAddEditView_OrderAdded(object sender, System.EventArgs e)
        {
            DialogResult = true;
        }

        private void orderAddEditView_OrderDeleted(object sender, System.EventArgs e)
        {
            DialogResult = true;
        }
    }
}

