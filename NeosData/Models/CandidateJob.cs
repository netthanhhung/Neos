using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblCandidatJob")]
    public class CandidateJob : IEntity
    {
        private int jobID;
        [EntityProperty(ColumnName = "CandidatJobID", IsIdentity = true, IsPrimaryKey = true)]
        public int JobID
        {
            get { return jobID; }
            set { jobID = value; }
        }
        private int candidateID;
        [EntityProperty(ColumnName = "CandidatID")]
        public int CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
        }
        private int jobRef;
        [EntityProperty(ColumnName = "JobRef")]
        public int JobRef
        {
            get { return jobRef; }
            set { jobRef = value; }
        }
    }
}
