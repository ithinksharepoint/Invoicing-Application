using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Itsp365.InvoiceFormApp.Api.Models.Entities
{
    public class InvoiceLine
    {
        public string Description { get; set; }
        public double UnitQuantity { get; set; }
        public string UnitType { get; set; }
        public double UnitValue { get; set; }
        public double LineTotal
        {
            get
            {
                var lineTotal = UnitQuantity * UnitValue;
                return lineTotal;
            }
        }

    }
}