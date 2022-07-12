using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;

namespace Evolution.ResourceSearch.Domain.Interfaces.Data
{
    public interface IOverrideResourceRepository : IGenericRepository<DbModel.OverrideResource>
    {
        IList<DomainModel.OverridenPreferredResource> Get(IList<int> ResourceSearchIds);
    }
}
