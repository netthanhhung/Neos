using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace Neos.Data
{
    public class InvoiceDetailsRepository : Repository<InvoiceDetails>
    {
        public int? GetMaxInvoiceDetailOrderNumber(int idFactNumber, string idTypeInvoice, int idYear)
        {
            string sqlQuery = @"select max(IdLigneNumber)
                            from InvoiceDetails 
                            where IdFactNumber = @IdFactNumber
                            and IdTypeInvoice = @IdTypeInvoice
                            and IdYear = @IdYear";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdFactNumber", idFactNumber));
            paramList.Add(new SqlParameter("@IdTypeInvoice", idTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", idYear));

            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            object max = SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sqlQuery, paramList.ToArray());
            if (!(max is DBNull))
                return Convert.ToInt32(max);
            else
                return null;
        }

        public void InserNewInvoiceDetails(InvoiceDetails newItem)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"insert InvoiceDetails (IdFactNumber, IdTypeInvoice, IdYear, IdLigneNumber, 
                                            Description, Quantity, UnitPriceEuro, AmountEuro, VatCode)
                           values (@IdFactNumber, @IdTypeInvoice, @IdYear, @IdLigneNumber, 
                                            @Description, @Quantity, @UnitPriceEuro, @AmountEuro, @VatCode)";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdFactNumber", newItem.IdFactNumber));
            paramList.Add(new SqlParameter("@IdTypeInvoice", newItem.IdTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", newItem.IdYear));
            paramList.Add(new SqlParameter("@IdLigneNumber", newItem.IdLigneNumber));
            paramList.Add(new SqlParameter("@Description", newItem.Description));
            paramList.Add(new SqlParameter("@Quantity", newItem.Quantity));
            paramList.Add(new SqlParameter("@UnitPriceEuro", newItem.UnitPriceEuro));
            paramList.Add(new SqlParameter("@AmountEuro", newItem.AmountEuro));
            paramList.Add(new SqlParameter("@VatCode", newItem.VatCode));               

            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
        }

        public void UpdateInvoiceDetails(InvoiceDetails newItem)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"update InvoiceDetails set 
                            Description = @Description, Quantity = @Quantity,
                            UnitPriceEuro = @UnitPriceEuro, AmountEuro = @AmountEuro, VatCode = @VatCode
                            where IdFactNumber = @IdFactNumber
                            and IdTypeInvoice = @IdTypeInvoice
                            and IdYear = @IdYear
                            and IdLigneNumber = @IdLigneNumber";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdFactNumber", newItem.IdFactNumber));
            paramList.Add(new SqlParameter("@IdTypeInvoice", newItem.IdTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", newItem.IdYear));
            paramList.Add(new SqlParameter("@IdLigneNumber", newItem.IdLigneNumber));
            paramList.Add(new SqlParameter("@Description", newItem.Description));
            paramList.Add(new SqlParameter("@Quantity", newItem.Quantity));
            paramList.Add(new SqlParameter("@UnitPriceEuro", newItem.UnitPriceEuro));
            paramList.Add(new SqlParameter("@AmountEuro", newItem.AmountEuro));
            paramList.Add(new SqlParameter("@VatCode", newItem.VatCode));

            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
        }

        public IList<InvoiceDetails> GetInvoiceDetailsOfInvoice(int idFactNumber, string idTypeInvoice, int idYear, int? idLigneNumber) 
        {
            IList<InvoiceDetails> result = new List<InvoiceDetails>();
            string sqlQuery = @"select IdFactNumber, IdTypeInvoice, IdYear, IdLigneNumber, 
                                  Description, Quantity, UnitPriceEuro, AmountEuro, VatCode, t2.TauxVat
                            from InvoiceDetails t1
                            inner join InvoiceVatCodes t2 on t1.VatCode = t2.IdVatCode 
                            where IdFactNumber = @IdFactNumber
                            and IdTypeInvoice = @IdTypeInvoice
                            and IdYear = @IdYear";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdFactNumber", idFactNumber));
            paramList.Add(new SqlParameter("@IdTypeInvoice", idTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", idYear));
            if (idLigneNumber.HasValue)
            {
                sqlQuery += " and IdLigneNumber = @IdLigneNumber";
                paramList.Add(new SqlParameter("@IdLigneNumber", idLigneNumber));
            }
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery, paramList.ToArray());
                while (reader.Read())
                {
                    InvoiceDetails invoices = new InvoiceDetails();
                    invoices.IdFactNumber = Convert.ToInt32(reader["IdFactNumber"]);
                    invoices.IdTypeInvoice = reader["IdTypeInvoice"] as string;
                    invoices.IdYear = Convert.ToInt32(reader["IdYear"]);
                    invoices.IdLigneNumber = Convert.ToInt32(reader["IdLigneNumber"]);
                    invoices.InvoiceDetailsId = invoices.IdFactNumber.ToString() + "-" + invoices.IdTypeInvoice 
                        + "-" + invoices.IdYear.ToString() + "-" + invoices.IdLigneNumber.ToString();
                    invoices.Description = reader["Description"] as string;
                    invoices.Quantity = reader["Quantity"] as double?;
                    invoices.UnitPriceEuro = reader["UnitPriceEuro"] as double?;
                    invoices.AmountEuro = reader["AmountEuro"] as double?;   
                    invoices.AmountEuro = RoundDoubleToDouble2Digits(invoices.AmountEuro);

                    if(!(reader["VatCode"] is DBNull))
                        invoices.VatCode = Convert.ToInt32(reader["VatCode"]);
                    invoices.VatRate = reader["TauxVat"] as double?;
                    if (invoices.VatRate.HasValue && invoices.AmountEuro.HasValue)
                    {
                        invoices.AmountVAT = invoices.AmountEuro.Value * invoices.VatRate.Value / 100;
                        invoices.AmountVAT = RoundDoubleToDouble2Digits(invoices.AmountVAT);
                    }

                    if (invoices.AmountEuro.HasValue) 
                        invoices.TotalAmountVAT += invoices.AmountEuro;
                    if (invoices.AmountVAT.HasValue)
                        invoices.TotalAmountVAT += invoices.AmountVAT;
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

        public void DeleteInvoiceDetails(int idFactNumber, string idTypeInvoice, int idYear, int? idLigneNumber)
        {
            string sql = @"delete from InvoiceDetails                            
                            where IdFactNumber = @IdFactNumber
                            and IdTypeInvoice = @IdTypeInvoice
                            and IdYear = @IdYear";
            
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdFactNumber", idFactNumber));
            paramList.Add(new SqlParameter("@IdTypeInvoice", idTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", idYear));

            if (idLigneNumber.HasValue)
            {
                sql += " and IdLigneNumber = @IdLigneNumber";
                paramList.Add(new SqlParameter("@IdLigneNumber", idLigneNumber));
            }

            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
        }

        public double GetSumTotalDetailOfInvoice(int idFactNumber, string idTypeInvoice, int idYear)
        {
            double result = 0;
            string sqlQuery = @"select sum(t1.AmountEuro + t2.TauxVat * t1.AmountEuro / 100) from
                                dbo.InvoiceDetails t1 inner join dbo.InvoiceVatCodes t2 on t1.VatCode = t2.IdVatCode
                                where t1.IdFactNumber = @IdFactNumber
                                and t1.IdTypeInvoice = @IdTypeInvoice
                                and t1.IdYear = @IdYear";
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

        private double? RoundDoubleToDouble2Digits(double? amount)
        {
            if (amount.HasValue)
            {
                decimal dec = Convert.ToDecimal(amount.Value);
                dec = Decimal.Round(dec, 2, MidpointRounding.AwayFromZero);
                return Decimal.ToDouble(dec);
            }
            else
            {
                return null;
            }
        }
    }
}
