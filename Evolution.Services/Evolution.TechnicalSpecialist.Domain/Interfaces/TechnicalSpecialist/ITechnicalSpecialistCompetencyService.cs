using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistCompetencyService
    {
        Response Get(TechnicalSpecialistCompetency searchModel);

        Response Get(IList<int> tsCompetencyIds);

        Response GetByTSPin(IList<string> tsPins);

        Response Get(IList<string> competencyNames);

        Response Add(IList<TechnicalSpecialistCompetency>  tsCompetencies,
                       bool commitChange = true,
                       bool isDbValidationRequired = true);

        Response Add(IList<TechnicalSpecialistCompetency> tsCompetencies,
                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistCompetency> tsCompetencies,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistCompetency> tsCompetencies,
                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistCompetency> tsCompetencies, bool commitChange = true, bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistCompetency> tsCompetencies,
             ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCompetency> tsCompetencies,
                                            ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCompetency> tsCompetencies,
                                                ValidationType validationType,
                                                ref IList<TechnicalSpecialistCompetency> filteredTSCompetencies,
                                                ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCompetency> tsCompetencies,
                                        ValidationType validationType,
                                         ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCompetency> tsCompetencies,
                                            ValidationType validationType,
                                            ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies);

        Response IsRecordExistInDb(IList<int> tsCompetencyIds,
                                       ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                                        ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<int> tsCompetencyIds,
                                          ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsCompetencies,
                                          ref IList<int> tsCompetencyIdNotExists,
                                          ref IList<ValidationMessage> validationMessages);
    }
}
