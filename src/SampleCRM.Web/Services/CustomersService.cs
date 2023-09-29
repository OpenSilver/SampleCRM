﻿using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;
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

        public Customers GetCustomerById(int customerId)
        {
            return _context.Customers.SingleOrDefault(x => x.CustomerID == customerId);
        }

        [Delete]
        public void DeleteCustomer(Customers customer)
        {
            _context.Customers.Remove(customer);
        }

        [Insert]
        public void InsertCustomer(Customers customer)
        {
            _context.Customers.AddOrUpdate(customer);
        }

        [Update]
        public void UpdateCustomer(Customers customer)
        {
            _context.Customers.AddOrUpdate(customer);
        }
    }
}