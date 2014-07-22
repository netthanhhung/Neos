using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamNiveauEtude")]
    public class ParamStudyLevel : IEntity
    {
        private int schoolID;
        private int? valueHierarchy;
        private string label;

        public ParamStudyLevel() { }
        public ParamStudyLevel(int schoolID)
        {
            this.schoolID = schoolID;            
        }


        [EntityProperty(ColumnName = "NiveauEtudeID", IsPersistent = true, IsIdentity = true, IsPrimaryKey = true)]
        public int SchoolID
        {
            get { return schoolID; }
            set { schoolID = value; }
        }

        [EntityProperty(ColumnName = "HierarchieValeur")]
        public int? ValueHierarchy
        {
            get { return valueHierarchy; }
            set { valueHierarchy = value; }
        }

        private string valueHierarchyString;
        [EntityProperty(IsPersistent=false)]
        public string ValueHierarchyString
        {
            get { return valueHierarchyString; }
            set { valueHierarchyString = value; }
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
