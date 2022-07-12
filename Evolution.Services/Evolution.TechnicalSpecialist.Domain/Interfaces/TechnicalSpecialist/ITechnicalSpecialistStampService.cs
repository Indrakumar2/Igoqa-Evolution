using System.Collections.Generic;
using Evolution.Common.Models.Responses;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Evolution.Common.Models.Messages;
using Evolution.Common.Enums;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistStampInfoService
    {
        Response Get(TechnicalSpecialistStampInfo searchModel);

        Response Get(IList<int> stampIds);

        Response GetByPinId(IList<string> pinIds);

        Response Get(IList<string> stampNumbers);

        Response Add(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                            bool commitChange = true,
                            bool isValidationRequire = true);

        Response Add(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                        ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbTsStampCountries,
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Modify(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Modify(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                        ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbTsStampCountries,
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Delete(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Delete(IList<TechnicalSpecialistStampInfo> tsStampInfos,
               ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                             bool commitChange = true,
                             bool isDbValidationRequire = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                            ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                         ValidationType validationType,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         ref IList<DbModel.Data> dbTsStampCountries,
                                         ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                         bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistStampInfo> tsStampInfos,
                                         ValidationType validationType,
                                         IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos);

        Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndStampNumber,
                                    ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                    ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndStampNumber,
                                    ref IList<DbModel.TechnicalSpecialistStamp> dbTsStampInfos,
                                    ref IList<KeyValuePair<string, string>> tsPinAndStampNumberNotExists,
                                    ref IList<ValidationMessage> validationMessages);
    }
}
