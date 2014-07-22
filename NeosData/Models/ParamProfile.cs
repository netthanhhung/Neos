using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblParamProfiles")]
    public class ParamProfile : IEntity
    {
        private int profileID;
        [EntityProperty(ColumnName = "ProfileId", IsIdentity=true, IsPrimaryKey = true)]
        public int ProfileID
        {
            get { return profileID; }
            set { profileID = value; }
        }

        private string profile;
        [EntityProperty(ColumnName = "Profile")]
        public string Profile
        {
            get { return profile; }
            set { profile = value; }
        }
        private string profileCode;
        [EntityProperty(ColumnName = "ProfileCode")]
        public string ProfileCode
        {
            get { return profileCode; }
            set { profileCode = value; }
        }

        private int numberProfileUsed;
        [EntityProperty(IsPersistent=false)]
        public int NumberProfileUsed
        {
            get { return numberProfileUsed; }
            set { numberProfileUsed = value; }
        }
        public ParamProfile() { }
        public ParamProfile(int id)
        {
            this.profileID = id;
        }
    }
}
