using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblCandidatConaiss")]
    [Serializable]
    public class CandidateKnowledge : IEntity
    {
        private int candidateKnowledgeID;
        [EntityProperty(ColumnName = "CandidatConnaissID", IsIdentity = true, IsPrimaryKey = true)]
        public int CandidateKnowledgeID
        {
            get { return candidateKnowledgeID; }
            set { candidateKnowledgeID = value; }
        }
        private int? candidateID;
        [EntityProperty(ColumnName = "CandidatID")]
        public int? CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
        }
        private int? knowledgeID;
        [EntityProperty(ColumnName = "ConnaissanceID")]        
        public int? KnowledgeID
        {
            get { return knowledgeID; }
            set { knowledgeID = value; }
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

        public CandidateKnowledge()
        {
        }
        public CandidateKnowledge(int id)
        {
            this.candidateKnowledgeID = id;
        }
    }
}
