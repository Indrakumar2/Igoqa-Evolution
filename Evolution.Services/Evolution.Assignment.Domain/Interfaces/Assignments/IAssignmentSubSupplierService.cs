using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentSubSupplierService
    {
        Response Get(DomainModel.AssignmentSubSupplier searchModel);

        Response Add(IList<DomainModel.AssignmentSubSupplier> AssignmentSubSuppliers, bool commitChange = true,bool isDbValidationRequired = true,int?assignmentId=null);

        Response Add(IList<DomainModel.AssignmentSubSupplier> AssignmentSubSuppliers, ref IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,ref IList<DbModel.Assignment> dbAssignment, bool commitChange = true, bool isDbValidationRequired = true,int?assignmentId=null);

        Response Modify(IList<DomainModel.AssignmentSubSupplier> AssignmentSubSuppliers, bool commitChange = true, bool isValidationRequired = true, int? assignmentId = null);

        Response Modify(IList<DomainModel.AssignmentSubSupplier> AssignmentSubSuppliers, ref IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,ref IList<DbModel.Assignment> dbAssignment, bool commitChange = true, bool isDbValidationRequired = true, int? assignmentId = null);

        Response Delete(IList<DomainModel.AssignmentSubSupplier> AssignmentSubSuppliers, bool commitChange = true,bool isDbValidationRequired=true, int? assignmentId = null);

        Response Delete(IList<DomainModel.AssignmentSubSupplier> AssignmentSubSuppliers, ref IList<DbModel.Assignment> dbAssignment, bool commitChange = true, bool isDbValidationRequired = true, int? assignmentId = null);

        //Response SaveMainSupplierContact(IList<DomainModel.AssignmentSubSupplier> AssignmentSubSuppliers,ref IList<DbModel.Assignment> assignments,bool commitChange=true,bool isDbValidationRequired = true);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
                                                  ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
                                                 ValidationType validationType,
                                                 ref IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier,
                                                 ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                 ref IList<DbModel.Assignment> dbAssignments);

        Response IsRecordValidForProcess(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
                                                 ValidationType validationType,
                                                 IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier,
                                                 ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                 ref IList<DbModel.Assignment> dbAssignments);

        Response GetSubSupplierForVisit(DomainModel.AssignmentSubSupplierVisit searchModel);
    }
}
