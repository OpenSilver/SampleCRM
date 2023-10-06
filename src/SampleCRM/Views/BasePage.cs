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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}