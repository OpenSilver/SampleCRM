using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;
using System.ComponentModel.DataAnnotations;
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
            _context.SaveChanges();
        }

        [Insert]
        public void InsertCustomer(Customers customer)
        {

            _context.Customers.Add(customer);
            var validationResult = _context.Entry(customer).GetValidationResult();
            if (validationResult.ValidationErrors.Any())
            {
                throw new ValidationException($"Validation Error in InsertCustomer: {validationResult.ValidationErrors.FirstOrDefault().PropertyName} {validationResult.ValidationErrors.FirstOrDefault().ErrorMessage}");
            }

            _context.SaveChanges();
        }

        [Update]
        public void UpdateCustomer(Customers customer)
        {
            _context.Customers.AddOrUpdate(customer);
            _context.SaveChanges();
        }
    }
}