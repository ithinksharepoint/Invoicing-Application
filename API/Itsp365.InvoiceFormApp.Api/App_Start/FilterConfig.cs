using System.Web;
using System.Web.Mvc;

namespace Itsp365.InvoiceFormApp.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
