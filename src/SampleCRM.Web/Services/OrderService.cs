using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class OrderService : SampleCRMService
    {
        [Query]
        public IQueryable<Orders> GetOrders()
        {
            return _context.Orders;
        }

        [Query]
        public IQueryable<Orders> GetOrdersOfCustomer(long customerId)
        {
            return _context.Orders.Where(x => x.CustomerID == customerId);
        }

        [Delete]
        public void DeleteOrder(Orders order)
        {
            var dorder = _context.Orders.SingleOrDefault(x => x.OrderID == order.OrderID);
            if (dorder == null)
                return;

            _context.Orders.Remove(dorder);
            _context.SaveChanges();
        }

        [Insert]
        public void InsertOrder(Orders order)
        {
            order.OrderID = new Random().Next((int)Math.Pow(10, 12), (int)Math.Pow(10, 13) - 1);
            order.LastModifiedOn = DateTime.Now.ToString();
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        [Update]
        public void UpdateOrder(Orders order)
        {
            order.LastModifiedOn = DateTime.Now.ToString();
            _context.Orders.AddOrUpdate(order);
            _context.SaveChanges();
        }
    }
}