using System;
using Evolution.Common.Enums;
using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Domain.Interfaces.Data
{
    public interface ITimesheetTechnicalSpecialistRepository : IGenericRepository<DbModel.TimesheetTechnicalSpecialist>, IDisposable
    {
        IList<DomainModel.TimesheetTechnicalSpecialist> Search(DomainModel.TimesheetTechnicalSpecialist searchModel, params string[] includes);

        IList<DbModel.TimesheetTechnicalSpecialist> IsUniqueTimesheetTechspec(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechSpecTypes,
                                                                            IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTecSpec,
                                                                            ValidationType validationType);

        List<DomainModel.ResourceInfo> IsEpinAssociated(DomainModel.TimesheetEmailData timesheetEmailData);

        IList<DomainModel.TimesheetTechnicalSpecialist> GetTechSpecForAssignment(List<long?> visitIds);

        //to be taken out after sync
        long? GetMaxEvoId();
    }
}
