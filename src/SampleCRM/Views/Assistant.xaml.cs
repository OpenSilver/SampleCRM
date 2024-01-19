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
#if USEAI
using OpenSilver.AI.Client; 
#endif


namespace SampleCRM.Web.Views
{
    public partial class Assistant : BasePage
    {
        int _count = 0;
        
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

#endif
        }


        private async void ButtonRunAssistant_Click(object sender, RoutedEventArgs e)
        {
#if USEAI
            var assistant = new AIAssistant
                (
                     placeWhereToDisplayGeneratedUI: PlaceWhereGeneratedUIIsShown,
                     assemblyContainingRIAGeneratedCode: Assembly.GetExecutingAssembly(),
                     namespaceOfEntitiesModel: "SampleCRM.Web.Models",
                     busyIndicatorForAICall: busyIndicatorForAICall,
                     busyIndicatorForCodeExecution: busyIndicatorForCodeExecution,
                     domainContextType: typeof(CustomersContext)
                );

            LogMessageQueueListBox.ItemsSource = assistant.LogMessageQueue;

            Action isAlive = () => { _count++; ShowHideDebugPanel.Content = $"Debug ({_count})"; };

            await assistant.Start(InputTextBox.Text, isAlive);
#endif
        }
    }
}