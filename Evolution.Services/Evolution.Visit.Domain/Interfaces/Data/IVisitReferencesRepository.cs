using Evolution.Common.Enums;
using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Interfaces.Data
{
    public interface IVisitReferencesRepository : IGenericRepository<DbModel.VisitReference>, IDisposable
    {
        IList<DomainModel.VisitReference> Search(DomainModel.VisitReference model);

        IList<DbModel.VisitReference> IsUniqueVisitReference(IList<DomainModel.VisitReference> timesheetReferenceTypes,
                                                                     IList<DbModel.VisitReference> dbVisitReference,
                                                                     ValidationType validationType);

        //to be taken out after sync
        long? GetMaxEvoId();
    }
}
