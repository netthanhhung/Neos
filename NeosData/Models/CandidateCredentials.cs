using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblCandidatMotdePasse")]
    public class CandidateCredentials : IEntity
    {
        private int pwdID;
        [EntityProperty(ColumnName = "CandidatPwdID", IsIdentity = true, IsPrimaryKey = true)]
        public int PwdID
        {
            get { return pwdID; }
            set { pwdID = value; }
        }
        private int candidateID;
        [EntityProperty(ColumnName = "CandidatID")]
        public int CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
        }
        private string password;
        [EntityProperty(ColumnName = "motdepasse")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        private string email;
        [EntityProperty(ColumnName = "email")]        
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public CandidateCredentials()
        {

        }
        public CandidateCredentials(int id)
        {
            this.pwdID = id;
        }
    }
}
