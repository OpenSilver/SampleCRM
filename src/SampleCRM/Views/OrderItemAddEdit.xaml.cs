using System;
using System.Windows;
using System.Windows.Controls.DataVisualization;
using OpenRiaServices.DomainServices.Client;

namespace SampleCRM.Web.Views
{
    public partial class OrderItemAddEdit : BaseUserControl
    {
        public event EventHandler ItemDeleted;
        public event EventHandler ItemAdded;

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

        public void Save(OrderItemsContext context)
        {
            if (context.OrderItems.CanAdd)
            {
                if (!form.CommitEdit())
                {
                    ErrorWindow.Show("Invalid Order Item");
                    return;
                }

                if (Item.IsNew)
                {
                    context.OrderItems.Add(Item);
                }

                context.SubmitChanges(OnAddSubmitCompleted, null);
            }
            else
            {
                throw new AccessViolationException("RIA Service Add Entity for Order Item Context is denied");
            }
        }

        private void OnDeleteSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                MessageBox.Show(string.Format("Submit Failed: {0}", so.Error.Message));
                Console.WriteLine(string.Format("Submit Failed: {0}", so.Error.StackTrace));
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
                MessageBox.Show(string.Format("Submit Failed: {0}", so.Error.Message));
                Console.WriteLine(string.Format("Submit Failed: {0}", so.Error.StackTrace));
                so.MarkErrorAsHandled();
            }
            else
            {
                if (ItemAdded != null)
                    ItemAdded(this, new EventArgs());
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var context = new OrderItemsContext();
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

        private void form_EditEnded(object sender, System.Windows.Controls.DataFormEditEndedEventArgs e)
        {

        }
    }
}