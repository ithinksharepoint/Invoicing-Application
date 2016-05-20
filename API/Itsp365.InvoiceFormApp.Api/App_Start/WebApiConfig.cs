using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Cors;
using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;

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
             name: "GetAllInvoicesApi",
             routeTemplate: "api/invoice",
             defaults: new { controller = "Invoice", action = "Get" }
         );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{reference}",
                defaults: new { reference = RouteParameter.Optional }
            );

            GlobalConfiguration.Configuration.Formatters.Remove(config.Formatters.XmlFormatter);

            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.UseDataContractJsonSerializer = false;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            GlobalConfiguration.Configuration.Formatters.Add(jsonFormatter);
        }
    }
}
