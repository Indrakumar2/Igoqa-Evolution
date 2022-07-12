using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentSubSupplierTSService
    {
        Response Get(DomainModel.AssignmentSubSupplierTechnicalSpecialist searchModel);

        Response Add(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialists, bool commitChange = true,bool isDbValidationRequired = true,int? assignmentId=null);

        Response Add(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialists, ref IList<DbRepository.Models.SqlDatabaseContext.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTS, bool commitChange = true, bool isDbValidationRequired = true,int ? assignmentId = null);

        Response Modify(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialists, bool commitChange = true, bool isValidationRequired = true, int? assignmentId = null);

        Response Modify(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialists, ref IList<DbRepository.Models.SqlDatabaseContext.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTS, bool commitChange = true, bool isDbValidationRequired = true, int? assignmentId = null);

        Response Delete(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialists, bool commitChange = true,bool isDbValidationRequired=true, int? assignmentId = null);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialists, ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialists, ValidationType validationType, ref IList<DbRepository.Models.SqlDatabaseContext.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTS,ref IList<DbRepository.Models.SqlDatabaseContext.TechnicalSpecialist> technicalSpecialists);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplierTechnicalSpecialist> assignmentSubSupplierTechnicalSpecialists, ValidationType validationType, IList<DbRepository.Models.SqlDatabaseContext.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTS,IList<DbRepository.Models.SqlDatabaseContext.TechnicalSpecialist> technicalSpecialists);
    }
}
