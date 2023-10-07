using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public partial class OrderItemAddEditWindow : ChildWindow
    {
        private OrderItemsContext _context;

        private const double windowSizeMult = .85;

        public static async Task<bool> Show(Models.OrderItems orderItem, OrderItemsContext context)
        {
            var window = new OrderItemAddEditWindow(orderItem, context);
            window.Width = Application.Current.MainWindow.ActualWidth * windowSizeMult;
            window.Height = Application.Current.MainWindow.ActualHeight * windowSizeMult;
            await window.ShowAndWait();
            return window.DialogResult.GetValueOrDefault(false);
        }

        public OrderItemAddEditWindow()
        {
            InitializeComponent();
        }

        public OrderItemAddEditWindow(Models.OrderItems orderItem, OrderItemsContext context)
            : this()
        {
            _context = context;
            orderItemAddEditView.Item = orderItem;
            Title = orderItem.IsNew ? "Add Order Item" : "Edit Order Item";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            orderItemAddEditView.Save(_context);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void orderItemAddEdit_ItemAdded(object sender, System.EventArgs e)
        {
            DialogResult = true;
        }

        private void orderItemAddEdit_ItemDeleted(object sender, System.EventArgs e)
        {
            DialogResult = true;
        }
    }
}

