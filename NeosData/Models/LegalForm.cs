using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamFormJur")]
    public class LegalForm : IEntity
    {
        private string f;
        [EntityProperty(ColumnName = "F", IsIdentity = true, IsPrimaryKey = true)]
        public string F
        {
            get { return f; }
            set { f = value; }
        }
    }
}
