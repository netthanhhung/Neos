using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    public class CompanyContactRepository : Repository<CompanyContact>
    {
        /*public static Comparison<Company> NameAscComparison = delegate(CompanyContact c1, CompanyContact c2)
        {
            return c1.CompanyName.CompareTo(c2.CompanyName);
        };*/
        public List<CompanyContact> GetContactOfCompany(int companyID)
        {
            Filter filter = Filter.Eq("SocieteID", companyID);
            List<CompanyContact> result = this.FindAll(filter) as List<CompanyContact>;
            if (result != null && result.Count > 0)
            {
                foreach (CompanyContact item in result)
                {
                    item.FullName = item.FirstName + " " + item.LastName;
                    /*if(!string.IsNullOrEmpty()) 
                        item.FullName += item.LastName;
                    if (!string.IsNullOrEmpty(item.FirstName))
                        item.FullName += ;*/

                }
            }
            return result;
        }
        public List<CompanyContact> SearchCompanyContact(string lastName, string firstName)
        {
            List<CompanyContact> result = new List<CompanyContact>();
            if (!string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(firstName))
            {
                Filter leftFilter = Filter.Like("Nom", lastName);
                Filter rightFilter = Filter.Like("Prenom", firstName);
                Filter filter = Filter.Or(leftFilter, rightFilter);
                result = this.FindAll(filter) as List<CompanyContact>;
            }
            else if (!string.IsNullOrEmpty(lastName))
            {
                Filter leftFilter = Filter.Like("Nom", lastName);
                result = this.FindAll(leftFilter) as List<CompanyContact>;
            }
            else if (!string.IsNullOrEmpty(firstName))
            {
                Filter rightFilter = Filter.Like("Prenom", firstName);
                result = this.FindAll(rightFilter) as List<CompanyContact>;
            }
            foreach (CompanyContact item in result)
            {
                item.FullName = item.FirstName + " " + item.LastName;
            }
            return result;
        }
    }
}
