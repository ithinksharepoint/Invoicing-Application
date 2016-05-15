using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace Itsp365.InvoiceFormApp.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var corsConfiguration = new EnableCorsAttribute("http://localhost:8080,http://127.0.0.1:8080,https://itsp365invoiceformappapitest.azurewebsites.net", "*", "*");
            config.EnableCors(corsConfiguration);


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
