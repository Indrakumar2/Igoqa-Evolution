using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DomainModel = Evolution.Contract.Domain.Models.Contracts;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Models.Messages;
using Evolution.Common.Enums;

namespace Evolution.Contract.Domain.Interfaces.Contracts
{
    public interface IContractScheduleRateService
    {
        Response SaveContractScheduleRate(string contractNumber, string ScheduleName, IList<DomainModel.ContractScheduleRate> contractScheduleRates, ValidationType validationType, bool commitChange = true, bool isResultSetRequired = false);
        Response SaveContractScheduleRate(string contractNumber, string ScheduleName, IList<DomainModel.ContractScheduleRate> contractScheduleRates, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbContractExpense, IList<DbModel.SqlauditModule> dbModule, ValidationType validationType, bool commitChange = true, bool isResultSetRequired = false);
        Response ModifyContractScheduleRate(string contractNumber, string ScheduleName, IList<DomainModel.ContractScheduleRate> contractScheduleRates, bool commitChange = true, bool isResultSetRequired = false);
        Response ModifyContractScheduleRate(string contractNumber, string ScheduleName, IList<DomainModel.ContractScheduleRate> contractScheduleRates, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbContractExpense, IList<DbModel.SqlauditModule> dbModule,bool commitChange = true, bool isResultSetRequired = false);
        Response GetContractScheduleRate(DomainModel.ContractScheduleRate searchModel);

        Response DeleteContractScheduleRate(string contractNumber, string ScheduleName, IList<DomainModel.ContractScheduleRate> deleteModel, bool commitChange = true, bool isResultSetRequired = false);
        Response DeleteContractScheduleRate(string contractNumber, string ScheduleName, IList<DomainModel.ContractScheduleRate> deleteModel, IList<DbModel.Contract> dbContracts, bool commitChange = true, bool isResultSetRequired = false);
        void AssignContractScheduleRatesToSchedule(string contractNumber, ref List<DbModel.ContractSchedule> dbContractSchedules, IList<DomainModel.ContractScheduleRate> contractScheduleRates, IList<DbModel.Contract> dbContracts, IList<DbModel.Data> dbContractExpense, ref List<MessageDetail> errorMessages);
        void StandardInspectionTypeChargeRate(int companyId, IList<DomainModel.ContractScheduleRate> contractRates, ref IList<DbModel.CompanyInspectionTypeChargeRate> companyInspectionTypeChargeRates, ref List<MessageDetail> errorMessages);
    }
}
