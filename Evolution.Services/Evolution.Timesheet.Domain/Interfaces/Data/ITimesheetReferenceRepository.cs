using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;
using System;

namespace Evolution.Timesheet.Domain.Interfaces.Data
{
    public interface ITimesheetReferenceRepository : IGenericRepository<DbModel.TimesheetReference>, IDisposable
    {
        IList<DomainModel.TimesheetReferenceType> Search(DomainModel.TimesheetReferenceType model);

        IList<DbModel.TimesheetReference> IsUniqueTimesheetReference(IList<DomainModel.TimesheetReferenceType> timesheetReferenceTypes,
                                                                     IList<DbModel.TimesheetReference> dbTimesheetReference,
                                                                     ValidationType validationType);

        //to be taken out after sync
        long? GetMaxEvoId();
    }
}

