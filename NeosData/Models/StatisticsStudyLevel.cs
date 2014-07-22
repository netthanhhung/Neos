using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{    
    public class StatisticsStudyLevel
    {
        private int studyLevelID;
        private string studyLevelString;

        public StatisticsStudyLevel() { }
        public StatisticsStudyLevel(int studyLevelID)
        {
            this.studyLevelID = studyLevelID;            
        }
        public StatisticsStudyLevel(int studyLevelID, string studyLevelString)
        {
            this.studyLevelID = studyLevelID;
            this.studyLevelString = studyLevelString;
        }
               
        public int StudyLevelID
        {
            get { return studyLevelID; }
            set { studyLevelID = value; }
        }
               
        public string StudyLevelString
        {
            get { return studyLevelString; }
            set { studyLevelString = value; }
        }

        IList<YearNumber> yearNumberList = new List<YearNumber>();

        public IList<YearNumber> YearNumberList
        {
            get { return yearNumberList; }
            set { yearNumberList = value; }
        }
    }

    public class YearNumber
    {
        private int year;
        private int number;

        public YearNumber() { }

        public YearNumber(int year1, int number1) 
        {
            year = year1;
            number = number1;
        }
        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

    }
}
