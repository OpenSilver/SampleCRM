using Microsoft.AspNetCore.Components.WebView.Maui;

namespace SampleCRM.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var rootComponent = new RootComponent();
            rootComponent.Selector = "#app";
            rootComponent.ComponentType = typeof(Browser.App);
            blazorWebView.RootComponents.Add(rootComponent);
        }
    }
}
