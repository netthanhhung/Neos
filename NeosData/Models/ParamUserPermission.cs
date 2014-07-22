using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamUserPermissions")]
    public class ParamUserPermission : IEntity
    {
        private string userID;
        [EntityProperty(ColumnName = "UserID")]
        public string UserID 
        {
            get { return userID; }
            set { userID = value; }
        }

        private string permissionCode;
        [EntityProperty(ColumnName = "PermissionCode")]
        public string PermissionCode
        {
            get { return permissionCode; }
            set { permissionCode = value; }
        }

        private string permissionDescription;
        [EntityProperty(IsPersistent=false)]
        public string PermissionDescription
        {
            get { return permissionDescription; }
            set { permissionDescription = value; }
        }

        public ParamUserPermission() 
        {

        }
    }
}
