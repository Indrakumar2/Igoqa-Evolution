using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
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
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Core.Services
{
    public class ProjectNotesService : IProjectNotesService
    {
        private readonly IMapper _mapper = null;
        private IAppLogger<ProjectNotesService> _logger = null;
        private IProjectNoteRepository _repository = null;
        private IProjectRepository _projectRepository = null;
        private readonly JObject _MessageDescriptions = null;
        private readonly IProjectNoteValidationService _validationService = null;
        private readonly IAuditSearchService _auditSearchService = null;


        public ProjectNotesService(IMapper mapper,
                                   IAppLogger<ProjectNotesService> logger,
                                   IProjectNoteRepository repository,
                                   IProjectRepository projectRepository,
                                   IProjectNoteValidationService validationService,
                                    JObject messages,
                                   IAuditSearchService auditSearchService)
        {
            _mapper = mapper;
            _logger = logger;
            _repository = repository;
            _projectRepository = projectRepository;
            _validationService = validationService;
            _auditSearchService = auditSearchService;
            _MessageDescriptions = messages;
        }
        #region PublicMethods 
        public Response DeleteProjectNotes(int projectNumebr, IList<ProjectNote> notes, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.RemoveProjectNotes(projectNumebr, notes, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectNotes(new ProjectNote() { ProjectNumber = projectNumebr });
            else
                return result;
        }

        public Response GetProjectNotes(ProjectNote searchModel)
        {
            IList<DomModel.ProjectNote> result = null;
            Exception exception = null;

            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                {
                    result = this._repository.Search(searchModel);
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


        public Response SaveProjectNotes(int projectNumebr, IList<ProjectNote> notes, bool commitChange = true, bool isResultSetRequired = false)
        {
            IList<DbModel.Project> dbProjects = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            var result = this.AddProjectNotes(projectNumebr, notes, dbProjects,dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectNotes(new ProjectNote() { ProjectNumber = projectNumebr });
            else
                return result;
        }

        public Response SaveProjectNotes(int projectNumebr, IList<ProjectNote> notes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.AddProjectNotes(projectNumebr, notes, dbProjects,dbModule, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectNotes(new ProjectNote() { ProjectNumber = projectNumebr });
            else
                return result;
        }

        //D661 issue 8 Start
        public Response ModifyProjectNotes(int projectNumebr, IList<ProjectNote> notes, bool commitChange = true, bool isResultSetRequired = false)
        {
            var result = this.ModifyProjectNotes(projectNumebr, notes, commitChange);
            if (result.Code == MessageType.Success.ToId() && isResultSetRequired)
                return this.GetProjectNotes(new ProjectNote() { ProjectNumber = projectNumebr });
            else
                return result;
        }
        //D661 issue 8 End
        #endregion

        #region PrivateMethods
        private bool IsvalidProject(int projectNumber, ref DbModel.Project project, ref List<MessageDetail> errorMessages)
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
                errorMessages.Add(new MessageDetail(ModuleType.Project, MessageType.InvalidProjectNumber.ToId(), _MessageDescriptions[MessageType.InvalidProjectNumber.ToId()].ToString()));

            return messageType == MessageType.Success;


        }

        private bool IsRecordValidForProcess(IList<ProjectNote> ProjectNotes, ValidationType validationType, ref IList<ProjectNote> filteredNotes, ref List<MessageDetail> errorMessages, ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredNotes = ProjectNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredNotes = ProjectNotes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)       //D661 issue 8 
                filteredNotes = ProjectNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            return filteredNotes?.Count > 0 ? IsNotesHasValidSchema(filteredNotes, validationType, ref validationMessages) : false;
        }

        private bool IsValidProjectNotes(IList<ProjectNote> projectNotes, IList<DbRepository.Models.SqlDatabaseContext.ProjectNote> notes, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = projectNotes.Where(x => !notes.ToList().Any(x1 => x1.Id == x.ProjectNoteId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.ProjectNotesIdIsInvalidOrNotExsists.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.ProjectNoteId)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private Response AddProjectNotes(int projectNumber, IList<ProjectNote> projectNotes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Project dbProject = dbProjects?.Count > 0 ? dbProjects.FirstOrDefault() : null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = projectNotes?.FirstOrDefault()?.EventId;
                IList<ProjectNote> recordToBeInserted = null;
                if (this.IsRecordValidForProcess(projectNotes, ValidationType.Add, ref recordToBeInserted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsvalidProject(projectNumber, ref dbProject, ref errorMessages))
                    {

                        var dbNotesToBeInserted = this._mapper.Map<List<DbModel.ProjectNote>>(recordToBeInserted);
                        dbNotesToBeInserted.ForEach(x =>
                        {
                            x.Id = 0;
                            x.ProjectId = dbProject.Id;
                            x.UpdateCount = null;
                            x.CreatedDate = x.CreatedDate;
                        });
                        _repository.Add(dbNotesToBeInserted);

                        if (commitChange && !_repository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                        {
                            int value = _repository.ForceSave();
                            if (value > 0)
                            {
                                dbNotesToBeInserted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.CreatedBy,
                                                                                                 null,
                                                                                                  ValidationType.Add.ToAuditActionType(),
                                                                                                  SqlAuditModuleType.ProjectNote,
                                                                                                   null,
                                                                                                    _mapper.Map<ProjectNote>(x1),
                                                                                                    dbModule
                                                                                                   ));
                            }

                        }
                    }


                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectNotes);

            }

            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
        //D661 issue 8 Start
        private Response ModifyProjectNotes(int projectNumber, IList<ProjectNote> projectNotes, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Project dbProject = null;
            IList<DbModel.ProjectNote> dbProjectNote = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = projectNotes?.FirstOrDefault()?.EventId;
                IList<ProjectNote> recordToBeModify = null;
                if (this.IsRecordValidForProcess(projectNotes, ValidationType.Update, ref recordToBeModify, ref errorMessages, ref validationMessages))
                {
                    if (this.IsvalidProject(projectNumber, ref dbProject, ref errorMessages))
                    {
                        dbProjectNote = GetProjectNote(recordToBeModify, ValidationType.Update);
                        if (dbProjectNote?.Count > 0)
                        {
                            if (this.IsValidProjectNotes(recordToBeModify, dbProjectNote, ref errorMessages))//D661 issue 8 
                            {
                                if (this.IsRecordUpdateCountMatching(recordToBeModify, dbProjectNote, ref errorMessages))
                                {
                                    if (recordToBeModify?.Count > 0)
                                    {
                                        dbProjectNote.ToList().ForEach(dbProjectNoteValue =>
                                        {
                                            var dbProjectNoteToBeModify = recordToBeModify.FirstOrDefault(x => x.ProjectNoteId == dbProjectNoteValue.Id);
                                            if (dbProjectNoteToBeModify != null)
                                            {
                                                dbProjectNoteValue.ProjectId = dbProject.Id;
                                                dbProjectNoteValue.Id = (int)dbProjectNoteToBeModify.ProjectNoteId;
                                                dbProjectNoteValue.CreatedBy = dbProjectNoteToBeModify.CreatedBy;
                                                dbProjectNoteValue.Note = dbProjectNoteToBeModify.Notes;
                                                dbProjectNoteValue.CreatedDate = dbProjectNoteValue.CreatedDate;
                                                dbProjectNoteValue.LastModification = DateTime.UtcNow;
                                                dbProjectNoteValue.UpdateCount = dbProjectNoteToBeModify.UpdateCount.CalculateUpdateCount();
                                                dbProjectNoteValue.ModifiedBy = dbProjectNoteToBeModify.ModifiedBy;
                                            }

                                        });
                                        _repository.AutoSave = false;
                                        _repository.Update(dbProjectNote);
                                        if (commitChange && recordToBeModify?.Count > 0)
                                            _repository.ForceSave();
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectNotes);

            }

            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }
        //D661 issue 8 End
        private Response RemoveProjectNotes(int projectNumber, IList<ProjectNote> projectNotes, bool commitChange)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            DbModel.Project dbProject = null;
            List<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                _repository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ProjectNote> recordToBeDeleted = null;
                validationMessages = new List<ValidationMessage>();
                eventId = projectNotes?.FirstOrDefault()?.EventId;

                if (this.IsRecordValidForProcess(projectNotes, ValidationType.Delete, ref recordToBeDeleted, ref errorMessages, ref validationMessages))
                {
                    if (this.IsvalidProject(projectNumber, ref dbProject, ref errorMessages))
                    {
                        var ProjectNoteId = recordToBeDeleted.Select(x => x.ProjectNoteId).ToList();
                        var prjNotes = _repository.FindBy(x => x.ProjectId == dbProject.Id && ProjectNoteId.Contains(x.Id));
                        if (this.IsValidProjectNotes(projectNotes, prjNotes.ToList(), ref errorMessages))
                        {
                            var dbProjectNotes = _repository.FindBy(x => recordToBeDeleted.Select(x1 => x1.ProjectNoteId).Contains(x.Id) && x.Project.ProjectNumber == projectNumber).ToList();

                            foreach (var notes in dbProjectNotes)
                                _repository.Delete(notes);

                            if (commitChange && !_repository.AutoSave && dbProjectNotes?.Count > 0)
                            {
                                int value = _repository.ForceSave();
                                if (value > 0)
                                {
                                    projectNotes?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                       null,
                                                                                                      ValidationType.Delete.ToAuditActionType(),
                                                                                                      SqlAuditModuleType.ProjectNote,
                                                                                                       x1,
                                                                                                        null
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectNotes);
            }
            finally
            {
                _repository.AutoSave = false;
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private bool IsNotesHasValidSchema(IList<ProjectNote> notes, ValidationType validationType, ref List<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(notes), validationType);
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

        //private Response AuditLog(ProjectNote projectNote,
        //                          SqlAuditActionType sqlAuditActionType,
        //                          SqlAuditModuleType sqlAuditModuleType,
        //                          object oldData,
        //                          object newData)
        //{
        //    LogEventGeneration logEventGeneration = new LogEventGeneration(_auditLogger);
        //    Exception exception = null;
        //    long? eventId = 0;
        //    if (projectNote != null && !string.IsNullOrEmpty(projectNote.ActionByUser))
        //    {
        //        string actionBy = projectNote.ActionByUser;
        //        if (projectNote.EventId > 0)
        //            eventId = projectNote.EventId;
        //        else
        //            eventId = logEventGeneration.GetEventLogId(eventId,
        //                                                          sqlAuditActionType,
        //                                                          actionBy,
        //                                                          projectNote.ProjectNumber.ToString(),
        //                                                          SqlAuditModuleType.ProjectNote.ToString());

        //        return _auditLogger.LogAuditData((long)eventId, sqlAuditModuleType, oldData, newData);
        //    }
        //    return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        //}
        //D661 issue 8 Start
        private bool IsRecordUpdateCountMatching(IList<ProjectNote> projectNote, IList<DbModel.ProjectNote> dbProjectNote, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            var notMatchedRecords = projectNote.Where(x => !dbProjectNote.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.ProjectNoteId)).ToList();

            notMatchedRecords.ForEach(x =>
            {
                string errorCode = MessageType.NotesHasBeenUpdatedByOther.ToId();
                messages.Add(new MessageDetail(ModuleType.Project, errorCode, string.Format(_MessageDescriptions[errorCode].ToString(), x.Notes)));
            });

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count <= 0;
        }

        private IList<DbModel.ProjectNote> GetProjectNote(IList<ProjectNote> projectNotes, ValidationType validationType)
        {
            IList<DbModel.ProjectNote> dbProjectNote = null;
            if (validationType != ValidationType.Add)
            {
                if (projectNotes?.Count > 0)
                {
                    var projectNoteId = projectNotes.Select(x => x.ProjectNoteId).Distinct().ToList();
                    dbProjectNote = _repository.FindBy(x => projectNoteId.Contains(x.Id)).ToList();
                }
            }
            return dbProjectNote;
        }
        //D661 issue 8 End
        #endregion

    }
}
