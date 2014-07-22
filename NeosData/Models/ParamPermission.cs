using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamPermissions")]
    public class ParamPermission : IEntity
    {
        private string permissionCode;
        [EntityProperty(ColumnName = "PermissionCode", IsPrimaryKey=true)]
        public string PermissionCode
        {
            get { return permissionCode; }
            set { permissionCode = value; }
        }

        private string permissionDescription;
        [EntityProperty(ColumnName = "PermissionDescription")]
        public string PermissionDescription
        {
            get { return permissionDescription; }
            set { permissionDescription = value; }
        }

        private int nbrUserUsed;
        [EntityProperty(IsPersistent=false)]
        public int NbrUserUsed
        {
            get { return nbrUserUsed; }
            set { nbrUserUsed = value; }
        }

        public ParamPermission() 
        {

        }

        public ParamPermission(string perCode)
        {
            permissionCode = perCode;
        }
    }
}
