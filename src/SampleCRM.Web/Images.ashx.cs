using System;
using System.Net;
using System.Web;
using System.Linq;

namespace SampleCRM.Web
{
    public class ImagesHandler : IHttpHandler
    {
        private const string QUERYPARAM_CUSTOMER_ID = "customerid";
        private const string QUERYPARAM_PRODUCT_ID = "productid";

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString.HasKeys()
                && context.Request.QueryString.AllKeys.Contains(QUERYPARAM_PRODUCT_ID))
            {
                GetProductImage(context);
                return;
            }

            if (context.Request.QueryString.HasKeys()
                && context.Request.QueryString.AllKeys.Contains(QUERYPARAM_CUSTOMER_ID))
            {
                GetCustomerImage(context);
                return;
            }

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.End();
            return;
        }

        private static void GetCustomerImage(HttpContext context)
        {
            if (!long.TryParse(context.Request.QueryString[QUERYPARAM_CUSTOMER_ID], out var customerid))
                throw new MissingFieldException($"{QUERYPARAM_CUSTOMER_ID} required");

            if (customerid < 1)
                throw new MissingFieldException($"{QUERYPARAM_CUSTOMER_ID} required");

            var sampleCRMService = new SampleCRMService();
            var pict = sampleCRMService.GetCustomerPicture(customerid);
            context.Response.ContentType = "image/jpeg";
            context.Response.BinaryWrite(pict);
        }

        private static void GetProductImage(HttpContext context)
        {
            try
            {
                var productId = context.Request.QueryString[QUERYPARAM_PRODUCT_ID];
                if (string.IsNullOrEmpty(productId))
                    throw new MissingFieldException($"{QUERYPARAM_PRODUCT_ID} required");

                var sampleCRMService = new SampleCRMService();
                var pict = sampleCRMService.GetProductPicture(productId);
                context.Response.ContentType = "image/jpeg";
                context.Response.BinaryWrite(pict);
            }
            catch (MissingFieldException me)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.Status = me.Message;
            }
            catch (ArgumentNullException ea)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.Status = ea.Message;
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.Status = e.Message;
            }
            finally
            {
                context.Response.End();
            }
        }

        public bool IsReusable => true;
    }
}