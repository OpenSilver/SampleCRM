using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SimpleCRM.Web.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleCRM.Web
{
    [EnableClientAccess()]
    public class CategoryService : SimpleCRMService
    {
        [Query]
        public IQueryable<Categories> GetCategories()
        {
            return _context.Categories;
        }

        [Delete]
        public void DeleteCategory(Categories category)
        {
            _context.Categories.Remove(category);
        }

        [Insert]
        public void InsertCategory(Categories category)
        {
            _context.Categories.AddOrUpdate(category);
        }

        [Update]
        public void UpdateCategory(Categories category)
        {
            _context.Categories.AddOrUpdate(category);
        }
    }
}