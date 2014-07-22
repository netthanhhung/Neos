using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamContactFonction")]
    public class ParamContactFunction : IEntity
    {
        public ParamContactFunction()
        {
        }
        public ParamContactFunction(int id) {
            this.contactFunctionID = id;
        }
        private int contactFunctionID;
        [EntityProperty(ColumnName = "ContactFoncID", IsIdentity = true, IsPrimaryKey = true)]
        public int ContactFunctionID
        {
            get { return contactFunctionID; }
            set { contactFunctionID = value; }
        }

        private string functionName;
        [EntityProperty(ColumnName = "F")]
        public string FunctionName
        {
            get { return functionName; }
            set { functionName = value; }
        }
        
        private string logicalOrder;
        [EntityProperty(ColumnName = "OrdreLogique")]
        public string LogicalOrder
        {
            get { return logicalOrder; }
            set { logicalOrder = value; }
        }
    }
}
