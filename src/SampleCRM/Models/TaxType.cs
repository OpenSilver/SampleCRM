using OpenRiaServices.DomainServices.Client;

namespace SampleCRM.Web.Models
{
    public partial class TaxType : Entity
    {
        public string Desc => $"{Name} ({Rate})";
    }
}