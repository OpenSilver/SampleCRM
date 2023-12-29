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
            var productId = context.Request.QueryString["productid"];
            if (string.IsNullOrEmpty(productId))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.End();
                return;
            }

            var productService = new ProductsService();
            try
            {
                var pict = productService.GetProductPicture(productId);
                //context.Response.ContentType = "image/jpeg";
                //context.Response.BinaryWrite(pict);
                //context.Response.StatusCode = (int)HttpStatusCode.OK;

                //clear the buffer stream
                context.Response.ClearHeaders();
                context.Response.Clear();
                context.Response.Buffer = true;

                //set the correct ContentType
                context.Response.ContentType = "image/jpeg";

                //set the filename for the image
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"ImageName.bmp\"");

                //set the correct length of the string being send
                context.Response.AddHeader("content-Length", pict.Length.ToString());

                //send the byte array to the browser
                context.Response.OutputStream.Write(pict, 0, pict.Length);

                //cleanup
                context.Response.Flush();
                context.ApplicationInstance.CompleteRequest();
            }
            catch (ArgumentNullException ea)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.Status = ea.Message;
                context.Response.End();
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.Status = e.Message;
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}