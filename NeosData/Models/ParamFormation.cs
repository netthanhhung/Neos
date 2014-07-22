using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamFormation")]
    public class ParamFormation : IEntity
    {
        private int formationID;
        private string label;

        public ParamFormation() { }
        public ParamFormation(int formationID)
        {
            this.formationID = formationID;            
        }
        public ParamFormation(int formationID, string label)
        {
            this.formationID = formationID;
            this.label = label;
        }


        [EntityProperty(ColumnName = "FormationID", IsPersistent = true, IsIdentity = true, IsPrimaryKey = true)]
        public int FormationID
        {
            get { return formationID; }
            set { formationID = value; }
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
