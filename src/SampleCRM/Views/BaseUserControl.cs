using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    public class BaseUserControl : UserControl, INotifyPropertyChanged
    {
        protected const string _imageFileExtFilter = "All Images Files (*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif)|*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif" +
            "|PNG Portable Network Graphics (*.png)|*.png" +
            "|JPEG File Interchange Format (*.jpg *.jpeg *jfif)|*.jpg;*.jpeg;*.jfif" +
            "|BMP Windows Bitmap (*.bmp)|*.bmp" +
            "|TIF Tagged Imaged File Format (*.tif *.tiff)|*.tif;*.tiff" +
            "|GIF Graphics Interchange Format (*.gif)|*.gif";
        
        protected virtual double MaxMobileWidth => 700d;

        public BaseUserControl()
        {
            Loaded += BaseUserControl_Loaded;
            Application.Current.MainWindow.SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            ArrangeLayout();
            OnPropertyChanged(nameof(IsMobileUI));
        }

        private void BaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ArrangeLayout();
        }

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