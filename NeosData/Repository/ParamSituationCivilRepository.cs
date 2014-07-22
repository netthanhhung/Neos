using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace Neos.Data
{
    public class ParamSituationCivilRepository : Repository<ParamSituationCivil>
    {
        public IList<ParamSituationCivil> GetAllSituationCivils()
        {
            IList<ParamSituationCivil> result = new List<ParamSituationCivil>();
            string sql = @"select t1.Code, t1.CodeType, t1.libelle,
                                (select count(*) from tblCandidat t2
                                    where t1.Code = t2.SituationCivile) as NumberIDUsed
                                from dbo.tblParamSituationCivile t1
                                order by t1.libelle ASC";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamSituationCivil item = new ParamSituationCivil();
                    item.Code = reader["Code"] as string;
                    item.CodeType = reader["CodeType"] as string;
                    item.Label = reader["libelle"] as string;
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

        public ParamSituationCivil GetSituationCivil(string code)
        {
            ParamSituationCivil result = null;
            string sql = @"select t1.Code, t1.CodeType, t1.libelle,
                                (select count(*) from tblCandidat t2
                                    where t1.Code = t2.SituationCivile) as NumberIDUsed
                                from dbo.tblParamSituationCivile t1
                                where t1.Code = @Code";
            SqlParameter param = new SqlParameter("@Code", code);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql, param);
                if (reader.Read())
                {
                    result = new ParamSituationCivil();
                    result.Code = reader["Code"] as string;
                    result.CodeType = reader["CodeType"] as string;
                    result.Label = reader["libelle"] as string;
                    result.NumberIDUsed = (int)reader["NumberIDUsed"];
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

        public void InserNewSituationCivil(ParamSituationCivil situation)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"insert tblParamSituationCivile (Code, CodeType, libelle)
                           values (@Code, @CodeType, @libelle)";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@Code", situation.Code));
            paramList.Add(new SqlParameter("@CodeType", situation.CodeType));
            paramList.Add(new SqlParameter("@libelle", situation.Label));

            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
        }
    }
}
