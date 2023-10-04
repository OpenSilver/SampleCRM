using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class OrderStatusService : SampleCRMService
    {
        [Query]
        public IQueryable<OrderStatus> GetOrderStatus()
        {
            return _context.OrderStatus;
        }

        public OrderStatus GetOrderStatusById(long statusId)
        {
            return _context.OrderStatus.SingleOrDefault(x => x.Status == statusId);
        }

        [Delete]
        public void DeleteStatus(OrderStatus status)
        {
            _context.OrderStatus.Remove(status);
        }

        [Insert]
        public void InsertStatus(OrderStatus status)
        {
            _context.OrderStatus.AddOrUpdate(status);
        }

        [Update]
        public void UpdateStatus(OrderStatus status)
        {
            _context.OrderStatus.AddOrUpdate(status);
        }
    }
}