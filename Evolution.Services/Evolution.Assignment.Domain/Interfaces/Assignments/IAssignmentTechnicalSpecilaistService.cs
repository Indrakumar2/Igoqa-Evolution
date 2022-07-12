using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentTechnicalSpecilaistService 
    {
        Response Get(DomainModel.AssignmentTechnicalSpecialist searchModel);

        Response Add(IList<DomainModel.AssignmentTechnicalSpecialist> assignmenTechniaclSpecialist, bool commitChange = true, bool isValidationRequired = true, int? assignmentIds = null);

        Response Add(IList<DomainModel.AssignmentTechnicalSpecialist> assignmenTechniaclSpecialist,
                     ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                     ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                     ref IList<DbModel.Assignment> dbAssignment,
                     bool commitChange = true,
                     bool isDbValidationRequired = true,
                     int? assignmentId = null);

        Response Modify(IList<DomainModel.AssignmentTechnicalSpecialist> assignmenTechniaclSpecialist, bool commitChange = true, bool isValidationRequired = true, int? assignmentIds = null);

        Response Modify(IList<DomainModel.AssignmentTechnicalSpecialist> assignmenTechniaclSpecialist,
                                ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                ref IList<DbModel.Assignment> dbAssignment,
                                bool commitChange = true,
                                bool isDbValidationRequired = true,
                                int? assignmentId = null);

        Response Delete(IList<DomainModel.AssignmentTechnicalSpecialist> assignmenTechniaclSpecialist, bool commitChange = true, bool isValidationRequired = true, int? assignmentIds = null);

        Response Delete(IList<DomainModel.AssignmentTechnicalSpecialist> assignmenTechniaclSpecialist,
                        ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                        ref IList<DbModel.Assignment> dbAssignment,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        int? assignmentId = null);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentTechnicalSpecialist> assignmenTechniaclSpecialist, ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists,
                                                ValidationType validationType,
                                                ref IList<DbModel.Assignment> dbAssignments,
                                                ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialists,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialist,
                                                ValidationType validationType,
                                                IList<DbModel.Assignment> dbAssignments,
                                                IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists);

        bool IsValidAssignmentTechnicalSpecialist(IList<int> assignmentTechnicalSpecilaistIds, ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist, ref IList<ValidationMessage> validationMessages);
    }
}
