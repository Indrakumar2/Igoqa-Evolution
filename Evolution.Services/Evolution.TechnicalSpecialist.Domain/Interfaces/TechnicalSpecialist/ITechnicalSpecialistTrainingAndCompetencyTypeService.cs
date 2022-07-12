using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistTrainingAndCompetancyTypeService
    {
        Response Get(IList<int> tsIds);

        Response Get(TechnicalSpecialistInternalTrainingAndCompetencyType searchModel);

        Response Get(IList<string> intTypeNames);

        Response Add(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes,
            CompCertTrainingType triningOrCompetacyType,
                       bool commitChange = true,
                       bool isDbValidationRequired = true);

        Response Add(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes,
            CompCertTrainingType triningOrCompetacyType,
                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys,
                         ref IList<DbModel.Data> dbMasterIntTrainingAndCompetencys,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes,
            CompCertTrainingType triningOrCompetacyType,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes,
            CompCertTrainingType triningOrCompetacyType,
                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys,
                         ref IList<DbModel.Data> dbMasterIntTrainingAndCompetencys,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes,
                        CompCertTrainingType triningOrCompetacyType,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes, CompCertTrainingType triningOrCompetacyType, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes, bool commitChange = true, bool isDbValidationRequired = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes,
                                            ValidationType validationType,
                                            CompCertTrainingType triningOrCompetacyType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes,
                                                ValidationType validationType,
                                                CompCertTrainingType triningOrCompetacyType,
                                                ref IList<TechnicalSpecialistInternalTrainingAndCompetencyType> filteredTSTypes,
                                                ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                                ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys,
                                                ref IList<DbModel.Data> dbMasterIntTrainingAndCompetencys);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTrainingAndCompetencyType> tsTypes,
                                        ValidationType validationType,
                                        CompCertTrainingType triningOrCompetacyType,
                                         ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsIntTrainingAndCompetencys,
                                        ref IList<DbModel.Data> dbMasterIntTrainingAndCompetencys);

        Response IsRecordExistInDb(IList<int> tsTypeIds,
                                       ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                        ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<int> tsTypeIds,
                                       ref IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> dbTsIntTrainingAndCompetencyTypes,
                                          ref IList<int> tsIntTrainingAndCompetencyTypeIdNotExists,
                                          ref IList<ValidationMessage> validationMessages);

    }
      
}
