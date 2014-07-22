using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamTypeAction")]
    public class ParamTypeAction : IEntity
    {
        private int paramActionID;
        private string label;
        private string unitCode;

        public ParamTypeAction() { }
        public ParamTypeAction(int id)
        {
            this.ParamActionID = id;            
        }
        public ParamTypeAction(int id, string lbl)
        {
            this.ParamActionID = id;
            this.Label = lbl;
        }


        [EntityProperty(ColumnName = "ParamActionID", IsIdentity = true, IsPrimaryKey = true)]
        public int ParamActionID
        {
            get { return paramActionID; }
            set { paramActionID = value; }
        }


        [EntityProperty(ColumnName = "libelle")]
        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        [EntityProperty(ColumnName = "UnitCode")]
        public string UnitCode
        {
            get { return unitCode; }
            set { unitCode = value; }
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
