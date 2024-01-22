using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Attributes;
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
        public IQueryable<Products> GetProducts(string search) =>
            _context.Products
                    .Where(x => x.Name.ToLower().Contains(search.ToLower())
                            || search == "")
                    .OrderBy(c => c.Name);

        public IQueryable<Products> GetProductsCombo() =>
            GetProducts(string.Empty)
                .ToList()
                .Select(x => new Products
                {
                    ProductID = x.ProductID,
                    Name = x.Name
                })
                .AsQueryable();

        [Query]
        public IQueryable<Products> GetProductsWithoutPictures(string search) =>
            GetProducts(search)
                .ToList()
                .Select(x => new Products
                {
                    ProductID = x.ProductID,
                    Name = x.Name,
                    CategoryID = x.CategoryID,
                    Color = x.Color,
                    CreatedOnUTC = x.CreatedOnUTC,
                    DealerPrice = x.DealerPrice,
                    Description = x.Description,
                    Discount = x.Discount,
                    DiscountEndDateUTC = x.DiscountEndDateUTC,
                    DiscountStartDateUTC = x.DiscountStartDateUTC,
                    LastModifiedOnUTC = x.LastModifiedOnUTC,
                    ListPrice = x.ListPrice,
                    SafetyStockLevel = x.SafetyStockLevel,
                    SearchTerms = x.SearchTerms,
                    Size = x.Size,
                    StockUnits = x.StockUnits,
                    TaxType = x.TaxType,
                    Picture = new byte[] { 0 }
                })
                .AsQueryable();

        [Query]
        public IQueryable<Products> GetTopSaleProducts(int limit) =>
            GetProducts(string.Empty)
                .Join(_context.OrderItems, p => p.ProductID, oi => oi.ProductID, (p, oi) => new { Product = p, OrderItem = oi })
                .Join(_context.Orders, oi => oi.OrderItem.OrderID, o => o.OrderID, (oi, o) => new { oi.Product, o.OrderID })
                .GroupBy(po => po.Product)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => group.Key)
                .ToList()
                .Select(x => new Products
                {
                    ProductID = x.ProductID,
                    Name = x.Name,
                    ListPrice = x.ListPrice,
                    Picture = new byte[] { 0 }
                })
                .AsQueryable();

        public Products GetProductById(string productId) =>
            GetProducts(string.Empty).SingleOrDefault(x => x.ProductID == productId);

        public byte[] GetProductPicture(string productId)
        {
            var product = GetProductById(productId);
            return product != null ? product.Picture : throw new ArgumentNullException($"No Such Product {productId}");
        }

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteProduct(Products product)
        {
            var dProduct = _context.Products.FirstOrDefault(x => x.ProductID == product.ProductID);
            if (dProduct == null)
                return;

            _context.Products.Remove(dProduct);
            _context.SaveChanges();
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertProduct(Products product)
        {
            product.ProductID = new Random().Next((int)Math.Pow(10, 12), (int)Math.Pow(10, 13) - 1).ToString();
            product.CreatedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            product.LastModifiedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _context.Products.Add(product);
#if DEBUG
            var validationResult = _context.Entry(product).GetValidationResult();
            if (validationResult.ValidationErrors.Any())
            {
                Console.WriteLine($"Validation Error in InsertProduct: {validationResult.ValidationErrors.FirstOrDefault().PropertyName} {validationResult.ValidationErrors.FirstOrDefault().ErrorMessage}");
            }
#endif
            _context.SaveChanges();
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateProduct(Products product)
        {
            product.LastModifiedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _context.Products.AddOrUpdate(product);
            _context.SaveChanges();
        }
    }
}