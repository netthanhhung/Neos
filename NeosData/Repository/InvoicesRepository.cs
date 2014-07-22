using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace Neos.Data
{
    public class InvoicesRepository : Repository<Invoices>
    {
        public Invoices GetInvoiceByID(int idFactNumber, string idTypeInvoice, int idYear)
        {
            IList<Invoices> result = new List<Invoices>();
            string sqlQuery = @"select t1.IdFactNumber, t1.IdTypeInvoice, t1.IdYear, t1.Date, t1.Currency, t1.TotalHtvaEuro, 
                                        t1.amountVatEuro, t1.DateOfPayement, t1.Duedate, t1.Payement, t1.RefCustomerNumber, 
                                        t1.Remark, Remark_Internal, t1.invnumber, t1.Factoring, t3.SocNom, t3.SocieteID
                                from Invoices t1
                                left outer join tblSocieteAdressesFacturation t2 on t1.RefCustomerNumber = t2.ID
                                left outer join tblSociete t3 on t2.SocieteID = t3.SocieteID
                                where t1.IdFactNumber = @IdFactNumber                                
                                and t1.IdTypeInvoice = @IdTypeInvoice
                                and t1.IdYear = @IdYear";                             
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdFactNumber", idFactNumber));
            paramList.Add(new SqlParameter("@IdTypeInvoice", idTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", idYear));
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery, paramList.ToArray());
                result = GetInvoicesFromReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            if (result.Count == 1)
                return result[0];
            else
                return null;            
        }
        
        public void InserNewInvoices(Invoices newItem)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"insert Invoices (IdFactNumber, IdTypeInvoice, IdYear, Date, Currency, TotalHtvaEuro, 
                                            amountVatEuro, DateOfPayement, Duedate, Payement, RefCustomerNumber, 
                                            Remark, Remark_Internal, invnumber, Factoring)
                           values (@IdFactNumber, @IdTypeInvoice, @IdYear, @Date, @Currency, @TotalHtvaEuro, 
                                    @amountVatEuro, @DateOfPayement, @Duedate, @Payement, @RefCustomerNumber, 
                                    @Remark, @Remark_Internal, @invnumber, @Factoring)";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdFactNumber", newItem.IdFactNumber));
            paramList.Add(new SqlParameter("@IdTypeInvoice", newItem.IdTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", newItem.IdYear));
            paramList.Add(new SqlParameter("@Date", newItem.Date));
            paramList.Add(new SqlParameter("@Currency", newItem.Currency));
            paramList.Add(new SqlParameter("@TotalHtvaEuro", newItem.TotalHtvaEuro));
            paramList.Add(new SqlParameter("@amountVatEuro", newItem.AmountVatEuro));
            paramList.Add(new SqlParameter("@DateOfPayement", newItem.DateOfPayement));
            paramList.Add(new SqlParameter("@Duedate", newItem.Duedate));
            paramList.Add(new SqlParameter("@Payement", newItem.Payement));
            paramList.Add(new SqlParameter("@RefCustomerNumber", newItem.RefCustomerNumber));
            paramList.Add(new SqlParameter("@Remark", newItem.Remark));
            paramList.Add(new SqlParameter("@Remark_Internal", newItem.Remark_Internal));
            paramList.Add(new SqlParameter("@invnumber", newItem.Invnumber));
            paramList.Add(new SqlParameter("@Factoring", newItem.Factoring));    

            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
        }

        public void UpdateInvoices(Invoices newItem)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"update Invoices set ";
            if(newItem.IdFactNumberNew.HasValue) 
                sql +=    @"IdFactNumber = @IdFactNumberNew,";

            sql += @"Date = @Date, Currency = @Currency , TotalHtvaEuro = @TotalHtvaEuro, 
                            amountVatEuro = @amountVatEuro, DateOfPayement =@DateOfPayement , Duedate = @Duedate, 
                            Payement = @Payement, RefCustomerNumber = @RefCustomerNumber, 
                            Remark = @Remark, Remark_Internal = @Remark_Internal, invnumber = @invnumber, Factoring = @Factoring
                            where IdFactNumber = @IdFactNumber
                            and IdTypeInvoice = @IdTypeInvoice
                            and IdYear = @IdYear";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdFactNumber", newItem.IdFactNumber));
            paramList.Add(new SqlParameter("@IdTypeInvoice", newItem.IdTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", newItem.IdYear));
            paramList.Add(new SqlParameter("@Date", newItem.Date));
            paramList.Add(new SqlParameter("@Currency", newItem.Currency));
            paramList.Add(new SqlParameter("@TotalHtvaEuro", newItem.TotalHtvaEuro));
            paramList.Add(new SqlParameter("@amountVatEuro", newItem.AmountVatEuro));
            paramList.Add(new SqlParameter("@DateOfPayement", newItem.DateOfPayement));
            paramList.Add(new SqlParameter("@Duedate", newItem.Duedate));
            paramList.Add(new SqlParameter("@Payement", newItem.Payement));
            paramList.Add(new SqlParameter("@RefCustomerNumber", newItem.RefCustomerNumber));
            paramList.Add(new SqlParameter("@Remark", newItem.Remark));
            paramList.Add(new SqlParameter("@Remark_Internal", newItem.Remark_Internal));
            paramList.Add(new SqlParameter("@invnumber", newItem.Invnumber));
            paramList.Add(new SqlParameter("@Factoring", newItem.Factoring));    

            if (newItem.IdFactNumberNew.HasValue)
            {
                paramList.Add(new SqlParameter("@IdFactNumberNew", newItem.IdFactNumberNew));

                //Also update detail and payments
                InvoiceDetailsRepository detailRepo = new InvoiceDetailsRepository();
                InvoicePaymentsRepository paymentRepo = new InvoicePaymentsRepository();

                IList<InvoiceDetails> detailList = detailRepo.GetInvoiceDetailsOfInvoice(
                    newItem.IdFactNumber, newItem.IdTypeInvoice, newItem.IdYear, null);
                IList<InvoicePayments> paymentList = paymentRepo.GetInvoicePaymentsOfInvoice(
                       newItem.IdFactNumber, newItem.IdTypeInvoice, newItem.IdYear);

                detailRepo.DeleteInvoiceDetails(newItem.IdFactNumber, newItem.IdTypeInvoice, newItem.IdYear, null);
                paymentRepo.DeleteInvoicePaymentsOfInvoice(newItem.IdFactNumber, newItem.IdTypeInvoice, newItem.IdYear);

                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
                foreach (InvoiceDetails detail in detailList)
                {
                    detail.IdFactNumber = newItem.IdFactNumberNew.Value;
                    detailRepo.InserNewInvoiceDetails(detail);
                }
                foreach (InvoicePayments payment in paymentList)
                {
                    payment.IdFactNumber = newItem.IdFactNumberNew.Value;
                    paymentRepo.Insert(payment);
                }
            }
            else
            {
                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
            }
//            if (newItem.IdFactNumberNew.HasValue)
//            {
//                sql = @"update InvoiceDetails set IdFactNumber = @IdFactNumberNew
//                        where IdFactNumber = @IdFactNumber
//                        and IdTypeInvoice = @IdTypeInvoice
//                        and IdYear = @IdYear";
//                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());


//                sql = @"update InvoicePayments set IdFactNumber = @IdFactNumberNew
//                        where IdFactNumber = @IdFactNumber
//                        and IdTypeInvoice = @IdTypeInvoice
//                        and IdYear = @IdYear";
//                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());
//            }
        }

        public IList<Invoices> SearchInvoices(InvoicesSearchCriteria criteria, int pageSize,
            int pageNumber, string sortOrder, string sortOrderInvert)
        {
            IList<Invoices> result = new List<Invoices>();
            string sqlQuery = @"select {0} t1.IdFactNumber, t1.IdTypeInvoice, t1.IdYear, t1.Date, t1.Currency, t1.TotalHtvaEuro, 
                                        t1.amountVatEuro, t1.DateOfPayement, t1.Duedate, t1.Payement, t1.RefCustomerNumber, 
                                        t1.Remark, Remark_Internal, t1.invnumber, t1.Factoring, t3.SocNom, t3.SocieteID
                                from Invoices t1
                                left outer join tblSocieteAdressesFacturation t2 on t1.RefCustomerNumber = t2.ID
                                left outer join tblSociete t3 on t2.SocieteID = t3.SocieteID";
            List<SqlParameter> paramList;
            //condition here :
            string whereCon = BuildWhereCondition(criteria, out paramList);
            sqlQuery += whereCon;
            string sortOrder1 = sortOrder;            

            sqlQuery += " order by " + sortOrder1;
            sqlQuery = string.Format(sqlQuery, "top " + pageSize * pageNumber + " ");
            sqlQuery = "(select top " + pageSize + " * from \n (" + sqlQuery + " ) As T1 \n"
                    + " Order by " + sortOrderInvert;
            sqlQuery = "select * from \n " + sqlQuery + " ) As T2 \n"
                    + " Order by " + sortOrder;

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery, paramList.ToArray());
                result = GetInvoicesFromReader(reader);
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

        public IList<Invoices> SearchInvoicesWithoutPage(InvoicesSearchCriteria criteria)
        {
            IList<Invoices> result = new List<Invoices>();
            string sqlQuery = @"select t1.IdFactNumber, t1.IdTypeInvoice, t1.IdYear, t1.Date, t1.Currency, t1.TotalHtvaEuro, 
                                        t1.amountVatEuro, t1.DateOfPayement, t1.Duedate, t1.Payement, t1.RefCustomerNumber, 
                                        t1.Remark, Remark_Internal, t1.invnumber, t1.Factoring, t3.SocNom, t3.SocieteID
                                from Invoices t1
                                left outer join tblSocieteAdressesFacturation t2 on t1.RefCustomerNumber = t2.ID
                                left outer join tblSociete t3 on t2.SocieteID = t3.SocieteID";
            List<SqlParameter> paramList;
            //condition here :
            string whereCon = BuildWhereCondition(criteria, out paramList);
            sqlQuery += whereCon;
            sqlQuery += " order by t1.IdYear DESC, t1.IdFactNumber DESC, t1.IdTypeInvoice ASC";
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery, paramList.ToArray());
                result = GetInvoicesFromReader(reader);
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

        public int CountInvoices(InvoicesSearchCriteria criteria, int pageSize,
            int pageNumber, string sortOrder, string sortOrderInvert)
        {
            string sqlCount = @"select count(*) 
                                from Invoices t1
                                left outer join tblSocieteAdressesFacturation t2 on t1.RefCustomerNumber = t2.ID
                                left outer join tblSociete t3 on t2.SocieteID = t3.SocieteID";
            List<SqlParameter> paramList;
            //condition here :
            string whereCon = BuildWhereCondition(criteria, out paramList);
            sqlCount += whereCon;
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            return (int)SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sqlCount, paramList.ToArray());
        }

        private string BuildWhereCondition(InvoicesSearchCriteria criteria, out List<SqlParameter> paramList)
        {
            paramList = new List<SqlParameter>();
            string whereCon = string.Empty;
            if (criteria.InvoiceNumberFrom.HasValue)
            {
                whereCon += " and t1.IdFactNumber >= @IdFactNumberFrom";     
                paramList.Add(new SqlParameter("@IdFactNumberFrom", criteria.InvoiceNumberFrom.Value));
            }
            if (criteria.InvoiceNumberTo.HasValue)
            {
                whereCon += " and t1.IdFactNumber <= @IdFactNumberTo";
                paramList.Add(new SqlParameter("@IdFactNumberTo", criteria.InvoiceNumberTo.Value));
            }

            if (criteria.FiscalYear.HasValue)
            {
                whereCon += " and t1.IdYear = @IdYear";
                paramList.Add(new SqlParameter("@IdYear", criteria.FiscalYear.Value));
            }

            if (criteria.InvoiceDateFrom.HasValue)
            {
                whereCon += " and t1.Date >= @DateFrom";
                paramList.Add(new SqlParameter("@DateFrom", criteria.InvoiceDateFrom.Value));
            }
            if (criteria.InvoiceDateTo.HasValue)
            {
                whereCon += " and t1.Date <= @DateTo";
                paramList.Add(new SqlParameter("@DateTo", criteria.InvoiceDateTo.Value));
            }


            if (!string.IsNullOrEmpty(criteria.InvoiceType))
            {
                whereCon += " and t1.IdTypeInvoice = @IdTypeInvoice";
                paramList.Add(new SqlParameter("@IdTypeInvoice", criteria.InvoiceType));
            }

            if (criteria.Customer.HasValue)
            {
                whereCon += " and t3.SocieteID = @SocieteID";
                paramList.Add(new SqlParameter("@SocieteID", criteria.Customer.Value));
            }
            
            if (!string.IsNullOrEmpty(whereCon))
            {
                whereCon = " where" + whereCon;
                whereCon = whereCon.Replace("where and", "where");
            }
            return whereCon;
        }

        private IList<Invoices> GetInvoicesFromReader(SqlDataReader reader)
        {
            List<Invoices> result = new List<Invoices>();
            while (reader.Read())
            {
                Invoices invoices = new Invoices();
                invoices.IdFactNumber = Convert.ToInt32(reader["IdFactNumber"]);
                invoices.IdTypeInvoice = reader["IdTypeInvoice"] as string;
                invoices.IdYear = Convert.ToInt32(reader["IdYear"]);
                invoices.InvoiceIdPK = invoices.IdFactNumber.ToString() + "-" 
                        + invoices.IdTypeInvoice + "-" + invoices.IdYear.ToString();
                invoices.Date = reader["Date"] as DateTime?;
                invoices.Currency = reader["Currency"] as string;
                invoices.TotalHtvaEuro = reader["TotalHtvaEuro"] as double?;
                invoices.TotalHtvaEuro = RoundDoubleToDouble2Digits(invoices.TotalHtvaEuro);

                invoices.AmountVatEuro = reader["amountVatEuro"] as double?;
                invoices.AmountVatEuro = RoundDoubleToDouble2Digits(invoices.AmountVatEuro);
                
                invoices.TotalAmountIncludeVatEuro = 0;
                if (invoices.TotalHtvaEuro.HasValue)
                    invoices.TotalAmountIncludeVatEuro += invoices.TotalHtvaEuro.Value;
                if (invoices.AmountVatEuro.HasValue)
                    invoices.TotalAmountIncludeVatEuro += invoices.AmountVatEuro.Value;
                //invoices.TotalAmountIncludeVatEuro = RoundDoubleToDouble2Digits(invoices.TotalAmountIncludeVatEuro);
                invoices.DateOfPayement = reader["DateOfPayement"] as DateTime?;
                invoices.Duedate = reader["Duedate"] as DateTime?;
                invoices.Payement = reader["Payement"] as bool?;
                invoices.RefCustomerNumber = reader["RefCustomerNumber"] as int?;
                invoices.Remark = reader["Remark"] as string;
                if (!string.IsNullOrEmpty(invoices.Remark) && invoices.Remark.Length > 15)
                    invoices.RemarkToShowed = invoices.Remark.Substring(0, 15) + "...";
               
                invoices.Remark_Internal = reader["Remark_Internal"] as string;
                if (!string.IsNullOrEmpty(invoices.Remark_Internal) && invoices.Remark_Internal.Length > 15)
                    invoices.InternalRemarkToShowed = invoices.Remark_Internal.Substring(0, 15) + "...";

                invoices.Invnumber = reader["invnumber"] as int?;
                invoices.CompanyName = reader["SocNom"] as string;
                invoices.CompanyId = reader["SocieteID"] as int?;
                invoices.Factoring = reader["Factoring"] as bool?;
                
                result.Add(invoices);
            }
            return result;
        }

        public void DeleteInvoice(int idFactNumber, string idTypeInvoice, int idYear)
        {
            string sql = @"delete from Invoices                            
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

        public int? GetMaxInvoiceNumber(int idYear, string idTypeInvoice, bool isFutureInvoice, int firstFutureNumber)
        {
            string sqlQuery = @"select max(IdFactNumber)
                            from Invoices 
                            where IdTypeInvoice = @IdTypeInvoice
                            and IdYear = @IdYear";
            List<SqlParameter> paramList = new List<SqlParameter>();            
            paramList.Add(new SqlParameter("@IdTypeInvoice", idTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", idYear));
            if (isFutureInvoice)
            {
                sqlQuery += " and IdFactNumber >= @FirstFutureNumber";                
            }
            else
            {
                sqlQuery += " and IdFactNumber < @FirstFutureNumber";
            }
            paramList.Add(new SqlParameter("@FirstFutureNumber", firstFutureNumber));

            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            int? result = SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sqlQuery, paramList.ToArray()) as int?;
            return result;
        }

        public Invoices GetInvoicesWithMaxNumber(int idYear, string idTypeInvoice, bool isFutureInvoice, int firstFutureNumber)
        {
            IList<Invoices> result = new List<Invoices>();
            string sqlQuery = @"select t1.IdFactNumber, t1.IdTypeInvoice, t1.IdYear, t1.Date, t1.Currency, t1.TotalHtvaEuro, 
                                        t1.amountVatEuro, t1.DateOfPayement, t1.Duedate, t1.Payement, t1.RefCustomerNumber, 
                                        t1.Remark, Remark_Internal, t1.invnumber, t1.Factoring, t3.SocNom, t3.SocieteID
                                from Invoices t1
                                left outer join tblSocieteAdressesFacturation t2 on t1.RefCustomerNumber = t2.ID
                                left outer join tblSociete t3 on t2.SocieteID = t3.SocieteID
                                where t1.IdTypeInvoice = @IdTypeInvoice
                                and t1.IdYear = @IdYear
                                and t1.IdFactNumber = (select max(IdFactNumber) from Invoices
						                                where IdTypeInvoice = @IdTypeInvoice
						                                and IdYear = @IdYear {0})";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdTypeInvoice", idTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", idYear));
            if (isFutureInvoice)
            {
                sqlQuery = string.Format(sqlQuery, " and IdFactNumber >= @FirstFutureNumber");
            }
            else
            {
                sqlQuery = string.Format(sqlQuery, " and IdFactNumber < @FirstFutureNumber");
            }
            paramList.Add(new SqlParameter("@FirstFutureNumber", firstFutureNumber));
            
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery, paramList.ToArray());
                result = GetInvoicesFromReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            if (result.Count == 1)
                return result[0];
            else
                return null;     
        }

        public Invoices GetPreviousInvoices(int idFactNumber, int idYear, string idTypeInvoice, bool isFutureInvoice, int firstFutureNumber)
        {
            IList<Invoices> result = new List<Invoices>();
            string sqlQuery = @"select t1.IdFactNumber, t1.IdTypeInvoice, t1.IdYear, t1.Date, t1.Currency, t1.TotalHtvaEuro, 
                                        t1.amountVatEuro, t1.DateOfPayement, t1.Duedate, t1.Payement, t1.RefCustomerNumber, 
                                        t1.Remark, Remark_Internal, t1.invnumber, t1.Factoring, t3.SocNom, t3.SocieteID
                                from Invoices t1
                                left outer join tblSocieteAdressesFacturation t2 on t1.RefCustomerNumber = t2.ID
                                left outer join tblSociete t3 on t2.SocieteID = t3.SocieteID
                                where t1.IdTypeInvoice = @IdTypeInvoice
                                and t1.IdYear = @IdYear
                                and t1.IdFactNumber = (select max(IdFactNumber) from Invoices
						                                where IdTypeInvoice = @IdTypeInvoice
						                                and IdYear = @IdYear 
                                                        and IdFactNumber < @IdFactNumber {0})";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdTypeInvoice", idTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", idYear));
            paramList.Add(new SqlParameter("@IdFactNumber", idFactNumber));
            if (isFutureInvoice)
            {
                sqlQuery = string.Format(sqlQuery, " and IdFactNumber >= @FirstFutureNumber");
            }
            else
            {
                sqlQuery = string.Format(sqlQuery, " and IdFactNumber < @FirstFutureNumber");
            }
            paramList.Add(new SqlParameter("@FirstFutureNumber", firstFutureNumber));

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery, paramList.ToArray());
                result = GetInvoicesFromReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            if (result.Count == 1)
                return result[0];
            else
                return null;
        }

        public Invoices GetNextInvoices(int idFactNumber, int idYear, string idTypeInvoice, bool isFutureInvoice, int firstFutureNumber)
        {
            IList<Invoices> result = new List<Invoices>();
            string sqlQuery = @"select t1.IdFactNumber, t1.IdTypeInvoice, t1.IdYear, t1.Date, t1.Currency, t1.TotalHtvaEuro, 
                                        t1.amountVatEuro, t1.DateOfPayement, t1.Duedate, t1.Payement, t1.RefCustomerNumber, 
                                        t1.Remark, Remark_Internal, t1.invnumber, t1.Factoring, t3.SocNom, t3.SocieteID
                                from Invoices t1
                                left outer join tblSocieteAdressesFacturation t2 on t1.RefCustomerNumber = t2.ID
                                left outer join tblSociete t3 on t2.SocieteID = t3.SocieteID
                                where t1.IdTypeInvoice = @IdTypeInvoice
                                and t1.IdYear = @IdYear
                                and t1.IdFactNumber = (select min(IdFactNumber) from Invoices
						                                where IdTypeInvoice = @IdTypeInvoice
						                                and IdYear = @IdYear 
                                                        and IdFactNumber > @IdFactNumber {0})";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@IdTypeInvoice", idTypeInvoice));
            paramList.Add(new SqlParameter("@IdYear", idYear));
            paramList.Add(new SqlParameter("@IdFactNumber", idFactNumber));
            if (isFutureInvoice)
            {
                sqlQuery = string.Format(sqlQuery, " and IdFactNumber >= @FirstFutureNumber");
            }
            else
            {
                sqlQuery = string.Format(sqlQuery, " and IdFactNumber < @FirstFutureNumber");
            }
            paramList.Add(new SqlParameter("@FirstFutureNumber", firstFutureNumber));

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery, paramList.ToArray());
                result = GetInvoicesFromReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            if (result.Count == 1)
                return result[0];
            else
                return null;
        }

        public void ReComputeAmountOfInvoice(int idFactNumber, string idTypeInvoice, int idYear)
        {
            string sql = @"update Invoices set 
                            TotalHtvaEuro = (select sum(AmountEuro) from InvoiceDetails 
				                            where IdFactNumber = @IdFactNumber
                                            and IdTypeInvoice = @IdTypeInvoice
                                            and IdYear = @IdYear), 
                            amountVatEuro = (select sum(VatAmount) from
                                                        (select t1.AmountEuro * t2.TauxVat/100 as VatAmount 
                                                        from InvoiceDetails t1
                                                        inner join dbo.InvoiceVatCodes t2 on t1.VatCode = t2.IdVatCode
                                                        where t1.IdFactNumber = @IdFactNumber
                                                        and t1.IdTypeInvoice = @IdTypeInvoice
                                                        and t1.IdYear = @IdYear) A 
				                            )
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

        public int CountUnpaidInvoices(int pageSize,
            int pageNumber, string sortOrder, string sortOrderInvert)
        {
            string sqlCount = @"select count(*) from 
	                            (select distinct n1.SocieteID, n1.SocNom as CompanyName, 
		                            ( (select (case when TotalAmount is null then 0 else TotalAmount end) as TotalAmount1 from
										(select (sum(TotalHtvaEuro) + sum(amountVatEuro)) as TotalAmount from Invoices t1
										inner join tblSocieteAdressesFacturation t2 on t1.RefCustomerNumber = t2.ID
										inner join tblSociete t3 on t2.SocieteID = t3.SocieteID
										where t3.SocieteID = n1.SocieteID) TT1)
		                            -	
									  (select (case when TotalAmount is null then 0 else TotalAmount end) as TotalAmount2 from
										(select sum(Amount) as TotalAmount from InvoicePayments h1
										inner join Invoices h2 on (h1.IDFactNumber = h2.IDFactNumber 
																	and h1.IDTypeInvoice = h2.IDTypeInvoice
																	and h1.IdYear = h2.IdYear)
										inner join tblSocieteAdressesFacturation h3 on h2.RefCustomerNumber = h3.ID
										inner join tblSociete h4 on h3.SocieteID = h4.SocieteID
										where h4.SocieteID = n1.SocieteID) TT2)											
									) as DueAmount,
                            		
		                            (select min(v1.Date) from Invoices v1
		                            inner join tblSocieteAdressesFacturation v2 on v1.RefCustomerNumber = v2.ID
		                            inner join tblSociete v3 on v2.SocieteID = v3.SocieteID
		                            where (v1.Payement = 0 or v1.Payement is null)                                    
		                            and v3.SocieteID = n1.SocieteID) as OldestDate

	                            from tblSociete n1 
	                            inner join tblSocieteAdressesFacturation n2 on n2.SocieteID = n1.SocieteID
	                            inner join Invoices n3 on n3.RefCustomerNumber = n2.ID
	                            where (n3.Payement = 0 or n3.Payement is null)) A

                            where DueAmount > 0";
                        
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            return (int)SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sqlCount);
        }

        public IList<InvoiceUnpaid> GetUnpaidInvoices(int pageSize,
            int pageNumber, string sortOrder, string sortOrderInvert)
        {
            IList<InvoiceUnpaid> result = new List<InvoiceUnpaid>();
            string sqlQuery = @"select {0} * from 
	                            (select distinct n1.SocieteID, n1.SocNom as CompanyName, 
                                    ( (select (case when TotalAmount is null then 0 else TotalAmount end) as TotalAmount1 from
			                            (select (sum(TotalHtvaEuro) + sum(amountVatEuro)) as TotalAmount from Invoices t1
			                            inner join tblSocieteAdressesFacturation t2 on t1.RefCustomerNumber = t2.ID
			                            inner join tblSociete t3 on t2.SocieteID = t3.SocieteID
			                            where t3.SocieteID = n1.SocieteID
                                        and (t1.Factoring is null or t1.Factoring = 0)) TT1)
                                    -	
		                              (select (case when TotalAmount is null then 0 else TotalAmount end) as TotalAmount2 from
			                            (select sum(Amount) as TotalAmount from InvoicePayments h1
			                            inner join Invoices h2 on (h1.IDFactNumber = h2.IDFactNumber 
										                            and h1.IDTypeInvoice = h2.IDTypeInvoice
										                            and h1.IdYear = h2.IdYear)
			                            inner join tblSocieteAdressesFacturation h3 on h2.RefCustomerNumber = h3.ID
			                            inner join tblSociete h4 on h3.SocieteID = h4.SocieteID
			                            where h4.SocieteID = n1.SocieteID
                                        and (h2.Factoring is null or  h2.Factoring = 0)) TT2)											
		                            ) as DueAmount,
                            		
                                    (select min(v1.Date) from Invoices v1
                                    inner join tblSocieteAdressesFacturation v2 on v1.RefCustomerNumber = v2.ID
                                    inner join tblSociete v3 on v2.SocieteID = v3.SocieteID
                                    where (v1.Payement = 0 or v1.Payement is null)                                    
                                    and v3.SocieteID = n1.SocieteID
                                    and (v1.Factoring is null or v1.Factoring = 0)) as OldestDate

                                from tblSociete n1 
                                inner join tblSocieteAdressesFacturation n2 on n2.SocieteID = n1.SocieteID
                                inner join Invoices n3 on n3.RefCustomerNumber = n2.ID
                                where (n3.Payement = 0 or n3.Payement is null)) A

                            where DueAmount > 0
                            ";
            
            //condition here :                        
            string sortOrder1 = sortOrder;

            sqlQuery += " order by " + sortOrder1;
            sqlQuery = string.Format(sqlQuery, "top " + pageSize * pageNumber + " ");
            sqlQuery = "(select top " + pageSize + " * from \n (" + sqlQuery + " ) As Y1 \n"
                    + " Order by " + sortOrderInvert;
            sqlQuery = "select * from \n " + sqlQuery + " ) As Y2 \n"
                    + " Order by " + sortOrder;

            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery);
                while (reader.Read())
                {
                    InvoiceUnpaid item = new InvoiceUnpaid();
                    item.CompanyID = (int)reader["SocieteID"];
                    item.CompanyName = reader["CompanyName"] as string;
                    item.DueAmount = reader["DueAmount"] as double?;
                    item.OldestDate = reader["OldestDate"] as DateTime?;
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

        public IList<Invoices> GetUnpaidInvoicesOfCompany(int companyID, bool loadFactoring)
        {
            IList<Invoices> result = new List<Invoices>();
            string sqlQuery = @"select t1.IdFactNumber, t1.IdTypeInvoice, t1.IdYear, t1.Date, t1.Currency, t1.TotalHtvaEuro, 
                                        t1.amountVatEuro, t1.DateOfPayement, t1.Duedate, t1.Payement, t1.RefCustomerNumber, 
                                        t1.Remark, Remark_Internal, t1.invnumber, t1.Factoring, t3.SocNom, t3.SocieteID
                                from Invoices t1
                                inner join tblSocieteAdressesFacturation t2 on t1.RefCustomerNumber = t2.ID
                                inner join tblSociete t3 on t2.SocieteID = t3.SocieteID
                                where t3.SocieteID = @SocieteID
                                and (t1.Payement is null or t1.Payement = 0)";

            SqlParameter param = new SqlParameter("@SocieteID", companyID);
            if (loadFactoring)
            {
                sqlQuery += " and (t1.Factoring is null or t1.Factoring = 0)";
            }
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery, param);
                result = GetInvoicesFromReader(reader);
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

        public IList<Invoices> GetFutureInvoices(int firstFutureNumber)
        {
            IList<Invoices> result = new List<Invoices>();
            string sqlQuery = @"select t1.IdFactNumber, t1.IdTypeInvoice, t1.IdYear, t1.Date, t1.Currency, t1.TotalHtvaEuro, 
                                        t1.amountVatEuro, t1.DateOfPayement, t1.Duedate, t1.Payement, t1.RefCustomerNumber, 
                                        t1.Remark, Remark_Internal, t1.invnumber, t1.Factoring, t3.SocNom, t3.SocieteID
                                from Invoices t1
                                inner join tblSocieteAdressesFacturation t2 on t1.RefCustomerNumber = t2.ID
                                inner join tblSociete t3 on t2.SocieteID = t3.SocieteID
                                where t1.IdFactNumber >= @FirstFutureNumber";

            SqlParameter param = new SqlParameter("@FirstFutureNumber", firstFutureNumber);
            SqlDataReader reader = null;
            try
            {
                string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
                reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlQuery, param);
                result = GetInvoicesFromReader(reader);
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

        public int CountNotConfirmedFutureInvoices(int firstFutureNumber)
        {
            string sqlCount = @"select count(*) 
                                from Invoices
                                where IdFactNumber >= @FirstFutureNumber
                                and Date <= GetDate()";
            SqlParameter param = new SqlParameter("@FirstFutureNumber", firstFutureNumber);
                        
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            return (int)SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sqlCount, param);        
        }

        public double? GetSumAmountNotVAT(string type, DateTime? startDate, DateTime? endDate)
        {
            string sqlQuery = @"select sum(TotalHtvaEuro) as IAmount from Invoices
                           where IdTypeInvoice = @Type";
            List<SqlParameter> paramList = new List<SqlParameter>();
            if (startDate.HasValue)
            {
                sqlQuery += " and Date >= @StartDate";
                paramList.Add(new SqlParameter("@StartDate", startDate.Value));
            }
            if (endDate.HasValue)
            {
                sqlQuery += " and Date <= @EndDate";
                paramList.Add(new SqlParameter("@EndDate", endDate.Value));
            }

            paramList.Add(new SqlParameter("@Type", type));
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            double? result = SqlHelper.ExecuteScalar(connectionString, CommandType.Text, sqlQuery, paramList.ToArray()) as double?;
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
