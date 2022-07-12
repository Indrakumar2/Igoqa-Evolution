using DbModel=Evolution.DbRepository.Models.SqlDatabaseContext;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Contract.Domain.Models.Contracts;
using Evolution.AuditLog.Domain.Interfaces.Audit;

namespace Evolution.Contract.Domain.Interfaces.Data
{
    public interface IContractBatchRepository
    {
        string RelatedFrameworkContractBatch(int frameworkContractId, string createdBy, List<DbModel.SqlauditModule> dbModule, IAuditLogger _auditLogger);

        string BulkRelatedFrameworkCopy(IList<DbModel.ContractSchedule> alstSchedulesToUpdate, IList<DbModel.ContractSchedule> alstSchedulesToInsert, IList<DbModel.ContractRate> alstRatesToInsert, IList<DbModel.ContractRate> alstRatesToUpdate ,IList<DbModel.ContractRate> ratesToDelete);

        string BulkRelatedFrameworkCopyAudit(IList<DbModel.SqlauditLogDetail> sqlauditLogDetails);

        void BulkInsertSchedule(int contractId, List<DbModel.ContractSchedule> dbContractSchedules, IList<ContractScheduleRate> contractScheduleRates, IList<DbModel.CompanyInspectionTypeChargeRate> companyInspectionTypeCharges, IList<DbModel.Data> dbExpense, ref IList<DbModel.ContractRate> dbContractRate, ref IList<DbModel.ContractSchedule> dbContractSchedule);

        void BulkInsertRate(IList<DbModel.ContractRate> dbcontractRate);

        void BulkAuditInsertSchedule(long? eventId, int contractId, string contractNumber, List<DbModel.ContractSchedule> dbContractSchedule, IList<DbModel.SqlauditModule> dbModule, IList<DbModel.Data> dbExpense, bool IsBulk = false);
    }
}
