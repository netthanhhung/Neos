using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamUtilisateurs")]
    public class ParamUser : IEntity
    {
        private string userID;
        [EntityProperty(ColumnName = "UserID", IsIdentity = true, IsPrimaryKey = true)]
        public string UserID 
        {
            get { return userID; }
            set { userID = value; }
        }
        private string lastName;
        [EntityProperty(ColumnName = "Nom")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        private string gender;
        [EntityProperty(ColumnName = "Sexe")]
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        private string email;
        [EntityProperty(ColumnName = "mail")]        
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private string telephone;
        [EntityProperty(ColumnName = "Cmtel")]
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }
        private bool active;
        [EntityProperty(ColumnName = "actif")]
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        private string password;
        [EntityProperty(ColumnName = "Password")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private IList<ParamUserPermission> permissions;
        [EntityProperty(IsPersistent=false)]
        public IList<ParamUserPermission> Permissions
        {
            get { return permissions; }
            set { permissions = value; }
        }

        public ParamUser() 
        { 
        }
        public ParamUser(string uID)
        {
            userID = uID;
        }

    }
}
