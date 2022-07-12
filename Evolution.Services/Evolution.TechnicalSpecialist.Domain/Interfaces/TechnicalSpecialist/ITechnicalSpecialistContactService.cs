using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistContactService
    {
        Response Get(TechnicalSpecialistContactInfo searchModel);

        Response Get(TechnicalSpecialistContactInfo searchModel, int takeCount);

        Response Get(IList<int> contactIds);

        Response GetByPinId(IList<string> pinIds);

        Response GetByPinAndContactType(IList<string> pins, IList<string> contactTypes);

        Response GetByPinAndContactType(IList<int> pins, IList<string> contactTypes);

        Response Add(IList<TechnicalSpecialistContactInfo> tsContacts,
                     bool commitChange = true,
                     bool isDbValidationRequired = true);

        Response Add(IList<TechnicalSpecialistContactInfo> tsContacts,
                        ref IList<DbModel.TechnicalSpecialistContact> dbTsContacts,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Country> dbCountries,
                        ref IList<DbModel.County> dbcounties,
                        ref IList<DbModel.City> dbcities,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistContactInfo> tsContacts,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Modify(IList<TechnicalSpecialistContactInfo> tsContacts,
                        ref IList<DbModel.TechnicalSpecialistContact> dbTsContacts,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Country> dbCountries,
                        ref IList<DbModel.County> dbcounties,
                        ref IList<DbModel.City> dbcities,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistContactInfo> tsContacts,
                        bool commitChange = true,
                        bool isDbValidationRequired = true);

        Response Delete(IList<TechnicalSpecialistContactInfo> tsContacts,
           ref IList<DbModel.TechnicalSpecialistContact> dbTsContacts,
                              bool commitChange = true,
                              bool isDbValidationRequired = true);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistContactInfo> tsContacts,
                                         ValidationType validationType);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistContactInfo> tsContacts,
                                         ValidationType validationType,
                                         ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                         ref IList<DbModel.Country> dbCountries,
                                         ref IList<DbModel.County> dbcounties,
                                         ref IList<DbModel.City> dbcities,
                                         ref IList<DbModel.TechnicalSpecialistContact> dbTsContacts,
                                         bool isDraft = false);

        Response IsRecordValidForProcess(IList<TechnicalSpecialistContactInfo> tsContacts,
                                         ValidationType validationType,
                                         IList<DbModel.TechnicalSpecialistContact> dbTsContacts);

        Response IsRecordExistInDb(IList<int> tsContactIds,
                                        ref IList<DbModel.TechnicalSpecialistContact> dbTsContacts,
                                        ref IList<ValidationMessage> validationMessages);

        Response IsRecordExistInDb(IList<int> tsContactIds,
                                          ref IList<DbModel.TechnicalSpecialistContact> dbTsContacts,
                                          ref IList<int> tsContactIdNotExists,
                                          ref IList<ValidationMessage> validationMessages);

        Response UpdateContactSyncStatus(IList<int> tsContactIds);
    }
}
