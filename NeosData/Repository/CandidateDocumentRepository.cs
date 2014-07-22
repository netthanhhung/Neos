using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    public class CandidateDocumentRepository : Repository<CandidateDocument>
    {
        public static Comparison<CandidateDocument> IdComparisonAsc = delegate(CandidateDocument d1, CandidateDocument d2)
        {
            return d1.DocumentID.CompareTo(d2.DocumentID);
        };

        public List<CandidateDocument> GetDocumentsOfCandidate(int candidateID)
        {
            Filter filter = Filter.Eq("CandidatID", candidateID);
            List<CandidateDocument> result = this.FindAll(filter) as List<CandidateDocument>;
            result.Sort(IdComparisonAsc);
            return result;
        }

        public int GetCandidateIDByDocument(string docName)
        {
            Filter filter = Filter.Eq("NomDoc", docName);
            List<CandidateDocument> result = this.FindAll(filter) as List<CandidateDocument>;

            if (result.Count > 0)
            {
                return result[0].CandidateID;
            }

            return -1;
        }
    }
}
