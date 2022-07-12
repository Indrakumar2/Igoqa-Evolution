using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistCodeAndStandardService
    {
        Response Get(TechnicalSpecialistCodeAndStandardinfo searchModel);

        Response Get(IList<int> CodeAndStandardIds);

        Response GetByPinId(IList<string> pinIds);

        Response Get(IList<string> CodeStandardName);

        Response Add(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                            bool commitChange = true,
                            bool isDbValidationRequire = true);

        Response Add(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                        ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbtsCodeAndStandardInfos,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbCodes,
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Modify(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Modify(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                        ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbtsCodeAndStandardInfos,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbCodes,
                        bool commitChange = true,
                        bool isDbValidationRequire = true);


        Response Delete(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                              bool commitChange = true,
                              bool isDbValidationRequire = true);

        Response Delete(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
             ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbTstCodeAndStandards,
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                                            ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                                         ValidationType validationType,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         ref IList<DbModel.Data> dbCodes,
                                         ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbtsCodeAndStandardInfos
                                         , bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCodeAndStandardinfo> tsCodeAndStandardInfos,
                                            ValidationType validationType,
                                            IList<DbModel.TechnicalSpecialistCodeAndStandard> dbtsCodeAndStandardInfos);

        Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndCodeAndStandard,
                                    ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbtsCodeAndStandardInfos,
                                    ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndCodeAndStandard,
                                    ref IList<DbModel.TechnicalSpecialistCodeAndStandard> dbtsCodeAndStandardInfos,
                                    ref IList<KeyValuePair<string, string>> tsPinAndCodeAndStandardsNotExists,
                                    ref IList<ValidationMessage> validationMessages);
    }
}
