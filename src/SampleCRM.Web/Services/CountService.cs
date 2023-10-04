using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class CountService : SampleCRMService
    {
        [Query]
        public IQueryable<CountModel> GetAllCounts()
        {
            var countModel = new CountModel
            {
                OrderCount = _context.Orders.Count(),
                CustomerCount = _context.Customers.Count(),
                CategoryCount = _context.Categories.Count(),
                ProductCount = _context.Products.Count()
            };
            return new CountModel[] { countModel }.AsQueryable();
        }
    }
}