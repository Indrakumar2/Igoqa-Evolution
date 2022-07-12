using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistTaxonomyService
    {        
        Response Get(TechnicalSpecialistTaxonomyInfo searchModel);

        Response IsTaxonomyHistoryExists(int  Epin); //D684

        Response Get(IList<int> taxonomyIds);

        Response GetByPinId(IList<string> pinIds);

        Response Add(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                     bool commitChange = true,
                     bool isDbValidationRequired = true);

        Response Add(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                        ref IList<DbModel.TechnicalSpecialistTaxonomy> dbtsTaxonomies,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbCategories,
                        ref IList<DbModel.TaxonomySubCategory> dbSubCategories,
                        ref IList<DbModel.TaxonomyService> dbTaxonomyService,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                        ref IList<DbModel.TechnicalSpecialistTaxonomy> dbtsTaxonomies,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbCategories,
                        ref IList<DbModel.TaxonomySubCategory> dbSubCategories,
                        ref IList<DbModel.TaxonomyService> dbTaxonomyService,                       
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
              ref IList<DbModel.TechnicalSpecialistTaxonomy> dbtsTaxonomies,
                     bool commitChange = true,
                     bool isDbValidationRequired = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                                            ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                                         ValidationType validationType,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         ref IList<DbModel.Data> dbCategories,
                                         ref IList<DbModel.TaxonomySubCategory> dbSubCategories,
                                         ref IList<DbModel.TaxonomyService> dbTaxonomyService,
                                         ref IList<DbModel.TechnicalSpecialistTaxonomy> dbtsTaxonomies,
                                         bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistTaxonomyInfo> tsTaxonomies,
                                         ValidationType validationType,
                                        
                                         IList<DbModel.TechnicalSpecialistTaxonomy> dbtsTaxonomies);

        Response IsRecordExistInDb(IList<int> tsTaxonomyIds,
                                        ref IList<DbModel.TechnicalSpecialistTaxonomy> dbtsTaxonomies,
                                        ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<int> tsTaxonomyIds,
                                          ref IList<DbModel.TechnicalSpecialistTaxonomy> dbtsTaxonomies,
                                          ref IList<int> tsPayScheduleIdNotExists,
                                          ref IList<ValidationMessage> validationMessages);

    }
}
