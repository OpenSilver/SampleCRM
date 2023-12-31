using System;
using System.Net;
using System.Web;
using System.Linq;

namespace SampleCRM.Web
{
    public class ImagesHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString.HasKeys()
                && context.Request.QueryString.AllKeys.Contains("productid"))
            {
                GetProductImage(context);
                return;
            }

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.End();
            return;
        }

        private static void GetProductImage(HttpContext context)
        {
            try
            {
                var productId = context.Request.QueryString["productid"];
                if (string.IsNullOrEmpty(productId))
                    throw new MissingFieldException("productid required");

                var productService = new ProductsService();
                var pict = productService.GetProductPicture(productId);
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