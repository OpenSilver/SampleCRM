using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public class BasePage : Page
    {
        protected virtual double MaxMobileWidth => 640d;

        public BasePage()
        {
            SizeChanged += OnSizeChanged;
            Title = $"{App.Title} - {GetType().Name}";
        }

        protected virtual void OnSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            IsMobileUI= ActualWidth <= MaxMobileWidth;
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