using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

namespace Neos.Data
{
    public class CompanyAddressRepository : Repository<CompanyAddress>
    {
        public IList<CompanyAddress> GetAddressesOfCompany(int companyID)
        {
            Filter filter = Filter.Eq("SocieteID", companyID);
            List<CompanyAddress> result = this.FindAll(filter) as List<CompanyAddress>;
            return result;
        }
    }
}
