using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Interfaces.Data
{
    public interface ICompanyInspectionTypeChargeRateRepository : IGenericRepository<DbRepository.Models.SqlDatabaseContext.CompanyInspectionTypeChargeRate>
    {
        IList<Models.CompanyInspectionTypeChargeRate> Search(Models.CompanyInspectionTypeChargeRate search);
    }
}
