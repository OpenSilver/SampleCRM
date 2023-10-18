using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public class BaseUserControl : UserControl, INotifyPropertyChanged
    {
        protected virtual double MaxMobileWidth => 700d;

        public BaseUserControl()
        {
            //SizeChanged += OnSizeChanged;
            Loaded += BaseUserControl_Loaded;
        }

        private void BaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ArrangeLayout();
        }

        //protected virtual void OnSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        //{
        //    OnPropertyChanged(nameof(IsMobileUI));
        //}

        public virtual void ArrangeLayout() { }

        public bool IsMobileUI
        {
            get { return Application.Current.MainWindow.ActualWidth <= MaxMobileWidth; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}