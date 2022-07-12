using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistPayScheduleService
    {
        Response Get(TechnicalSpecialistPayScheduleInfo searchModel);

        Response Get(IList<int> payscheduleIds);

        Response GetByTSPin(IList<string> tsPins);

        Response Get(IList<string> payScheduleName);

        Response Add(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                       bool commitChange = true,
                       bool isDbValidationRequired = true);

        Response Add(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                        ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, 
                        ref IList<DbModel.Data> dbCurrencies,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                        ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbCurrencies,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
            ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
            bool commitChange = true,
            bool isDbValidationRequired = true);


        Response IsRecordValidForProcess(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                                            ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                                            ValidationType validationType,
                                            ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                                               ValidationType validationType,
                                               ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                               ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                               ref IList<DbModel.Data> dbCurrencies,
                                                bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistPayScheduleInfo> tsPaySchedules,
                                            ValidationType validationType,
                                            IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules);

        Response IsRecordExistInDb(IList<int> tsPayScheduleIds,
                                        ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                        ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<int> tsPayScheduleIds,
                                          ref IList<DbModel.TechnicalSpecialistPaySchedule> dbTsPaySchedules,
                                          ref IList<int> tsPayScheduleIdNotExists,
                                          ref IList<ValidationMessage> validationMessages);

        bool IsValidPaySchedule(IList<int> payScheduleIds, ref IList<DbModel.TechnicalSpecialistPaySchedule> dbPaySchedule, ref IList<ValidationMessage> validationMessages);
    }
}
