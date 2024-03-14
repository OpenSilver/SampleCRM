using System.Windows;

namespace SampleCRM.Web.Views
{
    public partial class Customers : BasePage
    {
        public Customers()
        {
            InitializeComponent();
        }

        protected override void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            base.OnSizeChanged(sender, e);

            if (IsMobileUI)
            {
                formCustomer.EditTemplate = Resources["dtNarrowCustomers"] as DataTemplate;
            }
            else
            {
                formCustomer.EditTemplate = Resources["dtWideCustomers"] as DataTemplate;
            }
        }
    }
}