using System.Windows;
using System.Windows.Controls;

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
                grdHead.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);

                Grid.SetColumn(grdSearch, 0);
                Grid.SetRow(grdSearch, 2);
                grdSearch.Margin = new Thickness(0, 0, 0, 10);

                Grid.SetRow(orderCard, 0);
                Grid.SetColumn(orderCard, 0);

                Grid.SetColumn(grdOrderDetails, 0);
                Grid.SetRow(grdOrderDetails, 1);

                grdTbOrder.RowDefinitions[0].Height = GridLength.Auto;
                grdTbOrder.RowDefinitions[1].Height = GridLength.Auto;

                grdTbOrder.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
                grdTbOrder.ColumnDefinitions[1].Width = GridLength.Auto;

                formOrder.EditTemplate = Resources["dtNarrowOrders"] as DataTemplate;


                grdTbOrderItems.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);

                Grid.SetColumn(grdOrderItemSearch, 0);
                Grid.SetRow(grdOrderItemSearch, 2);
                grdOrderItemSearch.Margin = new Thickness(0, 0, 0, 10);
            }
            else
            {
                grdHead.ColumnDefinitions[2].Width = new GridLength(405, GridUnitType.Pixel);

                Grid.SetColumn(grdSearch, 2);
                Grid.SetRow(grdSearch, 0);
                grdSearch.Margin = new Thickness();

                Grid.SetRow(orderCard, 0);
                Grid.SetColumn(orderCard, 0);

                Grid.SetRow(grdOrderDetails, 0);
                Grid.SetColumn(grdOrderDetails, 1);

                grdTbOrder.RowDefinitions[0].Height = new GridLength(2, GridUnitType.Star);
                grdTbOrder.RowDefinitions[1].Height = GridLength.Auto;

                grdTbOrder.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
                grdTbOrder.ColumnDefinitions[1].Width = new GridLength(4, GridUnitType.Star);

                formOrder.EditTemplate = Resources["dtWideOrders"] as DataTemplate;


                grdTbOrderItems.ColumnDefinitions[2].Width = new GridLength(405, GridUnitType.Pixel);

                Grid.SetColumn(grdOrderItemSearch, 2);
                Grid.SetRow(grdOrderItemSearch, 0);
                grdOrderItemSearch.Margin = new Thickness();
            }
        }
    }
}