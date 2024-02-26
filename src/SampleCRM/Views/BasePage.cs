using OpenRiaServices.Controls;
using OpenRiaServices.DomainServices.Client;
using SampleCRM.LoginUI;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public class BasePage : Page
    {
        protected virtual double MaxMobileWidth => 640d;

        public BasePage()
        {
            Title = $"{App.Title} - {GetType().Name}";
            SizeChanged += OnSizeChanged;
            Loaded += BasePage_Loaded;
            Unloaded += BasePage_Unloaded;
        }

        private void BasePage_Unloaded(object sender, RoutedEventArgs e)
        {
            foreach (var field in sender.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.FieldType == typeof(DomainDataSource))
                {
                    var domainDataSource = (DomainDataSource)field.GetValue(this);
                    domainDataSource.LoadedData -= domainDataSource_LoadedData;
                }
            }
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var field in sender.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.FieldType == typeof(DomainDataSource))
                {
                    var domainDataSource = (DomainDataSource)field.GetValue(this);
                    domainDataSource.LoadedData += domainDataSource_LoadedData;
                }
            }
        }

        protected void domainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                var ee = (e.Error as DomainOperationException);
                if (ee != null)
                {
                    if (ee.ErrorCode == 401 || ee.Status == OperationErrorStatus.Unauthorized)
                    {
                        e.MarkErrorAsHandled();

                        var loginWindow = new LoginRegistrationWindow();
                        loginWindow.Show();
                    }
                }
            }
        }

        protected virtual void OnSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            IsMobileUI = ActualWidth <= MaxMobileWidth;
        }

        public bool IsMobileUI
        {
            get { return (bool)GetValue(IsMobileUIProperty); }
            set { SetValue(IsMobileUIProperty, value); }
        }
        public static readonly DependencyProperty IsMobileUIProperty =
            DependencyProperty.Register("IsMobileUI", typeof(bool), typeof(BaseUserControl), new PropertyMetadata(null));
    }
}