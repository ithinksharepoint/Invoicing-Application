using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ITSP365.InvoiceFormApp.Api.DataAccess
{
    public class InvoiceDataContext
    {
        private InvoiceDatabase _database;
        private InvoiceDocumentCollection _documentCollection;
        private DatabaseConfig _databaseConfiguration = null;

        private static InvoiceDataContext _invoiceDataContext =null;

        private InvoiceDataContext()
        {
           
        }

        public static async Task<InvoiceDataContext> GetCurrent()
        {
            if (_invoiceDataContext == null)

            {
                _invoiceDataContext = new InvoiceDataContext();
                await _invoiceDataContext.Initialise();
            }

            return _invoiceDataContext;
        }

        public InvoiceDatabase Database { get { return _database; } }
        public InvoiceDocumentCollection DocumentCollection { get { return _documentCollection ;} }

        private async Task Initialise()
        {
            _databaseConfiguration = new DatabaseConfig();
            _database = await InvoiceDatabase.GetCurrent(_databaseConfiguration.DatabaseEndPoint, _databaseConfiguration.SecondaryKey, _databaseConfiguration.DatabaseName);
            _documentCollection = await InvoiceDocumentCollection.GetCurrent(_database, _databaseConfiguration.DocumentCollectionName);
        }
    }
}