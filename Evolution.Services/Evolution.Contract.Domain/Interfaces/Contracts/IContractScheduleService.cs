using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Contract.Domain.Interfaces.Contracts
{
    public interface IContractScheduleService
    {
        Response SaveContractSchedule(string contractNumber, IList<Models.Contracts.ContractSchedule> contractSchedules, bool commitChange = true, bool isResultSetRequired = false);
        Response SaveContractScheduleAndScheduleRate(string contractNumber, IList<Models.Contracts.ContractSchedule> contractSchedules, IList<Models.Contracts.ContractScheduleRate> contractScheduleRates,
                                                     IList<DbModels.Contract> dbContracts, IList<DbModels.Data> dbCurrency, IList<DbModels.Data> dbExpense, IList<DbModels.SqlauditModule> dbModule, 
                                                     ref IList<DbModels.ContractRate> dbContractRate, ref IList<DbModels.ContractSchedule> dbInsertedContractSchedules, ValidationType validationType, bool commitChange = true, 
                                                     bool isResultSetRequired = false);

        Response ModifyContractSchedule(string contractNumber, IList<Models.Contracts.ContractSchedule> contractSchedules, bool commitChange = true, bool isResultSetRequired = false);
        Response ModifyContractSchedule(string contractNumber, IList<Models.Contracts.ContractSchedule> contractSchedules, IList<DbModels.Contract> dbContracts, IList<DbModels.Data> dbCurrency, IList<DbModels.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);
        Response GetContractSchedule(Models.Contracts.ContractSchedule searchModel);
        Response DeleteContractSchedule(string contractNumber, IList<Models.Contracts.ContractSchedule> contractSchedules, bool commitChange = true, bool isResultSetRequired = false);
        Response DeleteContractSchedule(string contractNumber, IList<Models.Contracts.ContractSchedule> contractSchedules, IList<DbModels.Contract> dbContract, IList<DbModels.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false);
        bool IsValidContractSchedule(IList<int> contractScheduleIds, ref IList<DbModels.ContractSchedule> dbContractSchedules, ref IList<ValidationMessage> validationMessages);
        /*AddScheduletoRF*/
        Response CopyCStoRFC(IList<Models.Contracts.ContractSchedule> contractSchedule);
        /// <summary>
        /// Bulk Process
        /// </summary>
        /// <param name="frameworkContractId"></param>
        /// <returns></returns>
        string RelatedFrameworkContractBatch(int frameworkContractId, string createdBy);

        Response IsRelatedFrameworkContractExists(int contractId);

        Response IsDuplicateFrameworkSchedulesExists(int contractId);

        void AddRates(IList<DbModels.ContractRate> dbContractRate);

        void AuditSchedules(long? eventId, int contractId,string contractNumber, List<DbModels.ContractSchedule> dbContractSchedule, IList<DbModels.SqlauditModule> dbModule,IList<DbModels.Data> dbExpense,bool IsBulk);
    }
}
