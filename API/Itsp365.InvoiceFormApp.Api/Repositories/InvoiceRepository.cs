using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Itsp365.InvoiceFormApp.Api.Models.Entities;

namespace Itsp365.InvoiceFormApp.Api.Repositories
{

    public class InvoiceRepository
    {
        private static InvoiceRepository _instance = null;
        private List<InvoiceForm> _repository;

        private InvoiceRepository()
        {
            _repository = new List<InvoiceForm>();
            var invoice = new InvoiceForm();
            invoice.Id = 1;
            invoice.Reference = "LCCSD01";
            invoice.CompanyName = "Leeds City Council";
            invoice.AgencyName = "Mondo";
            invoice.AgencyContact = "Danny Collins";
            invoice.CurrencyType = "GBP";
            invoice.InvoiceDate = DateTime.UtcNow;
            invoice.VatRate = 0.2;

            var invoiceLine = new InvoiceLine();
            invoiceLine.Description = "SharePoint Project";
            invoiceLine.UnitType = "Day";
            invoiceLine.UnitValue = 450;
            invoiceLine.UnitQuantity = 22;

            invoice.InvoiceLines.Add(invoiceLine);

            _repository.Add(invoice);

        }

        public static InvoiceRepository GetCurrent()
        {
            if (_instance == null)
            {
                _instance = new InvoiceRepository();
            }

            return _instance;
        }

        public IList<InvoiceForm> GetAll()
        {
            return _repository;
        }

        public InvoiceForm Add(InvoiceForm addingForm)
        {
            addingForm.Id = _repository.Max(i => i.Id) + 1;

            _repository.Add(addingForm);
            return addingForm;
        }

        public bool Remove(long Id)
        {
            bool bSuccess = false;

            var form = _repository.FirstOrDefault(f => f.Id == Id);
            if (form != null)
            {
                _repository.Remove(form);
                bSuccess = true;
            }
            return bSuccess;
        }

        public bool Update(InvoiceForm updatingForm)
        {
            bool bSuccess = false;

            var form = _repository.FirstOrDefault(f => f.Id == updatingForm.Id);
            if (form != null)
            {
                form.ModifiedBy = updatingForm.ModifiedBy;
                form.Modified = updatingForm.Modified;
                form.Reference = updatingForm.Reference;
                form.AddressCity = updatingForm.AddressCity;
                form.AddressCountry = updatingForm.AddressCountry;
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

                bSuccess = true;
            }
            return bSuccess;
        }
    }
}