using System.Threading.Tasks;
using System.Windows;

namespace SampleCRM.Web.Views
{
    public partial class OrderItemAddEditWindow : BaseChildWindow
    {
        private OrderItemsContext _context;

        public static async Task<bool> Show(Models.OrderItems orderItem, OrderItemsContext context)
        {
            var window = new OrderItemAddEditWindow(orderItem, context);
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

