using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    public enum CompanyStatus
    {
        Inactive = 0,
        Client = 1,
        Propect = 2,
    }

    [EntityClass(DbTableName = "tblSociete")]
    public class Company : IEntity
    {
        private int companyID;

        [EntityProperty(ColumnName = "SocieteID", IsIdentity = true, IsPrimaryKey = true)]
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }
        private string nVAT;
        [EntityProperty(ColumnName = "nTva")]
        public string NVAT
        {
            get { return nVAT; }
            set { nVAT = value; }
        }
        
        private string companyName;
        [EntityProperty(ColumnName = "SocNom")]        
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        private string legalForm;
        [EntityProperty(ColumnName = "FormeJuridique")]
        
        public string LegalForm
        {
            get { return legalForm; }
            set { legalForm = value; }
        }

        private string group;
        [EntityProperty(ColumnName = "Groupe")]
        public string Group
        {
            get { return group; }
            set { group = value; }
        }
        
        private string address;
        [EntityProperty(ColumnName = "Adresse")]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        private string zipCode;
        [EntityProperty(ColumnName = "CodePostal")]
        public string ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }
        private string city;
        [EntityProperty(ColumnName = "Commune")]        
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        private string telephoneZone;
        [EntityProperty(ColumnName = "ZoneTel")]        
        public string TelephoneZone
        {
            get { return telephoneZone; }
            set { telephoneZone = value; }
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
        private string activity;
        [EntityProperty(ColumnName = "Activité")]
        public string Activity
        {
            get { return activity; }
            set { activity = value; }
        }
        private string segment;
        [EntityProperty(ColumnName = "Segment")]
        public string Segment
        {
            get { return segment; }
            set { segment = value; }
        }
        private string specialSegment;
        [EntityProperty(ColumnName = "SegmentSpecialisation")]
        public string SpecialSegment
        {
            get { return specialSegment; }
            set { specialSegment = value; }
        }

        private int? status;
        /// <summary>
        /// return the number (0,1,2,..) represent the status of company
        /// </summary>
        [EntityProperty(ColumnName = "Statut")]
        public int? Status
        {
            get { return status; }
            set { status = value; }
        }

        private string responsible;
        [EntityProperty(ColumnName = "Responsable")]
        public string Responsible
        {
            get { return responsible; }
            set { responsible = value; }
        }

        private string responsibleLastName;
        [EntityProperty(IsPersistent = false)]
        public string ResponsibleLastName
        {
            get { return responsibleLastName; }
            set { responsibleLastName = value; }
        }

        private string tariffConditions;
        [EntityProperty(ColumnName = "ConditionsTarif")]
        public string TariffConditions
        {
            get { return tariffConditions; }
            set { tariffConditions = value; }
        }

        private string remark;
        [EntityProperty(ColumnName = "Remarques")]
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        private DateTime? createdDate;
        [EntityProperty(ColumnName = "DateCreation")]
        public DateTime? CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        private string sector;
        [EntityProperty(ColumnName = "Secteur")]
        public string Sector
        {
            get { return sector; }
            set { sector = value; }
        }

        private string unitCode;
        [EntityProperty(ColumnName = "Unitcode")]
        public string UnitCode
        {
            get { return unitCode; }
            set { unitCode = value; }
        }
        private string webLink;
        [EntityProperty(ColumnName = "WebLink")]
        public string WebLink
        {
            get { return webLink; }
            set { webLink = value; }
        }

        private bool? sponsorArea;
        [EntityProperty(ColumnName = "ZoneSponsor")]
        public bool? SponsorArea
        {
            get { return sponsorArea; }
            set { sponsorArea = value; }
        }

        private string statusLable;
        /// <summary>
        /// return the client status of the company (Inactif, Client, Prospect)
        /// </summary>
        [EntityProperty( IsPersistent=false)]
        public string StatusLabel
        {
            get { return statusLable; }
            set { statusLable = value; }
        }

        public Company() {}

        public Company(int cid)
        {
            this.companyID = cid;
        }
    }
}
