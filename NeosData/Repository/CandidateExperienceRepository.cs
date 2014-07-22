using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;

namespace Neos.Data
{
    public class CandidateExperienceRepository : Repository<CandidateExperience>
    {
        public IList<CandidateExperience> GetCandidateExperienceByCandidateID(int candidateID)
        {
            IList<CandidateExperience> result = new List<CandidateExperience>();
            string sql = @"select t1.CandidatExperienceID, t1.CandidatID, t1.Periode, 
                                  t1.Societe, t1.DescrFonction, t1.Salaire, t1.SalairePackage, t1.RaisonDepart,
                                  t1.FonctionID, t2.Libelle as FunctionString
                            from tblCandidatExperience t1                            
                            left outer join tblParamFonction t2 on t1.FonctionID = t2.FonctionID                            
                            where t1.CandidatID = {0} ";
            sql = string.Format(sql, candidateID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    CandidateExperience item = new CandidateExperience();
                    item.ExperienceID = (int)reader["CandidatExperienceID"];
                    item.CandidateID = candidateID;
                    item.Period = reader["Periode"] as string;
                    item.Company = reader["Societe"] as string;
                    item.FunctionDesc = reader["DescrFonction"] as string;
                    item.Salary = reader["Salaire"] as string;
                    item.ExtraAdvantage = reader["SalairePackage"] as string;
                    item.LeftReason = reader["RaisonDepart"] as string;
                    if (!(reader["FonctionID"] is DBNull))
                    {
                        item.FunctionID = (int)reader["FonctionID"];
                    }
                    item.FunctionString = reader["FunctionString"] as string;                    

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
