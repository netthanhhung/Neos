using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;

namespace Neos.Data
{
    public class CandidateKnowledgeRepository : Repository<CandidateKnowledge>
    {
        public IList<CandidateKnowledge> GetCandidateKnowledgeByCandidateID(int candidateID)
        {
            IList<CandidateKnowledge> result = new List<CandidateKnowledge>();
            string sql = @"select t1.CandidatConnaissID, t1.CandidatID, t1.ConnaissanceID, t2.Code, t3.ConFamilleID, t3.Genre                              
                            from tblCandidatConaiss t1                            
                            inner join tblParamConnaissance t2 on t1.ConnaissanceID = t2.ConnaissanceID 
                            inner join tblParamConnaisFam t3 on t2.ConFamilleID = t3.ConFamilleID                                                        
                            where t1.CandidatID = {0} ";
            sql = string.Format(sql, candidateID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    CandidateKnowledge item = new CandidateKnowledge();
                    item.CandidateKnowledgeID = (int)reader["CandidatConnaissID"];
                    item.CandidateID = candidateID;
                    item.KnowledgeID = reader["ConnaissanceID"] as int?;
                    item.Code = reader["Code"] as string;
                    item.Group = reader["ConFamilleID"] as string;
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

        public IList<CandidateKnowledge> GetAllParamKnowledgeByKnowledgeFamID(string knowledgeFamID)
        {
            IList<CandidateKnowledge> result = new List<CandidateKnowledge>();
            int candidateKnowledgeID = -1;
            string sql = @"select ConnaissanceID, Code, ConFamilleID
                            from tblParamConnaissance                            
                            where ConFamilleID = '{0}' ";
            sql = string.Format(sql, knowledgeFamID);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    CandidateKnowledge item = new CandidateKnowledge();
                    item.CandidateKnowledgeID = candidateKnowledgeID--;                                        
                    item.KnowledgeID = reader["ConnaissanceID"] as int?;
                    item.Group = reader["ConFamilleID"] as string;
                    item.Code = reader["Code"] as string;
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
