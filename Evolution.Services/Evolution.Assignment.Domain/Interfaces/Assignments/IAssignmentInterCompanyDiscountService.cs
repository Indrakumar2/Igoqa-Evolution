using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentInterCompanyDiscountService
    {
        Response Get(int assignmentId);

        Response Add(AssignmentInterCoDiscountInfo assignmentInterCompanyDiscounts,
                    bool commitChange = true,
                    bool isValidationRequire = true);

        Response Add(AssignmentInterCoDiscountInfo assignmentInterCompanyDiscounts,
                    ref DbModel.Assignment dbAssignments,
                    ref IList<DbModel.Company> dbCompanies,
                    ref IList<DbModel.AssignmentInterCompanyDiscount> dbAssignmentInterCompanyDiscounts,
                    bool commitChange = true,
                    bool isValidationRequire = true);

        Response Modify(AssignmentInterCoDiscountInfo assignmentInterCompanyDiscounts,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Modify(AssignmentInterCoDiscountInfo assignmentInterCompanyDiscounts,
                        ref DbModel.Assignment dbAssignments,
                        ref IList<DbModel.Company> dbCompanies,
                        ref IList<DbModel.AssignmentInterCompanyDiscount> dbAssignmentInterCompanyDiscounts,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Delete(AssignmentInterCoDiscountInfo interCompanyDiscounts,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response IsRecordValidForProcess(AssignmentInterCoDiscountInfo interCompanyDiscounts,
                                        ValidationType validationType);

        Response IsRecordValidForProcess(AssignmentInterCoDiscountInfo interCompanyDiscounts,
                                        ref DbModel.Assignment dbAssignment,
                                        ref IList<DbModel.Company> dbCompanies,
                                        ValidationType validationType,
                                        ref IList<DbModel.AssignmentInterCompanyDiscount> dbisnterCompanyDiscount);

        Response IsRecordValidForProcess(AssignmentInterCoDiscountInfo interCompanyDiscounts,
                                        ValidationType validationType,
                                        DbModel.Assignment dbAssignment,
                                        IList<DbModel.Company> dbCompanies,
                                        IList<DbModel.AssignmentInterCompanyDiscount> dbinterCompanyDiscounts);
    }
}