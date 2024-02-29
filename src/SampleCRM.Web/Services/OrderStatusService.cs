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
        public IQueryable<OrderStatu> GetOrderStatus() => _context.OrderStatus;

        public OrderStatu GetOrderStatusById(long statusId) => 
            GetOrderStatus().SingleOrDefault(x => x.Status == statusId);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteStatus(OrderStatu status)
        {
            _context.OrderStatus.Remove(status);
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertStatus(OrderStatu status)
        {
            _context.OrderStatus.AddOrUpdate(status);
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateStatus(OrderStatu status)
        {
            _context.OrderStatus.AddOrUpdate(status);
        }
    }
}