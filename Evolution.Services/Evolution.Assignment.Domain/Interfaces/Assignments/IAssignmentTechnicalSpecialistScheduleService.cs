using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentTechnicalSpecialistScheduleService 
    {
        Response Get(DomainModel.AssignmentTechnicalSpecialistSchedule searchModel);

        Response Get(int assignmentId, int epin);

        Response Add(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, bool commitChange = true, bool isValidationRequired = true);

        Response Add(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ref IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules, ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                     ref IList<DbModel.ContractSchedule> dbContractSchedule, ref IList<DbModel.TechnicalSpecialistPaySchedule> dbPaySchedule,
                       bool commitChange = true, bool isDbValidationRequired = true);

        Response Modify(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, bool commitChange = true, bool isValidationRequired = true);

        Response Modify(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ref IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules,
                       ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist, ref IList<DbModel.ContractSchedule> dbContractSchedule,
                       ref IList<DbModel.TechnicalSpecialistPaySchedule> dbPaySchedule,bool commitChange = true, bool isDbValidationRequired = true);

        Response Delete(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, bool commitChange = true, bool isValidationRequired = true);
        Response Delete(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ref IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules, 
                        ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,ref IList<DbModel.ContractSchedule> dbContractSchedule, 
                         ref IList<DbModel.TechnicalSpecialistPaySchedule> dbPaySchedule,
                         bool commitChange = true, bool isDbValidationRequired = true);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ValidationType validationType, 
                                        ref IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules, 
                                        ref IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist, ref IList<DbModel.ContractSchedule> dbContractSchedule, 
                                        ref IList<DbModel.TechnicalSpecialistPaySchedule> dbPaySchedule);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentTechnicalSpecialistSchedule> assignmentTechnicalSpecialistSchedules, ValidationType validationType,
                                                   IList<DbRepository.Models.SqlDatabaseContext.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules,
                                                   IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist,
                                                   IList<DbModel.ContractSchedule> dbContractSchedule,
                                                   IList<DbModel.TechnicalSpecialistPaySchedule> dbTechnicalSpecialistPaySchedule);

        Response Get(int assignmentId,
                     IList<DbModel.Data> dbExpenseType);

        bool IsValidAssignmentTechnicalSpecialistSchedules(IList<int> assignmentTechnicalSpecialistScheduleIds, ref IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechnicalSpecialistSchedules, ref IList<ValidationMessage> validationMessages);
    }
}
