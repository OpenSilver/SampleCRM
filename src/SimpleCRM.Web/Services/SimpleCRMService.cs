using OpenRiaServices.DomainServices.Server;
using SimpleCRM.Web.Models;

namespace SimpleCRM.Web
{
    public abstract class SimpleCRMService: DomainService
    {
        protected SimpleCRMEntities _context = new SimpleCRMEntities();
    }
}