using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistTrainingService
    {  
        Response Get(TechnicalSpecialistTraining searchModel);

        Response Get(IList<int> tsTrainingIds);

        Response GetByTSPin(IList<string> tsPins);

        Response Get(IList<string> trainingNames);

        Response Add(IList<TechnicalSpecialistTraining> tsTrainings,
                       bool commitChange = true,
                       bool isDbValidationRequired = true);

        Response Add(IList<TechnicalSpecialistTraining> tsTrainings,
                        ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbTrainingTypes,
                        ref IList<DbModel.User> dbVarifiedByUsers,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistTraining> tsTrainings,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistTraining> tsTrainings,
                        ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbTrainingTypes,
                        ref IList<DbModel.User> dbVarifiedByUsers,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistTraining> tsTrainings,
                               bool commitChange = true,
                               bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistTraining> tsTrainings,
            ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistTraining> tsTrainings,
                                            ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistTraining> tsTrainings,
                                                ValidationType validationType,
                                                ref IList<TechnicalSpecialistTraining> filteredTSTrainings,
                                                ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbCertificationTypes,
                                                ref IList<DbModel.User> dbVarifiedByUsers,
                                                 bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistTraining> tsTrainings,
                                        ValidationType validationType, 
                                        ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                        ref IList<DbModel.Data> dbCertificationTypes,
                                        ref IList<DbModel.User> dbVarifiedByUsers,
                                         bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistTraining> tsTrainings,
                                            ValidationType validationType,
                                            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings);

        Response IsRecordExistInDb(IList<int> tsTrainingIds,
                                        ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                        ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<int> tsTrainingIds,
                                          ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsTrainings,
                                          ref IList<int> tsTrainingIdNotExists,
                                          ref IList<ValidationMessage> validationMessages);

    }
}
