#if OPENSILVER
using CSHTML5.Internal;
using OpenRiaServices.DomainServices.Client;
using SimpleCRM.Web;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SimpleCRM
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += this.Application_Startup;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();

#if OPENSILVER
            KnownTypesHelper.AddKnownType(typeof(student));
            ((DomainClientFactory)DomainContext.DomainClientFactory).ServerBaseUri = new Uri("http://localhost:54837/");
            //DomainContext.DomainClientFactory = new OpenRiaServices.DomainServices.Client.Web.SoapDomainClientFactory()
            //{
            //    ServerBaseUri = new Uri("http://localhost:54837/"),
            //};
#endif
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new MainPage();
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                e.Handled = true;
                ChildWindow errorWin = new ErrorWindow(e.ExceptionObject);
                errorWin.Show();
            }
        }
    }
}