using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamFonction")]
    public class ParamFunction : IEntity
    {
        private int functionID;
        private string functionFamID;
        private string label;

        public ParamFunction() { }
        public ParamFunction(int functionID)
        {
            this.functionID = functionID;            
        }


        [EntityProperty(ColumnName = "FonctionID", IsPersistent = true, IsIdentity = true, IsPrimaryKey = true)]
        public int FunctionID
        {
            get { return functionID; }
            set { functionID = value; }
        }

        [EntityProperty(ColumnName = "FonctionFamID")]
        public string FunctionFamID
        {
            get { return functionFamID; }
            set { functionFamID = value; }
        }

        [EntityProperty(ColumnName = "Libelle")]
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
