using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Attributes;
using SampleCRM.Web.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class PaymentTypeService : SampleCRMService
    {
        [Query]
        public IQueryable<PaymentTypes> GetPaymentTypes() => _context.PaymentTypes;

        public PaymentTypes GetPaymentTypeById(long paymentTypeId) => 
            GetPaymentTypes().SingleOrDefault(x => x.PaymentTypeID == paymentTypeId);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeletePaymentType(PaymentTypes paymentType)
        {
            _context.PaymentTypes.Remove(paymentType);
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertPaymentType(PaymentTypes paymentType)
        {
            _context.PaymentTypes.AddOrUpdate(paymentType);
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdatePaymentType(PaymentTypes paymentType)
        {
            _context.PaymentTypes.AddOrUpdate(paymentType);
        }
    }
}