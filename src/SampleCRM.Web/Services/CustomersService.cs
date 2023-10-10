using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
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
            return _context.Customers;
        }

        [Query]
        public IQueryable<Customers> GetLatestCustomers(int limit)
        {
            return _context.Customers.OrderByDescending(x => x.CreatedOn).Take(limit);
        }

        [Delete]
        public void DeleteCustomer(Customers customer)
        {
            var dCustomer = _context.Customers.FirstOrDefault(x => x.CustomerID == customer.CustomerID);
            if (dCustomer == null)
                return;

            _context.Customers.Remove(dCustomer);
            _context.SaveChanges();
        }

        [Insert]
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
        public void UpdateCustomer(Customers customer)
        {
            customer.LastModifiedOn = DateTime.Now.ToString();
            _context.Customers.AddOrUpdate(customer);
            _context.SaveChanges();
        }
    }
}