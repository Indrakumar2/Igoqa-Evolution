using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{

    public interface ITechnicalSpecialistPayRateService
    {
        Response Get(TechnicalSpecialistPayRateInfo searchModel);

        Response Get(IList<int> payRateIds);

        Response GetByTSPin(IList<string> tsPins);

        Response Get(IList<string> payScheduleNames);

        Response GetByPayRate(IList<string> expenseTypes);


        Response Add(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                       bool commitChange = true,
                       bool isDbValidationRequired = true);

        Response Add(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                        ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                        ref IList<DbModel.Data> dbExpenseTypes,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                        ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                        ref IList<DbModel.Data> dbExpenseTypes,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistPayRateInfo> tsPayRates, ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates, bool commitChange = true, bool isDbValidationRequired = true);


        Response IsRecordValidForProcess(IList<TechnicalSpecialistPayRateInfo> tsPayRates, ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                                         ValidationType validationType,
                                         ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                                ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTSPaySchedules,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbExpenseTypes,
                                                bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistPayRateInfo> tsPayRates,
                                            ValidationType validationType,
                                            IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates);

        Response IsRecordExistInDb(IList<int> tsPayRateIds,
                                        ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                        ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<int> tsPayRateIds,
                                          ref IList<DbModel.TechnicalSpecialistPayRate> dbTsPayRates,
                                          ref IList<int> tsPayRateIdNotExists,
                                          ref IList<ValidationMessage> validationMessages);

    }
}
