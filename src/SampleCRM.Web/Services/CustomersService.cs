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
        public IQueryable<Customers> GetCustomers()
        {
            return _context.Customers.OrderBy(c => c.FirstName).ThenBy(c => c.LastName);
        }

        [Query]
        public IQueryable<Customers> GetCustomersWithoutPictures()
        {
            return GetCustomers().ToList().Select(x => new Customers
            {
                CustomerID = x.CustomerID,
                AddressLine1 = x.AddressLine1,
                AddressLine2 = x.AddressLine2,
                BirthDate = x.BirthDate,
                ChildrenAtHome = x.ChildrenAtHome,
                City = x.City,
                CountryCode = x.CountryCode,
                CreatedOn = x.CreatedOn,
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
                LastModifiedOn = x.LastModifiedOn,
                Region = x.Region,
                SearchTerms = x.SearchTerms,
                Title = x.Title,
                TotalChildren = x.TotalChildren,
                Picture = new byte[] { 0 },
                Thumbnail = new byte[] { 0 }
            }).AsQueryable();
        }

        [Query]
        public IQueryable<Customers> GetLatestCustomers(int limit)
        {
            return GetCustomersWithoutPictures()
                .OrderByDescending(x => x.CreatedOn)
                .Take(limit);
        }

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

            customer.LastModifiedOn = customer.CreatedOn = DateTime.Now.ToString();
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateCustomer(Customers customer)
        {
            customer.LastModifiedOn = DateTime.Now.ToString();
            _context.Customers.AddOrUpdate(customer);
            _context.SaveChanges();
        }

        public Customers GetCustomerById(long customerId)
        {
            return _context.Customers.SingleOrDefault(x => x.CustomerID == customerId);
        }

        public byte[] GetCustomerPicture(long customerId)
        {
            var customer = GetCustomerById(customerId);
            if (customer != null)
            {
                return customer.Picture;
            }
            else
            {
                throw new ArgumentNullException("No Such Customer");
            }
        }
    }
}