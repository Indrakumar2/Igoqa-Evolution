using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System;

namespace Evolution.Assignment.Domain.Interfaces.Data
{
    public interface IAssignmentContributionCalculationRepository: IGenericRepository<DbModel.AssignmentContributionCalculation>, IDisposable
    {
        IList<DomainModel.AssignmentContributionCalculation> Search(DomainModel.AssignmentContributionCalculation model);
    }
}
