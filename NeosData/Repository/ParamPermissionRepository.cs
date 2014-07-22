using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace Neos.Data
{
    public class ParamPermissionRepository : Repository<ParamPermission>
    {
        public IList<ParamPermission> GetAllPermission()
        {
            IList<ParamPermission> result = new List<ParamPermission>();
            string sql = @"select t1.PermissionCode, t1.PermissionDescription, 
                            (select count(*) from tblParamUserPermissions t2 
                                where t1.PermissionCode = t2.PermissionCode) as NumberUserUsed
                            from dbo.tblParamPermissions t1";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamPermission item = new ParamPermission();
                    item.PermissionCode = reader["PermissionCode"] as string;
                    item.PermissionDescription = reader["PermissionDescription"] as string;
                    item.NbrUserUsed = (int) reader["NumberUserUsed"];
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
    }
}
