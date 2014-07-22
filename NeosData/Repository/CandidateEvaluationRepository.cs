using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace Neos.Data
{
    public class CandidateEvaluationRepository : Repository<CandidateEvaluation>
    {
        public CandidateEvaluation GetCandidateEvaluation(int candidateId)
        {
            CandidateEvaluation result = null;
            Filter filter = Filter.Eq("CandidateID", candidateId);
            IList<CandidateEvaluation> list = this.FindAll(filter);
            if (list.Count == 1)
            {
                foreach (CandidateEvaluation item in list)
                {
                    result = item;
                    break;
                }
            }
            return result;     
        }

        public void InsertNewEvaluation(CandidateEvaluation evaluation)
        {
            string connectionString = FrameworkLite.Configuration.Configuration.Instance.ConnectionString;
            string sql = @"insert tblCandidatEval (CandidatID, Neerlandais, Francais, 
                                Anglais, Allemand, Italien, Espagnol, AutreLangue, ScoreAutreLangue,
                                EvaluationPrensentation, EvaluationVerbal, EvaluationCaractere, EvaluationMotivation, 
                                EvaluationAutonomie, EvaluationDiverse, EvaluationGlobale) 
                           values (@CandidatID, @Neerlandais, @Francais, 
                                @Anglais, @Allemand, @Italien, @Espagnol, @AutreLangue, @ScoreAutreLangue,
                                @EvaluationPrensentation, @EvaluationVerbal, @EvaluationCaractere, @EvaluationMotivation, 
                                @EvaluationAutonomie, @EvaluationDiverse, @EvaluationGlobale);";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@CandidatID", evaluation.CandidateID));
            paramList.Add(new SqlParameter("@Neerlandais", evaluation.Dutch));
            paramList.Add(new SqlParameter("@Francais", evaluation.French));
            paramList.Add(new SqlParameter("@Anglais", evaluation.English));
            paramList.Add(new SqlParameter("@Allemand", evaluation.German));
            paramList.Add(new SqlParameter("@Italien", evaluation.Italian));
            paramList.Add(new SqlParameter("@Espagnol", evaluation.Spanish));
            paramList.Add(new SqlParameter("@AutreLangue", evaluation.AdditionLanguage));
            paramList.Add(new SqlParameter("@ScoreAutreLangue", evaluation.AdditionLanguageScore));
            paramList.Add(new SqlParameter("@EvaluationPrensentation", evaluation.PresentationEvaluation));
            paramList.Add(new SqlParameter("@EvaluationVerbal", evaluation.ExpressionEvaluation));
            paramList.Add(new SqlParameter("@EvaluationCaractere", evaluation.PersonalityEvaluation));
            paramList.Add(new SqlParameter("@EvaluationMotivation", evaluation.MotivationEvaluation));
            paramList.Add(new SqlParameter("@EvaluationAutonomie", evaluation.SelfEvaluation));
            paramList.Add(new SqlParameter("@EvaluationDiverse", evaluation.VariousMatters));
            paramList.Add(new SqlParameter("@EvaluationGlobale", evaluation.GlobalEvaluation));
            SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, paramList.ToArray());   
        }
    }
}
