using System;
using System.Collections.Generic;
using System.Text;

namespace Neos.Data
{
    public class PresentationEmailObject
    {
        private List<string> mainEmails = new List<string>();

        public List<string> MainEmails
        {
            get { return mainEmails; }
            set { mainEmails = value; }
        }
        private List<string> ccEmails = new List<string>();

        public List<string> CcEmails
        {
            get { return ccEmails; }
            set { ccEmails = value; }
        }
        private string body;

        public string Body
        {
            get { return body; }
            set { body = value; }
        }
        private Dictionary<string, string> attachmentList = new Dictionary<string, string>();

        public Dictionary<string, string> AttachmentList
        {
            get { return attachmentList; }
            set { attachmentList = value; }
        }
        private int companyId;

        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        private int contactId;
        public int ContactId
        {
            get { return contactId; }
            set { contactId = value; }
        }

        private int candidateId;

        public int CandidateId
        {
            get { return candidateId; }
            set { candidateId = value; }
        }
        private bool autoCreateAction = false;

        public bool AutoCreateAction
        {
            get { return autoCreateAction; }
            set { autoCreateAction = value; }
        }
    }
}
