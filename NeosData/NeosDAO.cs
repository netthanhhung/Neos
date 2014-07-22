using System;
using System.Collections.Generic;
using System.Text;
using Neos.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace Neos.Data
{
    public static class NeosDAO
    {
        private static string _connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
        public static IList<Candidate> SearchCandidates(string lastName, string firstName)
        {
            IList<Candidate> result = new List<Candidate>();
            string sql = @"select CandidatID, Unit, Nom, Prenom, Adresse, CVMail, CodePostal, inactif, DateModif
                           from tblCandidat ";
            bool hasWhere = false;
            List<SqlParameter> paramList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(lastName))
            {
                sql += " where Nom like @lastName ";
                hasWhere = true;
                paramList.Add(new SqlParameter("@lastName", "%" + lastName + "%"));
            }
            if (!string.IsNullOrEmpty(firstName))
            {
                if (hasWhere)
                    sql += " and";
                else
                    sql += " where";
                sql += " Prenom like @fistName ";
                paramList.Add(new SqlParameter("@fistName", "%" + firstName + "%"));
            }
            sql += " order by Nom Asc ";
            //sql = string.Format(sql, lastName);
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sql, paramList.ToArray());
                result = GetCandidateFromReader(reader);
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

        public static int CountSearchCandidates(string lastName, string firstName)
        {
            string sql = @"select count(*) from tblCandidat ";
            bool hasWhere = false;
            List<SqlParameter> paramList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(lastName))
            {
                sql += " where Nom like @lastName ";
                hasWhere = true;
                paramList.Add(new SqlParameter("@lastName", "%" + lastName + "%"));
            }
            if (!string.IsNullOrEmpty(firstName))
            {
                if (hasWhere)
                    sql += " and";
                else
                    sql += " where";
                sql += " Prenom like @fistName ";
                paramList.Add(new SqlParameter("@fistName", "%" + firstName + "%"));
            }
            int result = (int) SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, sql, paramList.ToArray());
            return result;
        }

        public static IList<Candidate> SearchCandidatesByPage(string lastName, string firstName, 
            int pageSize, int pageNumber, string sortOrder, string sortOrderInvert)
        {
            IList<Candidate> result = new List<Candidate>();
            string sql = @"select {0} CandidatID, Unit, Nom, Prenom, Adresse, CVMail, CodePostal, inactif, DateModif
                           from tblCandidat ";
            bool hasWhere = false;
            List<SqlParameter> paramList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(lastName))
            {
                sql += " where Nom like @lastName ";
                hasWhere = true;
                paramList.Add(new SqlParameter("@lastName", "%" + lastName + "%"));
            }
            if (!string.IsNullOrEmpty(firstName))
            {
                if (hasWhere)
                    sql += " and";
                else
                    sql += " where";
                sql += " Prenom like @fistName ";
                paramList.Add(new SqlParameter("@fistName", "%" + firstName + "%"));
            }
            sql += " order by " + sortOrder;
            sql = string.Format(sql, "top " + pageSize * pageNumber + " ");
            sql = "(select top " + pageSize + " * from \n (" + sql + " ) As T1 \n"
                    + " Order by " + sortOrderInvert;
            sql = "select * from \n " + sql + " ) As T2 \n" 
                    + " Order by " + sortOrder;

            //sql = string.Format(sql, lastName);
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sql, paramList.ToArray());
                result = GetCandidateFromReader(reader);
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

        public static int CountAdvancedSearchCandidates(CandidateSearchCriteria criteria)
        {
            List<SqlParameter> paramList;
            string sql = BuildAdvancedQuery(criteria, out paramList);
            sql = "select count(*) from (" + sql + ") as T0 ";

            return (int) SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, sql, paramList.ToArray());
        }

        public static IList<Candidate> AdvancedSearchCandidatesByPage(CandidateSearchCriteria criteria, 
            int pageSize, int pageNumber, string sortOrder, string sortOrderInvert)
        {
            IList<Candidate> result = new List<Candidate>();
            List<SqlParameter> paramList;
            string sql = BuildAdvancedQuery(criteria, out paramList);
            sql = "select {0} * from (" + sql + ") as T0 ";
            sql += " order by " + sortOrder;
            sql = string.Format(sql, "top " + pageSize * pageNumber + " ");
            sql = "(select top " + pageSize + " * from \n (" + sql + " ) As T1 \n"
                    + " Order by " + sortOrderInvert;
            sql = "select * from \n " + sql + " ) As T2 \n"
                    + " Order by " + sortOrder;

            //sql = string.Format(sql, lastName);
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sql, paramList.ToArray());
                result = GetCandidateFromReader(reader);
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

        private static string BuildAdvancedQuery(CandidateSearchCriteria criteria, out List<SqlParameter> paramList)
        {
            string sql = @"select distinct Cand.CandidatID, Cand.Unit, Cand.Nom, Cand.Prenom, Cand.Adresse, Cand.CVMail, 
				            Cand.CodePostal, Cand.inactif, Cand.DateModif
                            from tblCandidat as Cand 
                            left outer join tblCandidatFonction as Func on Cand.candidatID=Func.CandidatID                                                       
                            left outer join tblCandidatAttentes as Att on Cand.CandidatID=Att.CandidatID 
                            left outer join tblCandidatConaiss as Con on Cand.candidatID=Con.CandidatID                                                         
                            left outer join tblCandidatformation as Form  on Cand.candidatID=Form.CandidatID                             
                            left outer join tblCandidatEval as CanEval on Cand.candidatID=CanEval.candidatID 
                            ";

            StringBuilder whereCond = new StringBuilder();
            whereCond.Append(" where Cand.CandidatID is not null ");
            paramList = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(criteria.LastName))
            {
                whereCond.Append(" and Cand.Nom like @lastName ");
                paramList.Add(new SqlParameter("@lastName", "%" + criteria.LastName + "%"));
            }
            if (!string.IsNullOrEmpty(criteria.FirstName))
            {
                whereCond.Append(" and Cand.Prenom like @firstName ");
                paramList.Add(new SqlParameter("@firstName", "%" + criteria.FirstName + "%"));
            }
            if (criteria.AgeFrom.HasValue)
            {
                whereCond.Append(" and Cand.DateNaissance >= @DateNaissanceFrom ");
                paramList.Add(new SqlParameter("@DateNaissanceFrom", criteria.AgeFrom.Value));
            }
            if (criteria.AgeTo.HasValue)
            {
                whereCond.Append(" and Cand.DateNaissance <= @DateNaissanceTo ");
                paramList.Add(new SqlParameter("@DateNaissanceTo", criteria.AgeTo.Value));
            }
            if (criteria.Active == "Yes")
                whereCond.Append(" and Cand.inactif = 0 ");
            else if (criteria.Active == "No")
                whereCond.Append(" and Cand.inactif = 1 ");
            if (!string.IsNullOrEmpty(criteria.Interviewer))
            {
                whereCond.Append(" and Cand.Interviewer = @Interviewer ");
                paramList.Add(new SqlParameter("@Interviewer", criteria.Interviewer));
            }
            if (criteria.DateInterviewerFrom.HasValue)
            {
                whereCond.Append(" and Cand.DateInterview >= @DateInterviewFrom ");
                paramList.Add(new SqlParameter("@DateInterviewFrom", criteria.DateInterviewerFrom.Value));
            }
            if (criteria.DateInterviewerTo.HasValue)
            {
                whereCond.Append(" and Cand.DateInterview <= @DateInterviewTo ");
                paramList.Add(new SqlParameter("@DateInterviewTo", criteria.DateInterviewerTo.Value));
            }

            //location
            string[] locations = criteria.Locations.ToArray();
            ParamLocationsRepository locRepo = new ParamLocationsRepository();
            if (locations != null && locations.Length > 0)
            {
                ParamLocations location0 = locRepo.FindOne(new ParamLocations(locations[0]));
                whereCond.Append(" and ((Att.region like @location0 or Att.region like @locationUk0 or Att.region like @locationNl0)");
                paramList.Add(new SqlParameter("@location0", "%" + location0.Location + "%"));
                paramList.Add(new SqlParameter("@locationUk0", "%" + location0.LocationUk + "%"));
                paramList.Add(new SqlParameter("@locationNl0", "%" + location0.LocationNL + "%"));

                for (int i = 1; i < locations.Length; i++)
                {
                    ParamLocations locationi = locRepo.FindOne(new ParamLocations(locations[i]));
                    whereCond.AppendFormat(" or (Att.region like @location{0} or Att.region like @locationUk{0} or Att.region like @locationNl{0})", i.ToString());
                    paramList.Add(new SqlParameter(string.Format("@location{0}", i), "%" + locationi.Location + "%"));
                    paramList.Add(new SqlParameter(string.Format("@locationUk{0}", i), "%" + locationi.LocationUk + "%"));
                    paramList.Add(new SqlParameter(string.Format("@locationNl{0}", i), "%" + locationi.LocationNL + "%"));
                }
                whereCond.Append(") ");
            }

            //study
            string[] studies = criteria.StudyAndLevelIDs;
            if (studies != null && studies.Length > 0)
            {
                string[] composeID = studies[0].Split(';');
                whereCond.Append(" and ((Form.FormationID = @FormationID0 and Form.NiveauEtudeID = @NiveauEtudeID0) ");
                paramList.Add(new SqlParameter("@FormationID0", int.Parse(composeID[0])));
                paramList.Add(new SqlParameter("@NiveauEtudeID0", int.Parse(composeID[1])));
                for (int i = 1; i < studies.Length; i++)
                {
                    string[] composeID2 = studies[i].Split(';');
                    if (criteria.StudyHaveOne)
                        whereCond.AppendFormat(" or (Form.FormationID = @FormationID{0} and Form.NiveauEtudeID = @NiveauEtudeID{0}) ", i.ToString());
                    else
                    {
                        whereCond.AppendFormat(@" and ((select count(*) from tblCandidatFormation t1
	                                                  where t1.FormationID = @FormationID{0} and t1.NiveauEtudeID = @NiveauEtudeID{0}
	                                                  and Cand.candidatID = t1.candidatID) = 1)", i.ToString());
                        //whereCond.AppendFormat(" and (Form.FormationID = @FormationID{0} and Form.NiveauEtudeID = @NiveauEtudeID{0}) ", i.ToString());
                    }
                    paramList.Add(new SqlParameter(string.Format("@FormationID{0}", i), int.Parse(composeID2[0])));
                    paramList.Add(new SqlParameter(string.Format("@NiveauEtudeID{0}", i), int.Parse(composeID2[1])));
                }
                whereCond.Append(") ");
            }

            //knowledge
            if (criteria.FrenchLevel > 0)
            {
                whereCond.Append(" and CanEval.Francais >= @Francais ");
                paramList.Add(new SqlParameter("@Francais", criteria.FrenchLevel));
            }
            if (criteria.DutchLevel > 0)
            {
                whereCond.Append(" and CanEval.Neerlandais >= @Neerlandais ");
                paramList.Add(new SqlParameter("@Neerlandais", criteria.DutchLevel));
            }
            if (criteria.EnglishLevel > 0)
            {
                whereCond.Append(" and CanEval.Anglais >= @Anglais ");
                paramList.Add(new SqlParameter("@Anglais", criteria.EnglishLevel));
            }
            if (criteria.GermanLevel > 0)
            {
                whereCond.Append(" and CanEval.Allemand >= @Allemand ");
                paramList.Add(new SqlParameter("@Allemand", criteria.GermanLevel));
            }
            if (criteria.SpanishLevel > 0)
            {
                whereCond.Append(" and CanEval.Espagnol >= @Espagnol ");
                paramList.Add(new SqlParameter("@Espagnol", criteria.SpanishLevel));
            }
            if (criteria.ItalianLevel > 0)
            {
                whereCond.Append(" and CanEval.Italien >= @Italien ");
                paramList.Add(new SqlParameter("@Italien", criteria.ItalianLevel));
            }
            if (!string.IsNullOrEmpty(criteria.OtherLang) && criteria.OtherLevel > 0)
            {
                whereCond.Append(" and CanEval.AutreLangue = @AutreLangue ");
                whereCond.Append(" and CanEval.ScoreAutreLangue >= @ScoreAutreLangue ");
                paramList.Add(new SqlParameter("@AutreLangue", criteria.OtherLang));
                paramList.Add(new SqlParameter("@ScoreAutreLangue", criteria.OtherLevel));
            }
            int[] knowledgeIDs = criteria.KnowledgeIDs;
            if (knowledgeIDs != null && knowledgeIDs.Length > 0)
            {
                whereCond.Append(" and (Con.ConnaissanceID = @ConnaissanceID0 ");
                paramList.Add(new SqlParameter("@ConnaissanceID0", knowledgeIDs[0]));
                for (int i = 1; i < knowledgeIDs.Length; i++)
                {
                    if (criteria.KnowledgeHaveOne)
                        whereCond.AppendFormat(" or Con.ConnaissanceID = @ConnaissanceID{0} ", i.ToString());
                    else
                    {
                        whereCond.AppendFormat(@" and ((select count(*) from tblCandidatConaiss t1
	                                                  where t1.ConnaissanceID = @ConnaissanceID{0}
	                                                  and Cand.candidatID = t1.candidatID) = 1)", i.ToString()); 
                        //whereCond.AppendFormat(" and Con.ConnaissanceID = @ConnaissanceID{0} ", i.ToString());
                    }
                    paramList.Add(new SqlParameter(string.Format("@ConnaissanceID{0}", i), knowledgeIDs[i]));
                }
                whereCond.Append(") ");
            }

            //Function
            int[] functionIDs = criteria.FunctionIDs;
            if (functionIDs != null && functionIDs.Length > 0)
            {
                whereCond.Append(" and (Func.FonctionID = @FonctionID0 ");
                paramList.Add(new SqlParameter("@FonctionID0", functionIDs[0]));
                for (int i = 1; i < functionIDs.Length; i++)
                {
                    if (criteria.FunctionHaveOne)
                        whereCond.AppendFormat(" or Func.FonctionID = @FonctionID{0} ", i.ToString());
                    else
                    {
                        whereCond.AppendFormat(@" and ((select count(*) from tblCandidatFonction t1
	                                                  where t1.FonctionID = @FonctionID{0}
	                                                  and Cand.candidatID = t1.candidatID) = 1)", i.ToString());
                        //whereCond.AppendFormat(" and Func.FonctionID = @FonctionID{0} ", i.ToString());
                    }
                    paramList.Add(new SqlParameter(string.Format("@FonctionID{0}", i), functionIDs[i]));
                }
                whereCond.Append(") ");
            }
            return sql + whereCond.ToString();
        }

        public static IList<Candidate> GetLastModifCandidates(int numberOfRecords)
        {
            IList<Candidate> result = new List<Candidate>();
            string sql = @"select top {0} CandidatID, Unit, Nom, Prenom, Adresse, CVMail, CodePostal, inactif, DateModif
                            from tblCandidat 
                            order by DateModif desc";
            sql = string.Format(sql, numberOfRecords);
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sql);
                result = GetCandidateFromReader(reader);
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

        public static Candidate GetCandidateById(int candidateId)
        {
            CandidateRepository repository = new CandidateRepository();
            Candidate item = new Candidate();
            item.CandidateId = candidateId;
            item = repository.FindOne(item);
            item.ContactInfo = string.Empty;
            if (!string.IsNullOrEmpty(item.Address))
            {
                item.ContactInfo += "Address : " + item.Address + "/n";
            }
            if (!string.IsNullOrEmpty(item.CVMail))
            {
                item.ContactInfo += "Mail : " + item.CVMail;
            }

            if (item.Inactive.HasValue) 
            {
                if(item.Inactive.Value)
                    item.InactiveString = "inactif";
                else
                    item.InactiveString = "actif";               
            }

            return item;
        }

        private static IList<Candidate> GetCandidateFromReader(SqlDataReader reader)
        {
            List<Candidate> result = new List<Candidate>();
            while (reader.Read())
            {
                Candidate item = new Candidate();
                item.CandidateId = (int)reader["CandidatID"];
                item.Unit = reader["Unit"] as string;
                item.LastName = reader["Nom"] as string;
                item.FirstName = reader["Prenom"] as string;
                item.Address = reader["Adresse"] as string;                
                item.ZipCode = reader["CVMail"] as string;

                if (!(reader["inactif"] is DBNull))
                {
                    item.Inactive = (bool?) reader["inactif"];
                }
                if (item.Inactive.HasValue)
                {
                    if (item.Inactive.Value)
                        item.InactiveString = "inactif";
                    else
                        item.InactiveString = "actif";
                }
                if (!(reader["DateModif"] is DBNull))
                {
                    item.LastModifDate = (DateTime)reader["DateModif"];                    
                }
                result.Add(item);
            }
            return result;
        }

        public static IList<StatisticsStudyLevel> GetStatisticsStudyLevels()
        {
            IList<StatisticsStudyLevel> result = new List<StatisticsStudyLevel>();
            string sql = @"select A.MaxStudyLevel, A.annee, A.Number, G.NiveauEtudeGenr from
                            (select S1.MaxStudyLevel, S1.annee, 
	                            count(*) as Number from
	                            (select C.candidatID, year(C.DateCreation) as annee,
		                            (
		                            select max (E.hierarchievaleur)
		                            from tblcandidatformation as F inner join tblParamNiveauEtude as E
		                            on F.NiveauEtudeID=E.NiveauEtudeID 
		                            where F.CandidatID=C.CandidatID
		                            ) as MaxStudyLevel 
	                            from tblcandidat as C
	                            ) as S1 
                             group by S1.MaxStudyLevel, S1.annee
                             ) A 
                            inner join tblParamNiveauEtudeGenr G on G.NiveauEtudeGenrID = A.MaxStudyLevel
                             order by MaxStudyLevel, annee";

            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(_connectionString, CommandType.Text, sql);
                int studyLevelID = -1;
                int index = -1;
                while (reader.Read())
                {

                    int newLevelID = (int)reader["MaxStudyLevel"];
                    if (newLevelID != studyLevelID)
                    {
                        StatisticsStudyLevel newItem = new StatisticsStudyLevel();
                        result.Add(newItem);
                        studyLevelID = newLevelID;
                        index++;
                    }
                    StatisticsStudyLevel item = result[index];
                    item.StudyLevelID = newLevelID;
                    item.StudyLevelString = reader["NiveauEtudeGenr"] as string;
                    int year = -1;
                    if (!(reader["annee"] is DBNull))
                        year = (int)reader["annee"];
                    int number = (int)reader["Number"];
                    item.YearNumberList.Add(new YearNumber(year, number));
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

        public static int GetNumberOfCandiatesInscription(DateTime dateFrom, DateTime? dateTo)
        {
            string sql = @"select count(*) from tblCandidat
                            where DateCreation >= @DateFrom ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@DateFrom", dateFrom));
            if (dateTo.HasValue)
            {
                sql += " and DateCreation <= @DateTo";
                paramList.Add(new SqlParameter("@DateTo", dateTo.Value));
            }
            return (int)SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, sql, paramList.ToArray());
        }

        public static int GetNumberOfCandiateByLocation(string location, string locationUk, string locationNl)
        {
            string sql = @"select count(*) from tblCandidatAttentes
	                       where (region like @Location
                           or region like @LocationUk or region like @LocationNl)";
            SqlParameter param = new SqlParameter("@Location", "%" + location + "%");
            SqlParameter paramUk = new SqlParameter("@LocationUk", "%" + locationUk + "%");
            SqlParameter paramNl = new SqlParameter("@LocationNl", "%" + locationNl + "%");            
            return (int)SqlHelper.ExecuteScalar(_connectionString, CommandType.Text, sql, param, paramUk, paramNl);
        }
    }
}
