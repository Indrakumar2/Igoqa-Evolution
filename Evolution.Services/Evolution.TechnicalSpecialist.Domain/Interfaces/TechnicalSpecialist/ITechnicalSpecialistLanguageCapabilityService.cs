using System;
using System.Collections.Generic;
using Evolution.Common.Models.Responses;
using System.Text;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistLanguageCapabilityService
    {
        Response Get(TechnicalSpecialistLanguageCapabilityInfo searchModel);

        Response Get(IList<int> LanguageCapabilityIds);

        Response GetByPinId(IList<string> pinIds);

        Response Get(IList<string> Language);

        Response Add(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                          bool commitChange = true,
                          bool isValidationRequire = true);

        Response Add(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                                        ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         ref IList<DbModel.Data> dbLanguages,
                                        bool commitChange = true,
                                        bool isDbValidationRequire = true);

        Response Modify(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Modify(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                                        ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         ref IList<DbModel.Data> dbLanguages,
                                        bool commitChange = true,
                                        bool isDbValidationRequire = true);

        Response Delete(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Delete(IList<TechnicalSpecialistLanguageCapabilityInfo> tsCustomerApprovalInfos,
              ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                              bool commitChange = true,
                              bool isDbValidationRequire = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                                            ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                                            ValidationType validationType,
                                            ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistLanguageCapabilityInfo> tsCustomerApprovalInfos,
                                               ValidationType validationType,
                                               ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTsLanguageCapabilities,
                                               ref IList<DbModel.Data> dbLanguages,
                                               ref IList<DbModel.TechnicalSpecialist> dbTechnicalpecialists,
                                               bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistLanguageCapabilityInfo> tslangCapabilityInfos,
                                            ValidationType validationType,
                                            IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos);

        Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndCustomerName,
                                    ref IList<DbModel.TechnicalSpecialistLanguageCapability> dbTslangCapabilityInfos,
                                    ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndStampNumber,
                                    ref IList<DbModel.TechnicalSpecialistLanguageCapability> dblangCapabilityInfos,
                                    ref IList<KeyValuePair<string, string>> tsPinAndLanguageNotExists,
                                    ref IList<ValidationMessage> validationMessages);
    }
}
