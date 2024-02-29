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
        public IQueryable<TaxType> GetTaxTypes() => _context.TaxTypes.OrderBy(t => t.Name);

        public TaxType GetTaxTypeById(long taxTypeId) => 
            GetTaxTypes().SingleOrDefault(x => x.TaxTypeID == taxTypeId);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeleteTaxTypes(TaxType taxType)
        {
            _context.TaxTypes.Remove(taxType);
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertTaxTypes(TaxType taxType)
        {
            _context.TaxTypes.AddOrUpdate(taxType);
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdateTaxTypes(TaxType taxType)
        {
            _context.TaxTypes.AddOrUpdate(taxType);
        }
    }
}