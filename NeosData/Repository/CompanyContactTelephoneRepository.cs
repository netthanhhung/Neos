using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    public class CompanyContactTelephoneRepository : Repository<CompanyContactTelephone>
    {
        /*public static Comparison<Company> NameAscComparison = delegate(CompanyContact c1, CompanyContact c2)
        {
            return c1.CompanyName.CompareTo(c2.CompanyName);
        };*/

        public List<CompanyContactTelephone> GetContactInfo(int contactID)
        {
            Filter filter = Filter.Eq("ContactID", contactID);
            List<CompanyContactTelephone> result = this.FindAll(filter) as List<CompanyContactTelephone>;

            return result;
        }
    }
}
