using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "InvoiceVatCodes")]
    public class InvoiceVatCodes : IEntity
    {
        private int idVatCode;
        private double? tauxVat;

        public InvoiceVatCodes() { }
        public InvoiceVatCodes(int idVatCode)
        {
            this.idVatCode = idVatCode;            
        }

        [EntityProperty(ColumnName = "IdVatCode", IsIdentity = true, IsPrimaryKey = true)]
        public int IdVatCode
        {
            get { return idVatCode; }
            set { idVatCode = value; }
        }


        [EntityProperty(ColumnName = "TauxVat")]
        public double? TauxVat
        {
            get { return tauxVat; }
            set { tauxVat = value; }
        }
    }
}
