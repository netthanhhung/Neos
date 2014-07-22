using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;

namespace Neos.Data
{
    public class ParamLocationsRepository : Repository<ParamLocations>
    {
        public IList<ParamLocations> GetAllLocations()
        {
            IList<ParamLocations> result = new List<ParamLocations>();
            string sql = @"select t1.Location, t1.Hierarchie, t1.LocationUk, t1.LocationNl, t1.CodeLocation,
                                (select count(*) from tblCandidat t2
                                    where t1.CodeLocation = t2.CodeLocation) as NumberCodeUsed,
                                (select count(*) from tblJobs t3
                                    where t1.Location = t3.Location) as NumberIDUsed
                                from dbo.tblParamLocations t1
                                order by t1.Location ASC";

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql);
                while (reader.Read())
                {
                    ParamLocations item = new ParamLocations();
                    item.Location = reader["Location"] as string;
                    item.Hierarchie = (int?)reader["Hierarchie"];
                    item.LocationUk = reader["LocationUk"] as string;
                    item.LocationNL = reader["LocationNl"] as string;
                    item.CodeLocation = reader["CodeLocation"] as string;
                    item.NumberCodeUsed = (int) reader["NumberCodeUsed"];
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

        public ParamLocations GetLocation(string location)
        {
            ParamLocations item = null;
            string sql = @"select t1.Location, t1.Hierarchie, t1.LocationUk, t1.LocationNl, t1.CodeLocation,
                                (select count(*) from tblCandidat t2
                                    where t1.CodeLocation = t2.CodeLocation) as NumberCodeUsed,
                                (select count(*) from tblJobs t3
                                    where t1.Location = t3.Location) as NumberIDUsed
                                from dbo.tblParamLocations t1
                                where t1.Location = @Location";
            SqlParameter param = new SqlParameter("@Location", location);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql, param);
                if (reader.Read())
                {
                    item = new ParamLocations();
                    item.Location = reader["Location"] as string;
                    item.Hierarchie = (int?)reader["Hierarchie"];
                    item.LocationUk = reader["LocationUk"] as string;
                    item.LocationNL = reader["LocationNl"] as string;
                    item.CodeLocation = reader["CodeLocation"] as string;
                    item.NumberCodeUsed = (int)reader["NumberCodeUsed"];
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

        public ParamLocations GetLocationByCode(string code)
        {
            ParamLocations item = null;
            string sql = @"select t1.Location, t1.Hierarchie, t1.LocationUk, t1.LocationNl, t1.CodeLocation,
                                (select count(*) from tblCandidat t2
                                    where t1.CodeLocation = t2.CodeLocation) as NumberCodeUsed,
                                (select count(*) from tblJobs t3
                                    where t1.Location = t3.Location) as NumberIDUsed
                                from dbo.tblParamLocations t1
                                where t1.CodeLocation = @CodeLocation";
            SqlParameter param = new SqlParameter("@CodeLocation", code);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sql, param);
                if (reader.Read())
                {
                    item = new ParamLocations();
                    item.Location = reader["Location"] as string;
                    item.Hierarchie = (int?)reader["Hierarchie"];
                    item.LocationUk = reader["LocationUk"] as string;
                    item.LocationNL = reader["LocationNl"] as string;
                    item.CodeLocation = reader["CodeLocation"] as string;
                    item.NumberCodeUsed = (int)reader["NumberCodeUsed"];
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

        public void InserNewLocation(ParamLocations newLocation)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"insert tblParamLocations (Location, Hierarchie, LocationUk, LocationNl, CodeLocation)
                           values (@Location, @Hierarchie, @LocationUk, @LocationNl, @CodeLocation)";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@Location", newLocation.Location));
            paramList.Add(new SqlParameter("@Hierarchie", newLocation.Hierarchie));
            paramList.Add(new SqlParameter("@LocationUk", newLocation.LocationUk));
            paramList.Add(new SqlParameter("@LocationNl", newLocation.LocationNL));
            paramList.Add(new SqlParameter("@CodeLocation", newLocation.CodeLocation));
            
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
        }
    }
}
