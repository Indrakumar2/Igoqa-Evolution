using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using Evolution.Common.Enums;
using System;

namespace Evolution.Timesheet.Domain.Interfaces.Data
{
    public interface ITechSpecAccountItemConsumableRepository : IGenericRepository<TimesheetTechnicalSpecialistAccountItemConsumable>, IDisposable
    {
        IList<DomainModel.TimesheetSpecialistAccountItemConsumable> Search(DomainModel.TimesheetSpecialistAccountItemConsumable searchModel,
                                                                    params string[] includes);

        IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> IsUniqueTimesheetTSConsumables(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> timesheetTsConsumables,
                                                                    IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbTimesheetTsConsumables,
                                                                    ValidationType validationType);


        //to be taken out after sync
        long? GetMaxEvoId();
    }
}
