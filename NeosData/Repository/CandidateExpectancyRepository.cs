using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace Neos.Data
{
    public class CandidateExpectancyRepository : Repository<CandidateExpectancy>
    {
        private static string _connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;

        public List<CandidateExpectancy> GetCandidateExpectancyOfCandidate(int candidateID)
        {
            List<CandidateExpectancy> result = new List<CandidateExpectancy>();
            string sql = @"select * ";
                   sql += @"from tblCandidatAttentesfonction inner join tblparamfonction ";
	               sql += @"on tblCandidatAttentesfonction.fonctionid=tblparamfonction.fonctionid ";
	               sql += @"inner join tblparamfonctionfam on tblparamfonction.fonctionfamid=tblparamfonctionfam.fonctionfamid ";
                   sql += @"where tblcandidatAttentesfonction.candidatid={0}";
            sql = string.Format(sql, candidateID);
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sql);
                result = GetCandidateExpectancyFromReader(reader);
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

        private List<CandidateExpectancy> GetCandidateExpectancyFromReader(SqlDataReader reader)
        {
            List<CandidateExpectancy> result = new List<CandidateExpectancy>();
            while (reader.Read())
            {
                CandidateExpectancy candExpectancy = new CandidateExpectancy();
                candExpectancy.CandidateExpectancyID = (int)reader["CandidatAttentesFonctionID"];
                candExpectancy.CandidatID = (int)reader["CandidatID"];
                candExpectancy.FunctionID = (int)reader["FonctionID"];
                candExpectancy.FunctionFam = reader["FonctionFamID"] as string;
                candExpectancy.Group = reader["Libelle"] as string;
                candExpectancy.Type = reader["Genre"] as string;

                result.Add(candExpectancy);
            }

            return result;
        }
    }
}
