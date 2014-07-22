using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace Neos.Data
{
    public class InvoicePaymentsRepository : Repository<InvoicePayments>
    {
        public InvoicePayments GetInvoicePaymentByID(int idPayment)
        {
            InvoicePayments invoices = null;
            string sqlQuery = @"select IDPayment, IdFactNumber, IdTypeInvoice, IdYear, 
                                    DatePayment, Amount, Remark
                            from InvoicePayments                            
                            where IDPayment = @IDPayment";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IDPayment", idPayment));

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery, paramList.ToArray());
                if (reader.Read())
                {
                    invoices = new InvoicePayments();
                    invoices.IdPayment = Convert.ToInt32(reader["IDPayment"]);
                    invoices.IdFactNumber = Convert.ToInt32(reader["IdFactNumber"]);
                    invoices.IdTypeInvoice = reader["IdTypeInvoice"] as string;
                    invoices.IdYear = Convert.ToInt32(reader["IdYear"]);

                    invoices.DatePayment = reader["DatePayment"] as DateTime?;
                    invoices.Amount = reader["Amount"] as double?;
                    invoices.Remark = reader["Remark"] as string;

                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return invoices;
        }

        public IList<InvoicePayments> GetInvoicePaymentsOfInvoice(int idFactNumber, string idTypeInvoice, int idYear) 
        {
            IList<InvoicePayments> result = new List<InvoicePayments>();
            string sqlQuery = @"select IDPayment, IdFactNumber, IdTypeInvoice, IdYear, 
                                    DatePayment, Amount, Remark
                            from InvoicePayments                            
                            where IdFactNumber = @IdFactNumber
                            and IdTypeInvoice = @IdTypeInvoice
                            and IdYear = @IdYear
                            order by DatePayment Asc";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdFactNumber", idFactNumber));
            paramList.Add(new SqlParameter("@IdTypeInvoice", idTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", idYear));
           
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery, paramList.ToArray());
                while (reader.Read())
                {
                    InvoicePayments invoices = new InvoicePayments();
                    invoices.IdPayment = Convert.ToInt32(reader["IDPayment"]);
                    invoices.IdFactNumber = Convert.ToInt32(reader["IdFactNumber"]);
                    invoices.IdTypeInvoice = reader["IdTypeInvoice"] as string;
                    invoices.IdYear = Convert.ToInt32(reader["IdYear"]);

                    invoices.DatePayment = reader["DatePayment"] as DateTime?;
                    invoices.Amount = reader["Amount"] as double?;
                    invoices.Remark = reader["Remark"] as string;
                    
                    result.Add(invoices);
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

        public void DeleteInvoicePaymentsOfInvoice(int idFactNumber, string idTypeInvoice, int idYear)
        {
            string sql = @"delete from InvoicePayments                            
                            where IdFactNumber = @IdFactNumber
                            and IdTypeInvoice = @IdTypeInvoice
                            and IdYear = @IdYear";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdFactNumber", idFactNumber));
            paramList.Add(new SqlParameter("@IdTypeInvoice", idTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", idYear));
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
        }

        public double GetSumPaymentOfInvoice(int idFactNumber, string idTypeInvoice, int idYear)
        {
            double result = 0;
            string sqlQuery = @"select sum(Amount) from dbo.InvoicePayments                           
                            where IdFactNumber = @IdFactNumber
                            and IdTypeInvoice = @IdTypeInvoice
                            and IdYear = @IdYear";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdFactNumber", idFactNumber));
            paramList.Add(new SqlParameter("@IdTypeInvoice", idTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", idYear));
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            object sum = SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sqlQuery, paramList.ToArray());
            if (!(sum is DBNull))
                result = (double)sum;
            return result;
        }

        public DateTime? GetLatestDatePaymentOfInvoice(int idFactNumber, string idTypeInvoice, int idYear)
        {
            string sqlQuery = @"select max(DatePayment)
                            from InvoicePayments                            
                            where IdFactNumber = @IdFactNumber
                            and IdTypeInvoice = @IdTypeInvoice
                            and IdYear = @IdYear";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdFactNumber", idFactNumber));
            paramList.Add(new SqlParameter("@IdTypeInvoice", idTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", idYear));

            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            DateTime? result = SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sqlQuery, paramList.ToArray()) as DateTime?;            
            return result;
        }
    }
}
