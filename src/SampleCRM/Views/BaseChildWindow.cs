using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public class BaseChildWindow : ChildWindow
    {
        protected const double windowSizeMult = .85;
        protected const double windowMobileSizeMult = 1;
        protected virtual double MaxMobileWidth => 700d;

        public BaseUserControl InnerControl { get; protected set; }

        public bool IsMobileUI
        {
            get { return Application.Current.MainWindow.ActualWidth <= MaxMobileWidth; }
        }

        public BaseChildWindow()
        {
            arrangeSize();
            Application.Current.MainWindow.SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            //Console.WriteLine($"MainWindow_SizeChanged, w:{Application.Current.MainWindow.ActualWidth} h:{Application.Current.MainWindow.ActualHeight}");
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