using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Interfaces.Data
{
    public interface ICompanyChargeScheduleRepository : IGenericRepository<DbRepository.Models.SqlDatabaseContext.CompanyChargeSchedule>
    {
        IList<Models.CompanyChargeSchedule> Search(Models.CompanyChargeSchedule search);
    }
}
