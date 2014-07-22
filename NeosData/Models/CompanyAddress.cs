using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{

    [EntityClass(DbTableName = "tblSocieteAdressesFacturation")]
    public class CompanyAddress : IEntity
    {
        public CompanyAddress() { }
        public CompanyAddress(int adrID) {
            this.addressID = adrID;
        }

        private int addressID;
        [EntityProperty(ColumnName = "ID", IsIdentity = true, IsPrimaryKey = true)]
        public int AddressID
        {
            get { return addressID; }
            set { addressID = value; }
        }

        private int companyID;
        [EntityProperty(ColumnName = "SocieteID")]
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        private string name;
        [EntityProperty(ColumnName = "Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        
        private string address;
        [EntityProperty(ColumnName = "Address")]        
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        private string zipCode;
        [EntityProperty(ColumnName = "ZipCode")]        
        public string ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }

        private string city;
        [EntityProperty(ColumnName = "City")]
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        
        private string vatNumber;
        [EntityProperty(ColumnName = "VatNumber")]
        public string VatNumber
        {
            get { return vatNumber; }
            set { vatNumber = value; }
        }

        private string telephone;
        [EntityProperty(ColumnName = "Tel")]
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        private string fax;
        [EntityProperty(ColumnName = "Fax")]        
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }               

        private string email;
        [EntityProperty(ColumnName = "Email")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string co;
        [EntityProperty(ColumnName = "co")]
        public string Co
        {
            get { return co; }
            set { co = value; }
        }
        private bool isDefault;
        [EntityProperty(ColumnName = "default")]
        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }

        private string factoringCode;
        [EntityProperty(ColumnName = "FactoringCode")]
        public string FactoringCode
        {
            get { return factoringCode; }
            set { factoringCode = value; }
        }
    }
}
