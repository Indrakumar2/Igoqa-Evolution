using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using DomainModel = Evolution.Visit.Domain.Models.Visits;
using System;

namespace Evolution.Visit.Domain.Interfaces.Data
{
    public interface IVisitTechnicalSpecialistTravelRepository : IGenericRepository<VisitTechnicalSpecialistAccountItemTravel>, IDisposable
    {
        IList<DomainModel.VisitSpecialistAccountItemTravel> Search(DomainModel.VisitSpecialistAccountItemTravel searchModel,
                                                                    params string[] includes);

        //to be taken out after sync
        long? GetMaxEvoId();
    }
}
