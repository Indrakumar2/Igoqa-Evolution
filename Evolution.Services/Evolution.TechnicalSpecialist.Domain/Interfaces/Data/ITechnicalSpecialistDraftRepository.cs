using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
    public interface ITechnicalSpecialistDraftRepository : IGenericRepository<DbModel.Draft>
    {
        IList<DomainModel.TechnicalSpecialistDraft> Search(DomainModel.TechnicalSpecialistDraft searchModel);
    }
}
