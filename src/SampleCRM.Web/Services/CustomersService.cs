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
    public class CustomersService : SampleCRMService
    {
        [Query]
        public IQueryable<Customers> GetCustomers(string search) =>
            _context.Customers
                    .Where(x => x.FirstName.ToLower().Contains(search.ToLower())
                            || x.LastName.ToLower().Contains(search.ToLower())
                            || search == "")
                    .OrderBy(c => c.FirstName)
                    .ThenBy(c => c.LastName);

        [Query]
        public IQueryable<Customers> GetCustomersCombo() =>
            GetCustomers(string.Empty);

        [Query]
        public IQueryable<Customers> GetCustomersWithoutPictures(string search) =>
            GetCustomers(search)
            .ToList()
            .Select(x => new Customers
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
        public IQueryable<Customers> GetLatestCustomers(string search, int limit) =>
            GetCustomersWithoutPictures(search)
                .OrderByDescending(x => x.CreatedOnUTC)
                .Take(limit);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteCustomer(Customers customer)
        {
            var dCustomer = _context.Customers.FirstOrDefault(x => x.CustomerID == customer.CustomerID);
            if (dCustomer == null)
                return;

            _context.Customers.Remove(dCustomer);
            _context.SaveChanges();
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertCustomer(Customers customer)
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
        public void UpdateCustomer(Customers customer)
        {
            customer.LastModifiedOnUTC = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _context.Customers.AddOrUpdate(customer);
            _context.SaveChanges();
        }

        public Customers GetCustomerById(long customerId) =>
            GetCustomers(string.Empty).SingleOrDefault(x => x.CustomerID == customerId);

        public byte[] GetCustomerPicture(long customerId)
        {
            var customer = GetCustomerById(customerId);
            return customer != null ? customer.Picture : throw new ArgumentNullException($"No Such Customer {customerId}");
        }
    }
}