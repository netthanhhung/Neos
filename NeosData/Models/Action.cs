using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblAction")]
    public class Action : IEntity
    {
        private int actionID;
        private int? companyID;
        private string companyName;
        private int? contactID;
        private string contactFullName;
        private int? jobID;
        private int? candidateID;
        private string candidateFullName;
        private DateTime? dateAction;
        private DateTime? hour;
        private string lieuRDV;
        private int? typeAction;
        private string typeActionLabel;
        private string descrAction;
        private string resultCompany;
        private string resultCandidate;
        private bool actif;
        private string responsable;
        private string responsableName;
        private DateTime? dateCreation;

        #region properties
        [EntityProperty(ColumnName = "ActionID", IsIdentity = true, IsPrimaryKey = true)]
        public int ActionID
        {
            get { return actionID; }
            set { actionID = value; }
        }
        [EntityProperty(ColumnName = "SocieteID")]
        public int? CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }
        [EntityProperty(IsPersistent = false)]
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }
        [EntityProperty(ColumnName = "ContactID")]
        public int? ContactID
        {
            get { return contactID; }
            set { contactID = value; }
        }
        [EntityProperty(IsPersistent = false)]
        public string ContactFullName
        {
            get { return contactFullName; }
            set { contactFullName = value; }
        }
        [EntityProperty(ColumnName = "JobID")]
        public int? JobID
        {
            get { return jobID; }
            set { jobID = value; }
        }
        [EntityProperty(IsPersistent = false)]
        public string CandidateFullName
        {
            get { return candidateFullName; }
            set { candidateFullName = value; }
        }
        [EntityProperty(ColumnName = "CandidatID")]
        public int? CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
        }
        [EntityProperty(ColumnName = "DateAction")]
        public DateTime? DateAction
        {
            get { return dateAction; }
            set { dateAction = value; }
        }
        [EntityProperty(ColumnName = "Heure")]
        public DateTime? Hour
        {
            get { return hour; }
            set { hour = value; }
        }
        [EntityProperty(ColumnName = "LieuRDV")]
        public string LieuRDV
        {
            get { return lieuRDV; }
            set { lieuRDV = value; }
        }
        [EntityProperty(ColumnName = "TypeAction")]
        public int? TypeAction
        {
            get { return typeAction; }
            set { typeAction = value; }
        }
        [EntityProperty(IsPersistent = false)]
        public string TypeActionLabel
        {
            get { return typeActionLabel; }
            set { typeActionLabel = value; }
        }
        
        [EntityProperty(ColumnName = "DescrAction")]
        public string DescrAction
        {
            get { return descrAction; }
            set { descrAction = value; }
        }
        [EntityProperty(ColumnName = "ResultSociete")]
        public string ResultCompany
        {
            get { return resultCompany; }
            set { resultCompany = value; }
        }
        [EntityProperty(ColumnName = "ResultCandidat")]
        public string ResultCandidate
        {
            get { return resultCandidate; }
            set { resultCandidate = value; }
        }
        [EntityProperty(ColumnName = "Actif")]
        public bool Actif
        {
            get { return actif; }
            set { actif = value; }
        }
        [EntityProperty(ColumnName = "Responsable")]
        public string Responsable
        {
            get { return responsable; }
            set { responsable = value; }
        }
        [EntityProperty(IsPersistent = false)]
        public string ResponsableName
        {
            get { return responsableName; }
            set { responsableName = value; }
        }
        [EntityProperty(ColumnName = "DateCreation")]
        public DateTime? DateCreation
        {
            get { return dateCreation; }
            set { dateCreation = value; }
        }
        
        #endregion

        #region methods
        public Action()
        {
        }
        public Action(int actionID)
        {
            this.ActionID = actionID;
        }
        #endregion
    }
}
