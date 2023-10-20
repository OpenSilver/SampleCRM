using SampleCRM.Web.Views;
using System;
using System.Windows;

namespace SampleCRM
{
    public partial class ErrorWindow : BaseChildWindow
    {
        protected override double windowSizeMult => .5d;

        public static void Show(Exception ex)
        {
            var errorWindow = new ErrorWindow(ex);
            errorWindow.Show();
        }

        public static void Show(Uri uri)
        {
            var errorWindow = new ErrorWindow(uri);
            errorWindow.Show();
        }

        public static void Show(string message, string details = "")
        {
            var errorWindow = new ErrorWindow(message, details);
            errorWindow.Show();
        }

        public static void Show(string title, string message, string details = "")
        {
            var errorWindow = new ErrorWindow(title, message, details);
            errorWindow.Show();
        }

        private ErrorWindow(Exception e)
        {
            InitializeComponent();
            if (e != null)
            {
                ErrorTextBox.Text = e.Message + Environment.NewLine + Environment.NewLine + e.StackTrace;
            }
        }

        private ErrorWindow(Uri uri)
        {
            InitializeComponent();
            if (uri != null)
            {
                ErrorTextBox.Text = "Page not found: \"" + uri.ToString() + "\"";
            }
        }

        private ErrorWindow(string message, string details)
        {
            InitializeComponent();
            ErrorTextBox.Text = message + Environment.NewLine + Environment.NewLine + details;
        }

        private ErrorWindow(string title, string message, string details)
        {
            InitializeComponent();
            Title = title;
            IntroductoryText.Text = message;
            ErrorTextBox.Text = details;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}