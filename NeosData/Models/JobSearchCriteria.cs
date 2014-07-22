using System;
using System.Collections.Generic;
using System.Text;

namespace Neos.Data
{
    public class JobSearchCriteria
    {
        public JobSearchCriteria()
        {

        }

        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string active;
        public string Active
        {
            get { return active; }
            set { active = value; }
        }

        private DateTime? createdDateFrom;
        public DateTime? CreatedDateFrom
        {
            get { return createdDateFrom; }
            set { createdDateFrom = value; }
        }

        private DateTime? createdDateTo;
        public DateTime? CreatedDateTo
        {
            get { return createdDateTo; }
            set { createdDateTo = value; }
        }

        private DateTime? activatedDateFrom;
        public DateTime? ActivatedDateFrom
        {
            get { return activatedDateFrom; }
            set { activatedDateFrom = value; }
        }

        private DateTime? activatedDateTo;
        public DateTime? ActivatedDateTo
        {
            get { return activatedDateTo; }
            set { activatedDateTo = value; }
        }

        private DateTime? expiredDateFrom;
        public DateTime? ExpiredDateFrom
        {
            get { return expiredDateFrom; }
            set { expiredDateFrom = value; }
        }

        private DateTime? expiredDateTo;
        public DateTime? ExpiredDateTo
        {
            get { return expiredDateTo; }
            set { expiredDateTo = value; }
        }

        private int? profileID;
        public int? ProfileID
        {
            get { return profileID; }
            set { profileID = value; }
        }

        private string functionFam;
        public string FunctionFam
        {
            get { return functionFam; }
            set { functionFam = value; }
        }

        private string[] locations;
        public string[] Locations
        {
            get { return locations; }
            set { locations = value; }
        }

        private string responsible;
        public string Responsible
        {
            get { return responsible; }
            set { responsible = value; }
        }

        private string comName;
        public string ComName
        {
            get { return comName; }
            set { comName = value; }
        }
    }
}
