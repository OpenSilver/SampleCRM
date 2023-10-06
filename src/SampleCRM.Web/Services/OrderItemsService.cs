using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;
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
            return _context.OrderItems.Where(x => x.OrderID == orderId);
        }

        [Query]
        public IQueryable<OrderItems> GetOrderItemsOfProduct(string productId)
        {
            return _context.OrderItems.Where(x => x.ProductID == productId);
        }

        [Delete]
        public void DeleteOrderItem(OrderItems orderItem)
        {
            _context.OrderItems.Remove(orderItem);
        }

        [Insert]
        public void InsertOrderItem(OrderItems orderItem)
        {
            _context.OrderItems.AddOrUpdate(orderItem);
        }

        [Update]
        public void UpdateOrderItem(OrderItems orderItem)
        {
            _context.OrderItems.AddOrUpdate(orderItem);
        }
    }
}