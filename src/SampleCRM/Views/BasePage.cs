using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    /// <summary>
    /// BasePage is a Navigation Page with INotifyPropertyChanged interface implemented
    /// BasePage can be it selft a view model for the corresponding Page UI
    /// </summary>
    public class BasePage : Page, INotifyPropertyChanged
    {
        protected virtual double MaxMobileWidth => 640d;

        public BasePage()
        {
            SizeChanged += OnSizeChanged;
        }

        protected virtual void OnSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsMobileUI));
        }

        public bool IsMobileUI
        {
            get { return ActualWidth <= MaxMobileWidth; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}