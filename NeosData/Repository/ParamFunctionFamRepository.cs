using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace Neos.Data
{
    public class ParamFunctionFamRepository : Repository<ParamFunctionFam>
    {
        public IList<ParamFunctionFam> GetAllFunctionFams()
        {
            IList<ParamFunctionFam> result = new List<ParamFunctionFam>();
            string sql = @"select t1.FonctionFamID, t1.Genre,
                                (select count(*) from tblParamFonction t2
                                    where t1.FonctionFamID = t2.FonctionFamID) as NumberIDUsed
                                from dbo.tblParamFonctionFam t1
                                order by t1.FonctionFamID ASC";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamFunctionFam item = new ParamFunctionFam();
                    item.FonctionFamID = reader["FonctionFamID"] as string;
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

        public ParamFunctionFam GetFunctionFamsByID(string funcFamID)
        {
            ParamFunctionFam result = null;
            string sql = @"select t1.FonctionFamID, t1.Genre,
                                (select count(*) from tblParamFonction t2
                                    where t1.FonctionFamID = t2.FonctionFamID) as NumberIDUsed
                                from dbo.tblParamFonctionFam t1
                                where t1.FonctionFamID = @FonctionFamID";
            SqlParameter param = new SqlParameter("@FonctionFamID", funcFamID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql, param);
                if (reader.Read())
                {
                    result = new ParamFunctionFam();
                    result.FonctionFamID = reader["FonctionFamID"] as string;
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


        public IList<string> GetFunctionFamGenreList()
        {
            IList<string> result = new List<string>();
            string sql = "select distinct Genre from tblParamFonctionFam";
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

        public IList<ParamFunctionFam> GetParamFunctionFamByGenre(string genre)
        {
            Filter filter = Filter.Eq("Genre", genre);
            IList<ParamFunctionFam> list = this.FindAll(filter);
            return list;
        }

        public void InserNewFunctionFam(ParamFunctionFam funcFam)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"insert tblParamFonctionFam (FonctionFamID, Genre)
                           values (@FonctionFamID, @Genre)";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@FonctionFamID", funcFam.FonctionFamID));
            paramList.Add(new SqlParameter("@Genre", funcFam.Genre));

            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
        }
    }
}
