using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace Neos.Data
{
    public class ParamNationaliteRepository : Repository<ParamNationalite>
    {
        public IList<ParamNationalite> GetAllNationalities()
        {
            IList<ParamNationalite> result = new List<ParamNationalite>();
            string sql = @"select t1.NationaliteID, t1.Libele,
                                (select count(*) from tblCandidat t2
                                    where t1.NationaliteID = t2.Nationalite) as NumberIDUsed
                                from dbo.tblParamNationalite t1
                                order by t1.NationaliteID ASC";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamNationalite item = new ParamNationalite();
                    item.NationaliteID = reader["NationaliteID"] as string;
                    item.Label = reader["Libele"] as string;
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

        public ParamNationalite GetNationalityByID(string nationalityID)
        {
            ParamNationalite result = null;
            string sql = @"select t1.NationaliteID, t1.Libele,
                                (select count(*) from tblCandidat t2
                                    where t1.NationaliteID = t2.Nationalite) as NumberIDUsed
                                from dbo.tblParamNationalite t1
                                where t1.NationaliteID = @NationaliteID";
            SqlParameter param = new SqlParameter("@NationaliteID", nationalityID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql, param);
                if (reader.Read())
                {
                    result = new ParamNationalite();
                    result.NationaliteID = reader["NationaliteID"] as string;
                    result.Label = reader["Libele"] as string;
                    result.NumberIDUsed = (int)reader["NumberIDUsed"];
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

        public void InserNewNationality(ParamNationalite nationality)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"insert tblParamNationalite (NationaliteID, Libele)
                           values (@NationaliteID, @Libele)";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@NationaliteID", nationality.NationaliteID));
            paramList.Add(new SqlParameter("@Libele", nationality.Label));

            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
        }
    }
}
