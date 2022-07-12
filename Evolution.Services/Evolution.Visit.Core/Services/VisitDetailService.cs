using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Enums;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Enums;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Email.Models;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Interfaces.Visits;
using Evolution.Project.Domain.Interfaces.Data;
using Evolution.Project.Domain.Models.Projects;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainContractModel = Evolution.Contract.Domain.Models.Contracts;
using DomainModel = Evolution.Visit.Domain.Models.Visits;
using DomainTsModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using DomainAssignmentModel = Evolution.Assignment.Domain.Models.Assignments;
using System.Text.RegularExpressions;
using System.Web;

namespace Evolution.Visit.Core.Services
{
    public class VisitDetailService : IVisitDetailService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitDetailService> _logger = null;
        private readonly IAssignmentService _assignmentService = null;
        private readonly ITechnicalSpecialistService _techSpecService = null;
        private readonly IVisitService _visitService = null;
        private readonly IVisitRepository _visitRepository = null;
        private readonly IVisitNoteService _visitNoteService = null;
        private readonly IVisitReferenceService _visitReferenceService = null;
        private readonly IVisitSupplierPerformanceService _visitSupplierPerformanceService = null;
        private readonly IVisitTechnicalSpecialistConsumableService _visitTechSpecAccountItemConsumableService = null;
        private readonly IVisitTechnicalSpecialistTimeService _visitTechSpecAccountItemTimeService = null;
        private readonly IVisitTechnicalSpecialistExpenseService _visitTechSpecAccountItemExpenseService = null;
        private readonly IVisitTechnicalSpecialistTravelService _visitTechSpecAccountItemTravelService = null;
        private readonly IVisitTechnicalSpecilaistAccountsService _visitTechSpecService = null;
        private readonly IMasterService _masterService = null;
        private readonly IDocumentService _documentService = null;
        private readonly JObject _messages = null;
        private readonly IVisitTechnicalSpecialistsAccountRepository _visitTechnicalSpecialistAccountRepository = null;
        private readonly IVisitTechnicalSpecialistTimeRespository _visitTechnicalSpecialistTimeRespository = null;
        private readonly IVisitTechnicalSpecialistExpenseRepository _visitTechnicalSpecialistExpenseRepository = null;
        private readonly IVisitTechnicalSpecialistTravelRepository _visitTechnicalSpecialistTravelRepository = null;
        private readonly IVisitTechnicalSpecialistConsumableRepository _visitTechnicalSpecialistConsumableRepository = null;
        private readonly IVisitInterCompanyDiscountsRepository _visitInterCompanyDiscountsRepository = null;
        private readonly ICurrencyExchangeRateService _currencyExchangeRateService = null;
        private readonly IEmailQueueService _emailService = null;
        private readonly IUserService _userService = null;
        private readonly IAuditLogger _auditlogger = null;
        private readonly ITechnicalSpecialistCalendarService _technicalSpecialistCalendarService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly IProjectClientNotificationRepository _projectClientNotificationRepository = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly IContractExchangeRateService _contractExchangeRateService = null;
        private readonly ICompanyService _companyServices = null;
        public readonly string _emailDocumentEndpoint = "documents/UploadDocuments";

        public VisitDetailService(IAppLogger<VisitDetailService> logger,
                                        IVisitService visitService,
                                        IVisitRepository visitRepository,
                                        IVisitNoteService visitNoteService,
                                        IVisitReferenceService visitReferenceService,
                                        IVisitSupplierPerformanceService visitSupplierPerformanceService,
                                        IVisitTechnicalSpecialistConsumableService techSpecAccountItemConsumableService,
                                        IVisitTechnicalSpecialistTimeService techSpecAccountItemTimeService,
                                        IVisitTechnicalSpecialistExpenseService techSpecAccountItemExpenseService,
                                        IVisitTechnicalSpecialistTravelService techSpecAccountItemTravelService,
                                        IVisitTechnicalSpecilaistAccountsService visitTechSpecService,
                                        ITechnicalSpecialistService techSpecService,
                                        IAssignmentService assignmentService,
                                        IMasterService masterService,
                                        IDocumentService documentService,
                                        IMapper mapper, JObject messages,
                                        IVisitTechnicalSpecialistsAccountRepository visitTechnicalSpecialistAccountRepository,
                                        IVisitTechnicalSpecialistTimeRespository visitTechnicalSpecialistTimeRespository,
                                        IVisitTechnicalSpecialistExpenseRepository visitTechnicalSpecialistExpenseRepository,
                                        IVisitTechnicalSpecialistTravelRepository visitTechnicalSpecialistTravelRepository,
                                        IVisitTechnicalSpecialistConsumableRepository visitTechnicalSpecialistConsumableRepository,
                                        IVisitInterCompanyDiscountsRepository visitInterCompanyDiscountsRepository,
                                        ICurrencyExchangeRateService currencyExchangeRateService,
                                        IEmailQueueService emailService,
                                        IUserService userService,
                                        IAuditLogger auditLogger,
                                        ITechnicalSpecialistCalendarService technicalSpecialistCalendarService,
                                        IAuditSearchService auditSearchService,
                                        IProjectClientNotificationRepository projectClientNotificationRepository,
                                        IOptions<AppEnvVariableBaseModel> environment,
                                        IContractExchangeRateService contractExchangeRateService,
                                        ICompanyService companyServices)
        {
            _mapper = mapper;
            _logger = logger;
            _visitService = visitService;
            _visitRepository = visitRepository;
            _visitNoteService = visitNoteService;
            _visitReferenceService = visitReferenceService;
            _visitSupplierPerformanceService = visitSupplierPerformanceService;
            _visitTechSpecService = visitTechSpecService;
            _visitTechSpecAccountItemConsumableService = techSpecAccountItemConsumableService;
            _visitTechSpecAccountItemExpenseService = techSpecAccountItemExpenseService;
            _visitTechSpecAccountItemTimeService = techSpecAccountItemTimeService;
            _visitTechSpecAccountItemTravelService = techSpecAccountItemTravelService;
            _masterService = masterService;
            _assignmentService = assignmentService;
            _techSpecService = techSpecService;
            _documentService = documentService;
            this._messages = messages;
            _visitTechnicalSpecialistAccountRepository = visitTechnicalSpecialistAccountRepository;
            _visitTechnicalSpecialistTimeRespository = visitTechnicalSpecialistTimeRespository;
            _visitTechnicalSpecialistExpenseRepository = visitTechnicalSpecialistExpenseRepository;
            _visitTechnicalSpecialistTravelRepository = visitTechnicalSpecialistTravelRepository;
            _visitTechnicalSpecialistConsumableRepository = visitTechnicalSpecialistConsumableRepository;
            _visitInterCompanyDiscountsRepository = visitInterCompanyDiscountsRepository;
            _currencyExchangeRateService = currencyExchangeRateService;
            _emailService = emailService;
            _userService = userService;
            _technicalSpecialistCalendarService = technicalSpecialistCalendarService;
            _auditlogger = auditLogger;
            _auditSearchService = auditSearchService;
            _projectClientNotificationRepository = projectClientNotificationRepository;
            _environment = environment.Value;
            _contractExchangeRateService = contractExchangeRateService;
            _companyServices = companyServices;
        }

        public Response Get(DomainModel.VisitDetail searchModel)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetTechnicalSpecialistWithGrossMargin(DomainModel.VisitTechnicalSpecialist searchModel)
        {
            DomainModel.VisitTechnicalSpecialistGrossMargin visitTechSpecs = new DomainModel.VisitTechnicalSpecialistGrossMargin();
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (searchModel.VisitId > 0)
                {
                    string[] includes = new string[] { "Visit", "TechnicalSpecialist" };

                    visitTechSpecs.VisitTechnicalSpecialists = this._visitTechnicalSpecialistAccountRepository.Search(searchModel, includes);

                    if (visitTechSpecs.VisitTechnicalSpecialists != null && visitTechSpecs.VisitTechnicalSpecialists.Count > 0)
                    {
                        //variables that holds each technical specialist totalcharge and totalpay values
                        decimal accountTotalCharge = 0, accountTotalPay = 0;
                        //loop through each technical specialist and get the line items
                        foreach (var eachTechSpec in visitTechSpecs.VisitTechnicalSpecialists)
                        {
                            var calculatedData = await Task.Run(() => CalculateTechSpecGrossMargin(searchModel.VisitId,
                                eachTechSpec.VisitTechnicalSpecialistId ?? default(int)));
                            eachTechSpec.GrossMargin = calculatedData.Item3;

                            accountTotalCharge += calculatedData.Item1;
                            accountTotalPay += calculatedData.Item2;
                        }
                        //calculate total Timesheet Account Gross margin
                        if (accountTotalCharge == 0)
                            visitTechSpecs.VisitAccountGrossMargin = 0;
                        else
                            visitTechSpecs.VisitAccountGrossMargin = Math.Round(((accountTotalCharge - accountTotalPay) / accountTotalCharge) * 100, 2);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, visitTechSpecs, exception);
            //return await Task.Run(() => new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count));
        }

        /// <summary>
        /// Mandatory Validations check for Request Model
        /// </summary>
        /// <param name="visit"></param>
        /// <returns></returns>
        private Response MandatoryValidations(DomainModel.VisitDetail visit)
        {
            var validationMessages = new List<ValidationMessage>();
            if (visit?.TechnicalSpecialistCalendarList != null && visit?.TechnicalSpecialistCalendarList.Count > 0)
            {
                foreach (var calendar in visit?.TechnicalSpecialistCalendarList)
                {
                    if (!string.IsNullOrEmpty(calendar?.StartDateTime?.ToString()))
                    {
                        if (Convert.ToDateTime(calendar?.StartDateTime).Date < visit.VisitInfo.VisitStartDate && Convert.ToDateTime(calendar?.StartDateTime).Date > visit.VisitInfo.VisitEndDate)
                            validationMessages.Add(_messages, ModuleType.Visit, MessageType.InvalidCalendarStarttime);
                        if (Convert.ToDateTime(calendar?.EndDateTime).Date < visit.VisitInfo.VisitStartDate && Convert.ToDateTime(calendar?.EndDateTime).Date > visit.VisitInfo.VisitEndDate)
                            validationMessages.Add(_messages, ModuleType.Visit, MessageType.InvalidCalendarStarttime);
                    }

                    if (validationMessages.Count > 0)
                        return new Response().ToPopulate(ResponseType.Validation, null, null, validationMessages, null, null, null);
                }
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages, null, null, null);
        }

        public Response Add(DomainModel.VisitDetail visit, bool IsAPIValidationRequired = false)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = null;
            long visitId = 0;

            Response response = null;
            ResponseType responseType = ResponseType.Success;

            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialistCalendar> dBTsCalendarInfo = null;
            IList<DbModel.Company> dbCompanies = null;
            DbModel.NumberSequence dbNumberSequence = null;
            bool isConsumableLineItemsExists = false, isExpenseLineItemsExists = false, isTimeLineItemsExists = false, isTravelLineItemsExists = false, isVisitReferencesExists = false, isVisitSupplierPerformancesExists = false;
            bool isAwaitingApproval = false;
            EmailDocument emailDocument = new EmailDocument
            {
                IsDocumentUpload = false
            };
            IList<DbModel.SqlauditModule> dbModule = null;
            try
            {
                if (visit != null && visit.VisitInfo != null)
                {
                    response = MandatoryValidations(visit);
                    if (response?.Code != ResponseType.Success.ToId() && response?.ValidationMessages?.Count > 0)
                        return response;

                    DomainModel.DbVisit dbVisitData = null;
                    using (dbVisitData = new DomainModel.DbVisit())
                    {
                        if (visit != null && !string.IsNullOrEmpty(visit?.VisitInfo?.RecordStatus))
                            response = ValidateVisitInfo(visit,
                                                        ref dbVisitData,
                                                        ref validationMessages,
                                                        ValidationType.Add,
                                                        ref dbTechnicalSpecialists,
                                                        ref dBTsCalendarInfo,
                                                        ref dbCompanies,
                                                        response);
                        if (response.Code == ResponseType.Success.ToId() & validationMessages?.Count == 0)
                            GetMasterData(visit, ref dbVisitData, ref isConsumableLineItemsExists, ref isExpenseLineItemsExists, ref isTimeLineItemsExists,
                                        ref isTravelLineItemsExists, ref isVisitReferencesExists, ref isVisitSupplierPerformancesExists,
                                        ref validationMessages, response, ref dbModule);

                        if (response != null && validationMessages?.Count == 0)
                        {
                            //Get Visit Validation Data
                            DomainModel.BaseVisit searchModel = new DomainModel.BaseVisit
                            {
                                VisitAssignmentId = visit.VisitInfo.VisitAssignmentId
                            };
                            DomainModel.VisitValidationData validationData = _visitRepository.GetVisitValidationData(searchModel);
                            // Commented for duplicate Visit Number Generation -11 Mar 2021
                            //int visitNumber = _visitService.ProcessNumberSequence(visit.VisitInfo.VisitAssignmentId, ref dbNumberSequence);
                            //To-Do: Will create helper method get TransactionScope instance based on requirement
                            using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                            {
                                _visitRepository.AutoSave = false;
                                //visit.VisitInfo.VisitNumber = visitNumber;
                                response = ProcessVisitInfo(new List<DomainModel.Visit> { visit.VisitInfo },
                                                            ref dbVisitData,
                                                            dbModule,
                                                            ref eventId,
                                                            true,
                                                            ValidationType.Add);                                

                                if ((response == null || response?.Code == MessageType.Success.ToId()) && dbVisitData.DbVisits != null)
                                {
                                    visitId = (long)dbVisitData.DbVisits?.FirstOrDefault().Id;
                                    if (visitId > 0)
                                    {
                                        AssignVisitId(visitId, visit);
                                        response = ValidateData(visit, ref dbVisitData, isConsumableLineItemsExists, isExpenseLineItemsExists, isTimeLineItemsExists,
                                                    isTravelLineItemsExists, isVisitReferencesExists, isVisitSupplierPerformancesExists, ref validationMessages,
                                                    response);

                                        if (response.Code == ResponseType.Success.ToId())
                                        {
                                            List<ModuleDocument> uploadVisitDocuments = new List<ModuleDocument>();
                                            response = ProcessVisitDetail(visit, ValidationType.Add, dbVisitData, dbModule, ref visitId, ref eventId, ref uploadVisitDocuments);
                                            if (response.Code == ResponseType.Success.ToId())
                                            {
                                                if (visit.VisitInfo.VisitStatus == "C")
                                                {
                                                    isAwaitingApproval = true;
                                                    bool isDocumentUpload = false;
                                                    IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(visit.VisitInfo.VisitProjectNumber);
                                                    if (projectNotification != null && projectNotification.Count > 0)
                                                    {
                                                        isDocumentUpload = projectNotification.Where(x => x.IsSendCustomerDirectReportingNotification == true).Count() > 0;
                                                    }

                                                    if (isDocumentUpload)
                                                    {
                                                        DomainModel.VisitEmailData visitEmailData = new DomainModel.VisitEmailData
                                                        {
                                                            VisitDetail = visit
                                                        };
                                                        visitEmailData.VisitDetail.VisitInfo.VisitId = visitId;
                                                        var emailResponse = ProcessEmailNotifications(visitEmailData, EmailTemplate.EmailCustomerDirectReporting, ref eventId, dbModule);
                                                        if (emailResponse != null && emailResponse.Result != null)
                                                        {
                                                            EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                                                            if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                                                            {
                                                                if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                                                                emailDocumentUpload.DocumentUniqueName.ModuleCodeReference = Convert.ToString(visitId);
                                                                emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                                                                emailDocument.IsDocumentUpload = true;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (uploadVisitDocuments != null && uploadVisitDocuments.Count > 0)
                                                {
                                                    IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(visit.VisitInfo.VisitProjectNumber);
                                                    foreach (ModuleDocument moduleDocument in uploadVisitDocuments)
                                                    {
                                                        if (moduleDocument.DocumentType == VisitTimesheetConstants.REPORT_FLASH)
                                                        {
                                                            bool isDocumentUpload = false;
                                                            if (projectNotification != null && projectNotification.Count > 0)
                                                            {
                                                                isDocumentUpload = projectNotification.Where(x => x.IsSendFlashReportingNotification == true).Count() > 0;
                                                            }

                                                            if (isDocumentUpload)
                                                            {
                                                                DomainModel.VisitEmailData visitEmailData = new DomainModel.VisitEmailData
                                                                {
                                                                    VisitDetail = visit
                                                                };
                                                                var emailResponse = ProcessEmailNotifications(visitEmailData, EmailTemplate.EmailCustomerFlashReporting, ref eventId, dbModule);
                                                                if (emailResponse != null && emailResponse.Result != null)
                                                                {
                                                                    EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                                                                    if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                                                                    {
                                                                        if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                                                                        emailDocumentUpload.DocumentUniqueName.ModuleCodeReference = Convert.ToString(visitId);
                                                                        emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                                                                        emailDocument.IsDocumentUpload = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else if (moduleDocument.DocumentType == VisitTimesheetConstants.RELEASE_NOTE)
                                                        {
                                                            bool isDocumentUpload = false;
                                                            if (projectNotification != null && projectNotification.Count > 0)
                                                            {
                                                                isDocumentUpload = projectNotification.Where(x => x.IsSendInspectionReleaseNotesNotification == true).Count() > 0;
                                                            }
                                                            if (isDocumentUpload)
                                                            {
                                                                DomainModel.VisitEmailData visitEmailData = new DomainModel.VisitEmailData
                                                                {
                                                                    VisitDetail = visit
                                                                };
                                                                var emailResponse = ProcessEmailNotifications(visitEmailData, EmailTemplate.EmailCustomerInspectionReleaseNotes, ref eventId, dbModule);
                                                                if (emailResponse != null && emailResponse.Result != null)
                                                                {
                                                                    EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                                                                    if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                                                                    {
                                                                        if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                                                                        emailDocumentUpload.DocumentUniqueName.ModuleCodeReference = Convert.ToString(visitId);
                                                                        emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                                                                        emailDocument.IsDocumentUpload = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else if (moduleDocument.DocumentType == VisitTimesheetConstants.NON_CONFORMANCE_REPORT)
                                                        {
                                                            bool isDocumentUpload = false;
                                                            if (projectNotification != null && projectNotification.Count > 0)
                                                            {
                                                                isDocumentUpload = projectNotification.Where(x => x.IsSendNCRReportingNotification == true).Count() > 0;
                                                            }
                                                            if (isDocumentUpload)
                                                            {
                                                                DomainModel.VisitEmailData visitEmailData = new DomainModel.VisitEmailData
                                                                {
                                                                    VisitDetail = visit
                                                                };
                                                                var emailResponse = ProcessEmailNotifications(visitEmailData, EmailTemplate.EmailNCRReporting, ref eventId, dbModule);
                                                                if (emailResponse != null && emailResponse.Result != null)
                                                                {
                                                                    EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                                                                    if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                                                                    {
                                                                        if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                                                                        emailDocumentUpload.DocumentUniqueName.ModuleCodeReference = Convert.ToString(visitId);
                                                                        emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                                                                        emailDocument.IsDocumentUpload = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                this.UpdateAssignmentFinalVisit(visit.VisitInfo, validationData, dbModule);
                                                if (visit?.VisitInfo?.IsFinalVisit == true)
                                                {
                                                    visit.VisitInfo.VisitId = visitId;
                                                    response = this.DeleteVisitsFinalApproval(visit);
                                                }

                                                if (response.Code == ResponseType.Success.ToId())
                                                {
                                                    response = ProcessTSCalendarData(dbVisitData.filteredAddTSCalendarInfo, dbVisitData.filteredModifyTSCalendarInfo, dbVisitData.filteredDeleteTSCalendarInfo, visitId, dbCompanies, ref response, visit);
                                                    if (response == null || response.Code == ResponseType.Success.ToId())
                                                    {
                                                        _visitRepository.AutoSave = false;
                                                        _visitRepository.ForceSave();
                                                    }
                                                    else
                                                        return response;
                                                }
                                                else
                                                    return response;

                                                tranScope.Complete();
                                            }
                                            else
                                                return response;
                                        }
                                        else
                                            return response;
                                    }
                                    else
                                        return response;
                                }
                                else
                                    return response;
                            }
                            if (visitId > 0 && response != null && response.Code == ResponseType.Success.ToId())
                            {
                                // Commented for duplicate Visit Number Generation -11 Mar 2021
                                // _visitService.SaveNumberSequence(dbNumberSequence);
                                _visitService.AddVisitHistory(visitId, VisitTimesheetConstants.VISIT_TIMESHEET_CREATED, visit.VisitInfo.ActionByUser);
                                if (isAwaitingApproval)
                                {
                                    string changedBy = string.IsNullOrEmpty(visit.VisitInfo.ActionByUser) ?
                                        (string.IsNullOrEmpty(visit.VisitInfo.ModifiedBy) ? string.Empty : visit.VisitInfo.ModifiedBy) : visit.VisitInfo.ActionByUser;
                                    if(!string.IsNullOrEmpty(changedBy))
                                        _visitService.AddVisitHistory(visitId, VisitTimesheetConstants.VISIT_TIMESHEET_AWAITING_APPROVAL, changedBy);

                                }
                            }
                        }
                        else
                            return response;
                    }
                    emailDocument.VisitId = visitId;
                }
                else if (visit == null || visit?.VisitInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messages, visit, MessageType.InvalidPayLoad, visit }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(responseType.ToId(), ex.ToFullString(), visit);
            }
            finally
            {
                _visitRepository.AutoSave = true;
                //_visitRepository.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            return new Response().ToPopulate(responseType, null, null, validationMessages?.ToList(), emailDocument, exception);
        }

        private Response ProcessTSCalendarData(IList<DomainTsModel.TechnicalSpecialistCalendar> filteredAddTSCalendarInfo,
                                                IList<DomainTsModel.TechnicalSpecialistCalendar> filteredModifyTSCalendarInfo,
                                                IList<DomainTsModel.TechnicalSpecialistCalendar> filteredDeleteTSCalendarInfo,
                                                long visitId,
                                                IList<DbModel.Company> dbCompanies,
                                                ref Response response,
                                                DomainModel.VisitDetail visit)
        {
            if (filteredAddTSCalendarInfo != null && filteredAddTSCalendarInfo?.Count > 0)
            {
                filteredAddTSCalendarInfo.ToList().ForEach(filteredAddTSCalendar =>
                {
                    filteredAddTSCalendar.CalendarRefCode = visitId;
                    filteredAddTSCalendar.CreatedBy = filteredAddTSCalendar.ActionByUser;
                });
                response = _technicalSpecialistCalendarService.Save(filteredAddTSCalendarInfo, true, true, dbCompanies);
            }
            if ((response == null || response?.Code == MessageType.Success.ToId()) && filteredModifyTSCalendarInfo != null && filteredModifyTSCalendarInfo?.Count > 0)
            {
                filteredModifyTSCalendarInfo.ToList().ForEach(filteredModifyTSCalendar =>
                {
                    filteredModifyTSCalendar.CalendarRefCode = visitId;
                    filteredModifyTSCalendar.CreatedBy = filteredModifyTSCalendar.ActionByUser;
                });
                response = _technicalSpecialistCalendarService.Update(filteredModifyTSCalendarInfo, true, true, dbCompanies);
            }
            if ((response == null || response?.Code == MessageType.Success.ToId()) && filteredDeleteTSCalendarInfo != null && filteredDeleteTSCalendarInfo?.Count > 0)
            {
                //Commented the code Hotfix defect 671, Calendar allocation is not deleting on final visit
                //if (visit?.VisitInfo?.IsFinalVisit != true)
                //{
                    TechnicalSpecialistCalendar technicalSpecialistCalendarModel = new TechnicalSpecialistCalendar
                    {
                        CalendarRefCode = visitId,
                        IsActive = true,
                        CalendarType = CalendarType.VISIT.ToString()
                    };
                    var visitCalendarDataResult = _technicalSpecialistCalendarService.Get(technicalSpecialistCalendarModel, false)?.Result?.Populate<IList<TechnicalSpecialistCalendar>>();
                    if (visitCalendarDataResult?.Count != filteredDeleteTSCalendarInfo?.Count)
                    {
                        filteredDeleteTSCalendarInfo.ToList().ForEach(filteredDeleteTSCalendar =>
                        {
                            filteredDeleteTSCalendar.CalendarRefCode = visitId;
                            filteredDeleteTSCalendar.CreatedBy = filteredDeleteTSCalendar.ActionByUser;
                        });
                        response = _technicalSpecialistCalendarService.Delete(filteredDeleteTSCalendarInfo, true, true);
                    }
                    else
                    {
                        var validationMessages = new List<ValidationMessage>
                        {
                            { _messages, ModuleType.Calendar, MessageType.InvalidCalendarRecord }
                        };
                        response = new Response().ToPopulate(ResponseType.Error, null, null, validationMessages, null, null, null);
                    }
                //}
            }
            return response;
        }

        private Response IsValidCalendarData(IList<DomainTsModel.TechnicalSpecialistCalendar> technicalSpecialistCalendarList,
                                            ref IList<DomainTsModel.TechnicalSpecialistCalendar> filteredAddTSCalendarInfo,
                                            ref IList<DomainTsModel.TechnicalSpecialistCalendar> filteredModifyTSCalendarInfo,
                                            ref IList<DomainTsModel.TechnicalSpecialistCalendar> filteredDeleteTSCalendarInfo,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<ValidationMessage> validationMessages,
                                            ref IList<DbModel.TechnicalSpecialistCalendar> dBTsCalendarInfo,
                                            ref IList<DbModel.Company> dbCompanies,
                                            IList<TechnicalSpecialistCalendar> visitCalendarList,
                                            DomainModel.VisitDetail visit = null)
        {
            technicalSpecialistCalendarList = technicalSpecialistCalendarList ?? new List<TechnicalSpecialistCalendar>();
            if (visit != null && visit.VisitTechnicalSpecialists != null && visit.VisitTechnicalSpecialists.Count > 0)
            {
                IList<int> deletedTechSpecIds = visit.VisitTechnicalSpecialists.Where(techSpec => techSpec.RecordStatus == "D")?.Select(s => (int)s.Pin).ToList();
                if (deletedTechSpecIds != null && deletedTechSpecIds.Count > 0)
                {
                    var deletedTechSpecCalendarList = visitCalendarList?.Where(s => deletedTechSpecIds.Contains(s.TechnicalSpecialistId))?.ToList();
                    if (deletedTechSpecCalendarList != null && deletedTechSpecCalendarList.Count > 0)
                    {
                        deletedTechSpecCalendarList = deletedTechSpecCalendarList.Select(s =>
                        {
                            s.RecordStatus = "D";
                            return s;
                        }).ToList();
                        technicalSpecialistCalendarList.AddRange(deletedTechSpecCalendarList);
                        technicalSpecialistCalendarList = technicalSpecialistCalendarList.GroupBy(x => x.Id).Select(x => x.First()).ToList();
                    }
                }
            }
            bool isCalendarInfoValid = _technicalSpecialistCalendarService.CheckRecordValidForProcess(technicalSpecialistCalendarList, ValidationType.Add, ref filteredAddTSCalendarInfo, ref dbTechnicalSpecialists, ref validationMessages, ref dBTsCalendarInfo, ref dbCompanies);
            if (isCalendarInfoValid)
            {
                isCalendarInfoValid = _technicalSpecialistCalendarService.CheckRecordValidForProcess(technicalSpecialistCalendarList, ValidationType.Update, ref filteredModifyTSCalendarInfo, ref dbTechnicalSpecialists, ref validationMessages, ref dBTsCalendarInfo, ref dbCompanies);
                if (isCalendarInfoValid)
                {
                    isCalendarInfoValid = _technicalSpecialistCalendarService.CheckRecordValidForProcess(technicalSpecialistCalendarList, ValidationType.Delete, ref filteredDeleteTSCalendarInfo, ref dbTechnicalSpecialists, ref validationMessages, ref dBTsCalendarInfo, ref dbCompanies);
                    if (isCalendarInfoValid)
                    {
                        return new Response().ToPopulate(ResponseType.Success, null, null, null, technicalSpecialistCalendarList, null);
                    }
                }
            }
            return new Response().ToPopulate(ResponseType.Validation, null, null, validationMessages?.ToList(), technicalSpecialistCalendarList, null);
        }

        private bool IsCalendarDatainOutsideRange(IList<DomainTsModel.TechnicalSpecialistCalendar> technicalSpecialistCalendarList, ref IList<ValidationMessage> validationMessages, IList<TechnicalSpecialistCalendar> calendarData = null, DomainModel.VisitDetail visit = null)
        {
            bool isCalendarDatainOutsideRange = false;
            if (visit != null && visit.VisitInfo != null && visit.VisitInfo.VisitId != null)
            {
                if (technicalSpecialistCalendarList != null && technicalSpecialistCalendarList.Count > 0)
                {
                    var deleteCalendarDataList = technicalSpecialistCalendarList.Where(x => x.RecordStatus == "D").Select(s => s.Id).ToList();
                    calendarData = calendarData.Where(s => !deleteCalendarDataList.Contains(s.Id)).ToList();
                }
                isCalendarDatainOutsideRange = calendarData.Any(x => (Convert.ToDateTime(x.StartDateTime).Date < Convert.ToDateTime(visit?.VisitInfo?.VisitStartDate).Date || Convert.ToDateTime(x.StartDateTime).Date > Convert.ToDateTime(visit?.VisitInfo?.VisitEndDate).Date) || (Convert.ToDateTime(x.EndDateTime).Date < Convert.ToDateTime(visit?.VisitInfo?.VisitStartDate).Date || Convert.ToDateTime(x.EndDateTime).Date > Convert.ToDateTime(visit?.VisitInfo?.VisitEndDate).Date));
            }
            if (isCalendarDatainOutsideRange)
            {
                validationMessages.Add(_messages, ModuleType.Calendar, MessageType.VisitCalendarDataInOutsideRange);
            }

            return isCalendarDatainOutsideRange;
        }

        private void AssignVisitId(long visitId, DomainModel.VisitDetail visit)
        {
            visit?.VisitDocuments?.ToList().ForEach(x => { x.ModuleRefCode = visitId.ToString(); });
            visit?.VisitNotes?.ToList().ForEach(x => { x.VisitId = visitId; });
            visit?.VisitReferences?.ToList().ForEach(x => { x.VisitId = visitId; });
            visit?.VisitSupplierPerformances?.ToList().ForEach(x => { x.VisitId = visitId; });
            visit?.VisitTechnicalSpecialistConsumables?.ToList().ForEach(x => { x.VisitId = visitId; });
            visit?.VisitTechnicalSpecialistExpenses?.ToList().ForEach(x => { x.VisitId = visitId; });
            visit?.VisitTechnicalSpecialistTimes?.ToList().ForEach(x => { x.VisitId = visitId; });
            visit?.VisitTechnicalSpecialistTravels?.ToList().ForEach(x => { x.VisitId = visitId; });
            visit?.VisitTechnicalSpecialists?.ToList().ForEach(x => { x.VisitId = visitId; });
        }

        public Response Modify(DomainModel.VisitDetail visit, bool IsAPIValidationRequired = false)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long visitId = Convert.ToInt64(visit?.VisitInfo?.VisitId);
            Response response = null;
            ResponseType responseType = ResponseType.Success;
            long? eventId = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialistCalendar> dBTsCalendarInfo = null;
            IList<DbModel.Company> dbCompanies = null;
            bool isConsumableLineItemsExists = false, isExpenseLineItemsExists = false, isTimeLineItemsExists = false, isTravelLineItemsExists = false, isVisitReferencesExists = false, isVisitSupplierPerformancesExists = false;
            bool isAwaitingApproval = false;
            string searchRef = string.Empty;
            IList<DbModel.TechnicalSpecialistCalendar> tsCalendarData = null;
            EmailDocument emailDocument = new EmailDocument
            {
                IsDocumentUpload = false
            };
            IList<DbModel.SqlauditModule> dbModule = null;
            try
            {
                if (visit != null && visit.VisitInfo != null)
                {
                    response = MandatoryValidations(visit);
                    if (response.Code != ResponseType.Success.ToId() && response.ValidationMessages?.Count > 0)
                    {
                        return response;
                    }
                    
                    DomainModel.DbVisit dbVisitData = null;
                    
                    using (dbVisitData = new DomainModel.DbVisit())
                    {
                        if (visit != null && !string.IsNullOrEmpty(visit?.VisitInfo?.RecordStatus))
                            response = ValidateVisitInfo(visit,
                                ref dbVisitData,
                                ref validationMessages,
                                ValidationType.Update,
                                ref dbTechnicalSpecialists,
                                ref dBTsCalendarInfo,
                                ref dbCompanies,
                                response);
                        if (response.Code == ResponseType.Success.ToId() & validationMessages?.Count == 0)
                            GetMasterData(visit, ref dbVisitData, ref isConsumableLineItemsExists, ref isExpenseLineItemsExists, ref isTimeLineItemsExists,
                                         ref isTravelLineItemsExists, ref isVisitReferencesExists, ref isVisitSupplierPerformancesExists,
                                         ref validationMessages, response, ref dbModule);
                        if (response.Code == ResponseType.Success.ToId() & validationMessages?.Count == 0)
                        {
                            if (visit.VisitInfo?.VisitStatus == "D")
                            {
                                TechnicalSpecialistCalendar technicalSpecialistCalendar = new TechnicalSpecialistCalendar
                                {
                                    CalendarRefCode = (long)visit.VisitInfo.VisitId,
                                    CalendarType = CalendarType.VISIT.ToString(),
                                    IsActive = true
                                };
                                tsCalendarData = _technicalSpecialistCalendarService.GetCalendar(technicalSpecialistCalendar);
                                Response calendarResponse = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
                                if (tsCalendarData != null && tsCalendarData.Any())
                                    calendarResponse = _technicalSpecialistCalendarService.UpdateCalendar(tsCalendarData);
                            }
                            AssignVisitId(visitId, visit);
                            response = ValidateData(visit, ref dbVisitData, isConsumableLineItemsExists, isExpenseLineItemsExists, isTimeLineItemsExists,
                                                    isTravelLineItemsExists, isVisitReferencesExists, isVisitSupplierPerformancesExists, ref validationMessages,
                                                    response);

                            string visitStatus = dbVisitData.DbVisits?.FirstOrDefault().VisitStatus;
                            //Get Visit Validation Data
                            DomainModel.BaseVisit searchModel = new DomainModel.BaseVisit
                            {
                                VisitAssignmentId = visit.VisitInfo.VisitAssignmentId
                            };
                            DomainModel.VisitValidationData validationData = _visitRepository.GetVisitValidationData(searchModel);
                            //To-Do: Will create helper method get TransactionScope instance based on requirement
                            using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                            {
                                _visitRepository.AutoSave = false;
                                response = ProcessVisitInfo(new List<DomainModel.Visit> { visit.VisitInfo },
                                                            ref dbVisitData,
                                                            dbModule,
                                                            ref eventId,
                                                            true,
                                                            ValidationType.Update);                                

                                if ((response == null || response?.Code == MessageType.Success.ToId()) && dbVisitData.DbVisits != null)
                                {
                                    if (!eventId.HasValue || eventId == 0)
                                    {
                                        LogEventGeneration logEvent = new LogEventGeneration(_auditlogger);
                                        searchRef = this.GetSearchRef(dbVisitData);
                                        if(!string.IsNullOrEmpty(searchRef))
                                        {
                                            string changedBy = string.IsNullOrEmpty(visit.VisitInfo.ModifiedBy) ? (string.IsNullOrEmpty(visit.VisitInfo.ActionByUser) 
                                                                ? string.Empty : visit.VisitInfo.ActionByUser) : visit.VisitInfo.ModifiedBy;
                                            eventId = logEvent.GetEventLogId(eventId,
                                                ValidationType.Update.ToAuditActionType(),
                                                changedBy,
                                                searchRef,
                                                SqlAuditModuleType.Visit.ToString());
                                        }                                        
                                    }

                                    List<ModuleDocument> uploadVisitDocuments = new List<ModuleDocument>();
                                    response = ProcessVisitDetail(visit,
                                                                ValidationType.Update,
                                                                dbVisitData,
                                                                dbModule,
                                                                ref visitId,
                                                                ref eventId,
                                                                ref uploadVisitDocuments);
                                   
                                    if (response.Code == ResponseType.Success.ToId())
                                    {
                                        if (visit.VisitInfo.VisitStatus == "C" && visitStatus != "C")
                                        {
                                            isAwaitingApproval = true;
                                            bool isDocumentUpload = false;
                                            IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(visit.VisitInfo.VisitProjectNumber);
                                            if (projectNotification != null && projectNotification.Count > 0)
                                                isDocumentUpload = projectNotification.Where(x => x.IsSendCustomerDirectReportingNotification == true).Count() > 0;

                                            if (isDocumentUpload)
                                            {
                                                DomainModel.VisitEmailData visitEmailData = new DomainModel.VisitEmailData
                                                {
                                                    VisitDetail = visit
                                                };
                                                var emailResponse = ProcessEmailNotifications(visitEmailData, EmailTemplate.EmailCustomerDirectReporting, ref eventId, dbModule);
                                                if (emailResponse != null && emailResponse.Result != null)
                                                {
                                                    EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                                                    if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                                                    {
                                                        if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                                                        emailDocumentUpload.DocumentUniqueName.ModuleCodeReference = Convert.ToString(visitId);
                                                        emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                                                        emailDocument.IsDocumentUpload = true;
                                                    }
                                                }
                                            }
                                        }
                                        if (uploadVisitDocuments != null && uploadVisitDocuments.Count > 0)
                                        {
                                            IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(visit.VisitInfo.VisitProjectNumber);
                                            foreach (ModuleDocument moduleDocument in uploadVisitDocuments)
                                            {
                                                if (moduleDocument.DocumentType == VisitTimesheetConstants.REPORT_FLASH)
                                                {
                                                    bool isDocumentUpload = false;
                                                    if (projectNotification != null && projectNotification.Count > 0)
                                                    {
                                                        isDocumentUpload = projectNotification.Where(x => x.IsSendFlashReportingNotification == true).Count() > 0;
                                                    }

                                                    if (isDocumentUpload)
                                                    {
                                                        DomainModel.VisitEmailData visitEmailData = new DomainModel.VisitEmailData
                                                        {
                                                            VisitDetail = visit
                                                        };
                                                        var emailResponse = ProcessEmailNotifications(visitEmailData, EmailTemplate.EmailCustomerFlashReporting, ref eventId, dbModule);
                                                        if (emailResponse != null && emailResponse.Result != null)
                                                        {
                                                            EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                                                            if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                                                            {
                                                                if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                                                                emailDocumentUpload.DocumentUniqueName.ModuleCodeReference = Convert.ToString(visitId);
                                                                emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                                                                emailDocument.IsDocumentUpload = true;
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (moduleDocument.DocumentType == VisitTimesheetConstants.RELEASE_NOTE)
                                                {
                                                    bool isDocumentUpload = false;
                                                    if (projectNotification != null && projectNotification.Count > 0)
                                                    {
                                                        isDocumentUpload = projectNotification.Where(x => x.IsSendInspectionReleaseNotesNotification == true).Count() > 0;
                                                    }

                                                    if (isDocumentUpload)
                                                    {
                                                        DomainModel.VisitEmailData visitEmailData = new DomainModel.VisitEmailData
                                                        {
                                                            VisitDetail = visit
                                                        };
                                                        var emailResponse = ProcessEmailNotifications(visitEmailData, EmailTemplate.EmailCustomerInspectionReleaseNotes, ref eventId, dbModule);
                                                        if (emailResponse != null && emailResponse.Result != null)
                                                        {
                                                            EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                                                            if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                                                            {
                                                                if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                                                                emailDocumentUpload.DocumentUniqueName.ModuleCodeReference = Convert.ToString(visitId);
                                                                emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                                                                emailDocument.IsDocumentUpload = true;
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (moduleDocument.DocumentType == VisitTimesheetConstants.NON_CONFORMANCE_REPORT)
                                                {
                                                    bool isDocumentUpload = false;
                                                    if (projectNotification != null && projectNotification.Count > 0)
                                                    {
                                                        isDocumentUpload = projectNotification.Where(x => x.IsSendNCRReportingNotification == true).Count() > 0;
                                                    }

                                                    if (isDocumentUpload)
                                                    {
                                                        DomainModel.VisitEmailData visitEmailData = new DomainModel.VisitEmailData
                                                        {
                                                            VisitDetail = visit
                                                        };
                                                        var emailResponse = ProcessEmailNotifications(visitEmailData, EmailTemplate.EmailNCRReporting, ref eventId, dbModule);
                                                        if (emailResponse != null && emailResponse.Result != null)
                                                        {
                                                            EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                                                            if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                                                            {
                                                                if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                                                                emailDocumentUpload.DocumentUniqueName.ModuleCodeReference = Convert.ToString(visitId);
                                                                emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                                                                emailDocument.IsDocumentUpload = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        this.UpdateAssignmentFinalVisit(visit.VisitInfo, validationData, dbModule);
                                        if (visit?.VisitInfo?.IsFinalVisit == true)
                                        {
                                            response = this.DeleteVisitsFinalApproval(visit);
                                        }

                                        if (response.Code == ResponseType.Success.ToId())
                                        {
                                            response = ProcessTSCalendarData(dbVisitData.filteredAddTSCalendarInfo,
                                                dbVisitData.filteredModifyTSCalendarInfo, dbVisitData.filteredDeleteTSCalendarInfo, visitId,
                                                dbCompanies, ref response, visit);
                                            if (response != null && response.Code == ResponseType.Success.ToId())
                                            {
                                                _visitRepository.AutoSave = false;
                                                _visitRepository.ForceSave();
                                            }
                                            else
                                                return response;
                                        }
                                        else
                                            return response;

                                        tranScope.Complete();
                                        if (isAwaitingApproval)
                                        {
                                            string changedBy = string.IsNullOrEmpty(visit.VisitInfo.ModifiedBy) ?
                                            (string.IsNullOrEmpty(visit.VisitInfo.ActionByUser) ? string.Empty : visit.VisitInfo.ActionByUser) : visit.VisitInfo.ModifiedBy;
                                            if (!string.IsNullOrEmpty(changedBy))
                                                _visitService.AddVisitHistory(visitId, VisitTimesheetConstants.VISIT_TIMESHEET_AWAITING_APPROVAL, changedBy);
                                        }
                                    }
                                    else
                                        return response;

                                }
                                else
                                    return response;
                            }
                        }
                        else
                            return response;
                    }
                    emailDocument.VisitId = visitId;
                    emailDocument.EventId = eventId;
                }
                else if (visit == null || visit?.VisitInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messages, visit, MessageType.InvalidPayLoad, visit }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visit);
            }
            finally
            {
                dBTsCalendarInfo = null;
                dbTechnicalSpecialists = null;
                dBTsCalendarInfo = null;
                dbCompanies = null;
                _visitRepository.AutoSave = true;
                //_visitRepository.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            return new Response().ToPopulate(responseType, null, null, validationMessages?.ToList(), emailDocument, exception);
        }

        private Response UpdateAssignmentStartEndDate(DomainModel.Visit visit, DomainModel.VisitValidationData validationData)
        {
            ResponseType responseType = ResponseType.Success;
            Exception exception = null;
            try
            {
                DateTime? visitStartDate = validationData.Visits?.Where(x => x.VisitId != visit.VisitId)?.OrderBy(x => x.FromDate)?.Select(x => x.FromDate)?.FirstOrDefault();
                DateTime? visitEndDate = validationData.Visits?.Where(x => x.VisitId != visit.VisitId)?.OrderBy(x => x.ToDate)?.Select(x => x.ToDate)?.FirstOrDefault();

                var updateValueProps = new List<KeyValuePair<string, object>> {
                                            new KeyValuePair<string, object>("FirstVisitTimesheetStartDate", (visit.VisitStartDate > visitStartDate ? visitStartDate : visit.VisitStartDate)),
                                            new KeyValuePair<string, object>("FirstVisitTimesheetEndDate", (visit.VisitEndDate > visitEndDate ? visitEndDate : visit.VisitEndDate))
                                           };
                _assignmentService.Modify(visit.VisitAssignmentId, updateValueProps, true, a => a.FirstVisitTimesheetStartDate, b => b.FirstVisitTimesheetEndDate);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visit);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void UpdateAssignmentFinalVisit(DomainModel.Visit visit, DomainModel.VisitValidationData validationData, IList<DbModel.SqlauditModule> dbModule)
        {
            long? eventId = null;
            try
            {
                if (validationData != null && validationData.FinalVisitId == visit.VisitId && validationData.HasFinalVisit == true && visit.IsFinalVisit == false)
                {
                    var updateValueProps = new List<KeyValuePair<string, object>> {
                                            new KeyValuePair<string, object>("AssignmentStatus", "P"),
                                            new KeyValuePair<string, object>("IsAssignmentComplete", false),
                                            new KeyValuePair<string, object>("LastModification", DateTime.UtcNow) // D - 828
                                           };

                    _assignmentService.Modify(visit.VisitAssignmentId, updateValueProps, true, a => a.AssignmentStatus, b => b.IsAssignmentComplete);
                    _auditSearchService.AuditLog(visit, ref eventId, visit?.ActionByUser?.ToString(), "{" + AuditSelectType.Id + ":" + visit?.VisitAssignmentId + "}${" + AuditSelectType.ProjectNumber + ":" + visit?.VisitProjectNumber + "}${" + AuditSelectType.ProjectAssignment + ":" + visit?.VisitProjectNumber + "-" + visit?.VisitAssignmentNumber + "}",
                                                ValidationType.Update.ToAuditActionType(), SqlAuditModuleType.Assignment,
                                                new DomainAssignmentModel.Assignment { AssignmentId = visit.VisitAssignmentId, IsAssignmentCompleted = validationData.HasFinalVisit, AssignmentStatus = "C" },
                                                new DomainAssignmentModel.Assignment { AssignmentId = visit.VisitAssignmentId, IsAssignmentCompleted = visit.IsFinalVisit, AssignmentStatus = "P" }, dbModule);

                    //return _visitRepository.UpdateAssignmentFinalVisit(visit.VisitAssignmentId, "P");
                }
                else if (visit.IsFinalVisit == true)
                {

                    var updateValueProps = new List<KeyValuePair<string, object>> {
                                            new KeyValuePair<string, object>("AssignmentStatus", "C"),
                                            new KeyValuePair<string, object>("IsAssignmentComplete", true),
                                            new KeyValuePair<string, object>("LastModification", DateTime.UtcNow) // D - 828
                                           };
                    _assignmentService.Modify(visit.VisitAssignmentId, updateValueProps, true, a => a.AssignmentStatus, b => b.IsAssignmentComplete);
                    _auditSearchService.AuditLog(visit, ref eventId, visit?.ActionByUser?.ToString(), "{" + AuditSelectType.Id + ":" + visit?.VisitAssignmentId + "}${" + AuditSelectType.ProjectNumber + ":" + visit?.VisitProjectNumber + "}${" + AuditSelectType.ProjectAssignment + ":" + visit?.VisitProjectNumber + "-" + visit?.VisitAssignmentNumber + "}",
                                             ValidationType.Update.ToAuditActionType(), SqlAuditModuleType.Assignment,
                                             new DomainAssignmentModel.Assignment { AssignmentId = visit.VisitAssignmentId, IsAssignmentCompleted = validationData.HasFinalVisit, AssignmentStatus = "P" },
                                             new DomainAssignmentModel.Assignment { AssignmentId = visit.VisitAssignmentId, IsAssignmentCompleted = visit.IsFinalVisit, AssignmentStatus = "C" }, dbModule);

                    // return _visitRepository.UpdateAssignmentFinalVisit(visit.VisitAssignmentId, "C");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visit);
            }
        }

        private Response DeleteVisitsFinalApproval(DomainModel.VisitDetail visitDetails)
        {
            Exception exception = null;
            Response response = null;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                DomainModel.BaseVisit searchModel = new DomainModel.BaseVisit
                {
                    VisitAssignmentId = visitDetails.VisitInfo.VisitAssignmentId
                };
                List<DomainModel.Visit> visits = _visitRepository.GetVisitsByAssignment(searchModel);
                if (visits != null && visits.Count > 0)
                {
                    long? visitId = visitDetails.VisitInfo.VisitId;
                    foreach (DomainModel.Visit visit in visits)
                    {
                        if ((visit.VisitStatus == "Q" || visit.VisitStatus == "T" || visit.VisitStatus == "W" || visit.VisitStatus == "S"
                            || visit.VisitStatus == "U") && (response == null || response.Code == ResponseType.Success.ToId())
                            && visit.VisitId != visitId)
                        {
                            if (VisitLineItemsDeleteValidation(visit.VisitId) == false)
                            {
                                visit.RecordStatus = "D";
                                visit.ActionByUser = Convert.ToString(!string.IsNullOrEmpty(visitDetails?.VisitInfo?.ActionByUser)
                                                            ? visitDetails?.VisitInfo?.ActionByUser : visitDetails?.VisitInfo?.ModifiedBy);
                                visitDetails.VisitInfo = visit;
                                response = this.Delete(visitDetails, false, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitDetails.VisitInfo);
            }
            if (response == null)
                return new Response().ToPopulate(ResponseType.Success, null, null, null, null, null);
            else
                return response;
        }

        private bool VisitLineItemsDeleteValidation(long? visitId)
        {
            bool result = false;
            try
            {
                DomainModel.VisitSpecialistAccountItemTime visitTechSpecTime = new DomainModel.VisitSpecialistAccountItemTime
                {
                    VisitId = visitId
                };
                var visitTechSpecTimeItems = this._visitTechnicalSpecialistTimeRespository.Search(visitTechSpecTime);
                if (visitTechSpecTimeItems != null && visitTechSpecTimeItems.Count > 0)
                {
                    result = visitTechSpecTimeItems.Where(x => ((x.ChargeTotalUnit > 0 && Convert.ToDecimal(x.ChargeRate) > 0) || (x.PayUnit > 0 && x.PayRate > 0))).Count() > 0;
                }
                if (!result)
                {
                    DomainModel.VisitSpecialistAccountItemExpense visitTechSpecExpense = new DomainModel.VisitSpecialistAccountItemExpense
                    {
                        VisitId = visitId
                    };
                    var visitTechSpecExpenseItems = this._visitTechnicalSpecialistExpenseRepository.Search(visitTechSpecExpense);
                    if (visitTechSpecExpenseItems != null && visitTechSpecExpenseItems.Count > 0)
                    {
                        result = visitTechSpecExpenseItems.Where(x => ((x.ChargeUnit > 0 && Convert.ToDecimal(x.ChargeRate) > 0) || (x.PayUnit > 0 && x.PayRate > 0))).Count() > 0;
                    }
                }
                if (!result)
                {
                    DomainModel.VisitSpecialistAccountItemTravel visitTechSpecTravel = new DomainModel.VisitSpecialistAccountItemTravel
                    {
                        VisitId = visitId
                    };
                    var visitTechSpecTravelItems = this._visitTechnicalSpecialistTravelRepository.Search(visitTechSpecTravel);
                    if (visitTechSpecTravelItems != null && visitTechSpecTravelItems.Count > 0)
                    {
                        result = visitTechSpecTravelItems.Where(x => ((x.ChargeTotalUnit > 0 && Convert.ToDecimal(x.ChargeRate) > 0) || (x.PayUnit > 0 && x.PayRate > 0))).Count() > 0;
                    }
                }
                if (!result)
                {
                    DomainModel.VisitSpecialistAccountItemConsumable visitTechSpecConsumable = new DomainModel.VisitSpecialistAccountItemConsumable
                    {
                        VisitId = visitId
                    };
                    var visitTechSpecConsumableItems = this._visitTechnicalSpecialistConsumableRepository.Search(visitTechSpecConsumable);
                    if (visitTechSpecConsumableItems != null && visitTechSpecConsumableItems.Count > 0)
                    {
                        result = visitTechSpecConsumableItems.Where(x => ((x.ChargeTotalUnit > 0 && Convert.ToDecimal(x.ChargeRate) > 0) || (x.PayUnit > 0 && x.PayRate > 0))).Count() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitId);
            }
            return result;
        }

        public Response Delete(DomainModel.VisitDetail visitDetails, bool IsAPIValidationRequired = false, bool isFinalVisit = false)
        {
            Exception exception = null;
            Response response = null;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            IList<DbModel.TechnicalSpecialistCalendar> tsCalendarData = null;
            IList<DbModel.Visit> dbVisit = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            long? eventId = null;
            int count = 0;
            try
            {
                if (visitDetails != null && visitDetails.VisitInfo != null && visitDetails.VisitInfo.RecordStatus.IsRecordStatusDeleted() && visitDetails?.VisitInfo?.VisitId > 0)
                {
                    _visitService.IsValidVisitData(new List<long>() { (long)visitDetails.VisitInfo.VisitId }, ref dbVisit, ref validationMessages);
                    if (validationMessages?.Count == 0)
                    {
                        TechnicalSpecialistCalendar technicalSpecialistCalendar = new TechnicalSpecialistCalendar
                        {
                            CalendarRefCode = (long)visitDetails.VisitInfo.VisitId,
                            CalendarType = CalendarType.VISIT.ToString(),
                            IsActive = true
                        };
                        tsCalendarData = _technicalSpecialistCalendarService.GetCalendar(technicalSpecialistCalendar);
                        if (isFinalVisit)
                        {
                            response = DeleteVisit(tsCalendarData, (long)visitDetails.VisitInfo.VisitId, out count);
                        }
                        else
                        {
                            using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                            {
                                response = DeleteVisit(tsCalendarData, (long)visitDetails.VisitInfo.VisitId, out count);
                                if (count > 0)
                                    tranScope.Complete();
                                else
                                    return response;
                            }
                        }
                        if (count > 0)
                        {
                            dbModule = _auditSearchService.GetAuditModule(new List<string>() { SqlAuditModuleType.Visit.ToString() });
                            _auditSearchService.AuditLog(visitDetails, ref eventId, visitDetails.VisitInfo.ActionByUser.ToString(), "{" + AuditSelectType.Id + ":" + visitDetails?.VisitInfo?.VisitId + "}${" +
                                                          AuditSelectType.ReportNumber + ":" + visitDetails?.VisitInfo?.VisitReportNumber?.Trim() + "}${" +
                                                          AuditSelectType.JobReferenceNumber + ":" + visitDetails?.VisitInfo?.VisitProjectNumber + "-" + visitDetails?.VisitInfo?.VisitAssignmentNumber + "-" + visitDetails?.VisitInfo?.VisitNumber + "}${" +
                                                          AuditSelectType.ProjectAssignment + ":" + visitDetails?.VisitInfo?.VisitProjectNumber + "-" + visitDetails?.VisitInfo?.VisitAssignmentNumber + "}", SqlAuditActionType.D, SqlAuditModuleType.Visit, visitDetails?.VisitInfo, null, dbModule);
                        }
                    }
                    else
                        response = new Response(ResponseType.Validation.ToId(), null, null, validationMessages?.ToList(), null);
                }
                else if (visitDetails == null || visitDetails?.VisitInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messages, visitDetails, MessageType.InvalidPayLoad, visitDetails }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitDetails);
            }

            return response;
        }
        
        private Response DeleteVisit(IList<DbModel.TechnicalSpecialistCalendar> tsCalendarData, long visitId, out int count)
        {
            Response calendarResponse = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
            if (tsCalendarData != null && tsCalendarData.Any())
                calendarResponse = _technicalSpecialistCalendarService.UpdateCalendar(tsCalendarData);

            if (calendarResponse == null || calendarResponse.Code == ResponseType.Success.ToId())
                count = _visitRepository.DeleteVisit(visitId);
            else
            {
                count = 0;
                return calendarResponse;
            }

            return new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
        }

        public Response ApproveVisit(DomainModel.VisitEmailData visitEmailData)
        {
            Exception exception = null;
            Response response = null;
            Response emailResponse = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            ResponseType responseType = ResponseType.Success;

            long? eventId = null;
            try
            {
                ValidateVisitTechSecAvailability(visitEmailData, ref response);
                if (response.Code == ResponseType.Success.ToId())
                {
                    response = Modify(visitEmailData.VisitDetail);
                    if (response.Code == ResponseType.Success.ToId() && response.Result != null && response.ValidationMessages?.Count == 0)
                    {
                        EmailDocument emailDocument = (EmailDocument)response.Result;
                        eventId = emailDocument.EventId;
                        emailDocument.IsDocumentUpload = false;
                        DomainModel.Visit visitInfo = visitEmailData.VisitDetail.VisitInfo;
                        //Process Email only for intercompany Assignment's visit
                        bool isIntercompanyAssignment = visitInfo.VisitContractCompanyCode != visitInfo.VisitOperatingCompanyCode;
                        List<string> ApprovalVisitStatus = new List<string>() { "A", "O" };
                        if (ApprovalVisitStatus.Contains(visitEmailData?.VisitDetail?.VisitInfo?.VisitStatus) && response.Code == "1" && response.Result != null)
                        {
                            EmailTemplate emailtemplate = EmailTemplate.VisitApproveClientRequirement;
                            bool isDocumentUpload = false;
                            IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(visitEmailData?.VisitDetail?.VisitInfo?.VisitProjectNumber);
                            if (projectNotification != null && projectNotification.Count > 0)
                            {
                                isDocumentUpload = projectNotification.Where(x => x.IsSendCustomerReportingNotification == true).Count() > 0;
                            }
                            if (visitEmailData?.VisitDetail?.VisitInfo?.VisitStatus == "O")
                            {
                                isDocumentUpload = false;
                                if (isIntercompanyAssignment)
                                {
                                    isDocumentUpload = true;
                                    emailtemplate = EmailTemplate.VisitApprove;
                                }                                
                            }
                            if (isDocumentUpload && visitEmailData.IsProcessNotification)
                            {
                                emailResponse = ProcessEmailNotifications(visitEmailData, emailtemplate, ref eventId, dbModule);
                                if (emailResponse != null && emailResponse.Result != null)
                                {
                                    long responsevisitId = emailDocument.VisitId;
                                    EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                                    if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                                    {
                                        emailDocument.VisitId = responsevisitId;
                                        if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                                        emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                                        emailDocument.IsDocumentUpload = true;
                                    }
                                }
                            }

                            long visitId = visitEmailData?.VisitDetail?.VisitInfo?.VisitId ?? 0;
                            if (visitEmailData?.VisitDetail?.VisitInfo?.VisitStatus == "A")
                            {
                                _visitService.AddVisitHistory(visitId, VisitTimesheetConstants.VISIT_TIMESHEET_APPROVED_BY_CH, visitEmailData?.VisitDetail?.VisitInfo?.ModifiedBy);
                                if (visitEmailData.IsProcessNotification && projectNotification != null && projectNotification.Count > 0
                                    && projectNotification.Where(x => x.IsSendCustomerReportingNotification == true).Count() > 0)
                                {
                                    _visitService.AddVisitHistory(visitId, VisitTimesheetConstants.VISIT_TIMESHEET_CUSTOMER_REPORTING_EMAIL_SENT, visitEmailData?.VisitDetail?.VisitInfo?.ModifiedBy);
                                }
                            }
                            else if (visitEmailData?.VisitDetail?.VisitInfo?.VisitStatus == "O" && visitEmailData.IsProcessNotification)
                                _visitService.AddVisitHistory(visitId, VisitTimesheetConstants.VISIT_TIMESHEET_APPROVED_BY_OPERATOR, visitEmailData?.VisitDetail?.VisitInfo?.ModifiedBy);
                        }
                        response = new Response().ToPopulate(responseType, null, null, null, emailDocument, exception);

                        if (visitEmailData.isSendClientReportingNotification)
                        {
                            // 2.Process Client Reporting Notification Email
                        }
                    }
                    return response;
                }
                return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitEmailData);
            }
            return response;
        }

        private void ValidateVisitTechSecAvailability(DomainModel.VisitEmailData visitEmailData, ref Response response)
        {
            if (visitEmailData.IsValidationRequired == true)
            {
                List<DomainModel.ResourceInfo> resourceInfo = _visitTechnicalSpecialistAccountRepository.IsEpinAssociated(visitEmailData);
                if (resourceInfo != null)
                    response = new Response(ResponseType.Validation.ToId(), null, null, null, resourceInfo);
                else
                    response = new Response().ToPopulate(ResponseType.Success, null, null, null, null, null);
            }
            else
                response = new Response().ToPopulate(ResponseType.Success, null, null, null, null, null);
        }

        public Response RejectVisit(DomainModel.VisitEmailData VisitEmailData)
        {
            //rejectNotes,RejectDate
            Response response = Modify(VisitEmailData.VisitDetail);
            Response emailResponse = null;
            EmailDocument emailDocument = new EmailDocument();
            ResponseType responseType = ResponseType.Success;
            IList<DbModel.SqlauditModule> dbModule = null;
            long? eventId = null;
            try
            {
                if (response.Code == ResponseType.Success.ToId() && response.Result != null && response.ValidationMessages?.Count == 0)
                {
                    emailDocument = (EmailDocument)response.Result;
                    eventId = emailDocument?.EventId;
                    emailDocument.IsDocumentUpload = false;
                }
                //validate the response and start Reject email processing
                DomainModel.Visit visitInfo = VisitEmailData.VisitDetail.VisitInfo;
                //Process Email only for intercompany Assignment's visit
                bool isIntercompanyAssignment = visitInfo.VisitContractCompanyCode != visitInfo.VisitOperatingCompanyCode;
                //if(isIntercompanyAssignment && response.Code == "1" && response.Result != null)
                {

                    emailResponse = ProcessEmailNotifications(VisitEmailData, EmailTemplate.VisitReject, ref eventId, dbModule);
                    if (emailResponse != null && emailResponse.Result != null)
                    {
                        long responsevisitId = emailDocument.VisitId;
                        EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                        if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                        {
                            emailDocument.VisitId = responsevisitId;
                            if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                            emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                            emailDocument.IsDocumentUpload = true;
                        }
                    }

                    long visitId = VisitEmailData?.VisitDetail?.VisitInfo?.VisitId ?? 0;
                    if (VisitEmailData?.VisitDetail?.VisitInfo?.VisitStatus == "R")
                        _visitService.AddVisitHistory(visitId, VisitTimesheetConstants.VISIT_TIMESHEET_REJECTED_BY_CH, VisitEmailData?.VisitDetail?.VisitInfo?.ModifiedBy);
                    else if (VisitEmailData?.VisitDetail?.VisitInfo?.VisitStatus == "J")
                        _visitService.AddVisitHistory(visitId, VisitTimesheetConstants.VISIT_TIMESHEET_REJECTED_BY_OPERRTOR, VisitEmailData?.VisitDetail?.VisitInfo?.ModifiedBy);
                }
                response = new Response().ToPopulate(responseType, null, null, null, emailDocument, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), VisitEmailData);
            }
            return response;
        }

        private void GetMasterData(DomainModel.VisitDetail visit, ref DomainModel.DbVisit dbVisitDbData,
                                    ref bool isConsumableLineItemsExists, ref bool isExpenseLineItemsExists, ref bool isTimeLineItemsExists,
                                    ref bool isTravelLineItemsExists, ref bool isVisitReferencesExists, ref bool isVisitSupplierPerformancesExists,
                                    ref IList<ValidationMessage> validationMessages, Response response, ref IList<DbModel.SqlauditModule> dbModule)
        {
            GetTechnicalSpecialists(visit, ref dbVisitDbData, ref validationMessages, response);
            isVisitReferencesExists = visit.VisitReferences != null ? visit.VisitReferences.Any(x => !string.IsNullOrEmpty(x.RecordStatus)) : false;
            isVisitSupplierPerformancesExists = visit.VisitSupplierPerformances != null ? visit.VisitSupplierPerformances.Any(x => !string.IsNullOrEmpty(x.RecordStatus)) : false;
            isConsumableLineItemsExists = visit.VisitTechnicalSpecialistConsumables != null ? visit.VisitTechnicalSpecialistConsumables.Any(x => !string.IsNullOrEmpty(x.RecordStatus)) : false;
            isExpenseLineItemsExists = visit.VisitTechnicalSpecialistExpenses != null ? visit.VisitTechnicalSpecialistExpenses.Any(x => !string.IsNullOrEmpty(x.RecordStatus)) : false;
            isTimeLineItemsExists = visit.VisitTechnicalSpecialistTimes != null ? visit.VisitTechnicalSpecialistTimes.Any(x => !string.IsNullOrEmpty(x.RecordStatus)) : false;
            isTravelLineItemsExists = visit.VisitTechnicalSpecialistTravels != null ? visit.VisitTechnicalSpecialistTravels.Any(x => !string.IsNullOrEmpty(x.RecordStatus)) : false;

            if (isConsumableLineItemsExists || isExpenseLineItemsExists || isTimeLineItemsExists ||
            isTravelLineItemsExists || isVisitReferencesExists || isVisitSupplierPerformancesExists)
                MasterData(ref dbVisitDbData);

            dbModule = _auditSearchService.GetAuditModule(new List<string>() {
                                                        SqlAuditModuleType.Assignment.ToString(),
                                                        SqlAuditModuleType.Visit.ToString(),
                                                        SqlAuditModuleType.VisitDocument.ToString(),
                                                        SqlAuditModuleType.VisitNote.ToString(),
                                                        SqlAuditModuleType.VisitReference.ToString(),
                                                        SqlAuditModuleType.VisitSpecialistAccount.ToString(),
                                                        SqlAuditModuleType.VisitSupplierPerformance.ToString(),
                                                        SqlAuditModuleType.VisitTechnicalSpecialistAccountItemConsumable.ToString(),
                                                        SqlAuditModuleType.VisitTechnicalSpecialistAccountItemExpense.ToString(),
                                                        SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTime.ToString(),
                                                        SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTravel.ToString(),
                                                        SqlAuditModuleType.VisitInterCompanyDiscount.ToString()
                                                        });
        }

        private Response ValidateData(DomainModel.VisitDetail visit,
                                      ref DomainModel.DbVisit dbVisitDbData,
                                      bool isConsumableLineItemsExists, bool isExpenseLineItemsExists, bool isTimeLineItemsExists,
                                      bool isTravelLineItemsExists, bool isVisitReferencesExists, bool isVisitSupplierPerformancesExists,
                                      ref IList<ValidationMessage> validationMessages,
                                      Response response)
        {
            if (visit != null)
            {
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();
                if (isVisitReferencesExists && validationMessages?.Count == 0)

                    response = ValidateVisitReferences(visit?.VisitReferences,
                                                          ref dbVisitDbData,
                                                          response);

                if (isVisitSupplierPerformancesExists && validationMessages?.Count == 0)

                    response = ValidateSupplierPerformances(visit?.VisitSupplierPerformances,
                                                          ref dbVisitDbData,
                                                          response);

                if (visit.VisitTechnicalSpecialists != null && visit.VisitTechnicalSpecialists.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                    && validationMessages?.Count == 0 && response.ValidationMessages?.Count == 0)
                    response = ValidateVisitTechSpec(visit.VisitTechnicalSpecialists,
                                                         ref dbVisitDbData,
                                                         response);
                if (isConsumableLineItemsExists && validationMessages?.Count == 0)
                    response = ValidateVisitTechSpecAccItemConsumables(visit.VisitTechnicalSpecialistConsumables,
                                                                         ref dbVisitDbData,
                                                                         response);
                if (isExpenseLineItemsExists && validationMessages?.Count == 0)
                    response = ValidateVisitTechSpecAccItemExpense(visit.VisitTechnicalSpecialistExpenses,
                                                                     ref dbVisitDbData,
                                                                     response);
                if (isTimeLineItemsExists && validationMessages?.Count == 0)
                    response = ValidateVisitTechSpecAccItemTime(visit.VisitTechnicalSpecialistTimes,
                                                                     ref dbVisitDbData,
                                                                     response);
                if (isTravelLineItemsExists && validationMessages?.Count == 0)
                    response = ValidateVisitTechSpecAccItemTravel(visit.VisitTechnicalSpecialistTravels,
                                                                     ref dbVisitDbData,
                                                                     response);

                if (visit.VisitNotes != null && visit.VisitNotes.Any(x => !string.IsNullOrEmpty(x.RecordStatus)) && validationMessages?.Count > 0)
                    response = ValidateVisitNotes(visit.VisitNotes,
                                                      ref dbVisitDbData,
                                                      response);

                if (response?.ValidationMessages != null)
                    validationMessages = response.ValidationMessages;

            }
            return response;
        }

        /*This section is used to load child visit data at the time of Update*/
        private void LoadVisitChildData(DomainModel.VisitDetail visit,
                                            ref DomainModel.DbVisit dbVisitDBData,
                                            ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var visitId = visit.VisitInfo.VisitId > 0 ? (long)visit.VisitInfo.VisitId : 0;
            if (visitId > 0)
                _visitService.IsValidVisit(new List<long>() { visitId },
                                                   ref dbVisitDBData.DbVisits,
                                                   ref validationMessages
                                                   //"Visit",
                                                   //"Visit.Assignment",
                                                   //"Visit.Assignment.Project",
                                                   //"Visit.Assignment.Project.Contract",
                                                   //"VisitTechnicalSpecialist",
                                                   //"VisitTechnicalSpecialistAccountItemTime",
                                                   //"VisitTechnicalSpecialistAccountItemTravel",
                                                   //"VisitTechnicalSpecialistAccountItemExpense",
                                                   //"VisitTechnicalSpecialistAccountItemConsumable",
                                                   //"VisitReference",
                                                   //"VisitSupplierPerformance",
                                                   //"VisitNote"
                                                   );

            if (dbVisitDBData?.DbVisits?.Count > 0 && validationMessages?.Count == 0)
            {
                dbVisitDBData.DbAssignments = dbVisitDBData.DbVisits.ToList().Select(x => x.Assignment).ToList();
                dbVisitDBData.DbProjects = dbVisitDBData.DbVisits.ToList().Select(x => x.Assignment.Project).ToList();
                dbVisitDBData.DbContracts = dbVisitDBData.DbVisits.ToList().Select(x => x.Assignment.Project.Contract).ToList();
                dbVisitDBData.DbVisitReference = dbVisitDBData.DbVisits.ToList().SelectMany(x => x.VisitReference).ToList();
                dbVisitDBData.DbVisitSupplierPerformance = dbVisitDBData.DbVisits.ToList().SelectMany(x => x.VisitSupplierPerformance).ToList();
                //dbVisitDBData.DbVisitTechSpecialists = dbVisitDBData.DbVisits.ToList().SelectMany(x => x.VisitTechnicalSpecialist).ToList();
                //dbVisitDBData.DbVisitTechSpecConsumables = dbVisitDBData.DbVisits.ToList().SelectMany(x => x.VisitTechnicalSpecialistAccountItemConsumable).ToList();
                //dbVisitDBData.DbVisitTechSpecExpenses = dbVisitDBData.DbVisits.ToList().SelectMany(x => x.VisitTechnicalSpecialistAccountItemExpense).ToList();
                //dbVisitDBData.DbVisitTechSpecTimes = dbVisitDBData.DbVisits.ToList().SelectMany(x => x.VisitTechnicalSpecialistAccountItemTime).ToList();
                //dbVisitDBData.DbVisitTechSpecTravels = dbVisitDBData.DbVisits.ToList().SelectMany(x => x.VisitTechnicalSpecialistAccountItemTravel).ToList();
                dbVisitDBData.DbVisitNotes = dbVisitDBData.DbVisits.ToList().SelectMany(x => x.VisitNote).ToList();
            }
        }

        /*This section is used to load child transaction data at the time of Add*/
        private void LoadRelatedData(DomainModel.VisitDetail visit,
                                     ref DomainModel.DbVisit dbVisitDBData,
                                     ref IList<ValidationMessage> validationMessages,
                                     ValidationType validationType)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var assignmentId = visit.VisitInfo.VisitAssignmentId > 0 ? visit.VisitInfo.VisitAssignmentId : 0;
            if (assignmentId > 0 && validationType == ValidationType.Add)
            {
                string[] includes = {       "Project",
                                             "Project.Contract",
                                             "Visit",
                    //"Visit.VisitTechnicalSpecialist",
                    //"Visit.VisitTechnicalSpecialistAccountItemTime",
                    //"Visit.VisitTechnicalSpecialistAccountItemTravel",
                    //"Visit.VisitTechnicalSpecialistAccountItemExpense",
                    //"Visit.VisitTechnicalSpecialistAccountItemConsumable",
                    //"Visit.VisitReference",
                    //"Visit.VisitNote"
                };

                _assignmentService.IsValidAssignment(new List<int>() { assignmentId },
                                             ref dbVisitDBData.DbAssignments,
                                             ref validationMessages,
                                             includes
                                             );

                if (dbVisitDBData?.DbAssignments != null && validationMessages?.Count == 0)
                {
                    dbVisitDBData.DbProjects = dbVisitDBData.DbAssignments.ToList().Select(x => x.Project).ToList();
                    dbVisitDBData.DbContracts = dbVisitDBData.DbAssignments.ToList().Select(x => x.Project.Contract).ToList();
                    //    dbVisitDBData.DbVisitReference = dbVisitDBData.DbAssignments.ToList().SelectMany(x => x.Visit)?.SelectMany(x1 => x1.VisitReference).ToList();
                    //    dbVisitDBData.DbVisitTechSpecialists = dbVisitDBData.DbAssignments.ToList().SelectMany(x => x.Visit)?.SelectMany(x => x.VisitTechnicalSpecialist).ToList();
                    //    dbVisitDBData.DbVisitTechSpecConsumables = dbVisitDBData.DbAssignments.ToList().SelectMany(x => x.Visit)?.SelectMany(x => x.VisitTechnicalSpecialistAccountItemConsumable).ToList();
                    //    dbVisitDBData.DbVisitTechSpecExpenses = dbVisitDBData.DbAssignments.ToList().SelectMany(x => x.Visit)?.SelectMany(x => x.VisitTechnicalSpecialistAccountItemExpense).ToList();
                    //    dbVisitDBData.DbVisitTechSpecTimes = dbVisitDBData.DbAssignments.ToList().SelectMany(x => x.Visit)?.SelectMany(x => x.VisitTechnicalSpecialistAccountItemTime).ToList();
                    //    dbVisitDBData.DbVisitTechSpecTravels = dbVisitDBData.DbAssignments.ToList().SelectMany(x => x.Visit)?.SelectMany(x => x.VisitTechnicalSpecialistAccountItemTravel).ToList();
                    //    dbVisitDBData.DbVisitNotes = dbVisitDBData.DbAssignments.ToList().SelectMany(x => x.Visit)?.SelectMany(x => x.VisitNote).ToList();
                }
            }
        }

        /*This section is used to load master Data for Currency & Expense*/
        private void MasterData(ref DomainModel.DbVisit dbVisitDBData)
        {
            dbVisitDBData.DbData = _masterService.Get(new List<MasterType>() { MasterType.ExpenseType,
                                                                                 MasterType.Currency,
                                                                                 MasterType.AssignmentReferenceType});
        }

        /*This section is used to get the needed tech Spec*/
        private Response GetTechnicalSpecialists(DomainModel.VisitDetail visit,
                                                 ref DomainModel.DbVisit dbVisitDBData,
                                                 ref IList<ValidationMessage> validationMessages,
                                                 Response response)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (visit?.VisitTechnicalSpecialists != null)
            {
                List<string> ePins = visit?.VisitTechnicalSpecialists.ToList().Where(x => x.Pin > 0).Select(x => x.Pin.ToString()).ToList();
                _techSpecService.IsRecordExistInDb(ePins, ref dbVisitDBData.DbTechnicalSpecialists, ref validationMessages);
            }
            return response;
        }

        /*This section is used to validate Visit Reference*/
        private Response ValidateVisitInfo(DomainModel.VisitDetail visit,
                                               ref DomainModel.DbVisit dbVisitDBData,
                                               ref IList<ValidationMessage> validationMessages,
                                               ValidationType validationType,
                                               ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                               ref IList<DbModel.TechnicalSpecialistCalendar> dBTsCalendarInfo,
                                               ref IList<DbModel.Company> dbCompanies,
                                               Response response)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            if (validationType == ValidationType.Add)
                LoadRelatedData(visit,
                                ref dbVisitDBData,
                                ref validationMessages,
                                validationType);
            if (validationType == ValidationType.Update)
                LoadVisitChildData(visit,
                                ref dbVisitDBData,
                                ref validationMessages);

            IList<DomainModel.Visit> visitInfo = new List<DomainModel.Visit>
            {
                visit.VisitInfo
            };
            response = _visitService.IsRecordValidForProcess(visitInfo,
                                                                ValidationType.Add,
                                                                ref dbVisitDBData.DbVisits,
                                                                ref dbVisitDBData.DbAssignments);

            if (validationType == ValidationType.Update && response.Code == ResponseType.Success.ToId() && (bool)response.Result && validationMessages?.Count == 0)
                response = _visitService.IsRecordValidForProcess(visitInfo,
                                                                     ValidationType.Update,
                                                                     ref dbVisitDBData.DbVisits,
                                                                     ref dbVisitDBData.DbAssignments);

            if (validationType == ValidationType.Delete && response.Code == ResponseType.Success.ToId() && (bool)response.Result && validationMessages?.Count == 0)
                response = _visitService.IsRecordValidForProcess(visitInfo,
                                                                    ValidationType.Delete,
                                                                    ref dbVisitDBData.DbVisits,
                                                                    ref dbVisitDBData.DbAssignments);
            if (validationType == ValidationType.Delete && response.Code == ResponseType.Success.ToId() && (bool)response.Result && validationMessages?.Count == 0)
                response = _visitService.IsRecordValidForProcess(visitInfo,
                                                                                    ValidationType.Delete,
                                                                                    ref dbVisitDBData.DbVisits,
                                                                                    ref dbVisitDBData.DbAssignments);


            if (response.Code == ResponseType.Success.ToId() && validationMessages?.Count == 0)
            {
                IList<TechnicalSpecialistCalendar> visitCalendarList = new List<TechnicalSpecialistCalendar>();
                List<string> visitStatus = new List<string>() { "C", "N", "Q", "T", "U", "W", "D" };
                if(visit.VisitInfo?.VisitContractCompanyCode == visit.VisitInfo?.VisitOperatingCompanyCode) 
                    visitStatus.AddRange(new string[] { "A", "J", "R" });

                if (visit.VisitInfo?.VisitId != null && visitStatus.Contains(visit.VisitInfo?.VisitStatus))
                {
                    TechnicalSpecialistCalendar technicalSpecialistCalendar = new TechnicalSpecialistCalendar
                    {
                        CalendarRefCode = (long)visit.VisitInfo.VisitId,
                        IsActive = true,
                        CalendarType = CalendarType.VISIT.ToString()
                    };
                    visitCalendarList = _technicalSpecialistCalendarService.SearchGet(technicalSpecialistCalendar, false).Result?.Populate<IList<TechnicalSpecialistCalendar>>();
                    if (IsCalendarDatainOutsideRange(visit.TechnicalSpecialistCalendarList, ref validationMessages, visitCalendarList, visit))
                    {
                        return new Response().ToPopulate(ResponseType.Validation, null, null, validationMessages?.ToList(), visit.TechnicalSpecialistCalendarList, null);
                    }

                    if (visit.TechnicalSpecialistCalendarList == null || visit.TechnicalSpecialistCalendarList.Count == 0)
                    {
                        if ((visitCalendarList == null || visitCalendarList.Count == 0) && visit.VisitInfo?.VisitStatus != "D")
                        {
                            var calendarValidationMessages = new List<ValidationMessage>
                            {
                                { _messages, ModuleType.Calendar, MessageType.InvalidCalendarRecord }
                            };
                            response = new Response().ToPopulate(ResponseType.Error, null, null, calendarValidationMessages, null, null, null);
                        }
                    }
                }
                if ((visit.TechnicalSpecialistCalendarList != null || (visit.VisitTechnicalSpecialists != null && visit.VisitTechnicalSpecialists.Count > 0)) && response.Code == ResponseType.Success.ToId() && (bool)response.Result && validationMessages?.Count == 0)
                {
                    response = IsValidCalendarData(visit.TechnicalSpecialistCalendarList, ref dbVisitDBData.filteredAddTSCalendarInfo, ref dbVisitDBData.filteredModifyTSCalendarInfo, ref dbVisitDBData.filteredDeleteTSCalendarInfo, ref dbTechnicalSpecialists, ref validationMessages, ref dBTsCalendarInfo, ref dbCompanies, visitCalendarList, visit);
                }
            }

            return response;

        }

        /*This section is used to validate Visit Reference*/
        private Response ValidateVisitReferences(IList<DomainModel.VisitReference> visitReferences,
                                                     ref DomainModel.DbVisit dbVisitDBData,
                                                     Response response)
        {
            IList<DbModel.Data> dbReference = dbVisitDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.AssignmentReferenceType)?.ToList();
            response = _visitReferenceService.IsRecordValidForProcess(visitReferences,
                                                                              ValidationType.Add,
                                                                              ref dbVisitDBData.DbVisitReference,
                                                                              ref dbVisitDBData.DbVisits,
                                                                              ref dbReference
                                                                              );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitReferenceService.IsRecordValidForProcess(visitReferences,
                                                                              ValidationType.Update,
                                                                              ref dbVisitDBData.DbVisitReference,
                                                                              ref dbVisitDBData.DbVisits,
                                                                              ref dbReference
                                                                              );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitReferenceService.IsRecordValidForProcess(visitReferences,
                                                                              ValidationType.Delete,
                                                                              ref dbVisitDBData.DbVisitReference,
                                                                              ref dbVisitDBData.DbVisits,
                                                                              ref dbReference
                                                                              );
            return response;

        }

        /*This section is used to validate Supplier Performance*/
        private Response ValidateSupplierPerformances(IList<DomainModel.VisitSupplierPerformanceType> supplierPerformances,
                                                     ref DomainModel.DbVisit dbVisitDBData,
                                                     Response response)
        {
            response = _visitSupplierPerformanceService.IsRecordValidForProcess(supplierPerformances,
                                                                              ValidationType.Add,
                                                                              ref dbVisitDBData.DbVisitSupplierPerformance,
                                                                              ref dbVisitDBData.DbVisits
                                                                              );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitSupplierPerformanceService.IsRecordValidForProcess(supplierPerformances,
                                                                              ValidationType.Update,
                                                                              ref dbVisitDBData.DbVisitSupplierPerformance,
                                                                              ref dbVisitDBData.DbVisits
                                                                              );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitSupplierPerformanceService.IsRecordValidForProcess(supplierPerformances,
                                                                              ValidationType.Delete,
                                                                              ref dbVisitDBData.DbVisitSupplierPerformance,
                                                                              ref dbVisitDBData.DbVisits
                                                                              );
            return response;

        }

        /*This section is used to validate Visit Tech Spec*/
        private Response ValidateVisitTechSpec(IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                                   ref DomainModel.DbVisit dbVisitDBData,
                                                   Response response)
        {
            response = _visitTechSpecService.IsRecordValidForProcess(visitTechnicalSpecialists,
                                                                         ValidationType.Add,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbVisitTechSpecialists,
                                                                         ref dbVisitDBData.DbTechnicalSpecialists
                                                                         );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitTechSpecService.IsRecordValidForProcess(visitTechnicalSpecialists,
                                                                              ValidationType.Update,
                                                                              ref dbVisitDBData.DbVisits,
                                                                              ref dbVisitDBData.DbVisitTechSpecialists,
                                                                              ref dbVisitDBData.DbTechnicalSpecialists
                                                                             );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitTechSpecService.IsRecordValidForProcess(visitTechnicalSpecialists,
                                                                              ValidationType.Delete,
                                                                              ref dbVisitDBData.DbVisits,
                                                                              ref dbVisitDBData.DbVisitTechSpecialists,
                                                                              ref dbVisitDBData.DbTechnicalSpecialists
                                                                             );
            return response;

        }

        /*This section is used to validate Visit Tech Spec Account Item Time*/
        private Response ValidateVisitTechSpecAccItemTime(IList<DomainModel.VisitSpecialistAccountItemTime> visitTechSpecAccountItemTimes,
                                                              ref DomainModel.DbVisit dbVisitDBData,
                                                              Response response)
        {
            IList<DbModel.Data> dbExpenses = dbVisitDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType || x.MasterDataTypeId == (int)MasterType.Currency)?.ToList();
            response = _visitTechSpecAccountItemTimeService.IsRecordValidForProcess(visitTechSpecAccountItemTimes,
                                                                         ValidationType.Add,
                                                                         ref dbVisitDBData.DbVisitTechSpecTimes,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitTechSpecAccountItemTimeService.IsRecordValidForProcess(visitTechSpecAccountItemTimes,
                                                                              ValidationType.Update,
                                                                              ref dbVisitDBData.DbVisitTechSpecTimes,
                                                                              ref dbVisitDBData.DbVisits,
                                                                              ref dbVisitDBData.DbAssignments,
                                                                              ref dbVisitDBData.DbProjects,
                                                                              ref dbVisitDBData.DbContracts,
                                                                              ref dbExpenses
                                                                             );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitTechSpecAccountItemTimeService.IsRecordValidForProcess(visitTechSpecAccountItemTimes,
                                                                                            ValidationType.Delete,
                                                                                            ref dbVisitDBData.DbVisitTechSpecTimes,
                                                                                            ref dbVisitDBData.DbVisits,
                                                                                            ref dbVisitDBData.DbAssignments,
                                                                                            ref dbVisitDBData.DbProjects,
                                                                                            ref dbVisitDBData.DbContracts,
                                                                                            ref dbExpenses
                                                                                            );
            return response;

        }
        /*This section is used to validate Visit Tech Spec Account Item Travel*/
        private Response ValidateVisitTechSpecAccItemTravel(IList<DomainModel.VisitSpecialistAccountItemTravel> visitTechSpecAccountItemTravels,
                                                                ref DomainModel.DbVisit dbVisitDBData,
                                                                Response response)
        {
            IList<DbModel.Data> dbExpenses = dbVisitDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType || x.MasterDataTypeId == (int)MasterType.Currency)?.ToList();
            response = _visitTechSpecAccountItemTravelService.IsRecordValidForProcess(visitTechSpecAccountItemTravels,
                                                                         ValidationType.Add,
                                                                         ref dbVisitDBData.DbVisitTechSpecTravels,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitTechSpecAccountItemTravelService.IsRecordValidForProcess(visitTechSpecAccountItemTravels,
                                                                         ValidationType.Update,
                                                                         ref dbVisitDBData.DbVisitTechSpecTravels,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitTechSpecAccountItemTravelService.IsRecordValidForProcess(visitTechSpecAccountItemTravels,
                                                                         ValidationType.Delete,
                                                                         ref dbVisitDBData.DbVisitTechSpecTravels,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            return response;

        }

        /*This section is used to validate Visit Tech Spec Account Item Expense*/
        private Response ValidateVisitTechSpecAccItemExpense(IList<DomainModel.VisitSpecialistAccountItemExpense> visitTechSpecAccountItemExpenses,
                                                                ref DomainModel.DbVisit dbVisitDBData,
                                                                Response response)
        {
            IList<DbModel.Data> dbExpenses = dbVisitDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType || x.MasterDataTypeId == (int)MasterType.Currency)?.ToList();
            response = _visitTechSpecAccountItemExpenseService.IsRecordValidForProcess(visitTechSpecAccountItemExpenses,
                                                                         ValidationType.Add,
                                                                         ref dbVisitDBData.DbVisitTechSpecExpenses,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitTechSpecAccountItemExpenseService.IsRecordValidForProcess(visitTechSpecAccountItemExpenses,
                                                                         ValidationType.Update,
                                                                         ref dbVisitDBData.DbVisitTechSpecExpenses,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitTechSpecAccountItemExpenseService.IsRecordValidForProcess(visitTechSpecAccountItemExpenses,
                                                                         ValidationType.Delete,
                                                                         ref dbVisitDBData.DbVisitTechSpecExpenses,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            return response;

        }

        /*This section is used to validate Visit Tech Spec Account Item Consumables & Equipment*/
        private Response ValidateVisitTechSpecAccItemConsumables(IList<DomainModel.VisitSpecialistAccountItemConsumable> visitTechSpecAccountItemConsumables,
                                                                ref DomainModel.DbVisit dbVisitDBData,
                                                                Response response)
        {
            IList<DbModel.Data> dbExpenses = dbVisitDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType || x.MasterDataTypeId == (int)MasterType.Currency)?.ToList();
            response = _visitTechSpecAccountItemConsumableService.IsRecordValidForProcess(visitTechSpecAccountItemConsumables,
                                                                         ValidationType.Add,
                                                                         ref dbVisitDBData.DbVisitTechSpecConsumables,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitTechSpecAccountItemConsumableService.IsRecordValidForProcess(visitTechSpecAccountItemConsumables,
                                                                         ValidationType.Update,
                                                                         ref dbVisitDBData.DbVisitTechSpecConsumables,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitTechSpecAccountItemConsumableService.IsRecordValidForProcess(visitTechSpecAccountItemConsumables,
                                                                         ValidationType.Delete,
                                                                         ref dbVisitDBData.DbVisitTechSpecConsumables,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            return response;

        }


        /*This section is used to validate Visit Note*/
        private Response ValidateVisitNotes(IList<DomainModel.VisitNote> visitNote,
                                                 ref DomainModel.DbVisit dbVisitDBData,
                                                 Response response)
        {
            response = _visitNoteService.IsRecordValidForProcess(visitNote, ValidationType.Add,
                                                                      ref dbVisitDBData.DbVisitNotes,
                                                                      ref dbVisitDBData.DbVisits);

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _visitNoteService.IsRecordValidForProcess(visitNote, ValidationType.Delete,
                                                                        ref dbVisitDBData.DbVisitNotes,
                                                                        ref dbVisitDBData.DbVisits);

            return response;
        }

        /*This section is called to process Visit Info*/
        private Response ProcessVisit(DomainModel.VisitDetail visit,
                                           ValidationType validationType,
                                           DomainModel.DbVisit dbVisitData,
                                           IList<DbModel.SqlauditModule> dbModule,
                                           ref long visitId)
        {
            bool commitChanges = true;
            Response response = null;
            Exception exception = null;
            long? eventId = null;
            try
            {
                if (visit != null)
                {
                    _visitRepository.AutoSave = false;
                    visitId = visit?.VisitInfo?.VisitId == null ? 0 : (int)visit?.VisitInfo?.VisitId;

                    response = this.ProcessVisitInfo(new List<DomainModel.Visit> { visit.VisitInfo },
                                                         ref dbVisitData,
                                                         dbModule,
                                                         ref eventId,
                                                         commitChanges,
                                                         validationType);
                    if (response.Code == MessageType.Success.ToId())
                        visitId = dbVisitData.DbVisits.ToList().FirstOrDefault().Id;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visit);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, visit, exception);
        }

        public Response ApprovalCustomerReportNotification(DomainModel.CustomerReportingNotification clientReportingNotification)
        {
            Exception exception = null;
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            IList<KeyValuePair<string, string>> emailContentPlaceholders = null;
            string visitDate = string.Empty;
            string visitNumber = string.Empty;
            DomainModel.Visit visitInfo = null;
            try
            {
                visitInfo = clientReportingNotification.VisitInfo;
                if (visitInfo.VisitStatus == "A")
                {
                    if (clientReportingNotification.ToAddress == null && clientReportingNotification.ToAddress.Count == 0)
                    {
                        //Add respective validation message
                    }
                    if (string.IsNullOrEmpty(clientReportingNotification.EmailSubject))
                    {
                        //Add respective validation message
                    }
                    if (string.IsNullOrEmpty(clientReportingNotification.EmailContent))
                    {
                        //Add respective validation message
                    }
                    visitDate = visitInfo.VisitStartDate.ToString("dd-MM-yyyy");
                    string reportNumber = string.IsNullOrEmpty(visitInfo.ReportNumber) ? "" : visitInfo.ReportNumber;
                    string assignmentNumber = visitInfo.VisitAssignmentNumber.ToString("00000");
                    string formattedCustomerName = !String.IsNullOrWhiteSpace(visitInfo.VisitCustomerName) && visitInfo.VisitCustomerName.Length >= 5
                                                            ? visitInfo.VisitCustomerName.Substring(0, 5)
                                                            : visitInfo.VisitCustomerName;
                    visitNumber = string.Format("({0} : {1})", assignmentNumber, reportNumber);
                    string projectNumber = string.Format("({0} : {1})", formattedCustomerName, visitInfo.VisitProjectNumber);

                    emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, visitInfo.VisitContractCompany),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, visitInfo.VisitCustomerName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER,assignmentNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, visitInfo.VisitContractCoordinator),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, VisitTimesheetConstants.VISIT),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE,  visitDate)
                                    };
                    string token = DateTime.Now.ToString(VisitTimesheetConstants.TOKEN_DATE_FORMAT);
                    emailMessage = ProcessEmailMessage(ModuleType.Visit,
                                                       EmailTemplate.VisitApproveClientRequirement,
                                                       string.Empty,
                                                       EmailType.Notification, ModuleCodeType.VST,
                                                       visitInfo.VisitId.ToString(),
                                                       clientReportingNotification.EmailSubject,
                                                       emailContentPlaceholders,
                                                       clientReportingNotification.ToAddress,
                                                       new List<EmailAddress>(),
                                                       token,
                                                       clientReportingNotification.Attachments,
                                                       string.Empty,
                                                       null, new List<EmailAddress>());

                    //return _emailService.Add(new List<EmailQueueMessage> { emailMessage });
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessVisitInfo(IList<DomainModel.Visit> visit,
                                               ref DomainModel.DbVisit dbVisitData,
                                               IList<DbModel.SqlauditModule> dbModule,
                                               ref long? eventId,
                                               bool commitChanges,
                                               ValidationType validationType)
        {
            Exception exception = null;
            try
            {
                if (visit != null)
                {
                    if (validationType == ValidationType.Delete)
                        return this._visitService.Delete(visit, dbModule,
                                                             ref eventId,
                                                             commitChanges,
                                                             false);

                    else if (validationType == ValidationType.Add)
                        return this._visitService.Add(visit,
                                                          ref dbVisitData.DbVisits,
                                                          ref dbVisitData.DbAssignments,
                                                          dbModule,
                                                          ref eventId,
                                                          commitChanges,
                                                          false, false);

                    else if (validationType == ValidationType.Update)
                        return this._visitService.Modify(visit,
                                                             ref dbVisitData.DbVisits,
                                                             ref dbVisitData.DbAssignments,
                                                             dbModule,
                                                             ref eventId,
                                                             commitChanges,
                                                             false);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visit);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessVisitReference(long visitId,
                                                 IList<DomainModel.VisitReference> visitReferences,
                                                 ref DomainModel.DbVisit dbVisitDBData,
                                                 IList<DbModel.SqlauditModule> dbModule,
                                                 bool commitChanges,
                                                 ValidationType validationType)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (visitReferences != null)
                {
                    IList<DbModel.Data> dbReference = dbVisitDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.AssignmentReferenceType)?.ToList();
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && dbVisitDBData.DbVisitReference != null)
                        response = this._visitReferenceService.Modify(visitReferences,
                                                                       ref dbVisitDBData.DbVisitReference,
                                                                       ref dbVisitDBData.DbVisits,
                                                                       ref dbReference,
                                                                       dbModule,
                                                                       commitChanges,
                                                                       false,
                                                                       visitId);
                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && dbVisitDBData.DbVisitReference != null)
                    {
                        IList<DbModel.VisitReference> dbVisitReference = dbVisitDBData.DbVisitReference
                                                                                   .Where(x => visitReferences.ToList()
                                                                                   .Any(x1 => x1.VisitReferenceId == x.Id &&
                                                                                              x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this._visitReferenceService.Delete(visitReferences,
                                                                        ref dbVisitReference,
                                                                        ref dbVisitDBData.DbVisits,
                                                                        ref dbReference,
                                                                        dbModule,
                                                                        commitChanges,
                                                                        false,
                                                                        visitId);
                    }
                    if (ValidationType.Delete != validationType)
                        response = this._visitReferenceService.Add(visitReferences,
                                                                    ref dbVisitDBData.DbVisitReference,
                                                                    ref dbVisitDBData.DbVisits,
                                                                    ref dbReference,
                                                                    dbModule,
                                                                    commitChanges,
                                                                    false,
                                                                    visitId);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitReferences);
            }

            return response;
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessSupplierPerformance(long visitId,
                                                 IList<DomainModel.VisitSupplierPerformanceType> supplierPerformances,
                                                 ref DomainModel.DbVisit dbVisitDBData,
                                                 IList<DbModel.SqlauditModule> dbModule,
                                                 bool commitChanges,
                                                 ValidationType validationType)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (supplierPerformances != null)
                {
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && dbVisitDBData.DbVisitSupplierPerformance != null)
                        response = this._visitSupplierPerformanceService.Modify(supplierPerformances,
                                                                       ref dbVisitDBData.DbVisitSupplierPerformance,
                                                                       ref dbVisitDBData.DbVisits,
                                                                       dbModule,
                                                                       commitChanges,
                                                                       false,
                                                                       visitId);
                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && dbVisitDBData.DbVisitSupplierPerformance != null)
                    {
                        IList<DbModel.VisitSupplierPerformance> dbVisitSupplierPerformance = dbVisitDBData.DbVisitSupplierPerformance
                                                                                   .Where(x => supplierPerformances.ToList()
                                                                                   .Any(x1 => x1.VisitSupplierPerformanceTypeId == x.Id &&
                                                                                              x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this._visitSupplierPerformanceService.Delete(supplierPerformances,
                                                                        ref dbVisitSupplierPerformance,
                                                                        ref dbVisitDBData.DbVisits,
                                                                        dbModule,
                                                                        commitChanges,
                                                                        false,
                                                                        visitId);
                    }
                    if (ValidationType.Delete != validationType)
                        response = this._visitSupplierPerformanceService.Add(supplierPerformances,
                                                                    ref dbVisitDBData.DbVisitSupplierPerformance,
                                                                    ref dbVisitDBData.DbVisits,
                                                                    dbModule,
                                                                    commitChanges,
                                                                    false,
                                                                    visitId);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), supplierPerformances);
            }

            return response;
        }

        private Response ProcessVisitDetail(DomainModel.VisitDetail visit,
                                              ValidationType validationType,
                                              DomainModel.DbVisit dbVisitDbData,
                                              IList<DbModel.SqlauditModule> dbModule,
                                              ref long visitId,
                                              ref long? eventId,
                                              ref List<ModuleDocument> uploadVisitDocuments)
        {
            bool commitChanges = true;
            Response response = null;
            Exception exception = null;
            try
            {
                if (visit != null)
                {
                    AppendEvent(visit, eventId);
                    if (visit.VisitReferences != null && visit.VisitReferences.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessVisitReference(visitId,
                                                         visit.VisitReferences,
                                                         ref dbVisitDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType);
                    }
                    if (visit.VisitSupplierPerformances != null && visit.VisitSupplierPerformances.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessSupplierPerformance(visitId,
                                                         visit.VisitSupplierPerformances,
                                                         ref dbVisitDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType);
                    }
                    DomainModel.DbVisit dbVisitDBData = dbVisitDbData;
                    if (visit.VisitTechnicalSpecialists != null && visit.VisitTechnicalSpecialists.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessVisitTechnicalSpecialist(visitId,
                                                         visit.VisitTechnicalSpecialists,
                                                         ref dbVisitDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType,
                                                         false,
                                                         dbVisitDBData);
                    }
                    if (visit.VisitTechnicalSpecialistConsumables != null && visit.VisitTechnicalSpecialistConsumables.Any(x => !string.IsNullOrEmpty(x.RecordStatus)
                    && !string.IsNullOrEmpty(x.Pin)) && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        if (dbVisitDbData.DbVisitTechSpecialists != null)
                            dbVisitDbData.DbVisitTechSpecialists.ToList().ForEach(x =>
                            {
                                visit.VisitTechnicalSpecialistConsumables.ToList().Where(x1 => Convert.ToInt32(x1.Pin) == x.TechnicalSpecialist.Pin).ToList()
                                         .ForEach(x2 => { x2.VisitTechnicalSpecialistId = x.Id; });
                            });


                        response = ProcessVisitTechSpecAccItemConsumables(visitId,
                                                         visit.VisitTechnicalSpecialistConsumables,
                                                         ref dbVisitDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType);
                    }
                    if (visit.VisitTechnicalSpecialistExpenses != null && visit.VisitTechnicalSpecialistExpenses.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {

                        if (dbVisitDbData.DbVisitTechSpecialists != null)
                            dbVisitDbData.DbVisitTechSpecialists.ToList().ForEach(x =>
                            {
                                visit.VisitTechnicalSpecialistExpenses.ToList().Where(x1 => Convert.ToInt32(x1.Pin) == x.TechnicalSpecialist.Pin).ToList()
                                         .ForEach(x2 => { x2.VisitTechnicalSpecialistId = x.Id; });
                            });

                        visit.VisitTechnicalSpecialistExpenses.ToList()
                        .ForEach(x2 =>
                        {
                            if (x2.ChargeRateExchange == 0 && !string.IsNullOrEmpty(x2.Currency) && !string.IsNullOrEmpty(x2.ChargeRateCurrency))
                            {
                                if (x2.Currency == x2.ChargeRateCurrency)
                                {
                                    x2.ChargeRateExchange = (decimal)1.000000;
                                }
                                else
                                {
                                    List<ExchangeRate> exchangeRates = new List<ExchangeRate>
                                    {
                                        new ExchangeRate{ CurrencyFrom = x2.Currency, CurrencyTo = x2.ChargeRateCurrency, EffectiveDate = x2.ExpenseDate }
                                    };
                                    Response exchangeRateResponse = this.GetExpenseLineItemChargeExchangeRates(exchangeRates, visit.VisitInfo.VisitContractNumber);
                                    if (exchangeRateResponse.Result != null)
                                    {
                                        List<ExchangeRate> exchangeRate = (List<ExchangeRate>)exchangeRateResponse.Result;
                                        if (exchangeRate.Count > 0)
                                        {
                                            x2.ChargeRateExchange = exchangeRate[0] != null ? Convert.ToDecimal(string.Format("{0:N6}", exchangeRate[0].Rate)) : (decimal)0.000000;
                                        }
                                    }
                                }
                            }

                            if (x2.PayRateExchange == 0 && !string.IsNullOrEmpty(x2.Currency) && !string.IsNullOrEmpty(x2.PayRateCurrency))
                            {
                                if (x2.Currency == x2.PayRateCurrency)
                                {
                                    x2.PayRateExchange = (decimal)1.000000;
                                }
                                else
                                {
                                    List<ExchangeRate> exchangeRates = new List<ExchangeRate>
                                    {
                                        new ExchangeRate{ CurrencyFrom = x2.Currency, CurrencyTo = x2.PayRateCurrency, EffectiveDate = x2.ExpenseDate }
                                    };
                                    Response exchangeRateResponse = this.GetExpenseLineItemChargeExchangeRates(exchangeRates, visit.VisitInfo.VisitContractNumber);
                                    if (exchangeRateResponse.Result != null)
                                    {
                                        List<ExchangeRate> exchangeRate = (List<ExchangeRate>)exchangeRateResponse.Result;
                                        if (exchangeRate.Count > 0)
                                        {
                                            x2.PayRateExchange = exchangeRate[0] != null ? Convert.ToDecimal(string.Format("{0:N6}", exchangeRate[0].Rate)) : (decimal)0.000000;
                                        }
                                    }
                                }
                            }
                        });

                        response = ProcessVisitTechSpecAccItemExpense(visitId,
                                                         visit.VisitTechnicalSpecialistExpenses,
                                                         ref dbVisitDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType);
                    }
                    if (visit.VisitTechnicalSpecialistTimes != null && visit.VisitTechnicalSpecialistTimes.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        if (dbVisitDbData.DbVisitTechSpecialists != null)
                            dbVisitDbData.DbVisitTechSpecialists.ToList().ForEach(x =>
                            {
                                visit.VisitTechnicalSpecialistTimes.ToList().Where(x1 => Convert.ToInt32(x1.Pin) == x.TechnicalSpecialist.Pin).ToList()
                                         .ForEach(x2 => { x2.VisitTechnicalSpecialistId = x.Id; });
                            });

                        response = ProcessVisitTechSpecAccItemTime(visitId,
                                                         visit.VisitTechnicalSpecialistTimes,
                                                         ref dbVisitDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType);
                    }
                    if (visit.VisitTechnicalSpecialistTravels != null && visit.VisitTechnicalSpecialistTravels.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        if (dbVisitDbData.DbVisitTechSpecialists != null)
                            dbVisitDbData.DbVisitTechSpecialists.ToList().ForEach(x =>
                            {
                                visit.VisitTechnicalSpecialistTravels.ToList().Where(x1 => Convert.ToInt32(x1.Pin) == x.TechnicalSpecialist.Pin).ToList()
                                         .ForEach(x2 => { x2.VisitTechnicalSpecialistId = x.Id; });
                            });

                        response = ProcessVisitTechSpecAccItemTravel(visitId,
                                                         visit.VisitTechnicalSpecialistTravels,
                                                         ref dbVisitDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType);
                    }
                    if (visit.VisitTechnicalSpecialists != null && visit.VisitTechnicalSpecialists.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessVisitTechnicalSpecialist(visitId,
                                                         visit.VisitTechnicalSpecialists,
                                                         ref dbVisitDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType,
                                                         true,
                                                         dbVisitDBData);
                    }
                    if (visit.VisitNotes != null && visit.VisitNotes.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = this.ProcessVisitNote(visitId,
                                                             visit.VisitNotes,
                                                             ref dbVisitDbData,
                                                             dbModule,
                                                             commitChanges,
                                                             validationType);
                    }
                    if (visit.VisitDocuments != null && visit.VisitDocuments.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = this.ProcessVisitDocument(visit, dbModule,
                                                                 commitChanges,
                                                                 validationType,
                                                                 visit,
                                                                 validationType.ToAuditActionType(),
                                                                 ref eventId,
                                                                 ref uploadVisitDocuments);
                    }
                    if (visit.VisitInterCompanyDiscounts != null && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        visit.VisitInterCompanyDiscounts.VisitId = visitId;
                        string actionByUser = Convert.ToString(!string.IsNullOrEmpty(visit?.VisitInfo?.ActionByUser)
                                                            ? visit?.VisitInfo?.ActionByUser : visit?.VisitInfo?.ModifiedBy);
                        response = this.ProcessVisitInterCompany(visit.VisitInterCompanyDiscounts, validationType, actionByUser, dbModule, visit);
                    }
                    return response ?? new Response().ToPopulate(ResponseType.Success, null, null, null, null, null);

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visit);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, visitId, exception);
        }

        private Response ProcessVisitInterCompany(DomainModel.VisitInterCoDiscountInfo visitInterCoDiscounts,
                                ValidationType validationType,
                                string actionByUser,
                                IList<DbModel.SqlauditModule> dbModule, DomainModel.VisitDetail visit)
        {
            Response response = null;
            try
            {
                long? eventId = 0;
                DomainModel.VisitEmailData visitEmailData = new DomainModel.VisitEmailData
                {
                    VisitDetail = visit
                };
                DomainModel.Visit visitdata = visit.VisitInfo;
                eventId = visitInterCoDiscounts.EventId;
                if (ValidationType.Update == validationType)
                {
                    ProcessEmailAmendmentReason(visitEmailData, EmailTemplate.EmailVisitInterCompanyAmendmentReason, ref eventId, dbModule, visitInterCoDiscounts);
                    response = this.UpdateInterCompanyDiscounts(visitInterCoDiscounts, true, actionByUser, dbModule, visitdata);
                }
                 if (ValidationType.Add == validationType)
                    response = this.AddInterCompanyDiscounts(visitInterCoDiscounts, true, actionByUser);

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitInterCoDiscounts);
            }

            return response;
        }

        private Response AddInterCompanyDiscounts(DomainModel.VisitInterCoDiscountInfo interCompanyDiscounts, bool commitChange, string actionByUser)
        {
            Exception exception = null;
            long? eventId = 0;
            try
            {
                IList<DbModel.VisitInterCompanyDiscount> visitInterCompanyDiscount = PopulateRecordsToAdd(interCompanyDiscounts);
                if (visitInterCompanyDiscount?.Count > 0)
                {
                    _visitInterCompanyDiscountsRepository.AutoSave = false;
                    _visitInterCompanyDiscountsRepository.Add(visitInterCompanyDiscount);
                    if (commitChange)
                    {
                        eventId = interCompanyDiscounts.EventId;
                        _visitInterCompanyDiscountsRepository.ForceSave();
                        visitInterCompanyDiscount?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, actionByUser, null,
                                                                                        ValidationType.Add.ToAuditActionType(),
                                                                                        SqlAuditModuleType.VisitInterCompanyDiscount,
                                                                                        null,
                                                                                        _mapper.Map<DomainModel.VisitInterCompanyDiscounts>(x1),
                                                                                        null));

                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), interCompanyDiscounts);
            }
            finally
            {
                _visitInterCompanyDiscountsRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response UpdateInterCompanyDiscounts(DomainModel.VisitInterCoDiscountInfo intercompanyDiscounts,
                            bool commitChange,
                            string actionByuser,
                            IList<DbModel.SqlauditModule> dbModule, DomainModel.Visit visitdata)

        {
            Exception exception = null;
            try
            {
                var dbRecordToUpdate = _visitInterCompanyDiscountsRepository.GetVisitInterCompanyDiscounts(intercompanyDiscounts.VisitId, ValidationType.Update);
                IList<DomainModel.VisitInterCompanyDiscounts> domExistingVisitInterCompanyDiscounts = new List<DomainModel.VisitInterCompanyDiscounts>();
                dbRecordToUpdate?.ToList()?.ForEach(x =>
                {
                    domExistingVisitInterCompanyDiscounts.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.VisitInterCompanyDiscounts>(x)));
                });
                _visitInterCompanyDiscountsRepository.AutoSave = false;
                IList<DbModel.VisitInterCompanyDiscount> visitInterCompanyDiscount = PopulateRecordsToUpdate(intercompanyDiscounts, visitdata);
                if (visitInterCompanyDiscount?.Count > 0)
                    _visitInterCompanyDiscountsRepository.Update(visitInterCompanyDiscount);
                if (commitChange)
                {
                    long? eventId = intercompanyDiscounts.EventId;
                    _visitInterCompanyDiscountsRepository.ForceSave();
                    visitInterCompanyDiscount?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, actionByuser, null,
                                                                                        ValidationType.Update.ToAuditActionType(),
                                                                                        SqlAuditModuleType.VisitInterCompanyDiscount,
                                                                                        domExistingVisitInterCompanyDiscounts?.FirstOrDefault(x2 => x2.VisitInterCompanyDiscountId == x1.Id),
                                                                                        _mapper.Map<DomainModel.VisitInterCompanyDiscounts>(x1),
                                                                                        dbModule));
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), intercompanyDiscounts);
            }
            finally
            {
                _visitInterCompanyDiscountsRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private IList<DbModel.VisitInterCompanyDiscount> PopulateRecordsToAdd(DomainModel.VisitInterCoDiscountInfo visitInterCoDiscountInfo)
        {
            List<string> companyCodes = new List<string>();
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Company> dbCompanies = null;
            if (!string.IsNullOrEmpty(visitInterCoDiscountInfo.ParentContractHoldingCompanyCode))
            {
                companyCodes.Add(visitInterCoDiscountInfo.ParentContractHoldingCompanyCode);
            }
            if (!string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentHostcompanyCode))
            {
                companyCodes.Add(visitInterCoDiscountInfo.AssignmentHostcompanyCode);
            }
            if (!string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code))
            {
                companyCodes.Add(visitInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code);
            }
            if (!string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code))
            {
                companyCodes.Add(visitInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code);
            }
            if (!string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentContractHoldingCompanyCode))
            {
                companyCodes.Add(visitInterCoDiscountInfo.AssignmentContractHoldingCompanyCode);
            }
            _companyServices.IsValidCompany(companyCodes, ref dbCompanies, ref validationMessages);
            IList<DbModel.VisitInterCompanyDiscount> recordToAdd = new List<DbModel.VisitInterCompanyDiscount>();
            if (!string.IsNullOrEmpty(visitInterCoDiscountInfo.ParentContractHoldingCompanyCode) && visitInterCoDiscountInfo.ParentContractHoldingCompanyDiscount != null && !string.IsNullOrEmpty(visitInterCoDiscountInfo.ParentContractHoldingCompanyDescription))
            {
                var companyId = dbCompanies?.ToList().FirstOrDefault(x => x.Code == visitInterCoDiscountInfo.ParentContractHoldingCompanyCode)?.Id;
                var data = GetDbRecordsToAdd(visitInterCoDiscountInfo.VisitId, AssignmentInterCompanyDiscountType.ParentContract.DisplayName(), (int)companyId, visitInterCoDiscountInfo.ParentContractHoldingCompanyDescription, (decimal)visitInterCoDiscountInfo.ParentContractHoldingCompanyDiscount);
                if (data != null)
                    recordToAdd.Add(data);
            }

            if (!string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentOperatingCompanyCode) && !string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentHostcompanyCode) && visitInterCoDiscountInfo.AssignmentHostcompanyDiscount != null
                && !string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentHostcompanyDescription) && visitInterCoDiscountInfo.AssignmentOperatingCompanyCode != visitInterCoDiscountInfo.AssignmentHostcompanyCode)
            {
                var companyId = dbCompanies?.ToList().FirstOrDefault(x => x.Code == visitInterCoDiscountInfo.AssignmentHostcompanyCode)?.Id;
                var data = GetDbRecordsToAdd(visitInterCoDiscountInfo.VisitId, AssignmentInterCompanyDiscountType.OperatingCountryCompany.DisplayName(), (int)companyId, visitInterCoDiscountInfo.AssignmentHostcompanyDescription, (decimal)visitInterCoDiscountInfo.AssignmentHostcompanyDiscount);
                if (data != null)
                    recordToAdd.Add(data);
            }

            if (visitInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount != null && !string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code)
                && !string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Description))
            {
                var companyId = dbCompanies?.ToList().FirstOrDefault(x => x.Code == visitInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code)?.Id;
                var data = GetDbRecordsToAdd(visitInterCoDiscountInfo.VisitId, AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId.DisplayName(), (int)companyId, visitInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Description, (decimal)visitInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount);
                if (data != null)
                    recordToAdd.Add(data);
            }

            if (visitInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount != null && !string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code) && !string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Description))

            {
                var companyId = dbCompanies?.ToList().FirstOrDefault(x => x.Code == visitInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code)?.Id;
                var data = GetDbRecordsToAdd(visitInterCoDiscountInfo.VisitId, AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2.DisplayName(), (int)companyId, visitInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Description, (decimal)visitInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount);
                if (data != null)
                    recordToAdd.Add(data);
            }

            if (!string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentContractHoldingCompanyCode) && visitInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount != null
                && !string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentContractHoldingCompanyDescription))
            {
                var companyId = dbCompanies?.ToList().FirstOrDefault(x => x.Code == visitInterCoDiscountInfo.AssignmentContractHoldingCompanyCode)?.Id;
                var data = GetDbRecordsToAdd(visitInterCoDiscountInfo.VisitId, AssignmentInterCompanyDiscountType.Contract.DisplayName(), (int)companyId, visitInterCoDiscountInfo.AssignmentContractHoldingCompanyDescription, (decimal)visitInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount);
                if (data != null)
                    recordToAdd.Add(data);
            }
            return recordToAdd;
        }

        private IList<DbModel.VisitInterCompanyDiscount> PopulateRecordsToUpdate(DomainModel.VisitInterCoDiscountInfo visitInterCoDiscountInfo, DomainModel.Visit visitdata)
        {
            IList<DbModel.VisitInterCompanyDiscount> recordToUpdate = new List<DbModel.VisitInterCompanyDiscount>();
            var dbRecordToUpdate = _visitInterCompanyDiscountsRepository.GetVisitInterCompanyDiscounts(visitInterCoDiscountInfo.VisitId, ValidationType.Update);

          IList<DomainModel.VisitInterCompanyDiscounts> domExistingVisitInterCompanyDiscounts = new List<DomainModel.VisitInterCompanyDiscounts>();
            dbRecordToUpdate?.ToList()?.ForEach(x =>
            {
                domExistingVisitInterCompanyDiscounts.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.VisitInterCompanyDiscounts>(x)));
            });
            // Parent Contract
            if (!string.IsNullOrEmpty(visitInterCoDiscountInfo.ParentContractHoldingCompanyCode) && visitInterCoDiscountInfo.ParentContractHoldingCompanyDiscount != null
            && !string.IsNullOrEmpty(visitInterCoDiscountInfo.ParentContractHoldingCompanyDescription))
                recordToUpdate.Add(GetRecordsToUpdate(AssignmentInterCompanyDiscountType.ParentContract.DisplayName(), (decimal)visitInterCoDiscountInfo.ParentContractHoldingCompanyDiscount, visitInterCoDiscountInfo.ParentContractHoldingCompanyDescription, visitInterCoDiscountInfo.ModifiedBy, dbRecordToUpdate, visitdata.AmendmentReason));

            // Additional InterCompany Office 1
            if (!string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code)
                && visitInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount != null
                && !string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Description))
            {
                var data = GetRecordsToUpdate(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId.DisplayName(), (decimal)visitInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount, visitInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Description, visitInterCoDiscountInfo.ModifiedBy, dbRecordToUpdate, visitdata.AmendmentReason);
                if (data != null)
                    recordToUpdate.Add(data);
                
                
            }

            // Additional InterCompany Office 2
            if (!string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code) && visitInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount != null && !string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Description))
            {
                var data = GetRecordsToUpdate(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2.DisplayName(), (decimal)visitInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount, visitInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Description, visitInterCoDiscountInfo.ModifiedBy, dbRecordToUpdate, visitdata.AmendmentReason);
                if (data != null)
                    recordToUpdate.Add(data);
            }

            // Host Company
            if (!string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentHostcompanyCode) && visitInterCoDiscountInfo.AssignmentHostcompanyDiscount != null && !string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentHostcompanyDescription))
            {
                var data = GetRecordsToUpdate(AssignmentInterCompanyDiscountType.OperatingCountryCompany.DisplayName(), (decimal)visitInterCoDiscountInfo.AssignmentHostcompanyDiscount, visitInterCoDiscountInfo.AssignmentHostcompanyDescription, visitInterCoDiscountInfo.ModifiedBy, dbRecordToUpdate, visitdata.AmendmentReason);
                if (data != null)
                    recordToUpdate.Add(data);
            }

            // Contract Company
            if (!string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentContractHoldingCompanyCode) && visitInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount != null && !string.IsNullOrEmpty(visitInterCoDiscountInfo.AssignmentContractHoldingCompanyDescription))
            {
                var data = GetRecordsToUpdate(AssignmentInterCompanyDiscountType.Contract.DisplayName(), (decimal)visitInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount, visitInterCoDiscountInfo.AssignmentContractHoldingCompanyDescription, visitInterCoDiscountInfo.ModifiedBy, dbRecordToUpdate, visitdata.AmendmentReason);
                if (data != null)
                    recordToUpdate.Add(data);
            }

            return recordToUpdate;
        }

        private Response ProcessEmailAmendmentReason(DomainModel.VisitEmailData visitEmailData, EmailTemplate emailTemplateType, ref long? eventId, IList<DbModel.SqlauditModule> dbModule, DomainModel.VisitInterCoDiscountInfo visitInterCoDiscounts)
        {
            string emailSubject = string.Empty;
            string visitDate = string.Empty;
            string visitNumber = string.Empty;
            string coordinatorName = string.Empty;
            string SAMAccountName = string.Empty;
            string loggedInUser = string.Empty;
            string companyCode = string.Empty;
            Exception exception = null;
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            List<EmailAddress> toAddresses = null;
            List<EmailAddress> fromAddresses = null;
            IList<KeyValuePair<string, string>> emailContentPlaceholders = null;
            List<string> CHApprovalStatus = new List<string>() { "A", "R" };
            List<string> OCApprovalStatus = new List<string>() { "O", "C" };
            IList<UserInfo> userInfos = null;
            DomainModel.Visit visitInfo = null;
            EmailType emailType = EmailType.Notification;
            EmailDocumentUpload emailDocumentUpload = new EmailDocumentUpload
            {
                IsDocumentUpload = false
            };
            List<EmailAddress> ccAddresses = null;
            try
            {
                //Get The Existing the Visit inter Company discount method 
                var dbRecordToUpdate = _visitInterCompanyDiscountsRepository.GetVisitInterCompanyDiscounts(visitInterCoDiscounts.VisitId, ValidationType.Update);
                IList<DomainModel.VisitInterCompanyDiscounts> domExistingVisitInterCompanyDiscounts = new List<DomainModel.VisitInterCompanyDiscounts>();
                dbRecordToUpdate?.ToList()?.ForEach(x =>
                {
                    domExistingVisitInterCompanyDiscounts.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.VisitInterCompanyDiscounts>(x)));
                });

                DomainModel.VisitInterCompanyDiscounts existing_visitintercompanyrecords = domExistingVisitInterCompanyDiscounts[0];
                visitInfo = visitEmailData.VisitDetail.VisitInfo;
                SAMAccountName = visitInfo.VisitOperatingCompanyCoordinatorCode;
                loggedInUser = (!string.IsNullOrEmpty(visitEmailData?.VisitDetail?.VisitInfo?.ModifiedBy) ? visitEmailData?.VisitDetail?.VisitInfo?.ModifiedBy : visitEmailData?.VisitDetail?.VisitInfo?.ActionByUser);
                List<string> userTypes = new List<string> { VisitTimesheetConstants.UserType_Coordinator,
                    VisitTimesheetConstants.UserType_MICoordinator, VisitTimesheetConstants.Technical_Specialist };
                List<string> loginNames = new List<string>();
                bool isIntercompanyAssignment = visitInfo.VisitContractCompanyCode != visitInfo.VisitOperatingCompanyCode;
                
                    loginNames = visitEmailData?.VisitDetail?.VisitTechnicalSpecialists?.Select(x => x.LoginName).ToList();               
              
                if (!string.IsNullOrEmpty(visitInfo.VisitContractCoordinatorCode))
                    loginNames.Add(visitInfo.VisitContractCoordinatorCode);
                if (!string.IsNullOrEmpty(visitInfo.VisitOperatingCompanyCoordinatorCode))
                    loginNames.Add(visitInfo.VisitOperatingCompanyCoordinatorCode);

                userInfos = _userService.GetByUserType(loginNames, userTypes, true)
                                        .Result
                                        .Populate<IList<UserInfo>>();

                var emailTemplateContent = _visitRepository.MailTemplateForVisitInterCompanyAmendment()?.FirstOrDefault()?.KeyValue;
                var fromEmails = new List<EmailAddress>();
                var toEmails = new List<EmailAddress>();
                var ccEmails = new List<EmailAddress>();
                var bodyPlaceHolders = new List<EmailPlaceHolderItem>();
                var subjectPlaceHolders = new List<EmailPlaceHolderItem>();
                string subject = string.Empty;
                
                subject = VisitTimesheetConstants.AMENDMENTREASON_SUBJECT;
                fromEmails?.Add(new EmailAddress() { Address = userInfos[0].Email?.Trim(), DisplayName = visitInfo.VisitContractCoordinator.ToString() });
                if (visitInfo.VisitOperatingCompanyCoordinatorCode.Equals(visitInfo.VisitContractCoordinatorCode))
                {
                    toEmails?.Add(new EmailAddress() { Address = userInfos[0].Email?.Trim(), DisplayName = visitInfo.VisitOperatingCompanyCoordinator?.ToString() });
                }
                else
                {
                    toEmails?.Add(new EmailAddress() { Address = userInfos[1].Email?.Trim(), DisplayName = visitInfo.VisitOperatingCompanyCoordinator?.ToString() });
                }
                subjectPlaceHolders.AddRange(new List<EmailPlaceHolderItem>()
                            {
                                new EmailPlaceHolderItem()
                                {
                                    PlaceHolderName = VisitTimesheetConstants.AMENDMENT_REASON.ToString(),
                                    PlaceHolderValue ="InterCompany Discount Amendment",
                                }
                            });
                visitDate = visitInfo.VisitStartDate.ToString("dd/MMM/yyyy");
                    string reportNumber = string.IsNullOrEmpty(visitInfo.VisitReportNumber) ? "" : visitInfo.VisitReportNumber;
                    string assignmentNumber = Convert.ToString(visitInfo.VisitAssignmentNumber);

                    string formattedCustomerName = !String.IsNullOrWhiteSpace(visitInfo.VisitCustomerName) && visitInfo.VisitCustomerName.Length >= 5
                                                        ? visitInfo.VisitCustomerName.Substring(0, 5)
                                                        : visitInfo.VisitCustomerName;
                    visitNumber = string.Format("({0} : {1})", assignmentNumber, reportNumber);
                    string projectNumber = Convert.ToString(visitInfo.VisitProjectNumber);
                    string visitReportNumber = visitInfo.VisitNumber.ToString("000000") + " " + reportNumber;
                      List<Attachment> attachments = new List<Attachment>();
                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                         new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, visitInfo.VisitCustomerName),
                                        new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, visitInfo.VisitAssignmentNumber.ToString("00000")),
                                        new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                        new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_NUMBER, visitReportNumber.ToString()),
                                          new KeyValuePair<string, string>(VisitTimesheetConstants.OLD_PERCENTAGE,existing_visitintercompanyrecords.DiscountPercentage.ToString()),
                                           new KeyValuePair<string, string>(VisitTimesheetConstants.OLD_DESCRIPTION,existing_visitintercompanyrecords.Description.ToString()),
                                            new KeyValuePair<string, string>(VisitTimesheetConstants.AMENDEDRECENTAGE,visitInterCoDiscounts.AssignmentContractHoldingCompanyDiscount.ToString()),
                                             new KeyValuePair<string, string>(VisitTimesheetConstants.AMENDEDDESCRIPTION,visitInterCoDiscounts.AssignmentContractHoldingCompanyDescription.ToString()),
                                             new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME,SAMAccountName),
                                             new KeyValuePair<string, string>(VisitTimesheetConstants.AMENDMENTREASON,visitInfo.AmendmentReason),
                                             new KeyValuePair<string, string>(VisitTimesheetConstants.MAILTOKEN,DateTime.Now.ToString(VisitTimesheetConstants.TOKEN_DATE_FORMAT))
                         };

                if (emailContentPlaceholders?.Count > 0 && !string.IsNullOrEmpty(emailTemplateContent))
                {
                    emailContentPlaceholders.ToList().ForEach(x =>
                    {
                        emailTemplateContent = emailTemplateContent.Replace(x.Key, x.Value);
                    });
                }
                string token = DateTime.Now.ToString(VisitTimesheetConstants.TOKEN_DATE_FORMAT);
                emailMessage.CreatedOn = DateTime.UtcNow;
                emailMessage.EmailType = EmailType.ICR.ToString();
                emailMessage.ModuleCode = ModuleCodeType.VST.ToString();
                emailMessage.ModuleEmailRefCode = Convert.ToString(visitInfo.VisitId);
                emailMessage.Subject = ParseSubject(subject, subjectPlaceHolders);
                emailMessage.Content = emailTemplateContent; //+ "<p>token = " + token + "</p>";//"Message: " + emailTemplateContent;
                emailMessage.FromAddresses = fromEmails;
                emailMessage.ToAddresses = toEmails;
               // string token = DateTime.Now.ToString(VisitTimesheetConstants.TOKEN_DATE_FORMAT);
                _emailService.Add(new List<EmailQueueMessage> { emailMessage });
                emailDocumentUpload = UploadVisitDocuments(emailTemplateType, visitInfo, emailMessage, visitEmailData, ref eventId, dbModule, token);
             }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, emailDocumentUpload, exception);
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

        private class EmailPlaceHolderItem
{
    public string PlaceHolderName { get; set; }

    public string PlaceHolderValue { get; set; }

    public string PlaceHolderForEmail { get; set; }
}


        private static DbModel.VisitInterCompanyDiscount GetRecordsToUpdate(string discountType, decimal discountPercentage, string description, string modifiedBy, IList<DbModel.VisitInterCompanyDiscount> interCompanyDiscounts,string amendmentReason)
        {
            var dbInterCompanyDiscount = interCompanyDiscounts.FirstOrDefault(x => x.DiscountType == discountType);
            if (dbInterCompanyDiscount != null)
            {
                dbInterCompanyDiscount.Description = description;
                dbInterCompanyDiscount.Percentage = discountPercentage;
                dbInterCompanyDiscount.LastModification = DateTime.UtcNow;
                dbInterCompanyDiscount.UpdateCount = dbInterCompanyDiscount.UpdateCount.CalculateUpdateCount();
                dbInterCompanyDiscount.ModifiedBy = modifiedBy;
                dbInterCompanyDiscount.AmendmentReason = amendmentReason;

            }
            return dbInterCompanyDiscount;
        }

        private static DbModel.VisitInterCompanyDiscount GetDbRecordsToAdd(long visitId, string discountType, int companyId, string description, decimal discountPercentage)
        {
            return new DbModel.VisitInterCompanyDiscount()
            {
                VisitId = visitId,
                DiscountType = discountType,
                CompanyId = companyId,
                Description = description,
                Percentage = discountPercentage
            };
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessVisitTechnicalSpecialist(long visitId,
                                                  IList<DomainModel.VisitTechnicalSpecialist> visitTechnicalSpecialists,
                                                  ref DomainModel.DbVisit dbVisitDBData,
                                                  IList<DbModel.SqlauditModule> dbModule,
                                                  bool commitChanges,
                                                  ValidationType validationType,
                                                  bool isDelete,
                                                  DomainModel.DbVisit dbVisitDBDataDelete)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (visitTechnicalSpecialists != null)
                {
                    if(isDelete)
                    {
                        if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && dbVisitDBDataDelete.DbVisitTechSpecialists != null)
                        {
                            IList<DbModel.VisitTechnicalSpecialist> dbVisitTS = dbVisitDBDataDelete.DbVisitTechSpecialists
                                                                                     .Where(x => visitTechnicalSpecialists.ToList()
                                                                                     .Any(x1 => x1.VisitTechnicalSpecialistId == x.Id &&
                                                                                                x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                            response = this._visitTechSpecService.Delete(visitTechnicalSpecialists,
                                                                             ref dbVisitTS,
                                                                             ref dbVisitDBDataDelete.DbTechnicalSpecialists,
                                                                             ref dbVisitDBDataDelete.DbVisits,
                                                                             dbModule,
                                                                             commitChanges,
                                                                             true,
                                                                             visitId);
                        }
                    }
                    else
                    {
                        if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && dbVisitDBData.DbVisitTechSpecialists != null)
                            response = this._visitTechSpecService.Modify(visitTechnicalSpecialists,
                                                                             ref dbVisitDBData.DbVisitTechSpecialists,
                                                                             ref dbVisitDBData.DbTechnicalSpecialists,
                                                                             ref dbVisitDBData.DbVisits,
                                                                             dbModule,
                                                                             commitChanges,
                                                                             false,
                                                                             visitId);

                        if (ValidationType.Delete != validationType)
                            response = this._visitTechSpecService.Add(visitTechnicalSpecialists,
                                                                           ref dbVisitDBData.DbVisitTechSpecialists,
                                                                           ref dbVisitDBData.DbTechnicalSpecialists,
                                                                           ref dbVisitDBData.DbVisits,
                                                                           dbModule,
                                                                           commitChanges,
                                                                           false,
                                                                           visitId);
                    }                    

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitTechnicalSpecialists);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessVisitTechSpecAccItemTime(long visitId,
                                                              IList<DomainModel.VisitSpecialistAccountItemTime> visitTechSpecAccItemTime,
                                                              ref DomainModel.DbVisit dbVisitDBData,
                                                              IList<DbModel.SqlauditModule> dbModule,
                                                              bool commitChanges,
                                                              ValidationType validationType)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (visitTechSpecAccItemTime != null)
                {
                    IList<DbModel.Data> dbExpenses = dbVisitDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType)?.ToList();
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && dbVisitDBData.DbVisitTechSpecTimes != null)
                        response = this._visitTechSpecAccountItemTimeService.Modify(visitTechSpecAccItemTime,
                                                                         ref dbVisitDBData.DbVisitTechSpecTimes,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         visitId);

                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && dbVisitDBData.DbVisitTechSpecTimes != null)
                    {
                        IList<DbModel.VisitTechnicalSpecialistAccountItemTime> dbVisitTimeTS = dbVisitDBData.DbVisitTechSpecTimes
                                                                                 .Where(x => visitTechSpecAccItemTime.ToList()
                                                                                 .Any(x1 => x1.VisitTechnicalSpecialistAccountTimeId == x.Id &&
                                                                                            x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this._visitTechSpecAccountItemTimeService.Delete(visitTechSpecAccItemTime,
                                                                         ref dbVisitTimeTS,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         visitId);
                    }
                    if (ValidationType.Delete != validationType)
                        response = this._visitTechSpecAccountItemTimeService.Add(visitTechSpecAccItemTime,
                                                                     ref dbVisitDBData.DbVisitTechSpecTimes,
                                                                     ref dbVisitDBData.DbVisits,
                                                                     ref dbVisitDBData.DbAssignments,
                                                                     ref dbVisitDBData.DbProjects,
                                                                     ref dbVisitDBData.DbContracts,
                                                                     ref dbExpenses,
                                                                     dbModule,
                                                                     commitChanges,
                                                                     false,
                                                                     visitId);

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitTechSpecAccItemTime);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessVisitTechSpecAccItemTravel(long visitId,
                                                              IList<DomainModel.VisitSpecialistAccountItemTravel> visitTechSpecAccItemTravel,
                                                              ref DomainModel.DbVisit dbVisitDBData,
                                                              IList<DbModel.SqlauditModule> dbModule,
                                                              bool commitChanges,
                                                              ValidationType validationType)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (visitTechSpecAccItemTravel != null)
                {
                    IList<DbModel.Data> dbExpenses = dbVisitDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType)?.ToList();
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && dbVisitDBData.DbVisitTechSpecTravels != null)
                        response = this._visitTechSpecAccountItemTravelService.Modify(visitTechSpecAccItemTravel,
                                                                         ref dbVisitDBData.DbVisitTechSpecTravels,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         visitId);

                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && dbVisitDBData.DbVisitTechSpecTravels != null)
                    {
                        IList<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbVisitTravelTS = dbVisitDBData.DbVisitTechSpecTravels
                                                                                 .Where(x => visitTechSpecAccItemTravel.ToList()
                                                                                 .Any(x1 => x1.VisitTechnicalSpecialistAccountTravelId == x.Id &&
                                                                                            x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this._visitTechSpecAccountItemTravelService.Delete(visitTechSpecAccItemTravel,
                                                                         ref dbVisitTravelTS,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         visitId);
                    }
                    if (ValidationType.Delete != validationType)
                        response = this._visitTechSpecAccountItemTravelService.Add(visitTechSpecAccItemTravel,
                                                                     ref dbVisitDBData.DbVisitTechSpecTravels,
                                                                     ref dbVisitDBData.DbVisits,
                                                                     ref dbVisitDBData.DbAssignments,
                                                                     ref dbVisitDBData.DbProjects,
                                                                     ref dbVisitDBData.DbContracts,
                                                                     ref dbExpenses,
                                                                     dbModule,
                                                                     commitChanges,
                                                                     false,
                                                                     visitId);

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitTechSpecAccItemTravel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessVisitTechSpecAccItemExpense(long visitId,
                                                              IList<DomainModel.VisitSpecialistAccountItemExpense> visitTechSpecAccItemExpense,
                                                              ref DomainModel.DbVisit dbVisitDBData,
                                                              IList<DbModel.SqlauditModule> dbModule,
                                                              bool commitChanges,
                                                              ValidationType validationType)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (visitTechSpecAccItemExpense != null)
                {
                    IList<DbModel.Data> dbExpenses = dbVisitDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType)?.ToList();
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && dbVisitDBData.DbVisitTechSpecExpenses != null)
                        response = this._visitTechSpecAccountItemExpenseService.Modify(visitTechSpecAccItemExpense,
                                                                         ref dbVisitDBData.DbVisitTechSpecExpenses,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         visitId);

                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && dbVisitDBData.DbVisitTechSpecExpenses != null)
                    {
                        IList<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbVisitExpenseTS = dbVisitDBData.DbVisitTechSpecExpenses
                                                                                 .Where(x => visitTechSpecAccItemExpense.ToList()
                                                                                 .Any(x1 => x1.VisitTechnicalSpecialistAccountExpenseId == x.Id &&
                                                                                            x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this._visitTechSpecAccountItemExpenseService.Delete(visitTechSpecAccItemExpense,
                                                                         ref dbVisitExpenseTS,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         visitId);
                    }
                    if (ValidationType.Delete != validationType)
                        response = this._visitTechSpecAccountItemExpenseService.Add(visitTechSpecAccItemExpense,
                                                                     ref dbVisitDBData.DbVisitTechSpecExpenses,
                                                                     ref dbVisitDBData.DbVisits,
                                                                     ref dbVisitDBData.DbAssignments,
                                                                     ref dbVisitDBData.DbProjects,
                                                                     ref dbVisitDBData.DbContracts,
                                                                     ref dbExpenses,
                                                                     dbModule,
                                                                     commitChanges,
                                                                     false,
                                                                     visitId);

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitTechSpecAccItemExpense);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessVisitTechSpecAccItemConsumables(long visitId,
                                                              IList<DomainModel.VisitSpecialistAccountItemConsumable> visitTechSpecAccItemConsumables,
                                                              ref DomainModel.DbVisit dbVisitDBData,
                                                              IList<DbModel.SqlauditModule> dbModule,
                                                              bool commitChanges,
                                                              ValidationType validationType)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (visitTechSpecAccItemConsumables != null)
                {
                    IList<DbModel.Data> dbExpenses = dbVisitDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType)?.ToList();
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && dbVisitDBData.DbVisitTechSpecConsumables != null)
                        response = this._visitTechSpecAccountItemConsumableService.Modify(visitTechSpecAccItemConsumables,
                                                                         ref dbVisitDBData.DbVisitTechSpecConsumables,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         visitId);

                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && dbVisitDBData.DbVisitTechSpecConsumables != null)
                    {
                        IList<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbVisitConsumablesTS = dbVisitDBData.DbVisitTechSpecConsumables
                                                                                 .Where(x => visitTechSpecAccItemConsumables.ToList()
                                                                                 .Any(x1 => x1.VisitTechnicalSpecialistAccountConsumableId == x.Id &&
                                                                                            x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this._visitTechSpecAccountItemConsumableService.Delete(visitTechSpecAccItemConsumables,
                                                                         ref dbVisitConsumablesTS,
                                                                         ref dbVisitDBData.DbVisits,
                                                                         ref dbVisitDBData.DbAssignments,
                                                                         ref dbVisitDBData.DbProjects,
                                                                         ref dbVisitDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         visitId);
                    }
                    if (ValidationType.Delete != validationType)
                        response = this._visitTechSpecAccountItemConsumableService.Add(visitTechSpecAccItemConsumables,
                                                                     ref dbVisitDBData.DbVisitTechSpecConsumables,
                                                                     ref dbVisitDBData.DbVisits,
                                                                     ref dbVisitDBData.DbAssignments,
                                                                     ref dbVisitDBData.DbProjects,
                                                                     ref dbVisitDBData.DbContracts,
                                                                     ref dbExpenses,
                                                                     dbModule,
                                                                     commitChanges,
                                                                     false,
                                                                     visitId);

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitTechSpecAccItemConsumables);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessVisitDocument(DomainModel.VisitDetail visit,
                                                 IList<DbModel.SqlauditModule> dbModule,
                                                 bool commitChanges,
                                                 ValidationType validationType,
                                                 DomainModel.VisitDetail visitDetail,
                                                 SqlAuditActionType sqlAuditActionType,
                                                 ref long? eventId, ref List<ModuleDocument> uploadVisitDocuments)
        {
            Exception exception = null;
            Response response = null;
            List<DbModel.Document> dbDocuments = null;
            IList<ModuleDocument> visitDocuments = visit.VisitDocuments;
            try
            {
                if (visitDocuments != null)
                {
                    bool isAuditDocument = false; //Hotfix Id 2 and 3
                    var auditVisitDetails = ObjectExtension.Clone(visitDetail);

                    if (ValidationType.Delete != validationType)
                    {
                        response = this._documentService.Save(visitDocuments, ref dbDocuments,
                                                             commitChanges);
                        if (response != null && response.Code == ResponseType.Success.ToId())
                        {
                            foreach (ModuleDocument moduleDocument in visitDocuments)
                            {
                                if (moduleDocument.RecordStatus == "N" && (moduleDocument.DocumentType == VisitTimesheetConstants.REPORT_FLASH
                                    || moduleDocument.DocumentType == VisitTimesheetConstants.RELEASE_NOTE || moduleDocument.DocumentType == VisitTimesheetConstants.NON_CONFORMANCE_REPORT))
                                {
                                    uploadVisitDocuments.Add(moduleDocument);
                                }
                            }
                        }
                        isAuditDocument = true; //Hotfix Id 2 and 3                        
                    }

                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                        response = this._documentService.Modify(visitDocuments, ref dbDocuments,
                                                                commitChanges);

                    if (validationType == ValidationType.Update || validationType == ValidationType.Delete)
                        response = this._documentService.Delete(visitDocuments,
                                                                commitChanges);

                    //Hotfix Id 2 and 3
                    if (isAuditDocument)
                    {
                        auditVisitDetails.VisitDocuments = visitDocuments.Select(x =>
                        {
                            x.CreatedOn = dbDocuments?.FirstOrDefault(y => y.DocumentUniqueName == x.DocumentUniqueName)?.CreatedDate;
                            return x;
                        }).ToList(); // audit create date issue fix
                    }
                    if (response.Code == MessageType.Success.ToId())
                    {
                        DocumentAudit(auditVisitDetails.VisitDocuments, dbModule, sqlAuditActionType, auditVisitDetails, ref eventId, ref dbDocuments);
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitDocuments);
            }

            return response;
        }

        private void DocumentAudit(IList<ModuleDocument> visitDocuments, IList<DbModel.SqlauditModule> dbModule, SqlAuditActionType sqlAuditActionType, DomainModel.VisitDetail visitDetail, ref long? eventId, ref List<DbModel.Document> dbDocuments)
        {
            //For Document Audit
            if (visitDocuments.Count > 0)
            {
                object newData;
                object oldData;
                var newDocument = visitDocuments?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
                var modifiedDocument = visitDocuments?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
                var deletedDocument = visitDocuments?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();
                if (newDocument.Count > 0)
                {
                    newData = newDocument;
                    _auditSearchService.AuditLog(visitDetail, ref eventId, visitDetail?.VisitInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.VisitDocument, null, newData, dbModule);
                }
                if (modifiedDocument.Count > 0)
                {
                    newData = modifiedDocument?.OrderBy(x => x.Id)?.ToList();
                    oldData = (dbDocuments != null && dbDocuments.Count > 0) ? _mapper.Map<List<ModuleDocument>>(dbDocuments)?.OrderBy(x => x.Id)?.ToList() : null;
                    _auditSearchService.AuditLog(visitDetail, ref eventId, visitDetail?.VisitInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.VisitDocument, oldData, newData, dbModule);
                }
                if (deletedDocument.Count > 0)
                {
                    oldData = deletedDocument;
                    _auditSearchService.AuditLog(visitDetail, ref eventId, visitDetail?.VisitInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.VisitDocument, oldData, null, dbModule);
                }
            }
        }

        /*This section is called after validation to perform operation to save data*/
        private Response ProcessVisitNote(long visitId,
                                              IList<DomainModel.VisitNote> visitNotes,
                                              ref DomainModel.DbVisit dbVisitDBData,
                                              IList<DbModel.SqlauditModule> dbModule,
                                              bool commitChanges,
                                              ValidationType validationType)
        {
            Exception exception = null;
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());       //D661 issue 8 
            try
            {
                if (visitNotes != null)
                {
                    var addNotes = visitNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();       //D661 issue 8 
                    var updateNotes = visitNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    if (addNotes?.Count > 0)
                        response = this._visitNoteService.Add(visitNotes,
                                                                    ref dbVisitDBData.DbVisitNotes,
                                                                    ref dbVisitDBData.DbVisits,
                                                                    dbModule,
                                                                    commitChanges,
                                                                    false,
                                                                    visitId);
                    if (updateNotes?.Count > 0 && response.Code == MessageType.Success.ToId())        //D661 issue 8 
                        response = this._visitNoteService.Update(visitNotes,
                                                                    ref dbVisitDBData.DbVisitNotes,
                                                                    ref dbVisitDBData.DbVisits,
                                                                    dbModule,
                                                                    commitChanges,
                                                                    false,
                                                                    visitId);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitNotes);
            }

            return response;
        }


        private async Task<Tuple<decimal, decimal, decimal>> CalculateTechSpecGrossMargin(long visitId,
                                                                                            long visitTechnicalSpecialistId)
        {
            decimal totalCharge = 0, totalPay = 0, grossMargin = 0;

            List<ExchangeRate> timesheetTechSpecExchangeRates = new List<ExchangeRate>();

            var timeTask = await this._visitTechSpecAccountItemTimeService
            .GetAsync(new DomainModel.VisitSpecialistAccountItemTime
            {
                VisitId = visitId,
                VisitTechnicalSpecialistId = visitTechnicalSpecialistId
            });
            var timeLineItems = timeTask.Result.Populate<IList<DomainModel.VisitSpecialistAccountItemTime>>();

            var tarvelTask = await _visitTechSpecAccountItemTravelService
            .GetAsync(new DomainModel.VisitSpecialistAccountItemTravel
            {
                VisitId = visitId,
                VisitTechnicalSpecialistId = visitTechnicalSpecialistId
            });
            var travelLineItems = tarvelTask.Result.Populate<IList<DomainModel.VisitSpecialistAccountItemTravel>>();

            var expenseTask = await _visitTechSpecAccountItemExpenseService
               .GetAsync(new DomainModel.VisitSpecialistAccountItemExpense
               {
                   VisitId = visitId,
                   VisitTechnicalSpecialistId = visitTechnicalSpecialistId
               });
            var expenseLineItems = expenseTask.Result.Populate<IList<DomainModel.VisitSpecialistAccountItemExpense>>();

            var consumableTask = await _visitTechSpecAccountItemConsumableService
                .GetAsync(new DomainModel.VisitSpecialistAccountItemConsumable
                {
                    VisitId = visitId,
                    VisitTechnicalSpecialistId = visitTechnicalSpecialistId
                });
            var consumableLineItems = consumableTask.Result.Populate<IList<DomainModel.VisitSpecialistAccountItemConsumable>>();

            bool isTimeLineItemsExists = (timeLineItems != null && timeLineItems.Count > 0);
            bool isTravelLineItemsExists = (travelLineItems != null && travelLineItems.Count > 0);
            bool isExpenseLineItemsExists = (expenseLineItems != null && expenseLineItems.Count > 0);
            bool isConsumableLineItemsExists = (consumableLineItems != null && consumableLineItems.Count > 0);
            var effectiveDate = DateTime.Today;
            //Get all line items currency to get ExchangeRate for calculation
            if (isTimeLineItemsExists)
            {
                foreach (var timeLI in timeLineItems.Where(x =>
                                                    !string.IsNullOrEmpty(x.ChargeRateCurrency)
                                                 && !string.IsNullOrEmpty(x.PayRateCurrency)))
                {
                    timesheetTechSpecExchangeRates.Add(new ExchangeRate
                    {
                        CurrencyFrom = timeLI.ChargeRateCurrency,
                        CurrencyTo = "GBP",
                        EffectiveDate = effectiveDate
                    });
                    timesheetTechSpecExchangeRates.Add(new ExchangeRate
                    {
                        CurrencyFrom = timeLI.PayRateCurrency,
                        CurrencyTo = "GBP",
                        EffectiveDate = effectiveDate
                    });
                }
            }
            if (isTravelLineItemsExists)
            {
                foreach (var travelLI in travelLineItems.Where(x =>
                                                    !string.IsNullOrEmpty(x.ChargeRateCurrency)
                                                 && !string.IsNullOrEmpty(x.PayRateCurrency)))
                {
                    timesheetTechSpecExchangeRates.Add(new ExchangeRate
                    {
                        CurrencyFrom = travelLI.ChargeRateCurrency,
                        CurrencyTo = "GBP",
                        EffectiveDate = effectiveDate
                    });
                    timesheetTechSpecExchangeRates.Add(new ExchangeRate
                    {
                        CurrencyFrom = travelLI.PayRateCurrency,
                        CurrencyTo = "GBP",
                        EffectiveDate = effectiveDate
                    });
                }
            }
            if (isExpenseLineItemsExists)
            {
                foreach (var expenseLI in expenseLineItems.Where(x =>
                                                    !string.IsNullOrEmpty(x.ChargeRateCurrency)
                                                 && !string.IsNullOrEmpty(x.PayRateCurrency)))
                {
                    timesheetTechSpecExchangeRates.Add(new ExchangeRate
                    {
                        CurrencyFrom = expenseLI.ChargeRateCurrency,
                        CurrencyTo = "GBP",
                        EffectiveDate = effectiveDate
                    });
                    timesheetTechSpecExchangeRates.Add(new ExchangeRate
                    {
                        CurrencyFrom = expenseLI.PayRateCurrency,
                        CurrencyTo = "GBP",
                        EffectiveDate = effectiveDate
                    });
                }
            }
            if (isConsumableLineItemsExists)
            {
                foreach (var consumableLI in consumableLineItems.Where(x =>
                                                    !string.IsNullOrEmpty(x.ChargeRateCurrency)
                                                 && !string.IsNullOrEmpty(x.PayRateCurrency)))
                {
                    timesheetTechSpecExchangeRates.Add(new ExchangeRate
                    {
                        CurrencyFrom = consumableLI.ChargeRateCurrency,
                        CurrencyTo = "GBP",
                        EffectiveDate = effectiveDate
                    });
                    timesheetTechSpecExchangeRates.Add(new ExchangeRate
                    {
                        CurrencyFrom = consumableLI.PayRateCurrency,
                        CurrencyTo = "GBP",
                        EffectiveDate = effectiveDate
                    });
                }
            }

            var uniqueTechSpecExchangeRates = timesheetTechSpecExchangeRates
                            .GroupBy(x => new { x.CurrencyFrom, x.CurrencyTo })
                            .Select(g => g.FirstOrDefault()).ToList();

            //get all currency exhange rates once
            var currencyExchangeRates = this._currencyExchangeRateService
                                            .GetMiiwaExchangeRates(uniqueTechSpecExchangeRates)
                                            .Result
                                            .Populate<IList<ExchangeRate>>();

            if (isTimeLineItemsExists && currencyExchangeRates != null)
            {
                foreach (var time in timeLineItems.Where(x => ((Convert.ToDecimal(x.ChargeRate)> 0 && x.ChargeTotalUnit > 0) || (x.PayRate > 0 && x.PayUnit > 0))))
                {
                    decimal chargeCurrencyRate = currencyExchangeRates
                           .Where(x => (x.CurrencyFrom == time.ChargeRateCurrency && x.CurrencyTo == "GBP"))
                           .Select(s => s.Rate).SingleOrDefault();

                    decimal payCurrencyRate = currencyExchangeRates
                           .Where(x => (x.CurrencyFrom == time.PayRateCurrency && x.CurrencyTo == "GBP"))
                           .Select(s => s.Rate).SingleOrDefault();
                    if (string.IsNullOrEmpty(time.ModeOfCreation))
                    {
                        totalCharge += ((Convert.ToDecimal(time.ChargeRate) * time.ChargeTotalUnit) * chargeCurrencyRate);
                    }
                    totalPay += ((time.PayRate * time.PayUnit) * payCurrencyRate);
                }
            }
            if (isTravelLineItemsExists && currencyExchangeRates != null)
            {
                foreach (var tarvel in travelLineItems.Where(x => ((Convert.ToDecimal(x.ChargeRate) > 0 && x.ChargeTotalUnit > 0) || (x.PayRate > 0 && x.PayUnit > 0))))
                {
                    decimal chargeCurrencyRate = currencyExchangeRates
                           .Where(x => (x.CurrencyFrom == tarvel.ChargeRateCurrency && x.CurrencyTo == "GBP"))
                           .Select(s => s.Rate).SingleOrDefault();

                    decimal payCurrencyRate = currencyExchangeRates
                           .Where(x => (x.CurrencyFrom == tarvel.PayRateCurrency && x.CurrencyTo == "GBP"))
                           .Select(s => s.Rate).SingleOrDefault();

                    if (string.IsNullOrEmpty(tarvel.ModeOfCreation))
                    {
                        totalCharge += ((Convert.ToDecimal(tarvel.ChargeRate) * tarvel.ChargeTotalUnit) * chargeCurrencyRate);
                    }
                    totalPay += ((tarvel.PayRate * tarvel.PayUnit) * payCurrencyRate);
                }
            }
            if (isExpenseLineItemsExists && currencyExchangeRates != null)
            {
                foreach (var expense in expenseLineItems.Where(x => ((Convert.ToDecimal(x.ChargeRate) > 0 && x.ChargeUnit > 0) || (x.PayRate > 0 && x.PayUnit > 0))))
                {
                    decimal chargeCurrencyRate = currencyExchangeRates
                           .Where(x => (x.CurrencyFrom == expense.ChargeRateCurrency && x.CurrencyTo == "GBP"))
                           .Select(s => s.Rate).SingleOrDefault();

                    decimal payCurrencyRate = currencyExchangeRates
                           .Where(x => (x.CurrencyFrom == expense.PayRateCurrency && x.CurrencyTo == "GBP"))
                           .Select(s => s.Rate).SingleOrDefault();

                    if (string.IsNullOrEmpty(expense.ModeOfCreation))
                    {
                        totalCharge += ((Convert.ToDecimal(expense.ChargeRate) * expense.ChargeUnit) * expense.ChargeRateExchange * chargeCurrencyRate);
                    }
                    totalPay += ((expense.PayRate * expense.PayUnit) * expense.PayRateExchange * payCurrencyRate);
                }
            }

            if (isConsumableLineItemsExists && currencyExchangeRates != null)
            {
                foreach (var consumable in consumableLineItems.Where(x => ((Convert.ToDecimal(x.ChargeRate) > 0 && x.ChargeTotalUnit > 0) || (x.PayRate > 0 && x.PayUnit > 0))))
                {
                    decimal chargeCurrencyRate = currencyExchangeRates
                           .Where(x => (x.CurrencyFrom == consumable.ChargeRateCurrency && x.CurrencyTo == "GBP"))
                           .Select(s => s.Rate).SingleOrDefault();

                    decimal payCurrencyRate = currencyExchangeRates
                           .Where(x => (x.CurrencyFrom == consumable.PayRateCurrency && x.CurrencyTo == "GBP"))
                           .Select(s => s.Rate).SingleOrDefault();

                    if (string.IsNullOrEmpty(consumable.ModeOfCreation))
                    {
                        totalCharge += ((Convert.ToDecimal(consumable.ChargeRate) * consumable.ChargeTotalUnit) * chargeCurrencyRate);
                    }
                    totalPay += ((consumable.PayRate * consumable.PayUnit) * payCurrencyRate);
                }
            }
            if (totalCharge == 0)
                return new Tuple<decimal, decimal, decimal>(0, totalPay, 0);

            grossMargin = Math.Round(((totalCharge - totalPay) / totalCharge) * 100, 2);

            return new Tuple<decimal, decimal, decimal>(totalCharge, totalPay, grossMargin); ;
        }

        private Response ProcessEmailNotifications(DomainModel.VisitEmailData visitEmailData, EmailTemplate emailTemplateType, ref long? eventId, IList<DbModel.SqlauditModule> dbModule)
        {
            string emailSubject = string.Empty;
            string visitDate = string.Empty;
            string visitNumber = string.Empty;
            string coordinatorName = string.Empty;
            string SAMAccountName = string.Empty;
            string loggedInUser = string.Empty;
            string companyCode = string.Empty;
            Exception exception = null;
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            List<EmailAddress> toAddresses = null;
            List<EmailAddress> fromAddresses = null;
            IList<KeyValuePair<string, string>> emailContentPlaceholders = null;
            List<string> CHApprovalStatus = new List<string>() { "A", "R" };
            List<string> OCApprovalStatus = new List<string>() { "O", "C" };
            IList<UserInfo> userInfos = null;
            DomainModel.Visit visitInfo = null;
            EmailType emailType = EmailType.Notification;
            EmailDocumentUpload emailDocumentUpload = new EmailDocumentUpload
            {
                IsDocumentUpload = false
            };
            List<EmailAddress> ccAddresses = null;
            try
            {
                visitInfo = visitEmailData.VisitDetail.VisitInfo;
                SAMAccountName = visitInfo.VisitOperatingCompanyCoordinatorCode;
                //if CHC approves or rejects send mail to OC
                if (CHApprovalStatus.Contains(visitInfo.VisitStatus))
                {
                    coordinatorName = visitInfo.VisitOperatingCompanyCoordinator;
                    SAMAccountName = visitInfo.VisitOperatingCompanyCoordinatorCode;
                    companyCode = visitInfo.VisitContractCompanyCode;
                }
                //if oc approves send mail to CHC
                if (OCApprovalStatus.Contains(visitInfo.VisitStatus))
                {
                    coordinatorName = visitInfo.VisitContractCoordinator;
                    SAMAccountName = visitInfo.VisitContractCoordinatorCode;
                    companyCode = visitInfo.VisitOperatingCompanyCode;
                }
                //if oc rejects send mail to resource
                if (visitInfo.VisitStatus == "J")
                {
                    coordinatorName = visitInfo.VisitOperatingCompanyCoordinator;
                    SAMAccountName = visitInfo.VisitOperatingCompanyCoordinatorCode;
                    companyCode = visitInfo.VisitOperatingCompanyCode;
                }

                loggedInUser = (!string.IsNullOrEmpty(visitEmailData?.VisitDetail?.VisitInfo?.ModifiedBy) ? visitEmailData?.VisitDetail?.VisitInfo?.ModifiedBy : visitEmailData?.VisitDetail?.VisitInfo?.ActionByUser);
                List<string> userTypes = new List<string> { VisitTimesheetConstants.UserType_Coordinator,
                VisitTimesheetConstants.UserType_MICoordinator, VisitTimesheetConstants.Technical_Specialist };
                List<string> loginNames = new List<string>();
                bool isIntercompanyAssignment = visitInfo.VisitContractCompanyCode != visitInfo.VisitOperatingCompanyCode;
                if (visitInfo.VisitStatus == "J" || (!isIntercompanyAssignment && visitInfo.VisitStatus == "R"
                    && (visitInfo.VisitOperatingCompanyCoordinatorCode == visitInfo.VisitContractCoordinatorCode
                    || loggedInUser == visitInfo.VisitOperatingCompanyCoordinatorCode)))
                {
                    loginNames = visitEmailData?.VisitDetail?.VisitTechnicalSpecialists?.Select(x => x.LoginName).ToList();
                }
                if (!string.IsNullOrEmpty(SAMAccountName))
                {
                    loginNames.Add(SAMAccountName);
                }
                if (!string.IsNullOrEmpty(loggedInUser))
                {
                    loginNames.Add(loggedInUser);
                }
                if(!string.IsNullOrEmpty(visitInfo.VisitContractCoordinatorCode))
                    loginNames.Add(visitInfo.VisitContractCoordinatorCode);
                if(!string.IsNullOrEmpty(visitInfo.VisitOperatingCompanyCoordinatorCode))
                    loginNames.Add(visitInfo.VisitOperatingCompanyCoordinatorCode);
                userInfos = _userService.GetByUserType(loginNames, userTypes, true)
                                        .Result
                                        .Populate<IList<UserInfo>>();


                if (userInfos != null && userInfos.Count > 0)
                {
                    if (emailTemplateType == EmailTemplate.VisitReject && (visitInfo.VisitStatus == "J" || (!isIntercompanyAssignment && visitInfo.VisitStatus == "R"
                        && (visitInfo.VisitOperatingCompanyCoordinatorCode == visitInfo.VisitContractCoordinatorCode
                        || loggedInUser == visitInfo.VisitOperatingCompanyCoordinatorCode))))
                    {
                        var technicalSpecialists = visitEmailData?.VisitDetail?.VisitTechnicalSpecialists?.ToList();
                        toAddresses = (from usr in userInfos
                                       join ts in technicalSpecialists
                                       on usr.LogonName equals ts.LoginName
                                       where (usr.LogonName == ts.LoginName)
                                       select new EmailAddress()
                                       {
                                           DisplayName = ts.FullName,
                                           Address = usr.Email
                                       }).ToList();
                    }
                    else if (emailTemplateType == EmailTemplate.VisitApproveClientRequirement)
                    {
                        if (visitEmailData.ToAddress != null && visitEmailData.ToAddress.Count > 0)
                        {
                            toAddresses = new List<EmailAddress>();
                            foreach (EmailAddress emailAddress in visitEmailData.ToAddress)
                            {
                                toAddresses.Add(new EmailAddress() { DisplayName = string.Empty, Address = emailAddress.Address });
                            }
                        }
                        else
                        {
                            IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(visitEmailData.VisitDetail.VisitInfo.VisitProjectNumber);
                            if (projectNotification != null && projectNotification.Count > 0)
                            {
                                toAddresses = projectNotification.Where(x => x.IsSendCustomerReportingNotification == true)
                                                .Select(x => new EmailAddress() { DisplayName = string.Empty, Address = x.EmailAddress }).ToList();
                            }
                        }

                    }
                    else if (emailTemplateType == EmailTemplate.EmailCustomerDirectReporting)
                    {
                        IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(visitEmailData.VisitDetail.VisitInfo.VisitProjectNumber);
                        if (projectNotification != null && projectNotification.Count > 0)
                        {
                            toAddresses = projectNotification.Where(x => x.IsSendCustomerDirectReportingNotification == true)
                                            .Select(x => new EmailAddress() { DisplayName = string.Empty, Address = x.EmailAddress }).ToList();
                        }
                        loggedInUser = SAMAccountName;
                    }
                    else if (emailTemplateType == EmailTemplate.EmailCustomerFlashReporting)
                    {
                        IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(visitEmailData.VisitDetail.VisitInfo.VisitProjectNumber);
                        if (projectNotification != null && projectNotification.Count > 0)
                        {
                            toAddresses = projectNotification.Where(x => x.IsSendFlashReportingNotification == true)
                                            .Select(x => new EmailAddress() { DisplayName = string.Empty, Address = x.EmailAddress }).ToList();

                            ccAddresses = (from usr in userInfos
                                       where usr.LogonName == visitInfo.VisitOperatingCompanyCoordinatorCode || usr.LogonName == visitInfo.VisitContractCoordinatorCode
                                       select new EmailAddress()
                                       {
                                           DisplayName = string.Empty,
                                           Address = usr.Email
                                       }).ToList();

                            if(ccAddresses != null && ccAddresses.Count > 0 && visitInfo.VisitOperatingCompanyCoordinatorCode == visitInfo.VisitContractCoordinatorCode)
                            {
                                ccAddresses.AddRange(ccAddresses);
                            }
                        }
                    }
                    else if (emailTemplateType == EmailTemplate.EmailCustomerInspectionReleaseNotes)
                    {
                        IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(visitEmailData.VisitDetail.VisitInfo.VisitProjectNumber);
                        if (projectNotification != null && projectNotification.Count > 0)
                        {
                            toAddresses = projectNotification.Where(x => x.IsSendInspectionReleaseNotesNotification == true)
                                            .Select(x => new EmailAddress() { DisplayName = string.Empty, Address = x.EmailAddress }).ToList();

                            ccAddresses = (from usr in userInfos
                                       where usr.LogonName == visitInfo.VisitOperatingCompanyCoordinatorCode || usr.LogonName == visitInfo.VisitContractCoordinatorCode
                                       select new EmailAddress()
                                       {
                                           DisplayName = string.Empty,
                                           Address = usr.Email
                                       }).ToList();
                            
                            if(ccAddresses != null && ccAddresses.Count > 0 && visitInfo.VisitOperatingCompanyCoordinatorCode == visitInfo.VisitContractCoordinatorCode)
                            {
                                ccAddresses.AddRange(ccAddresses);
                            }
                        }
                    }
                    else if (emailTemplateType == EmailTemplate.EmailNCRReporting)
                    {
                        IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(visitEmailData.VisitDetail.VisitInfo.VisitProjectNumber);
                        if (projectNotification != null && projectNotification.Count > 0)
                        {
                            toAddresses = projectNotification.Where(x => x.IsSendNCRReportingNotification == true)
                                            .Select(x => new EmailAddress() { DisplayName = string.Empty, Address = x.EmailAddress }).ToList();

                            ccAddresses = (from usr in userInfos
                                       where usr.LogonName == visitInfo.VisitOperatingCompanyCoordinatorCode || usr.LogonName == visitInfo.VisitContractCoordinatorCode
                                       select new EmailAddress()
                                       {
                                           DisplayName = string.Empty,
                                           Address = usr.Email
                                       }).ToList();
                            
                            if(ccAddresses != null && ccAddresses.Count > 0 && visitInfo.VisitOperatingCompanyCoordinatorCode == visitInfo.VisitContractCoordinatorCode)
                            {
                                ccAddresses.AddRange(ccAddresses);
                            }
                        }
                    }
                    else
                    {
                        toAddresses = userInfos.Where(x => string.Equals(x.LogonName, SAMAccountName))
                                        .Select(x => new EmailAddress()
                                        { DisplayName = x.UserName, Address = x.Email })
                                        .ToList();
                        if (!string.IsNullOrEmpty(loggedInUser))
                        {
                            ccAddresses = userInfos.Where(x => string.Equals(x.LogonName, loggedInUser))
                                     .Select(x => new EmailAddress()
                                      { DisplayName = x.UserName, Address = x.Email })
                                      .ToList();
                        }
                    }

                    #region From Address for VisitAppove ,VisitReject and Others - CR1021
                    if (emailTemplateType==EmailTemplate.VisitApprove)
                    {
                        fromAddresses = userInfos.Where(x => string.Equals(x.LogonName, visitInfo.VisitOperatingCompanyCoordinatorCode))
                                            .Select(x => new EmailAddress()
                                            { DisplayName = x.UserName, Address = x.Email })
                                            .ToList();
                        if (fromAddresses != null && fromAddresses.Count > 0)
                            coordinatorName = fromAddresses[0].DisplayName;
                    }
                    else if(emailTemplateType == EmailTemplate.VisitReject)
                    {
                        fromAddresses = userInfos.Where(x => string.Equals(x.LogonName, visitInfo.VisitContractCoordinatorCode))
                                            .Select(x => new EmailAddress()
                                            { DisplayName = x.UserName, Address = x.Email })
                                            .ToList();
                        if (fromAddresses != null && fromAddresses.Count > 0)
                            coordinatorName = fromAddresses[0].DisplayName;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(loggedInUser))
                        {
                            fromAddresses = userInfos.Where(x => string.Equals(x.LogonName, loggedInUser))
                                            .Select(x => new EmailAddress()
                                            { DisplayName = x.UserName, Address = x.Email })
                                            .ToList();
                            if (fromAddresses != null && fromAddresses.Count > 0)
                                coordinatorName = fromAddresses[0].DisplayName;
                        }
                    }
                    #endregion

                    if (emailTemplateType == EmailTemplate.EmailCustomerFlashReporting || emailTemplateType == EmailTemplate.EmailCustomerInspectionReleaseNotes
                        || emailTemplateType == EmailTemplate.EmailNCRReporting || toAddresses != null && toAddresses.Count > 0)
                    {
                        visitDate = visitInfo.VisitStartDate.ToString("dd/MMM/yyyy");
                        string reportNumber = string.IsNullOrEmpty(visitInfo.VisitReportNumber) ? "" : visitInfo.VisitReportNumber;
                        string assignmentNumber = Convert.ToString(visitInfo.VisitAssignmentNumber);

                        string formattedCustomerName = !String.IsNullOrWhiteSpace(visitInfo.VisitCustomerName) && visitInfo.VisitCustomerName.Length >= 5
                                                            ? visitInfo.VisitCustomerName.Substring(0, 5)
                                                            : visitInfo.VisitCustomerName;
                        visitNumber = string.Format("({0} : {1})", assignmentNumber, reportNumber);
                        string projectNumber = Convert.ToString(visitInfo.VisitProjectNumber);
                        string visitReportNumber = visitInfo.VisitNumber.ToString("000000") + " " + reportNumber;
                        string visitNDT = (!string.IsNullOrEmpty(visitInfo.WorkflowType) && visitInfo.WorkflowType.Trim() == "S"
                                                ? VisitTimesheetConstants.VISITNDT : VisitTimesheetConstants.VISIT);
                        string visitNDTLower = (!string.IsNullOrEmpty(visitInfo.WorkflowType) && visitInfo.WorkflowType.Trim() == "S"
                                                ? VisitTimesheetConstants.VISITNDT_LOWER : VisitTimesheetConstants.VISIT_LOWER);
                        string visitURL = (string.IsNullOrEmpty(_environment.ExtranetURL) ? "" : _environment.ExtranetURL + Convert.ToString(visitInfo.VisitId));
                        List<Attachment> attachments = new List<Attachment>();

                        switch (emailTemplateType)
                        {
                            case EmailTemplate.VisitReject:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, visitInfo.VisitAssignmentNumber.ToString("00000")),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, visitDate),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, visitNDT),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, visitNDTLower),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_REJECT_NOTES,
                                                            string.IsNullOrEmpty(visitEmailData.ReasonForRejection) ? "": string.Concat(VisitTimesheetConstants.REASON_FOR_REJECTION, visitEmailData.ReasonForRejection)),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER,  reportNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, visitInfo.VisitCustomerName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, (visitInfo.VisitStatus == "R" ? visitInfo.VisitContractCompany : visitInfo.VisitOperatingCompany)),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, visitInfo.VisitContractNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, visitInfo.VisitCustomerProjectName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, visitInfo.VisitSupplier),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, visitInfo.VisitSupplierPONumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, visitURL),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, visitInfo.VisitDatePeriod),
                                    };
                                if (!string.IsNullOrEmpty(visitInfo.WorkflowType) && visitInfo.WorkflowType.Trim() == "S")
                                    emailSubject = string.Format(VisitTimesheetConstants.EMAIL_NOTIFICATION_VISIT_NDT_REJECT_SUBJECT, visitInfo.VisitNumber.ToString("000000"));
                                else
                                    emailSubject = string.Format(VisitTimesheetConstants.EMAIL_NOTIFICATION_VISIT_REJECT_SUBJECT, visitInfo.VisitNumber.ToString("000000"));
                                emailType = visitInfo.VisitStatus == "R" ? EmailType.RVC : EmailType.RVT;
                                break;
                            case EmailTemplate.VisitApproveClientRequirement:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, assignmentNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, visitDate),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, visitNDT),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, visitNDTLower),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER,  reportNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, visitInfo.VisitCustomerName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, visitInfo.VisitContractCompany),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, visitInfo.VisitContractNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, visitInfo.VisitCustomerProjectName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, visitInfo.VisitSupplier),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, visitInfo.VisitSupplierPONumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, visitInfo.VisitDatePeriod),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, visitURL)
                                    };
                                if (visitEmailData.Attachments != null && visitEmailData.Attachments.Count > 0)
                                {
                                    attachments = visitEmailData.Attachments;
                                }
                                if (string.IsNullOrEmpty(visitEmailData.EmailSubject))
                                    emailSubject = string.Format(VisitTimesheetConstants.EMAIL_NOTIFICATION_VISIT_CLIENTREPORTING_SUBJECT, visitNumber);
                                else
                                    emailSubject = visitEmailData.EmailSubject;
                                emailType = EmailType.CRN;
                                break;
                            case EmailTemplate.VisitApprove:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, visitInfo.VisitAssignmentNumber.ToString("00000")),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, visitDate),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, visitNDT),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, visitNDTLower),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER,  reportNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, visitInfo.VisitCustomerName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, visitInfo.VisitContractCompany),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, visitInfo.VisitContractNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, visitInfo.VisitCustomerProjectName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, visitInfo.VisitSupplier),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, visitInfo.VisitSupplierPONumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, visitInfo.VisitDatePeriod),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, visitURL),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISITTIMESHEET, visitNDT),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.OPERATING_COMPANY_NAME, visitInfo.VisitOperatingCompany),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.OPERATOR_NAME, visitInfo.VisitOperatingCompanyCoordinator)
                                    };
                                emailSubject = string.Format(VisitTimesheetConstants.EMAIL_NOTIFICATION_VISIT_APPROVE_SUBJECT, visitInfo.VisitContractNumber, projectNumber, visitInfo.VisitAssignmentNumber.ToString("00000"), visitReportNumber);
                                emailType = EmailType.IVA;
                                break;
                            case EmailTemplate.EmailCustomerDirectReporting:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, visitInfo.VisitAssignmentNumber.ToString("00000")),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, visitDate),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, visitNDT),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, visitNDTLower),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER,  reportNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, visitInfo.VisitCustomerName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, visitInfo.VisitOperatingCompany),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, visitInfo.VisitContractNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, visitInfo.VisitCustomerProjectName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, visitInfo.VisitSupplier),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, visitInfo.VisitSupplierPONumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, visitInfo.VisitDatePeriod),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, visitURL)
                                    };
                                emailSubject = VisitTimesheetConstants.EMAIL_DIRECT_REPORTING;
                                emailType = EmailType.CDR;
                                break;
                            case EmailTemplate.EmailCustomerFlashReporting:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, assignmentNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, visitDate),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, visitNDT),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, visitNDTLower),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER,  reportNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, visitInfo.VisitCustomerName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, visitInfo.VisitContractCompany),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, visitInfo.VisitContractNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, visitInfo.VisitCustomerProjectName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, visitInfo.VisitSupplier),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, visitInfo.VisitSupplierPONumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, visitInfo.VisitDatePeriod),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, visitURL)
                                    };
                                emailSubject = string.Format(VisitTimesheetConstants.FLASH_REPORT_NOTIFICATION, visitInfo.VisitSupplier, visitDate);
                                emailType = EmailType.FRN;
                                break;
                            case EmailTemplate.EmailCustomerInspectionReleaseNotes:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, assignmentNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, visitDate),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, visitNDT),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, visitNDTLower),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER,  reportNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, visitInfo.VisitCustomerName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, visitInfo.VisitContractCompany),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, visitInfo.VisitContractNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, visitInfo.VisitCustomerProjectName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, visitInfo.VisitSupplier),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, visitInfo.VisitSupplierPONumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, visitInfo.VisitDatePeriod),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, visitURL)
                                    };
                                emailSubject = string.Format(VisitTimesheetConstants.RELEASE_NOTES_NOTIFICATION, visitInfo.VisitSupplier, visitDate);
                                emailType = EmailType.IRN;
                                break;
                            case EmailTemplate.EmailNCRReporting:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, assignmentNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, visitDate),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, visitNDT),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, visitNDTLower),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER,  reportNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, visitInfo.VisitCustomerName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, visitInfo.VisitContractCompany),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, visitInfo.VisitContractNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, visitInfo.VisitCustomerProjectName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, visitInfo.VisitSupplier),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, visitInfo.VisitSupplierPONumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, visitInfo.VisitDatePeriod),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, visitURL)
                                    };
                                emailSubject = string.Format(VisitTimesheetConstants.NCR_NOTIFICATION, visitInfo.VisitSupplier, visitDate);
                                emailType = EmailType.NCR;
                                break;
                        }
                        string token = DateTime.Now.ToString(VisitTimesheetConstants.TOKEN_DATE_FORMAT);
                        emailMessage = ProcessEmailMessage(ModuleType.Visit, emailTemplateType, companyCode,
                                        emailType, ModuleCodeType.VST,
                                        visitInfo.VisitJobReference.ToString(), emailSubject, emailContentPlaceholders, 
                                        toAddresses, fromAddresses, token, attachments, visitEmailData.EmailContent, ccAddresses);
                        emailDocumentUpload = UploadVisitDocuments(emailTemplateType, visitInfo, emailMessage, visitEmailData, ref eventId, dbModule, token);
                        _emailService.Add(new List<EmailQueueMessage> { emailMessage });
                    }
                }
                else
                {
                    //add respective validation message
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, emailDocumentUpload, exception);
        }

        private EmailDocumentUpload UploadVisitDocuments(EmailTemplate emailTemplateType, DomainModel.Visit visitInfo, EmailQueueMessage emailMessage, DomainModel.VisitEmailData visitEmailData, ref long? eventId, IList<DbModel.SqlauditModule> dbModule, string token)
        {
            EmailDocumentUpload emailDocumentUpload = new EmailDocumentUpload();
            DocumentUniqueNameDetail documentUniquename = new DocumentUniqueNameDetail();
            try
            {
                bool IsVisibleToCustomer = false;
                StringBuilder documentMessage = new StringBuilder();
                if (emailTemplateType == EmailTemplate.VisitReject)
                {
                    documentUniquename.DocumentName = VisitTimesheetConstants.VISIT_REJECTED_EMAIL_LOG;
                    if (emailMessage.FromAddresses != null && emailMessage.FromAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_FROM);
                        documentMessage.AppendLine(string.Concat(emailMessage.FromAddresses[0].DisplayName, " <span><</span>", emailMessage.FromAddresses[0].Address, "<span>></span>"));
                    }
                    if (emailMessage.ToAddresses != null && emailMessage.ToAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_TO);
                        for (int i = 0; i < emailMessage.ToAddresses.Count; i++)
                        {
                            documentMessage.Append(string.Concat("\"", emailMessage.ToAddresses[i].DisplayName, "\" <span><</span>", emailMessage.ToAddresses[i].Address, "<span>></span>"));
                            if (i < emailMessage.ToAddresses.Count - 1)
                                documentMessage.Append("; ");
                        }
                        documentMessage.AppendLine();
                    }
                    if (emailMessage.CcAddresses != null && emailMessage.CcAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_CC);
                        documentMessage.AppendLine(string.Concat(emailMessage.CcAddresses[0].DisplayName, " <span><</span>", emailMessage.CcAddresses[0].Address, "<span>></span>"));
                    }
                    documentMessage.Append(VisitTimesheetConstants.EMAIL_SUBJECT);
                    documentMessage.AppendLine(emailMessage.Subject);
                    documentMessage.Append(VisitTimesheetConstants.EMAIL_MESSAGE);
                    documentMessage.AppendLine(emailMessage.Content);
                }
                else if (emailTemplateType == EmailTemplate.VisitApproveClientRequirement)
                {
                    documentUniquename.DocumentName = VisitTimesheetConstants.CUSTOMER_REPORT_NOTIFICATION_EMAIL_LOG;
                    if (emailMessage.FromAddresses != null && emailMessage.FromAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_FROM);
                        documentMessage.AppendLine(string.Concat(emailMessage.FromAddresses[0].DisplayName, " <span><</span>", emailMessage.FromAddresses[0].Address, "<span>></span>"));
                    }
                    if (emailMessage.ToAddresses != null && emailMessage.ToAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_TO);
                        for (int i = 0; i < emailMessage.ToAddresses.Count; i++)
                        {
                            documentMessage.Append(emailMessage.ToAddresses[i].Address);
                            if (i < emailMessage.ToAddresses.Count - 1)
                                documentMessage.Append("; ");
                        }
                        documentMessage.AppendLine();
                    }
                    documentMessage.Append(VisitTimesheetConstants.EMAIL_SUBJECT);
                    documentMessage.AppendLine(emailMessage.Subject);
                    documentMessage.AppendLine(emailMessage.Content);
                    if (emailMessage.Attachments != null && emailMessage.Attachments.Count > 0)
                    {
                        documentMessage.AppendLine(VisitTimesheetConstants.EMAIL_DOCUMENT_ATTACHED);
                        foreach (Attachment attachment in emailMessage.Attachments)
                        {
                            documentMessage.AppendLine(attachment.AttachmentFileName);
                        }
                    }
                }
                else if (emailTemplateType == EmailTemplate.VisitApprove)
                {
                    documentUniquename.DocumentName = VisitTimesheetConstants.VISIT_APPROVED_EMAIL_LOG;
                    if (emailMessage.FromAddresses != null && emailMessage.FromAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_FROM);
                        documentMessage.AppendLine(string.Concat(emailMessage.FromAddresses[0].DisplayName, " <span><</span>", emailMessage.FromAddresses[0].Address, "<span>></span>"));
                    }
                    if (emailMessage.ToAddresses != null && emailMessage.ToAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_TO);
                        for (int i = 0; i < emailMessage.ToAddresses.Count; i++)
                        {
                            documentMessage.Append(emailMessage.ToAddresses[i].Address);
                            if (i < emailMessage.ToAddresses.Count - 1)
                                documentMessage.Append("; ");
                        }
                        documentMessage.AppendLine();
                    }
                    if (emailMessage.CcAddresses != null && emailMessage.CcAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_CC);
                        documentMessage.AppendLine(string.Concat(emailMessage.CcAddresses[0].DisplayName, " <span><</span>", emailMessage.CcAddresses[0].Address, "<span>></span>"));
                    }
                    documentMessage.Append(VisitTimesheetConstants.EMAIL_SUBJECT);
                    documentMessage.AppendLine(emailMessage.Subject);
                    documentMessage.AppendLine(emailMessage.Content);
                }
                else if (emailTemplateType == EmailTemplate.EmailCustomerDirectReporting)
                {
                    documentUniquename.DocumentName = VisitTimesheetConstants.CUSTOMER_DIRECT_REPORT_EMAIL_LOG;
                    if (emailMessage.FromAddresses != null && emailMessage.FromAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_FROM);
                        documentMessage.AppendLine(string.Concat(emailMessage.FromAddresses[0].DisplayName, " <span><</span>", emailMessage.FromAddresses[0].Address, "<span>></span>"));
                    }
                    if (emailMessage.ToAddresses != null && emailMessage.ToAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_TO);
                        for (int i = 0; i < emailMessage.ToAddresses.Count; i++)
                        {
                            documentMessage.Append(string.Concat("\"", visitInfo.VisitCustomerName, "\" <span><</span>", emailMessage.ToAddresses[i].Address, "<span>></span>"));
                            if (i < emailMessage.ToAddresses.Count - 1)
                                documentMessage.Append("; ");
                        }
                        documentMessage.AppendLine();
                    }
                    documentMessage.Append(VisitTimesheetConstants.EMAIL_SUBJECT);
                    documentMessage.AppendLine(emailMessage.Subject);
                    documentMessage.AppendLine(emailMessage.Content);
                }
                else if (emailTemplateType == EmailTemplate.EmailCustomerFlashReporting)
                {
                    documentUniquename.DocumentName = VisitTimesheetConstants.FLASH_REPORT_NOTIFICATION_EMAIL_LOG;
                    IsVisibleToCustomer = true;
                    documentMessage.AppendLine(emailMessage.Content);
                }
                else if (emailTemplateType == EmailTemplate.EmailCustomerInspectionReleaseNotes)
                {
                    documentUniquename.DocumentName = VisitTimesheetConstants.INSPECTION_RELEASE_NOTES_EMAIL_LOG;
                    IsVisibleToCustomer = true;
                    documentMessage.AppendLine(emailMessage.Content);
                }
                else if (emailTemplateType == EmailTemplate.EmailNCRReporting)
                {
                    documentUniquename.DocumentName = VisitTimesheetConstants.NCR_REPORT_NOTIFICATIO_EMAIL_LOG;
                    IsVisibleToCustomer = true;
                    documentMessage.AppendLine(emailMessage.Content);
                }

              else if (emailTemplateType == EmailTemplate.EmailVisitInterCompanyAmendmentReason)
                {
                    documentUniquename.DocumentName = VisitTimesheetConstants.VISIT_INTER_COMPANY_AMENDMENT_REASON_LOG;
                    if (emailMessage.FromAddresses != null && emailMessage.FromAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_FROM);
                        documentMessage.AppendLine(string.Concat(emailMessage.FromAddresses[0].DisplayName, " <span><</span>", emailMessage.FromAddresses[0].Address, "<span>></span>"));
                    }
                    if (emailMessage.ToAddresses != null && emailMessage.ToAddresses.Count > 0)
                    {
                        documentMessage.Append(VisitTimesheetConstants.EMAIL_TO);
                        for (int i = 0; i < emailMessage.ToAddresses.Count; i++)
                        {
                            documentMessage.Append(string.Concat("\"", emailMessage.ToAddresses[i].DisplayName, "\" <span><</span>", emailMessage.ToAddresses[i].Address, "<span>></span>"));
                            if (i < emailMessage.ToAddresses.Count - 1)
                                documentMessage.Append("; ");
                        }
                        documentMessage.AppendLine();
                    }
                    documentMessage.Append(VisitTimesheetConstants.EMAIL_SUBJECT);
                    documentMessage.AppendLine(emailMessage.Subject);
                    documentMessage.Append(VisitTimesheetConstants.EMAIL_MESSAGE);
                    documentMessage.AppendLine(emailMessage.Content);
                }
                // documentMessage.Append(VisitTimesheetConstants.EMAIL_TOKEN);
                // documentMessage.Append(token);

                documentUniquename.ModuleCode = ModuleCodeType.VST.ToString();
                documentUniquename.RequestedBy = string.Empty;
                documentUniquename.ModuleCodeReference = Convert.ToString(visitInfo.VisitId);
                documentUniquename.DocumentType = VisitTimesheetConstants.VISIT_TIMSHEET_EVOLUTION_EMAIL;
                documentUniquename.SubModuleCodeReference = "0";

                emailDocumentUpload.IsDocumentUpload = true;
                emailDocumentUpload.DocumentUniqueName = documentUniquename;
                emailDocumentUpload.DocumentMessage = this.FormatDocumentMessage(Convert.ToString(documentMessage));
                emailDocumentUpload.IsVisibleToCustomer = IsVisibleToCustomer;
                emailDocumentUpload.IsVisibleToTS = false;

                this.PostRequest(emailDocumentUpload, visitEmailData, ref eventId, dbModule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documentUniquename);
            }
            return emailDocumentUpload;
        }

        private string FormatDocumentMessage(string documentMessage)
        {
            try
            {
                documentMessage = documentMessage.ToString().Replace("<p>", "").Replace("</p>", "<br/>").Replace("\r\n", "<br/>").Replace("\n", "<br/>");
                if (documentMessage.Contains("<http"))
                {
                    int startIndex = documentMessage.IndexOf("<http");
                    string documentUrl = documentMessage.Substring(startIndex);
                    int endIndex = documentUrl.IndexOf(">");
                    documentUrl = documentMessage.Substring(startIndex, endIndex + 1);
                    string formattedURL = string.Concat("<a href=\"", documentUrl.Substring(1, documentUrl.Length - 2), "\">", documentUrl.Substring(1, documentUrl.Length - 2), "</a>");
                    documentMessage = Regex.Replace(documentMessage, documentUrl, formattedURL, RegexOptions.IgnoreCase);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documentMessage);
            }
            return documentMessage;
        }

        private Response PostRequest(EmailDocumentUpload model, DomainModel.VisitEmailData visitEmailData, ref long? eventId, IList<DbModel.SqlauditModule> dbModule)
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
                        var responserResult = JsonConvert.DeserializeObject<Response>(response.Result.Content.ReadAsStringAsync().Result);
                        long? documentSize = null;
                        if (responserResult != null && responserResult.Result != null)
                        {
                            var documentResult = JsonConvert.DeserializeObject<IList<DocumentUniqueNameDetail>>(JsonConvert.SerializeObject(responserResult.Result));
                            documentSize = documentResult != null && documentResult.Count > 0 ? documentResult[0].DocumentSize : null;
                        }
                        ModuleDocument moduleDocument = new ModuleDocument
                        {
                            DocumentName = model?.DocumentUniqueName?.DocumentName,
                            DocumentType = model?.DocumentUniqueName?.DocumentType,
                            IsVisibleToTS = model?.IsVisibleToTS,
                            IsVisibleToCustomer = model?.IsVisibleToCustomer,
                            Status = model?.DocumentUniqueName?.Status,
                            ModuleCode = model?.DocumentUniqueName?.ModuleCode,
                            ModuleRefCode = model?.DocumentUniqueName?.ModuleCodeReference,
                            CreatedOn = DateTime.UtcNow,
                            DocumentSize = documentSize,
                            RecordStatus = "N"
                        };

                        IList<ModuleDocument> listModuleDocument = new List<ModuleDocument>
                        {
                            moduleDocument
                        };
                        List<DbModel.Document> dbDocuments = new List<DbModel.Document>();
                        DocumentAudit(listModuleDocument, dbModule, SqlAuditActionType.I, visitEmailData.VisitDetail, ref eventId, ref dbDocuments);
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

        private EmailQueueMessage ProcessEmailMessage(ModuleType moduleType, EmailTemplate emailTemplateType, string companyCode, EmailType emailType
                                                    , ModuleCodeType moduleCodeType, string moduleRefCode, string emailSubject
                                                    , IList<KeyValuePair<string, string>> emailContentPlaceholders, List<EmailAddress> toAddresses, List<EmailAddress> fromAddresses, string token
                                                    , List<Attachment> attachment = null, string emailContent = null, List<EmailAddress> ccAddresses = null, List<EmailAddress> bccAddresses = null)
        {
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            string emailTemplateContent = string.Empty;
            try
            {
                if (emailTemplateType == EmailTemplate.VisitApproveClientRequirement)
                    emailTemplateContent = _visitRepository.GetTemplate(companyCode, CompanyMessageType.EmailCustomerReportingNotification, EmailKey.EmailCustomerReportingNotification.ToString());
                else if (emailTemplateType == EmailTemplate.VisitApprove)
                    emailTemplateContent = _emailService.GetEmailTemplate(new List<string> { EmailTemplate.EmailApproveIVA.ToString() })?.FirstOrDefault(x => x.KeyName == EmailTemplate.EmailApproveIVA.ToString())?.KeyValue;
                else if (emailTemplateType == EmailTemplate.VisitReject)
                    emailTemplateContent = _visitRepository.GetTemplate(companyCode, CompanyMessageType.EmailRejectedVisit, EmailKey.EmailRejectedVisit.ToString());
                else if (emailTemplateType == EmailTemplate.EmailCustomerDirectReporting)
                    emailTemplateContent = _visitRepository.GetTemplate(companyCode, CompanyMessageType.EmailCustomerDirectReporting, EmailKey.EmailCustomerDirectReporting.ToString());
                else if (emailTemplateType == EmailTemplate.EmailCustomerFlashReporting)
                    emailTemplateContent = _visitRepository.GetTemplate(string.Empty, CompanyMessageType.NotRequired, EmailKey.EmailCustomerFlashReporting.ToString());
                else if (emailTemplateType == EmailTemplate.EmailCustomerInspectionReleaseNotes)
                    emailTemplateContent = _visitRepository.GetTemplate(string.Empty, CompanyMessageType.NotRequired, EmailKey.EmailCustomerInspectionReleaseNotes.ToString());
                else if (emailTemplateType == EmailTemplate.EmailNCRReporting)
                    emailTemplateContent = _visitRepository.GetTemplate(string.Empty, CompanyMessageType.NotRequired, EmailKey.EmailNCRReporting.ToString());

                if (emailTemplateType == EmailTemplate.VisitApproveClientRequirement && !string.IsNullOrEmpty(emailContent))
                {
                    emailContentPlaceholders.ToList().ForEach(x =>
                    {
                        emailContent = Regex.Replace(emailContent, x.Key, (string.IsNullOrEmpty(x.Value) ? string.Empty : x.Value), RegexOptions.None);
                    });
                    emailTemplateContent = emailContent;
                }
                else if (emailContentPlaceholders?.Count > 0 && !string.IsNullOrEmpty(emailTemplateContent))
                {
                    emailContentPlaceholders.ToList().ForEach(x =>
                    {
                        emailTemplateContent = Regex.Replace(emailTemplateContent, x.Key, (string.IsNullOrEmpty(x.Value) ? string.Empty : x.Value), RegexOptions.None);
                    });
                }
                else // Standard Timesheet -  Different Coordinator, Parent Child - Intercompany - Timesheet, Visit failed because of content null error
                {
                    emailTemplateContent = string.Empty;
                }

                emailMessage.BccAddresses = bccAddresses ?? new List<EmailAddress>();
                emailMessage.CcAddresses = ccAddresses ?? new List<EmailAddress>();
                emailMessage.FromAddresses = fromAddresses;
                emailMessage.ToAddresses = toAddresses;
                emailMessage.CreatedOn = DateTime.UtcNow;
                emailMessage.EmailType = emailType.ToString();
                emailMessage.ModuleCode = moduleCodeType.ToString();
                emailMessage.ModuleEmailRefCode = moduleRefCode.ToString();
                emailMessage.IsMailSendAsGroup = true;
                emailMessage.Subject = emailSubject;
                emailMessage.Content = this.FormatDocumentMessage(emailTemplateContent) + "<br/>Token = " + token;
                emailMessage.Token = token;
                if (attachment?.Count > 0)
                    emailMessage.Attachments = attachment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return emailMessage;
        }
        private void AppendEvent(DomainModel.VisitDetail visitDetail,
                         long? eventId)
        {
            ObjectExtension.SetPropertyValue(visitDetail.VisitInfo, "EventId", eventId);
            ObjectExtension.SetPropertyValue(visitDetail.HistoricalVisits, "EventId", eventId);
            ObjectExtension.SetPropertyValue(visitDetail.VisitInterCompanyDiscounts, "EventId", eventId);
            ObjectExtension.SetPropertyValue(visitDetail.VisitNotes, "EventId", eventId);
            ObjectExtension.SetPropertyValue(visitDetail.VisitReferences, "EventId", eventId);
            ObjectExtension.SetPropertyValue(visitDetail.VisitSupplierPerformances, "EventId", eventId);
            ObjectExtension.SetPropertyValue(visitDetail.VisitTechnicalSpecialistConsumables, "EventId", eventId);
            ObjectExtension.SetPropertyValue(visitDetail.VisitTechnicalSpecialistExpenses, "EventId", eventId);
            ObjectExtension.SetPropertyValue(visitDetail.VisitTechnicalSpecialists, "EventId", eventId);
            ObjectExtension.SetPropertyValue(visitDetail.VisitTechnicalSpecialistTimes, "EventId", eventId);
            ObjectExtension.SetPropertyValue(visitDetail.VisitTechnicalSpecialistTravels, "EventId", eventId);

        }

        private IList<ProjectClientNotification> GetProjectClientNotifications(int? projectNumber)
        {
            try
            {
                ProjectClientNotification projectSearchModel = new ProjectClientNotification
                {
                    ProjectNumber = projectNumber
                };
                return _projectClientNotificationRepository.Search(projectSearchModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return null;
        }


        private Response GetExpenseLineItemChargeExchangeRates(IList<ExchangeRate> models, string ContractNumber)
        {
            List<ExchangeRate> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                var currenciesValueNeedToFetch = models?.Where(x => x.CurrencyFrom != x.CurrencyTo).ToList();

                if (currenciesValueNeedToFetch != null && currenciesValueNeedToFetch?.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ContractNumber))
                    {
                        var contractExchangeRateSearchModel = new DomainContractModel.ContractExchangeRate
                        {
                            ContractNumber = ContractNumber
                        };
                        var listContractExchangeRate = this._contractExchangeRateService
                                                       .GetContractExchangeRate(contractExchangeRateSearchModel)
                                                       .Result
                                                       .Populate<List<DomainContractModel.ContractExchangeRate>>();
                        if (listContractExchangeRate != null && listContractExchangeRate.Count > 0)
                        {
                            foreach (var currencyVal in currenciesValueNeedToFetch)
                            {
                                DateTime? MaxEffDate = null;
                                var MaxEffDateQuery = listContractExchangeRate.Where(cer => cer.FromCurrency == currencyVal.CurrencyFrom
                                                                      && cer.ToCurrency == currencyVal.CurrencyTo
                                                                      && cer.EffectiveFrom <= currencyVal.EffectiveDate)?
                                                                      .Select(x => x);
                                if (MaxEffDateQuery.Any())
                                {
                                    MaxEffDate = MaxEffDateQuery.Max(x => x.EffectiveFrom);
                                }
                                if (MaxEffDate != null)
                                {
                                    currencyVal.Rate = Convert.ToDecimal(listContractExchangeRate.Where(cer => cer.FromCurrency == currencyVal.CurrencyFrom
                                                                       && cer.ToCurrency == currencyVal.CurrencyTo
                                                                       && cer.EffectiveFrom == MaxEffDate)
                                                                    .FirstOrDefault(x => x.EffectiveFrom == MaxEffDate).ExchangeRate);
                                }
                            }
                            var exchangeCurrenciesWithRates = currenciesValueNeedToFetch.Where(r => r.Rate != 0).ToList();
                            if (exchangeCurrenciesWithRates != null && exchangeCurrenciesWithRates.Count > 0)
                            {
                                result = result ?? new List<ExchangeRate>();
                                result.AddRange(exchangeCurrenciesWithRates);
                            }
                        }
                    }
                    var exchangeCurrenciesWithOutRates = currenciesValueNeedToFetch.Where(r => r.Rate == 0).ToList();
                    var fetchResult = this._currencyExchangeRateService
                                            .GetMiiwaExchangeRates(exchangeCurrenciesWithOutRates)
                                            .Result
                                            .Populate<List<ExchangeRate>>();

                    if (fetchResult != null && fetchResult.Count > 0)
                    {
                        result = result ?? new List<ExchangeRate>();
                        result.AddRange(fetchResult);
                    }
                }

                var sameCurrencies = models?.Where(x => x.CurrencyFrom == x.CurrencyTo).ToList();
                sameCurrencies.ForEach(x1 => { x1.Rate = 1; });

                result = result ?? new List<ExchangeRate>();

                if (sameCurrencies?.Count > 0)
                    result.AddRange(sameCurrencies);
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                this._logger.LogError(responseType.ToId(), this._messages[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }

        private string GetSearchRef(DomainModel.DbVisit dbVisitData)
        {
            string searchRef = string.Empty;
            try
            {
                if (dbVisitData != null && dbVisitData.DbVisits != null && dbVisitData.DbVisits.Count > 0)
                {
                    searchRef =
                        "{" + AuditSelectType.Id + ":" + dbVisitData.DbVisits[0].Id + "}${" + AuditSelectType.ReportNumber + ":" + dbVisitData.DbVisits[0].ReportNumber?.Trim() + 
                        "}${" + AuditSelectType.JobReferenceNumber + ":" + dbVisitData.DbVisits[0].Assignment?.Project.ProjectNumber + "-" + 
                        dbVisitData.DbVisits[0].Assignment?.AssignmentNumber + "-" + dbVisitData.DbVisits[0].VisitNumber + "}${" +
                        AuditSelectType.ProjectAssignment + ":" + dbVisitData.DbVisits[0].Assignment?.Project.ProjectNumber + "-" + dbVisitData.DbVisits[0].Assignment?.AssignmentNumber + "}";
                }
            }
            catch(Exception ex)
            {
                searchRef = string.Empty;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return searchRef;
        }
    }
}
