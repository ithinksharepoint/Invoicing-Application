using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace ITSP365.InvoiceFormApp.Api.DataAccess
{
    internal class DatabaseConfig
    {
        internal string DatabaseEndPoint {
            get
            {
                var value = ConfigurationManager.AppSettings["documentDb:EndPoint"];
                return value;
            }
        }
        internal string PrimaryKey
        {
            get
            {
                var value = ConfigurationManager.AppSettings["documentDb:PrimaryKey"];
                return value;
            }
        }
        internal string SecondaryKey
        {
            get
            {
                var value = ConfigurationManager.AppSettings["documentDb:SecondaryKey"];
                return value;
            }
        }
        internal string DatabaseName
        {
            get
            {
                var value = ConfigurationManager.AppSettings["documentDb:DatabaseName"];
                return value;
            }
        }
        internal string DocumentCollectionName
        {
            get
            {
                var value = ConfigurationManager.AppSettings["documentDb:DocumentCollectionName"];
                return value;
            }
        }


    }
}