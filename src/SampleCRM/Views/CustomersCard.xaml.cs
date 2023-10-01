using System;
using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public partial class CustomersCard : BaseUserControl
    {
        public CustomersCard()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty SelectedCustomerProperty =
            DependencyProperty.Register("SelectedCustomer", typeof(Models.Customers), typeof(CustomersCard), new PropertyMetadata(null));

        private Models.Customers _selectedCustomer;
        public Models.Customers SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (_selectedCustomer != value)
                {
                    _selectedCustomer = value;
#if DEBUG
                    Console.WriteLine($"CustomersCard, Customer: {value.FirstName} selected");
#endif
                    OnPropertyChanged();
                }
            }
        }
    }
}
