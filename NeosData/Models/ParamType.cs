using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamType")]
    public class ParamType : IEntity
    {
        private string typeID;
        private string label;

        public ParamType() { }
        public ParamType(string typeID)
        {
            this.typeID = typeID;            
        }
        public ParamType(string typeID, string label)
        {
            this.typeID = typeID;
            this.label = label;
        }

        
        [EntityProperty(ColumnName = "TypeID", IsPrimaryKey = true)]
        public string TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }
       

        [EntityProperty(ColumnName = "Libele")]
        public string Label
        {
            get { return label; }
            set { label = value; }
        }
    }
}
