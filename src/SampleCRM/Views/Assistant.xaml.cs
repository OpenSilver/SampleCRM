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
#else
            InitializeAIAssistant();
#endif
        }


        #region AI-related stuff
#if USEAI

        ObservableCollection<ChatMessage> _messages;
        AIAssistant _assistant;
        Action _isAlive;
        AIChatControl AIChat;

        BusyIndicator busyIndicatorForAICall;
        BusyIndicator busyIndicatorForCodeExecution;

        void InitializeAIAssistant()
        {
            //Programmatically creating the AIChatControl so the CRMSample that does not use AI can still build.
            AIChat = new AIChatControl() { Header = "Assistant", Width=250, FontSize=22 };
            AIChatControlContainer.Children.Add(AIChat);

            busyIndicatorForAICall = new BusyIndicator();
            busyIndicatorForCodeExecution = new BusyIndicator();
            busyIndicatorForAICall.Content = busyIndicatorForCodeExecution;

            _assistant = new AIAssistant
                (
                     placeWhereToDisplayGeneratedUI: PlaceWhereGeneratedUIIsShown,
                     assemblyContainingRIAGeneratedCode: Assembly.GetExecutingAssembly(),
                     namespaceOfEntitiesModel: "SampleCRM.Web.Models",
                     busyIndicatorForAICall: busyIndicatorForAICall,
                     busyIndicatorForCodeExecution: busyIndicatorForCodeExecution,
                     domainContextType: typeof(CustomersContext)
                );
            LogMessageQueueListBox.ItemsSource = _assistant.LogMessageQueue;


            _assistant.AIMessage += Assistant_AIMessage;

            _isAlive = () => { _count++; ShowHideDebugPanel.Content = $"Debug ({_count})"; };

            _messages = AIChat.ChatMessages;

            AIChat.UserMessage += AiChatControl_UserMessage;
            AIChat.QueryCancel += AiChatControl_QueryCancel;
        }


        private void Assistant_AIMessage(object sender, AIMessageEventArgs e)
        {
            if (e.Type == AIMessageType.Error)
            {
                _messages.Add(new ChatMessage(Owner.AIError, e.MessageText));
            }
            else
            {
                _messages.Add(new ChatMessage(Owner.AI, e.MessageText));
            }
        }

        private void AiChatControl_QueryCancel(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void AiChatControl_UserMessage(object sender, UserMessageEventArgs e)
        {
            string message = e.MessageText;
            if (!string.IsNullOrWhiteSpace(message))
            {
                ChatMessage acknowledgement = new ChatMessage(Owner.AI, "Please wait while I Process your request...");
                _messages.Add(acknowledgement);
                ChatMessage chatMessage = new ChatMessage(Owner.None, busyIndicatorForAICall);
                _messages.Add(chatMessage);
                await _assistant.Start(message, _isAlive);
                _messages.Remove(chatMessage);
                _messages.Remove(acknowledgement);
            }
        }

        private async void ButtonRunAssistant_Click(object sender, RoutedEventArgs e)
        {


        }
#endif

        #endregion
    }
}