using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace Neos.Data
{
    public class JobRepository : Repository<Job>
    {
        private static string _connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;

        public static Comparison<Job> TitleAscComparison = delegate(Job j1, Job j2)
        {
            return j1.Title.CompareTo(j2.Title);
        };

        public static Comparison<Job> TitleEN_AscComparison = delegate(Job j1, Job j2)
        {
            return j1.Title_EN.CompareTo(j2.Title_EN);
        };
        public static Comparison<Job> TitleNL_AscComparison = delegate(Job j1, Job j2)
        {
            return j1.Title_NL.CompareTo(j2.Title_NL);
        };

        
        
        public List<Job> GetJobsOfCompany(int companyID)
        {
            Filter filter = Filter.Eq("SocieteID", companyID);
            List<Job> list = this.FindAll(filter) as List<Job>;
            return list;
        }

        public List<Job> GetTopJobs(int numberOfRecords)
        {
            List<Job> list = new List<Job>();
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            StringBuilder query = new StringBuilder();
            query.AppendFormat(@"select  top ({0})  j.Ref, j.JobActive, j.Date, j.Title, j.Company_Description, j.Job_Description, 
                                    j.Personal_Description, j.Location, j.CM, j.CMTitre, j.CMNom, j.CMTel,
                                    j.email, j.CMDepartement, j.StatClickSite, j.DatStatClick, j.FonctionFam,
                                    j.Package_Description, j.confidentiel, j.ExpirationDate, j.DateModif, j.URL,
                                    j.ActivationDate, j.DateRemind, j.visites, j.Title_NL, j.Company_Description_NL,.
                                    j.Job_Description_NL, j.Personal_Description_NL, j.Package_Description_NL,
                                    j.Title_EN, j.Company_Description_EN, j.Job_Description_EN, j.Personal_Description_EN,
                                    j.Package_Description_EN, j.Applications, j.Title_Track, j.SocieteID, c.SocNom,
                                    j.Profile, p.Profile as [ProfileName], p.ProfileCode", numberOfRecords);
            query.Append(" from tblJobs j left join tblSociete c on j.SocieteID = c.SocieteID left join tblParamProfiles p on j.Profile = p.ProfileId");
            query.Append(" order by j.Date desc ");
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, query.ToString());
                list = GetJobsFromReader(reader);
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

        public List<Job> GetAllJobs()
        {
            List<Job> list = new List<Job>();
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            StringBuilder query = new StringBuilder();
            query.Append(@"select   j.Ref, j.JobActive, j.Date, j.Title, j.Company_Description, j.Job_Description, 
                                    j.Personal_Description, j.Location, j.CM, j.CMTitre, j.CMNom, j.CMTel,
                                    j.email, j.CMDepartement, j.StatClickSite, j.DatStatClick, j.FonctionFam,
                                    j.Package_Description, j.confidentiel, j.ExpirationDate, j.DateModif, j.URL,
                                    j.ActivationDate, j.DateRemind, j.visites, j.Title_NL, j.Company_Description_NL,.
                                    j.Job_Description_NL, j.Personal_Description_NL, j.Package_Description_NL,
                                    j.Title_EN, j.Company_Description_EN, j.Job_Description_EN, j.Personal_Description_EN,
                                    j.Package_Description_EN, j.Applications, j.Title_Track, j.SocieteID, c.SocNom,
                                    j.Profile, p.Profile as [ProfileName], p.ProfileCode");
            query.Append(" from tblJobs j left join tblSociete c on j.SocieteID = c.SocieteID left join tblParamProfiles p on j.Profile = p.ProfileId");
            
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, query.ToString());
                list = GetJobsFromReader(reader);
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

        public List<Job> GetActiveJobs()
        {
            List<Job> list = new List<Job>();
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            StringBuilder query = new StringBuilder();
            query.Append(@"select   j.Ref, j.JobActive, j.Date, j.Title, j.Company_Description, j.Job_Description, 
                                    j.Personal_Description, j.Location, j.CM, j.CMTitre, j.CMNom, j.CMTel,
                                    j.email, j.CMDepartement, j.StatClickSite, j.DatStatClick, j.FonctionFam,
                                    j.Package_Description, j.confidentiel, j.ExpirationDate, j.DateModif, j.URL,
                                    j.ActivationDate, j.DateRemind, j.visites, j.Title_NL, j.Company_Description_NL,.
                                    j.Job_Description_NL, j.Personal_Description_NL, j.Package_Description_NL,
                                    j.Title_EN, j.Company_Description_EN, j.Job_Description_EN, j.Personal_Description_EN,
                                    j.Package_Description_EN, j.Applications, j.Title_Track, j.SocieteID, c.SocNom,
                                    j.Profile, p.Profile as [ProfileName], p.ProfileCode");
            query.Append(" from tblJobs j left join tblSociete c on j.SocieteID = c.SocieteID left join tblParamProfiles p on j.Profile = p.ProfileId");
            query.Append(" where j.JobActive = 'true' ");
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, query.ToString());
                list = GetJobsFromReader(reader);
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

        public List<Job> GetInActiveJobs()
        {
            List<Job> list = new List<Job>();
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            StringBuilder query = new StringBuilder();
            query.Append(@"select   j.Ref, j.JobActive, j.Date, j.Title, j.Company_Description, j.Job_Description, 
                                    j.Personal_Description, j.Location, j.CM, j.CMTitre, j.CMNom, j.CMTel,
                                    j.email, j.CMDepartement, j.StatClickSite, j.DatStatClick, j.FonctionFam,
                                    j.Package_Description, j.confidentiel, j.ExpirationDate, j.DateModif, j.URL,
                                    j.ActivationDate, j.DateRemind, j.visites, j.Title_NL, j.Company_Description_NL,.
                                    j.Job_Description_NL, j.Personal_Description_NL, j.Package_Description_NL,
                                    j.Title_EN, j.Company_Description_EN, j.Job_Description_EN, j.Personal_Description_EN,
                                    j.Package_Description_EN, j.Applications, j.Title_Track, j.SocieteID, c.SocNom,
                                    j.Profile, p.Profile as [ProfileName], p.ProfileCode");
            query.Append(" from tblJobs j left join tblSociete c on j.SocieteID = c.SocieteID left join tblParamProfiles p on j.Profile = p.ProfileId");
            query.Append(" where j.JobActive = 'false' ");
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, query.ToString());
                list = GetJobsFromReader(reader);
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

        /// <summary>
        /// search Jobs base on multi-criteria
        /// </summary>
        /// <param name="args">tilte, createdMin, createdMax, activatedMin, activatedMax, expiredMin, expiredMax, profile, function, location, responsible, company, isActive</param>
        /// <returns></returns>
        public List<Job> SearchJobs(JobSearchCriteria criteria)
        {
            List<Job> list = new List<Job>();
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            StringBuilder query = new StringBuilder();
            query.Append(@"select   j.Ref, j.JobActive, j.Date, j.Title, j.Company_Description, j.Job_Description, 
                                    j.Personal_Description, j.Location, j.CM, j.CMTitre, j.CMNom, j.CMTel,
                                    j.email, j.CMDepartement, j.StatClickSite, j.DatStatClick, j.FonctionFam,
                                    j.Package_Description, j.confidentiel, j.ExpirationDate, j.DateModif, j.URL,
                                    j.ActivationDate, j.DateRemind, j.visites, j.Title_NL, j.Company_Description_NL,.
                                    j.Job_Description_NL, j.Personal_Description_NL, j.Package_Description_NL,
                                    j.Title_EN, j.Company_Description_EN, j.Job_Description_EN, j.Personal_Description_EN,
                                    j.Package_Description_EN, j.Applications, j.Title_Track, j.SocieteID, c.SocNom,
                                    j.Profile, p.Profile as [ProfileName], p.ProfileCode");
            query.Append(" from tblJobs j left join tblSociete c on j.SocieteID = c.SocieteID left join tblParamProfiles p on j.Profile = p.ProfileId");
            query.Append(" where j.Ref is not null ");

            if (!string.IsNullOrEmpty(criteria.Title))
            {
                query.Append(" and ((j.Title like  @title) or (j.Title_EN like  @title) or (j.Title_NL like  @title)) ");
                sqlParams.Add(new SqlParameter("@title", '%' + criteria.Title + '%'));
            }

            if (!string.IsNullOrEmpty(criteria.Active))
            {
                if (criteria.Active == "Yes")
                {
                    query.Append(" and j.JobActive = 1");
                }
                else if (criteria.Active == "No")
                {
                    query.Append(" and j.JobActive = 0");
                }
            }

            if (criteria.CreatedDateFrom.HasValue)//created date from
            {
                query.AppendFormat(" and j.Date >= Convert(datetime,'{0} 00:00:00',103) ", criteria.CreatedDateFrom.Value.ToString("dd/MM/yyyy"));
                //sqlParams.Add(new SqlParameter("@createdMin", criteria.CreatedDateFrom.Value.ToString("dd/MM/yyyy")));                
            }
             if (criteria.CreatedDateTo.HasValue)//created date to
            {
                query.AppendFormat(" and j.Date <= Convert(datetime,'{0} 23:59:59',103)", criteria.CreatedDateTo.Value.ToString("dd/MM/yyyy"));
                //sqlParams.Add(new SqlParameter("@createdMax", criteria.CreatedDateTo.Value.ToString("dd/MM/yyyy")));                
            }

            if (criteria.ActivatedDateFrom.HasValue)
            {
                query.AppendFormat(" and j.ActivationDate >= Convert(datetime,'{0} 00:00:00',103) ", criteria.ActivatedDateFrom.Value.ToString("dd/MM/yyyy"));
                //sqlParams.Add(new SqlParameter("@activatedMin", criteria.ActivatedDateFrom.Value));                
            }
            if (criteria.ActivatedDateTo.HasValue)
            {
                query.AppendFormat(" and j.ActivationDate <= Convert(datetime,'{0} 23:59:59',103) ", criteria.ActivatedDateTo.Value.ToString("dd/MM/yyyy"));
                //sqlParams.Add(new SqlParameter("@activatedMax", criteria.ActivatedDateTo.Value));                
            }

             if (criteria.ExpiredDateFrom.HasValue)
            {
                query.AppendFormat(" and j.ExpirationDate >= Convert(datetime,'{0} 00:00:00',103) ", criteria.ExpiredDateFrom.Value.ToString("dd/MM/yyyy"));
                //sqlParams.Add(new SqlParameter("@expiredMin", criteria.ExpiredDateFrom.Value));                
            }
            if (criteria.ExpiredDateTo.HasValue)
            {
                query.AppendFormat(" and j.ExpirationDate <= Convert(datetime,'{0} 23:59:59',103) ", criteria.ExpiredDateTo.Value.ToString("dd/MM/yyyy"));
                //sqlParams.Add(new SqlParameter("@expiredMax", criteria.ExpiredDateTo.Value));                
            }            
           
            if (criteria.ProfileID.HasValue) //profile
            {
                query.Append(" and j.Profile =  @profile ");
                sqlParams.Add(new SqlParameter("@profile", criteria.ProfileID.Value));
            }
            if (!string.IsNullOrEmpty(criteria.FunctionFam)) //function
            {
                query.Append(" and j.FonctionFam =  @functionFam ");
                sqlParams.Add(new SqlParameter("@functionFam", criteria.FunctionFam));
            }
            if (criteria.Locations != null && criteria.Locations.Length > 0) //location
            {               
                query.Append(" and (j.Location like  @location0 ");
                sqlParams.Add(new SqlParameter("@location0", '%' + criteria.Locations[0] + '%'));

                for (int i = 1; i < criteria.Locations.Length; i++)
                {                   
                    query.AppendFormat(" or j.Location like  @location{0} ", i.ToString());
                    sqlParams.Add(new SqlParameter(string.Format("@location{0}", i), '%' + criteria.Locations[i] + '%'));                   
                }
                query.Append(") ");
                
            }
            if (!string.IsNullOrEmpty(criteria.Responsible)) //career manager (CM)
            {
                query.Append(" and j.CM = @responsible ");
                sqlParams.Add(new SqlParameter("@responsible", criteria.Responsible));
            }
            if (!string.IsNullOrEmpty(criteria.ComName)) //companyName
            {
                query.Append(" and c.SocNom like  @companyName ");
                sqlParams.Add(new SqlParameter("@companyName", '%' + criteria.ComName + '%'));
            }            

            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, query.ToString(), sqlParams.ToArray());


                list = GetJobsFromReader(reader);
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

        private List<Job> GetJobsFromReader(SqlDataReader reader)
        {
            List<Job> result = new List<Job>();
            while (reader.Read())
            {
                Job job = new Job();
                job.JobID = Int32.Parse(reader["Ref"].ToString());
                job.IsActive = (bool)reader["JobActive"];
                job.CreatedDate = reader["Date"] as DateTime?;
                job.Title = reader["Title"] as string;
                job.CompanyDescription = reader["Company_Description"] as string;
                job.JobDescription = reader["Job_Description"] as string;
                job.PersonalDescription = reader["Personal_Description"] as string;
                job.ProfileID = reader["Profile"] as int?;
                job.Location = reader["Location"] as string;
                job.CareerManager = reader["CM"] as string;
                job.CareerManagerTitle = reader["CMTitre"] as string;
                job.CareerManagerLastName = reader["CMNom"] as string;
                job.CareerManagerTelephone = reader["CMTel"] as string;
                job.CareerManagerEmail = reader["email"] as string;
                job.CareerManagerDepart = reader["CMDepartement"] as string;
                job.CompanyID = reader["SocieteID"] as int?;
                job.FamilyFunctionID = reader["FonctionFam"] as string;
                job.PackageDescription = reader["Package_Description"] as string;
                job.IsConfidential = reader["confidentiel"] as bool?;
                job.ExpiredDate = reader["ExpirationDate"] as DateTime?;
                job.LastModifiedDate = reader["DateModif"] as DateTime?;
                job.URL = reader["URL"] as string;
                job.ActivatedDate = reader["ActivationDate"] as DateTime?;
                job.RemindDate = reader["DateRemind"] as DateTime?;
                job.NrOfVisites = reader["visites"] as int?;
                
                job.Title_NL = reader["Title_NL"] as string;
                job.CompanyDescription_NL = reader["Company_Description_NL"] as string;
                job.JobDescription_NL = reader["Job_Description_NL"] as string;
                job.PersonalDescription_NL = reader["Personal_Description_NL"] as string;
                job.PackageDescription_NL = reader["Package_Description_NL"] as string;

                job.Title_EN = reader["Title_EN"] as string;
                job.CompanyDescription_EN = reader["Company_Description_EN"] as string;
                job.JobDescription_EN = reader["Job_Description_EN"] as string;
                job.PersonalDescription_EN = reader["Personal_Description_EN"] as string;
                job.PackageDescription_EN = reader["Package_Description_EN"] as string;

                job.NrOfApplications = reader["Applications"] as int?;
                job.TitleTrack = reader["Title_Track"] as string;

                job.CompanyName = reader["SocNom"] as string;
                job.ProfileName = reader["ProfileName"] as string;
                job.ProfileCode = reader["ProfileCode"] as string;

                result.Add(job);

            }
            return result;
        }
    }
}
