using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
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
    public class ProjectInvoiceAttachmentService : IProjectInvoiceAttachmentService
    {
        private IAppLogger<ProjectInvoiceAttachmentService> _logger = null;
        private IProjectInvoiceAttachmentRepository _projectInvoiceAttachmentRepository = null;
        private IProjectRepository _projectRepository = null;
        private readonly IModuleDocumentTypeRepository _documentTypeRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IInvoiceAttachmentValidationService _validationService = null;
        private readonly IMapper _mapper = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public ProjectInvoiceAttachmentService(IAppLogger<ProjectInvoiceAttachmentService> logger,
                                               IProjectInvoiceAttachmentRepository repository,
                                               IProjectRepository projectRepository,
                                               IModuleDocumentTypeRepository documentTypeRepository,
                                               IInvoiceAttachmentValidationService validationService,
                                               IMapper mapper,
                                               JObject messages,
                                               IAuditSearchService auditSearchService)
        {
            _logger = logger;
            _projectInvoiceAttachmentRepository = repository;
            _projectRepository = projectRepository;
            _documentTypeRepository = documentTypeRepository;
            _validationService = validationService;
            _mapper = mapper;
            this._messageDescriptions = messages;
            this._auditSearchService = auditSearchService;
        }

        #region Public Exposed Method

        public Response DeleteProjectInvoiceAttachments(int projectNumber, IList<ProjectInvoiceAttachment> invoiceAttachmentTypes, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Project> dbProjects = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = this.RemoveProjectInvoiceAttachment(projectNumber, invoiceAttachmentTypes, dbProjects, dbModule,commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectInvoiceAttachments(new ProjectInvoiceAttachment() { ProjectNumber = projectNumber });
            else
                return result;
        }
        public Response DeleteProjectInvoiceAttachments(int projectNumber, IList<ProjectInvoiceAttachment> invoiceAttachmentTypes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.RemoveProjectInvoiceAttachment(projectNumber, invoiceAttachmentTypes,dbProjects,dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectInvoiceAttachments(new ProjectInvoiceAttachment() { ProjectNumber = projectNumber });
            else
                return result;
        }

        public Response GetProjectInvoiceAttachments(ProjectInvoiceAttachment searchModel)
        {
            IList<ProjectInvoiceAttachment> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                {
                    result = _projectInvoiceAttachmentRepository.Search(searchModel);
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

        public Response ModifyProjectInvoiceAttachments(int projectNumber, IList<ProjectInvoiceAttachment> invoiceAttachmentTypes, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Project> dbProjects=null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = this.UpdateProjectInvoiceAttachment(projectNumber, invoiceAttachmentTypes,dbProjects, dbModule,commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectInvoiceAttachments(new ProjectInvoiceAttachment() { ProjectNumber = projectNumber });
            else
                return result;
        }

        public Response ModifyProjectInvoiceAttachments(int projectNumber, IList<ProjectInvoiceAttachment> invoiceAttachmentTypes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.UpdateProjectInvoiceAttachment(projectNumber, invoiceAttachmentTypes, dbProjects,dbModule,commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectInvoiceAttachments(new ProjectInvoiceAttachment() { ProjectNumber = projectNumber });
            else
                return result;
        }

        public Response SaveProjectInvoiceAttachments(int projectNumber, IList<ProjectInvoiceAttachment> invoiceAttachmentTypes, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Project> dbProjects= null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = this.AddProjectInvoiceAttachment(projectNumber, invoiceAttachmentTypes,dbProjects,dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectInvoiceAttachments(new ProjectInvoiceAttachment() { ProjectNumber = projectNumber });
            else
                return result;
        }

        public Response SaveProjectInvoiceAttachments(int projectNumber, IList<ProjectInvoiceAttachment> invoiceAttachmentTypes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.AddProjectInvoiceAttachment(projectNumber, invoiceAttachmentTypes, dbProjects,dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectInvoiceAttachments(new ProjectInvoiceAttachment() { ProjectNumber = projectNumber });
            else
                return result;
        }

        #endregion

        #region Private Exposed Methods

        private Response AddProjectInvoiceAttachment(int projectNumber, IList<ProjectInvoiceAttachment> projectInvoiceAttachmentTypes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Project dbProject = dbProjects?.Count > 0 ? dbProjects.FirstOrDefault(): null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _projectInvoiceAttachmentRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ProjectInvoiceAttachment> recordToBeInserted = null;
                validationMessages = new List<ValidationMessage>();
                eventId = projectInvoiceAttachmentTypes?.FirstOrDefault()?.EventId;
                if (this.IsRecordValidForProcess(projectInvoiceAttachmentTypes, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidProject(projectNumber, ref dbProject, ref errorMessages))
                    {
                        IList<DbModel.ModuleDocumentType> dbDocumentTypes = null;
                        if (this.IsValidDocumentType(recordToBeInserted, ref dbDocumentTypes, ref errorMessages))
                        {
                            if (!this.IsProjectInvoiceAttachmentAlreadyAssociated(dbProject.Id, recordToBeInserted, ValidationType.Add, ref errorMessages))
                            {
                                var dbProjectInvAttachmentToBeInserted = recordToBeInserted.Select(x => new DbModel.ProjectInvoiceAttachment()
                                {
                                    ProjectId = dbProject.Id,
                                    DocumentTypeId = dbDocumentTypes.FirstOrDefault(x1 => x1.DocumentType.Name == x.DocumentType).DocumentTypeId,
                                    ModifiedBy = x.ModifiedBy,
                                }).ToList();

                                _projectInvoiceAttachmentRepository.Add(dbProjectInvAttachmentToBeInserted);

                                if (commitChange && !_projectInvoiceAttachmentRepository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                                {
                                    int value = _projectInvoiceAttachmentRepository.ForceSave();
                                    if (value > 0)

                                        dbProjectInvAttachmentToBeInserted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(recordToBeInserted.FirstOrDefault(), ref eventId, recordToBeInserted?.FirstOrDefault()?.ActionByUser,
                                                                                                 null,
                                                                                                  ValidationType.Add.ToAuditActionType(),
                                                                                                  SqlAuditModuleType.ProjectAttachment,
                                                                                                  null,
                                                                                                    _mapper.Map<ProjectInvoiceAttachment>(x1),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectInvoiceAttachmentTypes);
            }
            finally
            {
                _projectInvoiceAttachmentRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response UpdateProjectInvoiceAttachment(int projectNumber, IList<ProjectInvoiceAttachment> projectInvoiceAttachmentTypes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Project dbProject = dbProjects?.Count > 0 ? dbProjects.FirstOrDefault(): null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _projectInvoiceAttachmentRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ProjectInvoiceAttachment> recordToBeupdated = null;
                eventId = projectInvoiceAttachmentTypes?.FirstOrDefault()?.EventId;
                validationMessages = new List<ValidationMessage>();
                if (this.IsRecordValidForProcess(projectInvoiceAttachmentTypes, ValidationType.Update, ref recordToBeupdated, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidProject(projectNumber, ref dbProject, ref errorMessages))
                    {
                        IList<DbModel.ModuleDocumentType> dbDocumentTypes = null;

                        if (this.IsValidDocumentType(recordToBeupdated, ref dbDocumentTypes, ref errorMessages))
                        {
                            if (!this.IsProjectInvoiceAttachmentAlreadyAssociated(dbProject.Id, recordToBeupdated, ValidationType.Update, ref errorMessages))
                            {
                                var modRecordContractInvoiceAttachmentId = recordToBeupdated.Select(x => x.ProjectInvoiceAttachmentId).ToList();
                                var prjInvoiceAttachments = _projectInvoiceAttachmentRepository.FindBy(x => x.ProjectId == dbProject.Id && modRecordContractInvoiceAttachmentId.Contains(x.Id));

                                if (IsValidProjectInvoiceAttachment(recordToBeupdated, prjInvoiceAttachments.ToList(), ref errorMessages))
                                {
                                    if (this.IsProjectInvoiceAttachmentCanBeUpdated(recordToBeupdated, prjInvoiceAttachments.ToList(), ref errorMessages))
                                    {
                                        IList<ProjectInvoiceAttachment> domExistingProjectInvoiceAttachment = new List<ProjectInvoiceAttachment>();
                                        prjInvoiceAttachments.ToList().ForEach(x =>
                                        {
                                            domExistingProjectInvoiceAttachment.Add(ObjectExtension.Clone(_mapper.Map<ProjectInvoiceAttachment>(x)));
                                        });

                                        foreach (var dbProjectInvAttachment in prjInvoiceAttachments)
                                        {
                                            var projectInvoiceAttachment = recordToBeupdated.FirstOrDefault(x => x.ProjectInvoiceAttachmentId == dbProjectInvAttachment.Id);

                                            dbProjectInvAttachment.DocumentTypeId = dbDocumentTypes.FirstOrDefault(x1 => x1.DocumentType.Name == projectInvoiceAttachment.DocumentType).DocumentTypeId;
                                            dbProjectInvAttachment.LastModification = DateTime.UtcNow;
                                            dbProjectInvAttachment.UpdateCount = projectInvoiceAttachment.UpdateCount.CalculateUpdateCount();
                                            dbProjectInvAttachment.ModifiedBy = projectInvoiceAttachment.ModifiedBy;
                                            _projectInvoiceAttachmentRepository.Update(dbProjectInvAttachment);
                                        }

                                        if (commitChange && recordToBeupdated?.Count > 0)
                                        {
                                            int value = _projectInvoiceAttachmentRepository.ForceSave();
                                            if (value > 0)
                                                recordToBeupdated?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                   null,
                                                                  ValidationType.Update.ToAuditActionType(),
                                                                  SqlAuditModuleType.ProjectAttachment,
                                                                  domExistingProjectInvoiceAttachment?.FirstOrDefault(x2 => x2.ProjectInvoiceAttachmentId == x1.ProjectInvoiceAttachmentId),
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectInvoiceAttachmentTypes);
            }
            finally
            {
                _projectInvoiceAttachmentRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response RemoveProjectInvoiceAttachment(int projectNumber, IList<ProjectInvoiceAttachment> projectInvoiceAttachmentTypes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Project dbProject = dbProjects?.Count > 0 ? dbProjects.FirstOrDefault() : null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _projectInvoiceAttachmentRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                eventId = projectInvoiceAttachmentTypes?.FirstOrDefault()?.EventId;
                IList<ProjectInvoiceAttachment> recordToBedeleted = null;
                validationMessages = new List<ValidationMessage>();
                if (this.IsRecordValidForProcess(projectInvoiceAttachmentTypes, ValidationType.Delete, ref recordToBedeleted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidProject(projectNumber, ref dbProject, ref errorMessages))
                    {
                        var modRecordProjectInvoiceAttachmentId = recordToBedeleted.Select(x => x.ProjectInvoiceAttachmentId).ToList();
                        var dbProjectInvoiceAttachments = _projectInvoiceAttachmentRepository.FindBy(x => x.ProjectId == dbProject.Id && modRecordProjectInvoiceAttachmentId.Contains(x.Id));

                        if (IsValidProjectInvoiceAttachment(recordToBedeleted, dbProjectInvoiceAttachments.ToList(), ref errorMessages))
                        {
                            foreach (var projectInvAttachment in dbProjectInvoiceAttachments)
                                _projectInvoiceAttachmentRepository.Delete(projectInvAttachment);

                            if (commitChange && recordToBedeleted?.Count > 0)
                            {
                                int value = _projectInvoiceAttachmentRepository.ForceSave();
                                if (value > 0)
                                {

                                    projectInvoiceAttachmentTypes?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                                         null,
                                                                                                                          ValidationType.Delete.ToAuditActionType(),
                                                                                                                       SqlAuditModuleType.ProjectAttachment,
                                                                                                                             x1, null,dbModule));
                                }


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectInvoiceAttachmentTypes);
            }
            finally
            {
                _projectInvoiceAttachmentRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool IsValidProject(int projectNumber, ref DbModel.Project project, ref List<MessageDetail> errorMessages)
        {
            MessageType messageType = MessageType.Success;

            if (projectNumber <= 0)
                messageType = MessageType.InvalidProjectNumber;
            else
            {
                if (project == null)
                    project = _projectRepository.FindBy(x => x.ProjectNumber == projectNumber).FirstOrDefault();
                if (project == null)
                    messageType = MessageType.InvalidProjectNumber;
            }

            if (messageType != MessageType.Success)
                errorMessages.Add(new MessageDetail(ModuleType.Project, messageType.ToId(), _messageDescriptions[messageType.ToId()].ToString()));
            //tranScope.Complete();

            return messageType == MessageType.Success;
        }

        private bool IsValidDocumentType(IList<ProjectInvoiceAttachment> projectInvoiceAttachments, ref IList<DbModel.ModuleDocumentType> dbDocumentTypes, ref List<MessageDetail> errorMessages)
        {
            //using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            //{
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var documentTypeNames = projectInvoiceAttachments.Select(x => x.DocumentType).ToList();

            var dbDocTypes = this._documentTypeRepository.FindBy(x => documentTypeNames.Contains(x.DocumentType.Name)).ToList();

            var documentTypeNotExists = projectInvoiceAttachments.Where(x => !dbDocTypes.Any(x1 => x1.DocumentType.Name == x.DocumentType)).ToList();
            documentTypeNotExists.ForEach(x =>
            {
                string errorCode = MessageType.ProjectDocumentTypeNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.DocumentType)));
            });

            dbDocumentTypes = dbDocTypes;

            if (messages.Count > 0)
                errorMessages.AddRange(messages);
            //tranScope.Complete();
            return errorMessages?.Count <= 0;
            //}
        }

        private bool IsProjectInvoiceAttachmentCanBeUpdated(IList<ProjectInvoiceAttachment> projectInvoiceAttachments, IList<DbModel.ProjectInvoiceAttachment> dbProjectInvoiceAttachments, ref List<MessageDetail> errorMessages)
        {
            //using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            //{
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = projectInvoiceAttachments?.Where(x => !dbProjectInvoiceAttachments.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.ProjectInvoiceAttachmentId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ProjectInvoiceAttachementRecordUpdatedAlready.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.DocumentType)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);
            //tranScope.Complete();
            return errorMessages?.Count <= 0;
            //}
        }

        private bool IsProjectInvoiceAttachmentAlreadyAssociated(int projectId, IList<ProjectInvoiceAttachment> projectInvoiceAttachments, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {
            IList<DbModel.ProjectInvoiceAttachment> projectInvoiceAttachmentExists = null;
            List<MessageDetail> messages = new List<MessageDetail>();
            var filterExpressions = new List<Expression<Func<DbModel.ProjectInvoiceAttachment, bool>>>();
            Expression<Func<DbModel.ProjectInvoiceAttachment, bool>> predicate = null;
            Expression<Func<DbModel.ProjectInvoiceAttachment, bool>> containsExpression = null;
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var prjInvAttachments = projectInvoiceAttachments?.Select(x => new { x.ProjectNumber, x.DocumentType, x.ProjectInvoiceAttachmentId }).ToList();
            if (prjInvAttachments?.Count > 0)
            {
                if (validationType == ValidationType.Add)
                {
                    foreach (var prjInvAttmts in prjInvAttachments)
                    {
                        containsExpression = a => a.DocumentType.Name == prjInvAttmts.DocumentType;
                        filterExpressions.Add(containsExpression);
                    }
                }
                else if (validationType == ValidationType.Update)
                {
                    foreach (var prjInvAttmts in prjInvAttachments)
                    {
                        containsExpression = a => a.DocumentType.Name == prjInvAttmts.DocumentType && a.Id != prjInvAttmts.ProjectInvoiceAttachmentId;
                        filterExpressions.Add(containsExpression);
                    }
                }
                predicate = filterExpressions.CombinePredicates<DbModel.ProjectInvoiceAttachment>(Expression.OrElse);
                if (predicate != null)
                {
                    containsExpression = a => (a.ProjectId == projectId);
                    predicate = predicate.CombineWithAndAlso(containsExpression);
                }

                projectInvoiceAttachmentExists = this._projectInvoiceAttachmentRepository?.FindBy(predicate).ToList();

                projectInvoiceAttachmentExists?.ToList().ForEach(x =>
                {
                    string errorCode = MessageType.ProjectInvoiceAttachementExists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.DocumentType.Name)));
                });
            }

            //if (validationType == ValidationType.Add)
            //    projectInvoiceAttachmentExists = this._projectInvoiceAttachmentRepository?.FindBy(x => x.ProjectId == projectId && prjInvAttachments.Any(x1 => x.DocumentType.Name == x1.DocumentType && x.ProjectId == projectId)).ToList();

            //else if (validationType == ValidationType.Update)
            //    projectInvoiceAttachmentExists = this._projectInvoiceAttachmentRepository?.FindBy(x => x.ProjectId == projectId && prjInvAttachments.Any(x1 => x.DocumentType.Name == x1.DocumentType && x.ProjectId == projectId && x.Id != x1.ProjectInvoiceAttachmentId)).ToList();

            //projectInvoiceAttachmentExists?.ToList().ForEach(x =>
            //{
            //    string errorCode = MessageType.ProjectInvoiceAttachementExists.ToId();
            //    messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.DocumentType.Name)));
            //});

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }

        private bool IsRecordValidForProcess(IList<ProjectInvoiceAttachment> projectInvoiceAttachments, ValidationType validationType, ref IList<ProjectInvoiceAttachment> filteredProjectInvoiceAttachments, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredProjectInvoiceAttachments = projectInvoiceAttachments?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredProjectInvoiceAttachments = projectInvoiceAttachments?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredProjectInvoiceAttachments = projectInvoiceAttachments?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            return filteredProjectInvoiceAttachments?.Count > 0 ? IsAttachmentsHasValidSchema(filteredProjectInvoiceAttachments, validationType, ref validationMessages) : false;
        }

        private bool IsValidProjectInvoiceAttachment(IList<ProjectInvoiceAttachment> projectInvoiceAttachments, IList<DbModel.ProjectInvoiceAttachment> dbProjectInvoiceAttachments, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = projectInvoiceAttachments?.Where(x => !dbProjectInvoiceAttachments.ToList().Any(x1 => x1.Id == x.ProjectInvoiceAttachmentId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ProjectInvoiceAttachementIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Contract, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectInvoiceAttachmentId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsAttachmentsHasValidSchema(IList<ProjectInvoiceAttachment> projectInvoiceAttachments, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(projectInvoiceAttachments), validationType);
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
