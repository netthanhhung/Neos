using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamLocations")]
    public class ParamLocations : IEntity
    {                
        public ParamLocations() { }
        public ParamLocations(string location)
        {
            this.Location = location;            
        }

        private string location;
        [EntityProperty(ColumnName = "Location", IsIdentity = true, IsPrimaryKey = true)]
        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        private int? hierarchie;
        [EntityProperty(ColumnName = "Hierarchie")]
        public int? Hierarchie
        {
            get { return hierarchie; }
            set 
            {
                hierarchie = value;
            }
        }

        private string locationUk;
        [EntityProperty(ColumnName = "LocationUk")]
        public string LocationUk
        {
            get { return locationUk; }
            set { locationUk = value; }
        }

        private string locationNL;
        [EntityProperty(ColumnName = "LocationNL")]
        public string LocationNL
        {
            get { return locationNL; }
            set { locationNL = value; }
        }

        private string codeLocation;
        [EntityProperty(ColumnName = "CodeLocation")]
        public string CodeLocation
        {
            get { return codeLocation; }
            set { codeLocation = value; }
        }

        private int numberCodeUsed;
        [EntityProperty(IsPersistent = false)]
        public int NumberCodeUsed
        {
            get { return numberCodeUsed; }
            set
            {
                numberCodeUsed = value;
            }
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
