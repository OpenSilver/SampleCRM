using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class OrderItemsService : SampleCRMService
    {
        [Query]
        public IQueryable<OrderItems> GetOrderItems()
        {
            return _context.OrderItems;
        }

        public OrderItems GetOrderItemById(int orderId)
        {
            return _context.OrderItems.SingleOrDefault(x => x.OrderID == orderId);
        }

        [Query]
        public IQueryable<OrderItems> GetOrderItemsOfOrder(long orderId)
        {
            var itemsQuery = _context.OrderItems.Where(x => x.OrderID == orderId);
            return itemsQuery;
        }

        [Query]
        public IQueryable<OrderItems> GetOrderItemsOfProduct(string productId)
        {
            return _context.OrderItems.Where(x => x.ProductID == productId);
        }

        [Delete]
        public void DeleteOrderItem(OrderItems orderItem)
        {
            var dOrderItem = _context.OrderItems.SingleOrDefault(x => x.OrderID == orderItem.OrderID && x.OrderLine == orderItem.OrderLine);
            if (dOrderItem == null)
                return;

            _context.OrderItems.Remove(dOrderItem);
            _context.SaveChanges();
        }

        [Insert]
        public void InsertOrderItem(OrderItems orderItem)
        {
            orderItem.OrderID = new Random().Next((int)Math.Pow(10, 12), (int)Math.Pow(10, 13) - 1);
            orderItem.OrderLine = _context.OrderItems.Count(x => x.OrderID == orderItem.OrderID);
            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();
        }

        [Update]
        public void UpdateOrderItem(OrderItems orderItem)
        {
            _context.OrderItems.AddOrUpdate(orderItem);
            _context.SaveChanges();
        }
    }
}