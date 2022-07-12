using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistInternalTrainingService
    {
        Response Get(TechnicalSpecialistInternalTraining searchModel);

        Response Get(IList<int> tsIntTrainingIds);

        Response GetByTSPin(IList<string> tsPins);

        Response Get(IList<string> intTrainingNames);

        Response Add(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                       bool commitChange = true,
                       bool isDbValidationRequired = true);

        Response Add(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                        ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings, ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings, bool commitChange = true, bool isDbValidationRequired = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                                            ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                                                ValidationType validationType,
                                                ref IList<TechnicalSpecialistInternalTraining> filteredTSInternalTrainings,
                                                ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, 
                                                bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                                        ValidationType validationType,
                                         ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistInternalTraining> tsInternalTrainings,
                                            ValidationType validationType,
                                              ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings);

        Response IsRecordExistInDb(IList<int> tsInternalTrainingIds,
                                       ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                                        ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<int> tsInternalTrainingIds,
                                          ref IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsInternalTrainings,
                                          ref IList<int> tsInternalTrainingIdNotExists,
                                          ref IList<ValidationMessage> validationMessages);
    }
}
