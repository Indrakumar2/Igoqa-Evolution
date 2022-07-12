using System;
using System.Collections.Generic;
using System.Text;
using Evolution.GenericDbRepository.Interfaces;
using DomainModel = Evolution.Reports.Domain.Models.Reports;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Reports.Domain.Interfaces.Data
{
    public interface ITaxonomyRepository :IGenericRepository<DbModel.TechnicalSpecialist>
    {
        IList<DomainModel.Taxonomy> Search(DomainModel.Taxonomy searchModel);
    }
}
