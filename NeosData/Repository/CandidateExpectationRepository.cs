using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace Neos.Data
{
    public class CandidateExpectationRepository : Repository<CandidateExpectation>
    {
        public CandidateExpectation GetCandidateExpectation(int candidateId)
        {
            CandidateExpectation result = null;
            Filter filter = Filter.Eq("CandidateID", candidateId);
            IList<CandidateExpectation> list = this.FindAll(filter);
            if (list.Count == 1)
            {
                foreach (CandidateExpectation item in list)
                {
                    result = item;
                    break;
                }
            }
            return result;
        }

        public void InsertNewExpect(CandidateExpectation expect)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"insert tblCandidatAttentes (CandidatID, region, salaire, Societe, typecontrat, Fonction, Motivation) 
                           values (@CandidatID, @region, @salaire, @Societe, @typecontrat, @Fonction, @Motivation);";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@CandidatID", expect.CandidateID));
            paramList.Add(new SqlParameter("@region", expect.Region));
            paramList.Add(new SqlParameter("@salaire", expect.SalaryLevel));
            paramList.Add(new SqlParameter("@Societe", expect.Company));
            paramList.Add(new SqlParameter("@typecontrat", expect.TypeofContract));
            paramList.Add(new SqlParameter("@Fonction", expect.Function));
            paramList.Add(new SqlParameter("@Motivation", expect.Motivation));
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());                
        }
    }
}
