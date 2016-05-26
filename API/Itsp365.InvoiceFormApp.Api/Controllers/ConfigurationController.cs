using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Itsp365.InvoiceFormApp.Controllers
{
    [Authorize]
    public class ConfigurationController : ApiController
    {
        
        public IDictionary<string,string> Get()
        {
            var configuration = new Dictionary<string, string>();
            configuration.Add("SharePointUrl", "https://{sharepoint}.sharepoint.com/sites/dev");

            return configuration;
        }
    }
}