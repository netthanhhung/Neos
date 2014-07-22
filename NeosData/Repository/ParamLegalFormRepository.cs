using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace Neos.Data
{
    public class ParamLegalFormRepository : Repository<ParamLegalForm>
    {        
        public void UpdateLegalForm(string newForm, string oldForm) {
            string sql = "update tblParamFormJur set F = @NewForm where F = @OldForm";
            SqlParameter param1 = new SqlParameter("@NewForm", newForm);
            SqlParameter param2 = new SqlParameter("@OldForm", oldForm);

            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, param1, param2);
        }
    }
}
