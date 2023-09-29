using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class ProductsService: SampleCRMService
    {
        [Query]
        public IQueryable<Products> GetProducts()
        {
            return _context.Products;
        }

        public Products GetProductById(string productId)
        {
            return _context.Products.SingleOrDefault(x => x.ProductID == productId);
        }

        [Delete]
        public void DeleteProduct(Products product)
        {
            _context.Products.Remove(product);
        }

        [Insert]
        public void InsertProduct(Products product)
        {
            _context.Products.AddOrUpdate(product);
        }

        [Update]
        public void UpdateProduct(Products product)
        {
            _context.Products.AddOrUpdate(product);
        }
    }
}