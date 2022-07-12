using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;
using System;

namespace Evolution.Assignment.Domain.Interfaces.Data
{
    public interface IAssignmentReferenceTypeRepository: IGenericRepository<DbModel.AssignmentReference>, IDisposable
    {
        IList<DomainModel.AssignmentReferenceType> Search(DomainModel.AssignmentReferenceType model);

        IList<DbModel.AssignmentReference> IsUniqueAssignmentReference(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes,
                                                               IList<DbModel.AssignmentReference> dbAssignmentRefrenceType,
                                                               ValidationType validationType);
    }
}
