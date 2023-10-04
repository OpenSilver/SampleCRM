using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class ShippersService: SampleCRMService
    {
        [Query]
        public IQueryable<Shippers> GetShippers()
        {
            return _context.Shippers;
        }

        public Shippers GetShipperById(long shipperId)
        {
            return _context.Shippers.SingleOrDefault(x => x.ShipperID == shipperId);
        }

        [Delete]
        public void DeleteShippers(Shippers shipper)
        {
            _context.Shippers.Remove(shipper);
        }

        [Insert]
        public void InsertShippers(Shippers shipper)
        {
            _context.Shippers.AddOrUpdate(shipper);
        }

        [Update]
        public void UpdateShippers(Shippers shipper)
        {
            _context.Shippers.AddOrUpdate(shipper);
        }
    }
}