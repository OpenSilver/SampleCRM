using OpenRiaServices.DomainServices.Client;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public partial class OrderAddEdit : BaseUserControl
    {
        public event EventHandler OrderDeleted;
        public event EventHandler OrderAdded;

        private Models.Orders _order = new Models.Orders();
        public Models.Orders Order
        {
            get { return _order; }
            set
            {
                if (_order != value)
                {
                    _order = value;
                    OnPropertyChanged();
#if DEBUG
                    Console.WriteLine($"OrderItemAddEdit, Item: {value.OrderID} selected");
#endif
                }
            }
        }

        public OrderAddEdit()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void Save(OrderContext context)
        {
            if (context.Orders.CanAdd && Order.IsNew || context.Orders.CanEdit)
            {
                if (!formCustomerInfo.CommitEdit())
                {
                    ErrorWindow.Show("Invalid Customer Info");
                    return;
                }

                if (!formOrderStatus.CommitEdit())
                {
                    ErrorWindow.Show("Invalid Order Status");
                    return;
                }

                if (Order.IsNew)
                {
                    context.Orders.Add(Order);
                }

                context.SubmitChanges(OnAddSubmitCompleted, null);
            }
            else
            {
                throw new AccessViolationException("RIA Service Add / Edit Order for Order Context is denied");
            }
        }

        private void OnDeleteSubmitCompleted(SubmitOperation so)
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
                if (OrderDeleted != null)
                    OrderDeleted(this, new EventArgs());
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
                if (OrderAdded != null)
                    OrderAdded(this, new EventArgs());
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var context = new OrderContext();
            if (context.Orders.CanRemove)
            {
                context.Orders.Remove(Order);
                context.SubmitChanges(OnDeleteSubmitCompleted, null);
            }
            else
            {
                throw new AccessViolationException("RIA Service Delete Entity for Order Context is denied");
            }
        }

        private void formCustomerInfo_EditEnded(object sender, DataFormEditEndedEventArgs e)
        {

        }

        private void formOrderStatus_EditEnded(object sender, DataFormEditEndedEventArgs e)
        {

        }
    }
}
