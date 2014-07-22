using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamFormJur")]
    public class ParamLegalForm : IEntity
    {
        private string formID;        

        public ParamLegalForm() { }
        public ParamLegalForm(string formID)
        {
            this.formID = formID;
        }


        [EntityProperty(ColumnName = "F", IsPrimaryKey = true)]
        public string FormID
        {
            get { return formID; }
            set { formID = value; }
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
