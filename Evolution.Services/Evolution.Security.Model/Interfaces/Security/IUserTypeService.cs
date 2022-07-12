using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Security.Domain.Models.Security;

namespace Evolution.Security.Domain.Interfaces.Security
{
    public interface IUserTypeService
    {

        Response Get(DomainModel.UserTypeInfo searchModel);

        Response Get(IList<int> userIds);

        Response Get(DomainModel.UserTypeInfo searchModel, params string[] includes);

        Response Get(string companyCode, string userLoginName, params string[] includes);

        Response Get(string companyCode, IList<string> userTypes, params string[] includes);

        Response Get(int companyId, IList<string> userTypes, params string[] includes);

        Response GetUsers(string companyCode, IList<string> userTypes);

        Response IsUserTypeExistInDb(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                        ref IList<DomainModel.UserTypeInfo> userTypeNotExists,
                                        ref IList<DbModel.UserType> dbUserTypes);

        Response Add(IList<DomainModel.UserTypeInfo> userTypeInfos,
                        ref long? eventId, // Manage Security Audit changes
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Add(IList<DomainModel.UserTypeInfo> userTypeInfos,
                        ref IList<DbModel.UserType> dbUserTypes,
                        ref IList<DbModel.User> dbUsers,
                        ref IList<DbModel.Company> dbCompany,
                        ref long? eventId, // Manage Security Audit changes
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Modify(IList<DomainModel.UserTypeInfo> userTypeInfos,
                           ref long? eventId, // Manage Security Audit changes
                           bool commitChange = true,
                           bool isDbValidationRequire = true);

        Response Modify(IList<DomainModel.UserTypeInfo> userTypeInfos,
                            ref IList<DbModel.UserType> dbUserTypes,
                            ref IList<DbModel.User> dbUsers,
                            ref IList<DbModel.Company> dbCompany,
                            ref long? eventId, // Manage Security Audit changes
                            bool commitChange = true,
                            bool isDbValidationRequire = true);

        Response Delete(IList<DomainModel.UserTypeInfo> userTypeInfos,
                        ref long? eventId, // Manage Security Audit changes
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response Delete(IList<DomainModel.UserTypeInfo> userTypeInfos,
                        ref IList<DbModel.UserType> dbUserTypes,
                        ref IList<DbModel.User> dbUsers,
                        ref IList<DbModel.Company> dbCompany,
                        ref long? eventId, // Manage Security Audit changes
                        bool commitChange = true,
                        bool isDbValidationRequire = true);

        Response IsRecordValidForProcess(IList<DomainModel.UserTypeInfo> userTypeInfos, ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                            ValidationType validationType,
                                            ref IList<DbModel.UserType> dbUserTypes,
                                            ref IList<DbModel.User> dbUsers,
                                            ref IList<DbModel.Company> dbCompany);

        Response IsRecordUpdateCountMatching(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                          IList<DbModel.UserType> dbUserTypes,
                                          ref IList<ValidationMessage> validationMessages);

    }
}
