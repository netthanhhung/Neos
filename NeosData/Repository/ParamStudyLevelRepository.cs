using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace Neos.Data
{
    public class ParamStudyLevelRepository : Repository<ParamStudyLevel>
    {
        public IList<ParamStudyLevel> GetAllStudyLevels()
        {
            IList<ParamStudyLevel> result = new List<ParamStudyLevel>();
            string sql = @"select t1.NiveauEtudeID, t1.HierarchieValeur, t3.NiveauEtudeGenr, t1.libelle,
                                (select count(*) from tblCandidatFormation t2
                                    where t1.NiveauEtudeID = t2.NiveauEtudeID) as NumberIDUsed
                                from dbo.tblParamNiveauEtude t1
                                left outer join tblParamNiveauEtudeGenr t3 on t1.HierarchieValeur = t3.NiveauEtudeGenrID
                                order by t1.NiveauEtudeID ASC";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamStudyLevel item = new ParamStudyLevel();
                    item.SchoolID = (int)reader["NiveauEtudeID"];
                    item.ValueHierarchy = (int?) reader["HierarchieValeur"];
                    item.ValueHierarchyString = reader["NiveauEtudeGenr"] as string;
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
    }
}
