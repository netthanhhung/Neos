using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamNiveauEtudeGenr")]
    public class ParamStudyHierarchy : IEntity
    {
        private int hiararchyID;
        private string label;

        public ParamStudyHierarchy() { }
        public ParamStudyHierarchy(int hiararchyID)
        {
            this.hiararchyID = hiararchyID;
        }


        [EntityProperty(ColumnName = "NiveauEtudeGenrID", IsPersistent = true, IsIdentity = true, IsPrimaryKey = true)]
        public int HiararchyID
        {
            get { return hiararchyID; }
            set { hiararchyID = value; }
        }

        [EntityProperty(ColumnName = "NiveauEtudeGenr")]
        public string Label
        {
            get { return label; }
            set { label = value; }
        }
    }
}
