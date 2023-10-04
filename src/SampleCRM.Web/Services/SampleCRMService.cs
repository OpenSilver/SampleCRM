using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;

namespace SampleCRM.Web
{
    public abstract class SampleCRMService: DomainService
    {
        protected SampleCRMEntities _context = new SampleCRMEntities();
    }
}