using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblCandidatEval")]
    public class CandidateEvaluation : IEntity
    {
        private int candidateID;
        [EntityProperty(ColumnName = "CandidatID", IsIdentity = true, IsPrimaryKey = true)]
        public int CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
        }
        private short? dutch;
        [EntityProperty(ColumnName = "Neerlandais")]
        public short? Dutch
        {
            get { return dutch; }
            set { dutch = value; }
        }
        private short? french;
        [EntityProperty(ColumnName = "Francais")]
        public short? French
        {
            get { return french; }
            set { french = value; }
        }
        private short? english;
        [EntityProperty(ColumnName = "Anglais")]
        public short? English
        {
            get { return english; }
            set { english = value; }
        }
        private short? german;
        [EntityProperty(ColumnName = "Allemand")]
        public short? German
        {
            get { return german; }
            set { german = value; }
        }
        private short? italian;
        [EntityProperty(ColumnName = "Italien")]
        public short? Italian
        {
            get { return italian; }
            set { italian = value; }
        }
        private short? spanish;
        [EntityProperty(ColumnName = "Espagnol")]
        public short? Spanish
        {
            get { return spanish; }
            set { spanish = value; }
        }
        private string additionLanguageSkill;
        [EntityProperty(ColumnName = "AutreLangue")]        
        public string AdditionLanguage
        {
            get { return additionLanguageSkill; }
            set { additionLanguageSkill = value; }
        }
        private short? additionLanguageScore;
        [EntityProperty(ColumnName = "ScoreAutreLangue")]
        public short? AdditionLanguageScore
        {
            get { return additionLanguageScore; }
            set { additionLanguageScore = value; }
        }
        
        private string presentationEvaluation;
        [EntityProperty(ColumnName = "EvaluationPrensentation")]
        public string PresentationEvaluation
        {
            get { return presentationEvaluation; }
            set { presentationEvaluation = value; }
        }
        private string expressionEvaluation;
        [EntityProperty(ColumnName = "EvaluationVerbal")]
        public string ExpressionEvaluation
        {
            get { return expressionEvaluation; }
            set { expressionEvaluation = value; }
        }
        private string personalityEvaluation;
        [EntityProperty(ColumnName = "EvaluationCaractere")]
        public string PersonalityEvaluation
        {
            get { return personalityEvaluation; }
            set { personalityEvaluation = value; }
        }
        private string motivationEvaluation;
        [EntityProperty(ColumnName = "EvaluationMotivation")]
        public string MotivationEvaluation
        {
            get { return motivationEvaluation; }
            set { motivationEvaluation = value; }
        }
        private string selfEvaluation;
        [EntityProperty(ColumnName = "EvaluationAutonomie")]
        public string SelfEvaluation
        {
            get { return selfEvaluation; }
            set { selfEvaluation = value; }
        }
        private string variousMatters;
        [EntityProperty(ColumnName = "EvaluationDiverse")]        
        public string VariousMatters
        {
            get { return variousMatters; }
            set { variousMatters = value; }
        }
        //Global Evaluation
        private string globalEvaluation;
        [EntityProperty(ColumnName = "EvaluationGlobale")]
        public string GlobalEvaluation
        {
            get { return globalEvaluation; }
            set { globalEvaluation = value; }
        }
    }
}
