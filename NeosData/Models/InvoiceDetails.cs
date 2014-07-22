using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "InvoiceDetails")]
    public class InvoiceDetails : IEntity
    {
        private string invoiceDetailsId;
        [EntityProperty(IsPersistent=false)]
        public string InvoiceDetailsId
        {
            get { return invoiceDetailsId; }
            set { invoiceDetailsId = value; }
        }


        private int idFactNumber;
        private string idTypeInvoice;
        private int idYear;
        private int idLigneNumber;

        public InvoiceDetails() { }
        public InvoiceDetails(int idFactNumber, string type, int idYear, int idLigneNumber)
        {
            this.idFactNumber = idFactNumber; 
            this.idTypeInvoice = type;
            this.idYear = idYear;
            this.idLigneNumber = idLigneNumber;
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

        [EntityProperty(ColumnName = "IdLigneNumber", IsPrimaryKey = true)]
        public int IdLigneNumber
        {
            get { return idLigneNumber; }
            set { idLigneNumber = value; }
        }

        private string description;
        [EntityProperty(ColumnName = "Description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private double? quantity;
        [EntityProperty(ColumnName = "Quantity")]
        public double? Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        private double? unitPriceEuro;
        [EntityProperty(ColumnName = "UnitPriceEuro")]
        public double? UnitPriceEuro
        {
            get { return unitPriceEuro; }
            set { unitPriceEuro = value; }
        }

        private double? amountEuro;
        [EntityProperty(ColumnName = "AmountEuro")]
        public double? AmountEuro
        {
            get { return amountEuro; }
            set { amountEuro = value; }
        }        

        private int? vatCode;
        [EntityProperty(ColumnName = "VatCode")]
        public int? VatCode
        {
            get { return vatCode; }
            set { vatCode = value; }
        }

        private double? vatRate;
        [EntityProperty(IsPersistent=false)]
        public double? VatRate
        {
            get { return vatRate; }
            set { vatRate = value; }
        }

        private double? amountVAT = 0;
        [EntityProperty(IsPersistent=false)]
        public double? AmountVAT
        {
            get { return amountVAT; }
            set { amountVAT = value; }
        }

        private double? totalAmountVAT = 0;
        [EntityProperty(IsPersistent = false)]
        public double? TotalAmountVAT
        {
            get { return totalAmountVAT; }
            set { totalAmountVAT = value; }
        }
    }
}
