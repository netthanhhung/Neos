using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblJobs")]
    public class Job : IEntity
    {
        private int jobID;
        [EntityProperty(ColumnName = "Ref", IsIdentity = true, IsPrimaryKey = true)]
        public int JobID
        {
            get { return jobID; }
            set { jobID = value; }
        }

        private bool isActive;
        [EntityProperty(ColumnName = "JobActive")]
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        private DateTime? createdDate;
        [EntityProperty(ColumnName = "Date")]
        public DateTime? CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        private string title;
        [EntityProperty(ColumnName = "Title")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string companyDescription;
        [EntityProperty(ColumnName = "Company_Description")]
        public string CompanyDescription
        {
            get { return companyDescription; }
            set { companyDescription = value; }
        }
        private string jobDescription;
        [EntityProperty(ColumnName = "Job_Description")]
        public string JobDescription
        {
            get { return jobDescription; }
            set { jobDescription = value; }
        }

        private string personalDescription;
        [EntityProperty(ColumnName = "Personal_Description")]
        public string PersonalDescription
        {
            get { return personalDescription; }
            set { personalDescription = value; }
        }

        private int? profileID;
        [EntityProperty(ColumnName = "Profile")]
        public int? ProfileID
        {
            get { return profileID; }
            set { profileID = value; }
        }

        private string location;
        [EntityProperty(ColumnName = "Location")]
        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        private string careerManager;
        [EntityProperty(ColumnName = "CM")]
        public string CareerManager
        {
            get { return careerManager; }
            set { careerManager = value; }
        }

        private string careerManagerTitle;
        [EntityProperty(ColumnName = "CMTitre")]
        public string CareerManagerTitle
        {
            get { return careerManagerTitle; }
            set { careerManagerTitle = value; }
        }
        private string careerManagerLastName;
        [EntityProperty(ColumnName = "CMNom")]
        public string CareerManagerLastName
        {
            get { return careerManagerLastName; }
            set { careerManagerLastName = value; }
        }

        private string careerManagerTelephone;
        [EntityProperty(ColumnName = "CMTel")]
        public string CareerManagerTelephone
        {
            get { return careerManagerTelephone; }
            set { careerManagerTelephone = value; }
        }

        private string careerManagerEmail;
        [EntityProperty(ColumnName = "email")]
        public string CareerManagerEmail
        {
            get { return careerManagerEmail; }
            set { careerManagerEmail = value; }
        }

        private string careerManagerDepart;
        [EntityProperty(ColumnName = "CMDepartement")]
        public string CareerManagerDepart
        {
            get { return careerManagerDepart; }
            set { careerManagerDepart = value; }
        }

        private int statClickSite;
        /// <summary>
        /// not used
        /// </summary>
        [EntityProperty(ColumnName = "StatClickSite")]
        public int StatClickSite
        {
            get { return statClickSite; }
            set { statClickSite = value; }
        }

        private int datStatClick;
        /// <summary>
        /// not used
        /// </summary>
        [EntityProperty(ColumnName = "DatStatClick")]
        public int DatStatClick
        {
            get { return datStatClick; }
            set { datStatClick = value; }
        }

        private int? companyID;
        [EntityProperty(ColumnName = "SocieteID")]
        public int? CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }
        private string familyFunctionID;
        [EntityProperty(ColumnName = "FonctionFam")]
        public string FamilyFunctionID
        {
            get { return familyFunctionID; }
            set { familyFunctionID = value; }
        }

        private string packageDescription;
        [EntityProperty(ColumnName = "Package_Description")]
        public string PackageDescription
        {
            get { return packageDescription; }
            set { packageDescription = value; }
        }        

        private bool? isConfidential;
        [EntityProperty(ColumnName = "confidentiel")]
        public bool? IsConfidential
        {
            get { return isConfidential; }
            set { isConfidential = value; }
        }

        private DateTime? expiredDate;
        [EntityProperty(ColumnName = "ExpirationDate")]
        public DateTime? ExpiredDate
        {
            get { return expiredDate; }
            set { expiredDate = value; }
        }
        private DateTime? lastModifiedDate;
        [EntityProperty(ColumnName = "DateModif")]
        public DateTime? LastModifiedDate
        {
            get { return lastModifiedDate; }
            set { lastModifiedDate = value; }
        }

        private string url;
        [EntityProperty(ColumnName = "URL")]
        public string URL
        {
            get { return url; }
            set { url = value; }
        }

        private DateTime? activatedDate;
        [EntityProperty(ColumnName = "ActivationDate")]
        public DateTime? ActivatedDate
        {
            get { return activatedDate; }
            set { activatedDate = value; }
        }

        private DateTime? remindDate;
        [EntityProperty(ColumnName = "DateRemind")]
        public DateTime? RemindDate
        {
            get { return remindDate; }
            set { remindDate = value; }
        }

        private int? nrOfVisites;
        [EntityProperty(ColumnName = "visites")]
        public int? NrOfVisites
        {
            get { return nrOfVisites; }
            set { nrOfVisites = value; }
        }
        private string title_NL;
        [EntityProperty(ColumnName = "Title_NL")]
        public string Title_NL
        {
            get { return title_NL; }
            set { title_NL = value; }
        }

        private string companyDescription_NL;
        [EntityProperty(ColumnName = "Company_Description_NL")]
        public string CompanyDescription_NL
        {
            get { return companyDescription_NL; }
            set { companyDescription_NL = value; }
        }
        private string jobDescription_NL;
        [EntityProperty(ColumnName = "Job_Description_NL")]
        public string JobDescription_NL
        {
            get { return jobDescription_NL; }
            set { jobDescription_NL = value; }
        }

        private string personalDescription_NL;
        [EntityProperty(ColumnName = "Personal_Description_NL")]
        public string PersonalDescription_NL
        {
            get { return personalDescription_NL; }
            set { personalDescription_NL = value; }
        }

        private string packageDescription_NL;
        [EntityProperty(ColumnName = "Package_Description_NL")]
        public string PackageDescription_NL
        {
            get { return packageDescription_NL; }
            set { packageDescription_NL = value; }
        }

        private string title_EN;
        [EntityProperty(ColumnName = "Title_EN")]
        public string Title_EN
        {
            get { return title_EN; }
            set { title_EN = value; }
        }

        private string companyDescription_EN;
        [EntityProperty(ColumnName = "Company_Description_EN")]
        public string CompanyDescription_EN
        {
            get { return companyDescription_EN; }
            set { companyDescription_EN = value; }
        }
        private string jobDescription_EN;
        [EntityProperty(ColumnName = "Job_Description_EN")]
        public string JobDescription_EN
        {
            get { return jobDescription_EN; }
            set { jobDescription_EN = value; }
        }

        private string personalDescription_EN;
        [EntityProperty(ColumnName = "Personal_Description_EN")]
        public string PersonalDescription_EN
        {
            get { return personalDescription_EN; }
            set { personalDescription_EN = value; }
        }

        private string packageDescription_EN;
        [EntityProperty(ColumnName = "Package_Description_EN")]
        public string PackageDescription_EN
        {
            get { return packageDescription_EN; }
            set { packageDescription_EN = value; }
        }

        private int? nrOfApplications;
        [EntityProperty(ColumnName = "Applications")]
        public int? NrOfApplications
        {
            get { return nrOfApplications; }
            set { nrOfApplications = value; }
        }

        private string titleTrack;
        [EntityProperty(ColumnName = "Title_Track")]
        public string TitleTrack
        {
            get { return titleTrack; }
            set { titleTrack = value; }
        }

        private string companyName;
        [EntityProperty(IsPersistent=false)]
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        private string profileName;
        [EntityProperty(IsPersistent = false)]
        public string ProfileName
        {
            get { return profileName; }
            set { profileName = value; }
        }

        private string profileCode;
        [EntityProperty(IsPersistent = false)]
        public string ProfileCode
        {
            get { return profileCode; }
            set { profileCode = value; }
        }

        public Job() { }
        public Job(int id) { this.jobID = id; }
    }
}
