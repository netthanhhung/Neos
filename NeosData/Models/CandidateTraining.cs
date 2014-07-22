using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    [EntityClass(DbTableName = "tblCandidatFormation")]
    public class CandidateTraining : IEntity
    {
        private int candidateFormationID;
        [EntityProperty(ColumnName = "CandidatFormationID", IsIdentity = true, IsPrimaryKey = true)]
        public int CandidateFormationID
        {
            get { return candidateFormationID; }
            set { candidateFormationID = value; }
        }
        private int candidateID;
        [EntityProperty(ColumnName = "CandidatID")]
        public int CandidateID
        {
            get { return candidateID; }
            set { candidateID = value; }
        }
        private string period;
        [EntityProperty(ColumnName = "Periode")]
        public string Period
        {
            get { return period; }
            set { period = value; }
        }
        private int? formationID;
        [EntityProperty(ColumnName = "FormationID")]
        public int? FormationID
        {
            get { return formationID; }
            set { formationID = value; }
        }
        private string formationString;
        [EntityProperty(IsPersistent = false)]
        public string FormationString
        {
            get { return formationString; }
            set { formationString = value; }
        }
        private string school;
        [EntityProperty(ColumnName = "Ecole")]
        public string School
        {
            get { return school; }
            set { school = value; }
        }
        private int? studyLevelID;
        [EntityProperty(ColumnName = "NiveauEtudeID")]
        public int? StudyLevelID
        {
            get { return studyLevelID; }
            set { studyLevelID = value; }
        }
        private string studyLevelString;
        [EntityProperty(IsPersistent = false)]
        public string StudyLevelString
        {
            get { return studyLevelString; }
            set { studyLevelString = value; }
        }
        private string diplome;
        [EntityProperty(ColumnName = "diplome")]
        public string Diplome
        {
            get { return diplome; }
            set { diplome = value; }
        }

        #region methods
        public CandidateTraining()
        {
        }
        public CandidateTraining(int id)
        {
            this.CandidateFormationID = id;
        }
        #endregion
    }
}
