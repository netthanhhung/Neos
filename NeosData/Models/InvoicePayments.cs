using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "InvoicePayments")]
    public class InvoicePayments : IEntity
    {
        private int idPayment;
        [EntityProperty(ColumnName = "IDPayment", IsIdentity=true, IsPrimaryKey = true)]
        public int IdPayment
        {
            get { return idPayment; }
            set { idPayment = value; }
        }

        private int idFactNumber;
        private string idTypeInvoice;
        private int idYear;        

        public InvoicePayments() { }
        public InvoicePayments(int idPayment)
        {
            this.idPayment = idPayment;            
        }

        [EntityProperty(ColumnName = "IdFactNumber")]
        public int IdFactNumber
        {
            get { return idFactNumber; }
            set { idFactNumber = value; }
        }

        [EntityProperty(ColumnName = "IdTypeInvoice")]
        public string IdTypeInvoice
        {
            get { return idTypeInvoice; }
            set { idTypeInvoice = value; }
        }

        [EntityProperty(ColumnName = "IdYear")]
        public int IdYear
        {
            get { return idYear; }
            set { idYear = Convert.ToInt32(value); }
        }

        private DateTime? datePayment;
        [EntityProperty(ColumnName = "DatePayment")]
        public DateTime? DatePayment
        {
            get { return datePayment; }
            set { datePayment = value; }
        }

        private double? amount;
        [EntityProperty(ColumnName = "Amount")]
        public double? Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private string remark;
        [EntityProperty(ColumnName = "Remark")]
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
    }
}
