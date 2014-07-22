using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "Invoices")]
    public class Invoices : IEntity
    {
        private string invoiceIdPK;
        [EntityProperty(IsPersistent=false)]
        public string InvoiceIdPK
        {
            get { return invoiceIdPK; }
            set { invoiceIdPK = value; }
        }

        private int idFactNumber;
        private string idTypeInvoice;
        private int idYear;

        public Invoices() { }
        public Invoices(int idFactNumber, string type, int idYear)
        {
            this.idFactNumber = idFactNumber; 
            this.idTypeInvoice = type;
            this.idYear = idYear;
        }

        [EntityProperty(ColumnName = "IdFactNumber", IsPrimaryKey = true)]
        public int IdFactNumber
        {
            get { return idFactNumber; }
            set { idFactNumber = value; }
        }

        [EntityProperty(ColumnName = "IdTypeInvoice", IsPrimaryKey = true)]
        public string IdTypeInvoice
        {
            get { return idTypeInvoice; }
            set { idTypeInvoice = value; }
        }

        [EntityProperty(ColumnName = "IdYear", IsPrimaryKey = true)]
        public int IdYear
        {
            get { return idYear; }
            set { idYear = value; }
        }

        private DateTime? date;
        [EntityProperty(ColumnName = "Date")]
        public DateTime? Date
        {
            get { return date; }
            set { date = value; }
        }

        private string currency;
        [EntityProperty(ColumnName = "Currency")]
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        private double? totalHtvaEuro = 0;
        [EntityProperty(ColumnName = "TotalHtvaEuro")]
        public double? TotalHtvaEuro
        {
            get { return totalHtvaEuro; }
            set { totalHtvaEuro = value; }
        }

        private double? amountVatEuro = 0;
        [EntityProperty(ColumnName = "amountVatEuro")]
        public double? AmountVatEuro
        {
            get { return amountVatEuro; }
            set { amountVatEuro = value; }
        }

        private double? totalAmountIncludeVatEuro = 0;
        [EntityProperty(IsPersistent=false)]
        public double? TotalAmountIncludeVatEuro
        {
            get { return totalAmountIncludeVatEuro; }
            set { totalAmountIncludeVatEuro = value; }
        }

        private DateTime? dateOfPayement;
        [EntityProperty(ColumnName = "DateOfPayement")]
        public DateTime? DateOfPayement
        {
            get { return dateOfPayement; }
            set { dateOfPayement = value; }
        }

        private DateTime? duedate;
        [EntityProperty(ColumnName = "Duedate")]
        public DateTime? Duedate
        {
            get { return duedate; }
            set { duedate = value; }
        }

        private bool? payement;
        [EntityProperty(ColumnName = "Payement")]
        public bool? Payement
        {
            get { return payement; }
            set { payement = value; }
        }

        private double? totalAmountPayment = 0;
        [EntityProperty(IsPersistent = false)]
        public double? TotalAmountPayment
        {
            get { return totalAmountPayment; }
            set { totalAmountPayment = value; }
        }

        private int? refCustomerNumber;
        [EntityProperty(ColumnName = "RefCustomerNumber")]
        public int? RefCustomerNumber
        {
            get { return refCustomerNumber; }
            set { refCustomerNumber = value; }
        }

        private string remark;
        [EntityProperty(ColumnName = "Remark")]
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        private string remarkToShowed;
        [EntityProperty(IsPersistent=false)]
        public string RemarkToShowed
        {
            get { return remarkToShowed; }
            set { remarkToShowed = value; }
        }

        private string remark_Internal;
        [EntityProperty(ColumnName = "Remark_Internal")]
        public string Remark_Internal
        {
            get { return remark_Internal; }
            set { remark_Internal = value; }
        }

        private string internalRemarkToShowed;
        [EntityProperty(IsPersistent = false)]
        public string InternalRemarkToShowed
        {
            get { return internalRemarkToShowed; }
            set { internalRemarkToShowed = value; }
        }

        private int? invnumber;
        [EntityProperty(ColumnName = "invnumber")]
        public int? Invnumber
        {
            get { return invnumber; }
            set { invnumber = value; }
        }

        private string companyName;
        [EntityProperty(IsPersistent=false)]
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        private int? companyId;
        [EntityProperty(IsPersistent = false)]
        public int? CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        private int? idFactNumberNew;
        [EntityProperty(IsPersistent = false)]
        public int? IdFactNumberNew
        {
            get { return idFactNumberNew; }
            set { idFactNumberNew = value; }
        }

        private bool? factoring;
        [EntityProperty(ColumnName = "Factoring")]
        public bool? Factoring
        {
            get { return factoring; }
            set { factoring = value; }
        }
    }
}
