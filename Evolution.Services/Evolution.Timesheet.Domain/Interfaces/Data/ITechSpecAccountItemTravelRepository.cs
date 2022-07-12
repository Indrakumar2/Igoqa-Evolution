using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;
using System;

namespace Evolution.Timesheet.Domain.Interfaces.Data
{
    public interface ITechSpecAccountItemTravelRepository : IGenericRepository<TimesheetTechnicalSpecialistAccountItemTravel>, IDisposable
    {
        IList<DomainModel.TimesheetSpecialistAccountItemTravel> Search(DomainModel.TimesheetSpecialistAccountItemTravel searchModel,
                                                                    params string[] includes);

        IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> IsUniqueTimesheetTSTravel(IList<DomainModel.TimesheetSpecialistAccountItemTravel> timesheetTsTravel,
                                                           IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbTimesheetTsTravel,
                                                           ValidationType validationType);

        //to be taken out after sync
        long? GetMaxEvoId();
    }
}
