using System;
using System.Windows;
using OpenRiaServices.DomainServices.Client;

namespace SampleCRM.Web.Views
{
    public partial class OrderItemAddEdit : BaseUserControl
    {
        public event EventHandler ItemDeleted;
        public event EventHandler ItemAdded;
        public event EventHandler ItemUpdated;

        private Models.OrderItems _item = new Models.OrderItems();
        public Models.OrderItems Item
        {
            get { return _item; }
            set
            {
                if (_item != value)
                {
                    _item = value;
                    OnPropertyChanged();
#if DEBUG
                    Console.WriteLine($"OrderItemAddEdit, Item: {value.ProductID} selected");
#endif
                }
            }
        }

        public OrderItemAddEdit()
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

        public void Save(OrderItemsContext context)
        {
            if ((context.OrderItems.CanAdd && Item.IsNew) || context.OrderItems.CanEdit)
            {
                if (Item.IsNew)
                {
                    context.OrderItems.Add(Item);
                }

                context.SubmitChanges(OnAddSubmitCompleted, null);
            }
            else
            {
                throw new AccessViolationException("RIA Service Add / Edit Entity for Order Item Context is denied");
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
                if (ItemDeleted != null)
                    ItemDeleted(this, new EventArgs());
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
                if (_item.IsNew)
                {
                    if (ItemAdded != null)
                        ItemAdded(this, new EventArgs());
                }
                else
                {
                    if (ItemUpdated != null)
                        ItemUpdated(this, new EventArgs());
                }
            }
        }

        public void Delete(OrderItemsContext context)
        {
            if (context.OrderItems.CanRemove)
            {
                context.OrderItems.Remove(Item);
                context.SubmitChanges(OnDeleteSubmitCompleted, null);
            }
            else
            {
                throw new AccessViolationException("RIA Service Delete Entity for Order Item Context is denied");
            }
        }
    }
}