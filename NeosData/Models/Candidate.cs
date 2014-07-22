using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Runtime.Serialization;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblCandidat")]
    public class Candidate : IEntity
    {
        #region private variables
        private int candidateId = -1;
        private string unit;
        private string lastName;
        private string firstName;
        private string fullName;
        private string address;
        private string zipCode;
        private string city;
        private string countryCode;
        private string phoneArea;
        private string nationlity;
        private DateTime? birthDate;
        private string civilStatus;
        private string gender;
        private string drivingLicenceType;
        private bool hasCar;
        private DateTime? dateOfInterView;
        private string remark;
        private bool? inactive;
        private string inactiveString;
        private string reasonDesactivation;
        private string interviewer;
        private DateTime? creationDate;
        private string languageCode;
        private string locationCode;
        private bool veirify;
        private string cvMail;
        private string presentation;
        private int tempOldNbr;
        private bool confidentel;
        private string genre;
        private DateTime? lastModifDate;        
        private string provenence;
        private bool newsleter;
        private string contactInfo;
        private string contactInfoFull;

        private IList<CandidateTelephone> contactList = new List<CandidateTelephone>();
        #endregion

        #region properties
        [EntityProperty(ColumnName = "CandidatID", IsIdentity = true, IsPrimaryKey = true)]
        public int CandidateId
        {
            get { return candidateId; }
            set { candidateId = value; }
        }
        [EntityProperty(ColumnName = "Unit")]
        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        [EntityProperty(ColumnName = "Nom")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        [EntityProperty(ColumnName = "Prenom")]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [EntityProperty(IsPersistent = false)]
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        [EntityProperty(ColumnName = "Adresse")]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        [EntityProperty(ColumnName = "CodePostal")]
        public string ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }
        [EntityProperty(ColumnName = "Commune")]
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        [EntityProperty(ColumnName = "CodePays")]
        public string CountryCode
        {
            get { return countryCode; }
            set { countryCode = value; }
        }
        [EntityProperty(ColumnName = "ZoneTelephonique")]
        public string PhoneArea
        {
            get { return phoneArea; }
            set { phoneArea = value; }
        }
        [EntityProperty(ColumnName = "Nationalite")]
        public string Nationlity
        {
            get { return nationlity; }
            set { nationlity = value; }
        }
        [EntityProperty(ColumnName = "DateNaissance")]
        public DateTime? BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; }
        }
        private string birthLocation;
        [EntityProperty(ColumnName = "LieuNaissance")]
        public string BirthLocation
        {
            get { return birthLocation; }
            set { birthLocation = value; }
        }
        [EntityProperty(ColumnName = "SituationCivile")]
        public string CivilStatus
        {
            get { return civilStatus; }
            set { civilStatus = value; }
        }
        [EntityProperty(ColumnName = "Sexe")]
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        [EntityProperty(ColumnName = "PermisConduire")]
        public string DrivingLicenceType
        {
            get { return drivingLicenceType; }
            set { drivingLicenceType = value; }
        }
        [EntityProperty(ColumnName = "Voitue")]
        public bool HasCar
        {
            get { return hasCar; }
            set { hasCar = value; }
        }
        [EntityProperty(ColumnName = "DateInterview")]
        public DateTime? DateOfInterView
        {
            get { return dateOfInterView; }
            set { dateOfInterView = value; }
        }
        [EntityProperty(ColumnName = "Remarques")]
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
        [EntityProperty(ColumnName = "inactif")]
        public bool? Inactive
        {
            get { return inactive; }
            set { inactive = value; }
        }
        [EntityProperty(IsPersistent = false)]
        public string InactiveString
        {
            get { return inactiveString; }
            set { inactiveString = value; }
        }

        [EntityProperty(ColumnName = "RaisonInactif")]
        public string ReasonDesactivation
        {
            get { return reasonDesactivation; }
            set { reasonDesactivation = value; }
        }
        [EntityProperty(ColumnName = "Interviewer")]
        public string Interviewer
        {
            get { return interviewer; }
            set { interviewer = value; }
        }
        [EntityProperty(ColumnName = "DateCreation")]
        public DateTime? CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        [EntityProperty(ColumnName = "CodeLangue")]
        public string LanguageCode
        {
            get { return languageCode; }
            set { languageCode = value; }
        }
        [EntityProperty(ColumnName = "CodeLocation")]
        public string LocationCode
        {
            get { return locationCode; }
            set { locationCode = value; }
        }
        [EntityProperty(ColumnName = "verifie")]
        public bool Veirify
        {
            get { return veirify; }
            set { veirify = value; }
        }
        [EntityProperty(ColumnName = "CVMail")]
        public string CVMail
        {
            get { return cvMail; }
            set { cvMail = value; }
        }
        [EntityProperty(ColumnName = "presentation")]
        public string Presentation
        {
            get { return presentation; }
            set { presentation = value; }
        }
        [EntityProperty(ColumnName = "TempOldNbr")]
        public int TempOldNbr
        {
            get { return tempOldNbr; }
            set { tempOldNbr = value; }
        }
        [EntityProperty(ColumnName = "confidentiel")]
        public bool Confidentel
        {
            get { return confidentel; }
            set { confidentel = value; }
        }
        [EntityProperty(ColumnName = "Genre")]
        public string Genre
        {
            get { return genre; }
            set { genre = value; }
        }
        [EntityProperty(ColumnName = "DateModif")]
        public DateTime? LastModifDate
        {
            get { return lastModifDate; }
            set { lastModifDate = value; }
        }

        [EntityProperty(ColumnName = "provenance")]
        public string Provenence
        {
            get { return provenence; }
            set { provenence = value; }
        }

        [EntityProperty(ColumnName = "newsletter")]
        public bool Newsleter
        {
            get { return newsleter; }
            set { newsleter = value; }
        }
        [EntityProperty(IsPersistent = false)]
        public string ContactInfo
        {
            get { return contactInfo; }
            set { contactInfo = value; }
        }
        [EntityProperty(IsPersistent = false)]
        public string ContactInfoFull
        {
            get { return contactInfoFull; }
            set { contactInfoFull = value; }
        }
        #endregion
        #region methods
        public Candidate()
        {
        }
        public Candidate(int cid)
        {
            this.candidateId = cid;
        }
        #endregion
    }
}
