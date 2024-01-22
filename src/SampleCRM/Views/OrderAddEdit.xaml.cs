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
        public event EventHandler OrderUpdated;

        private Models.Orders _order = new Models.Orders();
        public Models.Orders Order
        {
            get { return _order; }
            set
            {
                if (_order != value)
                {
                    _order = value;
                    //OnPropertyChanged();
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

        public override void ArrangeLayout()
        {
            base.ArrangeLayout();
            grdNarrow.Visibility = BoolToVisibilityConverter.Convert(IsMobileUI);
            grdWide.Visibility = BoolToVisibilityConverter.Convert(!IsMobileUI);
        }

        public void Save(OrderContext context)
        {
            if ((context.Orders.CanAdd && Order.IsNew) || context.Orders.CanEdit)
            {
                if (Order.IsNew)
                    context.Orders.Add(Order);
                
                context.SubmitChanges(OnAddSubmitCompleted, null);
            }
            else
            {
                throw new AccessViolationException("RIA Service Add / Edit Order for Order Context is denied");
            }
        }

        public void Delete(OrderContext context)
        {
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
                if (_order.IsNew)
                {
                    if (OrderAdded != null)
                        OrderAdded(this, new EventArgs());
                }
                else
                {
                    if (OrderUpdated != null)
                        OrderUpdated(this, new EventArgs());
                }
            }
        }
    }
}
