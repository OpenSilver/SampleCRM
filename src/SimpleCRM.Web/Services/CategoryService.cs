using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SimpleCRM.Web.Models;
using System.Linq;

namespace SimpleCRM.Web
{
    [EnableClientAccess()]
    public class CategoryService: SimpleCRMService
    {
        [Query]
        public IQueryable<Categories> GetCategories()
        {
            return _context.Categories;
        }
    }
}