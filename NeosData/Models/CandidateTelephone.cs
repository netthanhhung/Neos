using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblCandidatTelephone")]
    public class CandidateTelephone : IEntity
    {
        #region private variables
        private int telePhoneId;
        [EntityProperty(ColumnName = "CandidatTelID", IsIdentity = true, IsPrimaryKey = true)]
        public int TelePhoneId
        {
            get { return telePhoneId; }
            set { telePhoneId = value; }
        }
        private int candidateID;
        [EntityProperty(ColumnName = "CandidatID")]
        public int CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
        }
        private string type;
        /// <summary>
        /// type of contact info: T:phone number, F: fax number, G: mobile number, E: email
        /// </summary>
        [EntityProperty(ColumnName = "Type")]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        private string typeLabel;
        [EntityProperty(IsPersistent = false)]
        public string TypeLabel
        {
            get { return typeLabel; }
            set { typeLabel = value; }
        }
        private string phoneArea;
        [EntityProperty(ColumnName = "ZoneTel")]
        public string PhoneArea
        {
            get { return phoneArea; }
            set { phoneArea = value; }
        }
        private string phoneMail;
        /// <summary>
        /// Phone number or Email
        /// </summary>
        [EntityProperty(ColumnName = "TelMail")]        
        public string PhoneMail
        {
            get { return phoneMail; }
            set { phoneMail = value; }
        }
        private string location;
        [EntityProperty(ColumnName = "Lieu")]
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        #endregion 

        #region methods
        public CandidateTelephone()
        {
        }
        public CandidateTelephone(int id)
        {
            this.telePhoneId = id;
        }
        #endregion
    }
}
