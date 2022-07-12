using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Email.Domain.Enums;
using Evolution.Email.Domain.Interfaces.Data;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Interfaces.Validations;
using Evolution.Email.Domain.Models;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Email.Models;
using Evolution.Common.Helpers;
using Evolution.Cryptography;
using System.Transactions;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Models;

namespace Evolution.Email.Core.Services
{
    public class EmailQueueService : IEmailQueueService
    {
        private readonly IEmailRepository _emailRepository = null;
        private readonly IMasterRepository _masterRepository = null;
        private readonly IAppLogger<EmailQueueService> _logger = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IMapper _mapper = null;
        private readonly IEmailValidationService _validationService = null;

        public EmailQueueService(IEmailRepository emailRepository, 
                                 IAppLogger<EmailQueueService> logger, 
                                 IMapper mapper,
                                 IEmailValidationService validationService,
                                 IMasterRepository masterRepository)
        {
            this._emailRepository = emailRepository;
            this._logger = logger;
            this._mapper = mapper;
            this._validationService = validationService;
            _masterRepository = masterRepository;
        }

        public Response Get(EmailQueueMessage model)
        {
            Exception exception = null;
            IList<EmailQueueMessage> result = null;
            try
            {
                var dbModel = _mapper.Map<DbModel.Email>(model);
                var expression = dbModel.ToExpression();
                result = _mapper.Map<IList<EmailQueueMessage>>(_emailRepository.FindBy(expression).ToList());
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<EmailSendStatus> statuses, int maxRetryValue = 3)
        {
            Exception exception = null;
            IList<EmailQueueMessage> result = null;
            try
            {
                string[] emailStatuses = { };
                if (statuses?.Count >= 0)
                    emailStatuses = statuses.Select(x => x.ToString()).ToArray();

                var v = _emailRepository.FindBy(x => emailStatuses.Contains(x.EmailStatus) && (x.RetryCount == null || x.RetryCount <= maxRetryValue)).ToList();
                result = _mapper.Map<IList<EmailQueueMessage>>(v);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), statuses);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Add(IList<EmailQueueMessage> emailMessages, bool commitChange = true, bool isDbValidationRequired = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Email> recordToBeInserted = null;

            try
            {
                this._emailRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();

                if (emailMessages != null && emailMessages.Any(x=>x!=null))
                {
                    if (IsValidPayload(emailMessages, ValidationType.Add, ref validationMessages))
                    {
                        recordToBeInserted = _mapper.Map<IList<DbModel.Email>>(emailMessages.Where(x => x.Id <= 0).ToList());
                        recordToBeInserted = recordToBeInserted.Where(x=> !string.IsNullOrEmpty(x.ToEmail))?.Select(x => { x.EmailStatus = EmailSendStatus.NEW.ToString(); return x; }).ToList();
                        if (recordToBeInserted?.Count > 0)
                        {
                            this._emailRepository.Add(recordToBeInserted);

                            if (commitChange && !this._emailRepository.AutoSave)
                            {
                                int value = this._emailRepository.ForceSave();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), emailMessages);

            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages?.ToList(), null, exception);

        }

        public Response Modify(IList<EmailQueueMessage> emailMessages, bool commitChange = true, bool isDbValidationRequired = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                this._emailRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();

                if (emailMessages != null && emailMessages.Count(x => x != null && x.Id > 0) > 0)
                {
                    if (IsValidPayload(emailMessages, ValidationType.Update, ref validationMessages))
                    {
                        var recordToBeModify = _mapper.Map<IList<DbModel.Email>>(emailMessages.Where(x => x.Id > 0));
                        var emailIds = recordToBeModify.Where(x => !string.IsNullOrEmpty(x.ToEmail))?.Select(x1 => x1.Id.ToString());
                        var dbEmails = _emailRepository.FindBy(x => emailIds.Contains(x.Id.ToString())).ToList();

                        dbEmails.ForEach(x =>
                        {
                            var email = recordToBeModify?.FirstOrDefault(x1 => x1.Id == x.Id);
                            if (email != null)
                            {
                                x.BccEmail = email.BccEmail;
                                x.BodyContent = email.BodyContent;
                                x.CcEmail = email.CcEmail;
                                x.CreatedOn = x.CreatedOn;
                                x.EmailStatus = email.EmailStatus;
                                x.EmailType = email.EmailType;
                                x.FromEmail = email.FromEmail;
                                x.LastAttemptOn = email.LastAttemptOn;
                                x.ModuleCode = email.ModuleCode;
                                x.ModuleEmailRefCode = email.ModuleEmailRefCode;
                                x.RetryCount = email.RetryCount;
                                x.StatusReason = email.StatusReason;
                                x.Subject = email.Subject;
                                x.ToEmail = email.ToEmail; 
                            }
                        });

                        this._emailRepository.Update(dbEmails);
                        if (commitChange && !this._emailRepository.AutoSave)
                        {
                            int value = this._emailRepository.ForceSave();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), emailMessages);

            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages?.ToList(), null, exception);
        }

        private bool IsValidPayload(IList<EmailQueueMessage> emailQueueMessages,
                              ValidationType validationType,
                              ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(emailQueueMessages), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Email, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        public EmailQueueMessage PopulateEmailQueueMessage(ModuleType moduleType,
                                                            ModuleCodeType moduleCodeType,
                                                            string moduleRefCode,
                                                            EmailTemplate templateType,
                                                            EmailType emailType,
                                                            string emailSubject,
                                                            IList<EmailBodyPlaceHolder> emailContentPlaceholders,
                                                            List<EmailAddress> toAddresses,
                                                            List<EmailAddress> ccAddresses = null,
                                                            List<EmailAddress> bccAddresses = null,
                                                            bool IsMailSendAsGroup = false,
                                                            bool isEmailContentNeedToEncryp = false,
                                                            List<EmailAddress> fromAddresses = null)
        {
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            try
            {
                string privateKey = null; 
                var emailTemplateContent = GetEmailTemplate(new List<string> { templateType.ToString() })?.FirstOrDefault(x => x.KeyName == templateType.ToString())?.KeyValue;
                var bodyPlaceHolder = ParseEmailContentPlaceholders(emailContentPlaceholders);

                emailTemplateContent = isEmailContentNeedToEncryp ? EncryptContent(emailTemplateContent, ref privateKey) : emailTemplateContent;
                bodyPlaceHolder = isEmailContentNeedToEncryp ? EncryptContent(bodyPlaceHolder, ref privateKey) : bodyPlaceHolder;

                emailMessage.FromAddresses = fromAddresses ?? new List<EmailAddress>();
                emailMessage.BccAddresses = bccAddresses ?? new List<EmailAddress>();
                emailMessage.CcAddresses = ccAddresses ?? new List<EmailAddress>();
                emailMessage.ToAddresses = toAddresses;
                emailMessage.CreatedOn = DateTime.UtcNow;
                emailMessage.EmailType = emailType.ToString();
                emailMessage.ModuleCode = moduleCodeType.ToString();
                emailMessage.ModuleEmailRefCode = moduleRefCode.ToString();
                emailMessage.Subject = emailSubject;
                emailMessage.Content = emailTemplateContent;
                emailMessage.BodyPlaceHolderAndValue = bodyPlaceHolder;
                emailMessage.IsContentEncrypt = isEmailContentNeedToEncryp;
                emailMessage.PrivateKey = privateKey;
                emailMessage.IsMailSendAsGroup = IsMailSendAsGroup;
            }
            catch (Exception ex)
            {
                emailMessage = null;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return emailMessage;
        }

        public EmailQueueMessage PopulateEmailQueueMessage(ModuleType moduleType, EmailTemplate emailTemplateType, EmailType emailType
                                               , ModuleCodeType moduleCodeType, string moduleRefCode, string emailSubject
                                               , IList<KeyValuePair<string, string>> emailContentPlaceholders, List<EmailAddress> toAddresses
                                               , List<EmailAddress> ccAddresses = null, List<EmailAddress> bccAddresses = null,
                                                List<EmailAddress> fromAddresses = null, bool IsMailSendAsGroup = false)
        {
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            try
            {
                var emailTemplateContent = GetEmailTemplate(new List<string> { emailTemplateType.ToString() })?.FirstOrDefault(x => x.KeyName == emailTemplateType.ToString())?.KeyValue; 

                if (emailContentPlaceholders?.Count > 0 && !string.IsNullOrEmpty(emailTemplateContent))
                {
                    emailContentPlaceholders.ToList().ForEach(x =>
                    {
                        emailTemplateContent = emailTemplateContent.Replace(x.Key, x.Value);
                    });
                }
                emailMessage.FromAddresses = fromAddresses ?? new List<EmailAddress>();
                emailMessage.BccAddresses = bccAddresses ?? new List<EmailAddress>();
                emailMessage.CcAddresses = ccAddresses ?? new List<EmailAddress>();
                emailMessage.ToAddresses = toAddresses;
                emailMessage.CreatedOn = DateTime.UtcNow;
                emailMessage.EmailType = emailType.ToString();
                emailMessage.ModuleCode = moduleCodeType.ToString();
                emailMessage.ModuleEmailRefCode = moduleRefCode.ToString();
                emailMessage.Subject = emailSubject;
                emailMessage.Content = emailTemplateContent;
                emailMessage.IsMailSendAsGroup = IsMailSendAsGroup;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return emailMessage;
        }

        public IList<SystemSetting> GetEmailTemplate(IList<string> emailTemplateKeyNames)
        {
            IList<SystemSetting> emailTemplates = null;
            try
            {
                if (emailTemplateKeyNames?.Any() == true)
                {
                    emailTemplates = _masterRepository.GetCommonSystemSetting(emailTemplateKeyNames)?.Where(x => !string.IsNullOrEmpty(x.KeyValue)).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                throw ex;
            }

            return emailTemplates;
        }

        private string ParseEmailContentPlaceholders(IList<EmailBodyPlaceHolder> emailContentPlaceholders)
        {
            string placeHolder = string.Empty;
            emailContentPlaceholders?.ToList().ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.PlaceHolderName) )
                {
                    if (string.IsNullOrEmpty(placeHolder))
                        placeHolder = string.Format("{0}${1}${2}", x.PlaceHolderName, x.PlaceHolderValue,x.PlaceHolderForEmailId);
                    else
                        placeHolder = string.Format("{0}||{1}${2}${3}", placeHolder, x.PlaceHolderName, x.PlaceHolderValue,x.PlaceHolderForEmailId);
                }
            });
            return placeHolder;
        }

        private string EncryptContent(string content, ref string privateKey)
        {
            //string key1 = privateKey;
            SymmetricCryptography crypto = new SymmetricCryptography();
            if(string.IsNullOrEmpty(privateKey))
            {
                KeyGenerator key = new KeyGenerator();
                privateKey = key.GenratePrivateKey();
            }
           
            return crypto.Encrypt(content, privateKey);
        }
    }
}
