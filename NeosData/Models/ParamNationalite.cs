using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamNationalite")]
    public class ParamNationalite : IEntity
    {
        private string nationaliteID;
        private string label;

        public ParamNationalite() { }
        public ParamNationalite(string nationaliteID)
        {
            this.NationaliteID = nationaliteID;            
        }
        public ParamNationalite(string nationaliteID, string label)
        {
            this.NationaliteID = nationaliteID;
            this.Label = label;
        }


        [EntityProperty(ColumnName = "NationaliteID", IsIdentity = true, IsPrimaryKey = true)]
        public string NationaliteID
        {
            get { return nationaliteID; }
            set { nationaliteID = value; }
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
