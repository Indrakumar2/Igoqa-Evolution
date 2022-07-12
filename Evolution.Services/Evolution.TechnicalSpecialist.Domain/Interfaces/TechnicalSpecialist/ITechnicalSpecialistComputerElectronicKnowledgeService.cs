using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistComputerElectronicKnowledgeService
    {
        
        Response Get(TechnicalSpecialistComputerElectronicKnowledgeInfo searchModel);

        Response Get(IList<int> Ids);

        Response GetByPinId(IList<string> pinIds);

        Response Get(IList<string> ComputerKnowledge);

        Response Add(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsSComputerKnowledgeInfos,
                            bool commitChange = true,
                            bool isValidationRequire = true);

        Response Add(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsSComputerKnowledgeInfos,
                     ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsComputerKnowledgeInfos,
                     ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                     ref IList<DbModel.Data> dbComputerElectronicKnowledge,
                     bool commitChange = true,
                     bool isDbValidationRequire = true);

        Response Modify(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsSComputerKnowledgeInfos,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Modify(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
                        ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsCompElecKnowledgeInfos,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbComputerElectronicKnowledge,
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Delete(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsSComputerKnowledgeInfos,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Delete(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
            ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComputerElectronicKnowledgeInfos,
                           bool commitChange = true,
                           bool isDbValidationRequire = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
                                                ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
                                         ValidationType validationType,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         ref IList<DbModel.Data> dbComputerElectronicKnowledge,
                                         ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsCompElecKnowledgeInfos,
                                         bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
                                         ValidationType validationType,                                      
                                         IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsCompElecKnowledgeInfos);

        Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndComputerKnowledge,
                                    ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsComputerKnowledgeInfos,
                                    ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndComputerKnowledge,
                                    ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsComputerKnowledgeInfos,
                                    ref IList<KeyValuePair<string, string>> tsPinAndComputerKnowledgeNotExists,
                                    ref IList<ValidationMessage> validationMessages);
    }
}
