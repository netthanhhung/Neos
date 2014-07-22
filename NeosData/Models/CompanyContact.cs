using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblSocieteContact")]
    public class CompanyContact : IEntity
    {
        private int contactID;
        [EntityProperty(ColumnName = "ContactID", IsIdentity = true, IsPrimaryKey = true)]
        public int ContactID
        {
            get { return contactID; }
            set { contactID = value; }
        }
        private int companyID;

        [EntityProperty(ColumnName = "SocieteID")]
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        private string lastName;
        [EntityProperty(ColumnName = "Nom")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private string firstName;
        [EntityProperty(ColumnName = "Prenom")]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        private string fullName;
        [EntityProperty(IsPersistent = false)]
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }
        private string gender;
        [EntityProperty(ColumnName = "Sexe")]        
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        private string position;
        /// <summary>
        /// function
        /// </summary>
        [EntityProperty(ColumnName = "Fonction")]
        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        private string language;
        [EntityProperty(ColumnName = "Langue")]
        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        private string mailing;
        [EntityProperty(ColumnName = "Mailing")]
        public string Mailing
        {
            get { return mailing; }
            set { mailing = value; }
        }

        private string remark;
        [EntityProperty(ColumnName = "Remarque")]
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        private string level;
        [EntityProperty(ColumnName = "Niveau")]        
        public string Level
        {
            get { return level; }
            set { level = value; }
        }

        private bool newsletter;
        [EntityProperty(ColumnName = "newsletter")]
        public bool Newsletter
        {
            get { return newsletter; }
            set { newsletter = value; }
        }
        private List<CompanyContactTelephone> contactInfo = new List<CompanyContactTelephone>();
        [EntityProperty(IsPersistent = false)]
        public List<CompanyContactTelephone> ContactInfo
        {
            get 
            {
                if ((contactInfo == null || contactInfo.Count == 0) && (this.contactID > 0))
                {
                    return new CompanyContactTelephoneRepository().GetContactInfo(this.contactID);
                }
                return contactInfo; 
            }
            set
            {
                contactInfo = value;
            }
        }

        public CompanyContact() { }
        public CompanyContact(int cid)
        {
            this.contactID = cid;
        }

        public CompanyContactTelephone GetContactEmail()
        {
            List<CompanyContactTelephone> list = new List<CompanyContactTelephone>();
            if ((contactInfo == null || contactInfo.Count == 0) && (this.contactID > 0))
                list = new CompanyContactTelephoneRepository().GetContactInfo(this.contactID);
            else
                list = contactInfo;
            if (list.Count <= 0) return null;
            //.Find(delegate(RailOneWS.proposedprice p) { return p.proposedpriceid == pid; });
            CompanyContactTelephone info = list.Find(delegate(CompanyContactTelephone ctInfo) { return ctInfo.Type == "E"; });
            return info;
        }
    }
}
