using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using SampleCRM.Web.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SampleCRM.Web
{
    [EnableClientAccess]
    public class PaymentTypeService : SampleCRMService
    {
        [Query]
        public IQueryable<PaymentTypes> GetPaymentTypes()
        {
            return _context.PaymentTypes;
        }

        public PaymentTypes GetPaymentTypeById(long paymentTypeId)
        {
            return _context.PaymentTypes.SingleOrDefault(x => x.PaymentTypeID == paymentTypeId);
        }

        [Delete]
        public void DeletePaymentType(PaymentTypes paymentType)
        {
            _context.PaymentTypes.Remove(paymentType);
        }

        [Insert]
        public void InsertPaymentType(PaymentTypes paymentType)
        {
            _context.PaymentTypes.AddOrUpdate(paymentType);
        }

        [Update]
        public void UpdatePaymentType(PaymentTypes paymentType)
        {
            _context.PaymentTypes.AddOrUpdate(paymentType);
        }
    }
}