using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Data;

namespace Neos.Data
{
    public class ParamTypeActionRepository : Repository<ParamTypeAction>
    {
        public IList<ParamTypeAction> GetAllParamTypeActions()
        {
            IList<ParamTypeAction> result = new List<ParamTypeAction>();
            string sql = @"select t1.ParamActionID, t1.libelle, t1.UnitCode,
                                (select count(*) from tblAction t2
                                    where t1.ParamActionID = t2.TypeAction) as NumberIDUsed
                                from dbo.tblParamTypeAction t1
                                order by t1.libelle ASC";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamTypeAction item = new ParamTypeAction();
                    item.ParamActionID = (int)reader["ParamActionID"];
                    item.Label = reader["libelle"] as string;
                    item.UnitCode = reader["UnitCode"] as string;
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

        public ParamTypeAction GetParamTypeActionByLibelle(string label)
        {
            ParamTypeAction item = null;
            string sql = @"select t1.ParamActionID, t1.libelle, t1.UnitCode,
                                (select count(*) from tblAction t2
                                    where t1.ParamActionID = t2.TypeAction) as NumberIDUsed
                                from dbo.tblParamTypeAction t1
                                where t1.libelle = @Label";

            SqlParameter param = new SqlParameter("@Label", label);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql, param);
                if (reader.Read())
                {
                    item = new ParamTypeAction();
                    item.ParamActionID = (int)reader["ParamActionID"];
                    item.Label = reader["libelle"] as string;
                    item.UnitCode = reader["UnitCode"] as string;
                    item.NumberIDUsed = (int)reader["NumberIDUsed"];                    
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
