using Evolution.Common.Enums;
using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Data
{
    public  interface IAssignmentTechnicalSpecilaistRepository: IGenericRepository<DbModel.AssignmentTechnicalSpecialist>, IDisposable
    {
        IList<DomainModel.AssignmentTechnicalSpecialist> Search(DomainModel.AssignmentTechnicalSpecialist model, params Expression<Func<DbModel.AssignmentTechnicalSpecialist, object>>[] includes);

        IList<DbModel.AssignmentTechnicalSpecialist> IsUniqueAssignmentTS(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTS,
                                                                          IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTS,
                                                                          ValidationType validationType);
    }
}
