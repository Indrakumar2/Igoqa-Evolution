using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Email.Models;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Validations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistTimeOffRequestService : ITechnicalSpecialistTimeOffRequestService
    {
        private ITechnicalSpecialistTimeOffRequestRepository _repository = null;
        private IAppLogger<TechnicalSpecialistTimeOffRequestService> _logger = null;
        private ITechnicalSpecialistTimeOffRequestValidationService _validationService = null; 
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly ITimeOffRequestService _timeOffRequestService = null;
        private JObject _message = null;
        private IMapper _mapper = null;
        private IDataRepository _masterRepository = null;
        private readonly IEmailQueueService _emailService = null; 
        private IUserRepository _userRepository = null;
        private readonly IAuditLogger _auditLogger = null;
        private ITechnicalSpecialistCalendarService _technicalSpecialistCalendarService = null;

        public TechnicalSpecialistTimeOffRequestService(ITechnicalSpecialistTimeOffRequestRepository repository,
                                                        IAppLogger<TechnicalSpecialistTimeOffRequestService> logger,
                                                        ITechnicalSpecialistTimeOffRequestValidationService validationService, 
                                                        ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                        ITimeOffRequestService timeOffRequestService,
                                                        IDataRepository masterRepository,
                                                        IEmailQueueService emailService,
                                                        IUserRepository userRepository,
                                                        ITechnicalSpecialistCalendarService technicalSpecialistCalendarService,
                                                        JObject message,
                                                        IMapper mapper,
                                                        IAuditLogger auditLogger)
        {
            _repository = repository;
            _logger = logger;
            _validationService = validationService; 
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _masterRepository = masterRepository;
            _timeOffRequestService = timeOffRequestService;
            _message = message;
            _mapper = mapper;
            _emailService = emailService;
            _userRepository = userRepository;
            _auditLogger = auditLogger;
            _technicalSpecialistCalendarService = technicalSpecialistCalendarService;

        }

        public Response Add(IList<Domain.Models.TechSpecialist.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest, bool commitChange = true, bool isDbValidationRequired = true)
        {

            IList<DbModel.TechnicalSpecialistTimeOffRequest> dbTechnicalSpecialistTimeOffRequest = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            IList<DbModel.Data> dbLeaveCategories = null;
            long? eventId = null;
            return AddTechnicalSpecialistTimeOffRequest(technicalSpecialistTimeOffRequest, ref dbTechnicalSpecialistTimeOffRequest, ref dbTechnicalSpecialist, ref eventId, ref dbLeaveCategories, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<Domain.Models.TechSpecialist.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest, ref IList<DbRepository.Models.SqlDatabaseContext.TechnicalSpecialistTimeOffRequest> dbTechnicalSpecialistTimeOffRequest, ref IList<DbModel.TechnicalSpecialist> technicalSpecialists, ref long? eventId, ref IList<DbModel.Data> dbLeaveCategories, bool commitChange = true, bool isDbValidationRequired = true)
        {
            return AddTechnicalSpecialistTimeOffRequest(technicalSpecialistTimeOffRequest, ref dbTechnicalSpecialistTimeOffRequest, ref technicalSpecialists, ref eventId, ref dbLeaveCategories, commitChange, isDbValidationRequired);
        }

        public Response Get(DomainModel.TechnicalSpecialistTimeOffRequest searchModel)
        {
            IList<DomainModel.TechnicalSpecialistTimeOffRequest> result = null;
            Exception exception = null;
            try
            {
                result = _repository.Search(searchModel, techSpecialist => techSpecialist.TechnicalSpecialist);

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<long> ptoIds, string[] includes)
        {
            IList<DbModel.TechnicalSpecialistTimeOffRequest> result = null;
            Exception exception = null;
            try
            {
                result = _repository.FindBy(x => ptoIds.Contains(x.Id), includes).ToList();

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), ptoIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordValidForProcess(IList<Domain.Models.TechSpecialist.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest, ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistTimeOffRequest> dbTechnicalSpecialistTimeOffRequest = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist = null;
            IList<DbModel.Data> dbLeaveCategoryType = null;
            return IsRecordValidForProcess(technicalSpecialistTimeOffRequest, validationType, dbTechnicalSpecialistTimeOffRequest, dbTechnicalSpecialist, dbLeaveCategoryType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest, ValidationType validationType, ref IList<DbModel.TechnicalSpecialistTimeOffRequest> dbTechnicalSpecialistTimeOffRequest, ref IList<DbModel.TechnicalSpecialist> technicalSpecialists, ref IList<DbModel.Data> dbLeaveCategoryType)
        {
            IList<DomainModel.TechnicalSpecialistTimeOffRequest> filteredTechTimeOffRequest = null;
            return IsRecordValidForProcess(technicalSpecialistTimeOffRequest, validationType, ref filteredTechTimeOffRequest, ref dbTechnicalSpecialistTimeOffRequest,
                                          ref technicalSpecialists, ref dbLeaveCategoryType);
        }

        public Response IsRecordValidForProcess(IList<DomainModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest, ValidationType validationType,
                                                IList<DbModel.TechnicalSpecialistTimeOffRequest> dbTechnicalSpecialistTimeOffRequest,
                                                IList<DbModel.TechnicalSpecialist> technicalSpecialists, IList<DbModel.Data> dbLeaveCategoryType)
        {
            return IsRecordValidForProcess(technicalSpecialistTimeOffRequest, validationType, ref dbTechnicalSpecialistTimeOffRequest, ref technicalSpecialists, ref dbLeaveCategoryType);
        }

        #region private Methods
        private IList<DomainModel.TechnicalSpecialistTimeOffRequest> FilterRecord(IList<DomainModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest, ValidationType filterType)
        {
            IList<DomainModel.TechnicalSpecialistTimeOffRequest> filteredTechTimeOffRequest = null;

            if (filterType == ValidationType.Add)
                filteredTechTimeOffRequest = technicalSpecialistTimeOffRequest?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredTechTimeOffRequest = technicalSpecialistTimeOffRequest?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredTechTimeOffRequest = technicalSpecialistTimeOffRequest?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredTechTimeOffRequest;
        }

        private bool IsValidPayLoad(IList<DomainModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest,
                                   ValidationType validationType,
                                   ref IList<ValidationMessage> validationMessages)
        {
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var validMessages = validationMessages;

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(technicalSpecialistTimeOffRequest), validationType);
            if (validationResults?.Count > 0)
                validationMessages.Add(_message, ModuleType.TechnicalSpecialist, validationResults);

            validationMessages = validMessages;

            return validationMessages?.Count <= 0;
        }

        private Response AddTechnicalSpecialistTimeOffRequest(IList<DomainModel.TechnicalSpecialistTimeOffRequest> tsTimeOffRequest,
                                                                ref IList<DbModel.TechnicalSpecialistTimeOffRequest> dbTsTimeOffRequest,
                                                                ref IList<DbModel.TechnicalSpecialist> dbTs,
                                                                ref long? eventId,
                                                                ref IList<DbModel.Data> LeaveCategories,
                                                                bool commitChange = true,
                                                                bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> technicalSpecialists = null;
            long? eventID = null;
            try
            {
                IList<DbModel.Data> dbLeaveCategoryType = null;
                IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
                Response valdResponse = null;
                AppendEvent(tsTimeOffRequest.FirstOrDefault(), eventId);
                var recordToBeAdd = FilterRecord(tsTimeOffRequest, ValidationType.Add);
                if (isDbValidationRequire)
                    valdResponse = IsRecordValidForProcess(tsTimeOffRequest,
                                                           ValidationType.Add,
                                                           ref recordToBeAdd,
                                                           ref dbTsTimeOffRequest,
                                                           ref technicalSpecialists,
                                                           ref LeaveCategories);

                dbTsTimeOffRequest = GetTechnicalSpecialistTimeOffRequestInfo(recordToBeAdd);
                bool IsLoggedInUserTS = tsTimeOffRequest.SelectMany(x => x.UserTypes)
                                                       .Any(x1 => x1 == UserType.TechnicalSpecialist.ToString());
                if (recordToBeAdd?.Count > 0)
                {
                    if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                    {
                        _repository.AutoSave = false;
                        dbTechnicalSpecialists = technicalSpecialists;
                        dbLeaveCategoryType = LeaveCategories;
                        dbTsTimeOffRequest = _mapper.Map<IList<DbModel.TechnicalSpecialistTimeOffRequest>>(recordToBeAdd, opt =>
                          {
                              opt.Items["isAssignId"] = false;
                              opt.Items["TechnicalSpecialistId"] = dbTechnicalSpecialists;
                              opt.Items["LeaveCategoryType"] = dbLeaveCategoryType;
                          });
                        _repository.Add(dbTsTimeOffRequest);

                        if (commitChange && recordToBeAdd.Count > 0)
                        {
                            _repository.ForceSave();
                            valdResponse = PopulatingTsCalanderInfo(dbTsTimeOffRequest, dbTechnicalSpecialists);
                            if (valdResponse != null && valdResponse?.Code != MessageType.Success.ToId())
                            {
                                return valdResponse;
                            }
                            valdResponse = ProcessEmailNotifications(tsTimeOffRequest, (IsLoggedInUserTS ? EmailTemplate.EmailTimeOffRequest : EmailTemplate.EmailTimeOffRequestByCoordinator), ref validationMessages, dbTechnicalSpecialists);
                            if (valdResponse != null && valdResponse?.Code != MessageType.Success.ToId())
                            {
                                return valdResponse;
                            }
                            if (dbTsTimeOffRequest?.Count > 0 && recordToBeAdd?.Count > 0)
                            {
                                dbTsTimeOffRequest?.ToList().ForEach(x =>
                                recordToBeAdd?.ToList().ForEach(x1 => AuditLog(x1,
                                                                        x,
                                                                        ValidationType.Add.ToAuditActionType(),
                                                                        SqlAuditModuleType.TechnicalSpecialistTimeOffRequest,
                                                                        null,
                                                                        x1,
                                                                         ref eventID)));
                                eventId = eventID;
                            }
                        }
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTimeOffRequest);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response IsRecordValidForProcess(IList<DomainModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest,
                                                    ValidationType validationType,
                                                    ref IList<DomainModel.TechnicalSpecialistTimeOffRequest> filteredtechnicalSpecialistTimeOffRequest,
                                                    ref IList<DbModel.TechnicalSpecialistTimeOffRequest> dbTechnicalSpecialistTimeOffRequest,
                                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                                    ref IList<DbModel.Data> dbLeaveCategoryTypes)
        {
            Exception exception = null;
            bool result = true;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (technicalSpecialistTimeOffRequest != null && technicalSpecialistTimeOffRequest.Count > 0)
                {
                    if (validationMessages == null)
                        validationMessages = new List<ValidationMessage>();

                    if (filteredtechnicalSpecialistTimeOffRequest == null || filteredtechnicalSpecialistTimeOffRequest.Count <= 0)
                        filteredtechnicalSpecialistTimeOffRequest = FilterRecord(technicalSpecialistTimeOffRequest, validationType);

                    if (filteredtechnicalSpecialistTimeOffRequest?.Count > 0 && IsValidPayLoad(filteredtechnicalSpecialistTimeOffRequest, validationType, ref validationMessages))
                    {
                        if (validationType == ValidationType.Add)
                            result = IsRecordValidForAdd(filteredtechnicalSpecialistTimeOffRequest, ref dbTechnicalSpecialistTimeOffRequest, ref dbTechnicalSpecialist, ref dbLeaveCategoryTypes, ref validationMessages);
                    }
                    else
                        result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), technicalSpecialistTimeOffRequest);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }


        private bool IsRecordValidForAdd(IList<DomainModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest,
                                            ref IList<DbModel.TechnicalSpecialistTimeOffRequest> dbTechSpecialistTimeOffRequest,
                                            ref IList<DbModel.TechnicalSpecialist> technicalSpecialists, ref IList<DbModel.Data> dbLeaveCategoryTypes,
                                            ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = null;
            validationMessages = validationMessages ?? new List<ValidationMessage>();
            var technicalSpecialistIds = technicalSpecialistTimeOffRequest.Where(x => x.Epin > 0).Select(u => u.Epin.ToString()).Distinct().ToList();
            //bool result = Convert.ToBoolean(_technicalSpecialistService.IsRecordExistInDb(technicalSpecialistIds, ref technicalSpecialists, ref validationMessages).Result);
            bool result = IsTechSpecialistExistInDb(technicalSpecialistIds, ref technicalSpecialists, ref validationMessages);
            if (result)
                IsValidLeaveCategroy(technicalSpecialistTimeOffRequest, ref dbLeaveCategoryTypes, ref validationMessages);

            messages = validationMessages;

            return messages?.Count <= 0;
        }

        private bool IsValidLeaveCategroy(IList<DomainModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest,
                                           ref IList<DbModel.Data> dbLeaveTypeCategories,
                                           ref IList<ValidationMessage> validationMessages,
                                           params Expression<Func<DbModel.TimeOffRequestCategory, object>>[] includes)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>(); 
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var referenceTypeNames = technicalSpecialistTimeOffRequest.Select(x => x.LeaveCategoryType).ToList();

            var dbData = _masterRepository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.LeaveCategoryType) &&
                                                                 referenceTypeNames.Contains(x.Name)).ToList();

            var referenceTypeNotExists = technicalSpecialistTimeOffRequest.Where(x => !dbData.Any(x1 => x1.Name == x.LeaveCategoryType)).ToList();
            referenceTypeNotExists.ToList().ForEach(x =>
            {
                messages.Add(_message, x.LeaveCategoryType, MessageType.TsInvalidLeaveCategory, x.LeaveCategoryType);
            });


            dbLeaveTypeCategories = dbData;

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private IList<DbModel.TechnicalSpecialistTimeOffRequest> GetTechnicalSpecialistTimeOffRequestInfo(IList<DomainModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest)
        {
            IList<DbModel.TechnicalSpecialistTimeOffRequest> dbTechnicalSpecialistTimeOffRequest = null;
            if (technicalSpecialistTimeOffRequest?.Count > 0)
            {
                var technicalSpecialistTimeOffRequestId = technicalSpecialistTimeOffRequest.Select(x => x.TechnicalSpecialistTimeOffRequestId).Distinct().ToList();
                dbTechnicalSpecialistTimeOffRequest = _repository.FindBy(x => technicalSpecialistTimeOffRequestId.Contains(x.Id)).ToList();
            }

            return dbTechnicalSpecialistTimeOffRequest;
        }

        private Response ProcessEmailNotifications(IList<DomainModel.TechnicalSpecialistTimeOffRequest> tsTimeOffRequest,
                                                   EmailTemplate emailTemplateType,
                                                   ref IList<ValidationMessage> validationMessages,
                                                   IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists)
        {
            Exception exception = null;
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            string companyCode = string.Empty;
            string requestedByName = string.Empty;
            try
            {
                companyCode = tsTimeOffRequest.FirstOrDefault().CompanyCode;
                if (tsTimeOffRequest != null && !string.IsNullOrEmpty(companyCode))
                {
                    var userTypes = new List<string>()
                        {
                            UserType.ResourceCoordinator.ToString(),
                            UserType.OperationManager.ToString(),
                        };
                    List<EmailAddress> fromEmails  =null;
                    List<EmailAddress> toEmails = null;
                    List<EmailAddress> ccEmails = null;
                    string subject = string.Empty;
                    string emailType = string.Empty;

                    var emailTemplateContent = _emailService.GetEmailTemplate(new List<string> { emailTemplateType.ToString() })?.FirstOrDefault(x => x.KeyName == emailTemplateType.ToString())?.KeyValue;
                    if (!string.IsNullOrEmpty(emailTemplateContent))
                    { 
                        if (emailTemplateType == EmailTemplate.EmailTimeOffRequest)
                        {
                            emailType = EmailType.TORR.ToString();
                            emailTemplateContent = emailTemplateContent?.Replace(TechnicalSpecialistConstants.Email_Content_Resource_Name, tsTimeOffRequest?.FirstOrDefault()?.ResourceName);
                            subject = TechnicalSpecialistConstants.Email_Notification_TimeOffRequest_Subject
                                ?.Replace(TechnicalSpecialistConstants.Email_Content_Epin, tsTimeOffRequest?.FirstOrDefault()?.Epin.ToString())
                                ?.Replace(TechnicalSpecialistConstants.Email_Content_Resource_Name, tsTimeOffRequest?.FirstOrDefault()?.ResourceName);
                        }
                        else if (emailTemplateType == EmailTemplate.EmailTimeOffRequestByCoordinator)
                        {
                            emailType = EmailType.TORU.ToString();
                            subject = TechnicalSpecialistConstants.Email_Notification_TimeOffRequestByCoordinator_Subject
                                ?.Replace(TechnicalSpecialistConstants.Email_Content_Epin, tsTimeOffRequest?.FirstOrDefault()?.Epin.ToString());
                            var requestedBy = _userRepository.Get(companyCode, tsTimeOffRequest?.FirstOrDefault()?.RequestedBy, true);
                            fromEmails = requestedBy?.Select(x => new EmailAddress() { DisplayName = x.Name, Address = x.Email }).ToList();
                            requestedByName = requestedBy.FirstOrDefault()?.Name;
                        }

                        var userUserTypeData = _userRepository.GetUserByType(companyCode, userTypes);
                        var toUsers = userUserTypeData?.Select(x1 => new DbModel.User { Id = x1.User.Id, Name = x1.User.Name, SamaccountName = x1.User.SamaccountName, Email = x1.User.Email })?.GroupBy(x2 => x2.Id)?.Select(x3 => x3.FirstOrDefault())?.ToList();
                        
                        toEmails = toUsers?.Select(x => new EmailAddress() { DisplayName = x.Name, Address = x.Email }).ToList();
                        if (emailTemplateType == EmailTemplate.EmailTimeOffRequestByCoordinator && dbTechnicalSpecialists != null)
                        {
                            ccEmails = new List<EmailAddress> { new EmailAddress { Address = dbTechnicalSpecialists?.FirstOrDefault(x => x.Pin == tsTimeOffRequest?.FirstOrDefault()?.Epin)?.TechnicalSpecialistContact?.FirstOrDefault(x => x.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress } };
                        }

                        emailTemplateContent = emailTemplateContent
                            ?.Replace(TechnicalSpecialistConstants.Email_Content_Coordinator_Name, requestedByName)
                            ?.Replace(TechnicalSpecialistConstants.Email_Content_Resource_Name, tsTimeOffRequest?.FirstOrDefault()?.ResourceName)
                            ?.Replace(TechnicalSpecialistConstants.Email_Content_Epin, tsTimeOffRequest?.FirstOrDefault()?.Epin.ToString())
                            ?.Replace(TechnicalSpecialistConstants.Email_Content_Date_Form, tsTimeOffRequest?.FirstOrDefault()?.TimeOffFrom.ToString("dd-MMM-yyyy"))
                            ?.Replace(TechnicalSpecialistConstants.Email_Content_Date_Till, tsTimeOffRequest?.FirstOrDefault()?.TimeOffThrough.ToString("dd-MMM-yyyy"));

                        emailMessage.CreatedOn = DateTime.UtcNow;
                        emailMessage.EmailType = emailType;
                        emailMessage.ModuleCode = ModuleCodeType.TS.ToString();
                        emailMessage.ModuleEmailRefCode = tsTimeOffRequest.FirstOrDefault()?.Epin.ToString();
                        emailMessage.Subject = subject;
                        emailMessage.Content = emailTemplateContent;
                        emailMessage.ToAddresses = toEmails;
                        emailMessage.CcAddresses = ccEmails;
                        emailMessage.FromAddresses = fromEmails;
                        emailMessage.IsMailSendAsGroup = true;//def 1163 fix

                        _emailService.Add(new List<EmailQueueMessage> { emailMessage });
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsTimeOffRequest);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response PopulatingTsCalanderInfo(IList<DbModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequests, IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists)
        {
            if (technicalSpecialistTimeOffRequests != null && technicalSpecialistTimeOffRequests?.Count > 0)
            {
                technicalSpecialistTimeOffRequests.ToList().ForEach(calendar =>
                {
                    calendar.FromDate = calendar.FromDate.Date.AddHours(9);
                    calendar.ToDate = calendar.ToDate.Date.AddHours(17);
                });
                var dbTsTimeOffRequest = _mapper.Map<IList<DbModel.TechnicalSpecialistCalendar>>(technicalSpecialistTimeOffRequests, opt =>
                {
                    opt.Items["dbTechnicalSpecialists"] = dbTechnicalSpecialists;
                });
                return _technicalSpecialistCalendarService.Save(dbTsTimeOffRequest, true);
            }
            return null;
        }


        private Response AuditLog(DomainModel.TechnicalSpecialistTimeOffRequest technicalSpecialistTimeOffRequest,
                       DbModel.TechnicalSpecialistTimeOffRequest dbTechnicalSpecialistTimeOffRequest,
                       SqlAuditActionType sqlAuditActionType,
                       SqlAuditModuleType sqlAuditModuleType,
                       object oldData,
                       object newData,
                       ref long? eventId)
        {
            LogEventGeneration logEventGeneration = new LogEventGeneration(_auditLogger);
            Exception exception = null;
            if (technicalSpecialistTimeOffRequest != null && !string.IsNullOrEmpty(technicalSpecialistTimeOffRequest.ActionByUser))
            {
                string actionBy = technicalSpecialistTimeOffRequest.ActionByUser;
                eventId = logEventGeneration.GetEventLogId(eventId,
                                                               sqlAuditActionType,
                                                               actionBy,
                                                               dbTechnicalSpecialistTimeOffRequest.Id.ToString(),
                                                               SqlAuditModuleType.TechnicalSpecialistTimeOffRequest.ToString());

                return _auditLogger.LogAuditData((long)eventId, sqlAuditModuleType, oldData, newData);

            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }
        private void AppendEvent(DomainModel.TechnicalSpecialistTimeOffRequest technicalSpecialistTimeOffRequest,
                      long? eventId)
        {
            ObjectExtension.SetPropertyValue(technicalSpecialistTimeOffRequest, "EventId", eventId); 
        }

        private bool IsTechSpecialistExistInDb(IList<string> tsPins,
                                              ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                              ref IList<ValidationMessage> validationMessages)
        {
            if ((dbTsInfos?.Count == 0 || dbTsInfos == null) && tsPins?.Count > 0)
                dbTsInfos = _technicalSpecialistRepository.FindBy(x => tsPins.Contains(x.Pin.ToString())).ToList();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsInfos == null)
                dbTsInfos = new List<DbModel.TechnicalSpecialist>();

            var validMessages = validationMessages;
            var dbTechSpecs = dbTsInfos;

            if (tsPins?.Count > 0)
            {
                IList<string> tsPinNotExists = tsPins.Where(pin => !dbTechSpecs.Any(x1 => x1.Pin.ToString() == pin))
                                        .Select(pin => pin)
                                        .ToList();

                tsPinNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_message, x, MessageType.TsEPinDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }
    }

    #endregion

    class EmailPlaceHolderItem
    {
        public string PlaceHolderName { get; set; }

        public string PlaceHolderValue { get; set; }

        public string PlaceHolderForEmail { get; set; }
    }
}


