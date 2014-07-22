using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamPays")]
    public class ParamPays : IEntity
    {
        private string paysID;
        private string pays;

        public ParamPays() { }
        public ParamPays(string paysID, string pays)
        {
            this.paysID = paysID;
            this.pays = pays;
        }


        [EntityProperty(ColumnName = "PaysID", IsIdentity = true, IsPrimaryKey = true)]
        public string PaysID
        {
            get { return paysID; }
            set { paysID = value; }
        }
       

        [EntityProperty(ColumnName = "Pays")]
        public string Pays
        {
            get { return pays; }
            set { pays = value; }
        }
    }
}
