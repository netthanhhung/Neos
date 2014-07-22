using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;

namespace Neos.Data
{
    public class CandidateRepository : Repository<Candidate>
    {
        private string _connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Candidate> SearchCandidatesOnName(string name)
        {
            string sql = @"select CandidatID, Nom, Prenom from tblCandidat ";

            bool hasWhere = false;
            List<SqlParameter> paramList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(name))
            {
                sql += " where (Nom + Prenom like  @SearchString) or (Prenom + Nom like  @SearchString) ";
                hasWhere = true;
                paramList.Add(new SqlParameter("@SearchString", name.Replace(" ", "%") + "%"));
            }

            List<Candidate> result = new List<Candidate>();
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sql, paramList.ToArray());
                result = GetCandidateFromReader(reader) as List<Candidate>;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">first name and last name are seperated by a space(ex: nga mai)</param>
        /// <returns></returns>
        public int CountSearchCandidatesOnName(string name)
        {
            string sql = @"select count(*) from tblCandidat ";
            
            bool hasWhere = false;
            List<SqlParameter> paramList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(name))
            {
                sql += " where (nom+prenom like  @SearchString) or (prenom+nom like  @SearchString) ";
                hasWhere = true;
                paramList.Add(new SqlParameter("@SearchString", name.Replace(" ", "%") + "%"));
            }
            
            int result = (int)SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, sql, paramList.ToArray());
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="sortOrder"></param>
        /// <param name="sortOrderInvert"></param>
        /// <returns></returns>
        public IList<Candidate> SearchCandidatesOnName(string name, int pageSize, int pageNumber, string sortOrder, string sortOrderInvert)
        {
            IList<Candidate> result = new List<Candidate>();
            string sql = @"select {0} CandidatID, Unit, Nom, Prenom, Adresse, CVMail, CodePostal, inactif, DateModif
                           from tblCandidat ";
            bool hasWhere = false;
            List<SqlParameter> paramList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(name))
            {
                sql += " where (nom+prenom like  @SearchString) or (prenom+nom like  @SearchString) ";
                hasWhere = true;
                paramList.Add(new SqlParameter("@SearchString", name.Replace(" ", "%") + "%"));
            }
            
            sql += " order by " + sortOrder;
            sql = string.Format(sql, "top " + pageSize * pageNumber + " ");
            sql = "(select top " + pageSize + " * from \n (" + sql + " ) As T1 \n"
                    + " Order by " + sortOrderInvert;
            sql = "select * from \n " + sql + " ) As T2 \n"
                    + " Order by " + sortOrder;

            //sql = string.Format(sql, lastName);
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sql, paramList.ToArray());
                result = GetCandidateFromReader(reader);
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


        private IList<Candidate> GetCandidateFromReader(SqlDataReader reader)
        {
            List<Candidate> result = new List<Candidate>();
            while (reader.Read())
            {
                Candidate item = new Candidate();
                item.CandidateId = (int)reader["CandidatID"];
                item.LastName = reader["Nom"] as string;
                item.FirstName = reader["Prenom"] as string;
                item.FullName = item.FirstName + " " + item.LastName;
                try
                {
                    item.Unit = reader["Unit"] as string;
                    item.Address = reader["Adresse"] as string;
                    item.ZipCode = reader["CVMail"] as string;

                    if (!(reader["inactif"] is DBNull))
                    {
                        item.Inactive = (bool?)reader["inactif"];
                    }
                    if (item.Inactive.HasValue)
                    {
                        if (item.Inactive.Value)
                            item.InactiveString = "inactif";
                        else
                            item.InactiveString = "actif";
                    }
                    if (!(reader["DateModif"] is DBNull))
                    {
                        item.LastModifDate = (DateTime)reader["DateModif"];
                    }
                }
                catch { }                

                
                result.Add(item);
            }
            return result;
        }
    }
}
