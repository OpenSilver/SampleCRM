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
    public class SampleCRMService : DomainService
    {
        protected SampleCRMEntities _context = new SampleCRMEntities();

        #region Category
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
        #endregion

        #region Country
        [Query]
        public IQueryable<CountryCode> GetCountries() =>
            _context.CountryCodes.OrderBy(c => c.Name);

        public CountryCode GetCountryById(string countryCodeID) =>
            GetCountries().SingleOrDefault(x => x.CountryCodeID == countryCodeID);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteCountry(CountryCode country) => _context.CountryCodes.Remove(country);

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertCountry(CountryCode country) => _context.CountryCodes.AddOrUpdate(country);

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateCountry(CountryCode country) => _context.CountryCodes.AddOrUpdate(country);
        #endregion

        #region Counts
        [Query]
        public IQueryable<CountModel> GetAllCounts() =>
            new CountModel[] { new CountModel
                {
                    OrderCount = _context.Orders.Count(),
                    CustomerCount = _context.Customers.Count(),
                    CategoryCount = _context.Categories.Count(),
                    ProductCount = _context.Products.Count()
                }
            }.AsQueryable();
        #endregion

        #region Customer
        [Query]
        public IQueryable<Customer> GetCustomers(string search) =>
            _context.Customers
                    .Where(x => x.FirstName.ToLower().Contains(search.ToLower())
                            || x.LastName.ToLower().Contains(search.ToLower())
                            || search == "");

        [Query]
        public IQueryable<Customer> GetCustomersWithoutPictures(string search) =>
            GetCustomers(search)
            .ToList()
            .Select(x => new Customer
            {
                CustomerID = x.CustomerID,
                AddressLine1 = x.AddressLine1,
                AddressLine2 = x.AddressLine2,
                BirthDateUTC = x.BirthDateUTC,
                ChildrenAtHome = x.ChildrenAtHome,
                City = x.City,
                CountryCode = x.CountryCode,
                CreatedOnUTC = x.CreatedOnUTC,
                Education = x.Education,
                EmailAddress = x.EmailAddress,
                FirstName = x.FirstName,
                Phone = x.Phone,
                NumberCarsOwned = x.NumberCarsOwned,
                Gender = x.Gender,
                YearlyIncome = x.YearlyIncome,
                Suffix = x.Suffix,
                IsHouseOwner = x.IsHouseOwner,
                LastName = x.LastName,
                MaritalStatus = x.MaritalStatus,
                MiddleName = x.MiddleName,
                Occupation = x.Occupation,
                PostalCode = x.PostalCode,
                LastModifiedOnUTC = x.LastModifiedOnUTC,
                Region = x.Region,
                SearchTerms = x.SearchTerms,
                Title = x.Title,
                TotalChildren = x.TotalChildren,
                Picture = new byte[] { 0 }
            }).AsQueryable();

        [Query]
        public IQueryable<Customer> GetLatestCustomers(string search, int limit) =>
            GetCustomersWithoutPictures(search)
                .OrderByDescending(x => x.CreatedOnUTC)
                .Take(limit);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteCustomer(Customer customer)
        {
            var dCustomer = _context.Customers.FirstOrDefault(x => x.CustomerID == customer.CustomerID);
            if (dCustomer == null)
                return;

            _context.Customers.Remove(dCustomer);
            _context.SaveChanges();
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertCustomer(Customer customer)
        {
            customer.CustomerID = new Random().Next((int)Math.Pow(10, 12), (int)Math.Pow(10, 13) - 1);
            if (customer.CustomerID < 0)
                customer.CustomerID *= -1;

            customer.LastModifiedOnUTC = customer.CreatedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateCustomer(Customer customer)
        {
            customer.LastModifiedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _context.Customers.AddOrUpdate(customer);
            _context.SaveChanges();
        }

        public Customer GetCustomerById(long customerId) =>
            GetCustomers(string.Empty).SingleOrDefault(x => x.CustomerID == customerId);

        public byte[] GetCustomerPicture(long customerId)
        {
            var customer = GetCustomerById(customerId);
            return customer != null ? customer.Picture : throw new ArgumentNullException($"No Such Customer {customerId}");
        }
        #endregion

        #region OrderItems
        [Query]
        public IQueryable<OrderItem> GetOrderItems(string search) =>
            _context.OrderItems.OrderBy(o => o.OrderLine)
                           .Where(x => x.ProductID.ToString().Contains(search)
                                       || x.OrderID.ToString().Contains(search)
                                       || search == "")
                           .AsQueryable();

        public OrderItem GetOrderItemById(int orderId, int orderLine) =>
            GetOrderItems(string.Empty)
                .SingleOrDefault(x => x.OrderID == orderId && x.OrderLine == orderLine);

        [Query]
        public IQueryable<OrderItem> GetOrderItemsOfOrder(long orderId, string search) =>
            GetOrderItems(search)
                .Where(x => x.OrderID == orderId);

        [Query]
        public IQueryable<OrderItem> GetOrderItemsOfProduct(string productId, string search) =>
            GetOrderItems(search)
                .Where(x => x.ProductID == productId);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteOrderItem(OrderItem orderItem)
        {
            var dOrderItem = _context.OrderItems
                                     .FirstOrDefault(x => x.OrderID == orderItem.OrderID && x.OrderLine == orderItem.OrderLine);
            if (dOrderItem == null)
                return;

            _context.OrderItems.Remove(dOrderItem);
            _context.SaveChanges();
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertOrderItem(OrderItem orderItem)
        {
            var lastItem = _context.OrderItems
                                   .Where(x => x.OrderID == orderItem.OrderID)
                                   .OrderByDescending(x => x.OrderLine)
                                   .FirstOrDefault();
            if (lastItem == null)
            {
                orderItem.OrderLine = 1;
            }
            else
            {
                orderItem.OrderLine = lastItem.OrderLine + 1;
            }

            _context.OrderItems.Add(orderItem);

#if DEBUG
            var validationResult = _context.Entry(orderItem).GetValidationResult();
            if (validationResult.ValidationErrors.Any())
            {
                Console.WriteLine($"Validation Error in InsertOrderItem: {validationResult.ValidationErrors.FirstOrDefault().PropertyName} {validationResult.ValidationErrors.FirstOrDefault().ErrorMessage}");
            }
#endif

            _context.SaveChanges();
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.AddOrUpdate(orderItem);
            _context.SaveChanges();
        }
        #endregion

        #region Order
        [Query]
        public IQueryable<Order> GetOrders(string search) =>
           _context.Orders.OrderByDescending(o => o.OrderDateUTC)
                          .Where(x => x.OrderID.ToString().Contains(search)
                                      || search == "")
                          .AsQueryable();

        [Query]
        public IQueryable<Order> GetLatestOrders(int limit, string search) =>
            GetOrders(search).Take(limit);

        [Query]
        public IQueryable<Order> GetOrdersOfCustomer(long customerId, string search) =>
            GetOrders(search).Where(x => x.CustomerID == customerId);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteOrder(Order order)
        {
            var dorder = _context.Orders.FirstOrDefault(x => x.OrderID == order.OrderID);
            if (dorder == null)
                return;

            _context.Orders.Remove(dorder);
            _context.SaveChanges();
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertOrder(Order order)
        {
            order.OrderID = new Random().Next((int)Math.Pow(10, 12), (int)Math.Pow(10, 13) - 1);
            if (order.OrderID < 0)
                order.OrderID *= -1;

            order.OrderDateUTC = order.LastModifiedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _context.Orders.Add(order);
#if DEBUG
            var validationResult = _context.Entry(order).GetValidationResult();
            if (validationResult.ValidationErrors.Any())
            {
                Console.WriteLine($"Validation Error in InsertOrder: {validationResult.ValidationErrors.FirstOrDefault().PropertyName} {validationResult.ValidationErrors.FirstOrDefault().ErrorMessage}");
            }
#endif
            _context.SaveChanges();
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateOrder(Order order)
        {
            order.LastModifiedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
#if DEBUG
            var validationResult = _context.Entry(order).GetValidationResult();
            if (validationResult.ValidationErrors.Any())
            {
                Console.WriteLine($"Validation Error in InsertOrder: {validationResult.ValidationErrors.FirstOrDefault().PropertyName} {validationResult.ValidationErrors.FirstOrDefault().ErrorMessage}");
            }
#endif
            _context.Orders.AddOrUpdate(order);
            _context.SaveChanges();
        }
        #endregion

        #region OrderStatus
        [Query]
        public IQueryable<OrderStatu> GetOrderStatus() => _context.OrderStatus;

        public OrderStatu GetOrderStatusById(long statusId) =>
            GetOrderStatus().SingleOrDefault(x => x.Status == statusId);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteStatus(OrderStatu status)
        {
            _context.OrderStatus.Remove(status);
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertStatus(OrderStatu status)
        {
            _context.OrderStatus.AddOrUpdate(status);
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateStatus(OrderStatu status)
        {
            _context.OrderStatus.AddOrUpdate(status);
        }
        #endregion

        #region Payment Type
        [Query]
        public IQueryable<PaymentType> GetPaymentTypes() => _context.PaymentTypes;

        public PaymentType GetPaymentTypeById(long paymentTypeId) =>
            GetPaymentTypes().SingleOrDefault(x => x.PaymentTypeID == paymentTypeId);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeletePaymentType(PaymentType paymentType)
        {
            _context.PaymentTypes.Remove(paymentType);
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertPaymentType(PaymentType paymentType)
        {
            _context.PaymentTypes.AddOrUpdate(paymentType);
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdatePaymentType(PaymentType paymentType)
        {
            _context.PaymentTypes.AddOrUpdate(paymentType);
        }
        #endregion

        #region Product
        [Query]
        public IQueryable<Product> GetProducts(string search) =>
            _context.Products
                    .Where(x => x.Name.ToLower().Contains(search.ToLower())
                            || search == "");

        [Query]
        public IQueryable<Product> GetProductsWithoutPictures(string search) =>
            GetProducts(search)
                .ToList()
                .Select(x => new Product
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
        public IQueryable<Product> GetTopSaleProducts(int limit) =>
            GetProducts(string.Empty)
                .Join(_context.OrderItems, p => p.ProductID, oi => oi.ProductID, (p, oi) => new { Product = p, OrderItem = oi })
                .Join(_context.Orders, oi => oi.OrderItem.OrderID, o => o.OrderID, (oi, o) => new { oi.Product, o.OrderID })
                .GroupBy(po => po.Product)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => group.Key)
                .ToList()
                .Select(x => new Product
                {
                    ProductID = x.ProductID,
                    Name = x.Name,
                    ListPrice = x.ListPrice,
                    Picture = new byte[] { 0 }
                })
                .AsQueryable();

        public Product GetProductById(string productId) =>
            GetProducts(string.Empty).SingleOrDefault(x => x.ProductID == productId);

        public byte[] GetProductPicture(string productId)
        {
            var product = GetProductById(productId);
            return product != null ? product.Picture : throw new ArgumentNullException($"No Such Product {productId}");
        }

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteProduct(Product product)
        {
            var dProduct = _context.Products.FirstOrDefault(x => x.ProductID == product.ProductID);
            if (dProduct == null)
                return;

            _context.Products.Remove(dProduct);
            _context.SaveChanges();
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertProduct(Product product)
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
        public void UpdateProduct(Product product)
        {
            product.LastModifiedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _context.Products.AddOrUpdate(product);
            _context.SaveChanges();
        }
        #endregion

        #region Shippers
        [Query]
        public IQueryable<Shipper> GetShippers() => _context.Shippers.OrderBy(s => s.Name);

        public Shipper GetShipperById(long shipperId) =>
            GetShippers().SingleOrDefault(x => x.ShipperID == shipperId);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteShippers(Shipper shipper)
        {
            _context.Shippers.Remove(shipper);
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertShippers(Shipper shipper)
        {
            _context.Shippers.AddOrUpdate(shipper);
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateShippers(Shipper shipper)
        {
            _context.Shippers.AddOrUpdate(shipper);
        }
        #endregion

        #region Tax Type
        [Query]
        public IQueryable<TaxType> GetTaxTypes() => _context.TaxTypes.OrderBy(t => t.Name);

        public TaxType GetTaxTypeById(long taxTypeId) =>
            GetTaxTypes().SingleOrDefault(x => x.TaxTypeID == taxTypeId);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteTaxTypes(TaxType taxType)
        {
            _context.TaxTypes.Remove(taxType);
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertTaxTypes(TaxType taxType)
        {
            _context.TaxTypes.AddOrUpdate(taxType);
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateTaxTypes(TaxType taxType)
        {
            _context.TaxTypes.AddOrUpdate(taxType);
        }
        #endregion
    }
}