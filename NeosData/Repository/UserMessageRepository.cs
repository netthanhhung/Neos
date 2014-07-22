using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using Neos.Data.Enums;
using System.Data.SqlClient;

namespace Neos.Data
{
    public class UserMessageRepository : Repository<UserMessage>
    {
        private static string _connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
        public static Comparison<UserMessage> CreatedDateDescComparison = delegate(UserMessage m1, UserMessage m2)
        {
            if (!m1.CreatedDate.HasValue)
                return -1;
            if (!m2.CreatedDate.HasValue)
                return 1;
            return m2.CreatedDate.Value.CompareTo(m1.CreatedDate.Value);
        };

        public static Comparison<UserMessage> RemindDateDescComparison = delegate(UserMessage m1, UserMessage m2)
        {
            if (!m1.RemindDate.HasValue)
                return -1;
            if (!m2.RemindDate.HasValue)
                return 1;
            return m2.RemindDate.Value.CompareTo(m1.RemindDate.Value);
        };

        public List<UserMessage> GetMessagesOfUser(string userID)
        {
            Filter filter = Filter.Eq("UserID", userID);
            List<UserMessage> list = this.FindAll(filter) as List<UserMessage>;
            list.Sort(CreatedDateDescComparison);
            
            return list;
        }

        public int CountUnreadMessage(string userID)
        {
            string sql = "select count(*) from tblNotification where UserID='{0}' and Unread='true'";
            sql = string.Format(sql, userID);

            return (int)SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, sql);
        }

        public int CountUnreadJobRemindMessagesToday(string userID)
        {
            string sql = @"select count(*) from tblNotification where UserID='{0}' and Unread='true' and Type={1} 
                            and year(RemindDate) = year(GetDate()) and month(RemindDate) = month(GetDate()) and day(RemindDate) = day(GetDate())";
            sql = string.Format(sql, userID, (int)UserMessageType.JobResponsibility);

            return (int)SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, sql);
        }

        public List<UserMessage> GetAllJobRemindMessages(string userID)
        {
            Filter filter = Filter.Eq("Type", UserMessageType.JobResponsibility) & Filter.Eq("UserID", userID); 
            List<UserMessage> list = this.FindAll(filter) as List<UserMessage>;

            list.Sort(RemindDateDescComparison);

            return list;
        }
        public List<UserMessage> GetUnreadJobRemindMessagesToday(string userID)
        {
            Filter filter = Filter.Eq("Type", UserMessageType.JobResponsibility)
                            & Filter.Eq("UserID", userID) & Filter.Eq("Unread", true) & Filter.Eq("RemindDate", DateTime.Today);
            List<UserMessage> list = this.FindAll(filter) as List<UserMessage>;

            list.Sort(RemindDateDescComparison);

            return list;
        }

        public UserMessage FindMessagesByRef(string refId)
        {
            Filter filter = Filter.Eq("Ref", refId);
            List<UserMessage> list = this.FindAll(filter) as List<UserMessage>;

            return list[0];
        }
    }
}
