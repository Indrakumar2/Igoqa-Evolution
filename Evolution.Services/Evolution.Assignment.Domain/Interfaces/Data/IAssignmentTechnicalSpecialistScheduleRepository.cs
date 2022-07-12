using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System;

namespace Evolution.Assignment.Domain.Interfaces.Data
{
    public interface IAssignmentTechnicalSpecialistScheduleRepository : IGenericRepository<DbModel.AssignmentTechnicalSpecialistSchedule>, IDisposable
    {
        IList<DomainModel.AssignmentTechnicalSpecialistSchedule> Search(DomainModel.AssignmentTechnicalSpecialistSchedule model);

        DomainModel.AssignmentTechSpecSchedules GetAssignmentTechSpecRateSchedules(int assignmentId, int epin, params string[] includes);
        
        IList<DbModel.AssignmentTechnicalSpecialistSchedule> IsUniqueTSSchedule(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTSSchedules,
                                                                                IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTSSchedule);

        DomainModel.AssignmentTechSpecSchedules GetAssignmentTechSpecRateSchedules(int assignmentId,
                                                                                   IList<DbModel.Data> dbExpenseType,
                                                                                   params string[] includes);
    }
}
