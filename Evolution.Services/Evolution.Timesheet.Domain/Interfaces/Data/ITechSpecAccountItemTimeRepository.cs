using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;
using System;

namespace Evolution.Timesheet.Domain.Interfaces.Data
{
    public interface ITechSpecAccountItemTimeRepository : IGenericRepository<TimesheetTechnicalSpecialistAccountItemTime>, IDisposable
    {
        IList<DomainModel.TimesheetSpecialistAccountItemTime> Search(DomainModel.TimesheetSpecialistAccountItemTime searchModel,
                                                                     params string[] includes);

        IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> IsUniqueTimesheetTSTime(IList<DomainModel.TimesheetSpecialistAccountItemTime> timesheetTsTime,
                                                            IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbTimesheetTsTime,
                                                            ValidationType validationType);


        //to be taken out after sync
        long? GetMaxEvoId();
    }
}
