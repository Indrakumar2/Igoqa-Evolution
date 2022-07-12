using Evolution.GenericDbRepository.Interfaces;
using System;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModels = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Data
{
    public interface IAssignmentInstructionsRepository : IGenericRepository<DbModel.AssignmentMessage>, IDisposable
    {
        DomainModels.AssignmentInstructions Search(int assignmentId);
    }
}
