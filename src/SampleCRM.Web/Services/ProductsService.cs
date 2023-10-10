using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class ProductsService : SampleCRMService
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

            var dProduct = _context.Products.FirstOrDefault(x => x.ProductID == product.ProductID);
            if (dProduct == null)
                return;

            _context.Products.Remove(dProduct);
            _context.SaveChanges();
        }

        [Insert]
        public void InsertProduct(Products product)
        {
            product.ProductID = new Random().Next((int)Math.Pow(10, 12), (int)Math.Pow(10, 13) - 1).ToString();
            product.CreatedOn = DateTime.Now.ToString();
            product.LastModifiedOn = DateTime.Now.ToString();
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        [Update]
        public void UpdateProduct(Products product)
        {
            product.LastModifiedOn = DateTime.Now.ToString();
            _context.Products.AddOrUpdate(product);
            _context.SaveChanges();
        }
    }
}