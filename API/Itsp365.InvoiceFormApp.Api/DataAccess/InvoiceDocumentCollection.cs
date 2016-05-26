using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ITSP365.InvoiceFormApp.Api.DataAccess
{
    public class InvoiceDocumentCollection
    {
        private InvoiceDatabase _database = null;
        private DocumentCollection _documentCollection = null;
        private string _collectionName = string.Empty;

        private static InvoiceDocumentCollection _invoiceDocumentCollection = null;

        private InvoiceDocumentCollection()
        {

        }

        private InvoiceDocumentCollection(InvoiceDatabase database, string collectionName)
        {
            _database = database;
            _collectionName = collectionName;
        }

        public DocumentCollection DocumentCollection
        {
            get
            {
                return _documentCollection;
            }
        }

        private async Task Initialise()
        {
            _documentCollection = await EnsureDocumentCollection(_collectionName);
        }

        public static async Task<InvoiceDocumentCollection> GetCurrent(InvoiceDatabase database, string collectionName)
        {
            if (_invoiceDocumentCollection == null)
            {
                _invoiceDocumentCollection = new InvoiceDocumentCollection(database, collectionName);
                await _invoiceDocumentCollection.Initialise();
            }

            return _invoiceDocumentCollection;
        }


        private async Task<DocumentCollection> GetDocumentCollection(string collectionName)
        {
            DocumentCollection documentCollection = null;

            try
            {
                var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_database.Database.Id, collectionName);
                documentCollection = await _database.Client.ReadDocumentCollectionAsync(documentCollectionUri);
            }
            catch (DocumentClientException clientException)
            {
                if (clientException.StatusCode == HttpStatusCode.NotFound)
                {
                    documentCollection = null;
                }
                else
                {
                    throw;
                }

            }


            return documentCollection;

        }

        private async Task<DocumentCollection> CreateDocumentCollection(string collectionName)
        {
            DocumentCollection documentCollection = null;

            try
            {
                var documentCollectionInfo = new DocumentCollection { Id = collectionName };
                documentCollectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

                var databaseUri = UriFactory.CreateDatabaseUri(_database.Database.Id);

                var requestOptions = new RequestOptions { OfferThroughput = 400 };

                documentCollection = await _database.Client.CreateDocumentCollectionAsync(databaseUri, documentCollectionInfo, requestOptions);
            }
            catch (DocumentClientException clientException)
            {
                throw;
            }


            return documentCollection;

        }

        private async Task<DocumentCollection> EnsureDocumentCollection(string collectionName)
        {
            DocumentCollection documentCollection = null;
            try
            {
                documentCollection = await GetDocumentCollection(collectionName);
                if (documentCollection == null)
                {
                    documentCollection = await CreateDocumentCollection(collectionName);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return documentCollection;
        }
    }
}