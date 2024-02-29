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
        public IQueryable<OrderItem> GetOrderItems(string search) => 
            _context.OrderItems.OrderBy(o => o.OrderLine)
                           .Where(x => x.ProductID.ToString().Contains(search) 
                                       || x.OrderID.ToString().Contains(search)
                                       || search == "")
                           .AsQueryable();

        public OrderItem GetOrderItemById(int orderId, int orderLine) => 
            GetOrderItems(string.Empty)
                .SingleOrDefault(x => x.OrderID == orderId && x.OrderLine == orderLine);

        [Query]
        public IQueryable<OrderItem> GetOrderItemsOfOrder(long orderId, string search) => 
            GetOrderItems(search)
                .Where(x => x.OrderID == orderId);

        [Query]
        public IQueryable<OrderItem> GetOrderItemsOfProduct(string productId, string search) => 
            GetOrderItems(search)
                .Where(x => x.ProductID == productId);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteOrderItem(OrderItem orderItem)
        {
            var dOrderItem = _context.OrderItems
                                     .FirstOrDefault(x => x.OrderID == orderItem.OrderID && x.OrderLine == orderItem.OrderLine);
            if (dOrderItem == null)
                return;

            _context.OrderItems.Remove(dOrderItem);
            _context.SaveChanges();
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertOrderItem(OrderItem orderItem)
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
        [RestrictAccessReadonlyMode]
        public void UpdateOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.AddOrUpdate(orderItem);
            _context.SaveChanges();
        }
    }
}