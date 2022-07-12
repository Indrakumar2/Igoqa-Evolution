using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Responses;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistEducationalQualificationService
    {
        Response Get(TechnicalSpecialistEducationalQualificationInfo searchModel);

        Response Get(IList<int> EducationalIds);

        Response GetByPinId(IList<string> pinIds);

        Response Add(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Add(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
                        ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Country> dbCountry,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
                        ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Country> Country,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification, ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification, bool commitChange = true, bool isDbValidationRequired = true);

        //Response IsRecordValidForProcess(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
        //                                    ValidationType validationType);

        //Response IsRecordValidForProcess(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
        //                                    ValidationType validationType,
        //                                    ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,       
        //                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
        //                                    ref IList<DbModel.Country> Country);

        //Response IsRecordValidForProcess(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulification,
        //                                    ValidationType validationType,
        //                                   ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification);


        Response IsRecordValidForProcess(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulificationInfos,
                                           ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulificationInfos,
                                            ValidationType validationType,
                                            ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsEduQulificationInfos);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulificationInfos,
                                               ValidationType validationType,
                                               ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsEduQulification,
                                               ref IList<DbModel.Country> dbCountry,
                                               ref IList<DbModel.TechnicalSpecialist> dbTechnicalpecialists,
                                                bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistEducationalQualificationInfo> tsEduQulificationInfos,
                                            ValidationType validationType,
                                            IList<DbModel.TechnicalSpecialistEducationalQualification> dbTsEduQulificationInfos);


        Response IsRecordExistInDb(IList<int> tsEduQulificationIds,
                                        ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,
                                        ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<int> tsEduQulificationIds,
                                          ref IList<DbModel.TechnicalSpecialistEducationalQualification> dbtsEduQulification,
                                          ref IList<int> tsEduQulificationIdNotExists,
                                          ref IList<ValidationMessage> validationMessages);
    }
}
