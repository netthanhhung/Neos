using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Collections;

namespace Neos.Data
{
    public class InvoiceVatCodesRepository : Repository<InvoiceVatCodes>
    {
        public IList<InvoiceVatCodes> GetAllVatCode()
        {
            IList<InvoiceVatCodes> result = new List<InvoiceVatCodes>();
            string sqlQuery = @"select IdVatCode, TauxVat from InvoiceVatCodes";
            
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery);
                while (reader.Read())
                {
                    InvoiceVatCodes item = new InvoiceVatCodes();
                    item.IdVatCode = Convert.ToInt32(reader["IdVatCode"]);
                    item.TauxVat = reader["TauxVat"] as double?;
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
