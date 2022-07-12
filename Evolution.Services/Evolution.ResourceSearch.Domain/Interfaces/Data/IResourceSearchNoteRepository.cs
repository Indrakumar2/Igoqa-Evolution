using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.ResourceSearch.Domain.Models.ResourceSearch;

namespace Evolution.ResourceSearch.Domain.Interfaces.Data
{
    public interface IResourceSearchNoteRepository : IGenericRepository<DbModel.ResourceSearchNote>
    {
        IList<DomainModel.ResourceSearchNote> Search(DomainModel.ResourceSearchNote searchModel);

        IList<DomainModel.ResourceSearchNote> Get(IList<int> ResourceSearchIds, bool fetchLatest = false);
    }
}
