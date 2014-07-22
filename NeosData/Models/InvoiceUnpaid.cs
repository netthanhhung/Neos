using System;
using System.Collections.Generic;
using System.Text;

namespace Neos.Data
{
    public class InvoiceUnpaid
    {
        public InvoiceUnpaid()
        {

        }
        private int companyID;

        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }
        private string companyName;

        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        private double? dueAmount;

        public double? DueAmount
        {
            get { return dueAmount; }
            set { dueAmount = value; }
        }
        private DateTime? oldestDate;

        public DateTime? OldestDate
        {
            get { return oldestDate; }
            set { oldestDate = value; }
        } 
    }
}
