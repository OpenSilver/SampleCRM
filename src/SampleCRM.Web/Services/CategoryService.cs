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
        public IQueryable<Categories> GetCategories()
        {
            return _context.Categories;
        }

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteCategory(Categories category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertCategory(Categories category)
        {
            
            _context.Categories.AddOrUpdate(category);
            _context.SaveChanges();
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateCategory(Categories category)
        {
            _context.Categories.AddOrUpdate(category);
            _context.SaveChanges();
        }
    }
}