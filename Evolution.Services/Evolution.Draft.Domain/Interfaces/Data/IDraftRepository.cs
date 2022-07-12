using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Draft.Domain.Models;

namespace Evolution.Draft.Domain.Interfaces.Data
{
    public interface IDraftRepository : IGenericRepository<DbModel.Draft>
    {
        IList<DomainModel.Draft> Search(DomainModel.Draft searchModel);
        IList<DomainModel.Draft> GetDraftMyTask(DomainModel.Draft searchModel);
        void UpdateTechSpec(DomainModel.Draft model, int pendingWithId); //D1301
    }
}
