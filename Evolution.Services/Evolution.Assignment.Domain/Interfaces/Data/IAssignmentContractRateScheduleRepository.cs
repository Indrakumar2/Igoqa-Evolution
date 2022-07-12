using Evolution.Common.Enums;
using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Data
{
    public interface IAssignmentContractRateScheduleRepository:IGenericRepository<DbModel.AssignmentContractSchedule>, IDisposable
    {
        IList<DomainModel.AssignmentContractRateSchedule> Search(DomainModel.AssignmentContractRateSchedule model);

        IList<DbModel.AssignmentContractSchedule> IsUniqueAssignmentContractSchedules(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules,
                                                                              IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedule,
                                                                              ValidationType validationType);


    }
}
