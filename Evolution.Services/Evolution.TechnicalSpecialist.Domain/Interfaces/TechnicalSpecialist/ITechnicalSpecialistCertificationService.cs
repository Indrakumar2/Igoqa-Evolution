using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistCertificationService
    {
        Response Get(TechnicalSpecialistCertification searchModel);

        Response Get(IList<int> tsCertificationIds);

        Response GetByTSPin(IList<string> tsPins);

        Response Get(IList<string> CertificationName);

        Response Add(IList<TechnicalSpecialistCertification> tsCertifications,
                       bool commitChange = true,
                       bool isDbValidationRequired = true);

        Response Add(IList<TechnicalSpecialistCertification> tsCertifications,
                        ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbCertificationTypes,
                        ref IList<DbModel.User> dbVarifiedByUsers,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistCertification> tsCertifications,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistCertification> tsCertifications,
                        ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbCertificationTypes,
                        ref IList<DbModel.User> dbVarifiedByUsers,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);


        Response Delete(IList<TechnicalSpecialistCertification> tsCertifications, bool commitChange = true, bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistCertification> tsCertifications,
             ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCertification> tsCertifications,
                                            ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCertification> tsCertifications,
                                                ValidationType validationType,
                                                ref IList<TechnicalSpecialistCertification> filteredTSCertifications,
                                                ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbCertificationTypes,
                                                ref IList<DbModel.User> dbVarifiedByUsers);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCertification> tsCertifications,
                                        ValidationType validationType, 
                                        ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                        ref IList<DbModel.Data> dbCertificationTypes,
                                        ref IList<DbModel.User> dbVarifiedByUsers,
                                        bool isDraft=false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistCertification> tsCertifications,
                                            ValidationType validationType,
                                            IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications);

        Response IsRecordExistInDb(IList<int> tsCertificationIds,
                                        ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                        ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<int> tsCertificationIds,
                                          ref IList<DbModel.TechnicalSpecialistCertificationAndTraining> dbTsCertifications,
                                          ref IList<int> tsCertificationIdNotExists,
                                          ref IList<ValidationMessage> validationMessages);
    }
}
