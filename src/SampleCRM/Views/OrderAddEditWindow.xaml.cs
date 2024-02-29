using System.Threading.Tasks;
using System.Windows;

namespace SampleCRM.Web.Views
{
    public partial class OrderAddEditWindow : BaseChildWindow
    {
        private OrderContext _context;

        public static async Task<bool> Show(Models.Order order, OrderContext context)
        {
            var window = new OrderAddEditWindow(order, context);
            await window.ShowAndWait();
            return window.DialogResult.GetValueOrDefault(false);
        }

        public OrderAddEditWindow()
        {
            InitializeComponent();
        }

        public OrderAddEditWindow(Models.Order order, OrderContext context)
            : this()
        {
            DataContext = order;
            _context = context;
            orderAddEditView.Order = order;
            Title = order.IsNew ? "Add Order" : "Edit Order";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            orderAddEditView.Save(_context);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _context.RejectChanges();
            DialogResult = false;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            orderAddEditView.Delete(_context);
        }

        private void orderAddEditView_OrderAdded(object sender, System.EventArgs e)
        {
            DialogResult = true;
        }

        private void orderAddEditView_OrderDeleted(object sender, System.EventArgs e)
        {
            DialogResult = true;
        }

        private void orderAddEditView_OrderUpdated(object sender, System.EventArgs e)
        {
            DialogResult = true;
        }
    }
}

