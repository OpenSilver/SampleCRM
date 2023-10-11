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
        [RestrictAccessDeveloperMode]
        public void DeleteOrderItem(OrderItems orderItem)
        {
            var dOrderItem = _context.OrderItems.FirstOrDefault(x => x.OrderID == orderItem.OrderID && x.OrderLine == orderItem.OrderLine);
            if (dOrderItem == null)
                return;

            _context.OrderItems.Remove(dOrderItem);
            _context.SaveChanges();
        }

        [Insert]
        [RestrictAccessDeveloperMode]
        public void InsertOrderItem(OrderItems orderItem)
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
        [RestrictAccessDeveloperMode]
        public void UpdateOrderItem(OrderItems orderItem)
        {
            _context.OrderItems.AddOrUpdate(orderItem);
            _context.SaveChanges();
        }
    }
}