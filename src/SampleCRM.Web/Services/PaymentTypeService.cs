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
        public IQueryable<PaymentType> GetPaymentTypes() => _context.PaymentTypes;

        public PaymentType GetPaymentTypeById(long paymentTypeId) => 
            GetPaymentTypes().SingleOrDefault(x => x.PaymentTypeID == paymentTypeId);

        [Delete]
        [RestrictAccessReadonlyMode]
        public void DeletePaymentType(PaymentType paymentType)
        {
            _context.PaymentTypes.Remove(paymentType);
        }

        [Insert]
        [RestrictAccessReadonlyMode]
        public void InsertPaymentType(PaymentType paymentType)
        {
            _context.PaymentTypes.AddOrUpdate(paymentType);
        }

        [Update]
        [RestrictAccessReadonlyMode]
        public void UpdatePaymentType(PaymentType paymentType)
        {
            _context.PaymentTypes.AddOrUpdate(paymentType);
        }
    }
}