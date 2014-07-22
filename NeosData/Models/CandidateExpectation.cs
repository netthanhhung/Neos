using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblCandidatAttentes")]
    public class CandidateExpectation : IEntity
    {
        #region private variables
        private int candidateID;
        [EntityProperty(ColumnName = "CandidatID", IsIdentity = true, IsPrimaryKey = true)]
        public int CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
        }
        private string region;
        [EntityProperty(ColumnName = "region")]
        public string Region
        {
            get { return region; }
            set { region = value; }
        }
        private string salaryLevel;
        [EntityProperty(ColumnName = "salaire")]
        public string SalaryLevel
        {
            get { return salaryLevel; }
            set { salaryLevel = value; }
        }
        private string company;
        [EntityProperty(ColumnName = "Societe")]
        public string Company
        {
            get { return company; }
            set { company = value; }
        }
        private string typeofContract;
        [EntityProperty(ColumnName = "typecontrat")]
        public string TypeofContract
        {
            get { return typeofContract; }
            set { typeofContract = value; }
        }
        private string function;
        [EntityProperty(ColumnName = "Fonction")]
        public string Function
        {
            get { return function; }
            set { function = value; }
        }
        private string motivation;
        [EntityProperty(ColumnName = "Motivation")]
        public string Motivation
        {
            get { return motivation; }
            set { motivation = value; }
        }
        #endregion

        #region methods
        public CandidateExpectation()
        {
        }
        public CandidateExpectation(int candidateId)
        {
            this.CandidateID = candidateId;
        }
        #endregion
        
    }
}
