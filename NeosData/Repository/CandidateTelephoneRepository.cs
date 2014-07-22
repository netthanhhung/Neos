using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    public class CandidateTelephoneRepository : Repository<CandidateTelephone>
    {
        public static Comparison<CandidateTelephone> TypeDescComparison = delegate(CandidateTelephone c1, CandidateTelephone c2)
        {
            if (string.IsNullOrEmpty(c1.Type))
                return 1;
            else if (string.IsNullOrEmpty(c2.Type))
                return -1;
            else 
                return c2.Type.CompareTo(c1.Type);
        };
        public IList<CandidateTelephone> GetCandidateTelephonesByCandidateID(int candidateID)
        {
            Filter filter = Filter.Eq("CandidateID", candidateID);            
            List<CandidateTelephone> result = this.FindAll(filter) as List<CandidateTelephone>;
            result.Sort(TypeDescComparison);
            return result;
        }
    }
}
