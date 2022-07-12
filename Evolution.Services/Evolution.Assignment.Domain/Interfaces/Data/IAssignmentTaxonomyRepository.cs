using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Data
{
    public interface IAssignmentTaxonomyRepository: IGenericRepository<DbModel.AssignmentTaxonomy>, IDisposable
    {
        IList<DomainModel.AssignmentTaxonomy> search(DomainModel.AssignmentTaxonomy assignmentTaxonomy);
    }
}
