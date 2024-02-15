using OpenRiaServices.DomainServices.Client;
using SampleCRM.LoginUI;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public partial class Dashboard : BasePage
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        protected override void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            base.OnSizeChanged(sender, e);

            if (IsMobileUI)
            {
                grdDashboard.ColumnDefinitions.Clear();
                grdDashboard.ColumnDefinitions.Add(new ColumnDefinition());

                Grid.SetColumn(cntCustomers, 0);
                Grid.SetColumn(cntOrders, 0);
                Grid.SetColumn(cntProducts, 0);

                grdDashboard.RowDefinitions.Clear();
                for (int i = 0; i < 3; i++)
                    grdDashboard.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                Grid.SetRow(cntCustomers, 0);
                Grid.SetRow(cntOrders, 1);
                Grid.SetRow(cntProducts, 2);
            }
            else
            {
                grdDashboard.ColumnDefinitions.Clear();
                grdDashboard.ColumnDefinitions.Add(new ColumnDefinition());
                grdDashboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Pixel) });
                grdDashboard.ColumnDefinitions.Add(new ColumnDefinition());

                Grid.SetColumn(cntCustomers, 0);
                Grid.SetColumn(cntOrders, 0);
                Grid.SetColumn(cntProducts, 2);

                grdDashboard.RowDefinitions.Clear();
                grdDashboard.RowDefinitions.Add(new RowDefinition { MinHeight = 300 });
                grdDashboard.RowDefinitions.Add(new RowDefinition { MinHeight = 400 });
                grdDashboard.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                Grid.SetRow(cntCustomers, 0);
                Grid.SetRow(cntOrders, 1);
                Grid.SetRow(cntProducts, 1);
            }
        }
    }
}