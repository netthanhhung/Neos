using System;
using System.Collections.Generic;
using System.Text;

namespace Neos.Data
{
    public class CandidateSearchCriteria
    {
        public CandidateSearchCriteria()
        {
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        private DateTime? ageFrom;

        public DateTime? AgeFrom
        {
            get { return ageFrom; }
            set { ageFrom = value; }
        }
        private DateTime? ageTo;

        public DateTime? AgeTo
        {
            get { return ageTo; }
            set { ageTo = value; }
        }
        private string active;

        public string Active
        {
            get { return active; }
            set { active = value; }
        }
        private string interviewer;

        public string Interviewer
        {
            get { return interviewer; }
            set { interviewer = value; }
        }
        private DateTime? dateInterviewerFrom;

        public DateTime? DateInterviewerFrom
        {
            get { return dateInterviewerFrom; }
            set { dateInterviewerFrom = value; }
        }
        private DateTime? dateInterviewerTo;

        public DateTime? DateInterviewerTo
        {
            get { return dateInterviewerTo; }
            set { dateInterviewerTo = value; }
        }
        private List<string> locations = new List<string>();

        public List<string> Locations
        {
            get { return locations; }
            set { locations = value; }
        }
        private string[] studyAndLevelIDs;

        public string[] StudyAndLevelIDs
        {
            get { return studyAndLevelIDs; }
            set { studyAndLevelIDs = value; }
        }

        private string[] studyAndLevelTexts;

        public string[] StudyAndLevelTexts
        {
            get { return studyAndLevelTexts; }
            set { studyAndLevelTexts = value; }
        }
        private bool studyHaveOne;

        public bool StudyHaveOne
        {
            get { return studyHaveOne; }
            set { studyHaveOne = value; }
        }
        private int frenchLevel;

        public int FrenchLevel
        {
            get { return frenchLevel; }
            set { frenchLevel = value; }
        }
        private int dutchLevel;

        public int DutchLevel
        {
            get { return dutchLevel; }
            set { dutchLevel = value; }
        }
        private int englishLevel;

        public int EnglishLevel
        {
            get { return englishLevel; }
            set { englishLevel = value; }
        }
        private int germanLevel;

        public int GermanLevel
        {
            get { return germanLevel; }
            set { germanLevel = value; }
        }
        private int spanishLevel;

        public int SpanishLevel
        {
            get { return spanishLevel; }
            set { spanishLevel = value; }
        }
        private int italianLevel;

        public int ItalianLevel
        {
            get { return italianLevel; }
            set { italianLevel = value; }
        }
        private string otherLang;

        public string OtherLang
        {
            get { return otherLang; }
            set { otherLang = value; }
        }
        private int otherLevel;

        public int OtherLevel
        {
            get { return otherLevel; }
            set { otherLevel = value; }
        }
        private int[] knowledgeIDs;

        public int[] KnowledgeIDs
        {
            get { return knowledgeIDs; }
            set { knowledgeIDs = value; }
        }

        private string[] knowledgeTexts;

        public string[] KnowledgeTexts
        {
            get { return knowledgeTexts; }
            set { knowledgeTexts = value; }
        }

        private bool knowledgeHaveOne;

        public bool KnowledgeHaveOne
        {
            get { return knowledgeHaveOne; }
            set { knowledgeHaveOne = value; }
        }
        private int[] functionIDs;

        public int[] FunctionIDs
        {
            get { return functionIDs; }
            set { functionIDs = value; }
        }

        private string[] functionTexts;

        public string[] FunctionTexts
        {
            get { return functionTexts; }
            set { functionTexts = value; }
        }

        private bool functionHaveOne;

        public bool FunctionHaveOne
        {
            get { return functionHaveOne; }
            set { functionHaveOne = value; }
        }

    }
}
