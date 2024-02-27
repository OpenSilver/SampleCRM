using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SampleCRM.Web.Models;
using System.ServiceModel.Channels;

#if USEAI
using OpenSilver.AI.Client;
using OpenSilver.AI.Controls;
#endif


namespace SampleCRM.Web.Views
{
    public partial class Assistant : BasePage
    {

        public Assistant()
        {
            InitializeComponent();

#if !USEAI

            this.Content = new TextBlock()
            {
                Text = "The assistant requires the use of AI. Please use the SampleCRM.With.AI solution to use it.",
                FontSize = 26,
                TextWrapping = TextWrapping.Wrap
            };
#else
            MyConversationalUI.RiaGeneratedMetadata = new RiaGeneratedMetadata()
            {
                AssemblyContainingCode = Assembly.GetExecutingAssembly(),
                DomainContextType = typeof(CustomersContext),
                NamespaceOfEntitiesModel = "SampleCRM.Web.Models"
            };
#endif
        }
    }
}