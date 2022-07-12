using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;
using System.Collections.Generic;
using System;

namespace Evolution.Visit.Domain.Interfaces.Data
{
    public interface IVisitTechnicalSpecialistConsumableRepository : IGenericRepository<VisitTechnicalSpecialistAccountItemConsumable>, IDisposable
    {
        IList<DomainModel.VisitSpecialistAccountItemConsumable> Search(DomainModel.VisitSpecialistAccountItemConsumable searchModel,
                                                                    params string[] includes);

        //to be taken out after sync
        long? GetMaxEvoId();
    }
}
