using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace Neos.Data
{
    public class CompanyRepository : Repository<Company>
    {
        private static string SELECT_COMPANY_QUERY = 
            @"Select {0} SocieteID, nTva, SocNom, FormeJuridique, Groupe, Adresse, CodePostal,Commune, 
            ZoneTel, Tel, Fax,Email,Activité, Segment, SegmentSpecialisation, c.Statut,
            Responsable, ConditionsTarif, Remarques, DateCreation, Secteur, Unitcode,
            WebLink, ZoneSponsor, p.Statut as StatusLabel, u.Nom as ResponsibleLastName
            From tblSociete c left join tblParamStatutClient p on c.Statut = p.StatutID
            left join tblParamUtilisateurs u on u.UserID = c.Responsable";

        private static string _connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
        public static Comparison<Company> NameAscComparison = delegate(Company c1, Company c2)
        {
            if (string.IsNullOrEmpty(c1.CompanyName))
                return 1;
            else if (string.IsNullOrEmpty(c2.CompanyName))
                return -1;
            else 
                return c1.CompanyName.CompareTo(c2.CompanyName);
        };

        public int CountAllCompanies()
        {
            string sqlQuery = "select count(*) from tblSociete";
            sqlQuery += " where SocNom is not null";            

            return (int) SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, sqlQuery);
        }

        public List<Company> GetAllCompanies()
        {
            List<Company> list = new List<Company>();

            string sqlQuery = SELECT_COMPANY_QUERY;
            sqlQuery = string.Format(sqlQuery, string.Empty);
            sqlQuery += " where SocNom is not null";
            sqlQuery += " order by SocNom ASC";

            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sqlQuery);
                list = GetCompanyFromDataReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return list;
        }

        public List<Company> GetTopCompany(int numberOfRecord)
        {
            List<Company> list = new List<Company>();
            string sqlQuery = string.Format(SELECT_COMPANY_QUERY, string.Format("top ({0})", numberOfRecord));
            sqlQuery += " Order by DateCreation desc";

            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sqlQuery);
                list = GetCompanyFromDataReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return list;
        }

        public List<Company> GetAllCompanies(int pageSize, int pageNumber, string sortOrder, string sortOrderInvert)
        {
            List<Company> list = new List<Company>();

            string sqlQuery = SELECT_COMPANY_QUERY;
            sqlQuery += " where SocNom is not null";
            sqlQuery += " order by " + sortOrder;
            sqlQuery = string.Format(sqlQuery, "top " + pageSize * pageNumber + " ");
            sqlQuery = "(select top " + pageSize + " * from \n (" + sqlQuery + " ) As T1 \n"
                    + " Order by " + sortOrderInvert;
            sqlQuery = "select * from \n " + sqlQuery + " ) As T2 \n"
                    + " Order by " + sortOrder;

            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sqlQuery);
                list = GetCompanyFromDataReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return list;
        }

        public Company FindOne(int companyID)
        {
            List<Company> list = new List<Company>();
            string sqlQuery = SELECT_COMPANY_QUERY;
            sqlQuery += " where SocieteID={1} and SocNom is not null";
            sqlQuery = string.Format(sqlQuery, string.Empty, companyID);

            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sqlQuery);
                list = GetCompanyFromDataReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return list.Count > 0 ? list[0] : null;
        }

        public int CountCustomerCompanies()
        {            
            string sqlQuery = "select count(*) from tblSociete";
            sqlQuery += " where SocNom is not null and Statut=1";            

            return (int)SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, sqlQuery);
        }
        public List<Company> GetCustomerCompanies(int pageSize, int pageNumber, string sortOrder, string sortOrderInvert)
        {
            List<Company> list = new List<Company>();

            string sqlQuery = SELECT_COMPANY_QUERY;
            sqlQuery += " where SocNom is not null and c.Statut=1";
            sqlQuery += " order by " + sortOrder;
            sqlQuery = string.Format(sqlQuery, "top " + pageSize * pageNumber + " ");
            sqlQuery = "(select top " + pageSize + " * from \n (" + sqlQuery + " ) As T1 \n"
                    + " Order by " + sortOrderInvert;
            sqlQuery = "select * from \n " + sqlQuery + " ) As T2 \n"
                    + " Order by " + sortOrder;

            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sqlQuery);
                list = GetCompanyFromDataReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return list;
        }

        public int CountInactiveCompanies()
        {            
            string sqlQuery = "select count(*) from tblSociete";
            sqlQuery += " where SocNom is not null and Statut=0";            

            return (int)SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, sqlQuery);
        }

        public List<Company> GetInactiveCompanies(int pageSize, int pageNumber, string sortOrder, string sortOrderInvert)
        {
            List<Company> list = new List<Company>();

            string sqlQuery = SELECT_COMPANY_QUERY;
            sqlQuery += " where SocNom is not null and c.Statut=0";
            sqlQuery += " order by " + sortOrder;
            sqlQuery = string.Format(sqlQuery, "top " + pageSize * pageNumber + " ");
            sqlQuery = "(select top " + pageSize + " * from \n (" + sqlQuery + " ) As T1 \n"
                    + " Order by " + sortOrderInvert;
            sqlQuery = "select * from \n " + sqlQuery + " ) As T2 \n"
                    + " Order by " + sortOrder;

            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sqlQuery);
                list = GetCompanyFromDataReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return list;
        }

        public int CountPropectCompanies()
        {   
            string sqlQuery = "select count(*) from tblSociete";
            sqlQuery += " where SocNom is not null and Statut=2";            

            return (int)SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, sqlQuery);
        }

        public List<Company> GetPropectCompanies(int pageSize, int pageNumber, string sortOrder, string sortOrderInvert)
        {
            List<Company> list = new List<Company>();

            string sqlQuery = SELECT_COMPANY_QUERY;
            sqlQuery += " where SocNom is not null and c.Statut=2";
            sqlQuery += " order by " + sortOrder;
            sqlQuery = string.Format(sqlQuery, "top " + pageSize * pageNumber + " ");
            sqlQuery = "(select top " + pageSize + " * from \n (" + sqlQuery + " ) As T1 \n"
                    + " Order by " + sortOrderInvert;
            sqlQuery = "select * from \n " + sqlQuery + " ) As T2 \n"
                    + " Order by " + sortOrder;

            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sqlQuery);
                list = GetCompanyFromDataReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return list;
        }

        public int CountSearchCompaniesByName(string name)
        {
            string sqlQuery = "select count(*) from tblSociete";
            sqlQuery += " where SocNom is not null and SocNom like @companyName";

            return (int)SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, 
                sqlQuery, new SqlParameter("@companyName", '%' + name + '%'));
        }

        public int CountSearchCompaniesByNameAndType(string name, string type)
        {
            string sqlQuery = "select count(*) from tblSociete";
            sqlQuery += " where SocNom is not null and SocNom like @companyName and Statut={0}";

            sqlQuery = string.Format(sqlQuery, type);
            return (int)SqlHelper.ExecuteScalar(_connectionString, CommandType.Text,
                sqlQuery, new SqlParameter("@companyName", '%' + name + '%'));
        }

        public List<Company> FindByName(string name)
        {
            List<Company> list = new List<Company>();

            string sqlQuery = SELECT_COMPANY_QUERY;
            sqlQuery = string.Format(sqlQuery, string.Empty);
            sqlQuery += " where SocNom is not null and SocNom like @companyName";
            sqlQuery += " order by SocNom ASC";

            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sqlQuery, new SqlParameter("@companyName", name + '%'));
                list = GetCompanyFromDataReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public List<Company> FindByNameWithStartCharacter(string name, int pageSize, int pageNumber, string sortOrder, string sortOrderInvert)
        {
            List<Company> list = new List<Company>();

            string sqlQuery = SELECT_COMPANY_QUERY;
            sqlQuery += " where SocNom is not null and SocNom like @companyName";
            sqlQuery += " order by " + sortOrder;
            sqlQuery = string.Format(sqlQuery, "top " + pageSize * pageNumber + " ");
            sqlQuery = "(select top " + pageSize + " * from \n (" + sqlQuery + " ) As T1 \n"
                    + " Order by " + sortOrderInvert;
            sqlQuery = "select * from \n " + sqlQuery + " ) As T2 \n"
                    + " Order by " + sortOrder;

            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sqlQuery, new SqlParameter("@companyName", name + '%'));
                list = GetCompanyFromDataReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public List<Company> FindByName(string name, 
            int pageSize, int pageNumber, string sortOrder, string sortOrderInvert)
        {
            List<Company> list = new List<Company>();

            string sqlQuery = SELECT_COMPANY_QUERY;
            sqlQuery += " where SocNom is not null and SocNom like @companyName";
            sqlQuery += " order by " + sortOrder;
            sqlQuery = string.Format(sqlQuery, "top " + pageSize * pageNumber + " ");
            sqlQuery = "(select top " + pageSize + " * from \n (" + sqlQuery + " ) As T1 \n"
                    + " Order by " + sortOrderInvert;
            sqlQuery = "select * from \n " + sqlQuery + " ) As T2 \n"
                    + " Order by " + sortOrder;

            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sqlQuery, new SqlParameter("@companyName", '%' + name + '%'));
                list = GetCompanyFromDataReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        public List<Company> FindByNameAndType(string name, string type,
            int pageSize, int pageNumber, string sortOrder, string sortOrderInvert)
        {
            List<Company> list = new List<Company>();

            string sqlQuery = SELECT_COMPANY_QUERY;
            sqlQuery += " where SocNom is not null and SocNom like @companyName";
            sqlQuery += " and c.Statut=" + type;
            sqlQuery += " order by " + sortOrder;
            sqlQuery = string.Format(sqlQuery, "top " + pageSize * pageNumber + " ");
            sqlQuery = "(select top " + pageSize + " * from \n (" + sqlQuery + " ) As T1 \n"
                    + " Order by " + sortOrderInvert;
            sqlQuery = "select * from \n " + sqlQuery + " ) As T2 \n"
                    + " Order by " + sortOrder;

            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sqlQuery, new SqlParameter("@companyName", '%' + name + '%'));
                list = GetCompanyFromDataReader(reader);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return list;
        }

        private List<Company> GetCompanyFromDataReader(SqlDataReader reader)
        {
            List<Company> result = new List<Company>();
            while (reader.Read())
            {
                Company com = new Company();
                com.CompanyID = (int)reader["SocieteID"];
                com.NVAT = reader["nTva"] as string;
                com.CompanyName = reader["SocNom"] as string;
                com.LegalForm = reader["FormeJuridique"] as string;
                com.Group = reader["Groupe"] as string;
                com.Address = reader["Adresse"] as string;
                com.ZipCode = reader["CodePostal"] as string;
                com.City = reader["Commune"] as string;
                com.TelephoneZone = reader["ZoneTel"] as string;
                com.Telephone = reader["Tel"] as string;
                com.Fax = reader["Fax"] as string;
                com.Email = reader["Email"] as string;
                com.Activity = reader["Activité"] as string;
                com.Segment = reader["Segment"] as string;
                com.SpecialSegment = reader["SegmentSpecialisation"] as string;
                com.Status = reader["Statut"] as int?;
                com.Responsible = reader["Responsable"] as string;
                com.ResponsibleLastName = reader["ResponsibleLastName"] != DBNull.Value ? reader["ResponsibleLastName"] as string : "";
                com.TariffConditions = reader["ConditionsTarif"] as string;
                com.Remark = reader["Remarques"] as string;
                com.CreatedDate = reader["DateCreation"] as DateTime?;
                com.Sector = reader["Secteur"] as string;
                com.UnitCode = reader["Unitcode"] as string;
                com.WebLink = reader["WebLink"] as string;
                com.SponsorArea = reader["ZoneSponsor"] as bool?;
                com.StatusLabel = reader["StatusLabel"] as string;

                result.Add(com);
            }

            return result;
        }
     

    }
}
