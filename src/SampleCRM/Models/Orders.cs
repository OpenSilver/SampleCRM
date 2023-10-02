using OpenRiaServices.DomainServices.Client;

namespace SampleCRM.Web.Models
{
    public partial class Orders : Entity
    {
        public string ShipCountryName { get; set; }
    }
}