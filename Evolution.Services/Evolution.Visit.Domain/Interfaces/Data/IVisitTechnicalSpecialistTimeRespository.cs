using Evolution.GenericDbRepository.Interfaces;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Collections.Generic;
using DomainModel = Evolution.Visit.Domain.Models.Visits;
using System;

namespace Evolution.Visit.Domain.Interfaces.Data
{
    public interface IVisitTechnicalSpecialistTimeRespository : IGenericRepository<VisitTechnicalSpecialistAccountItemTime>, IDisposable
    {
        IList<DomainModel.VisitSpecialistAccountItemTime> Search(DomainModel.VisitSpecialistAccountItemTime searchModel,
                                                                     params string[] includes);


        //to be taken out after sync
        long? GetMaxEvoId();
    }
}
