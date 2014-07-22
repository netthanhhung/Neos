using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace Neos.Data
{
    public class ParamTypeRepository : Repository<ParamType>
    {
        public void InserNewUnit(ParamType unit)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"insert tblParamType (TypeID, Libele)
                           values (@TypeID, @Libele)";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@TypeID", unit.TypeID));
            paramList.Add(new SqlParameter("@Libele", unit.Label));           

            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
        }
    }
}
