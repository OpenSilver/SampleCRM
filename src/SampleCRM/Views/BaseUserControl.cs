using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public abstract class BaseUserControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}