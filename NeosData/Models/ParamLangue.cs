using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamLangue")]
    public class ParamLangue : IEntity
    {
        private string langueID;
        private string label;

        public ParamLangue() { }
        public ParamLangue(string langueID)
        {
            this.LangueID = langueID;            
        }
        public ParamLangue(string langueID, string label)
        {
            this.LangueID = langueID;
            this.Label = label;
        }

        
        [EntityProperty(ColumnName = "LangueID", IsPrimaryKey = true)]
        public string LangueID
        {
            get { return langueID; }
            set { langueID = value; }
        }
       

        [EntityProperty(ColumnName = "Libele")]
        public string Label
        {
            get { return label; }
            set { label = value; }
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
