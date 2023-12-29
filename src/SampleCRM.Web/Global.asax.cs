using OpenRiaServices.DomainServices.Hosting;
using OpenRiaServices.DomainServices.Server;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;

namespace SampleCRM.Web
{
    public class Global : System.Web.HttpApplication
    {
        public static bool ReadOnlyMode { get; protected set; }

        protected void Application_Start(object sender, EventArgs e)
        {
            ReadOnlyMode = Convert.ToBoolean(ConfigurationManager.AppSettings["ReadOnlyMode"]);

            string path = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            initDomainServiceQueries();
        }

        private void initDomainServiceQueries()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var classes =
                from t in assembly.GetTypes().AsParallel()
                let attributes = t.GetCustomAttributes(typeof(EnableClientAccessAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<EnableClientAccessAttribute>() };

            foreach (var classType in classes)
            {
                var classInstance = Activator.CreateInstance(classType.Type);
                var methods = classType.Type
                                       .GetMethods()
                                       .Where(m => m.GetCustomAttributes(typeof(QueryAttribute), false).Length > 0)
                                       .ToArray();

                foreach (var method in methods)
                {
                    if (!method.GetParameters().Any())
                    {
                        method.Invoke(classInstance, null);
                    }
                }
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                //These headers are handling the "pre-flight" OPTIONS call sent by the browser
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, SOAPAction");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }

            Uri pageRequest = Request.Url;
            string requestPath = pageRequest.GetLeftPart(UriPartial.Path).ToLower();
            if (pageRequest.Scheme == "https" && requestPath.Contains("Images"))
            {
                Response.Redirect("http://" + pageRequest.Host + pageRequest.PathAndQuery, true);
                // Response.Redirect(requestPath, true);
            }

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}
