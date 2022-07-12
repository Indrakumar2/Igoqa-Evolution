using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.Logging.Interfaces;
using Evolution.Project.Domain.Interfaces.Data;
using Evolution.Project.Domain.Interfaces.Projects;
using Evolution.Project.Domain.Interfaces.Validations;
using Evolution.Project.Domain.Models.Projects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Project.Core.Services
{
    public class ProjectClientNotificationService : IProjectClientNotificationService
    {
        private IAppLogger<ProjectClientNotificationService> _logger = null;
        private IProjectClientNotificationRepository _projectClientNotificationRepository = null;
        private IProjectRepository _projectRepository = null;
        private ICustomerContactRepository _customerContactRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IProjectClientNotificationValidationService _validationService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly IMapper _mapper = null;

        public ProjectClientNotificationService(IAppLogger<ProjectClientNotificationService> logger,
                                                IProjectRepository projectRepository,
                                                IProjectClientNotificationRepository projectClientNotificationRepository,
                                                ICustomerContactRepository customerContactRepository,
                                                IProjectClientNotificationValidationService validationService,

                                               IAuditSearchService auditSearchService,
                                                IMapper mapper,
                                                JObject messages)

        {
            _logger = logger;
            _projectClientNotificationRepository = projectClientNotificationRepository;
            _projectRepository = projectRepository;
            _customerContactRepository = customerContactRepository;
            _validationService = validationService;
            _auditSearchService = auditSearchService;
            _mapper = mapper;
            this._messageDescriptions = messages;
        }

        #region Public Exposed Method


        public Response DeleteProjectClientNotifications(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, bool commitChange = true, int? projectId = null, bool isResultSetRequired = false)
        {
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = RemoveProjectClientNotification(projectNumber, projectClientNotifications, dbModule, commitChange, projectId);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectClientNotifications(new ProjectClientNotification() { ProjectNumber = projectNumber });
            else
                return result;
        }

        public Response DeleteProjectClientNotifications(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, int? projectId = null, bool isResultSetRequired = false)
        {
            var result = RemoveProjectClientNotification(projectNumber, projectClientNotifications,dbModule, commitChange, projectId);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectClientNotifications(new ProjectClientNotification() { ProjectNumber = projectNumber });
            else
                return result;
        }

        public Response GetProjectClientNotifications(ProjectClientNotification searchModel)
        {
            IList<ProjectClientNotification> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                {
                    result = _projectClientNotificationRepository.Search(searchModel);
                    tranScope.Complete();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response ModifyProjectClientNotifications(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, bool commitChange = true, int? projectId = null, bool isResultSetRequired = false)
        {
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = UpdateProjectClientNotification(projectNumber, projectClientNotifications,dbModule, commitChange, projectId);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectClientNotifications(new ProjectClientNotification() { ProjectNumber = projectNumber });
            else
                return result;
        }

        public Response ModifyProjectClientNotifications(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, int? projectId = null, bool isResultSetRequired = false)
        {
            var result = UpdateProjectClientNotification(projectNumber, projectClientNotifications,dbModule, commitChange, projectId);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectClientNotifications(new ProjectClientNotification() { ProjectNumber = projectNumber });
            else
                return result;
        }

        public Response SaveProjectClientNotifications(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, bool commitChange = true, int? projectId = null, bool isResultSetRequired = false)
        {
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = AddProjectClientNotification(projectNumber, projectClientNotifications, dbProject, dbModule, commitChange, projectId);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectClientNotifications(new ProjectClientNotification() { ProjectNumber = projectNumber });
            else
                return result;
        }

        public Response SaveProjectClientNotifications(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, IList<DbModel.Project> dbProject, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, int? projectId = null, bool isResultSetRequired = false)
        {
            var result = AddProjectClientNotification(projectNumber, projectClientNotifications, dbProject, dbModule, commitChange, projectId);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectClientNotifications(new ProjectClientNotification() { ProjectNumber = projectNumber });
            else
                return result;
        }

        #endregion

        #region Private Exposed Methods

        private Response AddProjectClientNotification(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, IList<DbModel.Project> dbProject, IList<DbModel.SqlauditModule> dbModule, bool commitChange, int? projectId = null)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            int? dbProjectId = projectId;
            List<DbModel.CustomerContact> DbCustomerContacts = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            DbModel.Project project = dbProject?.Count > 0 ? dbProject.FirstOrDefault() : null;
            try
            {
                _projectClientNotificationRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                IList<ProjectClientNotification> recordToBeInserted = null;
                eventId = projectClientNotifications?.FirstOrDefault()?.EventId;

                if (this.IsRecordValidForProcess(projectClientNotifications, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidProject(projectNumber, ref project, ref errorMessages))
                    {
                        if (this.IsValidCustomerContact(project, recordToBeInserted, ref DbCustomerContacts, ref errorMessages))
                        {
                            if (!IsProjectClientNotificationAlreadyAssociatedToContact(project.Id, recordToBeInserted, ValidationType.Add, ref errorMessages))
                            {
                                var dbProjectClientNotificationToBeInserted = recordToBeInserted.Select(x => new DbModel.ProjectClientNotification()
                                {
                                    ProjectId = project.Id,
                                    CustomerContactId = DbCustomerContacts.FirstOrDefault(x1 => x1.ContactName == x.CustomerContact).Id,
                                    SendInspectionReleaseNotesNotification = x.IsSendInspectionReleaseNotesNotification,
                                    SendFlashReportingNotification = x.IsSendFlashReportingNotification,
                                    SendCustomerReportingNotification = x.IsSendCustomerReportingNotification,
                                    SendNcrreportingNotification = x.IsSendNCRReportingNotification,
                                    SendCustomerDirectReportingNotification = x.IsSendCustomerDirectReportingNotification,
                                    ModifiedBy = x.ModifiedBy,
                                }).ToList();

                                _projectClientNotificationRepository.Add(dbProjectClientNotificationToBeInserted);

                                if (commitChange && !_projectClientNotificationRepository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                                {
                                    int value = _projectClientNotificationRepository.ForceSave();
                                    if (value > 0)
                                    {
                                        dbProjectClientNotificationToBeInserted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(recordToBeInserted?.FirstOrDefault(), ref eventId, recordToBeInserted?.FirstOrDefault()?.ActionByUser,
                                                                                                null,
                                                                                                ValidationType.Add.ToAuditActionType(),
                                                                                                SqlAuditModuleType.ProjectNotification,
                                                                                                null,
                                                                                                _mapper.Map<ProjectClientNotification>(x1),
                                                                                                dbModule
                                                                                                  ));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectClientNotifications);
            }
            finally
            {
                _projectClientNotificationRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response UpdateProjectClientNotification(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, IList<DbModel.SqlauditModule> dbModule, bool commitChange, int? projectId = null)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            int? dbProjectId = projectId;
            List<DbModel.CustomerContact> dbCustomerContacts = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _projectClientNotificationRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                IList<ProjectClientNotification> recordToBeModified = null;
                eventId = projectClientNotifications?.FirstOrDefault()?.EventId;
                if (this.IsRecordValidForProcess(projectClientNotifications, ValidationType.Update, ref recordToBeModified, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidProject(projectNumber, ref dbProjectId, ref errorMessages))
                    {
                        var modRecordProjectClientNotificationId = recordToBeModified.Select(x => x.ProjectClientNotificationId).ToList();
                        var prjProjectClientNotifications = _projectClientNotificationRepository.FindBy(x => x.ProjectId == dbProjectId && modRecordProjectClientNotificationId.Contains(x.Id));

                        if (IsValidProjectClientNotification(recordToBeModified, prjProjectClientNotifications.ToList(), ref errorMessages))
                        {
                            if (IsClientNotifiactionRecordCanBeUpdated(recordToBeModified, prjProjectClientNotifications.ToList(), ref errorMessages))
                            {
                                if (IsValidCustomerContact(dbProjectId, recordToBeModified, ref dbCustomerContacts, ref errorMessages))
                                {
                                    if (!IsProjectClientNotificationAlreadyAssociatedToContact(dbProjectId, recordToBeModified, ValidationType.Update, ref errorMessages))
                                    {
                                        IList<ProjectClientNotification> domExistingProjectNotification = new List<ProjectClientNotification>();
                                        prjProjectClientNotifications.ToList().ForEach(x =>
                                        {
                                            domExistingProjectNotification.Add(ObjectExtension.Clone(_mapper.Map<ProjectClientNotification>(x)));
                                        });
                                        foreach (var dbClientNotifications in prjProjectClientNotifications)
                                        {
                                            var projectClientNotification = recordToBeModified.FirstOrDefault(x => x.ProjectClientNotificationId == dbClientNotifications.Id);

                                            dbClientNotifications.CustomerContactId = dbCustomerContacts.FirstOrDefault(x => x.ContactName == projectClientNotification.CustomerContact).Id;
                                            dbClientNotifications.SendInspectionReleaseNotesNotification = projectClientNotification.IsSendInspectionReleaseNotesNotification;
                                            dbClientNotifications.SendFlashReportingNotification = projectClientNotification.IsSendFlashReportingNotification;
                                            dbClientNotifications.SendCustomerReportingNotification = projectClientNotification.IsSendCustomerReportingNotification;
                                            dbClientNotifications.SendNcrreportingNotification = projectClientNotification.IsSendNCRReportingNotification;
                                            dbClientNotifications.SendCustomerDirectReportingNotification = projectClientNotification.IsSendCustomerDirectReportingNotification;
                                            dbClientNotifications.UpdateCount = projectClientNotification.UpdateCount.CalculateUpdateCount();
                                            dbClientNotifications.LastModification = DateTime.UtcNow;
                                            dbClientNotifications.ModifiedBy = projectClientNotification.ModifiedBy;

                                            _projectClientNotificationRepository.Update(dbClientNotifications);
                                        }

                                        if (commitChange && !_projectClientNotificationRepository.AutoSave && recordToBeModified?.Count > 0 && errorMessages.Count <= 0)
                                        {
                                            int value = _projectClientNotificationRepository.ForceSave();
                                            if (value > 0)
                                                recordToBeModified?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                           null,
                                                                          ValidationType.Update.ToAuditActionType(),
                                                                          SqlAuditModuleType.ProjectNotification,
                                                                            domExistingProjectNotification?.FirstOrDefault(x2 => x2.ProjectClientNotificationId == x1.ProjectClientNotificationId),
                                                                          x1,dbModule
                                                                          ));

                                        }
                                    }

                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectClientNotifications);
            }
            finally
            {
                _projectClientNotificationRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response RemoveProjectClientNotification(int projectNumber, IList<ProjectClientNotification> projectClientNotifications, IList<DbModel.SqlauditModule> dbModule, bool commitChange, int? projectId = null)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            int? dbProjectId = projectId;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _projectClientNotificationRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                IList<ProjectClientNotification> recordToBedeleted = null;
                eventId = projectClientNotifications?.FirstOrDefault()?.EventId;
                if (this.IsRecordValidForProcess(projectClientNotifications, ValidationType.Delete, ref recordToBedeleted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidProject(projectNumber, ref dbProjectId, ref errorMessages))
                    {
                        var modRecordProjectClientNotificationId = recordToBedeleted.Select(x => x.ProjectClientNotificationId).ToList();
                        var dbProjectClientNotifications = _projectClientNotificationRepository.FindBy(x => x.ProjectId == projectNumber && modRecordProjectClientNotificationId.Contains(x.Id));

                        if (IsValidProjectClientNotification(recordToBedeleted, dbProjectClientNotifications.ToList(), ref errorMessages))
                        {
                            if (IsClientNotifiactionRecordCanBeUpdated(recordToBedeleted, dbProjectClientNotifications.ToList(), ref errorMessages))
                            {
                                foreach (var projectClientNotification in dbProjectClientNotifications)
                                {
                                    _projectClientNotificationRepository.Delete(projectClientNotification);
                                }

                                if (commitChange && !_projectClientNotificationRepository.AutoSave && recordToBedeleted?.Count > 0 && errorMessages.Count <= 0)
                                {
                                    int value = _projectClientNotificationRepository.ForceSave();
                                    if (value > 0)

                                        projectClientNotifications?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                                        null,
                                                                                                                         ValidationType.Delete.ToAuditActionType(),
                                                                                                                         SqlAuditModuleType.ProjectNotification,
                                                                                                                         x1,
                                                                                                                         null,
                                                                                                                         dbModule
                                                                                                                        ));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectClientNotifications);
            }
            finally
            {
                _projectClientNotificationRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool IsValidProject(int projectNumber, ref int? projectId, ref List<MessageDetail> errorMessages)
        {
            MessageType messageType = MessageType.Success;
            if (!projectId.HasValue)
            {
                if (projectNumber <= 0)
                    messageType = MessageType.InvalidProjectNumber;
                else
                {
                    projectId = _projectRepository.FindBy(x => x.ProjectNumber == projectNumber)?.FirstOrDefault().Id;
                    if (!projectId.HasValue)
                        messageType = MessageType.InvalidProjectNumber;
                }

                if (messageType != MessageType.Success)
                    errorMessages.Add(new MessageDetail(ModuleType.Project, messageType.ToId(), _messageDescriptions[messageType.ToId()].ToString()));
            }
            return messageType == MessageType.Success;
        }

        private bool IsValidProject(int projectNumber, ref DbModel.Project project, ref List<MessageDetail> errorMessages)
        {
            //using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            //{
            MessageType messageType = MessageType.Success;
            if (project == null)
            {
                if (projectNumber <= 0)
                    messageType = MessageType.InvalidProjectNumber;
                else
                {
                    project = _projectRepository.FindBy(x => x.ProjectNumber == projectNumber).FirstOrDefault();
                    if (project == null)
                        messageType = MessageType.InvalidProjectNumber;
                }

                if (messageType != MessageType.Success)
                    errorMessages.Add(new MessageDetail(ModuleType.Project, messageType.ToId(), _messageDescriptions[messageType.ToId()].ToString()));
            }
            // tranScope.Complete();
            return messageType == MessageType.Success;
            //}
        }

        private bool IsValidCustomerContact(int? projectId, IList<ProjectClientNotification> projectClientNotifications, ref List<DbModel.CustomerContact> dbCustomerContacts, ref List<MessageDetail> errorMessages)
        {
            //using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            //{
            List<MessageDetail> messages = new List<MessageDetail>();
            List<DbModel.CustomerContact> customerContacts = null;

            var contactNames = projectClientNotifications?.Select(x => x.CustomerContact);
            var contractCustomerId = _projectRepository.FindBy(x => x.Id == projectId).FirstOrDefault().Contract.CustomerId;
            customerContacts = _customerContactRepository.FindBy(x => x.CustomerAddress.CustomerId == contractCustomerId && contactNames.Any(x1 => x1 == x.ContactName)).ToList();

            var customerContactNameNotExists = projectClientNotifications.Where(x => !customerContacts.Any(x1 => x1.ContactName == x.CustomerContact)).ToList();

            customerContactNameNotExists.ForEach(x =>
            {
                string errorCode = MessageType.ProjectContactNameNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.CustomerContact)));
            });

            dbCustomerContacts = customerContacts;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);
            // tranScope.Complete();
            return errorMessages?.Count <= 0;
            // }
        }

        private bool IsValidCustomerContact(DbModel.Project project, IList<ProjectClientNotification> projectClientNotifications, ref List<DbModel.CustomerContact> dbCustomerContacts, ref List<MessageDetail> errorMessages)
        {
            //using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            //{
            List<MessageDetail> messages = new List<MessageDetail>();
            List<DbModel.CustomerContact> customerContacts = null;

            var contactNames = projectClientNotifications?.Select(x => x.CustomerContact);
            var contractCustomerId = project?.Contract?.CustomerId;
            customerContacts = _customerContactRepository.FindBy(x => x.CustomerAddress.CustomerId == contractCustomerId && contactNames.Any(x1 => x1 == x.ContactName)).ToList();

            var customerContactNameNotExists = projectClientNotifications.Where(x => !customerContacts.Any(x1 => x1.ContactName == x.CustomerContact)).ToList();

            customerContactNameNotExists.ForEach(x =>
            {
                string errorCode = MessageType.ProjectContactNameNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.CustomerContact)));
            });

            dbCustomerContacts = customerContacts;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);
            //tranScope.Complete();
            return errorMessages?.Count <= 0;
            //}
        }

        private bool IsClientNotifiactionRecordCanBeUpdated(IList<ProjectClientNotification> projectClientNotifications, IList<DbModel.ProjectClientNotification> dbProjectClientNotifications, ref List<MessageDetail> errorMessages)
        {
            //using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            //{
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = projectClientNotifications?.Where(x => !dbProjectClientNotifications.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.ProjectClientNotificationId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ProjectClientNotificationRecordUpdatedAlready.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.CustomerContact)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);
            // tranScope.Complete();

            return errorMessages?.Count <= 0;
            //}
        }

        private bool IsRecordValidForProcess(IList<ProjectClientNotification> projectClientNotifications, ValidationType validationType, ref IList<ProjectClientNotification> filteredProjectClientNotifications, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredProjectClientNotifications = projectClientNotifications?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredProjectClientNotifications = projectClientNotifications?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredProjectClientNotifications = projectClientNotifications?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            return filteredProjectClientNotifications?.Count > 0 ? IsProjectClientNotificationHasValidSchema(filteredProjectClientNotifications, validationType, ref validationMessages) : false;
        }

        private bool IsValidProjectClientNotification(IList<ProjectClientNotification> projectClientNotifications, IList<DbModel.ProjectClientNotification> dbProjectClientNotifications, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();

            var notMatchedRecords = projectClientNotifications?.Where(x => !dbProjectClientNotifications.ToList().Any(x1 => x1.Id == x.ProjectClientNotificationId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ProjectClientNotificationIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectClientNotificationId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsProjectClientNotificationAlreadyAssociatedToContact(int? projectId, IList<ProjectClientNotification> projectClientNotifications, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {
            IList<string> projectClientNotificationExists = null;
            List<MessageDetail> messages = new List<MessageDetail>();

            var prjClientNotifications = projectClientNotifications?.Select(x => new { x.CustomerContact, x.ProjectClientNotificationId }).ToList();
            if (prjClientNotifications?.Count > 0)
            {
                var filterExpressions = new List<Expression<Func<DbModel.ProjectClientNotification, bool>>>();
                Expression<Func<DbModel.ProjectClientNotification, bool>> predicate = null;
                Expression<Func<DbModel.ProjectClientNotification, bool>> containsExpression = null;
                if (validationType == ValidationType.Add)
                {
                    foreach (var clientNotification in prjClientNotifications)
                    {
                        containsExpression = a => a.ProjectId == projectId && a.CustomerContact.ContactName == clientNotification.CustomerContact;
                        filterExpressions.Add(containsExpression);
                    }
                }
                else if (validationType == ValidationType.Update)
                {
                    foreach (var clientNotification in prjClientNotifications)
                    {
                        containsExpression = a => a.ProjectId == projectId && a.CustomerContact.ContactName == clientNotification.CustomerContact && a.Id != clientNotification.ProjectClientNotificationId;
                        filterExpressions.Add(containsExpression);
                    }
                }
                predicate = filterExpressions.CombinePredicates<DbModel.ProjectClientNotification>(Expression.OrElse);

                projectClientNotificationExists = this._projectClientNotificationRepository?.FindBy(predicate).Select(x => x.CustomerContact.ContactName).ToList();

                projectClientNotificationExists?.ToList().ForEach(x =>
                {
                    string errorCode = MessageType.ProjectClientNotificationExistsForContact.ToId();
                    messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x)));
                });
            }
            //if (validationType == ValidationType.Add)
            //    projectClientNotificationExists = this._projectClientNotificationRepository?.FindBy(x => x.ProjectId == projectId && prjClientNotifications.Any(x1 => x.CustomerContact.ContactName == x1.CustomerContact && x.ProjectId == projectId)).Select(x => x.CustomerContact.ContactName).ToList();

            //else if (validationType == ValidationType.Update)
            //    projectClientNotificationExists = this._projectClientNotificationRepository?.FindBy(x => x.ProjectId == projectId && prjClientNotifications.Any(x1 => x.CustomerContact.ContactName == x1.CustomerContact && x.ProjectId == projectId && x.Id != x1.ProjectClientNotificationId)).Select(x => x.CustomerContact.ContactName).ToList();

            //projectClientNotificationExists?.ToList().ForEach(x =>
            //{
            //    string errorCode = MessageType.ProjectClientNotificationExistsForContact.ToId();
            //    messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x)));
            //});

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }

        private bool IsProjectClientNotificationHasValidSchema(IList<ProjectClientNotification> projectClientNotifications, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(projectClientNotifications), validationType);
            validationResults.ToList().ForEach(x =>
            {
                messages.Add(new ValidationMessage(x.Code, new List<MessageDetail> { new MessageDetail(ModuleType.Project, x.Code, x.Message) }));
            });

            if (messages.Count > 0)
            {
                validationMessages.AddRange(messages);
            }
            return validationMessages?.Count <= 0;
        }

        #endregion
    }
}
