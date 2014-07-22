using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamStatutClient")]
    public class ParamClientStatus : IEntity
    {
        public ParamClientStatus() { }
        public ParamClientStatus(int ID)
        {
            this.statusID = ID;
            
        }

        private int statusID;
        [EntityProperty(ColumnName = "StatutID", IsPrimaryKey = true)]
        public int StatusID
        {
            get { return statusID; }
            set { statusID = value; }
        }

        private string status;
        [EntityProperty(ColumnName = "Statut")]
        public string Status
        {
            get { return status; }
            set { status = value; }
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
