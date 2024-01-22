#if OPENSILVER

using OpenRiaServices.DomainServices.Client;
using OpenRiaServices.DomainServices.Client.ApplicationServices;
using SampleCRM.Web;

#endif

using System;
using System.Net;
using System.Windows;

namespace SampleCRM
{
    public partial class App : Application
    {
        public const string Title = "Sample CRM";

        public string BaseUrl { get; private set; }
        public string ImageUrl => $"{BaseUrl}Images";

        public App()
        {
            Current.Host.Settings.DefaultSoapCredentialsMode = CredentialsMode.Enabled;

            Startup += Application_Startup;
            UnhandledException += Application_UnhandledException;

            InitializeComponent();

            var webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication();
            //webContext.Authentication = new WindowsAuthentication();
            ApplicationLifetimeObjects.Add(webContext);

#if LOCAL_DEBUG
            BaseUrl = "http://localhost:7002/";
#elif DEBUG
            BaseUrl = "https://localhost:44350/";
#elif LOCAL_RELEASE
            BaseUrl = "http://localhost:7002/";
#elif RELEASE
            BaseUrl = "https://samplecrm-webservices.azurewebsites.net/";
#else
            throw new NotSupportedException();
#endif
            DomainContext.DomainClientFactory = new OpenRiaServices.DomainServices.Client.Web.WebAssemblySoapDomainClientFactory()
            {
                ServerBaseUri = new Uri(BaseUrl)
            };
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Host.Settings.EnableOptimizationWhereCollapsedControlsAreNotRendered = true;

            // This will enable you to bind controls in XAML to WebContext.Current properties.
            Resources.Add("WebContext", WebContext.Current);

            // This will automatically authenticate a user when using Windows authentication or when the user chose "Keep me signed in" on a previous login attempt.
            // WebContext.Current.Authentication.LoadUser(Application_UserLoaded, null);

            RootVisual = new MainPage();
        }

        /// <summary>
        /// Invoked when the <see cref="LoadUserOperation"/> completes.
        /// Use this event handler to switch from the "loading UI" you created in <see cref="InitializeRootVisual"/> to the "application UI".
        /// </summary>
        private void Application_UserLoaded(LoadUserOperation operation)
        {
            if (operation.HasError)
            {
                ErrorWindow.Show(operation.Error);
                operation.MarkErrorAsHandled();
            }
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                e.Handled = true;
                ErrorWindow.Show(e.ExceptionObject);
            }
        }
    }
}