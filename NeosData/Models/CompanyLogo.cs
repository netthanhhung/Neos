using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblSocietelogo")]
    public class CompanyLogo : IEntity
    {
        private int companyID;
        [EntityProperty(ColumnName = "societeID", IsPrimaryKey=true)]
        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }
        private string logoPath;
        [EntityProperty(ColumnName = "logopath")]
        public string LogoPath
        {
            get { return logoPath; }
            set { logoPath = value; }
        }

        public CompanyLogo()
        {
           
        }
        public CompanyLogo(int compID)
        {
            this.companyID = compID;
        }
    }
}
