using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;

namespace Neos.Data
{
    public class ActionRepository : Repository<Action>
    {
        private static string SELECT_ACTION_QUERY = @"select {0} ActionID, t1.SocieteID, t2.SocNom, JobID, 
                            t1.ContactID, t6.Nom as ConLastName, t6.Prenom as ConFirstName,
                            t1.CandidatID, t5.Nom as CanLastName, t5.Prenom as CanFirstName,
                            DateAction, Heure, LieuRDV, t1.TypeAction, t4.libelle as TypeActionLabel, 
                            DescrAction, ResultSociete, ResultCandidat, 
                            t1.Actif, t1.Responsable, t3.Nom as ResponsableName, t1.DateCreation
                          from tblAction t1
						  left outer join tblSociete t2 on t1.SocieteID = t2.SocieteID
						  left outer join tblParamUtilisateurs t3 on t1.Responsable = t3.UserID
                          left outer join tblParamTypeAction t4 on t1.TypeAction = t4.ParamActionID
                          left outer join tblCandidat t5 on t1.CandidatID = t5.CandidatID
                          left outer join tblSocieteContact t6 on t1.ContactID = t6.ContactID
                          ";
        /// <summary>
        /// Get the list of actions of the candidate.
        /// </summary>
        /// <param name="candidateID"></param>
        /// <returns></returns>
        public IList<Action> GetActionOfCandidate(int candidateID)
        {
            IList<Action> result = new List<Action>();
            string sql = SELECT_ACTION_QUERY; 
            sql += " where t1.CandidatID = {1}";
            sql = string.Format(sql, string.Empty, candidateID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                result = GetActionsFromReader(reader);
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
        /// Get the list of actions of the company.
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public IList<Action> GetActionOfCompany(int companyID)
        {
            IList<Action> result = new List<Action>();
            string sql = SELECT_ACTION_QUERY;                          
            sql += " where t1.SocieteID = {1}";
            sql = string.Format(sql, string.Empty, companyID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                result = GetActionsFromReader(reader);
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
        /// Get ation by its ID.
        /// </summary>
        /// <param name="actionID"></param>
        /// <returns></returns>
        public Action GetActionByActionID(int actionID)
        {
            IList<Action> result = new List<Action>();
            string sql = SELECT_ACTION_QUERY;  
            sql += " where ActionID = {1}";
            sql = string.Format(sql, string.Empty, actionID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                result = GetActionsFromReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            foreach (Action item in result)
            {
                return item;
            }
            return null;
        }

        /// <summary>
        /// Search the actions base on the search criteria, Just get the number of actions base on pageSize.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="sortOrder"></param>
        /// <param name="sortOrderInvert"></param>
        /// <returns></returns>
        public IList<Action> SearchActions(ActionSearchCriteria criteria, int pageSize,
            int pageNumber, string sortOrder, string sortOrderInvert)
        {
            IList<Action> result = new List<Action>();
            string sqlQuery = SELECT_ACTION_QUERY;
            List<SqlParameter> paramList;
            //condition here :
            string whereCon = BuildWhereCondition(criteria, out paramList);
            sqlQuery += whereCon;
            string sortOrder1 = sortOrder;
            if (sortOrder1.Contains("Actif"))
                sortOrder1 = sortOrder1.Replace("Actif", "t1.Actif");


            sqlQuery += " order by " + sortOrder1;
            sqlQuery = string.Format(sqlQuery, "top " + pageSize * pageNumber + " ");
            sqlQuery = "(select top " + pageSize + " * from \n (" + sqlQuery + " ) As T1 \n"
                    + " Order by " + sortOrderInvert;
            sqlQuery = "select * from \n " + sqlQuery + " ) As T2 \n"
                    + " Order by " + sortOrder;

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery, paramList.ToArray());
                result = GetActionsFromReader(reader);
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

        public int CountActions(ActionSearchCriteria criteria, int pageSize,
            int pageNumber, string sortOrder, string sortOrderInvert)
        {
            string sqlCount = @"select count(*)
                          from tblAction t1
						  left outer join tblSociete t2 on t1.SocieteID = t2.SocieteID
						  left outer join tblParamUtilisateurs t3 on t1.Responsable = t3.UserID
                          left outer join tblParamTypeAction t4 on t1.TypeAction = t4.ParamActionID
                          left outer join tblCandidat t5 on t1.CandidatID = t5.CandidatID
                          left outer join tblSocieteContact t6 on t1.ContactID = t6.ContactID
                          ";            
            List<SqlParameter> paramList;
            //condition here :
            string whereCon = BuildWhereCondition(criteria, out paramList);
            sqlCount += whereCon;
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            return (int)SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sqlCount, paramList.ToArray());
        }

        private string BuildWhereCondition(ActionSearchCriteria criteria, out List<SqlParameter> paramList)
        {
            paramList = new List<SqlParameter>();
            string whereCon = string.Empty;
            if (!string.IsNullOrEmpty(criteria.Active))
            {
                if (criteria.Active == "Yes")
                    whereCon += " and t1.Actif = 1";
                else if (criteria.Active == "No")
                    whereCon += " and t1.Actif = 0";
            }
            if (criteria.DateFrom.HasValue)
            {
                whereCon += " and DateAction >= @DateFrom";
                paramList.Add(new SqlParameter("@DateFrom", criteria.DateFrom.Value));
            }
            if (criteria.DateTo.HasValue)
            {
                whereCon += " and DateAction <= @DateTo";
                paramList.Add(new SqlParameter("@DateTo", criteria.DateTo.Value));
            }
            if (!string.IsNullOrEmpty(criteria.CanName))
            {
                whereCon += " and t5.Nom like @CanLastName";
                paramList.Add(new SqlParameter("@CanLastName", '%' + criteria.CanName + '%'));
            }
            if (!string.IsNullOrEmpty(criteria.ComName))
            {
                whereCon += " and t2.SocNom like @SocNom";
                paramList.Add(new SqlParameter("@SocNom", '%' + criteria.ComName + '%'));
            }
            if (criteria.TypeActionID.HasValue)
            {
                whereCon += " and TypeAction = @TypeAction";
                paramList.Add(new SqlParameter("@TypeAction", criteria.TypeActionID.Value));
            }
            if (!string.IsNullOrEmpty(criteria.Description))
            {
                whereCon += " and DescrAction like @DescrAction";
                paramList.Add(new SqlParameter("@DescrAction", '%' + criteria.Description + '%'));
            }
            if (!string.IsNullOrEmpty(criteria.Responsible))
            {
                whereCon += " and t1.Responsable = @Responsable";
                paramList.Add(new SqlParameter("@Responsable", criteria.Responsible));
            }
            if (!string.IsNullOrEmpty(whereCon))
            {
                whereCon = " where" + whereCon;
                whereCon = whereCon.Replace("where and", "where");
            }
            return whereCon;
        }

        private static IList<Action> GetActionsFromReader(SqlDataReader reader)
        {
            List<Action> result = new List<Action>();
            while (reader.Read())
            {
                Action action = new Action();
                action.ActionID = (int)reader["ActionID"];
                action.CompanyID = reader["SocieteID"] as int?;
                action.CompanyName = reader["SocNom"] as string;
                action.ContactID = reader["ContactID"] as int?;
                action.ContactFullName = reader["ConLastName"] as string;
                action.ContactFullName += " " + (reader["ConFirstName"] as string);
                action.JobID = reader["JobID"] as int?;
                action.CandidateID = reader["CandidatID"] as int?;
                action.CandidateFullName = reader["CanFirstName"] as string;
                action.CandidateFullName += " " + (reader["CanLastName"] as string);
                if (!(reader["DateAction"] is DBNull))
                    action.DateAction = (DateTime?)reader["DateAction"];
                if (!(reader["Heure"] is DBNull))
                    action.Hour = (DateTime?)reader["Heure"];
                action.LieuRDV = reader["LieuRDV"] as string;
                action.TypeAction = reader["TypeAction"] as int?;
                action.TypeActionLabel = reader["TypeActionLabel"] as string;
                action.DescrAction = reader["DescrAction"] as string;
                action.ResultCompany = reader["ResultSociete"] as string;
                action.ResultCandidate = reader["ResultCandidat"] as string;
                if (!(reader["Actif"] is DBNull))
                {
                    action.Actif = (bool)reader["Actif"];
                }
                action.Responsable = reader["Responsable"] as string;
                action.ResponsableName = reader["ResponsableName"] as string;
                action.DateCreation = reader["DateCreation"] as DateTime?;

                result.Add(action);
            }
            return result;
        }
    }
}
