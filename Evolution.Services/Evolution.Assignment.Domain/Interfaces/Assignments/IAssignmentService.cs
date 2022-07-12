using Evolution.Common.Enums;
using Evolution.Common.Models.Budget;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Filters;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;



namespace Evolution.Assignment.Domain.Interfaces.Assignments
{
    public interface IAssignmentService
    {
        Task<Response> GetAssignment(DomainModel.AssignmentSearch searchModel, AdditionalFilter filter = null);

        /*This is used for Document Approval dropdown binding*/
        List<DbModel.Assignment> addAssigenmentSubSupplier(int supplierPOId, List<int> subSupplierlists, String modifiedby,string SupplierType);
        void RemoveAsssubsupplier(int supplierPOId, List<int> subSupplierlists,string SupplierType);
        Response GetDocumentAssignment(DomainModel.AssignmentDashboard searchModel);

        Response GetAssignment(DomainModel.AssignmentSearch searchModel);

        Response GetAssignmentBudgetDetails(string companyCode = null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool isAssignmentOnly = true);

        Response GetAssignmentBudgetDetails(IList<BudgetAccountItem> budgetAccountItems, IList<Common.Models.ExchangeRate.ContractExchangeRate> contractExchangeRates = null, IList<ExchangeRate> currencyExchangeRates = null);

        Response GetAssignmentInvoiceInfo(string companyCode = null, List<int> contractIds = null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool showMyAssignmentsOnly = true);

        Response GetAssignmentInvoiceInfo(List<string> contractNumber = null, List<int> projectNumber = null, List<int> assignmentNumber = null, bool IsAssignmentFetchRequired = true, List<int> autoContractIds = null);

        Task<Response> SearchAssignment(DomainModel.AssignmentEditSearch searchModel);

        bool IsValidAssignment(IList<int> assignmentId,
                               ref IList<DbModel.Assignment> dbAssignments,
                               ref IList<ValidationMessage> messages,
                               params Expression<Func<DbModel.Assignment, object>>[] includes);

        bool IsValidAssignment(IList<int> assignmentId,
                               ref IList<DbModel.Assignment> dbAssignments,
                               ref IList<ValidationMessage> messages,
                               string[] includes);

        bool IsValidAssignment(int assignmentNumber,
                                int assignmentProjectNumber,
                                ref DbModel.Assignment dbAssignment,
                                ref IList<ValidationMessage> messages,
                               string[] includes);

        Response Add(IList<DomainModel.Assignment> assignments,
                     bool commitChange = true,
                     bool isValidationRequire = true);

        Response Add(IList<DomainModel.Assignment> assignments,
                     ref IList<DbModel.Assignment> dbAssignment,
                     ref DomainModel.AssignmentDatabaseCollection assignmentDatabaseCollection,
                     bool commitChange = true,
                     bool isValidationRequire = true);

        Response Modify(IList<DomainModel.Assignment> assignments,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Modify(IList<DomainModel.Assignment> assignments,
                        ref IList<DbModel.Assignment> dbAssignment,
                        ref DomainModel.AssignmentDatabaseCollection assignmentDatabaseCollection,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Modify(DbModel.Assignment dbAssignment, bool commitChange = true);

        Response Modify(int assignmentId, 
                        List<KeyValuePair<string,object>> updateValueProps,
                        bool commitChange = true,
                        params Expression<Func<DbModel.Assignment, object>>[] updatedProperties);
        Response Delete(IList<DomainModel.Assignment> assignments,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response IsRecordValidForProcess(IList<DomainModel.Assignment> assignments,
                                         ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.Assignment> assignments,
                                         ValidationType validationType,
                                         ref IList<DomainModel.Assignment> filteredAssignment,
                                         ref IList<DbModel.Assignment> dbAssignments);

        Response IsRecordValidForProcess(IList<DomainModel.Assignment> assignments,
                                         ValidationType validationType,
                                         ref IList<DbModel.Assignment> dbAssignments,
                                         ref DomainModel.AssignmentDatabaseCollection assignmentDatabaseCollection,
                                         bool IsAssignmentValidationRequired = true);

        Response IsRecordValidForProcess(IList<DomainModel.Assignment> assignments,
                                        ValidationType validationType,
                                        IList<DbModel.Assignment> dbAssignments,
                                        ref DomainModel.AssignmentDatabaseCollection assignmentDatabaseCollection);


        IList<DbModel.AssignmentMessage> AssignAssignmentMessages(List<DbModel.AssignmentMessage> dbMessages,
                                                                  DomainModel.Assignment assignment);

        Response IsRecordValidForProcess(IList<DomainModel.Assignment> assignments, IList<DbModel.Assignment> dbAssignments, ValidationType validationType);

        Response Add(IList<DomainModel.Assignment> assignments, ref DbModel.Assignment dbAssignment,bool commitChange = true);
    }
}
