using OpenSilver.AI.Client;
using SampleCRM.Web.Models;
using System.Reflection;

namespace SampleCRM.Web.Views
{
    public partial class Assistant : BasePage
    {
        public Assistant()
        {
            InitializeComponent();

            MyConversationalUI.RiaGeneratedMetadata = new RiaGeneratedMetadata()
            {
                AssemblyContainingCode = Assembly.GetExecutingAssembly(),
                DomainContextType = typeof(CustomersContext),
                NamespaceOfEntitiesModel = "SampleCRM.Web.Models"
            };
        }
    }
}
