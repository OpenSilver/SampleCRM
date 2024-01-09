using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Attributes;
using SampleCRM.Web.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class TaxTypesService : SampleCRMService
    {
        [Query]
        public IQueryable<TaxTypes> GetTaxTypes()
        {
            return _context.TaxTypes.OrderBy(t => t.Name);
        }

        public TaxTypes GetTaxTypeById(long taxTypeId)
        {
            return _context.TaxTypes.SingleOrDefault(x => x.TaxTypeID == taxTypeId);
        }

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteTaxTypes(TaxTypes taxType)
        {
            _context.TaxTypes.Remove(taxType);
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertTaxTypes(TaxTypes taxType)
        {
            _context.TaxTypes.AddOrUpdate(taxType);
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateTaxTypes(TaxTypes taxType)
        {
            _context.TaxTypes.AddOrUpdate(taxType);
        }
    }
}