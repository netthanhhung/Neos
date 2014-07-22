using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace Neos.Data
{
    public class ParamProfileRepository : Repository<ParamProfile>
    {
        public IList<ParamProfile> GetAllProfiles()
        {
            IList<ParamProfile> result = new List<ParamProfile>();
            string sql = @"select t1.ProfileId, t1.Profile, t1.ProfileCode,
                                (select count(*) from tblJobs t2
                                    where t1.ProfileId = t2.Profile) as NumberProfileUsed
                                from dbo.tblParamProfiles t1
                                order by t1.ProfileId ASC";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamProfile item = new ParamProfile();
                    item.ProfileID = (int)reader["ProfileId"];
                    item.Profile = reader["Profile"] as string;
                    item.ProfileCode = reader["ProfileCode"] as string;
                    item.NumberProfileUsed = (int) reader["NumberProfileUsed"];
                    
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
