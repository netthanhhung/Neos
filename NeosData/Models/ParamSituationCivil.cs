using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamSituationCivile")]
    public class ParamSituationCivil : IEntity
    {
        private string code;
        private string codeType;
        private string label;

        public ParamSituationCivil() { }
        public ParamSituationCivil(string code)
        {
            this.code = code;
        }


        [EntityProperty(ColumnName = "Code", IsPrimaryKey = true)]
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        [EntityProperty(ColumnName = "CodeType")]
        public string CodeType
        {
            get { return codeType; }
            set { codeType = value; }
        }

        [EntityProperty(ColumnName = "libelle")]
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
