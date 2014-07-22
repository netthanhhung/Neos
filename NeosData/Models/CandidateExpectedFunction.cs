using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblCandidatAttentesFonction")]
    public class CandidateExpectedFunction : IEntity
    {
        private int candidateExpectedFunctionID;
        [EntityProperty(ColumnName = "CandidatAttentesFonctionID", IsIdentity = true, IsPrimaryKey = true)]
        public int CandidateExpectedFunctionID
        {
            get { return candidateExpectedFunctionID; }
            set { candidateExpectedFunctionID = value; }
        }
        private int candidateID;
        [EntityProperty(ColumnName = "CandidatID")]
        public int CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
        }
        private int functionID;
        [EntityProperty(ColumnName = "FonctionID")]        
        public int FunctionID
        {
            get { return functionID; }
            set { functionID = value; }
        }

        public CandidateExpectedFunction()
        {
        }
        public CandidateExpectedFunction(int id)
        {
            this.candidateExpectedFunctionID = id;
        }
    }
}
