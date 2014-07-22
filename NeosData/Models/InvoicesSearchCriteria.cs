using System;
using System.Collections.Generic;
using System.Text;

namespace Neos.Data
{
    public class InvoicesSearchCriteria
    {
        public InvoicesSearchCriteria()
        {

        }
        
        private int? invoiceNumberFrom;
        public int? InvoiceNumberFrom
        {
            get { return invoiceNumberFrom; }
            set { invoiceNumberFrom = value; }
        }

        private int? invoiceNumberTo;
        public int? InvoiceNumberTo
        {
            get { return invoiceNumberTo; }
            set { invoiceNumberTo = value; }
        }
        private int? fiscalYear;
        public int? FiscalYear
        {
            get { return fiscalYear; }
            set { fiscalYear = value; }
        }

        private DateTime? invoiceDateFrom;
        public DateTime? InvoiceDateFrom
        {
            get { return invoiceDateFrom; }
            set { invoiceDateFrom = value; }
        }

        private DateTime? invoiceDateTo;
        public DateTime? InvoiceDateTo
        {
            get { return invoiceDateTo; }
            set { invoiceDateTo = value; }
        }

        private string invoiceType;
        public string InvoiceType
        {
            get { return invoiceType; }
            set { invoiceType = value; }
        }

        private int? customer;
        public int? Customer
        {
            get { return customer; }
            set { customer = value; }
        }       
    }
}
