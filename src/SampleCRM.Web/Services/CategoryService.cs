using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Attributes;
using SampleCRM.Web.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class CategoryService : SampleCRMService
    {
        [Query]
        public IQueryable<Category> GetCategories() => 
            _context.Categories.OrderBy(x => x.Name);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertCategory(Category category)
        {

            _context.Categories.AddOrUpdate(category);
            _context.SaveChanges();
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateCategory(Category category)
        {
            _context.Categories.AddOrUpdate(category);
            _context.SaveChanges();
        }
    }
}