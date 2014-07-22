using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblCandidatAttentesFonction")]
    public class CandidateExpectancy : IEntity
    {
        private int candidateExpectancyID;
        [EntityProperty(ColumnName = "CandidatAttentesFonctionID", IsIdentity = true, IsPrimaryKey = true)]
        public int CandidateExpectancyID
        {
            get { return candidateExpectancyID; }
            set { candidateExpectancyID = value; }
        }

        private int candidatID;
        [EntityProperty(ColumnName = "CandidatID")]
        public int CandidatID
        {
            get { return candidatID; }
            set { candidatID = value; }
        }

        private int? functionID;
        [EntityProperty(ColumnName = "FonctionID")]
        public int? FunctionID
        {
            get { return functionID; }
            set { functionID = value; }
        }

        private string functionFam;
        [EntityProperty(IsPersistent = false)]
        public string FunctionFam
        {
            get { return functionFam; }
            set { functionFam = value; }
        }

        private string group;
        [EntityProperty(IsPersistent = false)]
        public string Group
        {
            get { return group; }
            set { group = value; }
        }

        private string type;
        [EntityProperty(IsPersistent = false)]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public CandidateExpectancy() { }
        public CandidateExpectancy(int id)
        {
            this.candidateExpectancyID = id;
        }
    }
}
