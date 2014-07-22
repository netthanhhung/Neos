using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;

namespace Neos.Data
{
    public class ParamUserRepository : Repository<ParamUser>
    {
        public static Comparison<ParamUser> LastNameAscComparison = delegate(ParamUser u1, ParamUser u2)
        {
            return u1.LastName.CompareTo(u2.LastName);
        };

        public List<ParamUser> GetAllUser(bool insertEmtpy)
        {            
            List<ParamUser> result = new List<ParamUser>();
            result = this.FindAll() as List<ParamUser>;
            result.Sort(LastNameAscComparison);
            if (insertEmtpy)
            {
                ParamUser emptyItem = new ParamUser();
                emptyItem.UserID = string.Empty;
                emptyItem.LastName = string.Empty;
                result.Insert(0, emptyItem);
            }
            return result;
        }

        public List<ParamUser> GetActiveUser(bool insertEmpty)
        {
            Filter filter = Filter.Eq("actif", true);
            List<ParamUser> result = new List<ParamUser>();
            result = this.FindAll(filter) as List<ParamUser>;
            result.Sort(LastNameAscComparison);
            if (insertEmpty)
            {
                ParamUser emptyItem = new ParamUser();
                emptyItem.UserID = string.Empty;
                emptyItem.LastName = string.Empty;
                result.Insert(0, emptyItem);
            }
            return result;
        }

        public ParamUser GetUserById(string userID)
        {
            Filter filter = Filter.Eq("UserID", userID) & Filter.Eq("actif", true);
            return this.FindOne(filter);
        }

        public void InserNewUser(ParamUser newUser)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"insert tblParamUtilisateurs (UserID, Nom, Sexe, mail, Cmtel, actif, Password)
                           values (@UserID, @Nom, @Sexe, @mail, @Cmtel, @actif, @Password)";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@UserID", newUser.UserID));
            paramList.Add(new SqlParameter("@Nom", newUser.LastName));
            paramList.Add(new SqlParameter("@Sexe", newUser.Gender));
            paramList.Add(new SqlParameter("@mail", newUser.Email));
            paramList.Add(new SqlParameter("@Cmtel", newUser.Telephone));
            paramList.Add(new SqlParameter("@actif", newUser.Active));
            paramList.Add(new SqlParameter("@Password", newUser.Password));
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());   
        }

        public int CountNumberBeingUsedOfUser(string user)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"select (A1.Num + A2.Num) as Num
                            from (select count(*) As Num from tblAction where Responsable = @User) A1, 
	                            (select count(*) as Num from tblCandidat where Interviewer = @User) A2,
	                            (select count(*) as Num from tblSociete where Responsable = @User) A3,
	                            (select count(*) as Num from tblJobs where CM = @User) A4";
            SqlParameter userParam = new SqlParameter("@User", user);
            return (int)SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sql, userParam);
        }
    }
}
