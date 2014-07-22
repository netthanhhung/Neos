using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace Neos.Data
{
    public class ParamFormationRepository : Repository<ParamFormation>
    {
        public static Comparison<ParamFormation> NameAscComparison = delegate(ParamFormation f1, ParamFormation f2)
        {
            return f1.Label.CompareTo(f2.Label);
        };

        public List<ParamFormation> FindAllWithAscSort()
        {
            List<ParamFormation> list = new List<ParamFormation>();
            list = this.FindAll() as List<ParamFormation>;
            list.Sort(NameAscComparison);

            return list;
        }        

        public IList<ParamFormation> GetAllParamFormations()
        {
            IList<ParamFormation> result = new List<ParamFormation>();
            string sql = @"select t1.FormationID, t1.libelle,
                                (select count(*) from tblCandidatFormation t2
                                    where t1.FormationID = t2.FormationID) as NumberIDUsed
                                from dbo.tblParamFormation t1
                                order by t1.libelle ASC";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamFormation item = new ParamFormation();
                    item.FormationID = (int) reader["FormationID"];
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

        public IList<ParamFormation> GetAllParamFormations(string label)
        {
            IList<ParamFormation> result = new List<ParamFormation>();
            string sql = @"select t1.FormationID, t1.libelle,
                                (select count(*) from tblCandidatFormation t2
                                    where t1.FormationID = t2.FormationID) as NumberIDUsed
                                from dbo.tblParamFormation t1
                                where t1.libelle = @libelle";
            SqlParameter param = new SqlParameter("@libelle", label);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql, param);
                while (reader.Read())
                {
                    ParamFormation item = new ParamFormation();
                    item.FormationID = (int)reader["FormationID"];
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
