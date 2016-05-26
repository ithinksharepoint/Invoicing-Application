using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Itsp365.InvoiceFormApp.Api.Models.Entities;
using ITSP365.InvoiceFormApp.Api.DataAccess;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Itsp365.InvoiceFormApp.Api.Repositories
{

    public class InvoiceRepository
    {
        private static InvoiceRepository _instance = null;
        private List<InvoiceForm> _repository;
        private InvoiceDataContext _dataContext;

        private InvoiceRepository()
        {
            _repository = new List<InvoiceForm>();

            var invoice = new InvoiceForm();
            invoice.Id = 1;
            invoice.Reference = "ACME01";
            invoice.CompanyName = "Acme University";
            invoice.AgencyName = "Acme Agency";
            invoice.AgencyContact = "Bugs Bunny";
            invoice.CurrencyType = "GBP";
            invoice.InvoiceDate = DateTime.UtcNow;
            invoice.VatRate = 0.2;

            var invoiceLine = new InvoiceLine();
            invoiceLine.Description = "SharePoint Project";
            invoiceLine.UnitType = "Day";
            invoiceLine.UnitValue = 200;
            invoiceLine.UnitQuantity = 22;

            invoice.InvoiceLines.Add(invoiceLine);

            _repository.Add(invoice);

        }

        private async Task Initialise()
        {
            _dataContext = await InvoiceDataContext.GetCurrent();
        }

        public static async Task<InvoiceRepository> GetCurrent()
        {
            var telemetryClient = new TelemetryClient();
            try
            {
                if (_instance == null || (_instance != null && _instance._dataContext == null))
                {
                    _instance = new InvoiceRepository();
                    await _instance.Initialise();
                }
            }
            catch (Exception ex)
            {
                telemetryClient.TrackException(ex);
                throw;
            }

            return _instance;
        }

        private IOrderedQueryable<InvoiceForm> GetAllQuery()
        {
            var feedOptions = new FeedOptions { MaxItemCount = -1 };
            var documentCollectionUri = new Uri(_dataContext.DocumentCollection.DocumentCollection.AltLink, UriKind.RelativeOrAbsolute);
            var query = _dataContext.Database.Client.CreateDocumentQuery<InvoiceForm>(documentCollectionUri, feedOptions);
            return query;
        }

        public IList<InvoiceForm> GetAll()
        {
            var query = GetAllQuery();
            return query.ToList();
        }

        public InvoiceForm Get(string reference)
        {
            InvoiceForm form = new InvoiceNotFoundForm();
            var result = GetAllQuery().Where(f => f.Reference == reference).ToList();
            if (result.Count > 0)
            {
                form = result.First();
            }
            return form;
        }

        public async Task<bool> Add(InvoiceForm form)
        {
            if (string.IsNullOrEmpty(form.Reference))
            {
                throw new ArgumentException("invoice does not have a reference field set.");
            }


            form.Id = form.Id;

            var documentCollectionUri = new Uri(_dataContext.DocumentCollection.DocumentCollection.AltLink, UriKind.RelativeOrAbsolute);
            var addedDocument = await _dataContext.Database.Client.CreateDocumentAsync(documentCollectionUri, form);

            return true;
        }

        public async Task<bool> Remove(int Id)
        {
            bool bSuccess = false;

            var form = GetAllQuery().FirstOrDefault(f => f.Id == Id);
            if (form != null)
            {
                var documentUri = UriFactory.CreateDocumentUri(_dataContext.Database.Database.Id, _dataContext.DocumentCollection.DocumentCollection.Id, form.Reference);
                await _dataContext.Database.Client.DeleteDocumentAsync(documentUri);
                bSuccess = true;
            }
            return bSuccess;
        }

        public async Task<bool> Update(InvoiceForm updatingForm)
        {
            bool bSuccess = false;
            try
            {


                var documentCollectionUri = new Uri(_dataContext.DocumentCollection.DocumentCollection.AltLink, UriKind.RelativeOrAbsolute);
                string docId = updatingForm.Id.ToString();
                var doc = _dataContext.Database.Client.CreateDocumentQuery<Document>(documentCollectionUri).Where(d => d.Id == docId).AsEnumerable().SingleOrDefault();

                if (doc != null)
                {
                    InvoiceForm form = (dynamic)doc;
                    
                    form.ModifiedBy = updatingForm.ModifiedBy;
                    form.Modified = DateTime.UtcNow;
                    form.Reference = updatingForm.Reference;
                    form.AddressCity = updatingForm.AddressCity;
                    form.AddressCountry = updatingForm.AddressCountry;
                    form.AddressPostCode = updatingForm.AddressPostCode;
                    form.AddressCounty = updatingForm.AddressCounty;
                    form.AddressLine1 = updatingForm.AddressLine1;
                    form.AddressLine2 = updatingForm.AddressLine2;
                    form.AddressLine3 = updatingForm.AddressLine3;
                    form.AddressLine4 = updatingForm.AddressLine4;
                    form.AgencyContact = updatingForm.AgencyContact;
                    form.AgencyName = updatingForm.AgencyName;
                    form.CompanyName = updatingForm.CompanyName;
                    form.Contact = updatingForm.Contact;
                    form.CurrencyType = updatingForm.CurrencyType;
                    form.InvoiceDate = updatingForm.InvoiceDate;
                    form.VatRate = updatingForm.VatRate;
                    form.InvoiceLines = updatingForm.InvoiceLines;

                    Document updated = await _dataContext.Database.Client.ReplaceDocumentAsync(doc.SelfLink, form);

                    bSuccess = true;
                }
            }
            catch (DocumentClientException clientEx)
            {
                throw;
            }

            return bSuccess;
        }
    }
}