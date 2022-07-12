using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Data
{
    public interface IAssignmentContributionRevenueCostRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.AssignmentContributionRevenueCost>, IDisposable
    {
        IList<DomainModel.AssignmentContributionRevenueCost> Search(DomainModel.AssignmentContributionRevenueCost model);
    }
}
