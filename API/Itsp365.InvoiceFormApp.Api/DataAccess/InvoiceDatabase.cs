using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;

namespace ITSP365.InvoiceFormApp.Api.DataAccess
{
    public class InvoiceDatabase
    {
        private TelemetryClient _telemetryClient = new TelemetryClient();
        private string _endPoint;
        private string _primaryKey;
        private string _databaseName;
        private DocumentClient _client = null;
        private Database _database;

        private static InvoiceDatabase _invoiceDatabase = null;

        public HttpStatusCode? Httpstatuscode { get; private set; }

        private InvoiceDatabase()
        {

        }

        private InvoiceDatabase(string databaseEndPoint, string primaryKey, string databaseName)
        {
            _endPoint = databaseEndPoint;
            _primaryKey = primaryKey;
            _databaseName = databaseName;

        }

        public static async Task<InvoiceDatabase> GetCurrent(string databaseEndPoint, string primaryKey, string databaseName)
        {
            if (_invoiceDatabase == null)
            {
                _invoiceDatabase = new InvoiceDatabase(databaseEndPoint, primaryKey, databaseName);
                await _invoiceDatabase.Initialise();
            }

            return _invoiceDatabase;
        }

        public Database Database
        {
            get
            {
                return _database;
            }
        }

        public DocumentClient Client
        {
            get
            {
                return _client;
            }
        }

        private async Task Initialise()
        {
            var endPointUri = new Uri(_endPoint);
            _client = new DocumentClient(endPointUri, _primaryKey, ConnectionPolicy.Default, Microsoft.Azure.Documents.ConsistencyLevel.Session);
            _database = await EnsureDatabase();
            
        }

        private bool DoesDatabaseExist()
        {
            var bExists = false;

            var database = GetDatabase();
            bExists = database != null ? true : false;
            return bExists;
        }

        private async Task<Database> CreateDatabase()
        {
            Database database = null;

            try
            {
                var databaseStub = new Database{ Id = _databaseName };
                database = await this._client.CreateDatabaseAsync(databaseStub).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }

            return database;
        }

        private async Task<Database> GetDatabase()
        {
            Database database = null;

            try
            {
                var databaseUri = UriFactory.CreateDatabaseUri(_databaseName);
                var response = await _client.ReadDatabaseAsync(databaseUri); ;
                database = response.Resource;
            }
            catch (DocumentClientException clientException)
            {
                if (clientException.StatusCode == HttpStatusCode.NotFound)
                {
                    database = null;
                }
                else
                {
                    throw;
                }
                
            }
            catch(Exception ex)
            {
                throw;
            }

            return database;
        }

        private async Task<Database> EnsureDatabase()
        {
            Database database = null;

            try
            {
                database = await GetDatabase();
                if (database==null)
                {
                    database = await CreateDatabase();           
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return database;
        }

        
       
    }
}