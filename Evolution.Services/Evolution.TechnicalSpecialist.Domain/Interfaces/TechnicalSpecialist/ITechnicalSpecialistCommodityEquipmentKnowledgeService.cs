using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialCommodityEquipmentKnowledgeService
    {
        Response Get(TechnicalSpecialistCommodityEquipmentKnowledgeInfo searchModel);

        Response Get(IList<int> Ids);

        Response GetByPinId(IList<string> pinIds);

        Response Get(IList<string> Commodity);

        Response GetByEquipmentKnowledge(IList<string> EquipmentKnowledge);

        Response Add(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                    bool commitChange = true,
                    bool isDbValidationRequired = true);

        Response Add(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                        ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbCommodities,
                        ref IList<DbModel.Data> dbEquipments,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                        ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbCommodities,
                        ref IList<DbModel.Data> dbEquipments,                      
                        bool commitChange = true,
                        bool isDbValidationRequired = true);
        Response Delete(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                      bool commitChange = true,
                      bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
            ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsCommdEquipmentKnowledge,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                            ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                            ValidationType validationType,
                                            ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbCommodities,
                                                ref IList<DbModel.Data> dbEquipments
                                                , bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                            ValidationType validationType,
                                            IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge);

        Response IsRecordExistInDb(IList<int> tsComdEqipKnowledgeIds,
                                        ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                        ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<int> tsComdEqipKnowledgeIds,
                                          ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                          ref IList<int> tsComdEqipKnowledgeIdNotExists,
                                          ref IList<ValidationMessage> validationMessages);
    }
}
