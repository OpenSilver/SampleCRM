using System.Windows;

namespace SampleCRM.Web.Views
{
    public partial class Orders : BasePage
    {
        public Orders()
        {
            InitializeComponent();
        }

        protected override void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            base.OnSizeChanged(sender, e);

            if (IsMobileUI)
            {
                formOrder.EditTemplate = Resources["dtNarrowOrders"] as DataTemplate;
            }
            else
            {
                formOrder.EditTemplate = Resources["dtWideOrders"] as DataTemplate;
            }
        }
    }
}