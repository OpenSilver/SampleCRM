using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Attributes;
using SampleCRM.Web.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class ShippersService: SampleCRMService
    {
        [Query]
        public IQueryable<Shipper> GetShippers() => _context.Shippers.OrderBy(s => s.Name);

        public Shipper GetShipperById(long shipperId) => 
            GetShippers().SingleOrDefault(x => x.ShipperID == shipperId);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteShippers(Shipper shipper)
        {
            _context.Shippers.Remove(shipper);
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertShippers(Shipper shipper)
        {
            _context.Shippers.AddOrUpdate(shipper);
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateShippers(Shipper shipper)
        {
            _context.Shippers.AddOrUpdate(shipper);
        }
    }
}