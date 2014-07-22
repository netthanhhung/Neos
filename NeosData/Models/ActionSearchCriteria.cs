using System;
using System.Collections.Generic;
using System.Text;

namespace Neos.Data
{
    public class ActionSearchCriteria
    {
        public ActionSearchCriteria()
        {

        }
        
        private string active;
        public string Active
        {
            get { return active; }
            set { active = value; }
        }

        private DateTime? dateFrom;
        public DateTime? DateFrom
        {
            get { return dateFrom; }
            set { dateFrom = value; }
        }

        private DateTime? dateTo;
        public DateTime? DateTo
        {
            get { return dateTo; }
            set { dateTo = value; }
        }

        private string canName;
        public string CanName
        {
            get { return canName; }
            set { canName = value; }
        }

        private string comName;
        public string ComName
        {
            get { return comName; }
            set { comName = value; }
        }

        private int? typeActionID;
        public int? TypeActionID
        {
            get { return typeActionID; }
            set { typeActionID = value; }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private string responsible;
        public string Responsible
        {
            get { return responsible; }
            set { responsible = value; }
        }
    }
}
