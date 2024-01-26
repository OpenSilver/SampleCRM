using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public class BaseChildWindow : ChildWindow
    {
        protected virtual double windowMobileSizeMult => 1d;
        protected virtual double windowSizeMult => .85d;
        protected virtual double MaxMobileWidth => 700d;

        public BaseUserControl InnerControl { get; protected set; }

        public bool IsMobileUI => Application.Current.MainWindow.ActualWidth <= MaxMobileWidth;

        public BaseChildWindow()
        {
            arrangeSize();
            Application.Current.MainWindow.SizeChanged += MainWindow_SizeChanged;
            Loaded += BaseChildWindow_Loaded;
            Unloaded += BaseChildWindow_Unloaded;
        }

        private void BaseChildWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (AsyncHelper.ContentPage != null)
                AsyncHelper.ContentPage.MakeBlur(false);
        }

        private void BaseChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (AsyncHelper.ContentPage != null)
                AsyncHelper.ContentPage.MakeBlur(true);
        }

        private void MainWindow_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            arrangeSize();
        }

        private void arrangeSize()
        {
            if (IsMobileUI)
            {
                Width = Application.Current.MainWindow.ActualWidth * windowMobileSizeMult;
                Height = Application.Current.MainWindow.ActualHeight * windowMobileSizeMult;
            }
            else
            {
                Width = Application.Current.MainWindow.ActualWidth * windowSizeMult;
                Height = Application.Current.MainWindow.ActualHeight * windowSizeMult;
            }
        }
    }
}