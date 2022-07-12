using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Interfaces.Master;
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
    public class ProjectInvoiceReferenceService : IProjectInvoiceReferenceService
    {
        private IAppLogger<ProjectInvoiceReferenceService> _logger = null;
        private IProjectInvoiceReferenceRepository _projectInvoiceReferenceRepository = null;
        private IProjectRepository _projectRepository = null;
        private readonly IDataRepository _dataRepository = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IInvoiceReferenceValidationService _validationService = null;
        private readonly IMapper _mapper = null;
        private readonly IAuditSearchService _auditSearchService = null;

        public ProjectInvoiceReferenceService(IAppLogger<ProjectInvoiceReferenceService> logger,
                                              IProjectInvoiceReferenceRepository repository,
                                              IProjectRepository projectRepository,
                                              IDataRepository dataRepository,
                                              IInvoiceReferenceValidationService validationService,
                                              IAuditSearchService auditSearchService,
                                              IMapper mapper,
                                              JObject messages)
        {
            _logger = logger;
            _projectInvoiceReferenceRepository = repository;
            _projectRepository = projectRepository;
            _dataRepository = dataRepository;
            _validationService = validationService;
            _mapper = mapper;
            _auditSearchService = auditSearchService;
            this._messageDescriptions = messages;
        }

        #region Public Exposed Method
        public Response DeleteProjectInvoiceReferences(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferences, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Project> dbProjects = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = RemoveProjectInvoiceReferenceType(projectNumber, projectInvoiceReferences, dbProjects,dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectInvoiceReferences(new ProjectInvoiceReference() { ProjectNumber = projectNumber });
            else
                return result;
        }
        public Response DeleteProjectInvoiceReferences(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferences, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = RemoveProjectInvoiceReferenceType(projectNumber, projectInvoiceReferences, dbProjects,dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectInvoiceReferences(new ProjectInvoiceReference() { ProjectNumber = projectNumber });
            else
                return result;
        }

        public Response GetProjectInvoiceReferences(ProjectInvoiceReference searchModel)
        {
            IList<ProjectInvoiceReference> result = null;
            Exception exception = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                {
                    result = _projectInvoiceReferenceRepository.Search(searchModel);
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

        public Response ModifyProjectInvoiceReferences(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferences, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Project> dbProjects = null;
            IList<DbModel.Data> dbReference = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = UpdateProjectInvoiceReferenceType(projectNumber, projectInvoiceReferences, dbProjects, dbReference,dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectInvoiceReferences(new ProjectInvoiceReference() { ProjectNumber = projectNumber });
            else
                return result;
        }

        public Response ModifyProjectInvoiceReferences(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferences, IList<DbModel.Project> dbProjects, IList<DbModel.Data> dbReference, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = UpdateProjectInvoiceReferenceType(projectNumber, projectInvoiceReferences, dbProjects, dbReference,dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectInvoiceReferences(new ProjectInvoiceReference() { ProjectNumber = projectNumber });
            else
                return result;
        }

        public Response SaveProjectInvoiceReferences(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferences, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Project> dbProjects = null;
            IList<DbModel.Data> dbReference = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = AddProjectInvoiceReferenceType(projectNumber, projectInvoiceReferences, dbProjects, dbReference,dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectInvoiceReferences(new ProjectInvoiceReference() { ProjectNumber = projectNumber });
            else
                return result;
        }

        public Response SaveProjectInvoiceReferences(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferences, IList<DbModel.Project> dbProjects, IList<DbModel.Data> dbReference, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = AddProjectInvoiceReferenceType(projectNumber, projectInvoiceReferences, dbProjects, dbReference,dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectInvoiceReferences(new ProjectInvoiceReference() { ProjectNumber = projectNumber });
            else
                return result;
        }

        #endregion

        #region Private Exposed Methods

        private Response AddProjectInvoiceReferenceType(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferenceTypes, IList<DbModel.Project> dbProjects, IList<DbModel.Data> dbReferenceTypes, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Project dbProject = dbProjects?.Count > 0 ? dbProjects.FirstOrDefault() : null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                this._projectInvoiceReferenceRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                eventId = projectInvoiceReferenceTypes?.FirstOrDefault()?.EventId;
                IList<ProjectInvoiceReference> recordToBeInserted = null;
                IList<DbModel.ProjectInvoiceAssignmentReference> prjInvoiceReferenceTypes = null;
                //IList<DbModel.Data> dbReferenceTypes = null;
                validationMessages = new List<ValidationMessage>();

                bool IsValid = Validate(projectNumber, projectInvoiceReferenceTypes, ref recordToBeInserted, ref dbProject, ref dbReferenceTypes
                                       , ref prjInvoiceReferenceTypes, ValidationType.Add, ref validationMessages, ref errorMessages);

                if (IsValid && recordToBeInserted?.Count > 0)
                {
                    var dbProjectReferenceTypeToBeInserted = recordToBeInserted.Select(x => new DbModel.ProjectInvoiceAssignmentReference()
                    {
                        ProjectId = dbProject.Id,
                        AssignmentReferenceTypeId = dbReferenceTypes.FirstOrDefault(x1 => x1.Name == x.ReferenceType).Id,
                        SortOrder = Convert.ToByte(x.DisplayOrder),
                        IsAssignment = (bool)x.IsVisibleToAssignment,
                        IsVisit = (bool)x.IsVisibleToVisit,
                        IsTimesheet = (bool)x.IsVisibleToTimesheet,
                        ModifiedBy = x.ModifiedBy,
                    }).ToList();

                    this._projectInvoiceReferenceRepository.Add(dbProjectReferenceTypeToBeInserted);

                    if (commitChange && !_projectInvoiceReferenceRepository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                    {
                        int value = _projectInvoiceReferenceRepository.ForceSave();
                        if (value > 0)
                            dbProjectReferenceTypeToBeInserted?.ToList().ForEach(x1 =>
                            {
                                var tempProjectInvoiceReference = _mapper.Map<ProjectInvoiceReference>(x1);
                                tempProjectInvoiceReference.ReferenceType = dbReferenceTypes?.ToList()?.FirstOrDefault(referenceType => referenceType.Id == x1.AssignmentReferenceTypeId)?.Name;
                                _auditSearchService.AuditLog(recordToBeInserted.FirstOrDefault(), ref eventId, recordToBeInserted?.FirstOrDefault()?.ActionByUser,
                                     null,
                                     ValidationType.Add.ToAuditActionType(),
                                     SqlAuditModuleType.ProjectReference,
                                      null,
                                    tempProjectInvoiceReference,
                                    dbModule
                                          );
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectInvoiceReferenceTypes);
            }
            finally
            {
                _projectInvoiceReferenceRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool Validate(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferenceTypes,
                              ref IList<ProjectInvoiceReference> recordToProcess, ref DbModel.Project dbProject,
                              ref IList<DbModel.Data> dbReferenceTypes, ref IList<DbModel.ProjectInvoiceAssignmentReference> prjInvoiceReferenceTypes,
                              ValidationType validationType, ref List<ValidationMessage> validationMessages, ref List<MessageDetail> errorMessages)
        {

            if (IsRecordValidForProcess(projectInvoiceReferenceTypes, validationType, ref recordToProcess, ref errorMessages, ref validationMessages))
            {
                if (this.IsValidProject(projectNumber, ref dbProject, ref errorMessages))
                {
                    //using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                    //{
                    if (this.IsValidReferenceType(recordToProcess, ref dbReferenceTypes, ref errorMessages))
                    {
                        if (!this.IsProjectInvocieReferenceAlreadyAssociated(dbProject.Id, recordToProcess, ValidationType.Update, ref errorMessages))
                        {
                            if (validationType == ValidationType.Update)
                            {
                                var modRecordProjectInvoiceReferenceId = recordToProcess.Select(x => x.ProjectInvoiceReferenceTypeId).ToList();
                                prjInvoiceReferenceTypes = _projectInvoiceReferenceRepository.FindBy(x => x.ProjectId == projectNumber && modRecordProjectInvoiceReferenceId.Contains(x.Id))?.ToList();

                                if (IsValidProjectInvoiceReference(recordToProcess, prjInvoiceReferenceTypes, ref errorMessages))
                                {
                                    if (this.IsProjectInvoiceReferenceCanBeUpdated(recordToProcess, prjInvoiceReferenceTypes, ref errorMessages))
                                    {
                                        return true;
                                    }
                                    else
                                        return false;
                                }
                            }
                            return true;
                        }
                    }
                    // tranScope.Complete();
                    //}
                }
            }
            return recordToProcess?.Count == 0 ? true : false;
        }

        private Response UpdateProjectInvoiceReferenceType(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferenceTypes, IList<DbModel.Project> dbProjects, IList<DbModel.Data> dbReferenceTypes, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Project dbProject = dbProjects?.Count > 0 ? dbProjects.FirstOrDefault() : null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                this._projectInvoiceReferenceRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ProjectInvoiceReference> recordToBeModified = null;
                //IList<DbModel.Data> dbReferenceTypes = null;
                IList<DbModel.ProjectInvoiceAssignmentReference> prjInvoiceReferenceTypes = null;
                eventId = projectInvoiceReferenceTypes?.FirstOrDefault()?.EventId;

                validationMessages = new List<ValidationMessage>();

                bool IsValid = Validate(projectNumber, projectInvoiceReferenceTypes, ref recordToBeModified, ref dbProject, ref dbReferenceTypes
                         , ref prjInvoiceReferenceTypes, ValidationType.Update, ref validationMessages, ref errorMessages);

                if (IsValid && recordToBeModified?.Count > 0)
                {
                    IList<ProjectInvoiceReference> domExistingProjectInvoiceReference = new List<ProjectInvoiceReference>();
                    prjInvoiceReferenceTypes.ToList().ForEach(x =>
                    {
                        domExistingProjectInvoiceReference.Add(ObjectExtension.Clone(_mapper.Map<ProjectInvoiceReference>(x)));
                    });
                    foreach (var dbProjectInvReference in prjInvoiceReferenceTypes)
                    {
                        var projectInvoiceReference = recordToBeModified.FirstOrDefault(x => x.ProjectInvoiceReferenceTypeId == dbProjectInvReference.Id);
                        dbProjectInvReference.AssignmentReferenceTypeId = dbReferenceTypes.FirstOrDefault(x1 => x1.Name == projectInvoiceReference.ReferenceType).Id;
                        dbProjectInvReference.SortOrder = Convert.ToByte(projectInvoiceReference.DisplayOrder);
                        dbProjectInvReference.IsAssignment = Convert.ToBoolean(projectInvoiceReference.IsVisibleToAssignment);
                        dbProjectInvReference.IsVisit = Convert.ToBoolean(projectInvoiceReference.IsVisibleToVisit);
                        dbProjectInvReference.IsTimesheet = Convert.ToBoolean(projectInvoiceReference.IsVisibleToTimesheet);
                        dbProjectInvReference.LastModification = DateTime.UtcNow;
                        dbProjectInvReference.UpdateCount = projectInvoiceReference.UpdateCount.CalculateUpdateCount();
                        dbProjectInvReference.ModifiedBy = projectInvoiceReference.ModifiedBy;

                        _projectInvoiceReferenceRepository.Update(dbProjectInvReference);
                    }

                    if (commitChange && recordToBeModified?.Count > 0)
                    {
                        int value = _projectInvoiceReferenceRepository.ForceSave();
                        if (value > 0)
                            recordToBeModified?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                           null,
                                          ValidationType.Update.ToAuditActionType(),
                                          SqlAuditModuleType.ProjectReference,
                                         domExistingProjectInvoiceReference?.FirstOrDefault(x2 => x2.ProjectInvoiceReferenceTypeId == x1.ProjectInvoiceReferenceTypeId),
                                          x1,dbModule
                                          ));
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectInvoiceReferenceTypes);
            }
            finally
            {
                _projectInvoiceReferenceRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response RemoveProjectInvoiceReferenceType(int projectNumber, IList<ProjectInvoiceReference> projectInvoiceReferenceTypes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Project dbProject = dbProjects?.Count > 0 ? dbProjects.FirstOrDefault() : null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _projectInvoiceReferenceRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ProjectInvoiceReference> recordToBedeleted = null;
                validationMessages = new List<ValidationMessage>();
                eventId = projectInvoiceReferenceTypes?.FirstOrDefault()?.EventId;
                if (this.IsRecordValidForProcess(projectInvoiceReferenceTypes, ValidationType.Delete, ref recordToBedeleted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsValidProject(projectNumber, ref dbProject, ref errorMessages))
                    {
                        var modRecordProjectReferenceTypeId = recordToBedeleted.Select(x => x.ProjectInvoiceReferenceTypeId).ToList();
                        var dbProjectInvoiceReferenceTypes = _projectInvoiceReferenceRepository.FindBy(x => x.ProjectId == dbProject.Id && modRecordProjectReferenceTypeId.Contains(x.Id));

                        if (IsValidProjectInvoiceReference(recordToBedeleted, dbProjectInvoiceReferenceTypes.ToList(), ref errorMessages))
                        {
                            foreach (var projectInvReferenceType in dbProjectInvoiceReferenceTypes)
                                _projectInvoiceReferenceRepository.Delete(projectInvReferenceType);

                            if (commitChange && recordToBedeleted?.Count > 0)
                            {
                                int value = _projectInvoiceReferenceRepository.ForceSave();
                                if (value > 0)
                                {
                                    recordToBedeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                         null,
                                                                                                         ValidationType.Delete.ToAuditActionType(),
                                                                                                         SqlAuditModuleType.ProjectReference,
                                                                                                         x1,
                                                                                                         null,
                                                                                                         dbModule));
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectInvoiceReferenceTypes);
            }
            finally
            {
                _projectInvoiceReferenceRepository.AutoSave = true;
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

            return messageType == MessageType.Success;
        }

        private bool IsValidReferenceType(IList<ProjectInvoiceReference> projectInvoiceReferenceTypes, ref IList<DbModel.Data> dbInvoiceReferenceTypes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            IList<DbModel.Data> dbReferenceTypes = null;
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var referenceTypeNames = projectInvoiceReferenceTypes.Select(x => x.ReferenceType).ToList();

            if (dbInvoiceReferenceTypes?.Count > 0)
                dbInvoiceReferenceTypes = dbInvoiceReferenceTypes.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.AssignmentReferenceType) &&
                                                                 referenceTypeNames.Contains(x.Name)).ToList();
            else
                dbInvoiceReferenceTypes = this._dataRepository.FindBy(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.AssignmentReferenceType) &&
                                                                 referenceTypeNames.Contains(x.Name)).ToList();

            dbReferenceTypes = dbInvoiceReferenceTypes;
            var referenceTypeNotExists = projectInvoiceReferenceTypes.Where(x => !dbReferenceTypes.Any(x1 => x1.Name == x.ReferenceType)).ToList();
            referenceTypeNotExists.ForEach(x =>
            {
                string errorCode = MessageType.ProjectInvoiceReferenceTypeNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ReferenceType)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);
            return errorMessages?.Count <= 0;
        }

        private bool IsProjectInvoiceReferenceCanBeUpdated(IList<ProjectInvoiceReference> projectInvoiceReferenceTypes, IList<DbModel.ProjectInvoiceAssignmentReference> dbProjectInvoiceReferences, ref List<MessageDetail> errorMessages)
        {
            //using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            //{

            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = projectInvoiceReferenceTypes?.Where(x => !dbProjectInvoiceReferences.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.ProjectInvoiceReferenceTypeId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ProjectInvoiceReferenceRecordUpdatedAlready.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ReferenceType)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);
            //tranScope.Complete();
            return errorMessages?.Count <= 0;
            //}
        }

        private bool IsProjectInvocieReferenceAlreadyAssociated(int projectId, IList<ProjectInvoiceReference> projectInvoiceReferenceTypes, ValidationType validationType, ref List<MessageDetail> errorMessages)
        {
            IList<DbModel.ProjectInvoiceAssignmentReference> projectInvoiceReferenceExists = null;
            List<MessageDetail> messages = new List<MessageDetail>();

            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var prjInvReferences = projectInvoiceReferenceTypes?.Select(x => new { x.ReferenceType, x.ProjectInvoiceReferenceTypeId }).ToList();

            if (prjInvReferences?.Count > 0)
            {
                var filterExpressions = new List<Expression<Func<DbModel.ProjectInvoiceAssignmentReference, bool>>>();
                Expression<Func<DbModel.ProjectInvoiceAssignmentReference, bool>> predicate = null;
                Expression<Func<DbModel.ProjectInvoiceAssignmentReference, bool>> containsExpression = null;
                if (validationType == ValidationType.Add)
                {
                    foreach (var prjInvRefs in prjInvReferences)
                    {
                        containsExpression = a => a.AssignmentReferenceType.Name == prjInvRefs.ReferenceType;
                        filterExpressions.Add(containsExpression);
                    }
                }
                else if (validationType == ValidationType.Update)
                {
                    foreach (var prjInvRefs in prjInvReferences)
                    {
                        containsExpression = a => a.AssignmentReferenceType.Name == prjInvRefs.ReferenceType && a.Id != prjInvRefs.ProjectInvoiceReferenceTypeId;
                        filterExpressions.Add(containsExpression);
                    }
                }
                predicate = filterExpressions.CombinePredicates<DbModel.ProjectInvoiceAssignmentReference>(Expression.OrElse);
                if (predicate != null)
                {
                    containsExpression = a => (a.ProjectId == projectId);
                    predicate = predicate.CombineWithAndAlso(containsExpression);
                }

                projectInvoiceReferenceExists = this._projectInvoiceReferenceRepository?.FindBy(predicate).ToList();

                projectInvoiceReferenceExists?.ToList().ForEach(x =>
                {
                    string errorCode = MessageType.ProjectInvoiceReferenceExists.ToId();
                    messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.AssignmentReferenceType.Name)));
                });
            }

            //if (validationType == ValidationType.Add)
            //    projectInvoiceReferenceExists = this._projectInvoiceReferenceRepository?.FindBy(x =>  x.ProjectId == projectId && prjInvReferences.Any(x1 => x.AssignmentReferenceType.Name == x1.ReferenceType && x.ProjectId == projectId)).ToList();

            //else if (validationType == ValidationType.Update)
            //    projectInvoiceReferenceExists = this._projectInvoiceReferenceRepository?.FindBy(x => x.ProjectId == projectId && prjInvReferences.Any(x1 => x.AssignmentReferenceType.Name == x1.ReferenceType && x.ProjectId == projectId && x.Id != x1.ProjectInvoiceReferenceTypeId)).ToList();

            //projectInvoiceReferenceExists?.ToList().ForEach(x =>
            //{
            //    string errorCode = MessageType.ProjectInvoiceReferenceExists.ToId();
            //    messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.AssignmentReferenceType.Name)));
            //});

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count > 0;
        }

        private bool IsRecordValidForProcess(IList<ProjectInvoiceReference> projectInvoiceReferenceTypes, ValidationType validationType, ref IList<ProjectInvoiceReference> filteredProjectInvoiceReferenceTypes, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredProjectInvoiceReferenceTypes = projectInvoiceReferenceTypes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredProjectInvoiceReferenceTypes = projectInvoiceReferenceTypes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredProjectInvoiceReferenceTypes = projectInvoiceReferenceTypes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            return filteredProjectInvoiceReferenceTypes?.Count > 0 ? IsReferenceHasValidSchema(filteredProjectInvoiceReferenceTypes, validationType, ref validationMessages) : false;
        }

        private bool IsValidProjectInvoiceReference(IList<ProjectInvoiceReference> projectInvoiceReferenceTypes, IList<DbModel.ProjectInvoiceAssignmentReference> dbProjectInvoiceReferenceTypes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = projectInvoiceReferenceTypes?.Where(x => !dbProjectInvoiceReferenceTypes.ToList().Any(x1 => x1.Id == x.ProjectInvoiceReferenceTypeId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ProjectInvoiceReferenceTypeIdIsInvalidOrNotExists.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.ProjectInvoiceReferenceTypeId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private bool IsReferenceHasValidSchema(IList<ProjectInvoiceReference> projectInvoiceReferences, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(projectInvoiceReferences), validationType);
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
