using Evolution.GenericDbRepository.Interfaces;
using System;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Timesheet.Domain.Interfaces.Data
{
    public interface ITimesheetInterCompanyDiscountsRepository : IGenericRepository<DbModel.TimesheetInterCompanyDiscount>, IDisposable
    {

    }
}
