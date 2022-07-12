using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Logging.Interfaces;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Interfaces.Validations;
using Evolution.Security.Domain.Models.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Security.Domain.Models.Security;

namespace Evolution.Security.Core.Services
{
    public class UserTypeService : IUserTypeService
    {
        private readonly IAppLogger<UserTypeService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IUserTypeRepository _repository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IUserService _userService = null;
        private readonly IUserTypeValidationService _validationService = null;
        private readonly ICompanyService _companyService = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly IAuditSearchService _auditSearchService = null;

        #region Constructor

        public UserTypeService(IMapper mapper,
                                IUserTypeRepository repository,
                                IAppLogger<UserTypeService> logger,
                                IUserService userService,
                                ICompanyService companyService,
                                IUserTypeValidationService validationService,
                                JObject messgaes,
                                IOptions<AppEnvVariableBaseModel> environment,
                                IAuditSearchService auditSearchService)
        {
            this._repository = repository;
            this._logger = logger;
            this._mapper = mapper;
            this._validationService = validationService;
            this._companyService = companyService;
            this._userService = userService;
            this._messageDescriptions = messgaes;
            _environment = environment.Value;
            _auditSearchService = auditSearchService;
        }

        #endregion

        #region Public Metods

        public Response Add(IList<UserTypeInfo> userTypeInfos, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true) // Manage Security Audit changes
        {
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.UserType> dbUserTypes = null;
            IList<DbModel.Company> dbCompany = null;

            return AddUserType(userTypeInfos, ref dbUserTypes, ref dbUsers, ref dbCompany, ref eventId, commitChange, isDbValidationRequire); // Manage Security Audit changes
        }

        public Response Add(IList<UserTypeInfo> userTypeInfos, ref IList<DbModel.UserType> dbUserTypes, ref IList<DbModel.User> dbUsers, ref IList<DbModel.Company> dbCompany, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true) // Manage Security Audit changes
        {
            return AddUserType(userTypeInfos, ref dbUserTypes, ref dbUsers, ref dbCompany, ref eventId, commitChange, isDbValidationRequire); // Manage Security Audit changes
        }

        public Response Modify(IList<DomainModel.UserTypeInfo> userTypeInfos,
                           ref long? eventId, // Manage Security Audit changes
                           bool commitChange = true,
                           bool isDbValidationRequire = true)
        {
            IList<DbModel.UserType> dbUserTypes = null;
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.Company> dbCompany = null;
            return Modify(userTypeInfos, ref dbUserTypes, ref dbUsers, ref dbCompany, ref eventId, commitChange, isDbValidationRequire); // Manage Security Audit changes
        }
        public Response Modify(IList<DomainModel.UserTypeInfo> userTypeInfos,
                            ref IList<DbModel.UserType> dbUserTypes,
                            ref IList<DbModel.User> dbUsers,
                            ref IList<DbModel.Company> dbCompany,
                            ref long? eventId, // Manage Security Audit changes
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            return ModifyUserType(userTypeInfos, ref dbUserTypes, ref dbUsers, ref dbCompany, ref eventId, commitChange, isDbValidationRequire); // Manage Security Audit changes
        }

        public Response Delete(IList<UserTypeInfo> userTypeInfos, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true) // Manage Security Audit changes
        {
            IList<DbModel.UserType> dbUserTypes = null;
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.Company> dbCompany = null;

            return RemoveUserType(userTypeInfos, ref dbUserTypes,ref dbUsers, ref dbCompany, ref eventId, commitChange, isDbValidationRequire); // Manage Security Audit changes
        }

        public Response Delete(IList<UserTypeInfo> userTypeInfos, ref IList<DbModel.UserType> dbUserTypes, ref IList<DbModel.User> dbUsers, ref IList<DbModel.Company> dbCompany, ref long? eventId, bool commitChange = true, bool isDbValidationRequire = true) // Manage Security Audit changes
        {
            return RemoveUserType(userTypeInfos, ref dbUserTypes, ref dbUsers, ref dbCompany, ref eventId, commitChange, isDbValidationRequire); // Manage Security Audit changes
        }

        public Response Get(UserTypeInfo searchModel)
        {
            IList<DomainModel.UserTypeInfo> result = null;
            Exception exception = null;
            try
            {  
                result = _mapper.Map<IList<DomainModel.UserTypeInfo>>(this._repository.Search(searchModel, new string[] { "Company", "User" }).ToList());
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> userIds)
        {
            IList<DomainModel.UserTypeInfo> result = null;
            Exception exception = null;
            try
            {
                if (userIds?.Count > 0)
                    result = _mapper.Map<IList<DomainModel.UserTypeInfo>>(this._repository.FindBy(x => userIds.Contains(x.UserId)).ToList());
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(UserTypeInfo searchModel, params string[] includes)
        {
            IList<DbModel.UserType> result = null;
            Exception exception = null;
            try
            { 
                result = this._repository.Search(searchModel, includes).ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            } 
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(string companyCode, string userLoginName, params string[] includes)
        {
            IList<DbModel.UserType> result = null;
            Exception exception = null;
            try
            {
                result = this._repository.Search(new UserTypeInfo { CompanyCode = companyCode, UserLogonName = userLoginName });
                 
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode, userLoginName);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
         
        public Response Get(string companyCode, IList<string> userTypes, params string[] includes)
        {
            IList<DbModel.UserType> result = null;
            Exception exception = null;
            try
            {
                result = this._repository.Get(companyCode, userTypes, includes).ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode, userTypes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(int companyId, IList<string> userTypes, params string[] includes)
        {
            IList<DbModel.UserType> result = null;
            Exception exception = null;
            try
            {
                result = this._repository.Get(companyId, userTypes, includes).ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyId, userTypes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }


        public Response GetUsers(string companyCode, IList<string> userTypes)
        {
            IList<DbModel.User> result = null;
            Exception exception = null;
            try
            { 
                result = this._repository.Get(companyCode, userTypes, new string[] { "User" })?.Select(x=>x.User).ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode, userTypes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
         
        public Response IsRecordValidForProcess(IList<UserTypeInfo> userTypeInfos, ValidationType validationType)
        {
            IList<DomainModel.UserTypeInfo> filteredUserTypes = null;
            IList<DbModel.User> dbUsers = null;
            IList<DbModel.UserType> dbUserTypes = null; 
            IList<DbModel.Company> dbCompany = null; 
            return CheckRecordValidForProcess(userTypeInfos, validationType, ref filteredUserTypes, ref dbUserTypes, ref dbUsers, ref dbCompany);
        }

        public Response IsRecordValidForProcess(IList<UserTypeInfo> userTypeInfos, ValidationType validationType, ref IList<DbModel.UserType> dbUserTypes, ref IList<DbModel.User> dbUsers, ref IList<DbModel.Company> dbCompany)
        {
            IList<DomainModel.UserTypeInfo> filteredUserTypes = null;
            return CheckRecordValidForProcess(userTypeInfos, validationType, ref filteredUserTypes, ref dbUserTypes, ref dbUsers, ref dbCompany);
        }

        public Response IsUserTypeExistInDb(IList<UserTypeInfo> userTypeInfos, ref IList<UserTypeInfo> userTypeNotExists, ref IList<DbModel.UserType> dbUserTypes)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                result = IsUserTypeExistInDb(userTypeInfos, ref userTypeNotExists, ref dbUserTypes, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userTypeInfos);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        
        public Response IsRecordUpdateCountMatching(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                          IList<DbModel.UserType> dbUserTypes,
                                          ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                result = IsRecordUpdateCountMatch(userTypeInfos, dbUserTypes, ref validationMessages);
            }
            catch(Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userTypeInfos);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        #endregion

        #region Private Metods

        private bool IsUserTypeExistInDb(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                            ref IList<DomainModel.UserTypeInfo> userTypeNotExists,
                                            ref IList<DbModel.UserType> dbUserTypes,
                                            ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;
            if (userTypeInfos?.Count > 0)
            {
                var userLogonNames = userTypeInfos.Select(x => x.UserLogonName).Distinct().ToList();
                var userRoleNames = userTypeInfos.Select(x => x.UserType).Distinct().ToList();
                var compCodes = userTypeInfos.Select(x => x.CompanyCode).Distinct().ToList();
                if (dbUserTypes == null || dbUserTypes.Count <= 0)
                    dbUserTypes = _repository.FindBy(x => userLogonNames.Contains(x.User.SamaccountName) &&
                                                          userRoleNames.Contains(x.UserTypeName) &&
                                                          compCodes.Contains(x.Company.Code))
                                             .Include(x => x.Company)
                                             .Include(x => x.User)
                                             .ToList();

                var dbUTypes = dbUserTypes;
                userTypeNotExists = userTypeInfos.Where(x => !dbUTypes.Any(x1 => x1.Company.Code == x.CompanyCode &&
                                                                                    x1.User.SamaccountName == x.UserLogonName &&
                                                                                    x1.UserTypeName == x.UserType))
                                                 .Select(x => x).ToList();

                userTypeNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.UserTypeNotExist, x.UserLogonName, x.UserType, x.CompanyCode);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }
   

        private Response AddUserType(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                ref IList<DbModel.UserType> dbUserTypes,
                                ref IList<DbModel.User> dbUsers,
                                ref IList<DbModel.Company> dbCompany,
                                ref long? eventId, // Manage Security Audit changes
                                bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventID = 0; // Manage Security Audit changes
            try
            {
                Response valdResponse = null;
                IList<DomainModel.UserTypeInfo> recordToBeAdd = null;
                eventID = userTypeInfos?.FirstOrDefault()?.EventId; // Manage Security Audit changes
                if (userTypeInfos?.Count > 0)
                {
                    if (isDbValidationRequire)
                        valdResponse = CheckRecordValidForProcess(userTypeInfos, ValidationType.Add, ref recordToBeAdd, ref dbUserTypes, ref dbUsers, ref dbCompany);

                    if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                    {
                        IList<DbModel.User> dbUser = dbUsers;
                        IList<DbModel.Company> dbComps = dbCompany;
                        var mappedRecordToBeAdd = _mapper.Map<IList<DbModel.UserType>>(recordToBeAdd, opt =>
                        {
                            opt.Items["dbUser"] = dbUser;
                            opt.Items["dbCompany"] = dbComps;
                        });
                        mappedRecordToBeAdd = mappedRecordToBeAdd
                                              .Select(x =>
                                              {
                                                  x.Id = 0;
                                                  x.LastModification = null;
                                                  x.UpdateCount = null;
                                                  x.ModifiedBy = null;
                                                  return x;
                                              }).ToList();
                        //FInding distinct record.
                        mappedRecordToBeAdd = mappedRecordToBeAdd.GroupBy(x => new { x.CompanyId, x.UserId, x.UserTypeName })
                                                                 .Select(grp => grp.FirstOrDefault()).ToList();
                        _repository.AutoSave = false;
                        _repository.Add(mappedRecordToBeAdd);
                        if (commitChange)
                        {
                            int value = _repository.ForceSave();
                            dbUserTypes = mappedRecordToBeAdd;

                            // Manage Security Audit changes - Starts.
                            if(value > 0 && mappedRecordToBeAdd?.Count > 0 && recordToBeAdd?.Count > 0)
                            {
                                mappedRecordToBeAdd?.ToList().ForEach(x =>_auditSearchService.AuditLog(recordToBeAdd.FirstOrDefault(), ref eventID, recordToBeAdd.FirstOrDefault()?.ActionByUser,
                                                                                                   null,
                                                                                                   ValidationType.Add.ToAuditActionType(),
                                                                                                   SqlAuditModuleType.UserUserType,
                                                                                                   null,
                                                                                                   _mapper.Map<UserTypeInfo>(x)));
                                eventId = eventID;
                            }
                            // Manage Security Audit changes - Ends.
                        }
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userTypeInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response ModifyUserType(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                      ref IList<DbModel.UserType> dbUserTypes,
                                      ref IList<DbModel.User> dbUsers,
                                      ref IList<DbModel.Company> dbCompanies,
                                      ref long? eventId, // Manage Security Audit changes
                                      bool commitChange = true,
                                      bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventID = 0; // Manage Security Audit changes
            try
            {
                Response valdResponse = null;
                IList<DomainModel.UserTypeInfo> recordToModify = null;
                eventID = userTypeInfos?.FirstOrDefault()?.EventId; // Manage Security Audit changes
                if (userTypeInfos.Count > 0)
                {
                    if (isDbValidationRequire)
                        valdResponse = CheckRecordValidForProcess(userTypeInfos,
                                                                  ValidationType.Update,
                                                                  ref recordToModify,
                                                                  ref dbUserTypes,
                                                                  ref dbUsers,
                                                                  ref dbCompanies);
                    if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                    {
                        // Manage Security Audit changes - starts
                        IList<UserTypeInfo> domUserTypeInfo = new List<UserTypeInfo>();
                        dbUserTypes?.ToList().ForEach(x =>
                        {
                            domUserTypeInfo.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.UserTypeInfo>(x)));
                        });
                        // Manage Security Audit changes - Ends
                        var dbUserType = dbUserTypes;
                        recordToModify.ToList().ForEach(x =>
                        {
                            var dbRecordToModify = dbUserType.FirstOrDefault(x1 => x1.Company.Code == x.CompanyCode
                                                                              && x1.User.SamaccountName == x.UserLogonName
                                                                              && x1.UserTypeName == x.UserType);
                            dbRecordToModify.IsActive = x.IsActive;
                            _repository.Update(dbRecordToModify);
                        });
                        if (commitChange)
                        {
                            int value = _repository.ForceSave();
                            // Manage Security Audit changes - starts
                            if(value > 0 && dbUserType?.Count > 0 && recordToModify?.Count > 0)
                            {
                                dbUserType?.ToList().ForEach(x => _auditSearchService.AuditLog(recordToModify.FirstOrDefault(), ref eventID, recordToModify.FirstOrDefault()?.ActionByUser,
                                                                                                null,
                                                                                                ValidationType.Update.ToAuditActionType(),
                                                                                                SqlAuditModuleType.UserUserType,
                                                                                                _mapper.Map<UserTypeInfo>(domUserTypeInfo?.FirstOrDefault(x2 => x2.UserType == x.UserTypeName)),
                                                                                                x
                                                                                                ));
                                eventId = eventID;
                            }
                            // Manage Security Audit changes - Ends
                        }
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userTypeInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }


        private Response RemoveUserType(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                        ref IList<DbModel.UserType> dbUserTypes,
                                        ref IList<DbModel.User> dbUsers,
                                        ref IList<DbModel.Company> dbCompany,
                                        ref long? eventId, // Manage Security Audit changes
                                        bool commitChange,
                                        bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<DomainModel.UserTypeInfo> filteredUserTypes = null;
            long? eventID = 0; // Manage Security Audit changes
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventID = userTypeInfos?.FirstOrDefault()?.EventId; // Manage Security Audit changes
                Response response = null;
                if (isDbValidationRequire)
                    response = CheckRecordValidForProcess(userTypeInfos,
                                                          ValidationType.Delete,
                                                          ref filteredUserTypes, 
                                                          ref dbUserTypes,
                                                          ref dbUsers, 
                                                          ref dbCompany);

                if (!isDbValidationRequire || (response.Code == ResponseType.Success.ToId() && Convert.ToBoolean(response.Result)))
                {
                    _repository.AutoSave = false;
                    dbUserTypes = dbUserTypes.Where(x => filteredUserTypes.Any(x1 => x1.RecordStatus.IsRecordStatusDeleted() 
                                                                                && x1.CompanyCode == x.Company.Code
                                                                                && x1.UserLogonName == x.User.SamaccountName
                                                                                && x1.UserType == x.UserTypeName)).ToList();
                    //dbUserTypes = dbUserTypes.GroupBy(x => new {x.CompanyId, x.UserId, x.UserTypeName })
                    //                         .Select(grp => grp.FirstOrDefault()).ToList();
                    _repository.Delete(dbUserTypes);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        // Manage Security Audit changes - starts
                        if(value > 0 && dbUserTypes?.Count > 0 && filteredUserTypes?.Count > 0)
                        {
                            filteredUserTypes?.ToList().ForEach(x => _auditSearchService.AuditLog(x, ref eventID, x.ActionByUser,
                                                                                                  null,
                                                                                                  ValidationType.Delete.ToAuditActionType(),
                                                                                                  SqlAuditModuleType.UserUserType,
                                                                                                  x,
                                                                                                  null));
                            eventId = eventID;
                        }
                        // Manage Security Audit changes - Ends
                    }
                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userTypeInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                            ValidationType validationType,
                                            ref IList<DomainModel.UserTypeInfo> filteredUserType,
                                            ref IList<DbModel.UserType> dbUserTypes,
                                            ref IList<DbModel.User> dbUsers,
                                            ref IList<DbModel.Company> dbCompany)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(userTypeInfos, ref filteredUserType, ref dbUserTypes, ref dbUsers, ref dbCompany, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(userTypeInfos, ref filteredUserType, ref dbUserTypes, ref dbUsers, ref dbCompany, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(userTypeInfos, ref filteredUserType, ref dbUserTypes, ref dbUsers, ref dbCompany, ref validationMessages);

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), userTypeInfos);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }


        private bool IsRecordValidForAdd(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                 ref IList<DomainModel.UserTypeInfo> filteredUserTypes,
                                 ref IList<DbModel.UserType> dbUserTypes,
                                 ref IList<DbModel.User> dbUsers,
                                 ref IList<DbModel.Company> dbCompany,
                                 ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            if (userTypeInfos != null && userTypeInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredUserTypes == null || filteredUserTypes.Count <= 0)
                    filteredUserTypes = FilterRecord(userTypeInfos, validationType);

                if (filteredUserTypes?.Count > 0 && IsValidPayload(filteredUserTypes, validationType, ref validationMessages))
                {
                    IList<ValidationMessage> messages = new List<ValidationMessage>();
                    var valdResponse = this.IsMasterDataValid(filteredUserTypes, ref dbUsers, ref dbCompany, ref validationMessages);
                    if (valdResponse.Code == ResponseType.Success.ToId())
                    {
                        IList<DomainModel.UserTypeInfo> userTypeNotExists = null;
                        var response = (!IsUserTypeExistInDb(filteredUserTypes, ref userTypeNotExists, ref dbUserTypes, ref messages) && messages.Count == filteredUserTypes.Count);
                        if (!response)
                        {
                            var userRoleAlreadyExists = filteredUserTypes.Where(x => !userTypeNotExists.Any(x1 => x1.CompanyCode == x.CompanyCode && x1.UserLogonName == x.UserLogonName && x1.UserType == x.UserType)).ToList();
                            userRoleAlreadyExists?.ForEach(x =>
                            {
                                messages.Add(_messageDescriptions, x, MessageType.UserTypeAlreadyExist, x.UserType, x.UserLogonName);
                            });

                            if (messages.Count > 0)
                            {
                                result = false;
                                validationMessages.AddRange(messages);
                            }
                        }
                    }
                    else
                    {
                        validationMessages = valdResponse.ValidationMessages;
                        result = false;
                    }
                }
                else
                    result = false;
            }

            return result;
        }

        private bool IsRecordValidForUpdate(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                          ref IList<DomainModel.UserTypeInfo> filteredUserTypes,
                                          ref IList<DbModel.UserType> dbUserTypes,
                                          ref IList<DbModel.User> dbUsers,
                                          ref IList<DbModel.Company> dbCompany,
                                          ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            ValidationType validationType = ValidationType.Update;
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            if (filteredUserTypes == null || filteredUserTypes.Count <= 0)
                filteredUserTypes = FilterRecord(userTypeInfos, validationType);
            if (filteredUserTypes?.Count > 0 && IsValidPayload(filteredUserTypes, validationType, ref validationMessages))
            {
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                var valdResponse = this.IsMasterDataValid(filteredUserTypes, ref dbUsers, ref dbCompany, ref validationMessages);
                if (valdResponse.Code == ResponseType.Success.ToId())
                {
                    IList<DomainModel.UserTypeInfo> userTypeNotExists = null;
                    result = (IsUserTypeExistInDb(filteredUserTypes, ref userTypeNotExists, ref dbUserTypes, ref validationMessages)
                                && IsRecordUpdateCountMatch(filteredUserTypes, dbUserTypes, ref validationMessages));
                }
                else
                {
                    validationMessages = valdResponse.ValidationMessages;
                    result = false;
                }
            }
            else
                result = false;
            return result;
        }


        private bool IsRecordValidForRemove(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                    ref IList<DomainModel.UserTypeInfo> filteredUserTypes,
                                    ref IList<DbModel.UserType> dbUserTypes,
                                    ref IList<DbModel.User> dbUsers,
                                    ref IList<DbModel.Company> dbCompany,
                                    ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            if (userTypeInfos != null && userTypeInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredUserTypes == null || filteredUserTypes.Count <= 0)
                    filteredUserTypes = FilterRecord(userTypeInfos, validationType);

                if (filteredUserTypes?.Count > 0 && IsValidPayload(filteredUserTypes, validationType, ref validationMessages))
                {
                    IList<ValidationMessage> messages = new List<ValidationMessage>();
                    var valdResponse = this.IsMasterDataValid(filteredUserTypes, ref dbUsers, ref dbCompany, ref validationMessages);
                    if (valdResponse.Code == ResponseType.Success.ToId())
                    {
                        IList<DomainModel.UserTypeInfo> userTypeNotExists = null;
                        result = IsUserTypeExistInDb(filteredUserTypes, ref userTypeNotExists, ref dbUserTypes, ref validationMessages);
                    }
                    else
                    {
                        validationMessages = valdResponse.ValidationMessages;
                        result = false;
                    }
                }
                else
                    result = false;
            }

            return result;
        }

        private Response IsMasterDataValid(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                            ref IList<DbModel.User> dbUsers,
                                            ref IList<DbModel.Company> dbCompany,
                                            ref IList<ValidationMessage> validationMessages)
        {
            Response result = new Response().ToPopulate(ResponseType.Success);
            if (userTypeInfos?.Count() > 0)
            {
                IList<string> userNotExists = null;

                var appUserLogonNames = userTypeInfos.Select(x => new KeyValuePair<string, string>(_environment.SecurityAppName, x.UserLogonName)).ToList();
                var compCodes = userTypeInfos.Select(x => x.CompanyCode).ToList();

                result = this._userService.IsRecordExistInDb(appUserLogonNames, ref dbUsers, ref userNotExists);
                if (result.Code == ResponseType.Success.ToId() && Convert.ToBoolean(result.Result) == true)
                {
                    if (!this._companyService.IsValidCompany(compCodes, ref dbCompany, ref validationMessages))
                        result = result.ToPopulate(ResponseType.Validation, validationMessages, null);

                }
            }
            return result;
        }

        private IList<DomainModel.UserTypeInfo> FilterRecord(IList<DomainModel.UserTypeInfo> userTypes, ValidationType filterType)
        {
            IList<DomainModel.UserTypeInfo> filteredUserTypes = null;

            if (filterType == ValidationType.Add)
                filteredUserTypes = userTypes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredUserTypes = userTypes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (filterType == ValidationType.Update)
                filteredUserTypes = userTypes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            return filteredUserTypes;
        }

        private bool IsValidPayload(IList<DomainModel.UserTypeInfo> userTypes,
                            ValidationType validationType,
                            ref IList<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(userTypes), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Security, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsRecordUpdateCountMatch(IList<DomainModel.UserTypeInfo> userTypeInfos,
                                          IList<DbModel.UserType> dbUserTypes,
                                          ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var messages = validationMessages;
            var notMatchedRecords = userTypeInfos.Where(x => dbUserTypes.Any(x1 => x1.Company.Code == x.CompanyCode
                                                                                  && x1.User.Name == x.UserLogonName
                                                                                  && x1.UserTypeName == x.UserType
                                                                                  && x1.UpdateCount != x.UpdateCount)).ToList();
            notMatchedRecords?.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.UserTypeUpdateMismatch, x.UserLogonName, x.UserType);	
            });
            validationMessages = messages;
            return validationMessages?.Count <= 0;
        }

        #endregion


    }
}
