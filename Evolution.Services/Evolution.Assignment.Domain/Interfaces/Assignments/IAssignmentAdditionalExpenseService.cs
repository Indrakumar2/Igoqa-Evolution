using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentAdditionalExpenseService 
    {
        Response Get(AssignmentAdditionalExpense searchModel);

        Response Add(IList<AssignmentAdditionalExpense> additionalExpenses, bool commitChange = true, bool isValidationRequire = true,int? assignmentIds=null);

        Response Add(IList<AssignmentAdditionalExpense> additionalExpenses, ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpense, ref IList<DbModel.Company> dbCompanies, ref IList<DbModel.Data> dbExpenseTypes, ref IList<DbModel.Assignment> dbAssignment,bool commitChange = true, bool isValidationRequire = true,int ? assignmentIds = null);

        Response Modify(IList<AssignmentAdditionalExpense> additionalExpenses, bool commitChange = true, bool isValidationRequire = true, int? assignmentIds = null);

        Response Modify(IList<AssignmentAdditionalExpense> additionalExpenses, ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpense, ref IList<DbModel.Company> dbCompanies, ref IList<DbModel.Data> dbExpenseTypes, ref IList<DbModel.Assignment> dbAssignments, bool commitChange = true, bool isValidationRequire = true, int? assignmentIds = null);

        Response Delete(IList<AssignmentAdditionalExpense> additionalExpenses, bool commitChange = true, bool isValidationRequire = true, int? assignmentIds = null);

        Response Delete(IList<AssignmentAdditionalExpense> additionalExpenses, ref IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpense, ref IList<DbModel.Company> dbCompanies, ref IList<DbModel.Data> dbExpenseTypes, ref IList<DbModel.Assignment> dbAssignments,  bool commitChange = true, bool isValidationRequire = true, int? assignmentIds = null);

        Response IsRecordValidForProcess(IList<AssignmentAdditionalExpense> additionalExpenses, ValidationType validationType);

        Response IsRecordValidForProcess(IList<AssignmentAdditionalExpense> additionalExpenses, ValidationType validationType, ref IList<DbModel.AssignmentAdditionalExpense> dbAdditionalExpense,ref IList<DbModel.Company> dbCompanies,ref IList<DbModel.Data> dbExpenseTypes, ref IList<DbModel.Assignment> dbAssignmentss);

        Response IsRecordValidForProcess(IList<AssignmentAdditionalExpense> additionalExpenses, ValidationType validationType, IList<DbModel.AssignmentAdditionalExpense> dbAdditionalExpenses,IList<DbModel.Company> dbCompanies,  IList<DbModel.Data> dbExpenseTypes, IList<DbModel.Assignment> dbAssignments);
    }
}
