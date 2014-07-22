using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamConnaissance")]
    public class ParamKnowledge : IEntity
    {
        private int knowledgeID;
        private string code;
        private string knowledgeFamID;
        private string definition;

        public ParamKnowledge() { }
        public ParamKnowledge(int knowledgeID)
        {
            this.knowledgeID = knowledgeID;            
        }


        [EntityProperty(ColumnName = "ConnaissanceID", IsIdentity = true, IsPrimaryKey = true)]
        public int KnowledgeID
        {
            get { return knowledgeID; }
            set { knowledgeID = value; }
        }

        [EntityProperty(ColumnName = "Code")]
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        [EntityProperty(ColumnName = "ConFamilleID")]
        public string KnowledgeFamID
        {
            get { return knowledgeFamID; }
            set { knowledgeFamID = value; }
        }

        [EntityProperty(ColumnName = "Definition")]
        public string Definition
        {
            get { return definition; }
            set { definition = value; }
        }

        private int numberIDUsed;
        [EntityProperty(IsPersistent = false)]
        public int NumberIDUsed
        {
            get { return numberIDUsed; }
            set
            {
                numberIDUsed = value;
            }
        }
    }
}
