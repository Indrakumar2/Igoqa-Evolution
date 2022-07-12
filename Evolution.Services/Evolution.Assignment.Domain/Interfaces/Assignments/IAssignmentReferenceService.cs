using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentReferenceService 
    {
        Response Get(DomainModel.AssignmentReferenceType assignmentReference);

        Response Add(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes, bool commitChange = true, bool isValidationRequired = true, int? assignmentId = null);

        Response Add(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes, ref IList<DbModel.AssignmentReference> dbAssignmentReferenceTypes, ref IList<DbModel.Assignment> dbAssignment, ref IList<DbModel.Data> dbReferenceType, bool commitChange = true, bool isDbValidationRequired = true, int? assignmentId = null);

        Response Modify(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes, bool commitChange = true, bool isValidationRequired = true, int? assignmentId = null);

        Response Modify(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes, ref IList<DbModel.AssignmentReference> dbAssignmentReferenceTypes, ref IList<DbModel.Assignment> dbAssignment, ref IList<DbModel.Data> dbReferenceType, bool commitChange = true, bool isDbValidationRequired = true, int? assignmentId = null);

        Response Delete(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes, bool commitChange = true, bool isDbValidationRequired = true, int? assignmentIds = null);

        Response Delete(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes, ref IList<DbModel.AssignmentReference> dbAssignmentReferenceTypes, ref IList<DbModel.Assignment> dbAssignment, ref IList<DbModel.Data> dbReferenceType,bool commitChange = true, bool isValidationRequired = true, int? assignmentId = null);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes, ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes,
                                                 ValidationType validationType,
                                                 ref IList<DbModel.AssignmentReference> dbAssignmentReferenceTypes,
                                                 ref IList<DbModel.Assignment> dbAssignment,
                                                 ref IList<DbModel.Data> dbReferenceType);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes,
                                                  ValidationType validationType,
                                                  IList<DbModel.AssignmentReference> dbAssignmentReferenceTypes,
                                                  ref IList<DbModel.Assignment> dbAssignment,
                                                  ref IList<DbModel.Data> dbReferenceType);
    }
}
