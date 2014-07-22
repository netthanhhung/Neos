using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using Neos.Data.Enums;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblNotification")]
    public class UserMessage : IEntity
    {
        private int messageID;
        [EntityProperty(ColumnName = "MessageID", IsPrimaryKey = true, IsIdentity=true)]
        public int MessageID
        {
            get { return messageID; }
            set { messageID = value; }
        }

        private string userID;
        [EntityProperty(ColumnName = "UserID")]
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        private UserMessageType type;
        [EntityProperty(ColumnName = "Type")]
        public UserMessageType Type
        {
            get { return type; }
            set { type = value; }
        }

        private string subject;
        [EntityProperty(ColumnName = "Subject")]
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string messageContent;
        [EntityProperty(ColumnName = "Content")]
        public string MessageContent
        {
            get { return messageContent; }
            set { messageContent = value; }
        }

        private bool isUnread;
        [EntityProperty(ColumnName = "Unread")]
        public bool IsUnread
        {
            get { return isUnread; }
            set { isUnread = value; }
        }

        private DateTime? remindDate;
        [EntityProperty(ColumnName = "RemindDate")]
        public DateTime? RemindDate
        {
            get { return remindDate; }
            set { remindDate = value; }
        }

        private DateTime? createdDate;
        [EntityProperty(ColumnName = "CreatedDate")]
        public DateTime? CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        private string refID;
        [EntityProperty(ColumnName = "Ref")]
        public string RefID
        {
            get
            {
                return refID;
            }
            set
            {
                refID = value;
            }
        }
    

        public UserMessage() { }
        public UserMessage(int id)
        {
            this.messageID = id;
        }
    }
}
