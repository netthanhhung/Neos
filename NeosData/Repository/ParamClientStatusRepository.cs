using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace Neos.Data
{
    public class ParamClientStatusRepository : Repository<ParamClientStatus>
    {
        public IList<ParamClientStatus> GetAllClientStatuses()
        {
            IList<ParamClientStatus> result = new List<ParamClientStatus>();
            string sql = @"select t1.StatutID, t1.Statut,
                                (select count(*) from tblSociete t2
                                    where t1.StatutID = t2.Statut) as NumberIDUsed
                                from dbo.tblParamStatutClient t1
                                order by t1.StatutID ASC";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamClientStatus item = new ParamClientStatus();
                    item.StatusID = (int)reader["StatutID"];
                    item.Status = reader["Statut"] as string;
                    item.NumberIDUsed = (int)reader["NumberIDUsed"];
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

        public IList<ParamClientStatus> GetAllClientStatuses(string status)
        {
            IList<ParamClientStatus> result = new List<ParamClientStatus>();
            string sql = @"select t1.StatutID, t1.Statut,
                                (select count(*) from tblSociete t2
                                    where t1.StatutID = t2.Statut) as NumberIDUsed
                                from dbo.tblParamStatutClient t1
                                where t1.Statut = @Statut";
            SqlParameter param = new SqlParameter("@Statut", status);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql, param);
                while (reader.Read())
                {
                    ParamClientStatus item = new ParamClientStatus();
                    item.StatusID = (int)reader["StatutID"];
                    item.Status = reader["Statut"] as string;
                    item.NumberIDUsed = (int)reader["NumberIDUsed"];
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
