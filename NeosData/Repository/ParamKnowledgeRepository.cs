using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;

namespace Neos.Data
{
    public class ParamKnowledgeRepository : Repository<ParamKnowledge>
    {
        public IList<ParamKnowledge> GetAllKnowledges()
        {
            IList<ParamKnowledge> result = new List<ParamKnowledge>();
            string sql = @"select t1.ConnaissanceID, t1.Code, t1.ConFamilleID, t1.Definition,
                                (select count(*) from tblCandidatConaiss t2
                                    where t1.ConnaissanceID = t2.ConnaissanceID) as NumberIDUsed
                                from dbo.tblParamConnaissance t1
                                order by t1.Code ASC";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamKnowledge item = new ParamKnowledge();
                    item.KnowledgeID = (int)reader["ConnaissanceID"];
                    item.Code = reader["Code"] as string;
                    item.KnowledgeFamID = reader["ConFamilleID"] as string;
                    item.Definition = reader["Definition"] as string;
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
    }
}
