using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace Neos.Data
{
    public class ParamKnowledgeFamRepository : Repository<ParamKnowledgeFam>
    {
        public IList<ParamKnowledgeFam> GetAllKnowledgeFams()
        {
            IList<ParamKnowledgeFam> result = new List<ParamKnowledgeFam>();
            string sql = @"select t1.ConFamilleID, t1.Genre,
                                (select count(*) from tblParamConnaissance t2
                                    where t1.ConFamilleID = t2.ConFamilleID) as NumberIDUsed
                                from dbo.tblParamConnaisFam t1
                                order by t1.ConFamilleID ASC";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamKnowledgeFam item = new ParamKnowledgeFam();
                    item.ConFamilleID = reader["ConFamilleID"] as string;
                    item.Genre = reader["Genre"] as string;
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

        public ParamKnowledgeFam GetKnowledgeFamByID(string knowledgeFamID)
        {
            ParamKnowledgeFam result = null;
            string sql = @"select t1.ConFamilleID, t1.Genre,
                                (select count(*) from tblParamConnaissance t2
                                    where t1.ConFamilleID = t2.ConFamilleID) as NumberIDUsed
                                from dbo.tblParamConnaisFam t1
                                where t1.ConFamilleID = @ConFamilleID";
            SqlParameter param = new SqlParameter("@ConFamilleID", knowledgeFamID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql, param);
                if (reader.Read())
                {
                    result = new ParamKnowledgeFam();
                    result.ConFamilleID = reader["ConFamilleID"] as string;
                    result.Genre = reader["Genre"] as string;
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

        public IList<string> GetKnowledgeFamGenreList()
        {
            IList<string> result = new List<string>();
            string sql = "select distinct Genre from tblParamConnaisFam";            
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    String genre = reader["Genre"] as string;
                    result.Add(genre);
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

        public IList<ParamKnowledgeFam> GetParamKnowledgeFamByGenre(string genre)
        {
            Filter filter = Filter.Eq("Genre", genre);
            IList<ParamKnowledgeFam> list = this.FindAll(filter);
            return list;   
        }

        public void InserNewKnowledgeFam(ParamKnowledgeFam knowFam)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"insert tblParamConnaisFam (ConFamilleID, Genre)
                           values (@ConFamilleID, @Genre)";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@ConFamilleID", knowFam.ConFamilleID));
            paramList.Add(new SqlParameter("@Genre", knowFam.Genre));

            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
        }
    }
}
