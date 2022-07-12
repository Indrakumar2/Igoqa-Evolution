using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistService
    {
        Response GetResourceBasicInfo(string companyCode,string logonName = null);

        Response Get(BaseTechnicalSpecialistInfo searchModel);

        Task<Response> Get(SearchTechnicalSpecialist searchModel);

        Response Get(TechnicalSpecialistInfo searchModel);

        Response Get(IList<string> tsPins);

        Response GetTechnicalSpecialistChevronCV(long techspecialistEpin);

        Response GetTechnicalSpecialistExportCV(long techspecialistEpin);

        Response Add(IList<TechnicalSpecialistInfo> tsInfos,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Add(IList<TechnicalSpecialistInfo> tsInfos,
                        ref IList<DbModel.TechnicalSpecialist> dbTsInfos,

                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Add(IList<TechnicalSpecialistInfo> tsInfos,
                        ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                        IList<DbModel.Company> dbCompanies,
                        IList<DbModel.CompanyPayroll> dbCompPayrolls,
                        IList<DbModel.Data> dbSubDivisions,
                        IList<DbModel.Data> dbStatuses,
                        IList<DbModel.Data> dbActions,
                        IList<DbModel.Data> dbEmploymentTypes,
                        IList<DbModel.Country> dbCountries,
                        IList<DbModel.User> dbLoginUser,
                         ref long? eventId,
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Modify(IList<TechnicalSpecialistInfo> tsInfos,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Modify(IList<TechnicalSpecialistInfo> tsInfos,
                        ref IList<DbModel.TechnicalSpecialist> dbTsInfos,

                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Modify(IList<TechnicalSpecialistInfo> tsInfos,
                        ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                        IList<DbModel.Company> dbCompanies,
                        IList<DbModel.CompanyPayroll> dbCompPayrolls,
                        IList<DbModel.Data> dbSubDivisions,
                        IList<DbModel.Data> dbStatuses,
                        IList<DbModel.Data> dbActions,
                        IList<DbModel.Data> dbEmploymentTypes,
                        IList<DbModel.Country> dbCountries,
                        IList<DbModel.User> dbLoginUser,
                         ref long? eventId,
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Delete(IList<TechnicalSpecialistInfo> tsInfos,
                        bool commitChange = true,
                        bool isValidationRequire = true);

        Response Delete(IList<TechnicalSpecialistInfo> tsInfos,
                      ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                      ref long? eventId,
                      bool commitChange = true,
                      bool isDbValidationRequire = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistInfo> tsInfos,
                                            ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistInfo> tsInfos,
                                            ValidationType validationType,
                                            ref IList<DbModel.TechnicalSpecialist> dbTsInfos);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistInfo> tsInfos,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                                ref IList<DbModel.Company> dbCompanies,
                                                ref IList<DbModel.CompanyPayroll> dbCompPayrolls,
                                                ref IList<DbModel.Data> dbSubDivisions,
                                                ref IList<DbModel.Data> dbStatuses,
                                                ref IList<DbModel.Data> dbActions,
                                                ref IList<DbModel.Data> dbEmploymentTypes,
                                                ref IList<DbModel.Country> dbCountries,
                                                bool isDraft=false);

        Response IsRecordExistInDb(IList<string> tsPins,
                                   ref IList<DbModel.TechnicalSpecialist> dvTsInfos,
                                   ref IList<ValidationMessage> validationMessages,
                                   params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes);

        Response IsRecordExistInDb(IList<string> tsPins,
                                 ref IList<DbModel.TechnicalSpecialist> dvTsInfos,
                                 ref IList<ValidationMessage> validationMessages,
                                 string[] includes);

        Response IsRecordExistInDb(IList<string> tsPins,
                                         ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                         ref IList<string> tsPinNotExists,
                                         ref IList<ValidationMessage> validationMessages,
                                         string[] includes);

        Response IsRecordExistInDb(IList<string> tsPins,
                                   ref IList<DbModel.TechnicalSpecialist> dbTsInfosInfos,
                                   ref IList<string> tsPinNotExists,
                                   ref IList<ValidationMessage> validationMessages,
                                   params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes);

        Response IsRecordExistInDbById(IList<int> tsIds,
                                   ref IList<DbModel.TechnicalSpecialist> dvTsInfos,
                                   ref IList<ValidationMessage> validationMessages,
                                   params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes);
        //D946 CR Start
        Response Modify(List<KeyValuePair<DbModel.TechnicalSpecialist, List<KeyValuePair<string, object>>>> technicalSpecialist, params Expression<Func<DbModel.TechnicalSpecialist, object>>[] updatedProperties);

        Response GetTSBasedOnCompany(IList<int> companyCode, bool isActive);
    }
}
