using AutoMapper;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.Timesheet.Domain.Interfaces.Data;
using Evolution.Timesheet.Domain.Interfaces.Timesheets;
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
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Email.Models;
using Evolution.Common.Constants;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using DomainTsModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Evolution.Company.Domain.Enums;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.Project.Domain.Interfaces.Data;
using Evolution.Project.Domain.Models.Projects;
using System.Text.RegularExpressions;
using System.Web;
using Evolution.NumberSequence.InfraStructure.Interface;

namespace Evolution.Timesheet.Core.Services
{
    public class TimesheetDetailService : ITimesheetDetailService
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<TimesheetDetailService> _logger = null;
        private readonly IAssignmentService _assignmentService = null;
        private readonly ITechnicalSpecialistService _techSpecService = null;
        private readonly ITimesheetService _timesheetService = null;
        private readonly ITimesheetRepository _timesheetRepository = null;
        private readonly ITimesheetNoteService _timesheetNoteService = null;
        private readonly INumberSequenceRepository _numberSequenceRepository = null;
        private readonly ITimesheetReferenceService _timesheetReferenceService = null;
        private readonly ITechSpecAccountItemConsumableService _timesheetTechSpecAccountItemConsumableService = null;
        private readonly ITechSpecAccountItemTimeService _timesheetTechSpecAccountItemTimeService = null;
        private readonly ITechSpecAccountItemExpenseService _timesheetTechSpecAccountItemExpenseService = null;
        private readonly ITechSpecAccountItemTravelService _timesheetTechSpecAccountItemTravelService = null;
        private readonly ITimesheetTechSpecService _timesheetTechSpecService = null;
        private readonly IMasterService _masterService = null;
        private readonly IDocumentService _documentService = null;
        private readonly JObject _messages = null;
        private readonly ITimesheetTechnicalSpecialistRepository _timesheetTechnicalSpecialistRepository = null;
        private readonly ICurrencyExchangeRateService _currencyExchangeRateService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly IUserService _userService = null;
        private readonly IEmailQueueService _emailService = null;
        private readonly ITechnicalSpecialistCalendarService _technicalSpecialistCalendarService = null;
        private readonly IProjectClientNotificationRepository _projectClientNotificationRepository = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly ITimesheetInterCompanyDiscountsRepository _timesheetInterCompanyRepository = null;
        public readonly string _emailDocumentEndpoint = "documents/UploadDocuments";

        public TimesheetDetailService(IAppLogger<TimesheetDetailService> logger,
                                        ITimesheetService timesheetService,
                                        ITimesheetRepository timesheetRepository,
                                        ITimesheetNoteService timesheetNoteService,
                                        ITimesheetReferenceService timesheetReferenceService,
                                        ITechSpecAccountItemConsumableService techSpecAccountItemConsumableService,
                                        ITechSpecAccountItemTimeService techSpecAccountItemTimeService,
                                        ITechSpecAccountItemExpenseService techSpecAccountItemExpenseService,
                                        ITechSpecAccountItemTravelService techSpecAccountItemTravelService,
                                        ITimesheetTechSpecService timesheetTechSpecService,
                                        ITechnicalSpecialistService techSpecService,
                                        ITimesheetTechnicalSpecialistRepository timesheetTechnicalSpecialistRepository,
                                        ICurrencyExchangeRateService currencyExchangeRateService,
                                        IAssignmentService assignmentService,
                                        IMasterService masterService,
                                        IDocumentService documentService,
                                        IMapper mapper, JObject messages,
                                        IUserService userService,
                                        IEmailQueueService emailService,
                                        ITechnicalSpecialistCalendarService technicalSpecialistCalendarService,
                                        IAuditSearchService auditSearchService,
                                        IProjectClientNotificationRepository projectClientNotificationRepository,
                                        IOptions<AppEnvVariableBaseModel> environment,
                                        ITimesheetInterCompanyDiscountsRepository timesheetInterCompanyRepository,
                                        INumberSequenceRepository numberSequenceRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _timesheetService = timesheetService;
            _timesheetRepository = timesheetRepository;
            _timesheetNoteService = timesheetNoteService;
            _timesheetReferenceService = timesheetReferenceService;
            _timesheetTechSpecService = timesheetTechSpecService;
            _timesheetTechSpecAccountItemConsumableService = techSpecAccountItemConsumableService;
            _timesheetTechSpecAccountItemExpenseService = techSpecAccountItemExpenseService;
            _timesheetTechSpecAccountItemTimeService = techSpecAccountItemTimeService;
            _timesheetTechSpecAccountItemTravelService = techSpecAccountItemTravelService;
            _masterService = masterService;
            _assignmentService = assignmentService;
            _techSpecService = techSpecService;
            _documentService = documentService;
            _numberSequenceRepository = numberSequenceRepository;
            this._messages = messages;
            _timesheetTechnicalSpecialistRepository = timesheetTechnicalSpecialistRepository;
            _currencyExchangeRateService = currencyExchangeRateService;
            _userService = userService;
            _emailService = emailService;
            _technicalSpecialistCalendarService = technicalSpecialistCalendarService;
            _auditSearchService = auditSearchService;
            _projectClientNotificationRepository = projectClientNotificationRepository;
            _environment = environment.Value;
            _timesheetInterCompanyRepository = timesheetInterCompanyRepository;
        }


        #region Public Methods
        public Response Get(DomainModel.TimesheetDetail searchModel)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetTechnicalSpecialistWithGrossMargin(DomainModel.TimesheetTechnicalSpecialist searchModel)
        {
            DomainModel.TimesheetTechnicalSpecialistGrossMargin timesheetTechSpecs = new DomainModel.TimesheetTechnicalSpecialistGrossMargin();
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                // _logger.LogError("Module:TIMESHEET", string.Format("Entered Timesheet GetTechnicalSpecialistWithGrossMargin in {0}", DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")), searchModel);
                if (searchModel.TimesheetId > 0)
                {
                    string[] includes = new string[] { "Timesheet", "TechnicalSpecialist" };
                    timesheetTechSpecs.TimesheetTechnicalSpecialists =
                        this._timesheetTechnicalSpecialistRepository.Search(searchModel, includes);

                    if (timesheetTechSpecs.TimesheetTechnicalSpecialists != null &&
                        timesheetTechSpecs.TimesheetTechnicalSpecialists.Count > 0)
                    {
                        //variables that holds each technical specialist totalcharge and totalpay values
                        decimal accountTotalCharge = 0, accountTotalPay = 0;
                        //loop through each technical specialist and get the line items
                        foreach (var eachTechSpec in timesheetTechSpecs.TimesheetTechnicalSpecialists)
                        {
                            var calculatedData = await Task.Run(() => CalculateTechSpecGrossMargin(
                                searchModel.TimesheetId ?? default(int),
                                eachTechSpec.TimesheetTechnicalSpecialistId ?? default(int)));
                            eachTechSpec.GrossMargin = calculatedData.Item3;

                            accountTotalCharge += calculatedData.Item1;
                            accountTotalPay += calculatedData.Item2;
                        }

                        //calculate total Timesheet Account Gross margin
                        if (accountTotalCharge == 0)
                            timesheetTechSpecs.TimesheetAccountGrossMargin = 0;
                        else
                            timesheetTechSpecs.TimesheetAccountGrossMargin =
                                Math.Round(((accountTotalCharge - accountTotalPay) / accountTotalCharge) * 100, 2);
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

            return new Response().ToPopulate(responseType, null, null, null, timesheetTechSpecs, exception);
            //return await Task.Run(() => new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count));
        }

        /// <summary>
        /// Mandatory Validations check for Request Model
        /// </summary>
        /// <param name="visit"></param>
        /// <returns></returns>
        private Response MandatoryValidations(DomainModel.TimesheetDetail timesheet)
        {
            var validationMessages = new List<ValidationMessage>();
            if (timesheet?.TechnicalSpecialistCalendarList != null && timesheet?.TechnicalSpecialistCalendarList.Count > 0)
            {
                foreach (var calendar in timesheet?.TechnicalSpecialistCalendarList)
                {
                    if (!string.IsNullOrEmpty(calendar?.StartDateTime?.ToString()))
                    {
                        if (Convert.ToDateTime(calendar?.StartDateTime).Date < timesheet.TimesheetInfo.TimesheetStartDate && Convert.ToDateTime(calendar?.StartDateTime).Date > timesheet.TimesheetInfo.TimesheetEndDate)
                            validationMessages.Add(_messages, ModuleType.Visit, MessageType.InvalidCalendarStarttime);
                        if (Convert.ToDateTime(calendar?.EndDateTime).Date < timesheet.TimesheetInfo.TimesheetStartDate && Convert.ToDateTime(calendar?.EndDateTime).Date > timesheet.TimesheetInfo.TimesheetEndDate)
                            validationMessages.Add(_messages, ModuleType.Visit, MessageType.InvalidCalendarStarttime);
                    }

                    if (validationMessages.Count > 0)
                        return new Response().ToPopulate(ResponseType.Validation, null, null, validationMessages, null, null, null);
                }
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages, null, null, null);
        }

        public Response Add(DomainModel.TimesheetDetail timesheet, bool IsAPIValidationRequired = false)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long timesheetId = 0;
            Response response = null;
            ResponseType responseType = ResponseType.Success;
            long? eventId = null;

            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialistCalendar> dBTsCalendarInfo = null;
            IList<DbModel.Company> dbCompanies = null;
            DbModel.NumberSequence dbNumberSequence = null;
            bool isConsumableLineItemsExists = false, isExpenseLineItemsExists = false, isTimeLineItemsExists = false, isTravelLineItemsExists = false, isTimesheetReferencesExists = false;
            bool isAwaitingApproval = false;
            EmailDocument emailDocument = new EmailDocument
            {
                IsDocumentUpload = false
            };
            IList<DbModel.SqlauditModule> dbModule = null;
            try
            {
                if (timesheet != null && timesheet.TimesheetInfo != null)
                {
                    response = MandatoryValidations(timesheet);
                    if (response.Code != ResponseType.Success.ToId() && validationMessages?.Count > 0)
                    {
                        return response;
                    }
                    DomainModel.DbTimesheet dbTimesheetData = null;
                    using (dbTimesheetData = new DomainModel.DbTimesheet())
                    {
                        if (timesheet != null && !string.IsNullOrEmpty(timesheet?.TimesheetInfo?.RecordStatus))
                            response = ValidateTimesheetInfo(timesheet, ref dbTimesheetData, ref validationMessages, ValidationType.Add,
                                                            ref dbTechnicalSpecialists, ref dBTsCalendarInfo, ref dbCompanies, response);
                        if (response.Code == ResponseType.Success.ToId() & validationMessages?.Count == 0)
                            GetMasterData(timesheet, ref dbTimesheetData, ref isConsumableLineItemsExists, ref isExpenseLineItemsExists, ref isTimeLineItemsExists,
                                         ref isTravelLineItemsExists, ref isTimesheetReferencesExists, ref validationMessages, response, ref dbModule);
                        if (response != null && response?.Code == ResponseType.Success.ToId() && validationMessages?.Count == 0)
                        {
                            AssignTimesheetId(timesheetId, timesheet);
                            // Commented for duplicate Timesheet Number Generation -11 Mar 2021
                            //dbNumberSequence = _numberSequenceRepository.FindBy(x => x.ModuleData == timesheet.TimesheetInfo.TimesheetAssignmentId && x.ModuleId == 5)?.FirstOrDefault();
                            //int timesheetNumber = dbNumberSequence == null ? 1 : dbNumberSequence.LastSequenceNumber + 1;

                            //Commented for Snapshot Error -09 Mar 2021
                            //int timesheetNumber = _timesheetService.ProcessNumberSequence(timesheet.TimesheetInfo.TimesheetAssignmentId, out dbNumberSequence);

                            //To-Do: Will create helper method get TransactionScope instance based on requirement
                            using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                            {
                                _timesheetRepository.AutoSave = false;
                                //timesheet.TimesheetInfo.TimesheetNumber = timesheetNumber;
                                response = ProcessTimesheetInfo(new List<DomainModel.Timesheet> { timesheet.TimesheetInfo },
                                                                ref dbTimesheetData,
                                                                ref dbModule,
                                                                ref eventId,
                                                                true,
                                                                ValidationType.Add);
                                if ((response == null || response?.Code == MessageType.Success.ToId()) &&
                                    dbTimesheetData.DbTimesheets != null)
                                {
                                    timesheetId = Convert.ToInt64(dbTimesheetData.DbTimesheets?.FirstOrDefault()?.Id);
                                    if (timesheetId > 0)
                                    {

                                        AssignTimesheetId(timesheetId, timesheet);
                                        response = ValidateData(timesheet, ref dbTimesheetData, isConsumableLineItemsExists, isExpenseLineItemsExists, isTimeLineItemsExists,
                                                    isTravelLineItemsExists, isTimesheetReferencesExists, ref validationMessages, response);

                                        if (response.Code == ResponseType.Success.ToId())
                                        {
                                            List<ModuleDocument> uploadTimesheetDocuments = new List<ModuleDocument>();
                                            response = ProcessTimesheetDetail(timesheet,
                                                                                ValidationType.Add,
                                                                                dbTimesheetData,
                                                                                dbModule,
                                                                                ref timesheetId,
                                                                                ref eventId,
                                                                                ref uploadTimesheetDocuments);

                                            if (response.Code == ResponseType.Success.ToId())
                                            {
                                                response = ProcessTSCalendarData(dbTimesheetData.filteredAddTSCalendarInfo,
                                                                                dbTimesheetData.filteredModifyTSCalendarInfo,
                                                                                dbTimesheetData.filteredDeleteTSCalendarInfo, timesheetId,
                                                                                dbCompanies, ref response);
                                            }

                                            if (response.Code == ResponseType.Success.ToId())
                                            {
                                                if (timesheet.TimesheetInfo.TimesheetStatus == "C")
                                                {
                                                    isAwaitingApproval = true;
                                                    bool isDocumentUpload = false;
                                                    IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(timesheet.TimesheetInfo.TimesheetProjectNumber);
                                                    if (projectNotification != null && projectNotification.Count > 0)
                                                    {
                                                        isDocumentUpload = projectNotification.Where(x => x.IsSendCustomerDirectReportingNotification == true).Count() > 0;
                                                    }

                                                    if (isDocumentUpload)
                                                    {
                                                        DomainModel.TimesheetEmailData timesheetEmailData = new DomainModel.TimesheetEmailData
                                                        {
                                                            TimesheetDetail = timesheet
                                                        };
                                                        timesheetEmailData.TimesheetDetail.TimesheetInfo.TimesheetId = timesheetId;
                                                        var emailResponse = ProcessEmailNotifications(timesheetEmailData, EmailTemplate.EmailCustomerDirectReporting, ref validationMessages, ref eventId, dbModule);
                                                        if (emailResponse != null && emailResponse.Result != null)
                                                        {
                                                            EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                                                            if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                                                            {
                                                                if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                                                                emailDocumentUpload.DocumentUniqueName.ModuleCodeReference = Convert.ToString(timesheetId);
                                                                emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                                                                emailDocument.IsDocumentUpload = true;
                                                            }
                                                        }
                                                    }
                                                }

                                                _timesheetRepository.AutoSave = false;
                                                _timesheetRepository.ForceSave();
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
                            if (timesheetId > 0 && response.Code == ResponseType.Success.ToId())
                            {
                                // Commented for duplicate Timesheet Number Generation -11 Mar 2021
                                //SaveNumberSequence(timesheetNumber, timesheet.TimesheetInfo.TimesheetAssignmentId, dbNumberSequence);
                                //Commented for Snapshot Error -09 Mar 2021
                                // _timesheetService.SaveNumberSequence(dbNumberSequence);
                                _timesheetService.AddTimesheetHistory(timesheetId, VisitTimesheetConstants.VISIT_TIMESHEET_CREATED, timesheet.TimesheetInfo.ActionByUser);
                                if (isAwaitingApproval)
                                {
                                    _timesheetService.AddTimesheetHistory(timesheetId, VisitTimesheetConstants.VISIT_TIMESHEET_AWAITING_APPROVAL, timesheet.TimesheetInfo.ActionByUser);
                                }
                            }
                        }
                        else
                            return response;
                    }
                    emailDocument.TimesheetId = timesheetId;
                }
                else if (timesheet == null || timesheet?.TimesheetInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messages, timesheet, MessageType.InvalidPayLoad, timesheet }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheet);
            }
            finally
            {
                _timesheetRepository.AutoSave = true;
                //_timesheetRepository.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            return new Response().ToPopulate(responseType, null, null, validationMessages?.ToList(), emailDocument, exception);
        }


        public void SaveNumberSequence(int timesheetNumber, int moduleData, DbModel.NumberSequence dbNumberSequence)
        {
            Exception exception = null;
            try
            {
                _numberSequenceRepository.AutoSave = false;
                if (timesheetNumber == 1)
                {
                    var timesheetNumberSequence = new DbModel.NumberSequence()
                    {
                        LastSequenceNumber = 1,
                        ModuleId = 5,
                        ModuleData = moduleData,
                        ModuleRefId = 18
                    };
                    _numberSequenceRepository.Add(timesheetNumberSequence);
                }
                else
                {
                    dbNumberSequence.LastSequenceNumber = timesheetNumber;
                    _numberSequenceRepository.Update(dbNumberSequence, x => x.LastSequenceNumber);
                }

                _numberSequenceRepository.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), moduleData);
            }
            finally
            {
                _numberSequenceRepository.AutoSave = true;
            }
        }

        private Response ProcessTSCalendarData(IList<DomainTsModel.TechnicalSpecialistCalendar> filteredAddTSCalendarInfo,
            IList<DomainTsModel.TechnicalSpecialistCalendar> filteredModifyTSCalendarInfo,
            IList<DomainTsModel.TechnicalSpecialistCalendar> filteredDeleteTSCalendarInfo,
            long timesheetId,
            IList<DbModel.Company> dbCompanies,
            ref Response response)
        {
            if (filteredAddTSCalendarInfo != null && filteredAddTSCalendarInfo?.Count > 0)
            {
                filteredAddTSCalendarInfo.ToList().ForEach(filteredAddTSCalendar =>
                {
                    filteredAddTSCalendar.CalendarRefCode = timesheetId;
                    filteredAddTSCalendar.CreatedBy = filteredAddTSCalendar.ActionByUser;
                });
                response = _technicalSpecialistCalendarService.Save(filteredAddTSCalendarInfo, true, true, dbCompanies);
            }
            if ((response == null || response?.Code == MessageType.Success.ToId()) && filteredModifyTSCalendarInfo != null && filteredModifyTSCalendarInfo?.Count > 0)
            {
                filteredModifyTSCalendarInfo.ToList().ForEach(filteredModifyTSCalendar =>
                {
                    filteredModifyTSCalendar.CalendarRefCode = timesheetId;
                    filteredModifyTSCalendar.CreatedBy = filteredModifyTSCalendar.ActionByUser;
                });
                response = _technicalSpecialistCalendarService.Update(filteredModifyTSCalendarInfo, true, true, dbCompanies);
            }
            if ((response == null || response?.Code == MessageType.Success.ToId()) && filteredDeleteTSCalendarInfo != null && filteredDeleteTSCalendarInfo?.Count > 0)
            {
                TechnicalSpecialistCalendar technicalSpecialistCalendarModel = new TechnicalSpecialistCalendar
                {
                    CalendarRefCode = timesheetId,
                    IsActive = true,
                    CalendarType = CalendarType.TIMESHEET.ToString()
                };
                var timesheetCalendarDataResult = _technicalSpecialistCalendarService.Get(technicalSpecialistCalendarModel, false)?.Result?.Populate<IList<TechnicalSpecialistCalendar>>();
                if (timesheetCalendarDataResult?.Count != filteredDeleteTSCalendarInfo?.Count)
                {
                    filteredDeleteTSCalendarInfo.ToList().ForEach(filteredDeleteTSCalendar =>
                {
                    filteredDeleteTSCalendar.CalendarRefCode = timesheetId;
                    filteredDeleteTSCalendar.CreatedBy = filteredDeleteTSCalendar.ActionByUser;
                });
                    response = _technicalSpecialistCalendarService.Delete(filteredDeleteTSCalendarInfo, true, true);
                }
                else
                {
                    var validationMessages = new List<ValidationMessage>
                    {
                        { _messages, ModuleType.Calendar, MessageType.InvalidTimesheetCalendarRecord }
                    };
                    response = new Response().ToPopulate(ResponseType.Error, null, null, validationMessages, null, null, null);
                }
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
           IList<TechnicalSpecialistCalendar> timesheetCalendarList,
           DomainModel.TimesheetDetail timesheet = null)
        {
            if (timesheet != null && timesheet.TimesheetTechnicalSpecialists != null && timesheet.TimesheetTechnicalSpecialists.Count > 0)
            {
                IList<int> deletedTechSpecIds = timesheet.TimesheetTechnicalSpecialists.Where(techSpec => techSpec.RecordStatus == "D")?.Select(s => (int)s.Pin).ToList();
                if (deletedTechSpecIds != null && deletedTechSpecIds.Count > 0)
                {
                    var deletedTechSpecCalendarList = timesheetCalendarList?.Where(s => deletedTechSpecIds.Contains(s.TechnicalSpecialistId))?.ToList();
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

        private bool IsCalendarDatainOutsideRange(IList<DomainTsModel.TechnicalSpecialistCalendar> technicalSpecialistCalendarList, ref IList<ValidationMessage> validationMessages, IList<TechnicalSpecialistCalendar> calendarData = null, DomainModel.TimesheetDetail timesheet = null)
        {
            bool isCalendarDatainOutsideRange = false;
            if (timesheet != null && timesheet.TimesheetInfo != null && timesheet.TimesheetInfo?.TimesheetId != null)
            {
                if (technicalSpecialistCalendarList != null && technicalSpecialistCalendarList.Count > 0)
                {
                    var deleteCalendarDataList = technicalSpecialistCalendarList.Where(x => x.RecordStatus == "D").Select(s => s.Id).ToList();
                    calendarData = calendarData.Where(s => !deleteCalendarDataList.Contains(s.Id)).ToList();
                }
                isCalendarDatainOutsideRange = calendarData.Any(x => (Convert.ToDateTime(x.StartDateTime).Date < Convert.ToDateTime(timesheet.TimesheetInfo.TimesheetStartDate).Date || Convert.ToDateTime(x.StartDateTime).Date > Convert.ToDateTime(timesheet.TimesheetInfo?.TimesheetEndDate).Date) || (Convert.ToDateTime(x.EndDateTime).Date < Convert.ToDateTime(timesheet.TimesheetInfo?.TimesheetStartDate).Date || Convert.ToDateTime(x.EndDateTime).Date > Convert.ToDateTime(timesheet.TimesheetInfo?.TimesheetEndDate).Date));
            }
            if (isCalendarDatainOutsideRange)
            {
                validationMessages.Add(_messages, ModuleType.Calendar, MessageType.TimesheetCalendarDataInOutsideRange);
            }

            return isCalendarDatainOutsideRange;
        }

        public Response Modify(DomainModel.TimesheetDetail timesheet, bool IsAPIValidationRequired = false)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long timesheetId = 0;
            Response response = null;
            ResponseType responseType = ResponseType.Success;
            long? eventId = null;
            bool isConsumableLineItemsExists = false, isExpenseLineItemsExists = false, isTimeLineItemsExists = false, isTravelLineItemsExists = false, isTimesheetReferencesExists = false;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialistCalendar> dBTsCalendarInfo = null;
            IList<DbModel.Company> dbCompanies = null;
            bool isAwaitingApproval = false;
            EmailDocument emailDocument = new EmailDocument
            {
                IsDocumentUpload = false
            };
            IList<DbModel.SqlauditModule> dbModule = null;
            try
            {
                if (timesheet != null && timesheet.TimesheetInfo != null)
                {
                    response = MandatoryValidations(timesheet);
                    if (response.Code != ResponseType.Success.ToId() && validationMessages?.Count > 0)
                    {
                        return response;
                    }

                    DomainModel.DbTimesheet dbTimesheetData = null;
                    using (dbTimesheetData = new DomainModel.DbTimesheet())
                    {
                        if (timesheet != null && !string.IsNullOrEmpty(timesheet?.TimesheetInfo?.RecordStatus))
                            response = ValidateTimesheetInfo(timesheet, ref dbTimesheetData, ref validationMessages, ValidationType.Update,
                                                            ref dbTechnicalSpecialists, ref dBTsCalendarInfo, ref dbCompanies, response);

                        if (response.Code == ResponseType.Success.ToId() & validationMessages?.Count == 0)
                            GetMasterData(timesheet, ref dbTimesheetData, ref isConsumableLineItemsExists, ref isExpenseLineItemsExists, ref isTimeLineItemsExists,
                                         ref isTravelLineItemsExists, ref isTimesheetReferencesExists, ref validationMessages, response, ref dbModule);

                        if (response?.Code == ResponseType.Success.ToId() & validationMessages?.Count == 0)
                        {
                            if (timesheet.TimesheetInfo.TimesheetStatus == "E")
                            {
                                TechnicalSpecialistCalendar technicalSpecialistCalendar = new TechnicalSpecialistCalendar
                                {
                                    CalendarRefCode = (long)timesheet.TimesheetInfo.TimesheetId,
                                    CalendarType = CalendarType.TIMESHEET.ToString(),
                                    IsActive = true
                                };

                                dBTsCalendarInfo = _technicalSpecialistCalendarService.GetCalendar(technicalSpecialistCalendar);
                                Response calendarResponse = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
                                if (dBTsCalendarInfo != null && dBTsCalendarInfo.Count > 0)
                                    calendarResponse = _technicalSpecialistCalendarService.UpdateCalendar(dBTsCalendarInfo);
                            }
                            response = ValidateData(timesheet, ref dbTimesheetData, isConsumableLineItemsExists, isExpenseLineItemsExists, isTimeLineItemsExists,
                                                    isTravelLineItemsExists, isTimesheetReferencesExists, ref validationMessages, response);
                            if (response.Code == ResponseType.Success.ToId() & validationMessages?.Count == 0)
                            {
                                string timesheetStatus = dbTimesheetData.DbTimesheets?.FirstOrDefault().TimesheetStatus;
                                //To-Do: Will create helper method get TransactionScope instance based on requirement
                                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                        new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                                {
                                    _timesheetRepository.AutoSave = false;
                                    response = ProcessTimesheetInfo(
                                                                    new List<DomainModel.Timesheet> { timesheet.TimesheetInfo },
                                                                    ref dbTimesheetData,
                                                                    ref dbModule,
                                                                    ref eventId,
                                                                    true,
                                                                    ValidationType.Update);

                                    if ((response == null || response?.Code == MessageType.Success.ToId()) &&
                                        dbTimesheetData.DbTimesheets != null)
                                    {
                                        List<ModuleDocument> uploadTimesheetDocuments = new List<ModuleDocument>();
                                        timesheetId = Convert.ToInt64(dbTimesheetData.DbTimesheets?.FirstOrDefault()?.Id);
                                        response = ProcessTimesheetDetail(timesheet,
                                                                            ValidationType.Update,
                                                                            dbTimesheetData,
                                                                            dbModule,
                                                                            ref timesheetId,
                                                                            ref eventId,
                                                                            ref uploadTimesheetDocuments);

                                        if (response.Code == ResponseType.Success.ToId())
                                        {
                                            response = ProcessTSCalendarData(dbTimesheetData.filteredAddTSCalendarInfo,
                                                dbTimesheetData.filteredModifyTSCalendarInfo, dbTimesheetData.filteredDeleteTSCalendarInfo, timesheetId,
                                                dbCompanies, ref response);
                                        }

                                        if (response.Code == ResponseType.Success.ToId())
                                        {
                                            if (timesheet.TimesheetInfo.TimesheetStatus == "C" && timesheetStatus != "C")
                                            {
                                                isAwaitingApproval = true;
                                                bool isDocumentUpload = false;
                                                IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(timesheet.TimesheetInfo.TimesheetProjectNumber);
                                                if (projectNotification != null && projectNotification.Count > 0)
                                                {
                                                    isDocumentUpload = projectNotification.Where(x => x.IsSendCustomerDirectReportingNotification == true).Count() > 0;
                                                }

                                                if (isDocumentUpload)
                                                {
                                                    DomainModel.TimesheetEmailData timesheetEmailData = new DomainModel.TimesheetEmailData
                                                    {
                                                        TimesheetDetail = timesheet
                                                    };
                                                    var emailResponse = ProcessEmailNotifications(timesheetEmailData, EmailTemplate.EmailCustomerDirectReporting, ref validationMessages, ref eventId, dbModule);
                                                    if (emailResponse != null && emailResponse.Result != null)
                                                    {
                                                        EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                                                        if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                                                        {
                                                            if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                                                            emailDocumentUpload.DocumentUniqueName.ModuleCodeReference = Convert.ToString(timesheetId);
                                                            emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                                                            emailDocument.IsDocumentUpload = true;
                                                        }
                                                    }
                                                }
                                            }

                                            _timesheetRepository.AutoSave = false;
                                            _timesheetRepository.ForceSave();
                                            tranScope.Complete();
                                            if (isAwaitingApproval)
                                            {
                                                _timesheetService.AddTimesheetHistory(timesheetId, VisitTimesheetConstants.VISIT_TIMESHEET_AWAITING_APPROVAL, timesheet.TimesheetInfo.ModifiedBy);
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
                        else
                            return response;
                    }
                    emailDocument.TimesheetId = timesheetId;
                    emailDocument.EventId = eventId;
                }
                else if (timesheet == null || timesheet?.TimesheetInfo == null)
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messages, timesheet, MessageType.InvalidPayLoad, timesheet }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheet);
            }
            finally
            {
                _timesheetRepository.AutoSave = true;
                //_timesheetRepository.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            return new Response().ToPopulate(responseType, null, null, validationMessages?.ToList(), emailDocument, exception);
        }

        public Response Delete(DomainModel.TimesheetDetail timesheetDetails, bool IsAPIValidationRequired = false)
        {
            Exception exception = null;
            Response response = null;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            IList<DbModel.Timesheet> dbTimesheet = null;
            IList<DbModel.TechnicalSpecialistCalendar> tsCalendarData = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            Response calendarResponse = null;
            int count = 0;
            try
            {
                if (timesheetDetails != null && timesheetDetails.TimesheetInfo != null && timesheetDetails.TimesheetInfo.RecordStatus.IsRecordStatusDeleted() && timesheetDetails?.TimesheetInfo?.TimesheetId > 0)
                {
                    _timesheetService.IsValidTimesheetData(new List<long>() { (long)timesheetDetails.TimesheetInfo.TimesheetId }, ref dbTimesheet, ref validationMessages);
                    if (validationMessages?.Count == 0)
                    {
                        TechnicalSpecialistCalendar technicalSpecialistCalendar = new TechnicalSpecialistCalendar
                        {
                            CalendarRefCode = (long)timesheetDetails.TimesheetInfo.TimesheetId,
                            CalendarType = CalendarType.TIMESHEET.ToString(),
                            IsActive = true
                        };
                        tsCalendarData = _technicalSpecialistCalendarService.GetCalendar(technicalSpecialistCalendar);
                        using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                        {
                            if (tsCalendarData != null && tsCalendarData.Count > 0)
                                calendarResponse = _technicalSpecialistCalendarService.UpdateCalendar(tsCalendarData);

                            if (calendarResponse == null || calendarResponse.Code == ResponseType.Success.ToId())
                            {
                                count = _timesheetRepository.DeleteTimesheet((long)timesheetDetails.TimesheetInfo.TimesheetId);
                                if (count > 0)
                                {
                                    tranScope.Complete();
                                    response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
                                }
                            }
                            else
                                return calendarResponse;
                        }
                        if (count > 0)
                        {
                            long? eventId = null;
                            dbModule = _auditSearchService.GetAuditModule(new List<string>() { SqlAuditModuleType.Timesheet.ToString() });
                            _auditSearchService.AuditLog(timesheetDetails, ref eventId, timesheetDetails?.TimesheetInfo?.ActionByUser.ToString(), "{" + AuditSelectType.Id + ":" + timesheetDetails?.TimesheetInfo?.TimesheetId + "}${" +
                                                                                                                AuditSelectType.TimesheetDescription + ":" + timesheetDetails?.TimesheetInfo?.TimesheetDescription?.Trim() + "}${" +
                                                                                                                AuditSelectType.JobReferenceNumber + ":" + timesheetDetails?.TimesheetInfo?.TimesheetProjectNumber + "-" + timesheetDetails?.TimesheetInfo?.TimesheetAssignmentNumber + "-" + timesheetDetails?.TimesheetInfo?.TimesheetNumber + "}${" +
                                                                                                                AuditSelectType.ProjectAssignment + ":" + timesheetDetails?.TimesheetInfo?.TimesheetProjectNumber + "-" + timesheetDetails?.TimesheetInfo?.TimesheetAssignmentNumber + "}", SqlAuditActionType.D, SqlAuditModuleType.Timesheet, timesheetDetails?.TimesheetInfo, null, dbModule);
                        }
                    }
                    else
                        response = new Response(ResponseType.Validation.ToId(), null, null, validationMessages?.ToList(), null);
                }
                else
                {
                    var message = new List<ValidationMessage>
                        {
                            { _messages, timesheetDetails, MessageType.InvalidPayLoad, timesheetDetails }
                        };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetDetails);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, timesheetDetails?.TimesheetInfo?.TimesheetId, exception, null);
        }

        public Response ApproveTimesheet(DomainModel.TimesheetEmailData timesheetEmailData)
        {
            Exception exception = null;
            Response response = null;
            Response emailResponse = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = null;
            IList<DbModel.SqlauditModule> dbModule = null;
            try
            {
                ValidateTimesheetTechSecAvailability(timesheetEmailData, ref response);
                if (response.Code == ResponseType.Success.ToId())
                {
                    timesheetEmailData.TimesheetDetail.TimesheetInfo.IsAuditUpdate = ((timesheetEmailData.IsAuditUpdate == true && timesheetEmailData.IsProcessNotification == true)
                                || (timesheetEmailData.IsAuditUpdate == false && timesheetEmailData.IsProcessNotification == false) ? true : false);
                    response = Modify(timesheetEmailData.TimesheetDetail);
                    //validate the response and start email processing 
                    if (response.Code == ResponseType.Success.ToId() && response.Result != null && response.ValidationMessages?.Count == 0)
                    {
                        EmailDocument emailDocument = (EmailDocument)response.Result;
                        emailDocument.IsDocumentUpload = false;
                        eventId = emailDocument.EventId;
                        DomainModel.Timesheet timesheetInfo = timesheetEmailData.TimesheetDetail.TimesheetInfo;
                        bool isIntercompanyAssignment = timesheetInfo.TimesheetContractCompanyCode != timesheetInfo.TimesheetOperatingCompanyCode;
                        List<string> ApprovalTimesheetStatus = new List<string>() { "A", "O" };
                        if (ApprovalTimesheetStatus.Contains(timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetStatus))
                        {
                            EmailTemplate emailtemplate = EmailTemplate.TimesheetApproveClientRequirement;
                            bool isDocumentUpload = false;
                            IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetProjectNumber);
                            if (projectNotification != null && projectNotification.Count > 0)
                            {
                                isDocumentUpload = projectNotification.Where(x => x.IsSendCustomerReportingNotification == true).Count() > 0;
                            }
                            if (timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetStatus == "O")
                            {
                                isDocumentUpload = false;
                                List<string> loginNames = new List<string>();
                                string loggedInUser = (!string.IsNullOrEmpty(timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.ModifiedBy)
                                        ? timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.ModifiedBy : timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.ActionByUser);
                                if (!string.IsNullOrEmpty(loggedInUser))
                                {
                                    loginNames.Add(loggedInUser);
                                    List<string> userTypes = new List<string> { VisitTimesheetConstants.UserType_Coordinator,
                                                    VisitTimesheetConstants.UserType_MICoordinator, VisitTimesheetConstants.Technical_Specialist };
                                    IList<UserInfo> userInfos = _userService.GetByUserType(loginNames, userTypes, true)
                                                        .Result
                                                        .Populate<IList<UserInfo>>();
                                    if (userInfos != null && userInfos.Count > 0)
                                    {
                                        if (userInfos[0].CompanyCode != timesheetInfo.TimesheetOperatingCompanyCode)
                                        {
                                            isDocumentUpload = true;
                                        }
                                    }
                                }
                                emailtemplate = EmailTemplate.TimesheetApprove;
                            }
                            if (isDocumentUpload && timesheetEmailData.IsProcessNotification)
                            {
                                emailResponse = ProcessEmailNotifications(timesheetEmailData, emailtemplate, ref validationMessages, ref eventId, dbModule);
                                if (emailResponse != null && emailResponse.Result != null)
                                {
                                    long responseTimesheetId = emailDocument.TimesheetId;
                                    EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                                    if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                                    {
                                        emailDocument.TimesheetId = responseTimesheetId;
                                        if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                                        emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                                        emailDocument.IsDocumentUpload = true;
                                    }
                                }
                                if (emailResponse.ValidationMessages?.Count > 0)
                                    return new Response(ResponseType.Validation.ToId(), null, null, emailResponse.ValidationMessages?.ToList(), null);

                                if (validationMessages?.Count > 0)
                                    return new Response(ResponseType.Validation.ToId(), null, null, validationMessages?.ToList(), null);
                            }

                            long timesheetId = timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetId ?? 0;
                            if (timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetStatus == "A")
                            {
                                _timesheetService.AddTimesheetHistory(timesheetId, VisitTimesheetConstants.VISIT_TIMESHEET_APPROVED_BY_CH, timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.ModifiedBy);
                                if (timesheetEmailData.IsProcessNotification && projectNotification != null && projectNotification.Count > 0
                                    && projectNotification.Where(x => x.IsSendCustomerReportingNotification == true).Count() > 0)
                                {
                                    _timesheetService.AddTimesheetHistory(timesheetId, VisitTimesheetConstants.VISIT_TIMESHEET_CUSTOMER_REPORTING_EMAIL_SENT, timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.ModifiedBy);
                                }
                            }
                            else if (timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetStatus == "O" && timesheetEmailData.IsProcessNotification)
                                _timesheetService.AddTimesheetHistory(timesheetId, VisitTimesheetConstants.VISIT_TIMESHEET_APPROVED_BY_OPERATOR, timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.ModifiedBy);
                        }
                        response = new Response().ToPopulate(ResponseType.Success, null, null, null, emailDocument, exception);
                    }
                    return response;
                }
                return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetEmailData);
            }
            return response;
        }

        private void ValidateTimesheetTechSecAvailability(DomainModel.TimesheetEmailData timesheetEmailData, ref Response response)
        {
            if (timesheetEmailData.IsValidationRequired == true)
            {
                List<DomainModel.ResourceInfo> resourceInfo = _timesheetTechnicalSpecialistRepository.IsEpinAssociated(timesheetEmailData);
                if (resourceInfo != null)
                    response = new Response(ResponseType.Validation.ToId(), null, null, null, resourceInfo);
                else
                    response = new Response().ToPopulate(ResponseType.Success, null, null, null, null, null);
            }
            else
                response = new Response().ToPopulate(ResponseType.Success, null, null, null, null, null);
        }

        public Response RejectTimesheet(DomainModel.TimesheetEmailData timesheetEmailData)
        {
            Exception exception = null;
            timesheetEmailData.TimesheetDetail.TimesheetInfo.IsAuditUpdate = true;
            Response response = Modify(timesheetEmailData.TimesheetDetail);
            Response emailResponse = null;
            IList<ValidationMessage> validationMessages = null;
            EmailDocument emailDocument = new EmailDocument();
            IList<DbModel.SqlauditModule> dbModule = null;
            long? eventId = null;
            try
            {
                //validate the response and start Reject email processing
                if (response.Code == ResponseType.Success.ToId() && response.Result != null && response.ValidationMessages?.Count == 0)
                {
                    emailDocument = (EmailDocument)response.Result;
                    eventId = emailDocument?.EventId;
                    emailDocument.IsDocumentUpload = false;
                    DomainModel.Timesheet timesheetInfo = timesheetEmailData.TimesheetDetail.TimesheetInfo;
                    bool isIntercompanyAssignment = timesheetInfo.TimesheetContractCompanyCode != timesheetInfo.TimesheetOperatingCompanyCode;
                    emailResponse = ProcessEmailNotifications(timesheetEmailData, EmailTemplate.TimesheetReject, ref validationMessages, ref eventId, dbModule);
                    if (emailResponse != null && emailResponse.Result != null)
                    {
                        long responseTimesheetId = emailDocument.TimesheetId;
                        EmailDocumentUpload emailDocumentUpload = (EmailDocumentUpload)emailResponse.Result;
                        if (emailDocumentUpload != null && emailDocumentUpload.IsDocumentUpload)
                        {
                            emailDocument.TimesheetId = responseTimesheetId;
                            if (emailDocument.EmailDocumentUpload == null) emailDocument.EmailDocumentUpload = new List<EmailDocumentUpload>();
                            emailDocument.EmailDocumentUpload.Add(emailDocumentUpload);
                            emailDocument.IsDocumentUpload = true;
                        }
                    }
                    if (emailResponse.ValidationMessages?.Count > 0)
                        return new Response(ResponseType.Validation.ToId(), null, null, emailResponse.ValidationMessages?.ToList(), null);

                    if (validationMessages?.Count > 0)
                        return new Response(ResponseType.Validation.ToId(), null, null, validationMessages?.ToList(), null);

                    long timesheetId = timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetId ?? 0;
                    if (timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetStatus == "R")
                        _timesheetService.AddTimesheetHistory(timesheetId, VisitTimesheetConstants.VISIT_TIMESHEET_REJECTED_BY_CH, timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.ModifiedBy);
                    else if (timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetStatus == "J")
                        _timesheetService.AddTimesheetHistory(timesheetId, VisitTimesheetConstants.VISIT_TIMESHEET_REJECTED_BY_OPERRTOR, timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.ModifiedBy);
                }
                response = new Response().ToPopulate(ResponseType.Success, null, null, null, emailDocument, null);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetEmailData);
            }
            return response;
        }

        public Response ApprovalCustomerReportNotification(DomainModel.CustomerReportingNotification clientReportingNotification)
        {
            Exception exception = null;
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            IList<KeyValuePair<string, string>> emailContentPlaceholders = null;
            DomainModel.Timesheet timesheetInfo = null;
            try
            {
                timesheetInfo = clientReportingNotification.TimesheetInfo;
                string timesheetDate = string.Empty;
                string timesheetNumber = string.Empty;
                if (timesheetInfo.TimesheetStatus == "A")
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
                    timesheetDate = timesheetInfo.TimesheetStartDate?.ToString("dd-MM-yyyy");
                    string timesheetDesc = string.IsNullOrEmpty(timesheetInfo.TimesheetDescription) ? "" : timesheetInfo.TimesheetDescription;
                    string assignmentNumber = timesheetInfo.TimesheetAssignmentNumber?.ToString("00000");
                    string formattedCustomerName = !String.IsNullOrWhiteSpace(timesheetInfo.TimesheetCustomerName) && timesheetInfo.TimesheetCustomerName.Length >= 5
                                                                ? timesheetInfo.TimesheetCustomerName.Substring(0, 5)
                                                                : timesheetInfo.TimesheetCustomerName;
                    timesheetNumber = string.Format("({0} : {1})", assignmentNumber, timesheetDesc);
                    string projectNumber = string.Format("({0} : {1})", formattedCustomerName, timesheetInfo.TimesheetProjectNumber);
                    emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, timesheetInfo.TimesheetContractCompany),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER,assignmentNumber),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, timesheetInfo.TimesheetContractCoordinator),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, VisitTimesheetConstants.TIMESHEET),
                                    new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE,  timesheetDate)
                                    };
                    string token = DateTime.Now.ToString(VisitTimesheetConstants.TOKEN_DATE_FORMAT);
                    emailMessage = ProcessEmailMessage(ModuleType.Timesheet, EmailTemplate.TimesheetApproveClientRequirement, string.Empty,
                                                        EmailType.Notification, ModuleCodeType.TIME,
                                                        timesheetInfo.TimesheetId.ToString(),
                                                        clientReportingNotification.EmailSubject, emailContentPlaceholders,
                                                        clientReportingNotification.ToAddress, new List<EmailAddress>(), token, clientReportingNotification.Attachments, string.Empty);

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

        #endregion

        #region Private Methods

        private void GetMasterData(DomainModel.TimesheetDetail timesheet, ref DomainModel.DbTimesheet dbTimesheetDbData,
            ref bool isConsumableLineItemsExists, ref bool isExpenseLineItemsExists, ref bool isTimeLineItemsExists,
            ref bool isTravelLineItemsExists, ref bool isTimesheetReferencesExists, ref IList<ValidationMessage> validationMessages, Response response,
            ref IList<DbModel.SqlauditModule> dbModule)
        {
            GetTechnicalSpecialists(timesheet, ref dbTimesheetDbData, ref validationMessages, response);
            isTimesheetReferencesExists = timesheet.TimesheetReferences != null && timesheet.TimesheetReferences.Any(x => !string.IsNullOrEmpty(x.RecordStatus));
            isConsumableLineItemsExists = timesheet.TimesheetTechnicalSpecialistConsumables != null && timesheet.TimesheetTechnicalSpecialistConsumables.Any(x => !string.IsNullOrEmpty(x.RecordStatus));
            isExpenseLineItemsExists = timesheet.TimesheetTechnicalSpecialistExpenses != null && timesheet.TimesheetTechnicalSpecialistExpenses.Any(x => !string.IsNullOrEmpty(x.RecordStatus));
            isTimeLineItemsExists = timesheet.TimesheetTechnicalSpecialistTimes != null && timesheet.TimesheetTechnicalSpecialistTimes.Any(x => !string.IsNullOrEmpty(x.RecordStatus));
            isTravelLineItemsExists = timesheet.TimesheetTechnicalSpecialistTravels != null && timesheet.TimesheetTechnicalSpecialistTravels.Any(x => !string.IsNullOrEmpty(x.RecordStatus));

            if (isConsumableLineItemsExists || isExpenseLineItemsExists || isTimeLineItemsExists || isTravelLineItemsExists || isTimesheetReferencesExists)
                MasterData(ref dbTimesheetDbData);

            dbModule = _auditSearchService.GetAuditModule(new List<string>() {
                                                            SqlAuditModuleType.Timesheet.ToString(),
                                                            SqlAuditModuleType.TimesheetDocument.ToString(),
                                                            SqlAuditModuleType.TimesheetNote.ToString(),
                                                            SqlAuditModuleType.TimesheetReference.ToString(),
                                                            SqlAuditModuleType.TimesheetTechnicalSpecialist.ToString(),
                                                            SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemConsumable.ToString(),
                                                            SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemExpense.ToString(),
                                                            SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTime.ToString(),
                                                            SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTravel.ToString(),
                                                            SqlAuditModuleType.TimesheetInterCompanyDiscount.ToString(),
                                                            });
        }


        private Response ValidateData(DomainModel.TimesheetDetail timesheet, ref DomainModel.DbTimesheet dbTimesheetDbData, bool isConsumableLineItemsExists,
                                      bool isExpenseLineItemsExists, bool isTimeLineItemsExists, bool isTravelLineItemsExists, bool isTimesheetReferencesExists,
                                      ref IList<ValidationMessage> validationMessages,
                                      Response response)
        {
            if (timesheet != null)
            {
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (isTimesheetReferencesExists && validationMessages?.Count == 0)
                    response = ValidateTimesheetReferences(timesheet?.TimesheetReferences,
                                                          ref dbTimesheetDbData,
                                                          response);

                if (timesheet.TimesheetTechnicalSpecialists != null && timesheet.TimesheetTechnicalSpecialists.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                    && validationMessages?.Count == 0 && response.ValidationMessages?.Count == 0)
                    response = ValidateTimesheetTechSpec(timesheet.TimesheetTechnicalSpecialists,
                                                         ref dbTimesheetDbData,
                                                         response);
                if (isConsumableLineItemsExists && validationMessages?.Count == 0 && response.ValidationMessages?.Count == 0)
                    response = ValidateTimesheetTechSpecAccItemConsumables(timesheet.TimesheetTechnicalSpecialistConsumables,
                                                                         ref dbTimesheetDbData,
                                                                         response);
                if (isExpenseLineItemsExists && validationMessages?.Count == 0 && response.ValidationMessages?.Count == 0)
                    response = ValidateTimesheetTechSpecAccItemExpense(timesheet.TimesheetTechnicalSpecialistExpenses,
                                                                     ref dbTimesheetDbData,
                                                                     response);
                if (isTimeLineItemsExists && validationMessages?.Count == 0 && response.ValidationMessages?.Count == 0)
                    response = ValidateTimesheetTechSpecAccItemTime(timesheet.TimesheetTechnicalSpecialistTimes,
                                                                     ref dbTimesheetDbData,
                                                                     response);
                if (isTravelLineItemsExists && validationMessages?.Count == 0 && response.ValidationMessages?.Count == 0)
                    response = ValidateTimesheetTechSpecAccItemTravel(timesheet.TimesheetTechnicalSpecialistTravels,
                                                                     ref dbTimesheetDbData,
                                                                     response);


                if (timesheet.TimesheetNotes != null && timesheet.TimesheetNotes.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                    && validationMessages?.Count == 0 && response.ValidationMessages?.Count == 0)
                    response = ValidateTimesheetNotes(timesheet.TimesheetNotes,
                                                      ref dbTimesheetDbData,
                                                      response);

                if (response?.ValidationMessages != null)
                    validationMessages = response.ValidationMessages;

            }

            return response;
        }

        private void AssignTimesheetId(long timesheetId, DomainModel.TimesheetDetail timesheet)
        {
            timesheet?.TimesheetDocuments?.ToList().ForEach(x => { x.ModuleRefCode = timesheetId.ToString(); });
            timesheet?.TimesheetNotes?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });
            timesheet?.TimesheetReferences?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });
            timesheet?.TimesheetTechnicalSpecialistConsumables?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });
            timesheet?.TimesheetTechnicalSpecialistExpenses?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });
            timesheet?.TimesheetTechnicalSpecialistTimes?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });
            timesheet?.TimesheetTechnicalSpecialistTravels?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });
            timesheet?.TimesheetTechnicalSpecialists?.ToList().ForEach(x => { x.TimesheetId = timesheetId; });
        }

        /*This section is used to load child timesheet data at the time of Update*/
        private void LoadTimesheetChildData(DomainModel.TimesheetDetail timesheet,
                                            ref DomainModel.DbTimesheet dbTimesheetDBData,
                                            ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var timesheetId = timesheet.TimesheetInfo.TimesheetId > 0 ? (long)timesheet.TimesheetInfo.TimesheetId : 0;
            if (timesheetId > 0)
                _timesheetService.IsValidTimesheet(new List<long>() { timesheetId },
                                                   ref dbTimesheetDBData.DbTimesheets,
                                                   ref validationMessages,
                                                   "Assignment",
                                                   "Assignment.Project",
                                                   "Assignment.Project.Contract",
                                                   "TimesheetTechnicalSpecialist",
                                                   "TimesheetTechnicalSpecialist.TechnicalSpecialist",
                                                   "TimesheetTechnicalSpecialist.TimesheetTechnicalSpecialistAccountItemTime",
                                                   "TimesheetTechnicalSpecialist.TimesheetTechnicalSpecialistAccountItemTravel",
                                                   "TimesheetTechnicalSpecialist.TimesheetTechnicalSpecialistAccountItemExpense",
                                                   "TimesheetTechnicalSpecialist.TimesheetTechnicalSpecialistAccountItemConsumable",
                                                   "TimesheetReference",
                                                   "TimesheetNote");

            if (dbTimesheetDBData?.DbTimesheets?.Count > 0 && validationMessages?.Count == 0)
            {
                dbTimesheetDBData.DbAssignments = dbTimesheetDBData.DbTimesheets.ToList().Select(x => x.Assignment).ToList();
                dbTimesheetDBData.DbProjects = dbTimesheetDBData.DbTimesheets.ToList().Select(x => x.Assignment.Project).ToList();
                dbTimesheetDBData.DbContracts = dbTimesheetDBData.DbTimesheets.ToList().Select(x => x.Assignment.Project.Contract).ToList();
                dbTimesheetDBData.DbTimesheetReference = dbTimesheetDBData.DbTimesheets.ToList().SelectMany(x => x.TimesheetReference).ToList();
                dbTimesheetDBData.DbTimesheetTechSpecialists = dbTimesheetDBData.DbTimesheets.ToList().SelectMany(x => x.TimesheetTechnicalSpecialist).ToList();
                dbTimesheetDBData.DbTimesheetTechSpecConsumables = dbTimesheetDBData.DbTimesheetTechSpecialists?.ToList().SelectMany(x => x.TimesheetTechnicalSpecialistAccountItemConsumable).ToList();
                dbTimesheetDBData.DbTimesheetTechSpecExpenses = dbTimesheetDBData.DbTimesheetTechSpecialists?.ToList().SelectMany(x => x.TimesheetTechnicalSpecialistAccountItemExpense).ToList();
                dbTimesheetDBData.DbTimesheetTechSpecTimes = dbTimesheetDBData.DbTimesheetTechSpecialists?.ToList().SelectMany(x => x.TimesheetTechnicalSpecialistAccountItemTime).ToList();
                dbTimesheetDBData.DbTimesheetTechSpecTravels = dbTimesheetDBData.DbTimesheetTechSpecialists?.ToList().SelectMany(x => x.TimesheetTechnicalSpecialistAccountItemTravel).ToList();
                dbTimesheetDBData.DbTimesheetNotes = dbTimesheetDBData.DbTimesheets.ToList().SelectMany(x => x.TimesheetNote).ToList();
            }
        }

        /*This section is used to load child transaction data at the time of Add*/
        private void LoadRelatedData(DomainModel.TimesheetDetail timesheet,
                                     ref DomainModel.DbTimesheet dbTimesheetDBData,
                                     ref IList<ValidationMessage> validationMessages,
                                     ValidationType validationType)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var assignmentId = timesheet.TimesheetInfo.TimesheetAssignmentId > 0 ? timesheet.TimesheetInfo.TimesheetAssignmentId : 0;
            if (assignmentId > 0 && validationType == ValidationType.Add)
            {
                string[] includes = {"Timesheet",
                                     "Project",
                                     "Project.Contract"
                                    };

                _assignmentService.IsValidAssignment(new List<int>() { assignmentId },
                                             ref dbTimesheetDBData.DbAssignments,
                                             ref validationMessages,
                                             includes
                                             );

                if (dbTimesheetDBData?.DbAssignments != null && validationMessages?.Count == 0)
                {
                    dbTimesheetDBData.DbProjects = dbTimesheetDBData.DbAssignments.ToList().Select(x => x.Project).ToList();
                    dbTimesheetDBData.DbContracts = dbTimesheetDBData.DbAssignments.ToList().Select(x => x.Project.Contract).ToList();
                }
            }
        }

        /*This section is used to load master Data for Currency & Expense*/
        private void MasterData(ref DomainModel.DbTimesheet dbTimesheetDBData)
        {
            dbTimesheetDBData.DbData = _masterService.Get(new List<MasterType>() { MasterType.ExpenseType,
                                                                                 MasterType.Currency,
                                                                                 MasterType.AssignmentReferenceType});
        }

        /*This section is used to get the needed tech Spec*/
        private Response GetTechnicalSpecialists(DomainModel.TimesheetDetail timesheet,
                                                 ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                 ref IList<ValidationMessage> validationMessages,
                                                 Response response)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (timesheet?.TimesheetTechnicalSpecialists != null)
            {
                List<string> ePins = timesheet?.TimesheetTechnicalSpecialists.ToList().Where(x => x.Pin > 0).Select(x => x.Pin.ToString()).ToList();
                _techSpecService.IsRecordExistInDb(ePins, ref dbTimesheetDBData.DbTechnicalSpecialists, ref validationMessages);
            }
            return response;
        }

        /*This section is used to validate Timesheet Info*/
        private Response ValidateTimesheetInfo(DomainModel.TimesheetDetail timesheet,
                                               ref DomainModel.DbTimesheet dbTimesheetDBData,
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
                LoadRelatedData(timesheet,
                                ref dbTimesheetDBData,
                                ref validationMessages,
                                validationType);
            if (validationType == ValidationType.Update)
                LoadTimesheetChildData(timesheet,
                                ref dbTimesheetDBData,
                                ref validationMessages);

            IList<DomainModel.Timesheet> timesheetInfo = new List<DomainModel.Timesheet>
            {
                timesheet.TimesheetInfo
            };
            response = _timesheetService.IsRecordValidForProcess(timesheetInfo,
                                                                ValidationType.Add,
                                                                ref dbTimesheetDBData.DbTimesheets,
                                                                ref dbTimesheetDBData.DbAssignments);

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result && validationMessages?.Count == 0)
                response = _timesheetService.IsRecordValidForProcess(timesheetInfo,
                                                                     ValidationType.Update,
                                                                     ref dbTimesheetDBData.DbTimesheets,
                                                                     ref dbTimesheetDBData.DbAssignments);

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result && validationMessages?.Count == 0)
                response = _timesheetService.IsRecordValidForProcess(timesheetInfo,
                                                                    ValidationType.Delete,
                                                                    ref dbTimesheetDBData.DbTimesheets,
                                                                    ref dbTimesheetDBData.DbAssignments);
            if (response.Code == ResponseType.Success.ToId() && validationMessages?.Count == 0)
            {
                IList<TechnicalSpecialistCalendar> timesheetCalendarList = new List<TechnicalSpecialistCalendar>();
                List<string> timesheetStatus = new List<string>() { "C", "N", "Q", "T", "U", "W", "E" };
                if(timesheet.TimesheetInfo?.TimesheetContractCompanyCode == timesheet.TimesheetInfo?.TimesheetOperatingCompanyCode) 
                    timesheetStatus.AddRange(new string[] { "A", "J", "R" });

                if (timesheet.TimesheetInfo?.TimesheetId != null && timesheetStatus.Contains(timesheet.TimesheetInfo?.TimesheetStatus))
                {
                    TechnicalSpecialistCalendar technicalSpecialistCalendar = new TechnicalSpecialistCalendar
                    {
                        CalendarRefCode = (long)timesheet.TimesheetInfo?.TimesheetId,
                        IsActive = true,
                        CalendarType = CalendarType.TIMESHEET.ToString()
                    };
                    timesheetCalendarList = _technicalSpecialistCalendarService.SearchGet(technicalSpecialistCalendar, false).Result?.Populate<IList<TechnicalSpecialistCalendar>>();
                    if (IsCalendarDatainOutsideRange(timesheet.TechnicalSpecialistCalendarList, ref validationMessages, timesheetCalendarList, timesheet))
                    {
                        return new Response().ToPopulate(ResponseType.Validation, null, null, validationMessages?.ToList(), timesheet.TechnicalSpecialistCalendarList, null);
                    }
                    if (timesheet.TechnicalSpecialistCalendarList == null || timesheet.TechnicalSpecialistCalendarList.Count == 0)
                    {
                        if ((timesheetCalendarList == null || timesheetCalendarList.Count == 0) && timesheet.TimesheetInfo?.TimesheetStatus != "E")
                        {
                            var calendarValidationMessages = new List<ValidationMessage>
                            {
                                { _messages, ModuleType.Calendar, MessageType.InvalidTimesheetCalendarRecord }
                            };
                            response = new Response().ToPopulate(ResponseType.Error, null, null, calendarValidationMessages, null, null, null);
                        }
                    }
                }
                if ((timesheet.TechnicalSpecialistCalendarList != null || (timesheet.TimesheetTechnicalSpecialists != null && timesheet.TimesheetTechnicalSpecialists.Count > 0)) && response.Code == ResponseType.Success.ToId() && (bool)response.Result && validationMessages?.Count == 0)
                {
                    response = IsValidCalendarData(timesheet.TechnicalSpecialistCalendarList, ref dbTimesheetDBData.filteredAddTSCalendarInfo, ref dbTimesheetDBData.filteredModifyTSCalendarInfo, ref dbTimesheetDBData.filteredDeleteTSCalendarInfo, ref dbTechnicalSpecialists, ref validationMessages, ref dBTsCalendarInfo, ref dbCompanies, timesheetCalendarList, timesheet);
                }
            }

            return response;

        }

        /*This section is used to validate Timesheet Reference*/
        private Response ValidateTimesheetReferences(IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                                     ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                     Response response)
        {
            IList<DbModel.Data> dbReference = dbTimesheetDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.AssignmentReferenceType)?.ToList();
            response = _timesheetReferenceService.IsRecordValidForProcess(timesheetReferences,
                                                                              ValidationType.Add,
                                                                              ref dbTimesheetDBData.DbTimesheetReference,
                                                                              ref dbTimesheetDBData.DbTimesheets,
                                                                              ref dbReference
                                                                              );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _timesheetReferenceService.IsRecordValidForProcess(timesheetReferences,
                                                                              ValidationType.Update,
                                                                              ref dbTimesheetDBData.DbTimesheetReference,
                                                                              ref dbTimesheetDBData.DbTimesheets,
                                                                              ref dbReference
                                                                              );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _timesheetReferenceService.IsRecordValidForProcess(timesheetReferences,
                                                                              ValidationType.Delete,
                                                                              ref dbTimesheetDBData.DbTimesheetReference,
                                                                              ref dbTimesheetDBData.DbTimesheets,
                                                                              ref dbReference
                                                                              );
            return response;

        }

        /*This section is used to validate Timesheet Tech Spec*/
        private Response ValidateTimesheetTechSpec(IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                                   ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                   Response response)
        {
            response = _timesheetTechSpecService.IsRecordValidForProcess(timesheetTechnicalSpecialists,
                                                                         ValidationType.Add,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecialists,
                                                                         ref dbTimesheetDBData.DbTechnicalSpecialists
                                                                         );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _timesheetTechSpecService.IsRecordValidForProcess(timesheetTechnicalSpecialists,
                                                                              ValidationType.Update,
                                                                              ref dbTimesheetDBData.DbTimesheets,
                                                                              ref dbTimesheetDBData.DbTimesheetTechSpecialists,
                                                                              ref dbTimesheetDBData.DbTechnicalSpecialists
                                                                             );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _timesheetTechSpecService.IsRecordValidForProcess(timesheetTechnicalSpecialists,
                                                                              ValidationType.Delete,
                                                                              ref dbTimesheetDBData.DbTimesheets,
                                                                              ref dbTimesheetDBData.DbTimesheetTechSpecialists,
                                                                              ref dbTimesheetDBData.DbTechnicalSpecialists
                                                                             );
            return response;

        }

        /*This section is used to validate Timesheet Tech Spec Account Item Time*/
        private Response ValidateTimesheetTechSpecAccItemTime(IList<DomainModel.TimesheetSpecialistAccountItemTime> timesheetTechSpecAccountItemTimes,
                                                              ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                              Response response)
        {
            IList<DbModel.Data> dbExpenses = dbTimesheetDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType || x.MasterDataTypeId == (int)MasterType.Currency)?.ToList();
            response = _timesheetTechSpecAccountItemTimeService.IsRecordValidForProcess(timesheetTechSpecAccountItemTimes,
                                                                         ValidationType.Add,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecTimes,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _timesheetTechSpecAccountItemTimeService.IsRecordValidForProcess(timesheetTechSpecAccountItemTimes,
                                                                              ValidationType.Update,
                                                                              ref dbTimesheetDBData.DbTimesheetTechSpecTimes,
                                                                              ref dbTimesheetDBData.DbTimesheets,
                                                                              ref dbTimesheetDBData.DbAssignments,
                                                                              ref dbTimesheetDBData.DbProjects,
                                                                              ref dbTimesheetDBData.DbContracts,
                                                                              ref dbExpenses
                                                                             );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _timesheetTechSpecAccountItemTimeService.IsRecordValidForProcess(timesheetTechSpecAccountItemTimes,
                                                                                            ValidationType.Delete,
                                                                                            ref dbTimesheetDBData.DbTimesheetTechSpecTimes,
                                                                                            ref dbTimesheetDBData.DbTimesheets,
                                                                                            ref dbTimesheetDBData.DbAssignments,
                                                                                            ref dbTimesheetDBData.DbProjects,
                                                                                            ref dbTimesheetDBData.DbContracts,
                                                                                            ref dbExpenses
                                                                                            );
            return response;

        }
        /*This section is used to validate Timesheet Tech Spec Account Item Travel*/
        private Response ValidateTimesheetTechSpecAccItemTravel(IList<DomainModel.TimesheetSpecialistAccountItemTravel> timesheetTechSpecAccountItemTravels,
                                                                ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                                Response response)
        {
            IList<DbModel.Data> dbExpenses = dbTimesheetDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType || x.MasterDataTypeId == (int)MasterType.Currency)?.ToList();
            response = _timesheetTechSpecAccountItemTravelService.IsRecordValidForProcess(timesheetTechSpecAccountItemTravels,
                                                                         ValidationType.Add,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecTravels,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _timesheetTechSpecAccountItemTravelService.IsRecordValidForProcess(timesheetTechSpecAccountItemTravels,
                                                                         ValidationType.Update,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecTravels,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _timesheetTechSpecAccountItemTravelService.IsRecordValidForProcess(timesheetTechSpecAccountItemTravels,
                                                                         ValidationType.Delete,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecTravels,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            return response;

        }

        /*This section is used to validate Timesheet Tech Spec Account Item Expense*/
        private Response ValidateTimesheetTechSpecAccItemExpense(IList<DomainModel.TimesheetSpecialistAccountItemExpense> timesheetTechSpecAccountItemExpenses,
                                                                ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                                Response response)
        {
            IList<DbModel.Data> dbExpenses = dbTimesheetDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType || x.MasterDataTypeId == (int)MasterType.Currency)?.ToList();
            response = _timesheetTechSpecAccountItemExpenseService.IsRecordValidForProcess(timesheetTechSpecAccountItemExpenses,
                                                                         ValidationType.Add,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecExpenses,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _timesheetTechSpecAccountItemExpenseService.IsRecordValidForProcess(timesheetTechSpecAccountItemExpenses,
                                                                         ValidationType.Update,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecExpenses,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _timesheetTechSpecAccountItemExpenseService.IsRecordValidForProcess(timesheetTechSpecAccountItemExpenses,
                                                                         ValidationType.Delete,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecExpenses,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            return response;

        }

        /*This section is used to validate Timesheet Tech Spec Account Item Consumables & Equipment*/
        private Response ValidateTimesheetTechSpecAccItemConsumables(IList<DomainModel.TimesheetSpecialistAccountItemConsumable> timesheetTechSpecAccountItemConsumables,
                                                                ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                                Response response)
        {
            IList<DbModel.Data> dbExpenses = dbTimesheetDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType || x.MasterDataTypeId == (int)MasterType.Currency)?.ToList();
            response = _timesheetTechSpecAccountItemConsumableService.IsRecordValidForProcess(timesheetTechSpecAccountItemConsumables,
                                                                         ValidationType.Add,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecConsumables,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _timesheetTechSpecAccountItemConsumableService.IsRecordValidForProcess(timesheetTechSpecAccountItemConsumables,
                                                                         ValidationType.Update,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecConsumables,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _timesheetTechSpecAccountItemConsumableService.IsRecordValidForProcess(timesheetTechSpecAccountItemConsumables,
                                                                         ValidationType.Delete,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecConsumables,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses
                                                                         );
            return response;

        }


        /*This section is used to validate Timesheet Note*/
        private Response ValidateTimesheetNotes(IList<DomainModel.TimesheetNote> timesheetNote,
                                                 ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                 Response response)
        {
            response = _timesheetNoteService.IsRecordValidForProcess(timesheetNote, ValidationType.Add,
                                                                      ref dbTimesheetDBData.DbTimesheetNotes,
                                                                      ref dbTimesheetDBData.DbTimesheets);

            if (response.Code == ResponseType.Success.ToId() && (bool)response.Result == true)
                response = _timesheetNoteService.IsRecordValidForProcess(timesheetNote, ValidationType.Delete,
                                                                        ref dbTimesheetDBData.DbTimesheetNotes,
                                                                        ref dbTimesheetDBData.DbTimesheets);

            return response;
        }

        /*This section is called to process Timesheet Info*/
        private Response ProcessTimesheet(DomainModel.TimesheetDetail timesheet,
                                           ValidationType validationType,
                                           DomainModel.DbTimesheet dbTimesheetData,
                                           ref IList<DbModel.SqlauditModule> dbModule,
                                           ref long timesheetId,
                                           ref long? eventId)
        {
            bool commitChanges = true;
            Response response = null;
            Exception exception = null;

            try
            {
                if (timesheet != null)
                {
                    _timesheetRepository.AutoSave = false;
                    timesheetId = timesheet?.TimesheetInfo?.TimesheetId == null ? 0 : (int)timesheet?.TimesheetInfo?.TimesheetId;
                    response = this.ProcessTimesheetInfo(new List<DomainModel.Timesheet> { timesheet.TimesheetInfo },
                                                         ref dbTimesheetData,
                                                         ref dbModule,
                                                         ref eventId,
                                                         commitChanges,
                                                         validationType);
                    if (response.Code == MessageType.Success.ToId())
                        timesheetId = dbTimesheetData.DbTimesheets.ToList().FirstOrDefault().Id;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheet);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, timesheet, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessTimesheetInfo(IList<DomainModel.Timesheet> timesheet,
                                               ref DomainModel.DbTimesheet dbTimesheetData,
                                               ref IList<DbModel.SqlauditModule> dbModule,
                                               ref long? eventId,
                                               bool commitChanges,
                                               ValidationType validationType)
        {
            Exception exception = null;
            try
            {
                if (timesheet != null)
                {
                    if (validationType == ValidationType.Delete)
                        return this._timesheetService.Delete(timesheet,
                                                             dbModule,
                                                             ref eventId,
                                                             commitChanges,
                                                             false);

                    else if (validationType == ValidationType.Add)
                        return this._timesheetService.Add(timesheet,
                                                          ref dbTimesheetData.DbTimesheets,
                                                          ref dbTimesheetData.DbAssignments,
                                                          dbModule,
                                                          ref eventId,
                                                          commitChanges,
                                                          false);

                    else if (validationType == ValidationType.Update)
                        return this._timesheetService.Modify(timesheet,
                                                             ref dbTimesheetData.DbTimesheets,
                                                             ref dbTimesheetData.DbAssignments,
                                                             dbModule,
                                                             ref eventId,
                                                             commitChanges,
                                                             false);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheet);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessTimesheetReference(long timesheetId,
                                                 IList<DomainModel.TimesheetReferenceType> timesheetReferences,
                                                 ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                 IList<DbModel.SqlauditModule> dbModule,
                                                 bool commitChanges,
                                                 ValidationType validationType)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (timesheetReferences != null)
                {
                    IList<DbModel.Data> dbReference = dbTimesheetDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.AssignmentReferenceType)?.ToList();
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && dbTimesheetDBData.DbTimesheetReference != null)
                        response = this._timesheetReferenceService.Modify(timesheetReferences,
                                                                       ref dbTimesheetDBData.DbTimesheetReference,
                                                                       ref dbTimesheetDBData.DbTimesheets,
                                                                       ref dbReference,
                                                                       dbModule,
                                                                       commitChanges,
                                                                       false,
                                                                       timesheetId);
                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && dbTimesheetDBData.DbTimesheetReference != null)
                    {
                        IList<DbModel.TimesheetReference> dbTimesheetReference = dbTimesheetDBData.DbTimesheetReference
                                                                                   .Where(x => timesheetReferences.ToList()
                                                                                   .Any(x1 => x1.TimesheetReferenceId == x.Id &&
                                                                                              x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this._timesheetReferenceService.Delete(timesheetReferences,
                                                                        ref dbTimesheetReference,
                                                                        ref dbTimesheetDBData.DbTimesheets,
                                                                        ref dbReference,
                                                                        dbModule,
                                                                        commitChanges,
                                                                        false,
                                                                        timesheetId);
                    }
                    if (ValidationType.Delete != validationType)
                        response = this._timesheetReferenceService.Add(timesheetReferences,
                                                                    ref dbTimesheetDBData.DbTimesheetReference,
                                                                    ref dbTimesheetDBData.DbTimesheets,
                                                                    ref dbReference,
                                                                    dbModule,
                                                                    commitChanges,
                                                                    false,
                                                                    timesheetId);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetReferences);
            }

            return response;
        }

        private Response ProcessTimesheetDetail(DomainModel.TimesheetDetail timesheet,
                                              ValidationType validationType,
                                              DomainModel.DbTimesheet dbTimesheetDbData,
                                              IList<DbModel.SqlauditModule> dbModule,
                                              ref long timesheetId,
                                              ref long? eventId,
                                              ref List<ModuleDocument> uploadTimesheetDocuments)
        {
            bool commitChanges = true;
            Response response = null;
            Exception exception = null;
            try
            {
                if (timesheet != null)
                {
                    AppendEvent(timesheet, eventId);
                    if (timesheet.TimesheetReferences != null && timesheet.TimesheetReferences.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTimesheetReference(timesheetId,
                                                         timesheet.TimesheetReferences,
                                                         ref dbTimesheetDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType);
                    }
                    DomainModel.DbTimesheet dbTimesheetDBData = dbTimesheetDbData;
                    if (timesheet.TimesheetTechnicalSpecialists != null && timesheet.TimesheetTechnicalSpecialists.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTimesheetTechnicalSpecialist(timesheetId,
                                                         timesheet.TimesheetTechnicalSpecialists,
                                                         ref dbTimesheetDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType,
                                                         false,
                                                         dbTimesheetDBData);
                    }
                    if (timesheet.TimesheetTechnicalSpecialistConsumables != null
                        && timesheet.TimesheetTechnicalSpecialistConsumables.Any(x => !string.IsNullOrEmpty(x.RecordStatus)
                        && !string.IsNullOrEmpty(x.Pin)) && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        if (dbTimesheetDbData.DbTimesheetTechSpecialists != null)
                            dbTimesheetDbData.DbTimesheetTechSpecialists.ToList().ForEach(x =>
                            {
                                timesheet.TimesheetTechnicalSpecialistConsumables.ToList().Where(x1 => Convert.ToInt32(x1.Pin) == x.TechnicalSpecialist.Pin).ToList()
                                         .ForEach(x2 => { x2.TimesheetTechnicalSpecialistId = x.Id; });
                            });


                        response = ProcessTimehseetTechSpecAccItemConsumables(timesheetId,
                                                         timesheet.TimesheetTechnicalSpecialistConsumables,
                                                         ref dbTimesheetDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType);
                    }
                    if (timesheet.TimesheetTechnicalSpecialistExpenses != null
                        && timesheet.TimesheetTechnicalSpecialistExpenses.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {

                        if (dbTimesheetDbData.DbTimesheetTechSpecialists != null)
                            dbTimesheetDbData.DbTimesheetTechSpecialists.ToList().ForEach(x =>
                            {
                                timesheet.TimesheetTechnicalSpecialistExpenses.ToList().Where(x1 => Convert.ToInt32(x1.Pin) == x.TechnicalSpecialist.Pin).ToList()
                                         .ForEach(x2 => { x2.TimesheetTechnicalSpecialistId = x.Id; });
                            });

                        timesheet.TimesheetTechnicalSpecialistExpenses.ToList()
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
                                    Response exchangeRateResponse = _timesheetService.GetExpenseLineItemChargeExchangeRates(exchangeRates, timesheet.TimesheetInfo.TimesheetContractNumber);
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
                                    Response exchangeRateResponse = _timesheetService.GetExpenseLineItemChargeExchangeRates(exchangeRates, timesheet.TimesheetInfo.TimesheetContractNumber);
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
                        response = ProcessTimesheetTechSpecAccItemExpense(timesheetId,
                                                         timesheet.TimesheetTechnicalSpecialistExpenses,
                                                         ref dbTimesheetDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType);
                    }
                    if (timesheet.TimesheetTechnicalSpecialistTimes != null
                        && timesheet.TimesheetTechnicalSpecialistTimes.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        if (dbTimesheetDbData.DbTimesheetTechSpecialists != null)
                            dbTimesheetDbData.DbTimesheetTechSpecialists.ToList().ForEach(x =>
                            {
                                timesheet.TimesheetTechnicalSpecialistTimes.ToList().Where(x1 => Convert.ToInt32(x1.Pin) == x.TechnicalSpecialist.Pin).ToList()
                                         .ForEach(x2 => { x2.TimesheetTechnicalSpecialistId = x.Id; });
                            });

                        response = ProcessTimesheetTechSpecAccItemTime(timesheetId,
                                                         timesheet.TimesheetTechnicalSpecialistTimes,
                                                         ref dbTimesheetDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType);
                    }
                    if (timesheet.TimesheetTechnicalSpecialistTravels != null
                        && timesheet.TimesheetTechnicalSpecialistTravels.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        if (dbTimesheetDbData.DbTimesheetTechSpecialists != null)
                            dbTimesheetDbData.DbTimesheetTechSpecialists.ToList().ForEach(x =>
                            {
                                timesheet.TimesheetTechnicalSpecialistTravels.ToList().Where(x1 => Convert.ToInt32(x1.Pin) == x.TechnicalSpecialist.Pin).ToList()
                                         .ForEach(x2 => { x2.TimesheetTechnicalSpecialistId = x.Id; });
                            });

                        response = ProcessTimesheetTechSpecAccItemTravel(timesheetId,
                                                         timesheet.TimesheetTechnicalSpecialistTravels,
                                                         ref dbTimesheetDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType);
                    }
                    if (timesheet.TimesheetTechnicalSpecialists != null && timesheet.TimesheetTechnicalSpecialists.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = ProcessTimesheetTechnicalSpecialist(timesheetId,
                                                         timesheet.TimesheetTechnicalSpecialists,
                                                         ref dbTimesheetDbData,
                                                         dbModule,
                                                         commitChanges,
                                                         validationType,
                                                         true,
                                                         dbTimesheetDBData);
                    }
                    if (timesheet.TimesheetNotes != null && timesheet.TimesheetNotes.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = this.ProcessTimesheetNote(timesheetId,
                                                             timesheet.TimesheetNotes,
                                                             ref dbTimesheetDbData,
                                                             dbModule,
                                                             commitChanges,
                                                             validationType);
                    }
                    if (timesheet.TimesheetDocuments != null && timesheet.TimesheetDocuments.Any(x => !string.IsNullOrEmpty(x.RecordStatus))
                        && (response == null || response?.Code == MessageType.Success.ToId()))
                    {
                        response = this.ProcessTimehseetDocument(timesheet,
                                                                 dbModule,
                                                                 commitChanges,
                                                                 validationType,
                                                                 timesheet,
                                                                 validationType.ToAuditActionType(),
                                                                 ref eventId,
                                                                 ref uploadTimesheetDocuments);
                    }
                    if ((response == null || response?.Code == MessageType.Success.ToId()) && ValidationType.Add == validationType)
                    {
                        string actionByUser = (!string.IsNullOrEmpty(timesheet.TimesheetInfo?.ModifiedBy)
                                        ? timesheet.TimesheetInfo?.ModifiedBy : timesheet.TimesheetInfo?.ActionByUser);
                        response = this.ProcessTimesheetInterCompany(timesheet.TimesheetInfo.TimesheetAssignmentId, timesheetId, commitChanges, actionByUser, eventId);
                    }
                    return response ?? new Response().ToPopulate(ResponseType.Success, null, null, null, null, null);

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheet);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, timesheetId, exception);
        }


        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessTimesheetTechnicalSpecialist(long timesheetId,
                                                  IList<DomainModel.TimesheetTechnicalSpecialist> timesheetTechnicalSpecialists,
                                                  ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                  IList<DbModel.SqlauditModule> dbModule,
                                                  bool commitChanges,
                                                  ValidationType validationType,
                                                  bool isDelete,
                                                  DomainModel.DbTimesheet dbTimesheetDBDataDelete)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (timesheetTechnicalSpecialists != null)
                {
                    if(isDelete)
                    {
                        if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && dbTimesheetDBDataDelete.DbTimesheetTechSpecialists != null)
                        {
                            IList<DbModel.TimesheetTechnicalSpecialist> dbTimehseetTS = dbTimesheetDBDataDelete.DbTimesheetTechSpecialists
                                                                                     .Where(x => timesheetTechnicalSpecialists.ToList()
                                                                                     .Any(x1 => x1.TimesheetTechnicalSpecialistId == x.Id &&
                                                                                                x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                            response = this._timesheetTechSpecService.Delete(timesheetTechnicalSpecialists,
                                                                             ref dbTimehseetTS,
                                                                             ref dbTimesheetDBDataDelete.DbTechnicalSpecialists,
                                                                             ref dbTimesheetDBDataDelete.DbTimesheets,
                                                                             dbModule,
                                                                             commitChanges,
                                                                             true,
                                                                             timesheetId);
                        }
                    }
                    else
                    {
                        if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && dbTimesheetDBData.DbTimesheetTechSpecialists != null)
                            response = this._timesheetTechSpecService.Modify(timesheetTechnicalSpecialists,
                                                                             ref dbTimesheetDBData.DbTimesheetTechSpecialists,
                                                                             ref dbTimesheetDBData.DbTechnicalSpecialists,
                                                                             ref dbTimesheetDBData.DbTimesheets,
                                                                             dbModule,
                                                                             commitChanges,
                                                                             false,
                                                                             timesheetId);

                        if (ValidationType.Delete != validationType)
                            response = this._timesheetTechSpecService.Add(timesheetTechnicalSpecialists,
                                                                           ref dbTimesheetDBData.DbTimesheetTechSpecialists,
                                                                           ref dbTimesheetDBData.DbTechnicalSpecialists,
                                                                           ref dbTimesheetDBData.DbTimesheets,
                                                                           dbModule,
                                                                           commitChanges,
                                                                           false,
                                                                           timesheetId);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetTechnicalSpecialists);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessTimesheetTechSpecAccItemTime(long timesheetId,
                                                              IList<DomainModel.TimesheetSpecialistAccountItemTime> timesheetTechSpecAccItemTime,
                                                              ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                              IList<DbModel.SqlauditModule> dbModule,
                                                              bool commitChanges,
                                                              ValidationType validationType)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (timesheetTechSpecAccItemTime != null && timesheetTechSpecAccItemTime.Count > 0)
                {
                    IList<DbModel.Data> dbExpenses = dbTimesheetDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType)?.ToList();
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && dbTimesheetDBData.DbTimesheetTechSpecTimes != null)
                        response = this._timesheetTechSpecAccountItemTimeService.Modify(timesheetTechSpecAccItemTime,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecTimes,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         timesheetId);

                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && dbTimesheetDBData.DbTimesheetTechSpecTimes != null)
                    {
                        IList<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbTimehseetTimeTS = dbTimesheetDBData.DbTimesheetTechSpecTimes
                                                                                 .Where(x => timesheetTechSpecAccItemTime.ToList()
                                                                                 .Any(x1 => x1.TimesheetTechnicalSpecialistAccountTimeId == x.Id &&
                                                                                            x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this._timesheetTechSpecAccountItemTimeService.Delete(timesheetTechSpecAccItemTime,
                                                                         ref dbTimehseetTimeTS,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         timesheetId);
                    }
                    if (ValidationType.Delete != validationType)
                        response = this._timesheetTechSpecAccountItemTimeService.Add(timesheetTechSpecAccItemTime,
                                                                     ref dbTimesheetDBData.DbTimesheetTechSpecTimes,
                                                                     ref dbTimesheetDBData.DbTimesheets,
                                                                     ref dbTimesheetDBData.DbAssignments,
                                                                     ref dbTimesheetDBData.DbProjects,
                                                                     ref dbTimesheetDBData.DbContracts,
                                                                     ref dbExpenses,
                                                                     dbModule,
                                                                     commitChanges,
                                                                     false,
                                                                     timesheetId);

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetTechSpecAccItemTime);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessTimesheetTechSpecAccItemTravel(long timesheetId,
                                                              IList<DomainModel.TimesheetSpecialistAccountItemTravel> timesheetTechSpecAccItemTravel,
                                                              ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                              IList<DbModel.SqlauditModule> dbModule,
                                                              bool commitChanges,
                                                              ValidationType validationType)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (timesheetTechSpecAccItemTravel != null && timesheetTechSpecAccItemTravel.Count > 0)
                {
                    IList<DbModel.Data> dbExpenses = dbTimesheetDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType)?.ToList();
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && dbTimesheetDBData.DbTimesheetTechSpecTravels != null)
                        response = this._timesheetTechSpecAccountItemTravelService.Modify(timesheetTechSpecAccItemTravel,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecTravels,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         timesheetId);

                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && dbTimesheetDBData.DbTimesheetTechSpecTravels != null)
                    {
                        IList<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbTimehseetTravelTS = dbTimesheetDBData.DbTimesheetTechSpecTravels
                                                                                 .Where(x => timesheetTechSpecAccItemTravel.ToList()
                                                                                 .Any(x1 => x1.TimesheetTechnicalSpecialistAccountTravelId == x.Id &&
                                                                                            x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this._timesheetTechSpecAccountItemTravelService.Delete(timesheetTechSpecAccItemTravel,
                                                                         ref dbTimehseetTravelTS,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         timesheetId);
                    }
                    if (ValidationType.Delete != validationType)
                        response = this._timesheetTechSpecAccountItemTravelService.Add(timesheetTechSpecAccItemTravel,
                                                                     ref dbTimesheetDBData.DbTimesheetTechSpecTravels,
                                                                     ref dbTimesheetDBData.DbTimesheets,
                                                                     ref dbTimesheetDBData.DbAssignments,
                                                                     ref dbTimesheetDBData.DbProjects,
                                                                     ref dbTimesheetDBData.DbContracts,
                                                                     ref dbExpenses,
                                                                     dbModule,
                                                                     commitChanges,
                                                                     false,
                                                                     timesheetId);

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetTechSpecAccItemTravel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessTimesheetTechSpecAccItemExpense(long timesheetId,
                                                              IList<DomainModel.TimesheetSpecialistAccountItemExpense> timesheetTechSpecAccItemExpense,
                                                              ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                              IList<DbModel.SqlauditModule> dbModule,
                                                              bool commitChanges,
                                                              ValidationType validationType)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (timesheetTechSpecAccItemExpense != null && timesheetTechSpecAccItemExpense.Count > 0)
                {
                    IList<DbModel.Data> dbExpenses = dbTimesheetDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType)?.ToList();
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && dbTimesheetDBData.DbTimesheetTechSpecExpenses != null)
                        response = this._timesheetTechSpecAccountItemExpenseService.Modify(timesheetTechSpecAccItemExpense,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecExpenses,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         timesheetId);

                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && dbTimesheetDBData.DbTimesheetTechSpecExpenses != null)
                    {
                        IList<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbTimehseetExpenseTS = dbTimesheetDBData.DbTimesheetTechSpecExpenses
                                                                                 .Where(x => timesheetTechSpecAccItemExpense.ToList()
                                                                                 .Any(x1 => x1.TimesheetTechnicalSpecialistAccountExpenseId == x.Id &&
                                                                                            x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this._timesheetTechSpecAccountItemExpenseService.Delete(timesheetTechSpecAccItemExpense,
                                                                         ref dbTimehseetExpenseTS,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         timesheetId);
                    }
                    if (ValidationType.Delete != validationType)
                        response = this._timesheetTechSpecAccountItemExpenseService.Add(timesheetTechSpecAccItemExpense,
                                                                     ref dbTimesheetDBData.DbTimesheetTechSpecExpenses,
                                                                     ref dbTimesheetDBData.DbTimesheets,
                                                                     ref dbTimesheetDBData.DbAssignments,
                                                                     ref dbTimesheetDBData.DbProjects,
                                                                     ref dbTimesheetDBData.DbContracts,
                                                                     ref dbExpenses,
                                                                     dbModule,
                                                                     commitChanges,
                                                                     false,
                                                                     timesheetId);

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetTechSpecAccItemExpense);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessTimehseetTechSpecAccItemConsumables(long timesheetId,
                                                              IList<DomainModel.TimesheetSpecialistAccountItemConsumable> timesheetTechSpecAccItemConsumables,
                                                              ref DomainModel.DbTimesheet dbTimesheetDBData,
                                                              IList<DbModel.SqlauditModule> dbModule,
                                                              bool commitChanges,
                                                              ValidationType validationType)
        {
            Exception exception = null;
            Response response = null;
            try
            {
                if (timesheetTechSpecAccItemConsumables != null && timesheetTechSpecAccItemConsumables.Count > 0)
                {
                    IList<DbModel.Data> dbExpenses = dbTimesheetDBData.DbData?.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType)?.ToList();
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && dbTimesheetDBData.DbTimesheetTechSpecConsumables != null)
                        response = this._timesheetTechSpecAccountItemConsumableService.Modify(timesheetTechSpecAccItemConsumables,
                                                                         ref dbTimesheetDBData.DbTimesheetTechSpecConsumables,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         timesheetId);

                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && dbTimesheetDBData.DbTimesheetTechSpecConsumables != null)
                    {
                        IList<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbTimehseetConsumablesTS = dbTimesheetDBData.DbTimesheetTechSpecConsumables
                                                                                 .Where(x => timesheetTechSpecAccItemConsumables.ToList()
                                                                                 .Any(x1 => x1.TimesheetTechnicalSpecialistAccountConsumableId == x.Id &&
                                                                                            x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this._timesheetTechSpecAccountItemConsumableService.Delete(timesheetTechSpecAccItemConsumables,
                                                                         ref dbTimehseetConsumablesTS,
                                                                         ref dbTimesheetDBData.DbTimesheets,
                                                                         ref dbTimesheetDBData.DbAssignments,
                                                                         ref dbTimesheetDBData.DbProjects,
                                                                         ref dbTimesheetDBData.DbContracts,
                                                                         ref dbExpenses,
                                                                         dbModule,
                                                                         commitChanges,
                                                                         false,
                                                                         timesheetId);
                    }
                    if (ValidationType.Delete != validationType)
                        response = this._timesheetTechSpecAccountItemConsumableService.Add(timesheetTechSpecAccItemConsumables,
                                                                     ref dbTimesheetDBData.DbTimesheetTechSpecConsumables,
                                                                     ref dbTimesheetDBData.DbTimesheets,
                                                                     ref dbTimesheetDBData.DbAssignments,
                                                                     ref dbTimesheetDBData.DbProjects,
                                                                     ref dbTimesheetDBData.DbContracts,
                                                                     ref dbExpenses,
                                                                     dbModule,
                                                                     commitChanges,
                                                                     false,
                                                                     timesheetId);

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetTechSpecAccItemConsumables);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        /*This section is called after validation to perform operation to save,update, delete data*/
        private Response ProcessTimehseetDocument(DomainModel.TimesheetDetail timesheet,
                                                 IList<DbModel.SqlauditModule> dbModule,
                                                 bool commitChanges,
                                                 ValidationType validationType,
                                                 DomainModel.TimesheetDetail timesheetDetail,
                                                 SqlAuditActionType sqlAuditActionType,
                                                 ref long? eventId,
                                                 ref List<ModuleDocument> uploadTimesheetDocuments)
        {
            Exception exception = null;
            Response response = null;
            List<DbModel.Document> dbDocuments = null;
            var timesheetDocuments = timesheet.TimesheetDocuments;
            try
            {
                if (timesheetDocuments != null)
                {
                    bool isAuditDocument = false;  //Hotfix Id 2 and 3
                    var audittimesheetDetails = ObjectExtension.Clone(timesheetDetail);

                    if (ValidationType.Delete != validationType)
                    {
                        response = this._documentService.Save(timesheetDocuments, ref dbDocuments,
                                                             commitChanges);
                        if (response != null && response.Code == ResponseType.Success.ToId())
                        {
                            foreach (ModuleDocument moduleDocument in timesheetDocuments)
                            {
                                if (moduleDocument.RecordStatus != "D" && (moduleDocument.DocumentType == VisitTimesheetConstants.REPORT_FLASH
                                    || moduleDocument.DocumentType == VisitTimesheetConstants.RELEASE_NOTE || moduleDocument.DocumentType == VisitTimesheetConstants.NON_CONFORMANCE_REPORT))
                                {
                                    uploadTimesheetDocuments.Add(moduleDocument);
                                }
                            }
                        }

                        isAuditDocument = true;//Hotfix Id 2 and 3             
                    }

                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                        response = this._documentService.Modify(timesheetDocuments, ref dbDocuments,
                                                                commitChanges);

                    if (validationType == ValidationType.Update || validationType == ValidationType.Delete)
                        response = this._documentService.Delete(timesheetDocuments,
                                                                commitChanges);
                    //Hotfix Id 2 and 3
                    if (isAuditDocument)
                    {
                        audittimesheetDetails.TimesheetDocuments = timesheetDocuments.Select(x =>
                        {
                            x.CreatedOn = dbDocuments?.FirstOrDefault(y => y.DocumentUniqueName == x.DocumentUniqueName)?.CreatedDate;
                            return x;
                        }).ToList(); // audit create date issue fix
                    }

                    if (response.Code == MessageType.Success.ToId())
                    {
                        DocumentAudit(audittimesheetDetails.TimesheetDocuments, dbModule, sqlAuditActionType, audittimesheetDetails, ref eventId, ref dbDocuments);
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetDocuments);
            }

            return response;
        }

        private void DocumentAudit(IList<ModuleDocument> timesheetDocuments, IList<DbModel.SqlauditModule> dbModule, SqlAuditActionType sqlAuditActionType, DomainModel.TimesheetDetail timesheetDetail, ref long? eventId, ref List<DbModel.Document> dbDocuments)
        {
            //For Document Audit
            if (timesheetDocuments.Count > 0)
            {
                object newData;
                object oldData;
                var newDocument = timesheetDocuments?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
                var modifiedDocument = timesheetDocuments?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
                var deletedDocument = timesheetDocuments?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();
                if (newDocument.Count > 0)
                {
                    newData = newDocument;
                    _auditSearchService.AuditLog(timesheetDetail, ref eventId, timesheetDetail?.TimesheetInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.TimesheetDocument, null, newData, dbModule);
                }
                if (modifiedDocument.Count > 0)
                {
                    newData = modifiedDocument?.OrderBy(x => x.Id)?.ToList();
                    oldData = (dbDocuments != null && dbDocuments.Count > 0) ? _mapper.Map<List<ModuleDocument>>(dbDocuments)?.OrderBy(x => x.Id)?.ToList() : null;
                    _auditSearchService.AuditLog(timesheetDetail, ref eventId, timesheetDetail?.TimesheetInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.TimesheetDocument, oldData, newData, dbModule);
                }
                if (deletedDocument.Count > 0)
                {
                    oldData = deletedDocument;
                    _auditSearchService.AuditLog(timesheetDetail, ref eventId, timesheetDetail?.TimesheetInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.TimesheetDocument, oldData, null, dbModule);
                }
            }
        }

        /*This section is called after validation to perform operation to save data*/
        private Response ProcessTimesheetNote(long timesheetId,
                                              IList<DomainModel.TimesheetNote> timesheetNotes,
                                              ref DomainModel.DbTimesheet dbTimesheetDBData,
                                              IList<DbModel.SqlauditModule> dbModule,
                                              bool commitChanges,
                                              ValidationType validationType)
        {
            Exception exception = null;
            Response response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());       //D661 issue 8 
            try
            {
                if (timesheetNotes != null)
                {
                    var addNotes = timesheetNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();       //D661 issue 8 
                    var updateNotes = timesheetNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    if (addNotes?.Count > 0)
                        response = this._timesheetNoteService.Add(timesheetNotes,
                                                                    ref dbTimesheetDBData.DbTimesheetNotes,
                                                                    ref dbTimesheetDBData.DbTimesheets,
                                                                    dbModule,
                                                                    commitChanges,
                                                                    false,
                                                                    timesheetId);
                    if (updateNotes?.Count > 0 && response.Code == MessageType.Success.ToId())       //D661 issue 8 
                        response = this._timesheetNoteService.Update(timesheetNotes,
                                                                   ref dbTimesheetDBData.DbTimesheetNotes,
                                                                   ref dbTimesheetDBData.DbTimesheets,
                                                                   dbModule,
                                                                   commitChanges,
                                                                   false,
                                                                   timesheetId);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetNotes);
            }

            return response;
        }

        private Response ProcessTimesheetInterCompany(int assignmentId, long timesheetId, bool commitChange, string actionByUser, long? eventId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                IList<DbModel.TimesheetInterCompanyDiscount> dbTimesheetInterCompanyDiscounts = _timesheetRepository.GetAssignmentInterCompanyDiscounts(assignmentId, timesheetId);
                if (dbTimesheetInterCompanyDiscounts != null && dbTimesheetInterCompanyDiscounts.Count > 0)
                {
                    _timesheetInterCompanyRepository.AutoSave = false;
                    _timesheetInterCompanyRepository.Add(dbTimesheetInterCompanyDiscounts);
                    if (commitChange)
                    {
                        _timesheetInterCompanyRepository.ForceSave();
                        dbTimesheetInterCompanyDiscounts?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, actionByUser, null,
                                                                                        ValidationType.Add.ToAuditActionType(),
                                                                                        SqlAuditModuleType.TimesheetInterCompanyDiscount,
                                                                                        null,
                                                                                        _mapper.Map<DomainModel.TimesheetInterCompanyDiscounts>(x1),
                                                                                        null));
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetId);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private async Task<Tuple<decimal, decimal, decimal>> CalculateTechSpecGrossMargin(long TimesheetId,
                                                                                            long TimesheetTechnicalSpecialistId)
        {
            decimal totalCharge = 0, totalPay = 0, grossMargin = 0;

            List<ExchangeRate> timesheetTechSpecExchangeRates = new List<ExchangeRate>();

            var timeTask = await _timesheetTechSpecAccountItemTimeService
            .GetAsync(new DomainModel.TimesheetSpecialistAccountItemTime
            {
                TimesheetId = TimesheetId,
                TimesheetTechnicalSpecialistId = TimesheetTechnicalSpecialistId
            });
            var timeLineItems = timeTask.Result.Populate<IList<DomainModel.TimesheetSpecialistAccountItemTime>>();

            Response tarvelTask = await _timesheetTechSpecAccountItemTravelService
            .GetAsync(new DomainModel.TimesheetSpecialistAccountItemTravel
            {
                TimesheetId = TimesheetId,
                TimesheetTechnicalSpecialistId = TimesheetTechnicalSpecialistId
            });

            var travelLineItems = tarvelTask.Result.Populate<IList<DomainModel.TimesheetSpecialistAccountItemTravel>>();
            var expenseTask = await _timesheetTechSpecAccountItemExpenseService
               .GetAsync(new DomainModel.TimesheetSpecialistAccountItemExpense
               {
                   TimesheetId = TimesheetId,
                   TimesheetTechnicalSpecialistId = TimesheetTechnicalSpecialistId
               });
            var expenseLineItems = expenseTask.Result.Populate<IList<DomainModel.TimesheetSpecialistAccountItemExpense>>();

            var consumableTask = await _timesheetTechSpecAccountItemConsumableService
                .GetAsync(new DomainModel.TimesheetSpecialistAccountItemConsumable
                {
                    TimesheetId = TimesheetId,
                    TimesheetTechnicalSpecialistId = TimesheetTechnicalSpecialistId
                });
            var consumableLineItems = consumableTask.Result.Populate<IList<DomainModel.TimesheetSpecialistAccountItemConsumable>>();

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
                foreach (var time in timeLineItems.Where(x => ((Convert.ToDecimal(x.ChargeRate) > 0 && x.ChargeTotalUnit > 0) || (x.PayRate > 0 && x.PayUnit > 0))))
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
                foreach (var tarvel in travelLineItems.Where(x => ((Convert.ToDecimal(x.ChargeRate) > 0 && x.ChargeUnit > 0) || (x.PayRate > 0 && x.PayUnit > 0))))
                {
                    decimal chargeCurrencyRate = currencyExchangeRates
                           .Where(x => (x.CurrencyFrom == tarvel.ChargeRateCurrency && x.CurrencyTo == "GBP"))
                           .Select(s => s.Rate).SingleOrDefault();

                    decimal payCurrencyRate = currencyExchangeRates
                           .Where(x => (x.CurrencyFrom == tarvel.PayRateCurrency && x.CurrencyTo == "GBP"))
                           .Select(s => s.Rate).SingleOrDefault();
                    if (string.IsNullOrEmpty(tarvel.ModeOfCreation))
                    {
                        totalCharge += ((Convert.ToDecimal(tarvel.ChargeRate) * tarvel.ChargeUnit) * chargeCurrencyRate);
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
                foreach (var consumable in consumableLineItems.Where(x => ((Convert.ToDecimal(x.ChargeRate) > 0 && x.ChargeUnit > 0) || (x.PayRate > 0 && x.PayUnit > 0))))
                {
                    decimal chargeCurrencyRate = currencyExchangeRates
                           .Where(x => (x.CurrencyFrom == consumable.ChargeRateCurrency && x.CurrencyTo == "GBP"))
                           .Select(s => s.Rate).SingleOrDefault();

                    decimal payCurrencyRate = currencyExchangeRates
                           .Where(x => (x.CurrencyFrom == consumable.PayRateCurrency && x.CurrencyTo == "GBP"))
                           .Select(s => s.Rate).SingleOrDefault();
                    if (string.IsNullOrEmpty(consumable.ModeOfCreation))
                    {
                        totalCharge += ((Convert.ToDecimal(consumable.ChargeRate) * consumable.ChargeUnit) * chargeCurrencyRate);
                    }
                    totalPay += ((consumable.PayRate * consumable.PayUnit) * payCurrencyRate);
                }
            }
            if (totalCharge == 0)
                return new Tuple<decimal, decimal, decimal>(0, totalPay, 0);

            grossMargin = Math.Round(((totalCharge - totalPay) / totalCharge) * 100, 2);

            return new Tuple<decimal, decimal, decimal>(totalCharge, totalPay, grossMargin); ;
        }

        private Response ProcessEmailNotifications(DomainModel.TimesheetEmailData timesheetEmailData, EmailTemplate emailTemplateType, ref IList<ValidationMessage> validationMessages, ref long? eventId, IList<DbModel.SqlauditModule> dbModule)
        {
            string emailSubject = string.Empty;
            string timesheetDate = string.Empty;
            string timesheetNumber = string.Empty;
            string coordinatorName = string.Empty;
            string SAMAccountName = string.Empty;
            string loggedInUser = string.Empty;
            string companyCode = string.Empty;
            Exception exception = null;
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            List<EmailAddress> toAddresses = null;
            List<EmailAddress> fromAddresses = null;
            List<EmailAddress> ccAddresses = null;
            IList<KeyValuePair<string, string>> emailContentPlaceholders = null;
            List<string> CHApprovalStatus = new List<string>() { "A", "R" };
            List<string> OCApprovalStatus = new List<string>() { "O", "C" };
            IList<UserInfo> userInfos = null;
            DomainModel.Timesheet timesheetInfo = null;
            EmailType emailType = EmailType.Notification;
            EmailDocumentUpload emailDocumentUpload = new EmailDocumentUpload
            {
                IsDocumentUpload = false
            };

            try
            {
                timesheetInfo = timesheetEmailData.TimesheetDetail.TimesheetInfo;
                //if CHC approves or rejects send mail to OC
                if (CHApprovalStatus.Contains(timesheetInfo.TimesheetStatus))
                {
                    coordinatorName = timesheetInfo.TimesheetOperatingCoordinator;
                    SAMAccountName = timesheetInfo.TimesheetOperatingCoordinatorCode;
                    companyCode = timesheetInfo.TimesheetContractCompanyCode;
                }
                //if oc approves send mail to CHC
                if (OCApprovalStatus.Contains(timesheetInfo.TimesheetStatus))
                {
                    coordinatorName = timesheetInfo.TimesheetContractCoordinator;
                    SAMAccountName = timesheetInfo.TimesheetContractCoordinatorCode;
                    companyCode = timesheetInfo.TimesheetOperatingCompanyCode;
                }
                //if oc rejects send mail to resouce
                if (timesheetInfo.TimesheetStatus == "J")
                {
                    coordinatorName = timesheetInfo.TimesheetOperatingCoordinator;
                    companyCode = timesheetInfo.TimesheetOperatingCompanyCode;
                }

                loggedInUser = (!string.IsNullOrEmpty(timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.ModifiedBy)
                        ? timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.ModifiedBy : timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.ActionByUser);
                List<string> userTypes = new List<string> { VisitTimesheetConstants.UserType_Coordinator,
                VisitTimesheetConstants.UserType_MICoordinator, VisitTimesheetConstants.Technical_Specialist };
                List<string> loginNames = new List<string>();
                bool isIntercompanyAssignment = timesheetInfo.TimesheetContractCompanyCode != timesheetInfo.TimesheetOperatingCompanyCode;
                if (timesheetInfo.TimesheetStatus == "J" || (!isIntercompanyAssignment && timesheetInfo.TimesheetStatus == "R"
                    && (timesheetInfo.TimesheetOperatingCoordinatorCode == timesheetInfo.TimesheetContractCoordinatorCode
                    || loggedInUser == timesheetInfo.TimesheetOperatingCoordinatorCode)))
                {
                    loginNames = timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TechSpecialists?.Select(x => x.LoginName).ToList();
                }
                if (!string.IsNullOrEmpty(SAMAccountName))
                {
                    loginNames.Add(SAMAccountName);
                }
                if (!string.IsNullOrEmpty(loggedInUser))
                {
                    loginNames.Add(loggedInUser);
                }

                if (!string.IsNullOrEmpty(timesheetInfo.TimesheetContractCoordinatorCode))
                    loginNames.Add(timesheetInfo.TimesheetContractCoordinatorCode);
                if (!string.IsNullOrEmpty(timesheetInfo.TimesheetOperatingCoordinatorCode)) 
                    loginNames.Add(timesheetInfo.TimesheetOperatingCoordinatorCode);

                userInfos = _userService.GetByUserType(loginNames, userTypes, true)
                                        .Result
                                        .Populate<IList<UserInfo>>();
                if (userInfos != null && userInfos.Count > 0)
                {
                    if (timesheetInfo.TimesheetStatus == "J" || (!isIntercompanyAssignment && timesheetInfo.TimesheetStatus == "R"
                        && (timesheetInfo.TimesheetOperatingCoordinatorCode == timesheetInfo.TimesheetContractCoordinatorCode
                        || loggedInUser == timesheetInfo.TimesheetOperatingCoordinatorCode)))
                    {
                        var technicalSpecialists = timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TechSpecialists?.ToList();
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
                    else if (emailTemplateType == EmailTemplate.TimesheetApproveClientRequirement)
                    {
                        if (timesheetEmailData.ToAddress != null && timesheetEmailData.ToAddress.Count > 0)
                        {
                            toAddresses = new List<EmailAddress>();
                            foreach (EmailAddress emailAddress in timesheetEmailData.ToAddress)
                            {
                                toAddresses.Add(new EmailAddress() { DisplayName = string.Empty, Address = emailAddress.Address });
                            }
                        }
                        else
                        {
                            IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetProjectNumber);
                            if (projectNotification != null && projectNotification.Count > 0)
                            {
                                toAddresses = projectNotification.Where(x => x.IsSendCustomerReportingNotification == true)
                                                .Select(x => new EmailAddress() { DisplayName = string.Empty, Address = x.EmailAddress }).ToList();
                            }
                        }
                    }
                    else if (emailTemplateType == EmailTemplate.EmailCustomerDirectReporting)
                    {
                        IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetProjectNumber);
                        if (projectNotification != null && projectNotification.Count > 0)
                        {
                            toAddresses = projectNotification.Where(x => x.IsSendCustomerDirectReportingNotification == true)
                                            .Select(x => new EmailAddress() { DisplayName = string.Empty, Address = x.EmailAddress }).ToList();
                        }
                        loggedInUser = SAMAccountName;
                    }
                    else if (emailTemplateType == EmailTemplate.EmailCustomerFlashReporting)
                    {
                        IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetProjectNumber);
                        if (projectNotification != null && projectNotification.Count > 0)
                        {
                            toAddresses = projectNotification.Where(x => x.IsSendFlashReportingNotification == true)
                                            .Select(x => new EmailAddress() { DisplayName = string.Empty, Address = x.EmailAddress }).ToList();
                        }
                    }
                    else if (emailTemplateType == EmailTemplate.EmailCustomerInspectionReleaseNotes)
                    {
                        IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetProjectNumber);
                        if (projectNotification != null && projectNotification.Count > 0)
                        {
                            toAddresses = projectNotification.Where(x => x.IsSendInspectionReleaseNotesNotification == true)
                                            .Select(x => new EmailAddress() { DisplayName = string.Empty, Address = x.EmailAddress }).ToList();
                        }
                    }
                    else if (emailTemplateType == EmailTemplate.EmailNCRReporting)
                    {
                        IList<ProjectClientNotification> projectNotification = this.GetProjectClientNotifications(timesheetEmailData?.TimesheetDetail?.TimesheetInfo?.TimesheetProjectNumber);
                        if (projectNotification != null && projectNotification.Count > 0)
                        {
                            toAddresses = projectNotification.Where(x => x.IsSendNCRReportingNotification == true)
                                            .Select(x => new EmailAddress() { DisplayName = string.Empty, Address = x.EmailAddress }).ToList();
                        }
                    }
                    else
                    {
                        toAddresses = userInfos.Where(x => string.Equals(x.LogonName, SAMAccountName))
                                           .Select(x => new EmailAddress() { DisplayName = x.UserName, Address = x.Email })
                                           .ToList();

                        if (!string.IsNullOrEmpty(loggedInUser))
                        {
                            ccAddresses = userInfos.Where(x => string.Equals(x.LogonName, loggedInUser))
                                        .Select(x => new EmailAddress()
                                        { DisplayName = x.UserName, Address = x.Email })
                                        .ToList();
                        }
                    }

                    #region From Address for TimeAppove ,TimeReject and Others - CR1021
                    if (emailTemplateType == EmailTemplate.TimesheetApprove)
                    {
                        fromAddresses = userInfos.Where(x => string.Equals(x.LogonName, timesheetInfo.TimesheetOperatingCoordinatorCode))
                                            .Select(x => new EmailAddress()
                                            { DisplayName = x.UserName, Address = x.Email })
                                            .ToList();
                        if (fromAddresses != null && fromAddresses.Count > 0)
                            coordinatorName = fromAddresses[0].DisplayName;
                    }
                    else if (emailTemplateType == EmailTemplate.TimesheetReject)
                    {
                        fromAddresses = userInfos.Where(x => string.Equals(x.LogonName, timesheetInfo.TimesheetContractCoordinatorCode))
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

                    if (toAddresses != null && toAddresses.Count > 0)
                    {
                        timesheetDate = timesheetInfo.TimesheetStartDate?.ToString("dd/MMM/yyyy");
                        string timesheetDesc = string.IsNullOrEmpty(timesheetInfo.TimesheetDescription) ? "" : timesheetInfo.TimesheetDescription;
                        string assignmentNumber = Convert.ToString(timesheetInfo.TimesheetAssignmentNumber);
                        string formattedCustomerName = !String.IsNullOrWhiteSpace(timesheetInfo.TimesheetCustomerName) && timesheetInfo.TimesheetCustomerName.Length >= 5
                                                                    ? timesheetInfo.TimesheetCustomerName.Substring(0, 5)
                                                                    : timesheetInfo.TimesheetCustomerName;
                        timesheetNumber = string.Format("({0} : {1})", assignmentNumber, timesheetDesc);
                        string projectNumber = Convert.ToString(timesheetInfo.TimesheetProjectNumber);
                        string timesheetReportNumber = timesheetInfo.TimesheetNumber?.ToString("000000") + " " + timesheetDesc;
                        string timesheetNDT = (!string.IsNullOrEmpty(timesheetInfo.AssignmentProjectWorkFlow) && timesheetInfo.AssignmentProjectWorkFlow.Trim() == "N"
                                                ? VisitTimesheetConstants.TIMESHEETNDT : VisitTimesheetConstants.TIMESHEET);
                        string timesheetNDTLower = (!string.IsNullOrEmpty(timesheetInfo.AssignmentProjectWorkFlow) && timesheetInfo.AssignmentProjectWorkFlow.Trim() == "N"
                                                ? VisitTimesheetConstants.TIMESHEETNDT_LOWER : VisitTimesheetConstants.TIMESHEET_LOWER);
                        string timesheetURL = (string.IsNullOrEmpty(_environment.ExtranetURL) ? "" : _environment.ExtranetURL + Convert.ToString(timesheetInfo.TimesheetId));
                        List<Attachment> attachments = new List<Attachment>();

                        switch (emailTemplateType)
                        {
                            case EmailTemplate.TimesheetReject:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, timesheetInfo.TimesheetAssignmentNumber?.ToString("00000")),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, timesheetDate),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, timesheetNDT),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, timesheetNDTLower),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_REJECT_NOTES,
                                                        string.IsNullOrEmpty(timesheetEmailData.ReasonForRejection) ? "" : string.Concat(VisitTimesheetConstants.REASON_FOR_REJECTION, timesheetEmailData.ReasonForRejection)),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, (timesheetInfo.TimesheetStatus == "R" ? timesheetInfo.TimesheetContractCompany : timesheetInfo.TimesheetOperatingCompany)),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, timesheetInfo.TimesheetContractNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, timesheetInfo.CustomerProjectName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER, timesheetDesc),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, string.Empty),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, string.Empty),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, timesheetURL),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, timesheetInfo.TimesheetDatePeriod)
                                };
                                if (!string.IsNullOrEmpty(timesheetInfo.AssignmentProjectWorkFlow) && timesheetInfo.AssignmentProjectWorkFlow.Trim() == "N")
                                    emailSubject = string.Format(VisitTimesheetConstants.EMAIL_NOTIFICATION_TIMESHEET_NDT_REJECT_SUBJECT, timesheetInfo.TimesheetNumber?.ToString("000000"));
                                else
                                    emailSubject = string.Format(VisitTimesheetConstants.EMAIL_NOTIFICATION_TIMESHEET_REJECT_SUBJECT, timesheetInfo.TimesheetNumber?.ToString("000000"));
                                emailType = timesheetInfo.TimesheetStatus == "R" ? EmailType.RVC : EmailType.RVT;
                                break;
                            case EmailTemplate.TimesheetApproveClientRequirement:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, assignmentNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, timesheetDate),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, timesheetNDT),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, timesheetNDTLower),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, timesheetInfo.TimesheetDatePeriod),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, timesheetURL),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, timesheetInfo.TimesheetContractCompany),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, timesheetInfo.TimesheetContractNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, timesheetInfo.CustomerProjectName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER, timesheetDesc),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, string.Empty),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, string.Empty)
                                };

                                if (timesheetEmailData.Attachments != null && timesheetEmailData.Attachments.Count > 0)
                                {
                                    attachments = timesheetEmailData.Attachments;
                                }
                                if (string.IsNullOrEmpty(timesheetEmailData.EmailSubject))
                                    emailSubject = string.Format(VisitTimesheetConstants.EMAIL_NOTIFICATION_TIMESHEET_CLIENTREPORTING_SUBJECT, timesheetNumber);
                                else
                                    emailSubject = timesheetEmailData.EmailSubject;
                                emailType = EmailType.CRN;
                                break;
                            case EmailTemplate.TimesheetApprove:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, timesheetInfo.TimesheetAssignmentNumber?.ToString("00000")),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, timesheetDate),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, timesheetNDT),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, timesheetNDTLower),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, timesheetInfo.TimesheetDatePeriod),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, timesheetURL),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, timesheetInfo.TimesheetContractCompany),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, timesheetInfo.TimesheetContractNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, timesheetInfo.CustomerProjectName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER, timesheetDesc),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, string.Empty),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, string.Empty),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISITTIMESHEET, timesheetNDT),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.OPERATING_COMPANY_NAME, timesheetInfo.TimesheetOperatingCompany),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.OPERATOR_NAME, timesheetInfo.TimesheetOperatingCoordinator)
                                };
                                emailSubject = string.Format(VisitTimesheetConstants.EMAIL_NOTIFICATION_TIMESHEET_APPROVE_SUBJECT, timesheetInfo.TimesheetContractNumber, projectNumber, timesheetInfo.TimesheetAssignmentNumber?.ToString("00000"), timesheetReportNumber);
                                emailType = EmailType.IVA;
                                break;
                            case EmailTemplate.EmailCustomerDirectReporting:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, timesheetInfo.TimesheetAssignmentNumber?.ToString("00000")),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, timesheetDate),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, timesheetNDT),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, timesheetNDTLower),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, timesheetInfo.TimesheetDatePeriod),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, timesheetURL),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, timesheetInfo.TimesheetOperatingCompany),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, timesheetInfo.TimesheetContractNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, timesheetInfo.CustomerProjectName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER, timesheetDesc),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, string.Empty),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, string.Empty)
                                };
                                emailSubject = VisitTimesheetConstants.EMAIL_DIRECT_REPORTING;
                                emailType = EmailType.CDR;
                                break;
                            case EmailTemplate.EmailCustomerFlashReporting:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, assignmentNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, timesheetDate),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, timesheetNDT),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, timesheetNDTLower),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, timesheetInfo.TimesheetDatePeriod),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, timesheetURL),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, timesheetInfo.TimesheetContractCompany),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, timesheetInfo.TimesheetContractNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, timesheetInfo.CustomerProjectName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER, timesheetDesc),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, string.Empty),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, string.Empty)
                                };
                                emailSubject = VisitTimesheetConstants.FLASH_REPORT_NOTIFICATION;
                                break;
                            case EmailTemplate.EmailCustomerInspectionReleaseNotes:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, assignmentNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, timesheetDate),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, timesheetNDT),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, timesheetNDTLower),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, timesheetInfo.TimesheetDatePeriod),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, timesheetURL),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, timesheetInfo.TimesheetContractCompany),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, timesheetInfo.TimesheetContractNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, timesheetInfo.CustomerProjectName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER, timesheetDesc),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, string.Empty),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, string.Empty)
                                };
                                emailSubject = VisitTimesheetConstants.RELEASE_NOTES_NOTIFICATION;
                                break;
                            case EmailTemplate.EmailNCRReporting:
                                emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COORDINATOR_NAME, coordinatorName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.ASSIGNMENT_NUMBER, assignmentNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.PROJECT_NUMBER, projectNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_DATE, timesheetDate),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET, timesheetNDT),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_OR_TIMESHEET_LOWER, timesheetNDTLower),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_DATE_PERIOD, timesheetInfo.TimesheetDatePeriod),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.VISIT_TIMESHEET_URL, timesheetURL),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_NAME, timesheetInfo.TimesheetCustomerName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.COMPANY, timesheetInfo.TimesheetContractCompany),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CONTRACT_NUMBER, timesheetInfo.TimesheetContractNumber),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.CUSTOMER_PROJECT_NAME, timesheetInfo.CustomerProjectName),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.REPORT_NUMBER, timesheetDesc),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIER, string.Empty),
                                new KeyValuePair<string, string>(VisitTimesheetConstants.SUPPLIERPO, string.Empty)
                                };
                                emailSubject = VisitTimesheetConstants.NCR_NOTIFICATION;
                                break;
                        }

                        string token = DateTime.Now.ToString(VisitTimesheetConstants.TOKEN_DATE_FORMAT);
                        emailMessage = ProcessEmailMessage(ModuleType.Timesheet, emailTemplateType,
                                                           companyCode,
                                                           emailType, ModuleCodeType.TIME,
                                                           timesheetInfo.TimesheetId.ToString(), emailSubject,
                                                           emailContentPlaceholders, toAddresses, fromAddresses, token, attachments, timesheetEmailData.EmailContent,ccAddresses);
                        emailDocumentUpload = UploadTimesheetDocuments(emailTemplateType, timesheetInfo, emailMessage, timesheetEmailData, ref eventId, dbModule, token);
                        //if(emailTemplateType == EmailTemplate.TimesheetApprove || emailTemplateType == EmailTemplate.TimesheetReject)
                        _emailService.Add(new List<EmailQueueMessage> { emailMessage });
                    }
                    else
                    {
                        //_logger.LogInformation(code,"Failed to form toAddress", timesheetId,timesheetInfo.timesheetAssignmentId);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, emailDocumentUpload, exception);
        }

        private EmailDocumentUpload UploadTimesheetDocuments(EmailTemplate emailTemplateType, DomainModel.Timesheet timesheetInfo, EmailQueueMessage emailMessage, DomainModel.TimesheetEmailData timesheetEmailData, ref long? eventId, IList<DbModel.SqlauditModule> dbModule, string token)
        {
            EmailDocumentUpload emailDocumentUpload = new EmailDocumentUpload();
            DocumentUniqueNameDetail documentUniquename = new DocumentUniqueNameDetail();
            try
            {
                bool IsVisibleToCustomer = false;
                StringBuilder documentMessage = new StringBuilder();
                if (emailTemplateType == EmailTemplate.TimesheetReject)
                {
                    documentUniquename.DocumentName = VisitTimesheetConstants.TIMESHEET_REJECTED_EMAIL_LOG;
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
                else if (emailTemplateType == EmailTemplate.TimesheetApproveClientRequirement)
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
                else if (emailTemplateType == EmailTemplate.TimesheetApprove)
                {
                    documentUniquename.DocumentName = VisitTimesheetConstants.TIMESHEET_APPROVED_EMAIL_LOG;
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
                            documentMessage.Append(string.Concat("\"", timesheetInfo.TimesheetCustomerName, "\" <span><</span>", emailMessage.ToAddresses[i].Address, "<span>></span>"));
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
                // documentMessage.Append(VisitTimesheetConstants.EMAIL_TOKEN);
                // documentMessage.Append(token);

                documentUniquename.ModuleCode = ModuleCodeType.TIME.ToString();
                documentUniquename.RequestedBy = string.Empty;
                documentUniquename.ModuleCodeReference = Convert.ToString(timesheetInfo.TimesheetId);
                documentUniquename.DocumentType = VisitTimesheetConstants.VISIT_TIMSHEET_EVOLUTION_EMAIL;
                documentUniquename.SubModuleCodeReference = "0";

                emailDocumentUpload.IsDocumentUpload = true;
                emailDocumentUpload.DocumentUniqueName = documentUniquename;
                emailDocumentUpload.DocumentMessage = this.FormatDocumentMessage(Convert.ToString(documentMessage));
                emailDocumentUpload.IsVisibleToCustomer = IsVisibleToCustomer;
                emailDocumentUpload.IsVisibleToTS = false;
                this.PostRequest(emailDocumentUpload, timesheetEmailData, ref eventId, dbModule);
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

        private Response PostRequest(EmailDocumentUpload model, DomainModel.TimesheetEmailData timesheetEmailData, ref long? eventId, IList<DbModel.SqlauditModule> dbModule)
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
                        DocumentAudit(listModuleDocument, dbModule, SqlAuditActionType.I, timesheetEmailData.TimesheetDetail, ref eventId, ref dbDocuments);
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

        private EmailQueueMessage ProcessEmailMessage(ModuleType moduleType, EmailTemplate emailTemplateType, string companyCode
                                                    , EmailType emailType, ModuleCodeType moduleCodeType, string moduleRefCode, string emailSubject
                                                    , IList<KeyValuePair<string, string>> emailContentPlaceholders, List<EmailAddress> toAddresses, List<EmailAddress> fromAddresses, string token
                                                    , List<Attachment> attachment = null, string emailContent = null, List<EmailAddress> ccAddresses = null, List<EmailAddress> bccAddresses = null)
        {
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            string emailTemplateContent = string.Empty;
            try
            {
                if (emailTemplateType == EmailTemplate.TimesheetApproveClientRequirement)
                    emailTemplateContent = _timesheetRepository.GetTemplate(companyCode, CompanyMessageType.EmailCustomerReportingNotification, EmailKey.EmailCustomerReportingNotification.ToString());
                else if (emailTemplateType == EmailTemplate.TimesheetApprove)
                    emailTemplateContent = _emailService.GetEmailTemplate(new List<string> { EmailTemplate.EmailApproveIVA.ToString() })?.FirstOrDefault(x => x.KeyName == EmailTemplate.EmailApproveIVA.ToString())?.KeyValue;
                else if (emailTemplateType == EmailTemplate.TimesheetReject)
                    emailTemplateContent = _timesheetRepository.GetTemplate(companyCode, CompanyMessageType.EmailRejectedVisit, EmailKey.EmailRejectedVisit.ToString());
                else if (emailTemplateType == EmailTemplate.EmailCustomerDirectReporting)
                    emailTemplateContent = _timesheetRepository.GetTemplate(companyCode, CompanyMessageType.EmailCustomerDirectReporting, EmailKey.EmailCustomerDirectReporting.ToString());
                else if (emailTemplateType == EmailTemplate.EmailCustomerFlashReporting)
                    emailTemplateContent = _timesheetRepository.GetTemplate(string.Empty, CompanyMessageType.NotRequired, EmailKey.EmailCustomerFlashReporting.ToString());
                else if (emailTemplateType == EmailTemplate.EmailCustomerInspectionReleaseNotes)
                    emailTemplateContent = _timesheetRepository.GetTemplate(string.Empty, CompanyMessageType.NotRequired, EmailKey.EmailCustomerInspectionReleaseNotes.ToString());
                else if (emailTemplateType == EmailTemplate.EmailNCRReporting)
                    emailTemplateContent = _timesheetRepository.GetTemplate(string.Empty, CompanyMessageType.NotRequired, EmailKey.EmailNCRReporting.ToString());

                if (emailTemplateType == EmailTemplate.TimesheetApproveClientRequirement && !string.IsNullOrEmpty(emailContent))
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
                emailMessage.ToAddresses = toAddresses;
                emailMessage.FromAddresses = fromAddresses;
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

        private void AppendEvent(DomainModel.TimesheetDetail timesheetDetail,
                                 long? eventId)
        {
            ObjectExtension.SetPropertyValue(timesheetDetail.TimesheetInfo, "EventId", eventId);
            ObjectExtension.SetPropertyValue(timesheetDetail.TimesheetNotes, "EventId", eventId);
            ObjectExtension.SetPropertyValue(timesheetDetail.TimesheetReferences, "EventId", eventId);
            ObjectExtension.SetPropertyValue(timesheetDetail.TimesheetTechnicalSpecialistConsumables, "EventId", eventId);
            ObjectExtension.SetPropertyValue(timesheetDetail.TimesheetTechnicalSpecialistExpenses, "EventId", eventId);
            ObjectExtension.SetPropertyValue(timesheetDetail.TimesheetTechnicalSpecialists, "EventId", eventId);
            ObjectExtension.SetPropertyValue(timesheetDetail.TimesheetTechnicalSpecialistTimes, "EventId", eventId);
            ObjectExtension.SetPropertyValue(timesheetDetail.TimesheetTechnicalSpecialistTravels, "EventId", eventId);


        }

        private Response UpdateAssignmentStartEndDate(DomainModel.Timesheet timesheet)
        {
            ResponseType responseType = ResponseType.Success;
            Exception exception = null;
            try
            {
                DomainModel.TimesheetSearch searchModel = new DomainModel.TimesheetSearch
                {
                    TimesheetAssignmentId = timesheet.TimesheetAssignmentId
                };
                var joins = new string[] { "Assignment" };
                IList<DomainModel.TimesheetSearch> timesheetDetails = _timesheetRepository.GetSearchTimesheet(searchModel, joins);

                DateTime? timesheetStartDate = timesheetDetails.Where(x => x.TimesheetId != timesheet.TimesheetId).OrderBy(x => x.TimesheetStartDate).Select(x => x.TimesheetStartDate).FirstOrDefault();
                DateTime? timesheetEndDate = timesheetDetails.Where(x => x.TimesheetId != timesheet.TimesheetId).OrderBy(x => x.TimesheetEndDate).Select(x => x.TimesheetEndDate).FirstOrDefault();

                var updateValueProps = new List<KeyValuePair<string, object>> {
                                            new KeyValuePair<string, object>("FirstVisitTimesheetStartDate", (timesheet.TimesheetStartDate > timesheetStartDate ? timesheetStartDate : timesheet.TimesheetStartDate)),
                                            new KeyValuePair<string, object>("FirstVisitTimesheetEndDate", (timesheet.TimesheetEndDate > timesheetEndDate ? timesheetEndDate : timesheet.TimesheetEndDate))
                                           };
                _assignmentService.Modify(timesheet.TimesheetAssignmentId, updateValueProps, true, a => a.FirstVisitTimesheetStartDate, b => b.FirstVisitTimesheetEndDate);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheet);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
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

        #endregion
    }
}
