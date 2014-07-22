using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;

namespace Neos.Data
{
    public class ParamFunctionRepository : Repository<ParamFunction>
    {
        public IList<ParamFunction> GetAllFunctions()
        {
            IList<ParamFunction> result = new List<ParamFunction>();
            string sql = @"select t1.FonctionID, t1.FonctionFamID, t1.Libelle,
                                (select count(*) from tblCandidatFonction t2
                                    where t1.FonctionID = t2.FonctionID) as NumberIDUsed
                                from dbo.tblParamFonction t1
                                order by t1.Libelle ASC";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamFunction item = new ParamFunction();
                    item.FunctionID = (int) reader["FonctionID"];
                    item.FunctionFamID = reader["FonctionFamID"] as string;
                    item.Label = reader["Libelle"] as string;
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
