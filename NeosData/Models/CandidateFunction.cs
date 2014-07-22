using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblCandidatFonction")]
    [Serializable]
    public class CandidateFunction :IEntity
    {
        //CandidateFunctionID
        private int candidateFuntionID;
        [EntityProperty(ColumnName = "CandidatFonctionID", IsIdentity = true, IsPrimaryKey = true)]
        public int CandidateFunctionID
        {
            get { return candidateFuntionID; }
            set { candidateFuntionID = value; }
        }
        private int? candidateID;
        [EntityProperty(ColumnName = "CandidatID")]
        public int? CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
        }
        
        private int? functionID;
        [EntityProperty(ColumnName = "FonctionID")]
        public int? FunctionID
        {
            get { return functionID; }
            set { functionID = value; }
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

        private string code;
        [EntityProperty(IsPersistent = false)]
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        
    }
}
