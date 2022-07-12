using System;
using System.Collections.Generic;
using Evolution.Common.Models.Responses;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Evolution.Common.Models.Messages;
using Evolution.Common.Enums;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistWorkHistoryService
    {

        Response Get(TechnicalSpecialistWorkHistoryInfo searchModel);

        Response Get(IList<int> WorkhistoryIds);

        Response GetByPinId(IList<string> pinIds);

        Response Add(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistoryInfos,
                           bool commitChange = true,
                           bool isDbValidationRequire = true);

        Response Add(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistoryInfos,
                        ref IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistoryInfos,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Delete(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistoryInfos,
                     bool commitChange = true,
                     bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory, ref IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistory, bool commitChange = true, bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory,
                       bool commitChange = true,
                       bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory,
                        ref IList<DbModel.TechnicalSpecialistWorkHistory> dbWorkHistory,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);
        Response IsRecordValidForProcess(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory,
                                          ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory,
                                            ValidationType validationType,
                                            ref IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistory,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                             bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistWorkHistoryInfo> tsWorkHistory,
                                        ValidationType validationType,
                                        IList<DbModel.TechnicalSpecialistWorkHistory> dbtsWorkHistory);

        Response IsRecordExistInDb(IList<int> tsWorkHistorys,
                                        ref IList<DbModel.TechnicalSpecialistWorkHistory> dbTsWorkHistory,
                                        ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<int> tsWorkhistoryIds,
                                          ref IList<DbModel.TechnicalSpecialistWorkHistory> dbTsWorkHistory,
                                          ref IList<int> tsPayScheduleIdNotExists,
                                          ref IList<ValidationMessage> validationMessages);


    }
}
