using OpenRiaServices.Controls;
using SampleCRM.Web.Models;
using SampleCRM.Web.Views;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows.Navigation;

namespace SampleCRM
{
    public partial class MainPage : BaseUserControl, IBusyCapablePage
    {
        #region Properties
        public CountModel CountModel
        {
            get { return (CountModel)GetValue(CountModelProperty); }
            set { SetValue(CountModelProperty, value); }
        }
        public static readonly DependencyProperty CountModelProperty =
            DependencyProperty.Register("CountModel", typeof(CountModel), typeof(MainPage), new PropertyMetadata(null));

        public bool IsBlur
        {
            get { return (bool)GetValue(IsBlurProperty); }
            set { SetValue(IsBlurProperty, value); }
        }
        public static readonly DependencyProperty IsBlurProperty =
            DependencyProperty.Register("IsBlur", typeof(bool), typeof(MainPage),
                new PropertyMetadata(
                    new PropertyChangedCallback((s, t) =>
                    {
                        var page = s as MainPage;
                        var value = Convert.ToBoolean(t.NewValue);
                        page.ContentBorder.Effect = value ? new BlurEffect { Radius = 5 } : null;
                    })));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(MainPage),
                new PropertyMetadata(
                    new PropertyChangedCallback((s, t) =>
                    {
                        var page = s as MainPage;
                        var value = Convert.ToBoolean(t.NewValue);
                        page.IsBlur = value;
                    })));
        #endregion

        public MainPage()
        {
            AsyncHelper.ContentPage = this;
            DataContext = this;
            InitializeComponent();
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            highlightLinks(e, LinksStackPanel.Children);
            highlightLinks(e, MobileLinksStackPanel.Children);
            menuPane.CollapseIfMobile();
        }

        private void highlightLinks(NavigationEventArgs e, UIElementCollection links)
        {
            foreach (var child in links)
            {
                var hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    else
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                }
            }
        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ErrorWindow.Show(e.Uri);
        }

        public void MakeBusy(bool isBusy)
        {
            IsBusy = isBusy;
        }

        public void MakeBlur(bool isBlur)
        {
            IsBlur = isBlur;
        }

        private void menuPane_OnCurrentStateChanged(object sender, ResponsivePane.CurrentState e)
        {
            IsBlur = e == ResponsivePane.CurrentState.SmallResolution_ShowMenu;
        }

        private void countDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
                return;

            CountModel = e.AllEntities.Cast<CountModel>().FirstOrDefault();
        }
    }
}