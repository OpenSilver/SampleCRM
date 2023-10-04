using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class OrderService: SampleCRMService
    {
        [Query]
        public IQueryable<Orders> GetOrders()
        {
            return _context.Orders;
        }

        public Orders GetOrderById(int orderId)
        {
            return _context.Orders.SingleOrDefault(x => x.OrderID == orderId);
        }

        [Query]
        public IQueryable<Orders> GetOrdersOfCustomer(long customerId)
        {
            return _context.Orders.Where(x => x.CustomerID == customerId);
        }

        [Delete]
        public void DeleteOrder(Orders order)
        {
            _context.Orders.Remove(order);
        }

        [Insert]
        public void InsertOrder(Orders order)
        {
            _context.Orders.AddOrUpdate(order);
        }

        [Update]
        public void UpdateOrder(Orders order)
        {
            _context.Orders.AddOrUpdate(order);
        }
    }
}