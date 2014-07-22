using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace Neos.Data
{
    public class CandidateTrainingRepository : Repository<CandidateTraining>
    {
        public IList<CandidateTraining> GetCandidateTrainingByCandidateID(int candidateID)
        {
            IList<CandidateTraining> result = new List<CandidateTraining>();
            string sql = @"select t1.CandidatFormationID, t1.CandidatID, t1.Periode, 
                                  t1.FormationID, t2.libelle as FormationString, t1.Ecole, 
                                  t1.NiveauEtudeID, t3.libelle as StudyLevelString, t1.diplome
                            from tblCandidatFormation t1                            
                            left outer join tblParamFormation t2 on t1.FormationID = t2.FormationID
                            left outer join tblParamNiveauEtude t3 on t1.NiveauEtudeID = t3.NiveauEtudeID
                            where t1.CandidatID = {0} ";
            sql = string.Format(sql, candidateID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);                
                while (reader.Read())
                {
                    CandidateTraining item = new CandidateTraining();
                    item.CandidateFormationID = (int)reader["CandidatFormationID"];
                    item.CandidateID = candidateID;
                    item.Period = reader["Periode"] as string;
                    if (!(reader["FormationID"] is DBNull))
                    {
                        item.FormationID = (int)reader["FormationID"];
                    }
                    item.FormationString = reader["FormationString"] as string;
                    item.School = reader["Ecole"] as string;
                    if(!(reader["NiveauEtudeID"] is DBNull))
                    {
                        item.StudyLevelID = (int) reader["NiveauEtudeID"];
                    }
                    item.StudyLevelString = reader["StudyLevelString"] as string;
                    item.Diplome = reader["diplome"] as string;
                                        
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
