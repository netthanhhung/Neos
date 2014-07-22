using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;

namespace Neos.Data
{
    public class ParamUserPermissionRepository : Repository<ParamUserPermission>
    {
        public IList<ParamUserPermission> GetPermissionsOfUser(string userID)
        {
            IList<ParamUserPermission> result = new List<ParamUserPermission>();
            string sql = @"select t1.UserID, t1.PermissionCode, t2.PermissionDescription
                            from dbo.tblParamUserPermissions t1
                            inner join dbo.tblParamPermissions t2 on t1.PermissionCode = t2.PermissionCode
                            where t1.UserID = '{0}' ";
            sql = string.Format(sql, userID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamUserPermission item = new ParamUserPermission();
                    item.UserID = reader["UserID"] as string;
                    item.PermissionCode = reader["PermissionCode"] as string;
                    item.PermissionDescription = reader["PermissionDescription"] as string;
                    result.Add(item);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return result;            
        }

        public IList<ParamUserPermission> GetUsersHavePermission(string permissionCode)
        {
            IList<ParamUserPermission> result = new List<ParamUserPermission>();
            string sql = @"select t1.UserID, t1.PermissionCode, t2.PermissionDescription
                            from dbo.tblParamUserPermissions t1
                            inner join dbo.tblParamPermissions t2 on t1.PermissionCode = t2.PermissionCode
                            where t1.PermissionCode = '{0}' ";
            sql = string.Format(sql, permissionCode);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamUserPermission item = new ParamUserPermission();
                    item.UserID = reader["UserID"] as string;
                    item.PermissionCode = reader["PermissionCode"] as string;
                    item.PermissionDescription = reader["PermissionDescription"] as string;
                    result.Add(item);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return result;
        }

        public void DeleteUserPermission(ParamUserPermission userPermission)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"delete from tblParamUserPermissions 
                            where UserID = @UserID and PermissionCode = @PermissionCode";
            SqlParameter userParam = new SqlParameter("@UserID", userPermission.UserID);
            SqlParameter perParam = new SqlParameter("@PermissionCode", userPermission.PermissionCode);
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, userParam, perParam);
        }
    }
}
