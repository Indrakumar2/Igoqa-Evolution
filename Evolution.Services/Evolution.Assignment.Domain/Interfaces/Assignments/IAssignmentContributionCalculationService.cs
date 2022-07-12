using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentContributionCalculationService
    {
        Response Get(AssignmentContributionCalculation searchModel);

        Response Add(IList<AssignmentContributionCalculation> assignmentContributionCalculation, 
                     bool commitChange = true,
                     bool isValidationRequired = true,
                     int? assignmentIds=null);

        Response Add(IList<AssignmentContributionCalculation> assignmentContributionCalculation,
                     ref IList<DbModel.Assignment> dbAssignment,
                     ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation, 
                     bool commitChange = true, 
                     bool isValidationRequired = true, 
                     int? assignmentIds = null);

        Response Modify(IList<AssignmentContributionCalculation> assignmentContributionCalculation, 
                        bool commitChange = true,
                        bool isValidationRequired = true,
                        int? assignmentIds=null);

        Response Modify(IList<AssignmentContributionCalculation> assignmentContributionCalculation,
                        ref IList<DbModel.Assignment> dbAssignment,
                        ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation,
                        bool commitChange = true,
                        bool isValidationRequired = true,
                        int? assignmentIds = null);

        Response Delete(IList<AssignmentContributionCalculation> assignmentContributionCalculation,
                        bool commitChange = true,
                        bool isValidationRequired = true,
                        int? assignmentIds = null);

        Response Delete(IList<AssignmentContributionCalculation> assignmentContributionCalculation,
                             ref IList<DbModel.Assignment> dbAssignment,
                             ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation,
                             bool commitChange = true,
                             bool isValidationRequired = true,
                             int? assignmentIds = null);

        Response IsRecordValidForProcess(IList<AssignmentContributionCalculation> assignmentContributionCalculation,
                                         ValidationType validationType);

        Response IsRecordValidForProcess(IList<AssignmentContributionCalculation> assignmentContributionCalculation,
                                         ValidationType validationType,
                                         ref IList<DbModel.Assignment> dbAssignments,
                                         ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation);

        Response IsRecordValidForProcess(IList<AssignmentContributionCalculation> assignmentContributionCalculation,
                                         ValidationType validationType, 
                                         IList<DbModel.Assignment> dbAssignments,
                                         IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation);

        bool IsValidAssignmentContributionCalculation(IList<int> assignmentContributionRevenueCostIds,
                                                      ref IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculations,
                                                      ref IList<ValidationMessage> validationMessages);
    }
}
