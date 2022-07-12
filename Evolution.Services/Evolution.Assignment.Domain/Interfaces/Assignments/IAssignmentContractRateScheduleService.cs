using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentContractRateScheduleService 
    {
        Response Get(DomainModel.AssignmentContractRateSchedule searchModel);

        Response Add(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules, 
                     bool commitChange = true,
                     bool isDbValidationRequired = true,
                     int? assignmentId =null);

        Response Add(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules, 
                     ref IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules,
                     ref IList<DbModel.Assignment> dbAssignment,
                     ref IList<DbModel.ContractSchedule> dbContractSchedules,
                     bool commitChange = true, 
                     bool isDbValidationRequired = true, 
                     int? assignmentId = null);

        Response Modify(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules, 
                        bool commitChange = true, 
                        bool isDbValidationRequired = true, 
                        int? assignmentId = null);

        Response Modify(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules, 
                        ref IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules, 
                        ref IList<DbModel.Assignment> dbAssignment,
                        ref IList<DbModel.ContractSchedule> dbContractSchedules,
                        bool commitChange = true, 
                        bool isDbValidationRequired = true, 
                        int? assignmentId = null);

        Response Delete(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules, 
                        bool commitChange = true,
                        bool isDbValidationRequired=true, 
                        int? assignmentId = null);

        Response Delete(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules,
                        ref IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules,
                        ref IList<DbModel.Assignment> dbAssignment,
                        ref IList<DbModel.ContractSchedule> dbContractSchedules,
                        bool commitChange = true,
                        bool isDbValidationRequired = true,
                        int? assignmentId = null);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules,  
                                         ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules, 
                                         ValidationType validationType, 
                                         ref IList<DbModel.Assignment> dbAssignment,
                                         ref IList<DbModel.ContractSchedule> dbContractSchedules,
                                         ref IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules, 
                                         ValidationType validationType, 
                                         IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules,
                                          IList<DbModel.ContractSchedule> dbContractSchedules,
                                         ref IList<DbModel.Assignment> dbAssignment);
    }
}
