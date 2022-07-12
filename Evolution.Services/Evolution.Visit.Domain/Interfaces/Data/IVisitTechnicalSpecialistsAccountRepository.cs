using Evolution.Common.Enums;
using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;

namespace Evolution.Visit.Domain.Interfaces.Data
{
    public interface IVisitTechnicalSpecialistsAccountRepository : IGenericRepository<DbModel.VisitTechnicalSpecialist>, IDisposable
    {
        IList<DomainModel.VisitTechnicalSpecialist> Search(DomainModel.VisitTechnicalSpecialist searchModel, params string[] includes);

        IList<DbModel.VisitTechnicalSpecialist> IsUniqueVisitReference(IList<DomainModel.VisitTechnicalSpecialist> visitTechSpecTypes,
                                                                            IList<DbModel.VisitTechnicalSpecialist> dbVisitTecSpec,
                                                                            ValidationType validationType);

        List<DomainModel.ResourceInfo> IsEpinAssociated(DomainModel.VisitEmailData visitEmailData);

        IList<DomainModel.VisitTechnicalSpecialist> GetTechSpecForAssignment(List<long?> visitIds);

        //to be taken out after sync
        long? GetMaxEvoId();
    }
}
