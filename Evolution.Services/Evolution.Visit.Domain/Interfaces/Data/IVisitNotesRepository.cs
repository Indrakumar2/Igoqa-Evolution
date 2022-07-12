using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Interfaces.Data
{
    public interface IVisitNotesRepository : IGenericRepository<DbModel.VisitNote>, IDisposable
    {
        IList<DomainModel.VisitNote> Search(DomainModel.VisitNote searchModel);
        //to be taken out after sync
        long? GetMaxEvoId();
    }
}
