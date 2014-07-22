using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    public class CompanyDocumentRepository : Repository<CompanyDocument>
    {
        public static Comparison<CompanyDocument> IdComparisonAsc = delegate(CompanyDocument d1, CompanyDocument d2)
        {
            return d1.DocumentID.CompareTo(d2.DocumentID);
        };

        public List<CompanyDocument> GetDocumentsOfCompany(int companyID)
        {
            Filter filter = Filter.Eq("SocieteID", companyID);
            List<CompanyDocument> result = this.FindAll(filter) as List<CompanyDocument>;
            result.Sort(IdComparisonAsc);
            return result;
        }

        public int GetCompanyIDByDocument(string docName)
        {
            Filter filter = Filter.Eq("NomDoc", docName);
            List<CompanyDocument> result = this.FindAll(filter) as List<CompanyDocument>;

            if (result.Count > 0)
            {
                return result[0].CompanyID;
            }

            return -1;
        }
    }
}
