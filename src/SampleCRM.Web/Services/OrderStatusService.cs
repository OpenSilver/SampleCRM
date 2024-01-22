using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Attributes;
using SampleCRM.Web.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class OrderStatusService : SampleCRMService
    {
        [Query]
        public IQueryable<OrderStatus> GetOrderStatus() => _context.OrderStatus;

        public OrderStatus GetOrderStatusById(long statusId) => 
            GetOrderStatus().SingleOrDefault(x => x.Status == statusId);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteStatus(OrderStatus status)
        {
            _context.OrderStatus.Remove(status);
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertStatus(OrderStatus status)
        {
            _context.OrderStatus.AddOrUpdate(status);
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateStatus(OrderStatus status)
        {
            _context.OrderStatus.AddOrUpdate(status);
        }
    }
}