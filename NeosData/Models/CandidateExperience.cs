using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblCandidatExperience")]
    public class CandidateExperience :IEntity
    {
        //CandidatExperienceID
        private int experienceID;
        [EntityProperty(ColumnName = "CandidatExperienceID",  IsIdentity = true, IsPrimaryKey = true)]
        public int ExperienceID
        {
            get { return experienceID; }
            set { experienceID = value; }
        }
        private int candidateID;
        [EntityProperty(ColumnName = "CandidatID")]
        public int CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
        }
        private string period;
        [EntityProperty(ColumnName = "Periode")]
        public string Period
        {
            get { return period; }
            set { period = value; }
        }
        private string company;
        [EntityProperty(ColumnName = "Societe")]
        public string Company
        {
            get { return company; }
            set { company = value; }
        }
        private string functionDesc;
        [EntityProperty(ColumnName = "DescrFonction")]
        public string FunctionDesc
        {
            get { return functionDesc; }
            set { functionDesc = value; }
        }
        private string salary;
        [EntityProperty(ColumnName = "Salaire")]
        public string Salary
        {
            get { return salary; }
            set { salary = value; }
        }
        private string extraAdvantage;
        [EntityProperty(ColumnName = "SalairePackage")]
        public string ExtraAdvantage
        {
            get { return extraAdvantage; }
            set { extraAdvantage = value; }
        }
        //RaisonDepart
        private string leftReason;
        [EntityProperty(ColumnName = "RaisonDepart")]
        public string LeftReason
        {
            get { return leftReason; }
            set { leftReason = value; }
        }
        private int? functionID;
        [EntityProperty(ColumnName = "FonctionID")]
        public int? FunctionID
        {
            get { return functionID; }
            set { functionID = value; }
        }
        private string functionString;
        [EntityProperty(IsPersistent = false)]
        public string FunctionString
        {
            get { return functionString; }
            set { functionString = value; }
        }
        
        
        #region methods
        public CandidateExperience()
        {
        }
        public CandidateExperience(int id)
        {
            this.ExperienceID = id;
        }
        #endregion
    }
}
