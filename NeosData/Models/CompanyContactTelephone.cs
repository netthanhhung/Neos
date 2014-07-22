using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblSocieteContactTelephone")]
    public class CompanyContactTelephone : IEntity
    {
        private int contactTelephoneID;
        [EntityProperty(ColumnName = "ContactTelID", IsIdentity = true, IsPrimaryKey = true)]
        public int ContactTelephoneID
        {
            get { return contactTelephoneID; }
            set { contactTelephoneID = value; }
        }
        private int contactID;

        [EntityProperty(ColumnName = "ContactID")]
        public int ContactID
        {
            get { return contactID; }
            set { contactID = value; }
        }

        private string type;        
        /// <summary>
        /// T: phone, F: fax, G: mobile, E: email
        /// </summary>
        [EntityProperty(ColumnName = "Type")]
        
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private string telephoneZone;
        [EntityProperty(ColumnName = "ZoneTel")]
        public string TelephoneZone
        {
            get { return telephoneZone; }
            set { telephoneZone = value; }
        }

        private string tel;
        /// <summary>
        /// phone/fax number or email address
        /// </summary>
        [EntityProperty(ColumnName = "Tel")]        
        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }

        private string location;
        [EntityProperty(ColumnName = "Lieu")]
        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        public CompanyContactTelephone() { }
        public CompanyContactTelephone(int cid)
        {
            this.contactTelephoneID = cid;
        }
        
    }
}
