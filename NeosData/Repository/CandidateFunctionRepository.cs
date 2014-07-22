using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;

namespace Neos.Data
{
    public class CandidateFunctionRepository : Repository<CandidateFunction>
    {
        public IList<CandidateFunction> GetCandidateFunctionByCandidateID(int candidateID)
        {
            IList<CandidateFunction> result = new List<CandidateFunction>();
            string sql = @"select t1.CandidatFonctionID, t1.CandidatID, t1.FonctionID, t2.Libelle, t3.FonctionFamID, t3.Genre                                 
                            from tblCandidatFonction t1                            
                            inner join tblParamFonction t2 on t1.FonctionID = t2.FonctionID      
                            inner join tblParamFonctionFam t3 on t2.FonctionFamID = t3.FonctionFamID                       
                            where t1.CandidatID = {0} ";
            sql = string.Format(sql, candidateID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    CandidateFunction item = new CandidateFunction();
                    item.CandidateFunctionID = (int)reader["CandidatFonctionID"];
                    item.CandidateID = candidateID;
                    item.FunctionID = reader["FonctionID"] as int?;
                    item.Code = reader["Libelle"] as string; 
                    item.Group = reader["FonctionFamID"] as string;
                    item.Type = reader["Genre"] as string; 
                                       
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

        public IList<CandidateFunction> GetAllParamFunctionByFuntionFamID(string funtionFamID)
        {
            IList<CandidateFunction> result = new List<CandidateFunction>();
            int candidateFuntionID = -1;
            string sql = @"select FonctionID, FonctionFamID, Libelle                                  
                            from tblParamFonction                                                      
                            where FonctionFamID = '{0}' ";
            sql = string.Format(sql, funtionFamID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    CandidateFunction item = new CandidateFunction();                    
                    item.CandidateFunctionID = candidateFuntionID--;                    
                    item.FunctionID = reader["FonctionID"] as int?;
                    item.Group = reader["FonctionFamID"] as string; 
                    item.Code = reader["Libelle"] as string;
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

        public CandidateFunction GetFunctionByFunctionID(int functionID)
        {
            CandidateFunction item = null;
            string sql = @"select t1.FonctionID, t1.FonctionFamID, t1.Libelle, t2.Genre                               
                            from tblParamFonction t1
                            inner join tblParamFonctionFam t2 on t1.FonctionFamID = t2.FonctionFamID                                            
                            where t1.FonctionID = '{0}' ";
            sql = string.Format(sql, functionID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                if (reader.Read())
                {
                    item = new CandidateFunction();                    
                    item.FunctionID = reader["FonctionID"] as int?;
                    item.Group = reader["FonctionFamID"] as string;
                    item.Code = reader["Libelle"] as string;
                    item.Type = reader["Genre"] as string;                    
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return item;
        }
    }
}
