using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace Neos.Data
{
    public class ParamLangueRepository : Repository<ParamLangue>
    {
        public IList<ParamLangue> GetAllLanguages()
        {
            IList<ParamLangue> result = new List<ParamLangue>();
            string sql = @"select t1.LangueID, t1.Libele,
                                (select count(*) from tblCandidatEval t2
                                    where t1.LangueID = t2.AutreLangue) as NumberIDUsed
                                from dbo.tblParamLangue t1
                                order by t1.LangueID ASC";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamLangue item = new ParamLangue();
                    item.LangueID = reader["LangueID"] as string;
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

        public ParamLangue GetLanguageByID(string languageID)
        {
            ParamLangue result = null;
            string sql = @"select t1.LangueID, t1.Libele,
                                (select count(*) from tblCandidatEval t2
                                    where t1.LangueID = t2.AutreLangue) as NumberIDUsed
                                from dbo.tblParamLangue t1
                                where t1.LangueID = @LangueID";
            SqlParameter param = new SqlParameter("@LangueID", languageID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql, param);
                if (reader.Read())
                {
                    result = new ParamLangue();
                    result.LangueID = reader["LangueID"] as string;
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

        public void InserNewLanguage(ParamLangue language)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"insert tblParamLangue (LangueID, Libele)
                           values (@LangueID, @Libele)";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@LangueID", language.LangueID));
            paramList.Add(new SqlParameter("@Libele", language.Label));

            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
        }

    }
}
