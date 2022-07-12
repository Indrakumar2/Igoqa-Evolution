using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Email.Models;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Project.Domain.Interfaces.Data;
using Evolution.Project.Domain.Interfaces.Projects;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Project.Domain.Models.Projects;

namespace Evolution.Project.Core.Services
{
    public class ProjectDetailService : IProjectDetailService
    {
        private readonly IAppLogger<ProjectDetailService> _logger = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IProjectService _projectService = null;
        private readonly IProjectInvoiceAttachmentService _projectInvoiceAttachmentService = null;
        private readonly IProjectInvoiceReferenceService _projectReferenceTypeService = null;
        private readonly IProjectClientNotificationService _projectClientNotificationService = null;
        private readonly IDocumentService _documentService = null;
        private readonly IProjectNotesService _projectNoteService = null;
        private readonly IProjectRepository _projectRepository = null;
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IUserService _userService = null;
        private readonly IEmailQueueService _emailService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly IMapper _mapper = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        public readonly string _emailDocumentEndpoint = "documents/UploadDocuments";

        public ProjectDetailService(DbModel.EvolutionSqlDbContext dbContext,
                                    IProjectService projectService,
                                    IProjectInvoiceAttachmentService projectInvoiceAttachmentService,
                                    IProjectInvoiceReferenceService projectReferenceTypeService,
                                    IProjectClientNotificationService projectClientNotificationService,
                                    IDocumentService documentService,
                                    IProjectNotesService projectNoteService,
                                    IAuditLogger auditLogger,
                                    IProjectRepository projectRepository,
                                    IUserService userService,
                                    IEmailQueueService emailService,
                                    IAuditSearchService auditSearchService,
                                    IAppLogger<ProjectDetailService> logger,
                                    IMapper mapper, JObject messages ,IOptions<AppEnvVariableBaseModel> environment)
        {
            this._dbContext = dbContext;
            this._projectService = projectService;
            this._projectInvoiceAttachmentService = projectInvoiceAttachmentService;
            this._projectReferenceTypeService = projectReferenceTypeService;
            this._projectClientNotificationService = projectClientNotificationService;
            this._documentService = documentService;
            this._projectNoteService = projectNoteService;
            this._projectRepository = projectRepository;
            this._auditSearchService = auditSearchService;
            this._userService = userService;
            this._emailService = emailService;
            this._messageDescriptions = messages;
            _mapper = mapper;
            _environment = environment.Value;
            this._logger = logger;

        }

        #region Public Exposed Method

        public Response DeleteProjectDetail(DomainModel.ProjectDetail projectDetail)
        {
            Exception exception = null;
            Response response = null;
            long? eventId = 0;
            IList<DbModel.SqlauditModule> dbModule = null;
            try
            {
                if (projectDetail != null && projectDetail.ProjectInfo != null)
                {
                    IList<DbModel.Project> dbProjects = null;
                    response = this._projectService.ProjectValidForDeletion(new List<DomainModel.Project> { projectDetail.ProjectInfo }, ref dbProjects);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        //To-Do: Will create helper method get TransactionScope instance based on requirement
                        using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                        //using (var tranScope = new TransactionScope())
                        {
                            if(dbProjects!=null)
                            {
                                var count = _projectRepository.DeleteProject(dbProjects.FirstOrDefault().Id);
                                if (count > 0)
                                {
                                    dbModule = _auditSearchService.GetAuditModule(new List<string>() { SqlAuditModuleType.Project.ToString() });
                                    response = _auditSearchService.AuditLog(projectDetail, ref eventId, projectDetail?.ProjectInfo?.ActionByUser?.ToString(),
                                                                    "{" + AuditSelectType.Id + ":" + projectDetail.ProjectInfo.ProjectNumber + "}${" + AuditSelectType.ProjectNumber + ":" + projectDetail.ProjectInfo.ProjectNumber
                                                                    + "}${" + AuditSelectType.CustomerProjectName + ":" + projectDetail.ProjectInfo.CustomerProjectName.Trim()
                                                                    + "}${" + AuditSelectType.CustomerProjectNumber + ":" + projectDetail.ProjectInfo.CustomerProjectNumber.Trim()
                                                                    + "}${" + AuditSelectType.ContractNumber + ":" + projectDetail.ProjectInfo.ContractNumber + "}", SqlAuditActionType.D, SqlAuditModuleType.Project,
                                                                    projectDetail.ProjectInfo, null, dbModule);
                                    tranScope.Complete();
                                    response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
                                }
                            }
                        }
                    }
                }
                else if (projectDetail == null || projectDetail?.ProjectInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, projectDetail, MessageType.InvalidPayLoad, projectDetail }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectDetail);
            }

            return response;
        }

        public Response SaveProjectDetail(DomainModel.ProjectDetail projectDetail)
        {
            Exception exception = null;
            try
            {
                if (projectDetail != null && projectDetail.ProjectInfo != null)
                    return ProcessProjectDetail(projectDetail, ValidationType.Add);
                else if (projectDetail == null || projectDetail?.ProjectInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, projectDetail, MessageType.InvalidPayLoad, projectDetail }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectDetail);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response UpdateProjectDetail(DomainModel.ProjectDetail projectDetail)
        {
            Exception exception = null;
            try
            {
                if (projectDetail != null && projectDetail.ProjectInfo != null)
                    return ProcessProjectDetail(projectDetail, ValidationType.Update);
                else if (projectDetail == null || projectDetail?.ProjectInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, projectDetail, MessageType.InvalidPayLoad, projectDetail }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectDetail);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        #endregion

        #region Private Exposed Methods

        private Response ProcessProjectDetail(DomainModel.ProjectDetail projectDetail, ValidationType validationType)
        {
            bool commitChanges = true;
            Response response = null;
            Exception exception = null;
            DomainModel.Project newProject = null;
            IList<DbModel.Project> dbProject = null;
            IList<DbModel.Data> dbRefrence = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            int projectNumber = 0;
            long? eventId = null;
            try
            {
                if (projectDetail != null)
                {
                    using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                    {
                        this._projectRepository.AutoSave = false;
                        projectNumber = projectDetail?.ProjectInfo?.ProjectNumber == null ? 0 : (int)projectDetail?.ProjectInfo?.ProjectNumber;
                        if (projectDetail.ProjectInvoiceReferences?.Count > 0)
                            projectDetail.ProjectInfo.ProjectReferences = projectDetail.ProjectInvoiceReferences?.Where(x => !string.IsNullOrEmpty(x.ReferenceType))?.Select(x=>x.ReferenceType)?.ToList();
                        response = this.ProcessProjectInfo(new List<DomainModel.Project> { projectDetail.ProjectInfo },ref dbProject,ref dbRefrence,ref eventId, ref dbModule,commitChanges, validationType);
                        if (response.Code == MessageType.Success.ToId() && dbProject?.Count > 0)
                        {
                            if (response.Result != null)
                            {
                                newProject = response.Result?.Populate<IList<DomainModel.Project>>()?.FirstOrDefault();
                                projectNumber = newProject.ProjectNumber.Value;
                                AppendEvent(projectDetail, eventId);

                                response = this.ProcessProjectClientNotification(projectNumber, projectDetail.ProjectNotifications,dbProject, dbModule,commitChanges);
                                if (response.Code != MessageType.Success.ToId())
                                    return response;
                                response = this.ProcessProjectInvoiceAttachments(projectNumber, projectDetail.ProjectInvoiceAttachments,dbProject, dbModule,commitChanges);
                                if (response.Code != MessageType.Success.ToId())
                                    return response;
                                response = this.ProcessProjectInvoiceReferences(projectNumber, projectDetail.ProjectInvoiceReferences,dbProject,dbRefrence, dbModule, commitChanges);
                                if (response.Code != MessageType.Success.ToId())
                                    return response;
                                response = this.ProcessProjectDocument(projectNumber, projectDetail.ProjectDocuments, dbModule,commitChanges, projectDetail, validationType.ToAuditActionType(), ref eventId);
                                if (response.Code != MessageType.Success.ToId())
                                    return response;
                                response = this.ProcessProjectNote(projectNumber, projectDetail.ProjectNotes,dbProject, dbModule,commitChanges);
                                if (response.Code != MessageType.Success.ToId())
                                    return response;
                                if (validationType == ValidationType.Add)
                                    ProcessEmailNotifications(newProject,projectDetail.ProjectInfo.ProjectCoordinatorEmail, projectDetail,ref eventId, dbModule);

                                if (response != null && response.Code == MessageType.Success.ToId())
                                {
                                    tranScope.Complete();
                                }
                            }
                        }
                        else
                            return response;
                    }
                }
            }
            catch (Exception ex)
            {
                // this.RollbackTransaction();
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectDetail);
            }
            finally
            {
                this._projectRepository.AutoSave = true;
                // this.RollbackTransaction();
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, newProject, exception);
        }

        private Response ProcessProjectInfo(IList<DomainModel.Project> project,ref IList<DbModel.Project> dbProjects, ref IList<DbModel.Data> dbReference, ref long? eventId, ref IList<DbModel.SqlauditModule> dbModule, bool commitChanges, ValidationType validationType)
        {
            Exception exception = null;
            // projectId = -1;
            try
            {
                if (project != null)
                {          
                    if (validationType == ValidationType.Delete)
                        return this._projectService.DeleteProjects(project, ref eventId, dbModule, commitChanges);
                    else if (validationType == ValidationType.Add)
                        return this._projectService.SaveProjects(project,ref dbProjects,ref dbReference, ref eventId,ref dbModule, commitChanges);
                    else if (validationType == ValidationType.Update)
                        return this._projectService.ModifyProjects(project, ref dbProjects, ref dbReference, ref eventId, ref dbModule, commitChanges);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), project);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessProjectClientNotification(int projectNumber, IList<DomainModel.ProjectClientNotification> projectClientNotifications, IList<DbModel.Project> dbProjects,IList<DbModel.SqlauditModule> dbModule, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            try
            {
                if (projectClientNotifications != null && projectClientNotifications.Count > 0)
                {
                    response = this._projectClientNotificationService.DeleteProjectClientNotifications(projectNumber, projectClientNotifications,dbModule, commitChanges);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._projectClientNotificationService.SaveProjectClientNotifications(projectNumber, projectClientNotifications, dbProjects, dbModule, commitChanges);
                        if (response.Code == MessageType.Success.ToId())
                        {
                            response = this._projectClientNotificationService.ModifyProjectClientNotifications(projectNumber, projectClientNotifications, dbModule,commitChanges);
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectClientNotifications);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessProjectInvoiceAttachments(int projectNumber, IList<DomainModel.ProjectInvoiceAttachment> projectInvoiceAttachments, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            try
            {
                if (projectInvoiceAttachments != null && projectInvoiceAttachments.Count > 0)
                {
                    response = this._projectInvoiceAttachmentService.DeleteProjectInvoiceAttachments(projectNumber, projectInvoiceAttachments,dbProjects, dbModule,commitChanges);//, projectId);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._projectInvoiceAttachmentService.SaveProjectInvoiceAttachments(projectNumber, projectInvoiceAttachments,dbProjects, dbModule, commitChanges);//, projectId);
                        if (response.Code == MessageType.Success.ToId())
                        {
                            response = this._projectInvoiceAttachmentService.ModifyProjectInvoiceAttachments(projectNumber, projectInvoiceAttachments,dbProjects, dbModule, commitChanges);//, projectId);
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectInvoiceAttachments);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessProjectInvoiceReferences(int projectNumber, IList<DomainModel.ProjectInvoiceReference> projectInvoiceReferences, IList<DbModel.Project> dbProjects, IList<DbModel.Data> dbReference,  IList<DbModel.SqlauditModule> dbModule, bool commitChanges)
        {
            Response response = null;
            Exception exception = null;
            try
            {
                if (projectInvoiceReferences != null && projectInvoiceReferences.Count > 0)
                {
                    response = this._projectReferenceTypeService.DeleteProjectInvoiceReferences(projectNumber, projectInvoiceReferences, dbProjects, dbModule, commitChanges);//, projectId);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._projectReferenceTypeService.SaveProjectInvoiceReferences(projectNumber, projectInvoiceReferences, dbProjects, dbReference, dbModule, commitChanges);//, projectId);
                        if (response.Code == MessageType.Success.ToId())
                        {
                            response = this._projectReferenceTypeService.ModifyProjectInvoiceReferences(projectNumber, projectInvoiceReferences, dbProjects, dbReference, dbModule, commitChanges);//, projectId);
                        }
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectInvoiceReferences);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessProjectDocument(int projectNumber, IList<ModuleDocument> projectDocuments, IList<DbModel.SqlauditModule> dbModule, bool commitChanges, DomainModel.ProjectDetail projectDetail, SqlAuditActionType sqlAuditActionType, ref long? eventId)
        {
            Response response = null;
            Exception exception = null;
            List<DbModel.Document> dbDocuments = null;
            try
            {
                if (projectDocuments != null && projectDocuments.Count > 0)
                {
                    projectDocuments?.ToList()?.ForEach(x => { x.ModuleRefCode = projectNumber.ToString(); });
                    projectDetail.ProjectDocuments = projectDocuments;
                    var auditProjectDetails = ObjectExtension.Clone(projectDetail);

                    if (projectDocuments.Any(x => x.RecordStatus.IsRecordStatusDeleted())) 
                        response = this._documentService.Delete(projectDocuments, commitChanges);//, projectId);

                    if (projectDocuments.Any(x => x.RecordStatus.IsRecordStatusNew()))
                    {
                        response = this._documentService.Save(projectDocuments, ref dbDocuments, commitChanges);//, projectId);
                        auditProjectDetails.ProjectDocuments = projectDocuments.Select(x =>
                        {
                            x.CreatedOn = dbDocuments?.FirstOrDefault(y => y.DocumentUniqueName == x.DocumentUniqueName)?.CreatedDate;
                            return x;
                        }).ToList(); // audit create date issue fix
                    }
                    
                    if (projectDocuments.Any(x => x.RecordStatus.IsRecordStatusModified()))
                        response = this._documentService.Modify(projectDocuments, ref dbDocuments, commitChanges);//, projectId);
                       
                    if (response.Code == MessageType.Success.ToId()) 
                        DocumentAudit(auditProjectDetails.ProjectDocuments, sqlAuditActionType, auditProjectDetails, ref eventId, ref dbDocuments, dbModule);
                    
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectDocuments);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private void DocumentAudit(IList<ModuleDocument> projectDocuments, SqlAuditActionType sqlAuditActionType, DomainModel.ProjectDetail projectDetail, ref long? eventId, ref List<DbModel.Document> dbDocuments, IList<DbModel.SqlauditModule> dbModule)
        {
            //For Document Audit
            if (projectDocuments.Count > 0)
            {
                object newData;
                object oldData;
                var newDocument = projectDocuments?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
                var modifiedDocument = projectDocuments?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
                var deletedDocument = projectDocuments?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();
                if (newDocument.Count > 0)
                {
                    newData = newDocument;
                    _auditSearchService.AuditLog(projectDetail, ref eventId, projectDetail?.ProjectInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.ProjectDocument, null, newData, dbModule);
                }
                if (modifiedDocument.Count > 0)
                {
                    newData = modifiedDocument?.OrderBy(x => x.Id)?.ToList();
                    oldData = (dbDocuments != null && dbDocuments.Count > 0) ? _mapper.Map<List<ModuleDocument>>(dbDocuments)?.OrderBy(x => x.Id)?.ToList() : null;
                    _auditSearchService.AuditLog(projectDetail, ref eventId, projectDetail?.ProjectInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.ProjectDocument, oldData, newData, dbModule);
                }
                if (deletedDocument.Count > 0)
                {
                    oldData = deletedDocument;
                    _auditSearchService.AuditLog(projectDetail, ref eventId, projectDetail?.ProjectInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.ProjectDocument, oldData, null, dbModule);
                }
            }
        }

        private Response ProcessProjectNote(int projectNumber, IList<DomainModel.ProjectNote> projectNotes, IList<DbModel.Project> dbProjects, IList<DbModel.SqlauditModule> dbModule, bool commitChanges)
        {
            Exception exception = null;
            try
            {
                if (projectNotes != null && projectNotes.Count > 0)
                {
                    var response = this._projectNoteService.DeleteProjectNotes(projectNumber, projectNotes, commitChanges);//, projectId);
                    if (response.Code == MessageType.Success.ToId())
                    {
                        response = this._projectNoteService.SaveProjectNotes(projectNumber, projectNotes, dbProjects, dbModule, commitChanges);//, projectId);
                        if (response.Code == MessageType.Success.ToId())       //D661 issue 8 
                            response = this._projectNoteService.ModifyProjectNotes(projectNumber, projectNotes, commitChanges);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), projectNotes);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private void RollbackTransaction()
        {
            if (_dbContext.Database.CurrentTransaction != null)
                _dbContext.Database.RollbackTransaction();
        }

        private void AppendEvent(DomainModel.ProjectDetail projectDetail,
                         long? eventId)
        {
            ObjectExtension.SetPropertyValue(projectDetail.ProjectInfo, "EventId", eventId);
            ObjectExtension.SetPropertyValue(projectDetail.ProjectInvoiceAttachments, "EventId", eventId);
            ObjectExtension.SetPropertyValue(projectDetail.ProjectInvoiceReferences, "EventId", eventId);
            ObjectExtension.SetPropertyValue(projectDetail.ProjectNotifications, "EventId", eventId);
            ObjectExtension.SetPropertyValue(projectDetail.ProjectNotes, "EventId", eventId);
            ObjectExtension.SetPropertyValue(projectDetail.ProjectDocuments, "EventId", eventId);
        }

        #endregion

        #region Email

        private Response ProcessEmailNotifications(DomainModel.Project projectInfo,string coordinatorEmail, DomainModel.ProjectDetail projectDetail,ref long? eventId, IList<DbModel.SqlauditModule> dbModule)
        {
            Exception exception = null;
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            IList<KeyValuePair<string, string>> emailContentPlaceholders = null;
            string companyCode = string.Empty;
            string token = DateTime.Now.ToString(VisitTimesheetConstants.TOKEN_DATE_FORMAT);
            try
            {
                if (projectInfo != null)
                {
                    var emailTemplateContent = _projectRepository.MailTemplate()?.FirstOrDefault()?.KeyValue;
                    var fromEmail = new List<EmailAddress>();
                    var toEmails = new List<EmailAddress>();
                    var ccEmails = new List<EmailAddress>();
                    var bodyPlaceHolders = new List<EmailPlaceHolderItem>();
                    var subjectPlaceHolders = new List<EmailPlaceHolderItem>();
                    string subject = ProjectConstants.Email_Notification_Project_Creation_Subject;
                    IList<DbModel.User> dbUsers = null;

                    subjectPlaceHolders.AddRange(new List<EmailPlaceHolderItem>()
                    {
                        new EmailPlaceHolderItem()
                        {
                            PlaceHolderName = ProjectConstants.Project_Number.ToString(),
                            PlaceHolderValue = projectInfo?.ProjectNumber?.ToString(),
                        }
                    });

                    if (!string.IsNullOrEmpty(coordinatorEmail))
                    {
                        fromEmail.Add(new EmailAddress() { Address = coordinatorEmail, DisplayName = projectInfo?.ProjectCoordinatorName?.ToString() });
                    }
                    else
                    {
                        var userTypeInfos = this._userService.Get(new UserInfo() { CompanyCode = projectInfo.ContractHoldingCompanyCode, UserName = projectInfo.ProjectCoordinatorName.Trim() }, ref dbUsers);
                        if(dbUsers?.Count >0)
                            fromEmail.Add(new EmailAddress() { Address = dbUsers?.FirstOrDefault()?.Email?.Trim().ToString(), DisplayName = projectInfo?.ProjectCoordinatorName?.ToString() });
                    }
                    
                    toEmails.Add(new EmailAddress() { Address = ProjectConstants.To_Email, DisplayName = ProjectConstants.To_DisplayName });
                    var projectType = _dbContext.Data.FirstOrDefault(x => x.Id == projectInfo.ProjectTypeId);
                    string workFlow = "";
                    switch(projectInfo?.WorkFlowType?.ToString())
                    {
                        case "V":
                            workFlow = "Visit";
                            break;
                        case "M":
                            workFlow = "Timesheet";
                            break;
                        case "N":
                            workFlow = "Timesheet NDT";
                            break;
                        case "S":
                            workFlow = "Visit NDT";
                            break;
                        default:
                            workFlow = "";
                            break;
                    }

                    emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(ProjectConstants.Project_Number, projectInfo?.ProjectNumber?.ToString()),
                                    new KeyValuePair<string, string>(ProjectConstants.Contract_Number, projectInfo?.ContractNumber?.Trim()?.ToString()),
                                    new KeyValuePair<string, string>(ProjectConstants.Company, projectInfo?.ContractHoldingCompanyName?.Trim()?.ToString()),
                                    new KeyValuePair<string, string>(ProjectConstants.Company_Office, projectInfo?.CompanyOffice?.Trim()?.ToString()),
                                    new KeyValuePair<string, string>(ProjectConstants.Contract_Coordinator, projectInfo?.ProjectCoordinatorName?.ToString()),
                                    new KeyValuePair<string, string>(ProjectConstants.Contract_Coordinator_Email,
                                    (!string.IsNullOrEmpty(coordinatorEmail) ? coordinatorEmail: (dbUsers?.Count >0 ? dbUsers?.FirstOrDefault()?.Email?.Trim().ToString() : string.Empty))),
                                    new KeyValuePair<string, string>(ProjectConstants.Project_Start_Date,  projectInfo?.ProjectStartDate?.ToString("dd-MMM-yyyy")),
                                    new KeyValuePair<string, string>(ProjectConstants.Business_Unit, projectType.Name.ToString()),
                                    new KeyValuePair<string, string>(ProjectConstants.Workflow_Type,workFlow)
                                    }; 

                    if (emailContentPlaceholders?.Count > 0 && !string.IsNullOrEmpty(emailTemplateContent))
                    {
                        emailContentPlaceholders.ToList().ForEach(x =>
                        {
                            emailTemplateContent = emailTemplateContent.Replace(x.Key, x.Value);
                        });
                    }

                    emailMessage.CreatedOn = DateTime.UtcNow;
                    emailMessage.EmailType = EmailType.NPC.ToString();
                    emailMessage.ModuleCode = ModuleCodeType.PRJ.ToString();
                    emailMessage.ModuleEmailRefCode = projectInfo?.ProjectNumber?.ToString();
                    //emailMessage.Subject = ParseSubject(subject, subjectPlaceHolders);
                    emailMessage.Subject = subject;
                    emailMessage.Content = emailTemplateContent.ToString().Replace("<p>", "").Replace("</p>", "<br/>").Replace("\r\n", "<br/>") + "<br/>Token = " + token;
                    emailMessage.FromAddresses = fromEmail;
                    emailMessage.ToAddresses = toEmails;
                    emailMessage.CcAddresses = ccEmails;
                    emailMessage.Token = token;
                    //emailMessage.BodyPlaceHolderAndValue = PopulateBodyPlaceHolder(bodyPlaceHolders);

                    this._emailService.Add(new List<EmailQueueMessage> { emailMessage });

                    /*Store as a document*/
                    DocumentUniqueNameDetail documentUniquename = new DocumentUniqueNameDetail();
                    EmailDocumentUpload emailDocumentUpload = new EmailDocumentUpload();
                    documentUniquename.DocumentName = VisitTimesheetConstants.PROJECT_EMAIL_LOG;
                    StringBuilder documentMessage = new StringBuilder();
                    if (emailMessage.FromAddresses != null && emailMessage.FromAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_FROM);
                        documentMessage.AppendLine(emailMessage.FromAddresses[0].DisplayName + " <span><</span>" + emailMessage.FromAddresses[0].Address + "<span>></span>");
                    }
                    if (emailMessage.ToAddresses != null && emailMessage.ToAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_TO);
                        documentMessage.AppendLine(emailMessage.ToAddresses[0].DisplayName + " <span><</span>" + emailMessage.ToAddresses[0].Address + "<span>></span>");
                    }
                    documentMessage.Append(VisitTimesheetConstants.EMAIL_SUBJECT);
                    documentMessage.AppendLine(emailMessage.Subject);
                    documentMessage.Append(VisitTimesheetConstants.EMAIL_MESSAGE);
                    documentMessage.AppendLine(emailMessage.Content);

                    documentUniquename.ModuleCode = ModuleCodeType.PRJ.ToString();
                    documentUniquename.RequestedBy = string.Empty;
                    documentUniquename.ModuleCodeReference = projectInfo?.ProjectNumber?.ToString();
                    documentUniquename.DocumentType = VisitTimesheetConstants.VISIT_TIMSHEET_EVOLUTION_EMAIL;
                    documentUniquename.SubModuleCodeReference = "0";

                    emailDocumentUpload.IsDocumentUpload = true;
                    emailDocumentUpload.DocumentUniqueName = documentUniquename;
                    emailDocumentUpload.DocumentMessage = documentMessage.ToString().Replace("<p>", "").Replace("</p>", "<br/>").Replace("\r\n", "<br/>");
                    emailDocumentUpload.IsVisibleToCustomer = false;
                    emailDocumentUpload.IsVisibleToTS = _projectRepository.GetTsVisible();

                    this.PostRequest(emailDocumentUpload, projectDetail, ref eventId, dbModule);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), emailMessage);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response PostRequest(EmailDocumentUpload model, DomainModel.ProjectDetail projectInfo,ref long? eventId, IList<DbModel.SqlauditModule> dbModule)
        {
            Exception exception = null;
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };
                using (var httpClient = new HttpClient(clientHandler))
                {
                    string url = _environment.ApplicationGatewayURL + _emailDocumentEndpoint;
                    var uri = new Uri(url);
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                    var response = httpClient.PostAsync(uri, content);
                    if (!response.Result.IsSuccessStatusCode)
                        _logger.LogError(ResponseType.Exception.ToId(), response?.Result?.ReasonPhrase, model);
                    else
                    {
                        ModuleDocument moduleDocument = new ModuleDocument
                        {
                            DocumentName = model?.DocumentUniqueName?.DocumentName,
                            DocumentType = model?.DocumentUniqueName?.DocumentType,
                            IsVisibleToTS = model?.IsVisibleToTS,
                            IsVisibleToCustomer = model?.IsVisibleToCustomer,
                            Status = model?.DocumentUniqueName?.Status
                        };
                        moduleDocument.Status = model?.DocumentUniqueName?.Status;
                        moduleDocument.ModuleCode = model?.DocumentUniqueName?.ModuleCode;
                        moduleDocument.ModuleRefCode = model?.DocumentUniqueName?.ModuleCodeReference;
                        moduleDocument.CreatedOn = DateTime.UtcNow;
                        moduleDocument.RecordStatus = "N";

                        IList<ModuleDocument> listModuleDocument = new List<ModuleDocument>
                        {
                            moduleDocument
                        };
                        List<DbModel.Document> dbDocuments = new List<DbModel.Document>();

                        DocumentAudit(listModuleDocument, SqlAuditActionType.I, projectInfo, ref eventId, ref dbDocuments, dbModule);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private string ParseSubject(string subject, List<EmailPlaceHolderItem> subjectPlaceHolders)
        {
            string result = subject;
            subjectPlaceHolders?.ForEach(x =>
            {
                result = result.Replace(x.PlaceHolderName, x.PlaceHolderValue);
            });

            return result;
        }

        private string PopulateBodyPlaceHolder(List<EmailPlaceHolderItem> bodyPlaceHolders)
        {
            string result = string.Empty;
            bodyPlaceHolders?.ForEach(x =>
            {
                string formattedText = string.Format("{0}${1}${2}", x.PlaceHolderName, x.PlaceHolderValue, x.PlaceHolderForEmail);
                if (string.IsNullOrEmpty(result))
                    result = formattedText;
                else
                    result = string.Format("{0}||{1}", result, formattedText);
            });

            return result;
        }

        private class EmailPlaceHolderItem
        {
            public string PlaceHolderName { get; set; }

            public string PlaceHolderValue { get; set; }

            public string PlaceHolderForEmail { get; set; }
        }

        #endregion
    }



}
