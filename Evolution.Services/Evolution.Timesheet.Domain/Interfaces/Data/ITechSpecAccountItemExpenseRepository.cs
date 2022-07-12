using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using Evolution.Common.Enums;
using System;

namespace Evolution.Timesheet.Domain.Interfaces.Data
{
    public interface ITechSpecAccountItemExpenseRepository : IGenericRepository<TimesheetTechnicalSpecialistAccountItemExpense>, IDisposable
    {
        IList<DomainModel.TimesheetSpecialistAccountItemExpense> Search(DomainModel.TimesheetSpecialistAccountItemExpense searchModel,
                                                                      params string[] includes);

        IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> IsUniqueTimesheetTechSpecialistExpense(IList<DomainModel.TimesheetSpecialistAccountItemExpense> timesheetTsExpenses,
                                                                   IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbTimesheetTsExpenses,
                                                                   ValidationType validationType);

        //to be taken out after sync
        long? GetMaxEvoId();
    }
}
