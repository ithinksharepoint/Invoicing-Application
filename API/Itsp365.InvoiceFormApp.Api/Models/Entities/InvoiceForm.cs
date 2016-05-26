using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Itsp365.InvoiceFormApp.Api.Models.Entities
{
    public class InvoiceForm
    {
        public InvoiceForm()
        {
            this.InvoiceLines = new List<InvoiceLine>();
        }

        public long Id { get; set; }
        public string Reference { get; set; }
        public string CompanyName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Contact { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressCity { get; set; }
        public string AddressCounty { get; set; }
        public string AddressCountry { get; set; }
        public string AddressPostCode { get; set; }
        public string AgencyName { get; set; }
        public string AgencyContact { get; set; }

        public List<InvoiceLine> InvoiceLines { get; set; }

        public double InvoiceTotal
        {
            get
            {
                double invoiceTotal = 0;

                if (InvoiceLines.Count > 0)
                {
                    invoiceTotal = InvoiceLines.Sum(l => l.LineTotal);
                }

                return invoiceTotal;
            }
        }

        public string CurrencyType { get; set; }

        public double VatRate { get; set; }

        public double VatAmount { get; set; }

        public double InvoiceTotalWithVat
        {
            get
            {
                var totalAmount = VatAmount + InvoiceTotal;
                return totalAmount;
            }
        }

        public string Status { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    public class InvoiceNotFoundForm : InvoiceForm
    {
        public InvoiceNotFoundForm()
        {
            this.Id = -1;
            this.AgencyName = "Not Found";
            this.CompanyName = "Not Found";
            this.Reference = "N/A";
        }
    }
}