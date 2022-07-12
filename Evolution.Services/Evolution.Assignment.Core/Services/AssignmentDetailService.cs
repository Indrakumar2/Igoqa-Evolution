using AutoMapper;
using Evolution.Assignment.Domain.Enums;
using Evolution.Assignment.Domain.Interfaces.Assignments;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Interfaces.Validations;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Budget;
using Evolution.Common.Models.ExchangeRate;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Contract.Domain.Enums;
using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.DbRepository.Interfaces.Master;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Email.Models;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Master.Domain.Models;
using Evolution.NumberSequence.InfraStructure.Interface;
using Evolution.Project.Domain.Interfaces.Data;
using Evolution.ResourceSearch.Domain.Interfaces.ResourceSearch;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.SupplierPO.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.Timesheet.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Web.Gateway.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainContractModel = Evolution.Contract.Domain.Models.Contracts;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;
using DomainModelVisit = Evolution.Visit.Domain.Models.Visits;
using DomainModel1 = Evolution.Visit.Domain.Models.Visits;


namespace Evolution.Assignment.Core.Services
{
    public class AssignmentDetailService : IAssignmentDetailService
    {
        private readonly IAssignmentRepository _assignmentRepository = null;
        private readonly IAssignmentValidationService _assignmentValidationService = null;
        private readonly IAssignmentTaxonomyValidationService _assignmentTaxonomyValidationService = null;
        private readonly IAssignmentSubSupplierValidationService _assignmentSubSupplierValidationService = null;
        private readonly IAssignmentSubSupplierTSValidationService _assignmentSubSupplierTSValidationService = null;
        private readonly IAssignmentNoteValidationService _assignmentNoteValidationService = null;
        private readonly IAssignmentContributionCalculationValidationService _assignmentContributionCalculationValidationService = null;
        private readonly IAssignmentReferenceTypeValidationService _assignmentReferenceTypeValidationService = null;
        private readonly IAssignmentTechnicalSpecilaistValidationService _assignmentTechnicalSpecilaistValidationService = null;
        private readonly IAssignmentTechnicalSpecialistScheduleValidationService _assignmentTechnicalSpecialistScheduleValidationService = null;
        private readonly IAssignmentAdditionalExpenseValidationService _assignmentAdditionalExpenseValidationService = null;
        private readonly IAssignmentContractRateScheduleValidationService _assignmentContractRateScheduleValidationService = null;
        private readonly INumberSequenceRepository _numberSequenceRepository = null;
        private readonly IContractRepository _contractRepository = null;
        private readonly IProjectRepository _projectRepository = null;
        private readonly ICompanyRepository _companyRepository = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly ITaxonomyServiceRepository _taxonomyRepository = null;
        private readonly IAssignmentReferenceTypeRepository _assignmentReferenceRepository = null;
        private readonly IAssignmentTaxonomyRepository _assignmentTaxonomyRepository = null;
        private readonly IAssignmentContractRateScheduleRepository _assignmentContractRateScheduleRepository = null;
        private readonly IAssignmentTechnicalSpecilaistRepository _assignmentTechnicalSpecialistRepository = null;
        private readonly IAssignmentTechnicalSpecialistScheduleRepository _assignmentTechnicalSpecialistScheduleRepository = null;
        private readonly IAssignmentSubSupplerRepository _assignmentSubSupplierRepository = null;
        private readonly IAssignmentSubSupplerTSRepository _assignmentSubSupplerTsRepository = null;
        private readonly IAssignmentNoteRepository _assignmentNoteRepository = null;
        private readonly IAssignmentInstructionsRepository _assignmentMessageRepository = null;
        private readonly IAssignmentContributionCalculationRepository _assignmentContributionCalculationRepository = null;
        private readonly IAssignmentAdditionalExpenseRepository _assignmentAdditionalExpenseRepository = null;
        private readonly IAssignmentContributionRevenueCostRepository _revenueCostRepository = null;
        private readonly IAssignmentInterCompanyDiscountRepository _assignmentInterCompanyDiscount = null;
        private readonly ISupplierPOSubSupplierRepository _supplierPoSubSupplierRepository = null;
        private readonly ITechnicalSpecialistCalendarRepository _tsCalendarRepository = null;
        private readonly ITimesheetRepository _timesheetRepository = null;
        private readonly ITechSpecAccountItemTimeRepository _timesheetAccountItemTimeRepository = null;
        private readonly ITechSpecAccountItemConsumableRepository _timeAccountItemConsumableRepository = null;
        private readonly ITechSpecAccountItemExpenseRepository _timeAccountItemExpenseRepository = null;
        private readonly ITechSpecAccountItemTravelRepository _timeAccountItemTravelRepository = null;
        private readonly IVisitRepository _visitRepository = null;
        private readonly IVisitTechnicalSpecialistConsumableRepository _visitAccountItemConsumableRepository = null;
        private readonly IVisitTechnicalSpecialistExpenseRepository _visitAccountItemExpenseRepository = null;
        private readonly IVisitTechnicalSpecialistTravelRepository _visitAccountItemTravelRepository = null;
        private readonly IVisitTechnicalSpecialistTimeRespository _visitAccountItemTimeRepository = null;
        private readonly IVisitInterCompanyDiscountsRepository _visitInterCompanyRepository = null;
        private readonly ITimesheetInterCompanyDiscountsRepository _timesheetInterCompanyRepository = null;
        private readonly IDocumentService _documentService = null;
        private readonly IEmailQueueService _emailService = null;
        private readonly IUserService _userService = null;
        private readonly IResourceSearchService _resourceSearchService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly IAuditLogger _auditLogger = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<AssignmentDetailService> _logger = null;
        private readonly JObject _messageDescriptions = null;
        private readonly ICurrencyExchangeRateService _currencyExchangeRateService = null;
        private readonly IContractExchangeRateRepository _exchangeRateRepository = null;
        private readonly ITechnicalSpecialistContactRepository _technicalSpecialistContactRepository = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        public readonly string _emailDocumentEndpoint = "documents/UploadDocuments";
        private readonly IMasterRepository _masterRepository = null;

        public AssignmentDetailService(IAssignmentRepository assignmentRepository,
                                    IAssignmentValidationService assignmentValidationService,
                                    INumberSequenceRepository numberSequenceRepository,
                                    IContractRepository contractRepository,
                                    IProjectRepository projectRepository,
                                    ICompanyRepository companyRepository,
                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                    ITaxonomyServiceRepository taxonomyRepository,
                                    IAssignmentReferenceTypeRepository assignmentReferenceRepository,
                                    IAssignmentTaxonomyRepository assignmentTaxonomyRepository,
                                    IAssignmentContractRateScheduleRepository assignmentContractRateScheduleRepository,
                                    IAssignmentTechnicalSpecilaistRepository assignmentTechnicalSpecialistRepository,
                                    IAssignmentTechnicalSpecialistScheduleRepository assignmentTechnicalSpecialistScheduleRepository,
                                    IAssignmentSubSupplerRepository assignmentSubSupplierRepository,
                                    IAssignmentSubSupplerTSRepository assignmentSubSupplerTsRepository,
                                    IAssignmentNoteRepository assignmentNoteRepository,
                                    IAssignmentInstructionsRepository assignmentMessageRepository,
                                    IAssignmentContributionCalculationRepository assignmentContributionCalculationRepository,
                                    IAssignmentContributionRevenueCostRepository revenueCostRepository,
                                    IAssignmentAdditionalExpenseRepository assignmentAdditionalExpenseRepository,
                                    IAssignmentInterCompanyDiscountRepository assignmentInterCompanyDiscount,
                                    ISupplierPOSubSupplierRepository supplierPoSubSupplierRepository,
                                    ITechnicalSpecialistCalendarRepository tsCalendarRepository,
                                    ITimesheetRepository timesheetRepository,
                                    ITechSpecAccountItemTimeRepository timesheetAccountItemTimeRepository,
                                    ITechSpecAccountItemConsumableRepository timeAccountItemConsumableRepository,
                                    ITechSpecAccountItemExpenseRepository timeAccountItemExpenseRepository,
                                    ITechSpecAccountItemTravelRepository timeAccountItemTravelRepository,
                                    ITimesheetInterCompanyDiscountsRepository timesheetInterCompanyRepository,
                                    IVisitRepository visitRepository,
                                    IVisitTechnicalSpecialistConsumableRepository visitAccountItemConsumableRepository,
                                    IVisitTechnicalSpecialistExpenseRepository visitAccountItemExpenseRepository,
                                    IVisitTechnicalSpecialistTravelRepository visitAccountItemTravelRepository,
                                    IVisitTechnicalSpecialistTimeRespository visitAccountItemTimeRepository,
                                    IVisitInterCompanyDiscountsRepository visitInterCompanyRepository,
                                    IDocumentService documentService,
                                    IAuditSearchService auditSearchService,
                                    IResourceSearchService resourceSearchService,
                                    ICurrencyExchangeRateService currencyExchangeRateService,
                                    IContractExchangeRateRepository exchangeRateRepository,
                                    ITechnicalSpecialistContactRepository technicalSpecialistContactRepository,
                                    IEmailQueueService emailService,
                                    IUserService userService,
                                    IMapper mapper,
                                    IAppLogger<AssignmentDetailService> logger,
                                    JObject messageDescriptions,
                                    IOptions<AppEnvVariableBaseModel> environment,
                                    IAssignmentTaxonomyValidationService assignmentTaxonomyValidationService,
                                    IAssignmentSubSupplierValidationService assignmentSubSupplierValidationService,
                                    IAssignmentSubSupplierTSValidationService assignmentSubSupplierTSValidationService,
                                    IAssignmentNoteValidationService assignmentNoteValidationService,
                                    IAssignmentContributionCalculationValidationService assignmentContributionCalculationValidationService,
                                    IAssignmentReferenceTypeValidationService assignmentReferenceTypeValidationService,
                                    IAssignmentTechnicalSpecilaistValidationService assignmentTechnicalSpecilaistValidationService,
                                    IAssignmentTechnicalSpecialistScheduleValidationService assignmentTechnicalSpecialistScheduleValidationService,
                                    IAssignmentAdditionalExpenseValidationService assignmentAdditionalExpenseValidationService,
                                    IAssignmentContractRateScheduleValidationService assignmentContractRateScheduleValidationService,
                                    IMasterRepository masterRepository,
                                    IAuditLogger auditLogger
                                    )
        {
            _assignmentRepository = assignmentRepository;
            _assignmentValidationService = assignmentValidationService;
            _numberSequenceRepository = numberSequenceRepository;
            _contractRepository = contractRepository;
            _projectRepository = projectRepository;
            _companyRepository = companyRepository;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _taxonomyRepository = taxonomyRepository;
            _assignmentReferenceRepository = assignmentReferenceRepository;
            _assignmentTaxonomyRepository = assignmentTaxonomyRepository;
            _assignmentContractRateScheduleRepository = assignmentContractRateScheduleRepository;
            _assignmentTechnicalSpecialistRepository = assignmentTechnicalSpecialistRepository;
            _assignmentTechnicalSpecialistScheduleRepository = assignmentTechnicalSpecialistScheduleRepository;
            _assignmentSubSupplierRepository = assignmentSubSupplierRepository;
            _assignmentSubSupplerTsRepository = assignmentSubSupplerTsRepository;
            _assignmentNoteRepository = assignmentNoteRepository;
            _assignmentMessageRepository = assignmentMessageRepository;
            _assignmentContributionCalculationRepository = assignmentContributionCalculationRepository;
            _revenueCostRepository = revenueCostRepository;
            _assignmentAdditionalExpenseRepository = assignmentAdditionalExpenseRepository;
            _assignmentInterCompanyDiscount = assignmentInterCompanyDiscount;
            _supplierPoSubSupplierRepository = supplierPoSubSupplierRepository;
            _tsCalendarRepository = tsCalendarRepository;
            _timesheetRepository = timesheetRepository;
            _timesheetAccountItemTimeRepository = timesheetAccountItemTimeRepository;
            _timeAccountItemConsumableRepository = timeAccountItemConsumableRepository;
            _timeAccountItemExpenseRepository = timeAccountItemExpenseRepository;
            _timeAccountItemTravelRepository = timeAccountItemTravelRepository;
            _timesheetInterCompanyRepository = timesheetInterCompanyRepository;
            _visitRepository = visitRepository;
            _visitAccountItemConsumableRepository = visitAccountItemConsumableRepository;
            _visitAccountItemExpenseRepository = visitAccountItemExpenseRepository;
            _visitAccountItemTravelRepository = visitAccountItemTravelRepository;
            _visitAccountItemTimeRepository = visitAccountItemTimeRepository;
            _visitInterCompanyRepository = visitInterCompanyRepository;
            _documentService = documentService;
            _auditSearchService = auditSearchService;
            _resourceSearchService = resourceSearchService;
            _currencyExchangeRateService = currencyExchangeRateService;
            _exchangeRateRepository = exchangeRateRepository;
            _technicalSpecialistContactRepository = technicalSpecialistContactRepository;
            _logger = logger;
            _mapper = mapper;
            _emailService = emailService;
            _userService = userService;
            _messageDescriptions = messageDescriptions;
            _environment = environment.Value;
            _assignmentTaxonomyValidationService = assignmentTaxonomyValidationService;
            _assignmentSubSupplierValidationService = assignmentSubSupplierValidationService;
            _assignmentSubSupplierTSValidationService = assignmentSubSupplierTSValidationService;
            _assignmentNoteValidationService = assignmentNoteValidationService;
            _assignmentContributionCalculationValidationService = assignmentContributionCalculationValidationService;
            _assignmentReferenceTypeValidationService = assignmentReferenceTypeValidationService;
            _assignmentTechnicalSpecilaistValidationService = assignmentTechnicalSpecilaistValidationService;
            _assignmentTechnicalSpecialistScheduleValidationService = assignmentTechnicalSpecialistScheduleValidationService;
            _assignmentAdditionalExpenseValidationService = assignmentAdditionalExpenseValidationService;
            _assignmentContractRateScheduleValidationService = assignmentContractRateScheduleValidationService;
            _masterRepository = masterRepository;
            _auditLogger = auditLogger;
        }

        public Response Add(DomainModel.AssignmentDetail assignmentDetails, bool IsAPIValidationRequired = false)
        {
            Exception exception = null;
            string actionByUser = string.Empty;
            List<DbModel.Document> dbDocuments = null;
            try
            {
                DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection = null;
                using (assignmentDatabaseCollection = new DomainModel.AssignmentModuleDatabaseCollection())
                {
                    if (assignmentDetails != null && assignmentDetails.AssignmentInfo != null && assignmentDetails.AssignmentInfo.RecordStatus.IsRecordStatusNew())
                    {
                        assignmentDatabaseCollection = MasterData(assignmentDetails, assignmentDatabaseCollection);
                        var assignment = new List<DomainModel.Assignment>
                        {
                            assignmentDetails.AssignmentInfo
                        };
                        var response = IsRecordValidForProcess(assignment, ValidationType.Add, ref assignmentDatabaseCollection, IsAPIValidationRequired);
                        if (response.Code == ResponseType.Success.ToId())
                        {
                            using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                            {
                                _assignmentRepository.AutoSave = false;
                                response = ProcessAssignment(assignmentDetails, assignmentDatabaseCollection, ValidationType.Add);
                                if (response.Code == ResponseType.Success.ToId())
                                {
                                    response = Validate(assignmentDetails, ValidationType.Add, ref assignmentDatabaseCollection, response, IsAPIValidationRequired);
                                    if (response.Code == ResponseType.Success.ToId() && response.ValidationMessages?.Count == 0)
                                    {
                                        if (assignmentDetails.AssignmentInfo.AssignmentId != null)
                                            response = ProcessAssignmentDetail(assignmentDetails, assignmentDatabaseCollection, ValidationType.Add, ref dbDocuments);
                                        if (response.Code == ResponseType.Success.ToId())
                                        { 
                                            ProcessVisitTimesheetSkeletonDefaultLineItems(assignmentDetails, assignmentDatabaseCollection);
                                            int saveCount = _assignmentRepository.ForceSave();
                                            if (assignmentDatabaseCollection.DBAssignment?.Count > 0)
                                            {
                                                IList<ValidationMessage> validationMessages = null;
                                                if (assignmentDetails.AssignmentInfo?.PreAssignmentId != null && assignmentDetails.AssignmentInfo?.PreAssignmentId > 0)
                                                    _resourceSearchService.ChangeStatusOfPreAssignment(new List<int?> { assignmentDetails.AssignmentInfo?.PreAssignmentId }, assignmentDatabaseCollection.DBAssignment?.FirstOrDefault().Id, assignmentDetails.AssignmentInfo?.ModifiedBy, ref validationMessages);
                                            }
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
                            var assignmentInformaton = assignmentDatabaseCollection?.DBAssignment;
                            assignmentDetails.AssignmentInfo.AssignmentNumber = _numberSequenceRepository.FindBy(x => x.ModuleData == (int)assignmentDetails.AssignmentInfo.AssignmentProjectNumber && x.ModuleId == 4)?.FirstOrDefault().LastSequenceNumber;
                            if (assignmentInformaton != null && assignmentInformaton.Count > 0)
                            {
                                bool? isEformReportRequired = assignmentInformaton[0].IsEformReportRequired;
                                assignmentDetails.AssignmentInfo.AssignmentCreatedDate = assignmentInformaton[0].CreatedDate;
                                assignmentDetails.AssignmentInfo.VisitStatus = assignmentInformaton[0].FirstVisitTimesheetStatus;
                                assignmentDetails.AssignmentInfo.IsEFormReportRequired = isEformReportRequired.HasValue && isEformReportRequired.Value == true ? 1 : 0;
                            }
                            if (assignmentDetails.ResourceSearch?.SearchParameter != null) // Def 978 PLO issue
                            {
                                assignmentDetails.ResourceSearch.AssignmentId = assignmentDatabaseCollection.DBAssignment?.FirstOrDefault().Id;
                                this.ProcessArsSearch(assignmentDetails.ResourceSearch, ref assignmentDatabaseCollection);
                            }
                            ProcessHistory(assignmentDetails, assignmentDatabaseCollection);
                            string token = DateTime.Now.ToString(VisitTimesheetConstants.TOKEN_DATE_FORMAT);
                            ResourceAssignmentEmailNotification(assignmentDatabaseCollection.DBAssignment, assignmentDetails, assignmentDetails.AssignmentInfo.AssignmentNumber, token);
                            if (assignmentDetails?.AssignmentInfo.AssignmentOperatingCompanyCode != assignmentDetails?.AssignmentInfo.AssignmentContractHoldingCompanyCode)
                            {
                                ModuleDocument moduleDocument = new ModuleDocument();
                                ProcessEmailNotifications(assignmentDatabaseCollection, assignmentDetails, assignmentDetails.AssignmentInfo.AssignmentNumber, ref moduleDocument, token);
                                if (!string.IsNullOrEmpty(assignmentDetails.AssignmentInterCompanyDiscounts.AmendmentReason))
                                {
                                    IList<DomainModel.AssignmentDetail> originalAssignment = null;
                                    if (originalAssignment == null)
                                        originalAssignment = new List<DomainModel.AssignmentDetail>();
                                    assignmentDatabaseCollection.DBAssignment.ToList().ForEach(x =>
                                    {
                                        originalAssignment.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.AssignmentDetail>(x)));
                                    });
                                    ProcessEmailForAmendmentReasonForNewAssignment(assignmentDatabaseCollection, assignmentDetails, assignmentDetails.AssignmentInfo.AssignmentNumber, ref moduleDocument, DateTime.Now.ToString(AssignmentConstants.TOKEN_DATE_FORMAT), originalAssignment);
                                }
                                if (assignmentDetails.AssignmentDocuments == null)
                                    assignmentDetails.AssignmentDocuments = new List<ModuleDocument>();
                                assignmentDetails.AssignmentDocuments.Add(moduleDocument);
                            }
                            ProcessAddAudit(assignmentDetails, assignmentDatabaseCollection, dbDocuments);
                            if (assignmentDetails.AssignmentInfo?.PreAssignmentId != null)
                            {
                                ChangePreAssignmentCalendarStatus(assignmentDetails.AssignmentInfo.PreAssignmentId);
                            }
                        }
                        else
                            return response;
                    }
                    else if (assignmentDetails == null || assignmentDetails?.AssignmentInfo == null)
                    {
                        var message = new List<ValidationMessage>
                        {
                            { _messageDescriptions, assignmentDetails, MessageType.InvalidPayLoad, assignmentDetails }
                        };
                        return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentDetails);
            }
            finally
            {
                _assignmentRepository.AutoSave = true;
                _assignmentRepository.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, assignmentDetails?.AssignmentInfo?.AssignmentId, exception, null);
        }

        private void ProcessHistory(DomainModel.AssignmentDetail assignmentDetails, DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection)
        {
            string assignmentType = (assignmentDetails.AssignmentInfo.AssignmentContractHoldingCompanyCode == assignmentDetails.AssignmentInfo.AssignmentOperatingCompanyCode
                                                                       ? AssignmentConstants.CONTRACT_HOLDER_CREATES_ASSIGNMENT : AssignmentConstants.COTRACT_HOLDER_CREATES_INTER_COMPANY_ASSIGNMENT);

            AddAssignmentHistory(assignmentDetails.AssignmentInfo.AssignmentId ?? 0, assignmentDatabaseCollection?.Assignment?.DBMasterData?.Where(x => x.MasterDataTypeId == (int)MasterType.HistoryTable && x.Code == assignmentType)?.ToList(), assignmentDetails.AssignmentInfo.ActionByUser);

        }

        private void ProcessAddAudit(DomainModel.AssignmentDetail assignmentDetails, DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, List<DbModel.Document> dbDocuments)
        {
            long? eventID = 0;
            IList<DomainModel.AssignmentDetail> originalAssignment = new List<DomainModel.AssignmentDetail>();
            assignmentDatabaseCollection.DBAssignment.ToList().ForEach(x =>
            {
                originalAssignment.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.AssignmentDetail>(x)));
            });
            DomainModel.AssignmentDetail auditAssignmentDetails = ObjectExtension.Clone(assignmentDetails);
            assignmentDatabaseCollection.dbModule = _auditSearchService.GetAuditModule(new List<string>() {
                                                                                    SqlAuditModuleType.Assignment.ToString(),
                                                                                    SqlAuditModuleType.AssignmentAdditionalExpense.ToString(),
                                                                                    SqlAuditModuleType.AssignmentContractSchedule.ToString(),
                                                                                    SqlAuditModuleType.AssignmentContributionCalculation.ToString(),
                                                                                    SqlAuditModuleType.AssignmentContributionRenuveCost.ToString(),
                                                                                    SqlAuditModuleType.AssignmentDocument.ToString(),
                                                                                    SqlAuditModuleType.AssignmentInstructions.ToString(),
                                                                                    SqlAuditModuleType.AssignmentInterCo.ToString(),
                                                                                    SqlAuditModuleType.AssignmentNote.ToString(),
                                                                                    SqlAuditModuleType.AssignmentReference.ToString(),
                                                                                    SqlAuditModuleType.AssignmentSpecialistSubSupplier.ToString(),
                                                                                    SqlAuditModuleType.AssignmentSubSupplier.ToString(),
                                                                                    SqlAuditModuleType.AssignmentTechnicalSpecialist.ToString(),
                                                                                    SqlAuditModuleType.AssignmentTaxonomy.ToString(),
                                                                                    SqlAuditModuleType.AssignmentTechnicalSchedule.ToString(),
                                                                                    SqlAuditModuleType.Timesheet.ToString(),
                                                                                    SqlAuditModuleType.TimesheetReference.ToString(),
                                                                                    SqlAuditModuleType.TimesheetTechnicalSpecialist.ToString(),
                                                                                    SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemConsumable.ToString(),
                                                                                    SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTime.ToString(),
                                                                                    SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemExpense.ToString(),
                                                                                    SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTravel.ToString(),
                                                                                    SqlAuditModuleType.TimesheetInterCompanyDiscount.ToString(),
                                                                                    SqlAuditModuleType.Visit.ToString(),
                                                                                    SqlAuditModuleType.VisitReference.ToString(),
                                                                                    SqlAuditModuleType.VisitSpecialistAccount.ToString(),
                                                                                    SqlAuditModuleType.VisitTechnicalSpecialistAccountItemConsumable.ToString(),
                                                                                    SqlAuditModuleType.VisitTechnicalSpecialistAccountItemExpense.ToString(),
                                                                                    SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTime.ToString(),
                                                                                    SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTravel.ToString(),
                                                                                    SqlAuditModuleType.VisitInterCompanyDiscount.ToString(),
                                                                                    SqlAuditModuleType.ResourceSearch.ToString()
                                                                                    });
            if (auditAssignmentDetails != null)
                assignmentDatabaseCollection.DBAssignment.ToList().ForEach(x =>
                                                                        AuditLog(auditAssignmentDetails, x,
                                                                        assignmentDatabaseCollection,
                                                                        assignmentDatabaseCollection?.Assignment?.DBProjects?.FirstOrDefault(),
                                                                        ValidationType.Add.ToAuditActionType(),
                                                                        SqlAuditModuleType.Assignment,
                                                                        null,
                                                                        null,
                                                                        ref eventID, originalAssignment.FirstOrDefault(),
                                                                        dbDocuments));
        }

        public Response Modify(DomainModel.AssignmentDetail assignmentDetails, bool IsAPIValidationRequired = false)
        {
            IList<ValidationMessage> validationMessages = null;
            IList<DomainModel.AssignmentDetail> originalAssignment = null;
            long? eventID = 0;
            try
            {
                DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection = null;
                using (assignmentDatabaseCollection = new DomainModel.AssignmentModuleDatabaseCollection())
                {
                    if (assignmentDetails != null && assignmentDetails.AssignmentInfo != null && assignmentDetails.AssignmentInfo.RecordStatus.IsRecordStatusModified())
                    {
                        assignmentDatabaseCollection = MasterData(assignmentDetails, assignmentDatabaseCollection);
                        var assignment = new List<DomainModel.Assignment>
                        {
                            assignmentDetails.AssignmentInfo
                        };
                        assignmentDetails.AssignmentInfo.IsOverrideOrPLOForPage = assignmentDetails.AssignmentInfo.IsOverrideOrPLO;
                        var assignedChargeSchedule = assignmentDetails?.AssignmentTechnicalSpecialists?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules)?.Where(x1 => x1.RecordStatus.IsRecordStatusNew())?.ToList();
                        if (assignedChargeSchedule?.Count > 0 && assignmentDetails.AssignmentInfo.IsOverrideOrPLO)
                            assignmentDetails.AssignmentInfo.IsOverrideOrPLO = false;

                        LoadAssignmentChild(assignmentDetails, assignment, ref assignmentDatabaseCollection, ref validationMessages);
                        if (validationMessages?.Count == 0)
                        {
                            var response = IsRecordValidForProcess(assignment, ValidationType.Update, ref assignmentDatabaseCollection, IsAPIValidationRequired);
                            if (originalAssignment == null)
                                originalAssignment = new List<DomainModel.AssignmentDetail>();
                            assignmentDatabaseCollection.DBAssignment.ToList().ForEach(x =>
                            {
                                originalAssignment.Add(ObjectExtension.Clone(_mapper.Map<DomainModel.AssignmentDetail>(x)));
                                originalAssignment[0].AssignmentInfo.IsInternalAssignment = (bool)assignmentDatabaseCollection.DBAssignment[0].IsInternalAssignment;
                            });
                            string token = DateTime.Now.ToString(AssignmentConstants.TOKEN_DATE_FORMAT);
                            if (response.Code == ResponseType.Success.ToId() && response.ValidationMessages?.Count == 0 && validationMessages?.Count == 0)
                            {   
                                //To-Do: Will create helper method get TransactionScope instance based on requirement
                                using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                                {
                                    _assignmentRepository.AutoSave = false;
                                    response = ProcessAssignment(assignmentDetails, assignmentDatabaseCollection, ValidationType.Update);
                                    if (response.Code == ResponseType.Success.ToId())
                                    {
                                        response = Validate(assignmentDetails, ValidationType.Update, ref assignmentDatabaseCollection, response);
                                        if (response.Code == ResponseType.Success.ToId() && response.ValidationMessages?.Count == 0)
                                        {
                                            List<DbModel.Document> dbDocuments = null;
                                            var auditAssignmentDetails = ObjectExtension.Clone(assignmentDetails);
                                            if (assignmentDetails.AssignmentInfo.AssignmentId != null)
                                            {
                                                response = ProcessAssignmentDetail(assignmentDetails, assignmentDatabaseCollection, ValidationType.Update, ref dbDocuments);
                                            }
                                            if (response.Code == ResponseType.Success.ToId())
                                            {
                                                ProcessVisitTimesheetSkeletonDefaultLineItems(assignmentDetails, assignmentDatabaseCollection);
                                                
                                                var saveCount = _assignmentRepository.ForceSave();

                                                if (assignmentDetails.AssignmentInterCompanyDiscounts != null)
                                                {
                                                    int assignmentid = Convert.ToInt32(assignmentDetails.AssignmentInterCompanyDiscounts.AssignmentId);
                                                    getvisitidbyusingAssignmentID(Convert.ToInt32(assignmentDetails.AssignmentInterCompanyDiscounts.AssignmentId), assignmentDetails);
                                                    ModuleDocument moduleDocument = new ModuleDocument();
                                                    ProcessEmailNotificationsForAmendmentReason(assignmentDatabaseCollection, assignmentDetails, assignmentDetails.AssignmentInfo.AssignmentNumber, ref moduleDocument, token, originalAssignment);

                                                }

                                                if (assignmentDetails.AssignmentInfo?.PreAssignmentId != null && assignmentDetails.AssignmentInfo?.PreAssignmentId > 0)
                                                    _resourceSearchService.ChangeStatusOfPreAssignment(new List<int?> { assignmentDetails.AssignmentInfo?.PreAssignmentId }, assignmentDetails.AssignmentInfo?.AssignmentId, assignmentDetails.AssignmentInfo?.ModifiedBy, ref validationMessages);
                                                if (assignmentDetails.ResourceSearch?.SearchParameter != null) // Def 978 PLO issue
                                                {
                                                    assignmentDetails.ResourceSearch.AssignmentId = assignmentDatabaseCollection.DBAssignment?.FirstOrDefault().Id;
                                                    this.ProcessArsSearch(assignmentDetails.ResourceSearch, ref assignmentDatabaseCollection);
                                                }
                                                   //string token = DateTime.Now.ToString(VisitTimesheetConstants.TOKEN_DATE_FORMAT);
                                                 ResourceAssignmentEmailNotification(assignmentDatabaseCollection.DBAssignment, assignmentDetails, assignmentDetails.AssignmentInfo?.AssignmentNumber, token);
                                               
                                                if (assignmentDatabaseCollection.DBAssignment?.Count > 0)
                                                {
                                                    assignmentDatabaseCollection.dbModule = _auditSearchService.GetAuditModule(new List<string>() {
                                                                                                                    SqlAuditModuleType.Assignment.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentAdditionalExpense.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentContractSchedule.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentContributionCalculation.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentContributionRenuveCost.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentDocument.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentInstructions.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentInterCo.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentNote.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentReference.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentSpecialistSubSupplier.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentSubSupplier.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentTechnicalSpecialist.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentTaxonomy.ToString(),
                                                                                                                    SqlAuditModuleType.AssignmentTechnicalSchedule.ToString(),
                                                                                                                    SqlAuditModuleType.Timesheet.ToString(),
                                                                                                                    SqlAuditModuleType.TimesheetReference.ToString(),
                                                                                                                    SqlAuditModuleType.TimesheetTechnicalSpecialist.ToString(),
                                                                                                                    SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemConsumable.ToString(),
                                                                                                                    SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTime.ToString(),
                                                                                                                    SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemExpense.ToString(),
                                                                                                                    SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTravel.ToString(),
                                                                                                                    SqlAuditModuleType.TimesheetInterCompanyDiscount.ToString(),
                                                                                                                    SqlAuditModuleType.Visit.ToString(),
                                                                                                                    SqlAuditModuleType.VisitReference.ToString(),
                                                                                                                    SqlAuditModuleType.VisitSpecialistAccount.ToString(),
                                                                                                                    SqlAuditModuleType.VisitTechnicalSpecialistAccountItemConsumable.ToString(),
                                                                                                                    SqlAuditModuleType.VisitTechnicalSpecialistAccountItemExpense.ToString(),
                                                                                                                    SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTime.ToString(),
                                                                                                                    SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTravel.ToString(),
                                                                                                                    SqlAuditModuleType.VisitInterCompanyDiscount.ToString(),
                                                                                                                    SqlAuditModuleType.ResourceSearch.ToString()
                                                                                                                    });
                                                    assignmentDatabaseCollection.DBAssignment.ToList().ForEach(x =>
                                                                                     AuditLog(auditAssignmentDetails,
                                                                                     x,
                                                                                     assignmentDatabaseCollection,
                                                                                     assignmentDatabaseCollection?.Assignment?.DBProjects?.FirstOrDefault(),
                                                                                     ValidationType.Update.ToAuditActionType(),
                                                                                     SqlAuditModuleType.Assignment,
                                                                                     null,
                                                                                     null,
                                                                                     ref eventID, originalAssignment.FirstOrDefault(),
                                                                                     dbDocuments));
                                                }
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
                                if (assignmentDetails.AssignmentInfo?.PreAssignmentId != null)
                                {
                                    ChangePreAssignmentCalendarStatus(assignmentDetails.AssignmentInfo.PreAssignmentId);
                                }
                            }
                            else
                                return response;
                        }
                    }
                    else if (assignmentDetails == null || assignmentDetails?.AssignmentInfo == null)
                    {
                        Exception exception = null;
                        var message = new List<ValidationMessage>
                            {
                                { _messageDescriptions, assignmentDetails, MessageType.InvalidPayLoad, assignmentDetails }
                            };
                        return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentDetails);
            }
            finally
            {
                _assignmentRepository.AutoSave = true;
                _assignmentRepository.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), assignmentDetails?.AssignmentInfo?.AssignmentId, null);
        }

        public void getvisitidbyusingAssignmentID(int assignmentid, DomainModel.AssignmentDetail assignmentDetails)
        {
            List<DbModel.Visit> visitInfo = _visitRepository.GetAssignmentVisitIds(assignmentid);
            DomainModelVisit.Visit visit = new DomainModelVisit.Visit();
            DbModel.VisitInterCompanyDiscount dbVisitInterCompanyDiscount = null;
            for (var i = 0; i < visitInfo.Count; i++)
            {
                int VisitId = Convert.ToInt32(visitInfo[i].Id);
                DbModel.Visit visitdata = visitInfo[i];
                visit = _visitRepository.GetVisitByID1(visitdata);
                if (visit.VisitStatus.Equals("C") || visit.VisitStatus.Equals("U") || visit.VisitStatus.Equals("Q") || visit.VisitStatus.Equals("T"))
                {
                    long visitId = VisitId;
                    dbVisitInterCompanyDiscount = new DbModel.VisitInterCompanyDiscount
                    {
                      //  Id = 1108999,
                        VisitId = visitId,
                        Percentage = (decimal)assignmentDetails.AssignmentInterCompanyDiscounts.AssignmentContractHoldingCompanyDiscount,
                        Description = assignmentDetails.AssignmentInterCompanyDiscounts.AssignmentContractHoldingCompanyDescription.ToString()
                    };
                    /* _visitInterCompanyRepository.AutoSave = false;
                     _visitInterCompanyRepository.Update(dbVisitInterCompanyDiscount);
                     if (commitChange)
                     {
                         _visitInterCompanyRepository.ForceSave();
                     }*/
                    _visitInterCompanyRepository.UpdateVisitIntercompanyDiscount(dbVisitInterCompanyDiscount);
                }
            }
        }

        private void ChangePreAssignmentCalendarStatus(long? preAssignmentID)
        {
            var techSpecialListCal = this._tsCalendarRepository?.FindBy(x => x.CalendarRefCode == preAssignmentID
                                            && x.CalendarType == CalendarType.PRE.ToString()).ToList();
            _tsCalendarRepository.AutoSave = false;
            if (techSpecialListCal?.Count() > 0)
            {
                foreach (var tsList in techSpecialListCal)
                {
                    tsList.IsActive = false;
                    this._tsCalendarRepository.Update(tsList, x => x.IsActive);
                    _tsCalendarRepository.ForceSave();
                }
            }
        }

        public Response Delete(DomainModel.AssignmentDetail assignmentDetails, bool IsAPIValidationRequired = false)
        {
            Response response = null;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            Exception exception = null;
            List<ValidationMessage> messages = new List<ValidationMessage>();//MS-TS
            try
            {
                if (assignmentDetails != null && assignmentDetails.AssignmentInfo != null && assignmentDetails.AssignmentInfo.RecordStatus.IsRecordStatusDeleted())
                {
                    DomainModel.AssignmentDetail assignmentDeleteDetail = new DomainModel.AssignmentDetail();
                    DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection = new DomainModel.AssignmentModuleDatabaseCollection();
                    IList<DomainModel.Assignment> assignment = new List<DomainModel.Assignment>() { assignmentDetails.AssignmentInfo };
                    assignmentDeleteDetail.AssignmentInfo = assignmentDetails.AssignmentInfo;
                    assignmentDeleteDetail.AssignmentSubSuppliers = assignmentDetails.AssignmentSubSuppliers;

                    LoadAssignmentChild(assignmentDeleteDetail, assignment, ref assignmentDatabaseCollection, ref validationMessages);

                    //Defect Id-580 Fixed(moved sub-supplier validation up)
                    if (assignmentDatabaseCollection.DBAssignment != null && validationMessages?.Count == 0)
                    {
                        if (assignmentDatabaseCollection.DBAssignmentSubSupplier != null) ///this is null for Timesheets
                        {
                            assignmentDatabaseCollection.DBAssignmentSubSupplier.ToList().Where(x => x.SupplierContactId != null
                            && assignment.FirstOrDefault()?.AssignmentId == x.AssignmentId && x.SupplierType != SupplierType.MainSupplier.FirstChar() //MS-TS
                                    ).FirstOrDefault(x =>
                                    {
                                        validationMessages.Add(_messageDescriptions, x.Id, MessageType.AssignmentIsBeingUsed, "sub suppliers");
                                        return false;
                                    });
                        }
                        //MS-TS

                        if (validationMessages?.Count == 0 && assignmentDetails.AssignmentInfo.AssignmentProjectWorkFlow.IsVisitEntry())
                            assignmentDatabaseCollection.DBAssignment.ToList()?.Where(x => x.Visit?.Count > 0 || x.VisitTechnicalSpecialistAccountItemConsumable?.Count > 0
                                                || x.VisitTechnicalSpecialistAccountItemExpense?.Count > 0
                                                || x.VisitTechnicalSpecialistAccountItemTime?.Count > 0
                                                || x.VisitTechnicalSpecialistAccountItemTravel?.Count > 0
                                                ).ToList().ForEach(x =>
                                                {
                                                    validationMessages.Add(_messageDescriptions, x.Id, MessageType.AssignmentIsBeingUsed, "visits");
                                                });
                        else if (validationMessages?.Count == 0)
                        {
                            assignmentDatabaseCollection.DBAssignment.ToList()?.Where(x => x.Timesheet?.Count > 0 || x.TimesheetTechnicalSpecialistAccountItemConsumable?.Count > 0
                                            || x.TimesheetTechnicalSpecialistAccountItemExpense?.Count > 0
                                            || x.TimesheetTechnicalSpecialistAccountItemTime?.Count > 0
                                            || x.TimesheetTechnicalSpecialistAccountItemTravel?.Count > 0
                        ).ToList().ForEach(x =>
                        {
                            validationMessages.Add(_messageDescriptions, x.Id, MessageType.AssignmentIsBeingUsed, "timesheets");
                        });
                        }

                        if (validationMessages?.Count == 0)
                        {
                            int count = 0;
                            if (assignmentDetails.AssignmentInfo.AssignmentId != null)
                            {
                                using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Snapshot }))
                                {
                                    count = _assignmentRepository.DeleteAssignment((int)assignmentDetails.AssignmentInfo.AssignmentId);
                                    tranScope.Complete();
                                }
                                if (count > 0)
                                {
                                    long? eventId = null;
                                    response = new Response(ResponseType.Success.ToId(), ResponseType.Success.ToString());
                                    assignmentDatabaseCollection.dbModule = _auditSearchService.GetAuditModule(new List<string>() { SqlAuditModuleType.Assignment.ToString() });
                                    _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), "{" + AuditSelectType.Id + ":" + assignmentDetails.AssignmentInfo.AssignmentId
                                                             + "}${" + AuditSelectType.ProjectNumber + ":" + assignmentDetails.AssignmentInfo.AssignmentProjectNumber
                                                             + "}${" + AuditSelectType.ProjectAssignment + ":" + assignmentDetails.AssignmentInfo.AssignmentProjectNumber + "-" + assignmentDetails.AssignmentInfo.AssignmentNumber + "}", SqlAuditActionType.D, SqlAuditModuleType.Assignment, assignmentDetails.AssignmentInfo, null, assignmentDatabaseCollection.dbModule);
                                }
                            }
                        }
                        else
                            return new Response(ResponseType.Validation.ToId(), null, null, validationMessages?.ToList(), null);
                    }
                    else
                        return new Response(ResponseType.Validation.ToId(), null, null, validationMessages?.ToList(), null);
                }
                else
                {
                    var message = new List<ValidationMessage>
                    {
                        { _messageDescriptions, assignmentDetails, MessageType.InvalidPayLoad, assignmentDetails }
                    };
                    return new Response().ToPopulate(ResponseType.Warning, null, null, message, null, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentDetails);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, assignmentDetails?.AssignmentInfo?.AssignmentId, exception, null);
        }

        private DomainModel.AssignmentModuleDatabaseCollection MasterData(DomainModel.AssignmentDetail assignmentDetails, DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection)
        {
            try
            {
                if (assignmentDetails != null)
                {
                    assignmentDatabaseCollection.Assignment = assignmentDatabaseCollection.Assignment ?? new DomainModel.AssignmentDatabaseCollection();

                    var assignmentInfo = new List<string>() { assignmentDetails.AssignmentInfo.AssignmentLifecycle, assignmentDetails.AssignmentInfo.AssignmentReviewAndModerationProcess };
                    var assignmentStat = new List<string>() { assignmentDetails.AssignmentInfo.AssignmentStatus, assignmentDetails.AssignmentInfo.AssignmentType };

                    var masterData = new List<string>[] { assignmentDetails.AssignmentAdditionalExpenses?.Where(x => !string.IsNullOrEmpty(x.ExpenseType))?.Select(x => x.ExpenseType).Distinct().ToList(),
                                                               assignmentDetails.AssignmentReferences?.Where(x => !string.IsNullOrEmpty(x.ReferenceType))?.Select(x => x.ReferenceType).Distinct().ToList(),
                                                               assignmentInfo }.Where(x => x?.Count > 0).SelectMany(x => x).ToList();

                    string type = assignmentDetails.AssignmentInfo.AssignmentContractHoldingCompanyCode == assignmentDetails.AssignmentInfo.AssignmentOperatingCompanyCode
                                                                            ? AssignmentConstants.CONTRACT_HOLDER_CREATES_ASSIGNMENT : AssignmentConstants.COTRACT_HOLDER_CREATES_INTER_COMPANY_ASSIGNMENT;

                    assignmentDatabaseCollection.Assignment.DBMasterData = _assignmentRepository.GetMasterData(masterData, assignmentStat, new List<int>() { (int)MasterType.ExpenseType, (int)MasterType.AssignmentReferenceType, (int)MasterType.AssignmentType,
                                                       (int)MasterType.AssignmentStatus, (int)MasterType.AssignmentLifeCycle, (int)MasterType.ReviewAndModerationProcess,(int)MasterType.HistoryTable }, new List<string>() { type, AssignmentConstants.ASSIGNMENT_SPECIALIST_ADDED });

                    assignmentDatabaseCollection.DBReferenceType = assignmentDatabaseCollection.Assignment.DBMasterData.Where(x => x.MasterDataTypeId == (int)MasterType.AssignmentReferenceType).ToList();
                    assignmentDatabaseCollection.DBExpenseType = assignmentDatabaseCollection.Assignment.DBMasterData.Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType).ToList();
                    if (assignmentDetails.AssignmentInterCompanyDiscounts != null)
                        assignmentDetails.AssignmentInfo.InterCompCodes = new List<string> {
                                                                                    assignmentDetails.AssignmentInterCompanyDiscounts.AssignmentAdditionalIntercompany1_Code,
                                                                                    assignmentDetails.AssignmentInterCompanyDiscounts.AssignmentAdditionalIntercompany2_Code,
                                                                                    assignmentDetails.AssignmentInterCompanyDiscounts.AssignmentContractHoldingCompanyCode,
                                                                                    assignmentDetails.AssignmentInterCompanyDiscounts.AssignmentHostcompanyCode,
                                                                                    assignmentDetails.AssignmentInterCompanyDiscounts.AssignmentOperatingCompanyCode,
                                                                                    assignmentDetails.AssignmentInterCompanyDiscounts.ParentContractHoldingCompanyCode}
                                                                                            .Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();

                    assignmentDatabaseCollection.dbLineItemExpense = assignmentDatabaseCollection.Assignment?.DBMasterData?.ToList()?.Where(x => x.MasterDataTypeId == (int)MasterType.ExpenseType)?.ToList();

                    assignmentDatabaseCollection.Assignment.DBAssignmentStatus = assignmentDatabaseCollection.Assignment.DBMasterData.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.AssignmentStatus).ToList();
                    assignmentDatabaseCollection.Assignment.DBAssignmentType = assignmentDatabaseCollection.Assignment.DBMasterData.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.AssignmentType).ToList(); ;
                    assignmentDatabaseCollection.Assignment.DBAssignmentLifeCycle = assignmentDatabaseCollection.Assignment.DBMasterData.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.AssignmentLifeCycle).ToList();
                    assignmentDatabaseCollection.Assignment.DBAssignmentReviewAndModeration = assignmentDatabaseCollection.Assignment.DBMasterData.ToList().Where(x => x.MasterDataTypeId == (int)MasterType.ReviewAndModerationProcess)?.ToList();


                    GetTechnicalSpecialists(assignmentDetails.AssignmentTechnicalSpecialists, ref assignmentDatabaseCollection);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentDetails);
            }
            return assignmentDatabaseCollection;
        }

        private void LoadAssignmentChild(DomainModel.AssignmentDetail assignmentDetails, IList<DomainModel.Assignment> assignments, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            List<string> include = new List<string>();
            IList<int> assignmentId = assignments.Where(x => x.AssignmentId > 0).Select(x => (int)x.AssignmentId).ToList();

            if (assignmentDetails.AssignmentAdditionalExpenses != null && assignmentDetails.AssignmentAdditionalExpenses?.Count > 0) include.Add("AssignmentAdditionalExpense.ExpenseType");
            if (assignmentDetails.AssignmentReferences != null && assignmentDetails.AssignmentReferences?.Count > 0) include.Add("AssignmentReferenceNavigation.AssignmentReferenceType");
            if (assignmentDetails.AssignmentTaxonomy != null && assignmentDetails.AssignmentTaxonomy?.Count > 0) include.Add("AssignmentTaxonomy.TaxonomyService.TaxonomySubCategory.TaxonomyCategory");
            if (assignmentDetails.AssignmentContractSchedules != null && assignmentDetails.AssignmentContractSchedules?.Count > 0) { include.Add("AssignmentContractSchedule"); include.Add("Project.Contract.ContractSchedule"); };
            if (assignmentDetails.AssignmentInterCompanyDiscounts != null) include.Add("AssignmentInterCompanyDiscount");
            if (assignmentDetails.AssignmentContributionCalculators != null && assignmentDetails.AssignmentContributionCalculators?.Count > 0) include.Add("AssignmentContributionCalculation.AssignmentContributionRevenueCost");
            if (assignmentDetails.AssignmentSubSuppliers != null && assignmentDetails.AssignmentSubSuppliers?.Count > 0 && !assignmentDetails.AssignmentInfo.RecordStatus.IsRecordStatusDeleted())
            {
                include.Add("AssignmentSubSupplier.AssignmentSubSupplierTechnicalSpecialist");
                include.Add("SupplierPurchaseOrder.SupplierPurchaseOrderSubSupplier.Supplier.SupplierContact");
                include.Add("SupplierPurchaseOrder.Supplier");
            }
            if (assignmentDetails.AssignmentTechnicalSpecialists != null && assignmentDetails.AssignmentTechnicalSpecialists?.Count > 0) include.Add("AssignmentTechnicalSpecialist.AssignmentTechnicalSpecialistSchedule");
            if (assignmentDetails.AssignmentNotes != null && assignmentDetails.AssignmentNotes?.Count > 0) include.Add("AssignmentNote");
            include.Add(assignmentDetails.AssignmentInfo.AssignmentProjectWorkFlow.IsVisitEntry() ? "Visit" : "Timesheet");

            if (assignmentDetails.AssignmentInfo.RecordStatus.IsRecordStatusDeleted())
                IsValidAssignment(assignmentId, ref assignmentDatabaseCollection.DBAssignment, ref validationMessages, include.ToArray());
            else
            {
                string[] defaultItem ={
                                            "AssignmentMessage",
                                            "AssignmentMessage.MessageType",
                                            "Project.ProjectType",
                                            "Project.ProjectMessage",
                                            "Project.SupplierPurchaseOrder",
                                            "Project.Contract.Customer.CustomerAddress.CustomerContact",
                                            };

                string[] includes = include.ToArray().Concat(defaultItem).ToArray();
                IsValidAssignment(assignmentId, ref assignmentDatabaseCollection.DBAssignment, ref validationMessages, includes);
            }

            if (!(assignmentDatabaseCollection.DBAssignment?.Count > 0)) return;
            {
                if (assignmentDetails.AssignmentInfo.AssignmentProjectWorkFlow.IsVisitEntry())
                    assignmentDatabaseCollection.dbVisit = assignmentDatabaseCollection.DBAssignment.SelectMany(x => x.Visit).FirstOrDefault();
                else
                    assignmentDatabaseCollection.dbTimesheet = assignmentDatabaseCollection.DBAssignment.SelectMany(x => x.Timesheet).FirstOrDefault();
                if (assignmentDetails.AssignmentReferences != null && assignmentDetails.AssignmentReferences?.Count > 0)
                    assignmentDatabaseCollection.DBAssignmentReferenceTypes = assignmentDatabaseCollection.DBAssignment.SelectMany(x => x.AssignmentReferenceNavigation).ToList();
                if (assignmentDetails.AssignmentTaxonomy != null && assignmentDetails.AssignmentTaxonomy?.Count > 0)
                    assignmentDatabaseCollection.DBAssignmentTaxonomy = assignmentDatabaseCollection.DBAssignment.SelectMany(x => x.AssignmentTaxonomy).ToList();
                if (assignmentDetails.AssignmentContractSchedules != null && assignmentDetails.AssignmentContractSchedules?.Count > 0)
                    assignmentDatabaseCollection.DBAssignmentContractSchedules = assignmentDatabaseCollection.DBAssignment.SelectMany(x => x.AssignmentContractSchedule).ToList();
                if (assignmentDetails.AssignmentSubSuppliers != null && assignmentDetails.AssignmentSubSuppliers?.Count > 0)
                    assignmentDatabaseCollection.DBAssignmentSubSupplier = assignmentDatabaseCollection.DBAssignment.SelectMany(x => x.AssignmentSubSupplier).ToList();
                if (assignmentDetails.AssignmentTechnicalSpecialists != null && assignmentDetails.AssignmentTechnicalSpecialists?.Count > 0)
                    assignmentDatabaseCollection.DBAssignmentTechnicalSpecialists = assignmentDatabaseCollection.DBAssignment.SelectMany(x => x.AssignmentTechnicalSpecialist).ToList();
                if (assignmentDetails.AssignmentAdditionalExpenses != null && assignmentDetails.AssignmentAdditionalExpenses?.Count > 0)
                    assignmentDatabaseCollection.DBAssignmentAdditionalExpenses = assignmentDatabaseCollection.DBAssignment.SelectMany(x => x.AssignmentAdditionalExpense).ToList();
                if (assignmentDetails.AssignmentInterCompanyDiscounts != null)
                    assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts = assignmentDatabaseCollection.DBAssignment.SelectMany(x => x.AssignmentInterCompanyDiscount).ToList();
                if (assignmentDetails.AssignmentContributionCalculators != null && assignmentDetails.AssignmentContributionCalculators?.Count > 0)
                    assignmentDatabaseCollection.DBAssignmentContributionCalculations = assignmentDatabaseCollection.DBAssignment.SelectMany(x => x.AssignmentContributionCalculation).ToList();
                if (assignmentDetails.AssignmentNotes != null && assignmentDetails.AssignmentNotes?.Count > 0)//D661 issue 8
                    assignmentDatabaseCollection.DBAssignmentNotes = assignmentDatabaseCollection.DBAssignment.SelectMany(x => x.AssignmentNote).ToList();
            }
        }

        private List<DomainModel.Assignment> FilterRecord(List<DomainModel.Assignment> assignments, ValidationType filterType)
        {
            List<DomainModel.Assignment> filteredModules = null;

            if (filterType == ValidationType.Add)
                filteredModules = assignments?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filteredModules = assignments?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filteredModules = assignments?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filteredModules;
        }

        private bool IsValidPayload(IList<DomainModel.Assignment> assignments, ValidationType validationType, ref IList<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();

            var validationResults = _assignmentValidationService.Validate(JsonConvert.SerializeObject(assignments), validationType);
            if (assignments != null)
            {
                bool isVisit = assignments.Any(x => x.AssignmentProjectWorkFlow.IsVisitEntry());
                bool isInterCompany = assignments.Any(x => x.AssignmentContractHoldingCompanyCode != x.AssignmentOperatingCompanyCode);
                if (assignments.Any(x => x.AssignmentSupplierPurchaseOrderId == null && isVisit))
                    messages.Add(_messageDescriptions, "Supplier Po Id", MessageType.AssignmentSupplierPONumber);
                if (assignments.Any(x => string.IsNullOrEmpty(x.AssignmentReviewAndModerationProcess) && isVisit))
                    messages.Add(_messageDescriptions, "Review Moderation", MessageType.AssignmentReviewAndModeration);

                if (isVisit && !isInterCompany)
                    if (assignments.Any(x => x.VisitFromDate == null || x.VisitToDate == null))
                        messages.Add(_messageDescriptions, "Visit From To", MessageType.AssignmentFromTo);

                if (!isVisit && !isInterCompany)
                    if (assignments.Any(x => x.TimesheetFromDate == null || x.TimesheetToDate == null))
                        messages.Add(_messageDescriptions, "Timesheet From To", MessageType.AssignmentFromTo);

                if (isVisit && assignments.Any(x => x.VisitFromDate != null && x.VisitToDate != null) && assignments.Any(x => x.VisitFromDate > x.VisitToDate))
                    messages.Add(_messageDescriptions, "Visit From To", MessageType.AssignmentFromDate);

                if (!isVisit && assignments.Any(x => x.TimesheetFromDate != null && x.TimesheetToDate != null) && assignments.Any(x => x.TimesheetFromDate > x.TimesheetToDate))
                    messages.Add(_messageDescriptions, "Timesheet From To", MessageType.AssignmentFromDate);

                if (assignments.Any(x => x.WorkLocationCountry == null && !isVisit))
                    messages.Add(_messageDescriptions, "Work location Country", MessageType.AssignmentWorkCountry);
                if (assignments.Any(x => x.WorkLocationCounty == null && !isVisit))
                    messages.Add(_messageDescriptions, "Work location County", MessageType.AssignmentWorkCounty);
            }

            if (validationResults?.Count > 0)
                messages.Add(_messageDescriptions, ModuleType.Assignment, validationResults);

            return messages?.Count <= 0;
        }

        private Response AddAssignment(List<DomainModel.Assignment> assignments, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDbCollection, bool commitChange = true)
        {
            #region Declaration
            Exception exception = null;
            IList<DbModel.Project> dbProjects = null;
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.Company> dbHostCompanies = null;
            IList<DbModel.User> dbContractCoordinatorUsers = null;
            IList<DbModel.User> dbOperatingUsers = null;
            List<DbModel.Data> dbAssignmentLifeCycle = null;
            List<DbModel.Data> dbReviewAndModerations = null;
            IList<DbModel.CustomerContact> dbCustomerContacts = null;
            IList<DbModel.CustomerAddress> dbCustomerOffices = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbCounties = null;
            IList<DbModel.City> dbCities = null;
            ResponseType responseType = ResponseType.Success;
            #endregion
            try
            {
                var recordToBeAdd = FilterRecord(assignments, ValidationType.Add);
                dbProjects = assignmentDbCollection.Assignment.DBProjects;
                dbContracts = assignmentDbCollection.Assignment.DBContracts;
                dbCompanies = assignmentDbCollection.Assignment.DBCompanies;
                dbHostCompanies = assignmentDbCollection.Assignment.DBCompanies;
                dbContractCoordinatorUsers = assignmentDbCollection.Assignment.DBContractCoordinatorUsers;
                dbOperatingUsers = assignmentDbCollection.Assignment.DBOperatingUsers;
                dbAssignmentLifeCycle = assignmentDbCollection.Assignment.DBAssignmentLifeCycle;
                dbReviewAndModerations = assignmentDbCollection.Assignment.DBAssignmentReviewAndModeration;
                dbCustomerContacts = assignmentDbCollection.Assignment.DBCustomerContacts;
                dbCustomerOffices = assignmentDbCollection.Assignment.DBCustomerOffices;
                dbCountries = assignmentDbCollection.Assignment.DBCountry;
                dbCounties = assignmentDbCollection.Assignment.DBCounty;
                dbCities = assignmentDbCollection.Assignment.DBCity;
                _assignmentRepository.AutoSave = false;
                var dbRecordToBeInserted = _mapper.Map<IList<DbModel.Assignment>>(recordToBeAdd, opt =>
                {
                    opt.Items["AssignmentCompanyAddress"] = dbCustomerOffices;
                    opt.Items["AssignmentLifeCycle"] = dbAssignmentLifeCycle;
                    opt.Items["AssignmentReview"] = dbReviewAndModerations;
                    opt.Items["AssignmentCustomerContact"] = dbCustomerContacts;
                    opt.Items["AssignmentContractHoldingCompany"] = dbContracts;
                    opt.Items["AssignmentContractHoldingCoordinator"] = dbContractCoordinatorUsers;
                    opt.Items["AssignmentOperatingCompany"] = dbCompanies;
                    opt.Items["AssignmentOperatingCompanyCoordinator"] = dbOperatingUsers;
                    opt.Items["AssignmentHostCompany"] = dbCompanies;
                    opt.Items["AssignmentProjectNumber"] = dbProjects;
                    opt.Items["AssignmentWorkCountry"] = dbCountries;
                    opt.Items["AssignmentWorkCounty"] = dbCounties;
                    opt.Items["AssignmentWorkCity"] = dbCities;
                    opt.Items["AssignmentNumberSequence"] = 0;
                    opt.Items["isAssignId"] = false;
                    opt.Items["isAssignmentNumber"] = true;
                });

                _assignmentRepository.Add(dbRecordToBeInserted);
                if (commitChange)
                    _assignmentRepository.ForceSave();
                assignmentDbCollection.DBAssignment = dbRecordToBeInserted;
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignments);
            }
            finally
            {
                _assignmentRepository.AutoSave = true;
                //isInitialized = false;
            }
            return new Response().ToPopulate(responseType, null, null, null, assignmentDbCollection.DBAssignment, exception);
        }

        //Scrapped on 26-Feb-2021- On request recieved from IGO Team over a meeting call
        private List<DomainModel.AssignmentSearch> ProcessNumberSequence(int projectNumber)
        {
            List<DomainModel.AssignmentSearch> assignmentDetailList = new List<DomainModel.AssignmentSearch>();
            DbModel.NumberSequence dbNumberSequence = _numberSequenceRepository.FindBy(x => x.ModuleData == projectNumber && x.ModuleId == 4)?.FirstOrDefault();
            if (dbNumberSequence == null)
            {
                dbNumberSequence = new DbModel.NumberSequence()
                {
                    LastSequenceNumber = 0,
                    ModuleId = 4,
                    ModuleData = projectNumber,
                    ModuleRefId = 5,
                };
            }
            var dbAssigns = _assignmentRepository.FindBy(x => x.ProjectId == projectNumber && x.AssignmentNumber == 0)?.ToList();
            if (dbAssigns?.Count() > 0 && dbNumberSequence != null)
            {
                foreach (var dbAssign in dbAssigns)
                {
                    dbNumberSequence.LastSequenceNumber = dbNumberSequence.LastSequenceNumber + 1;
                    dbAssign.AssignmentNumber = dbNumberSequence.LastSequenceNumber;

                    //SaveNumberSequence(dbNumberSequence); Removed since the Number Sequence Logic is done in Trigger.
                    _assignmentRepository.Update(dbAssign, x => x.AssignmentNumber);
                    _assignmentRepository.ForceSave();

                    //We need assignment number to create audit data
                    assignmentDetailList.Add(new DomainModel.AssignmentSearch
                    {
                        AssignmentId = dbAssign.Id,
                        AssignmentNumber = dbAssign.AssignmentNumber
                    });
                }
            }
            return assignmentDetailList;
        }

        private Response SaveNumberSequence(DbModel.NumberSequence dbNumberSequence)
        {
            Exception exception = null;
            try
            {
                _numberSequenceRepository.AutoSave = false;

                if (dbNumberSequence.LastSequenceNumber == 1)
                    _numberSequenceRepository.Add(dbNumberSequence);

                if (dbNumberSequence.LastSequenceNumber > 1)
                    _numberSequenceRepository.Update(dbNumberSequence, x => x.LastSequenceNumber);

                _numberSequenceRepository.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), dbNumberSequence);
            }
            finally
            {
                _numberSequenceRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, dbNumberSequence, exception);
        }

        private Response ProcessAssignmentInfo(List<DomainModel.Assignment> assignment, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ValidationType validationType)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            DomainModel.AssignmentModuleDatabaseCollection assignmentIntDatabaseCollection = assignmentDatabaseCollection;
            try
            {
                if (assignment != null)
                {
                    if (validationType == ValidationType.Add)
                        AddAssignment(assignment, ref assignmentIntDatabaseCollection, true);
                    if (validationType == ValidationType.Update)
                        UpdateAssignment(assignment, ref assignmentDatabaseCollection, true);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignment);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private Response ProcessAssignment(DomainModel.AssignmentDetail assignmentDetail, DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection,
            ValidationType validationType)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                //using (var tranScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                //{
                
                var response = ProcessAssignmentInfo(new List<DomainModel.Assignment> { assignmentDetail.AssignmentInfo }, ref assignmentDatabaseCollection, validationType);
                if (response.Code == MessageType.Success.ToId())
                {
                    assignmentDatabaseCollection.DBARSAssignment = assignmentDatabaseCollection?.DBAssignment?.FirstOrDefault();

                    switch (validationType)
                    {
                        case ValidationType.Add when assignmentDetail.AssignmentInfo?.AssignmentContractHoldingCompanyCode == assignmentDetail.AssignmentInfo?.AssignmentOperatingCompanyCode:
                        case ValidationType.Update:
                            response = ProcessSkeletonData(assignmentDetail,
                                                assignmentDatabaseCollection,
                                                response, validationType);
                            break;
                    }
                }
                //    tranScope.Complete();
                //}
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentDetail);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private Response UpdateAssignment(List<DomainModel.Assignment> assignments, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDbCollection, bool commitChange = true)
        {
            #region Declaration
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DomainModel.Assignment> result = null;
            IList<DbModel.Project> dbProjects = null;
            IList<DbModel.Contract> dbContracts = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.Company> dbHostCompanies = null;
            IList<DbModel.User> dbContractCoordinatorUsers = null;
            IList<DbModel.User> dbOperatingUsers = null;
            List<DbModel.Data> dbAssignmentLifeCycle = null;
            List<DbModel.Data> dbReviewAndModerations = null;
            IList<DbModel.CustomerContact> dbCustomerContacts = null;
            IList<DbModel.CustomerAddress> dbCustomerOffices = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.County> dbCounties = null;
            IList<DbModel.City> dbCities = null;
            #endregion
            try
            {
                var recordToBeModify = FilterRecord(assignments, ValidationType.Update);
                {
                    dbProjects = assignmentDbCollection.Assignment.DBProjects;
                    dbContracts = assignmentDbCollection.Assignment.DBContracts;
                    dbCompanies = assignmentDbCollection.Assignment.DBCompanies;
                    dbHostCompanies = assignmentDbCollection.Assignment.DBCompanies;
                    dbContractCoordinatorUsers = assignmentDbCollection.Assignment.DBContractCoordinatorUsers;
                    dbOperatingUsers = assignmentDbCollection.Assignment.DBOperatingUsers;
                    dbAssignmentLifeCycle = assignmentDbCollection.Assignment.DBAssignmentLifeCycle;
                    dbReviewAndModerations = assignmentDbCollection.Assignment.DBAssignmentReviewAndModeration;
                    dbCustomerContacts = assignmentDbCollection.Assignment.DBCustomerContacts;
                    dbCustomerOffices = assignmentDbCollection.Assignment.DBCustomerOffices;
                    dbCountries = assignmentDbCollection.Assignment.DBCountry;
                    dbCounties = assignmentDbCollection.Assignment.DBCounty;
                    dbCities = assignmentDbCollection.Assignment.DBCity;
                    assignmentDbCollection.DBAssignment.ToList().ForEach(assignment =>
                    {
                        var assignmentToBeModify = recordToBeModify.FirstOrDefault(x => x.AssignmentId == assignment.Id);
                        _mapper.Map(assignmentToBeModify, assignment, opt =>
                        {
                            opt.Items["isAssignId"] = true;
                            opt.Items["isAssignmentNumber"] = false;
                            opt.Items["AssignmentCompanyAddress"] = dbCustomerOffices;
                            opt.Items["AssignmentLifeCycle"] = dbAssignmentLifeCycle;
                            opt.Items["AssignmentReview"] = dbReviewAndModerations;
                            opt.Items["AssignmentCustomerContact"] = dbCustomerContacts;
                            opt.Items["AssignmentContractHoldingCompany"] = dbContracts;
                            opt.Items["AssignmentContractHoldingCoordinator"] = dbContractCoordinatorUsers;
                            opt.Items["AssignmentOperatingCompany"] = dbCompanies;
                            opt.Items["AssignmentOperatingCompanyCoordinator"] = dbOperatingUsers;
                            opt.Items["AssignmentHostCompany"] = dbCompanies;
                            opt.Items["AssignmentProjectNumber"] = dbProjects;
                            opt.Items["AssignmentWorkCountry"] = dbCountries;
                            opt.Items["AssignmentWorkCounty"] = dbCounties;
                            opt.Items["AssignmentWorkCity"] = dbCities;
                        });
                        assignment.LastModification = DateTime.UtcNow;
                        assignment.UpdateCount = assignmentToBeModify.UpdateCount.CalculateUpdateCount();
                        assignment.ModifiedBy = assignmentToBeModify.ModifiedBy;
                    });

                    _assignmentRepository.AutoSave = false;
                    _assignmentRepository.Update(assignmentDbCollection.DBAssignment);
                    if (commitChange)
                        _assignmentRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignments);
            }
            finally
            {
                _assignmentRepository.AutoSave = true;
                //_assignmentRepository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private Response ProcessSkeletonData(DomainModel.AssignmentDetail assignmentDetail, DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, Response response, ValidationType validationtype)
        {
            var workFlowType = assignmentDetail.AssignmentInfo.AssignmentProjectWorkFlow.Trim().IsVisitEntry();
            if (workFlowType)
            {
                if (assignmentDatabaseCollection.dbVisit == null && assignmentDetail.AssignmentInfo.IsFirstVisit == false)
                    response = AddSkeletonVisit(assignmentDetail, ref assignmentDatabaseCollection, response);
                if (validationtype != ValidationType.Add)//only for Visit Update
                    response = UpdateSkeltonSupplierIdWithTS(ref assignmentDatabaseCollection, assignmentDetail);
            }
            else
            {
                if (assignmentDatabaseCollection.dbTimesheet == null)
                    response = AddSkeletonTimeSheet(assignmentDetail,
                                             ref assignmentDatabaseCollection,
                                             response);
                if (validationtype != ValidationType.Add)
                    response = UpdateSkeltonTimesheetWithTS(ref assignmentDatabaseCollection, assignmentDetail);
            }
            if (assignmentDetail.AssignmentInfo.IsOverrideOrPLOForPage)
                response = AssignTechSpecForLineItems(ref assignmentDatabaseCollection, workFlowType);

            return response;
        }
        //Added for Override and PLO Line items
        private Response AssignTechSpecForLineItems(ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool workFlowType)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (workFlowType)
                    assignmentDatabaseCollection.dbAddedVisitTS = assignmentDatabaseCollection.dbVisit?.VisitTechnicalSpecialist?.ToList();
                else
                    assignmentDatabaseCollection.dbAddedTimesheetTS = assignmentDatabaseCollection.dbTimesheet?.TimesheetTechnicalSpecialist?.ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(responseType.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        // Update Skeleton Timesheet with selected Resources from the assignment
        private Response UpdateSkeltonTimesheetWithTS(ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, DomainModel.AssignmentDetail assignmentDetail)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            IList<DbModel.TimesheetTechnicalSpecialist> dbTimesheetTechSpec = null;
            List<DbModel.Timesheet> dbtimesheet = new List<DbModel.Timesheet>
            {
                assignmentDatabaseCollection.dbTimesheet
            };
            try
            {
                if (assignmentDatabaseCollection.dbTimesheet?.TimesheetTechnicalSpecialist?.Count == 0 || assignmentDatabaseCollection.dbTimesheet?.TimesheetTechnicalSpecialist?.Count == null)
                {
                    if (assignmentDatabaseCollection?.DBTechnicalSpecialists?.Count > 0)
                    {
                        dbTimesheetTechSpec = assignmentDatabaseCollection?.DBTechnicalSpecialists?.Select((x1, i) => new DbModel.TimesheetTechnicalSpecialist
                        {
                            TechnicalSpecialistId = x1.Id,
                            UpdateCount = 0,
                        }).ToList();
                        _timesheetRepository.AutoSave = false;
                        dbtimesheet?.ToList()?.ForEach(dbTimesheetToBeUpdate =>//Update default supplierid for SKLTON visit and its TS's carefully
                        {
                            dbTimesheetToBeUpdate.TimesheetTechnicalSpecialist = dbTimesheetTechSpec;
                        });
                        if (dbtimesheet?.Count > 0)
                        {
                            _timesheetRepository.Update(dbtimesheet);
                            _timesheetRepository.ForceSave();
                        }
                        assignmentDatabaseCollection.dbTimesheet = dbtimesheet?.FirstOrDefault();
                        assignmentDatabaseCollection.dbAddedTimesheetTS = dbTimesheetTechSpec;
                        AddTsCalendarForSkeletonVsTs(assignmentDatabaseCollection, assignmentDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(responseType.ToId(), ex.ToFullString(), assignmentDetail);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        //MS-TS(Needs to be revisited)
        private Response UpdateSkeltonSupplierIdWithTS(ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, DomainModel.AssignmentDetail assignmentDetail)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            IList<DbModel.VisitTechnicalSpecialist> dbVisitTechSpec = null;
            List<DbModel.Visit> dbvisits = new List<DbModel.Visit>
            {
                assignmentDatabaseCollection.dbVisit
            };
            try
            {
                if (assignmentDatabaseCollection.dbVisit?.VisitTechnicalSpecialist?.Count == 0 || assignmentDatabaseCollection.dbVisit?.VisitTechnicalSpecialist?.Count == null)
                {
                    int? supplierId = null;
                    supplierId = assignmentDetail.AssignmentSubSuppliers?.FirstOrDefault(x1 => x1.IsMainSupplierFirstVisit == true && !(x1.RecordStatus.IsRecordStatusDeleted()))?.MainSupplierId;
                    var AssignmentSubOrMainTs = supplierId == null ? assignmentDetail.AssignmentSubSuppliers?.Where(x1 => x1.IsSubSupplierFirstVisit == true && !(x1.RecordStatus.IsRecordStatusDeleted()) && x1.AssignmentSubSupplierTS != null)?
                                                                                    .SelectMany(x1 => x1.AssignmentSubSupplierTS?.Where(z => z.IsAssignedToThisSubSupplier == true)?.Select(ts => ts.Epin)).ToList()
                                                                  : assignmentDetail.AssignmentSubSuppliers?.FirstOrDefault().AssignmentSubSupplierTS?.Where(z => z.IsAssignedToThisSubSupplier == false && !(z.RecordStatus.IsRecordStatusDeleted()))?.Select(tsepin => tsepin.Epin)?.ToList();
                    if (AssignmentSubOrMainTs?.Count > 0 && assignmentDatabaseCollection.DBTechnicalSpecialists?.Count > 0)
                    {
                        var tsToAdd = assignmentDatabaseCollection.DBTechnicalSpecialists?.Where(dbTs => AssignmentSubOrMainTs.Contains(dbTs.Pin));
                        assignmentDatabaseCollection.DBMainSupplierTechnicalSpecialists = assignmentDatabaseCollection.DBTechnicalSpecialists?.Where(dbTs => AssignmentSubOrMainTs.Contains(dbTs.Pin))?.ToList();
                        dbVisitTechSpec = tsToAdd?.Select((x1, i) => new DbModel.VisitTechnicalSpecialist
                        {
                            TechnicalSpecialistId = x1.Id,
                            UpdateCount = 0,
                        }).ToList();
                    }
                    supplierId = supplierId == null ? assignmentDetail.AssignmentSubSuppliers?.FirstOrDefault(x1 => x1.IsSubSupplierFirstVisit == true && !(x1.RecordStatus.IsRecordStatusDeleted()))?.SubSupplierId : supplierId;
                    if (supplierId != null)
                    {
                        _visitRepository.AutoSave = false;
                        dbvisits?.ToList()?.ForEach(dbVisitToBeUpdate =>//Update default supplierid for SKLTON visit and its TS's carefully
                        {
                            dbVisitToBeUpdate.SupplierId = supplierId;
                            dbVisitToBeUpdate.VisitTechnicalSpecialist = dbVisitTechSpec;
                        });
                        if (dbvisits?.Count > 0)
                        {
                            _visitRepository.Update(dbvisits);
                            _visitRepository.ForceSave();
                        }
                    }
                    assignmentDatabaseCollection.dbVisit = dbvisits?.FirstOrDefault();
                    assignmentDatabaseCollection.dbAddedVisitTS = dbVisitTechSpec;
                    AddTsCalendarForSkeletonVsTs(assignmentDatabaseCollection, assignmentDetail);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(responseType.ToId(), ex.ToFullString(), assignmentDetail);
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        //MS-TS
        private Response AddSkeletonVisit(DomainModel.AssignmentDetail assignmentDetail, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, Response response)
        {
            IList<DbModel.VisitTechnicalSpecialist> dbVisitTechSpec = null;
            IList<DbModel.VisitInterCompanyDiscount> dbVisitInterCompanyDiscount = null;
            try
            {
                _visitRepository.AutoSave = false;
                if (assignmentDetail.AssignmentInfo.VisitFromDate != null && assignmentDetail.AssignmentInfo.VisitToDate != null && assignmentDatabaseCollection.DBAssignment != null)
                {
                    int? supplierId = null;
                    if (assignmentDetail.AssignmentInfo?.AssignmentContractHoldingCompanyCode != assignmentDetail.AssignmentInfo?.AssignmentOperatingCompanyCode && assignmentDetail.AssignmentSubSuppliers?.Count == 0)
                        supplierId = assignmentDatabaseCollection.DBAssignment?.ToList()?.SelectMany(x => x.AssignmentSubSupplier)?.ToList()?.Where(x1 => x1.IsFirstVisit == true)?.ToList().FirstOrDefault().SupplierId;
                    else
                        supplierId = assignmentDetail.AssignmentSubSuppliers?.FirstOrDefault(x1 => x1.IsMainSupplierFirstVisit == true)?.MainSupplierId;//MS-TS
                    if (supplierId != null)
                    {
                        if (assignmentDatabaseCollection.DBTechnicalSpecialists?.Count > 0)
                        {
                            var assignmentAllSupTS = assignmentDetail.AssignmentSubSuppliers.FirstOrDefault().AssignmentSubSupplierTS.ToList();
                            var mainSupplierTs = assignmentAllSupTS.Where(z => z.IsAssignedToThisSubSupplier == false).Select(ts => ts.Epin).ToList();//MS-TS Nov 27
                            if (mainSupplierTs?.Count > 0 && assignmentDatabaseCollection.DBTechnicalSpecialists?.Count > 0)//MS-TS Nov 27
                            {
                                assignmentDatabaseCollection.DBMainSupplierTechnicalSpecialists = assignmentDatabaseCollection.DBTechnicalSpecialists?.Where(dbTs => mainSupplierTs.Contains(dbTs.Pin))?.ToList();//MS-TS Nov 27
                                dbVisitTechSpec = assignmentDatabaseCollection.DBMainSupplierTechnicalSpecialists?.Select((x1, i) => new DbModel.VisitTechnicalSpecialist
                                {
                                    TechnicalSpecialistId = x1.Id,
                                    UpdateCount = 0,
                                }).ToList();
                            }
                        }
                    }
                    //MS-TS Data insert//
                    else
                    {
                        supplierId = assignmentDetail.AssignmentSubSuppliers?.FirstOrDefault(x1 => x1.IsSubSupplierFirstVisit == true)?.SubSupplierId;//Added MS-TS Related
                        var subSupplierTs = assignmentDetail.AssignmentSubSuppliers?.Where(x1 => x1.IsSubSupplierFirstVisit == true && x1.AssignmentSubSupplierTS != null)?
                                                                                    .SelectMany(x1 => x1.AssignmentSubSupplierTS.Where(z => z.IsAssignedToThisSubSupplier == true)?.Select(ts => ts.Epin)).ToList();//MS-TS
                        if (subSupplierTs?.Count > 0 && assignmentDatabaseCollection.DBTechnicalSpecialists?.Count > 0)
                        {
                            assignmentDatabaseCollection.DBSubSupplierTechnicalSpecialists = assignmentDatabaseCollection.DBTechnicalSpecialists?.Where(dbTs => subSupplierTs.Contains(dbTs.Pin))?.ToList();
                            dbVisitTechSpec = assignmentDatabaseCollection.DBSubSupplierTechnicalSpecialists?.Select((x1, i) => new DbModel.VisitTechnicalSpecialist
                            {
                                TechnicalSpecialistId = x1.Id,
                                UpdateCount = 0,
                            }).ToList();
                        }
                    }
                    IList<DbModel.VisitReference> dbVisitRef = assignmentDatabaseCollection.Assignment.DBProjects.SelectMany(x => x.ProjectInvoiceAssignmentReference)?.Where(x => x.IsVisit == true)?.Select((x1, i) => new DbModel.VisitReference
                    {
                        AssignmentReferenceTypeId = x1.AssignmentReferenceTypeId,
                        ReferenceValue = "TBA",
                        UpdateCount = 0,
                    }).ToList();

                    if (assignmentDetail.AssignmentInfo.AssignmentContractHoldingCompanyCode != assignmentDetail.AssignmentInfo.AssignmentOperatingCompanyCode && assignmentDetail.AssignmentInfo.AssignmentId != null)
                    {
                        var assignmentInterCompany = _assignmentInterCompanyDiscount.FindBy(x => x.AssignmentId == assignmentDetail.AssignmentInfo.AssignmentId)?.ToList();
                        dbVisitInterCompanyDiscount = assignmentInterCompany?.Select(x => new DbModel.VisitInterCompanyDiscount
                        {
                            DiscountType = x.DiscountType,
                            CompanyId = x.CompanyId,
                            Description = x.Description,
                            Percentage = x.Percentage,
                            ModifiedBy = x.ModifiedBy,
                            LastModification = x.LastModification,
                            UpdateCount = x.UpdateCount
                        }).ToList();
                    }

                    var dbVisit = new DbModel.Visit
                    {
                        AssignmentId = assignmentDatabaseCollection.DBAssignment.FirstOrDefault().Id,
                        SupplierId = supplierId,
                        VisitNumber = 1,
                        VisitStatus = string.IsNullOrEmpty(assignmentDetail.AssignmentInfo.VisitStatus) ? "T" : assignmentDetail.AssignmentInfo.VisitStatus,
                        IsSkeltonVisit = true,
                        FromDate = (DateTime)assignmentDetail.AssignmentInfo.VisitFromDate,
                        ToDate = (DateTime)assignmentDetail.AssignmentInfo.VisitToDate,
                        IsFinalVisit = false,
                        UpdateCount = 0,
                        VisitTechnicalSpecialist = dbVisitTechSpec,
                        VisitReference = dbVisitRef,
                        VisitInterCompanyDiscount = dbVisitInterCompanyDiscount
                    };
                    // var modules = _moduleRepository.FindBy(x => x.Name == "Assignment" || x.Name == "Visit").ToList();
                    var visitNumberSequence = new DbModel.NumberSequence()
                    {
                        LastSequenceNumber = 1,
                        ModuleId = 5,//modules.FirstOrDefault(x1 => x1.Name == "Assignment").Id,
                        ModuleData = assignmentDatabaseCollection.DBAssignment.FirstOrDefault().Id,
                        ModuleRefId = 17,//modules.FirstOrDefault(x1 => x1.Name == "Visit").Id,
                    };

                    _visitRepository.Add(dbVisit);
                    _visitRepository.ForceSave();

                    if (dbVisit.Id > 0)
                    {
                        _visitRepository.AutoSave = true;
                        assignmentDatabaseCollection.dbVisit = dbVisit;
                        assignmentDatabaseCollection.dbAddedVisitTS = dbVisitTechSpec;
                        AddTsCalendarForSkeletonVsTs(assignmentDatabaseCollection, assignmentDetail);
                        //SaveNumberSequence(visitNumberSequence); Removed since the Number Sequence Logic is done in Trigger.
                        AddVisitTimesheetHistory(dbVisit.Id, assignmentDetail.AssignmentInfo.ActionByUser, true);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                return new Response().ToPopulate(ResponseType.Exception, null, null, null, null, ex);
            }
        }

        private void AddVisitTimesheetHistory(long visitTimesheetId, string changedBy, bool isVisit)
        {
            try
            {
                MasterData searchModel = new MasterData
                {
                    MasterDataTypeId = (int)(MasterType.HistoryTable),
                    Code = VisitTimesheetConstants.VISIT_TIMESHEET_CREATED.ToString()
                };
                var masterData = _masterRepository.Search(searchModel);
                if (masterData != null && masterData.Count > 0)
                {
                    if (isVisit)
                        _visitRepository.AddVisitHistory(visitTimesheetId, masterData[0].Id, changedBy);
                    else
                        _timesheetRepository.AddTimesheetHistory(visitTimesheetId, masterData[0].Id, changedBy);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }

        private Response AddSkeletonTimeSheet(DomainModel.AssignmentDetail assignmentDetail, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, Response response)
        {
            IList<DbModel.TimesheetTechnicalSpecialist> dbTimeTechSpec = null;
            IList<DbModel.TimesheetInterCompanyDiscount> dbTimesheetInterCompanyDiscount = null;
            try
            {
                _timesheetRepository.AutoSave = false;
                if (assignmentDetail.AssignmentInfo.TimesheetFromDate != null && assignmentDetail.AssignmentInfo.TimesheetToDate != null && assignmentDatabaseCollection.DBAssignment != null)
                {
                    if (assignmentDatabaseCollection.DBTechnicalSpecialists?.Count > 0)
                    {
                        dbTimeTechSpec = assignmentDatabaseCollection.DBTechnicalSpecialists?.Select((x1, i) => new DbModel.TimesheetTechnicalSpecialist
                        {
                            TechnicalSpecialistId = x1.Id,
                            UpdateCount = 0,
                        }).ToList();
                    }

                    var projectRef = assignmentDatabaseCollection.Assignment.DBProjects.SelectMany(x => x.ProjectInvoiceAssignmentReference)?.Where(x => x.IsTimesheet == true)?.ToList();
                    IList<DbModel.TimesheetReference> dbTimeSheetRef = projectRef?.Select((x1, i) => new DbModel.TimesheetReference
                    {
                        AssignmentReferenceTypeId = x1.AssignmentReferenceTypeId,
                        ReferenceValue = "TBA",
                        UpdateCount = 0,
                    }).ToList();

                    if (assignmentDetail.AssignmentInfo.AssignmentContractHoldingCompanyCode != assignmentDetail.AssignmentInfo.AssignmentOperatingCompanyCode && assignmentDetail.AssignmentInfo.AssignmentId != null)
                    {
                        var assignmentInterCompany = _assignmentInterCompanyDiscount.FindBy(x => x.AssignmentId == assignmentDetail.AssignmentInfo.AssignmentId)?.ToList();
                        dbTimesheetInterCompanyDiscount = assignmentInterCompany?.Select(x => new DbModel.TimesheetInterCompanyDiscount
                        {
                            DiscountType = x.DiscountType,
                            CompanyId = x.CompanyId,
                            Description = x.Description,
                            Percentage = x.Percentage,
                            ModifiedBy = x.ModifiedBy,
                            LastModification = x.LastModification,
                            UpdateCount = x.UpdateCount
                        }).ToList();
                    }
                    var dbTimeSheet = new DbModel.Timesheet()
                    {
                        AssignmentId = assignmentDatabaseCollection.DBAssignment.FirstOrDefault().Id,
                        IsSkeletonTimesheet = true,
                        TimesheetNumber = 1,
                        TimesheetStatus = string.IsNullOrEmpty(assignmentDetail.AssignmentInfo.TimesheetStatus) ? "N" : assignmentDetail.AssignmentInfo.TimesheetStatus,
                        FromDate = (DateTime)assignmentDetail.AssignmentInfo.TimesheetFromDate,
                        ToDate = (DateTime)assignmentDetail.AssignmentInfo.TimesheetToDate,
                        UpdateCount = 0,
                        TimesheetTechnicalSpecialist = dbTimeTechSpec,
                        TimesheetReference = dbTimeSheetRef,
                        TimesheetInterCompanyDiscount = dbTimesheetInterCompanyDiscount
                    };

                    var timesheetNumberSequence = new DbModel.NumberSequence()
                    {
                        LastSequenceNumber = 1,
                        ModuleId = 5,//modules.FirstOrDefault(x1 => x1.Name == "Assignment").Id,
                        ModuleData = assignmentDatabaseCollection.DBAssignment.FirstOrDefault().Id,
                        ModuleRefId = 18,// modules.FirstOrDefault(x1 => x1.Name == "TimeSheet").Id,
                    };

                    _timesheetRepository.Add(dbTimeSheet);
                    _timesheetRepository.ForceSave();
                    if (dbTimeSheet.Id > 0)
                    {
                        _timesheetRepository.AutoSave = true;
                        assignmentDatabaseCollection.dbTimesheet = dbTimeSheet;
                        assignmentDatabaseCollection.dbAddedTimesheetTS = dbTimeTechSpec;
                        AddTsCalendarForSkeletonVsTs(assignmentDatabaseCollection, assignmentDetail);
                        //SaveNumberSequence(timesheetNumberSequence); Removed since the Number Sequence Logic is done in Trigger.
                        AddVisitTimesheetHistory(dbTimeSheet.Id, assignmentDetail.AssignmentInfo.ActionByUser, false);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                return new Response().ToPopulate(ResponseType.Exception, null, null, null, null, ex);
            }
        }

        private void AddTsCalendarForSkeletonVsTs(DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, DomainModel.AssignmentDetail assignmentDetail)
        {
            IList<DbModel.TechnicalSpecialist> TechnicalSpecialistsToAdd = new List<DbModel.TechnicalSpecialist>();
            if (assignmentDetail.AssignmentInfo.AssignmentProjectWorkFlow.Trim().IsVisitEntry())
                TechnicalSpecialistsToAdd = Enumerable.Union(assignmentDatabaseCollection.DBMainSupplierTechnicalSpecialists ?? new List<DbModel.TechnicalSpecialist>(), assignmentDatabaseCollection.DBSubSupplierTechnicalSpecialists ?? new List<DbModel.TechnicalSpecialist>()).ToList();
            else
                TechnicalSpecialistsToAdd = assignmentDatabaseCollection.DBTechnicalSpecialists;

            IList<DbModel.TechnicalSpecialistCalendar> technicalSpecialistCalendars = new List<DbModel.TechnicalSpecialistCalendar>();
            _tsCalendarRepository.AutoSave = false;

            if (TechnicalSpecialistsToAdd != null && TechnicalSpecialistsToAdd.Count > 0)
            {
                TechnicalSpecialistsToAdd?.ToList().ForEach(x1 =>
                {
                    var dbTechCalender = new DbModel.TechnicalSpecialistCalendar
                    {
                        TechnicalSpecialistId = x1.Id,
                        CompanyId = (assignmentDetail.AssignmentInfo?.AssignmentContractHoldingCompanyCode == assignmentDetail.AssignmentInfo?.AssignmentOperatingCompanyCode) ? assignmentDatabaseCollection.DBAssignment?.FirstOrDefault()?.ContractCompanyId : assignmentDatabaseCollection.DBAssignment?.FirstOrDefault()?.OperatingCompanyId,
                        CalendarType = assignmentDatabaseCollection.dbTimesheet == null ? CalendarType.VISIT.ToString() : CalendarType.TIMESHEET.ToString(),
                        CalendarStatus = CalendarStatus.Confirmed.DisplayName(),
                        CalendarRefCode = assignmentDatabaseCollection.dbTimesheet?.Id ?? assignmentDatabaseCollection.dbVisit.Id,
                        StartDateTime = assignmentDatabaseCollection.dbTimesheet == null ? assignmentDetail.AssignmentInfo.VisitFromDate?.Date.AddHours(9) : assignmentDetail.AssignmentInfo.TimesheetFromDate?.Date.AddHours(9),
                        EndDateTime = assignmentDatabaseCollection.dbTimesheet == null ? assignmentDetail.AssignmentInfo.VisitToDate?.Date.AddHours(17) : assignmentDetail.AssignmentInfo.TimesheetToDate?.Date.AddHours(17),
                        CreatedBy = assignmentDetail.AssignmentInfo.ActionByUser,
                        IsActive = true,
                        UpdateCount = 0,
                        CreatedDate = DateTime.UtcNow
                    };

                    technicalSpecialistCalendars.Add(dbTechCalender);
                    _tsCalendarRepository.Add(technicalSpecialistCalendars);
                });
            }

            if (technicalSpecialistCalendars?.Count > 0)
                _tsCalendarRepository.ForceSave();

            _tsCalendarRepository.AutoSave = true;
        }

        private Response IsRecordValidForProcess(List<DomainModel.Assignment> assignments, ValidationType validationType, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDbCollection, bool IsAPIValidationRequired = false)
        {
            Exception exception = null;
            var result = true;
            IList<ValidationMessage> messages = null;
            try
            {
                assignments = FilterRecord(assignments, validationType);
                if (assignments != null && assignments.Count > 0)
                {
                    messages = new List<ValidationMessage>();
                    result = IsAPIValidationRequired == true ? IsValidPayload(assignments, validationType, ref messages) : true;

                    if (assignmentDbCollection.Assignment == null)
                        assignmentDbCollection.Assignment = new DomainModel.AssignmentDatabaseCollection();
                    if (result)
                    {
                        List<int?> assignmentNotExists = null;
                        var assignmentIds = assignments.Where(x => x.AssignmentId != null).Select(x => x.AssignmentId).ToList();
                        if (validationType == ValidationType.Delete || validationType == ValidationType.Update)
                        {
                            result = IsAssignmentExistInDb(assignmentIds,
                                                           assignmentDbCollection.DBAssignment,
                                                           ref assignmentNotExists,
                                                           ref messages,
                                                           IsAPIValidationRequired);

                            if (result && validationType == ValidationType.Delete)
                                result = IsAssignmentCanBeRemove(assignmentDbCollection.DBAssignment, ref messages);

                            else if (result && validationType == ValidationType.Update)
                                result = IsRecordValidForUpdate(assignments,
                                                                assignmentDbCollection.DBAssignment,
                                                                ref assignmentDbCollection.Assignment,
                                                                ref messages);
                        }
                        else if (validationType == ValidationType.Add)
                            result = IsRecordValidForAdd(assignments,
                                                         ref assignmentDbCollection.Assignment,
                                                         ref messages, IsAPIValidationRequired);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignments);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, messages?.ToList(), result, exception);
        }

        //Added for D-1304 -Start
        private bool IsValidAssignmentBudget(IList<DomainModel.Assignment> assignments, IList<DbModel.Project> dbProjects, ref IList<ValidationMessage> validationMessages)
        {
            string budgetCode = _assignmentRepository.ValidateAssignmentBudget(assignments.FirstOrDefault(), dbProjects.FirstOrDefault());
            if (!string.IsNullOrEmpty(budgetCode))
            {
                List<ValidationMessage> messages = new List<ValidationMessage>();

                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                messages.Add(new ValidationMessage(ResponseType.Warning.ToId(), new List<MessageDetail> { new MessageDetail(ModuleType.Assignment, budgetCode, string.Format(_messageDescriptions[budgetCode].ToString())) }));
                if (messages.Count > 0)
                {
                    validationMessages.AddRange(messages);
                }
            }
            return validationMessages?.Count <= 0;
        }
        //Added for D-1304 -End

        private bool IsRecordValidForAdd(IList<DomainModel.Assignment> filteredData, ref DomainModel.AssignmentDatabaseCollection assignmentDbCollection, ref IList<ValidationMessage> messages, bool IsAPIValidationRequired = false)
        {
            IList<DateTime?> assignmentStartdate = null;
            if (messages == null)
                messages = new List<ValidationMessage>();

            if (IsAPIValidationRequired)
            {
                if (filteredData.Select(x => x.AssignmentProjectWorkFlow.IsVisitEntry()).FirstOrDefault())
                    assignmentStartdate = filteredData.Where(x1 => x1.VisitFromDate != null)?.Select(x1 => x1.VisitFromDate).ToList();
                else
                    assignmentStartdate = filteredData.Where(x1 => x1.TimesheetFromDate != null)?.Select(x1 => x1.TimesheetFromDate).ToList();
            }

            var projectNumbers = filteredData.Where(x => x.AssignmentProjectNumber != null).Select(x1 => (int)x1.AssignmentProjectNumber).Distinct().ToList();
            var countries = filteredData.Where(x => x.WorkLocationCountry != null).Select(x1 => x1.WorkLocationCountry).Distinct().ToList();
            var cities = filteredData.Where(x => x.WorkLocationCity != null).Select(x1 => x1.WorkLocationCity).Distinct().ToList();
            var companyCodes = filteredData.Where(x => x.AssignmentOperatingCompanyCode != null).Select(x1 => x1.AssignmentOperatingCompanyCode)
                                                    .Union(filteredData.Where(x => x.AssignmentHostCompanyCode != null).Select(x1 => x1.AssignmentHostCompanyCode)).ToList();

            if (filteredData.FirstOrDefault()?.InterCompCodes != null)
                companyCodes = companyCodes.Union(filteredData.FirstOrDefault()?.InterCompCodes).ToList();

            var lifeCycle = filteredData.Where(x => x.AssignmentLifecycle != null).Select(x => x.AssignmentLifecycle)?.ToList();
            var reviewAndModeration = filteredData.Where(x => x.AssignmentReviewAndModerationProcess != null).Select(x => x.AssignmentReviewAndModerationProcess)?.ToList();

            var countryIncludes = new string[] { "County",
                                         "County.City"};

            var includes = new string[] {
                                         "Contract.ContractSchedule.ContractRate",
                                         "Contract.Customer.CustomerAddress.CustomerContact",
                                         "SupplierPurchaseOrder.SupplierPurchaseOrderSubSupplier.Supplier.SupplierContact"
                                          };

            return IsValidProjectNumber(projectNumbers, ref assignmentDbCollection.DBProjects, ref messages, includes, assignmentStartdate, IsAPIValidationRequired)
                   && IsValidContractNumber(assignmentDbCollection.DBProjects, ref assignmentDbCollection, filteredData, ref messages)
                   && IsValidCustomer(assignmentDbCollection.DBProjects, ref assignmentDbCollection, filteredData, ref messages)
                   && IsValidCompany(companyCodes, ref assignmentDbCollection.DBCompanies, ref messages)
                   && IsValidCompanyAddress(filteredData, ref assignmentDbCollection, ref messages)
                   && IsValidCustomerContact(filteredData, ref assignmentDbCollection, ref messages)
                   && IsValidSupplierPoNumber(filteredData, ref assignmentDbCollection, ref messages)
                   && IsValidUser(filteredData, ref assignmentDbCollection, ref messages)
                   && IsValidCountryName(countries, ref assignmentDbCollection.DBCountry, cities, ref messages, countryIncludes)
                   && IsValidCounty(filteredData, ref assignmentDbCollection, ref messages)
                   && IsValidCity(filteredData, ref assignmentDbCollection, ref messages)
                   && IsValidAssignmentLifeCycle(assignmentDbCollection.DBAssignmentLifeCycle, lifeCycle, ref messages)
                   && IsValidAssignmentBudget(filteredData, assignmentDbCollection.DBProjects, ref messages);
        }

        private bool IsValidAssignmentLifeCycle(List<DbModel.Data> dbLifecycle, List<string> lifeCycle, ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (lifeCycle?.Count > 0 && dbLifecycle?.Any() == true)
            {
                bool result = dbLifecycle.ToList().Any(x => lifeCycle.Contains(x.Name));
                if (!result)
                {
                    lifeCycle?.ForEach(x =>
                    {
                        string errorCode = MessageType.AssignmentLifeCycle.ToId();
                        message.Add(_messageDescriptions, x, MessageType.AssignmentLifeCycle, x);
                    });

                }
            }
            messages = message;

            return messages.Count <= 0;
        }

        private bool IsValidAssignmentReviewAndModeration(List<DbModel.Data> dbReviewAndModeration, List<string> reviewAndModeration, ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (reviewAndModeration?.Count > 0 && dbReviewAndModeration?.Any() == true)
            {
                bool result = dbReviewAndModeration.ToList().Any(x => reviewAndModeration.Contains(x.Name));
                if (!result)
                {
                    reviewAndModeration?.ForEach(x =>
                    {
                        string errorCode = MessageType.AssignmentReviewAndModeration.ToId();
                        message.Add(_messageDescriptions, x, MessageType.AssignmentReviewAndModeration, x);
                    });
                }
            }
            messages = message;

            return messages.Count <= 0;
        }

        private bool IsValidAssignmentReference(IList<DbModel.Data> dbAssignmentReference, List<string> assignmentReference, ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (assignmentReference?.Count > 0 && dbAssignmentReference?.Any() == true)
            {
                bool result = dbAssignmentReference.ToList().Any(x => assignmentReference.Contains(x.Name));
                if (!result)
                {
                    assignmentReference?.ForEach(x =>
                    {
                        string errorCode = MessageType.AssignmentRefrenceNotExists.ToId();
                        message.Add(_messageDescriptions, x, MessageType.AssignmentRefrenceNotExists, x);
                    });
                }
            }
            messages = message;

            return messages.Count <= 0;
        }

        private bool IsValidProjectNumber(IList<int> projectNumber, ref IList<DbModel.Project> dbProjects, ref IList<ValidationMessage> messages, string[] includes, IList<DateTime?> assignmentStartDate, bool IsAPIValidationRequired)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            List<DbModel.CustomerContact> dbCustomerContact = new List<DbModel.CustomerContact>();
            var dbProjectNumber = _projectRepository?.FindBy(x => projectNumber.Contains((int)x.ProjectNumber), includes)?.
                                                    Select(x => new DbModel.Project
                                                    {
                                                        Id = x.Id,
                                                        ProjectNumber = x.ProjectNumber,
                                                        StartDate = x.StartDate,
                                                        BudgetHours = x.BudgetHours,
                                                        Budget = x.Budget,
                                                        ContractId = x.ContractId,
                                                        InvoiceSalesTaxId = x.InvoiceSalesTaxId,
                                                        InvoiceWithholdingTaxId = x.InvoiceWithholdingTaxId,
                                                        Contract = new DbModel.Contract
                                                        {
                                                            Id = x.Contract.Id,
                                                            ContractNumber = x.Contract.ContractNumber,
                                                            ContractHolderCompanyId = x.Contract.ContractHolderCompanyId,
                                                            ContractType = x.Contract.ContractType,
                                                            ContractSchedule = x.Contract.ContractSchedule != null ? x.Contract.ContractSchedule.Select(x1 => new DbModel.ContractSchedule
                                                            {
                                                                Id = x1.Id
                                                            }).ToList() : null,
                                                            Customer = new DbModel.Customer
                                                            {
                                                                Id = x.Contract.Customer.Id,
                                                                Code = x.Contract.Customer.Code,
                                                                CustomerAddress = x.Contract.Customer.CustomerAddress.Select(x1 => new DbModel.CustomerAddress
                                                                {
                                                                    Id = x1.Id,
                                                                    Address = x1.Address,
                                                                    CustomerContact = x.Contract.Customer.CustomerAddress.SelectMany(x2 => x2.CustomerContact).Select(x2 => new DbModel.CustomerContact { Id = x2.Id, ContactName = x2.ContactName }).ToList(),
                                                                }).ToList()
                                                            }

                                                        },
                                                        SupplierPurchaseOrder = x.SupplierPurchaseOrder.Select(x1 => new DbModel.SupplierPurchaseOrder { Id = x1.Id, SupplierPonumber = x1.SupplierPonumber }).ToList(),
                                                        ProjectInvoiceAssignmentReference = x.ProjectInvoiceAssignmentReference.Select(x1 => new DbModel.ProjectInvoiceAssignmentReference { Id = x1.Id, AssignmentReferenceTypeId = x1.AssignmentReferenceTypeId, IsVisit = x1.IsVisit, IsTimesheet = x1.IsTimesheet }).ToList()
                                                    })?.
                                                    ToList();

            IsValidAssignmentStartDate(assignmentStartDate, dbProjectNumber, ref message);

            var projectNotExists = projectNumber?.Where(x => dbProjectNumber.All(x2 => x2.ProjectNumber != x))?.ToList();
            projectNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidProjectNumber.ToId();
                message.Add(_messageDescriptions, x, MessageType.InvalidProjectNumber, x);
            });
            dbProjects = dbProjectNumber;
            messages = message;

            return messages?.Count <= 0;
        }

        private bool IsValidAssignmentStartDate(IList<DateTime?> assignmentStartDate, IList<DbModel.Project> dbProjects, ref IList<ValidationMessage> messages)
        {
            if (assignmentStartDate != null && assignmentStartDate.Any())
            {
                IList<ValidationMessage> message = new List<ValidationMessage>();
                var assignmentDates = dbProjects.Where(x => assignmentStartDate.Any(x1 => x1 != null && x1 < x.StartDate))?.ToList();

                if (assignmentDates?.Count > 0)
                    message.Add(_messageDescriptions, assignmentStartDate.FirstOrDefault(), MessageType.AssignmentProjectStartDate, assignmentStartDate.FirstOrDefault());
                messages = message;
            }

            return messages?.Count <= 0;
        }

        private bool IsValidCompany(IList<string> companyCodes, ref IList<DbModel.Company> dbCompanies, ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> valdMessage = new List<ValidationMessage>();
            if (!(companyCodes?.Count() > 0)) return messages?.Count <= 0;
            if (dbCompanies == null || dbCompanies?.Count <= 0)
                dbCompanies = _companyRepository?.FindBy(x => companyCodes.Contains(x.Code))?.Select(x => new DbModel.Company { Id = x.Id, Code = x.Code, Name = x.Name, NativeCurrency = x.NativeCurrency })?.AsNoTracking().ToList();

            var dbCompany = dbCompanies;
            var companyCodeNotExists = companyCodes?.Where(x => dbCompany.All(x2 => x2.Code != x))?.ToList();
            companyCodeNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidCompanyCodeWithCode.ToId();
                valdMessage.Add(_messageDescriptions, x, MessageType.InvalidCompanyCodeWithCode, x);
            });

            messages = valdMessage;

            return messages?.Count <= 0;
        }

        private bool IsValidCountryName(IList<string> names, ref IList<DbModel.Country> dbCountries, IList<string> cities, ref IList<ValidationMessage> valdMessages, string[] includes)
        {
            var messages = new List<ValidationMessage>();
            if (!(names?.Count() > 0))
                return valdMessages.Count <= 0; // dbCountries != null ? dbCountries?.Count() == names?.Count: true;
            //if (dbCountries == null && names.Count > 0)
            //    dbCountries = _countryRepository.FindBy(x => names.Contains(x.Name.Trim()), includes)?
            //                                    .Select(x => new DbModel.Country
            //                                    {
            //                                        Id = x.Id,
            //                                        Code = x.Code,
            //                                        Name = x.Name,
            //                                        County = x.County.Select(x1 => new DbModel.County { Id = x1.Id, Name = x1.Name, City = x1.City }).ToList()
            //                                    })?.ToList();

            if (dbCountries == null && names.Count > 0)
                dbCountries = _assignmentRepository.GetCountry(names, cities);

            var dbDatas = dbCountries;
            var countryNotExists = names.Where(x => dbDatas.All(x2 => x2.Name.Trim() != x))?.ToList();
            countryNotExists?.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.MasterInvalidCountry, x);
            });
            valdMessages = messages;
            return valdMessages.Count <= 0;// dbCountries != null ? dbCountries?.Count() == names?.Count: true;
        }

        private bool IsValidCounty(IList<DomainModel.Assignment> filteredData, ref DomainModel.AssignmentDatabaseCollection assignmentDbCollection, ref IList<ValidationMessage> messages)
        {
            IList<DbModel.County> dbCounty = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbCounty = assignmentDbCollection.DBCountry?.ToList().SelectMany(x => x.County).ToList();

            var countyNotExists = filteredData.Where(x => !string.IsNullOrEmpty(x.WorkLocationCounty)
                                                                    && dbCounty.All(x1 => x1.Name != x.WorkLocationCounty))?.ToList();
            if (countyNotExists?.Count > 0)
            {
                countyNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.MasterInvalidCounty.ToId();
                    message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.MasterInvalidCounty, x.WorkLocationCounty);
                });
            }
            assignmentDbCollection.DBCounty = dbCounty;
            messages = message;

            return messages?.Count <= 0;
        }

        private bool IsValidCity(IList<DomainModel.Assignment> filteredData, ref DomainModel.AssignmentDatabaseCollection assignmentDbCollection, ref IList<ValidationMessage> messages)
        {
            IList<DbModel.City> dbCity = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbCity = assignmentDbCollection.DBCounty?.ToList().SelectMany(x => x.City).ToList();

            var cityNotExists = filteredData.Where(x => !string.IsNullOrEmpty(x.WorkLocationCity)
                                                                    && dbCity.All(x1 => x1.Name != x.WorkLocationCity))?.ToList();
            if (cityNotExists?.Count > 0)
            {
                cityNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.MasterInvalidCity.ToId();
                    message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.MasterInvalidCity, x.WorkLocationCity);
                });
            }
            assignmentDbCollection.DBCity = dbCity;
            messages = message;

            return messages?.Count <= 0;
        }

        private bool IsValidUser(IList<DomainModel.Assignment> filteredData, ref DomainModel.AssignmentDatabaseCollection assignmentDbCollection, ref IList<ValidationMessage> messages)
        {
            //var coordinators = new List<string>() { filteredData.FirstOrDefault(x => !string.IsNullOrEmpty(x.AssignmentContractHoldingCompanyCoordinator))?.AssignmentContractHoldingCompanyCoordinator,
            //                                          filteredData.FirstOrDefault(x => !string.IsNullOrEmpty(x.AssignmentOperatingCompanyCoordinator))?.AssignmentOperatingCompanyCoordinator}
            //                                     .Select(x => x).ToList();  //Sanity Defect-173

            var coordinators = new List<string>() { filteredData.FirstOrDefault(x => !string.IsNullOrEmpty(x.AssignmentContractHoldingCompanyCoordinatorCode))?.AssignmentContractHoldingCompanyCoordinatorCode,
                                                      filteredData.FirstOrDefault(x => !string.IsNullOrEmpty(x.AssignmentOperatingCompanyCoordinatorCode))?.AssignmentOperatingCompanyCoordinatorCode}
                                                 .Select(x => x).ToList();

            var dbCoordinators = _assignmentRepository.GetUser(coordinators);

            assignmentDbCollection.DBContractCoordinatorUsers = dbCoordinators;
            assignmentDbCollection.DBOperatingUsers = dbCoordinators;

            return messages?.Count <= 0;
        }

        private bool IsValidSupplierPoNumber(IList<DomainModel.Assignment> filteredData, ref DomainModel.AssignmentDatabaseCollection assignmentDbCollection, ref IList<ValidationMessage> messages)
        {
            IList<DbModel.SupplierPurchaseOrder> dbSupplierPO = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbSupplierPO = assignmentDbCollection.DBProjects?.SelectMany(x => x.SupplierPurchaseOrder).ToList();

            var supplierPONotExists = filteredData.Where(x => x.AssignmentSupplierPurchaseOrderId > 0
                                                              && dbSupplierPO.All(x1 => x1.Id != x.AssignmentSupplierPurchaseOrderId))?.ToList();
            supplierPONotExists?.ForEach(x =>
            {
                message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.AssignmentSupplierPONumber, x.AssignmentSupplierPurchaseOrderNumber);
            });
            assignmentDbCollection.DBSupplierPO = dbSupplierPO;
            messages = message;

            return messages?.Count <= 0;
        }

        private bool IsValidCustomerContact(IList<DomainModel.Assignment> filteredData, ref DomainModel.AssignmentDatabaseCollection assignmentDbCollection, ref IList<ValidationMessage> messages)
        {
            IList<DbModel.CustomerContact> dbCustomerContact = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbCustomerContact = assignmentDbCollection.DBCustomerOffices.SelectMany(x => x.CustomerContact).ToList();

            var customerContactNotExists = filteredData.Where(x => !string.IsNullOrEmpty(x.AssignmentCustomerAssigmentContact)
                                                                    && dbCustomerContact.All(x1 => x.AssignmentCustomerAssigmentContact != x1.ContactName))?.ToList();
            if (customerContactNotExists.Count > 0)
            {
                customerContactNotExists?.ForEach(x =>
                {
                    message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.AssignmentCustomerContact, x.AssignmentCustomerAssigmentContact);
                });
            }
            assignmentDbCollection.DBCustomerContacts = dbCustomerContact;

            messages = message;

            return messages?.Count <= 0;
        }

        private bool IsValidCompanyAddress(IList<DomainModel.Assignment> filteredData, ref DomainModel.AssignmentDatabaseCollection assignmentDbCollection, ref IList<ValidationMessage> messages)
        {
            IList<DbModel.CustomerAddress> dbCustomerOffice = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbCustomerOffice = assignmentDbCollection.DBCustomers?.SelectMany(x => x.CustomerAddress).ToList();

            var customerAddressesNotExists = filteredData.Where(x => !string.IsNullOrEmpty(x.AssignmentCompanyAddress)
                                                                    && dbCustomerOffice.All(x1 => String.Join("", Enumerable.Where<char>(x1.Address, c => !char.IsWhiteSpace(c))) != String.Join("", x.AssignmentCompanyAddress.Where(c => !char.IsWhiteSpace(c)))))?.ToList();
            if (customerAddressesNotExists?.Count > 0)
            {
                customerAddressesNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.AssignmentCompanyAddress.ToId();
                    message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.AssignmentCompanyAddress, x.AssignmentCompanyAddress);
                });
            }
            assignmentDbCollection.DBCustomerOffices = dbCustomerOffice;
            messages = message;

            return messages?.Count <= 0;
        }

        private bool IsValidCustomer(IList<DbModel.Project> dbProjects, ref DomainModel.AssignmentDatabaseCollection assignmentDbCollection, IList<DomainModel.Assignment> filteredData, ref IList<ValidationMessage> messages)
        {
            IList<DbModel.Customer> dbCustomer = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbCustomer = dbProjects?.ToList().Select(x => x.Contract.Customer).ToList();

            var customerNotExists = filteredData.Where(x => dbCustomer.All(x1 => x.AssignmentCustomerCode != x1.Code))?.ToList();
            customerNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.Customer_InvalidCustomerCode.ToId();
                message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.Customer_InvalidCustomerCode, x.AssignmentCustomerCode);
            });

            assignmentDbCollection.DBCustomers = dbCustomer;
            messages.AddRange(message);

            return messages?.Count <= 0;
        }

        private bool IsValidContractNumber(IList<DbModel.Project> dbProjects, ref DomainModel.AssignmentDatabaseCollection assignmentDbCollection, IList<DomainModel.Assignment> filteredData, ref IList<ValidationMessage> messages)
        {
            IList<DbModel.Contract> dbContract = null;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            dbContract = dbProjects?.ToList().Select(x => x.Contract).ToList();

            var contractNotExists = filteredData.Where(x => dbContract.All(x1 => x.AssignmentContractNumber != x1.ContractNumber))?.ToList();
            var contractHoldingCompanyNotExists = filteredData.Where(x => dbContract.All(x1 => x.AssignmentContractNumber != x1.ContractNumber))?.ToList();

            contractNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.InvalidContractNumber.ToId();
                message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.InvalidContractNumber, x.AssignmentContractNumber);
            });

            contractHoldingCompanyNotExists?.ForEach(x =>
            {
                string errorCode = MessageType.AssignmentContractHoldingCompany.ToId();
                message.Add(_messageDescriptions, ModuleType.Assignment, MessageType.AssignmentContractHoldingCompany, x.AssignmentContractHoldingCompanyCode);
            });

            assignmentDbCollection.DBContracts = dbContract;
            messages = message;

            return messages?.Count <= 0;
        }

        private bool IsRecordValidForUpdate(IList<DomainModel.Assignment> filteredData, IList<DbModel.Assignment> dbAssignment, ref DomainModel.AssignmentDatabaseCollection assignmentDbCollection, ref IList<ValidationMessage> messages)
        {
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();
            if (messages?.Count <= 0)
                if (IsRecordUpdateCountMatching(filteredData, dbAssignment, ref messages))
                {
                    var lifeCycle = filteredData.Where(x => x.AssignmentLifecycle != null).Select(x => x.AssignmentLifecycle)?.ToList();
                    var reviewAndModeration = filteredData.Where(x => x.AssignmentReviewAndModerationProcess != null).Select(x => x.AssignmentReviewAndModerationProcess)?.ToList();
                    assignmentDbCollection.DBProjects = dbAssignment?.Select(x => x.Project)?.ToList();
                    assignmentDbCollection.DBContracts = assignmentDbCollection.DBProjects?.Select(x2 => x2.Contract)?.ToList();
                    var countries = filteredData.Where(x => x.WorkLocationCountry != null).Select(x1 => x1.WorkLocationCountry).Distinct().ToList();
                    var cities = filteredData.Where(x => x.WorkLocationCity != null).Select(x1 => x1.WorkLocationCity).Distinct().ToList();
                    var companyCodes = filteredData.Where(x => x.AssignmentOperatingCompanyCode != null).Select(x1 => x1.AssignmentOperatingCompanyCode)
                                                   .Union(filteredData.Where(x => x.AssignmentHostCompanyCode != null).Select(x1 => x1.AssignmentHostCompanyCode)).ToList();

                    if (filteredData.FirstOrDefault()?.InterCompCodes != null)
                        companyCodes = companyCodes.Union(filteredData.FirstOrDefault()?.InterCompCodes).ToList();
                    var includes = new string[] { "County",
                                                 "County.City"};

                    if (IsValidCustomer(assignmentDbCollection.DBProjects, ref assignmentDbCollection, filteredData, ref messages)
                          && IsValidCompany(companyCodes, ref assignmentDbCollection.DBCompanies, ref messages)
                          && IsValidCompanyAddress(filteredData, ref assignmentDbCollection, ref messages)
                          && IsValidCustomerContact(filteredData, ref assignmentDbCollection, ref messages)
                          && IsValidSupplierPoNumber(filteredData, ref assignmentDbCollection, ref messages)
                          && IsValidUser(filteredData, ref assignmentDbCollection, ref messages)
                          && IsValidCountryName(countries, ref assignmentDbCollection.DBCountry, cities, ref messages, includes)
                          && IsValidCounty(filteredData, ref assignmentDbCollection, ref messages)
                          && IsValidCity(filteredData, ref assignmentDbCollection, ref messages)
                          && IsValidAssignmentLifeCycle(assignmentDbCollection.DBAssignmentLifeCycle, lifeCycle, ref messages)
                          && IsValidAssignmentBudget(filteredData, assignmentDbCollection.DBProjects, ref messages)
                          )
                    {
                        return true;
                    }
                }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private bool IsAssignmentExistInDb(List<int?> assignmentIds, IList<DbModel.Assignment> dbAssignment, ref List<int?> assignmentNotExists, ref IList<ValidationMessage> messages, bool IsAPIValidationRequired = false)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();

            if (dbAssignment == null)
                dbAssignment = new List<DbModel.Assignment>();

            var validMessages = messages;

            if (assignmentIds?.Count > 0 && IsAPIValidationRequired)
            {
                assignmentNotExists = assignmentIds.Where(x => x != null && !dbAssignment.Select(x1 => x1.Id).ToList().Contains((int)x)).ToList();
                assignmentNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.AssignmentNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                messages = validMessages;

            return messages.Count <= 0;
        }

        private bool IsAssignmentCanBeRemove(IList<DbModel.Assignment> dbAssignment, ref IList<ValidationMessage> messages)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            dbAssignment.ToList().ForEach(x =>
            {
                var result = x.IsAnyCollectionPropertyContainValue();
                if (result)
                    validationMessages.Add(_messageDescriptions, x.Id, MessageType.AssignmentIsBeingUsed, x.Id);
            });

            if (validationMessages.Count > 0)
                messages.AddRange(validationMessages);

            return messages.Count <= 0;
        }

        private bool IsRecordUpdateCountMatching(IList<DomainModel.Assignment> filteredData, IList<DbModel.Assignment> dbAssignment, ref IList<ValidationMessage> messages)
        {
            var validationMessages = new List<ValidationMessage>();
            if (messages == null)
                messages = new List<ValidationMessage>();

            var notMatchedRecords = filteredData.Where(x => !dbAssignment.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentId)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                validationMessages.Add(_messageDescriptions, x, MessageType.ModuleUpdateCountMismatch, x.AssignmentId);
            });

            if (validationMessages.Count > 0)
                messages.AddRange(validationMessages);

            return messages?.Count <= 0;
        }

        private void GetTechnicalSpecialists(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentDetails, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection)
        {
            var pins = assignmentDetails?.Where(x1 => x1.Epin > 0)?.Select(x1 => x1.Epin)?.ToList();
            if (pins?.Count > 0)
            {
                assignmentDatabaseCollection.DBTechnicalSpecialists = _technicalSpecialistRepository.FindBy(x => pins.Contains(x.Pin))?
                                                                                                    .Select(x => new DbModel.TechnicalSpecialist
                                                                                                    {
                                                                                                        Id = x.Id,
                                                                                                        Pin = x.Pin,
                                                                                                        FirstName = x.FirstName,
                                                                                                        MiddleName = x.MiddleName,
                                                                                                        LastName = x.LastName,
                                                                                                    }
                                                                                                    )?.AsNoTracking()?.ToList();
            }
        }

        private bool IsValidAssignment(IList<int> assignmentId, ref IList<DbModel.Assignment> dbAssignments, ref IList<ValidationMessage> messages, string[] includes)
        {
            IList<ValidationMessage> message = new List<ValidationMessage>();
            if (dbAssignments == null || dbAssignments?.Count == 0)
            {
                var dbAssignment = _assignmentRepository?.FindBy(x => assignmentId.Contains(x.Id), includes)?.ToList();
                var assignmentNotExists = assignmentId?.Where(x => !dbAssignment.Any(x2 => x2.Id == x))?.ToList();
                assignmentNotExists?.ForEach(x =>
                {
                    string errorCode = MessageType.AssignmentNotExists.ToId();
                    message.Add(_messageDescriptions, x, MessageType.AssignmentNotExists, x);
                });
                dbAssignments = dbAssignment;
                messages = message;
            }

            return messages?.Count <= 0;
        }

        private Response Validate(DomainModel.AssignmentDetail assignmentDetails, ValidationType validationType, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, Response response, bool IsAPIValidationRequired = false)
        {
            var messages = new List<ValidationMessage>();
            AssignAssignmentId(assignmentDetails, ref assignmentDatabaseCollection);

            if (assignmentDetails?.AssignmentTaxonomy?.Any() == true)
                ValidateTaxonomy(assignmentDetails?.AssignmentTaxonomy.ToList(), validationType, assignmentDatabaseCollection.DBAssignmentTaxonomy, ref assignmentDatabaseCollection, ref messages, IsAPIValidationRequired, taxo => taxo.TaxonomySubCategory, cat => cat.TaxonomySubCategory.TaxonomyCategory);

            if (assignmentDetails?.AssignmentReferences?.Any() == true)
                ValidateReference(assignmentDetails.AssignmentReferences.ToList(), validationType, assignmentDatabaseCollection.DBAssignmentReferenceTypes, ref messages, IsAPIValidationRequired);

            if (assignmentDetails?.AssignmentContractSchedules?.Any() == true)
                ValidateContractSchedule(assignmentDetails.AssignmentContractSchedules, assignmentDatabaseCollection, validationType, assignmentDatabaseCollection.DBAssignmentContractSchedules, ref messages, IsAPIValidationRequired);

            if (assignmentDetails?.AssignmentTechnicalSpecialists?.Any() == true)
                ValidateTechnicalSpecialist(assignmentDetails.AssignmentTechnicalSpecialists, validationType, assignmentDatabaseCollection.DBAssignmentTechnicalSpecialists, ref assignmentDatabaseCollection, ref messages, IsAPIValidationRequired);

            if (assignmentDetails?.AssignmentSubSuppliers?.Any() == true && assignmentDatabaseCollection != null)
                ValidateSubSupplier((int)assignmentDetails.AssignmentInfo.AssignmentSupplierPurchaseOrderId, validationType, assignmentDetails.AssignmentSubSuppliers, ref assignmentDatabaseCollection.DBAssignmentSubSupplier, ref assignmentDatabaseCollection.DBTechnicalSpecialists, ref assignmentDatabaseCollection.DBAssignment, ref messages, IsAPIValidationRequired);

            if (assignmentDetails?.AssignmentContributionCalculators?.Any() == true)
                ValidateContributionCalculator(assignmentDetails.AssignmentContributionCalculators.ToList(), validationType, assignmentDatabaseCollection.DBAssignmentContributionCalculations, ref messages, IsAPIValidationRequired);

            if (assignmentDetails?.AssignmentAdditionalExpenses?.Any() == true)
                ValidateAdditionalExpense(assignmentDetails.AssignmentAdditionalExpenses, validationType, assignmentDatabaseCollection.DBAssignmentAdditionalExpenses, ref messages, IsAPIValidationRequired);

            if (assignmentDetails?.ResourceSearch?.SearchParameter != null)
            {
                if (assignmentDatabaseCollection.DBService == null && assignmentDetails?.ResourceSearch?.SearchParameter.ServiceId > 0)
                    assignmentDatabaseCollection.DBService = _taxonomyRepository.FindBy(x => x.Id == assignmentDetails.ResourceSearch.SearchParameter.ServiceId, taxo => taxo.TaxonomySubCategory, cat => cat.TaxonomySubCategory.TaxonomyCategory).ToList();

                assignmentDatabaseCollection.DBSubCategory = assignmentDatabaseCollection.DBService?.Select(x => x.TaxonomySubCategory)?.ToList();
                assignmentDatabaseCollection.DBCategory = assignmentDatabaseCollection.DBService?.Select(x => x.TaxonomySubCategory)?.Select(x => x.TaxonomyCategory)?.ToList();
                assignmentDatabaseCollection.DBARSCoordinators = assignmentDatabaseCollection.Assignment.DBOperatingUsers != null && assignmentDatabaseCollection.Assignment.DBOperatingUsers.Count > 0
                                                                ? assignmentDatabaseCollection.Assignment.DBContractCoordinatorUsers?.Concat(assignmentDatabaseCollection.Assignment.DBOperatingUsers)?.ToList()
                                                                : assignmentDatabaseCollection.Assignment.DBContractCoordinatorUsers?.ToList();
                if (assignmentDetails.ResourceSearch.RecordStatus.IsRecordStatusModified())
                    response = _resourceSearchService.IsRecordValidForProcess(assignmentDetails?.ResourceSearch,
                        ref assignmentDatabaseCollection.DBARSSearches,
                        ref assignmentDatabaseCollection.DBARSCoordinators,
                        ref assignmentDatabaseCollection.Assignment.DBCompanies,
                        ref assignmentDatabaseCollection.Assignment.DBCustomers,
                        ref assignmentDatabaseCollection.DBCategory,
                        ref assignmentDatabaseCollection.DBSubCategory,
                        ref assignmentDatabaseCollection.DBService,
                        ref assignmentDatabaseCollection.DBOverrideResources,
                        ref assignmentDatabaseCollection.DBARSAssignment
                    );
            }

            if (assignmentDetails?.AssignmentNotes?.Any() == true && assignmentDatabaseCollection != null)//D661 issue 8
                ValidateAssignmentNote(assignmentDetails.AssignmentNotes, validationType, ref assignmentDatabaseCollection.DBAssignmentNotes, ref assignmentDatabaseCollection.DBAssignment, ref messages, IsAPIValidationRequired);

            if (messages?.Count > 0)
            {
                var vad = response?.ValidationMessages;
                vad.AddRange(messages);
                response = new Response().ToPopulate(ResponseType.Validation, null, null, vad?.ToList(), null, null);
            }

            return response;
        }

        private void ValidateContractSchedule(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractSchedules, DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ValidationType validationType, IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedules, ref List<ValidationMessage> validationMessages, bool IsAPIValidationRequired = false)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            if (IsAPIValidationRequired ? IsValidAssignmentContractSchedulePayload(assignmentContractSchedules, validationType, ref messages) : true)
            {
                if (assignmentDatabaseCollection?.DBContractSchedule != null && IsAPIValidationRequired)
                {
                    var notMatchedRecords = assignmentContractSchedules.Where(x => assignmentDatabaseCollection.DBContractSchedule.All(x2 => x2.Id != x.ContractScheduleId))?.ToList();
                    if (notMatchedRecords?.Count > 0)
                        notMatchedRecords.ForEach(x =>
                        {
                            messages.Add(_messageDescriptions, x.ContractScheduleId, MessageType.ContractScheduleInvalidId, x.ContractScheduleId);
                        });
                }
                if (IsAPIValidationRequired) IsAssignmentContractRateScheduleExistInDb(assignmentContractSchedules, dbAssignmentContractSchedules, ref validationMessages);
                assignmentContractSchedules = assignmentContractSchedules.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
                if (assignmentContractSchedules.Any())
                {
                    var notMatchedRecords = assignmentContractSchedules.Where(x => !dbAssignmentContractSchedules.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentContractRateScheduleId)).ToList();
                    if (notMatchedRecords?.Count > 0)
                        notMatchedRecords.ForEach(x =>
                        {
                            messages.Add(_messageDescriptions, x.AssignmentContractRateScheduleId, MessageType.AssignmentContractRateScheduleUpdateCountMisMatch, x.AssignmentContractRateScheduleId);
                        });
                }
            }
            if (messages.Count > 0)
                validationMessages.AddRange(messages);
        }

        private bool IsValidAssignmentContractSchedulePayload(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractSchedules, ValidationType validationType, ref List<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();

            var filteredAddRecords = assignmentContractSchedules?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
            var filteredModifyRecords = assignmentContractSchedules?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
            var filteredDeleteRecords = assignmentContractSchedules?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();

            if (filteredAddRecords?.Any() == true)
            {
                var validationResult = _assignmentContractRateScheduleValidationService.Validate(JsonConvert.SerializeObject(filteredAddRecords), ValidationType.Add);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredModifyRecords?.Any() == true)
            {
                var validationResult = _assignmentContractRateScheduleValidationService.Validate(JsonConvert.SerializeObject(filteredModifyRecords), ValidationType.Update);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredDeleteRecords?.Any() == true)
            {
                var validationResult = _assignmentContractRateScheduleValidationService.Validate(JsonConvert.SerializeObject(filteredDeleteRecords), ValidationType.Delete);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }

            if (assignmentContractSchedules?.Count > 0)
            {
                var dupData = assignmentContractSchedules.GroupBy(x => new { x.AssignmentId, x.ContractScheduleId })
                                                         .Where(group => group.Count() > 1)
                                                         .Select(group => group.Key);
                if (dupData?.ToList()?.Count != 0)
                    messages.Add(_messageDescriptions, dupData?.FirstOrDefault()?.ContractScheduleId, MessageType.AssignmentContractRateScheduleDuplicateRecord, dupData?.FirstOrDefault()?.ContractScheduleId);
            }

            return messages?.Count <= 0;
        }

        private bool IsAssignmentContractRateScheduleExistInDb(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractSchedules,
                                                               IList<DbModel.AssignmentContractSchedule> dbAssignmentContractRateSchedules,
                                                               ref List<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;
            if (assignmentContractSchedules?.Count > 0 && dbAssignmentContractRateSchedules == null && assignmentContractSchedules.Select(x => new { x.ContractScheduleId, x.AssignmentId }).ContainsDuplicates())
            {
                assignmentContractSchedules?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x.ContractScheduleName, MessageType.AssignmentContractRateScheduleAlreadyExist, x.ContractScheduleName);
                });
            }
            if (assignmentContractSchedules?.Count > 0 && dbAssignmentContractRateSchedules?.Any() == true)
            {
                var assignmentContractRateScheduleIdsExists = assignmentContractSchedules.Where(x => dbAssignmentContractRateSchedules.Any(x1 => x1.Id != x.AssignmentContractRateScheduleId &&
                                                                                                    x.ContractScheduleId == x1.ContractScheduleId && x.AssignmentId == x1.AssignmentId))?.ToList();
                assignmentContractRateScheduleIdsExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.AssignmentContractRateScheduleAlreadyExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private void ValidateTechnicalSpecialist(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists, ValidationType validationType, IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialists,
                                                ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ref List<ValidationMessage> validationMessages, bool IsAPIValidationRequired = false)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            if (IsAPIValidationRequired ? IsValidAssignmentTechSpecPayload(assignmentTechnicalSpecialists, validationType, ref messages) : true)
            {
                if (IsAPIValidationRequired) IsAssignmentTsExistInDb(assignmentTechnicalSpecialists, dbAssignmentTechnicalSpecialists, ref validationMessages);
                assignmentTechnicalSpecialists = assignmentTechnicalSpecialists.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                if (assignmentTechnicalSpecialists.Any())
                {
                    var notMatchedRecords = assignmentTechnicalSpecialists.Where(x => !dbAssignmentTechnicalSpecialists.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentTechnicalSplId)).ToList();
                    if (notMatchedRecords?.Count > 0)
                        notMatchedRecords.ForEach(x =>
                        {
                            messages.Add(_messageDescriptions, x.AssignmentTechnicalSplId, MessageType.AssignmentTechnicalSpecialistUpdateCountMisMatch, x.AssignmentTechnicalSplId);
                        });
                }
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);
        }

        private bool IsValidAssignmentTechSpecPayload(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechSpec, ValidationType validationType, ref List<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();

            var filteredAddRecords = assignmentTechSpec?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
            var filteredModifyRecords = assignmentTechSpec?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
            var filteredDeleteRecords = assignmentTechSpec?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();

            if (filteredAddRecords?.Any() == true)
            {
                var validationResult = _assignmentTechnicalSpecilaistValidationService.Validate(JsonConvert.SerializeObject(filteredAddRecords), ValidationType.Add);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredModifyRecords?.Any() == true)
            {
                var validationResult = _assignmentTechnicalSpecilaistValidationService.Validate(JsonConvert.SerializeObject(filteredModifyRecords), ValidationType.Update);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredDeleteRecords?.Any() == true)
            {
                var validationResult = _assignmentTechnicalSpecilaistValidationService.Validate(JsonConvert.SerializeObject(filteredDeleteRecords), ValidationType.Delete);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }

            if (assignmentTechSpec?.Count > 0)
            {
                var dupData = assignmentTechSpec.GroupBy(x => new { x.AssignmentId, x.Epin })
                                                         .Where(group => group.Count() > 1)
                                                         .Select(group => group.Key);
                if (dupData?.ToList()?.Count != 0)
                    messages.Add(_messageDescriptions, dupData?.FirstOrDefault()?.Epin, MessageType.AssignmentTechnicalSpecialistDuplicateRecord, dupData?.FirstOrDefault()?.Epin);
            }

            var filteredAddAssignmentTSScheduleRecord = assignmentTechSpec?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules)?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
            var filteredModifyAssignmentTSScheduleRecord = assignmentTechSpec?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules)?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
            var filteredDeleteAssignmentTSScheduleRecord = assignmentTechSpec?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules)?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();
            if (filteredAddAssignmentTSScheduleRecord?.Any() == true)
            {
                var validationResult = _assignmentTechnicalSpecialistScheduleValidationService.Validate(JsonConvert.SerializeObject(filteredAddAssignmentTSScheduleRecord), ValidationType.Add);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredModifyAssignmentTSScheduleRecord?.Any() == true)
            {
                var validationResult = _assignmentTechnicalSpecialistScheduleValidationService.Validate(JsonConvert.SerializeObject(filteredModifyAssignmentTSScheduleRecord), ValidationType.Update);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredDeleteAssignmentTSScheduleRecord?.Any() == true)
            {
                var validationResult = _assignmentTechnicalSpecialistScheduleValidationService.Validate(JsonConvert.SerializeObject(filteredDeleteAssignmentTSScheduleRecord), ValidationType.Delete);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }

            if (assignmentTechSpec?.Count > 0)
            {
                var assignmentTechSpecSchedules = assignmentTechSpec?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules)?.ToList();
                var dupData = assignmentTechSpecSchedules.GroupBy(x => new { x.AssignmentTechnicalSpecilaistId, x.ContractScheduleId, x.TechnicalSpecialistPayScheduleId })
                                                         .Where(group => group.Count() > 1)
                                                         .Select(group => group.Key);
                if (dupData?.ToList()?.Count != 0)
                    messages.Add(_messageDescriptions, dupData?.FirstOrDefault()?.AssignmentTechnicalSpecilaistId, MessageType.AssignmentTechnicalSpecialistScheduleDuplicateRecord,
                                                     dupData?.FirstOrDefault()?.ContractScheduleId, dupData?.FirstOrDefault()?.TechnicalSpecialistPayScheduleId);
            }

            return messages?.Count <= 0;
        }

        private bool IsAssignmentTsExistInDb(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechSpec,
                                                              IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialists,
                                                              ref List<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;
            if (assignmentTechSpec?.Count > 0 && dbAssignmentTechnicalSpecialists == null && assignmentTechSpec.Select(x => new { x.Epin, x.AssignmentId }).ContainsDuplicates())
            {
                assignmentTechSpec?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x.Epin, MessageType.AssignmentContractRateScheduleAlreadyExist, x.Epin);
                });
            }
            if (assignmentTechSpec?.Count > 0 && dbAssignmentTechnicalSpecialists?.Any() == true)
            {
                var assignmentTsIdsExists = assignmentTechSpec.Where(x => dbAssignmentTechnicalSpecialists.Any(x1 => x1.Id != x.AssignmentTechnicalSplId &&
                                                                                                    x.Epin == x1.TechnicalSpecialistId && x.AssignmentId == x1.AssignmentId))?.ToList();
                assignmentTsIdsExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x.Epin, MessageType.AssignmentTechnicalSpecialistAlreadyExist, x.Epin);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private void ValidateReference(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes, ValidationType validationType, IList<DbModel.AssignmentReference> dbAssignmentReferenceType, ref List<ValidationMessage> validationMessages, bool IsAPIValidationRequired = false)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            if (IsAPIValidationRequired ? IsValidAssignmentReferencePayload(assignmentReferenceTypes, validationType, ref messages) : true)
            {
                if (IsAPIValidationRequired) IsAssignmentReferenceExistInDb(assignmentReferenceTypes, dbAssignmentReferenceType, ref validationMessages);
                var assignmentReferenceType = assignmentReferenceTypes.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                if (assignmentReferenceType.Any())
                {
                    var notMatchedRecords = assignmentReferenceType.Where(x => !dbAssignmentReferenceType.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentReferenceTypeId)).ToList();
                    if (notMatchedRecords?.Count > 0)
                        notMatchedRecords.ForEach(x =>
                        {
                            messages.Add(_messageDescriptions, x.AssignmentReferenceTypeId, MessageType.AssignmentRefrenceUpdateCountMisMatch, x.AssignmentReferenceTypeId);
                        });
                }
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);
        }

        private bool IsValidAssignmentReferencePayload(IList<DomainModel.AssignmentReferenceType> assignmentReference, ValidationType validationType, ref List<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();
            var filteredAddRecords = assignmentReference?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
            var filteredModifyRecords = assignmentReference?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
            var filteredDeleteRecords = assignmentReference?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();

            if (filteredAddRecords?.Any() == true)
            {
                var validationResult = _assignmentReferenceTypeValidationService.Validate(JsonConvert.SerializeObject(filteredAddRecords), ValidationType.Add);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredModifyRecords?.Any() == true)
            {
                var validationResult = _assignmentReferenceTypeValidationService.Validate(JsonConvert.SerializeObject(filteredModifyRecords), ValidationType.Update);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredAddRecords?.Any() == true)
            {
                var validationResult = _assignmentReferenceTypeValidationService.Validate(JsonConvert.SerializeObject(filteredDeleteRecords), ValidationType.Delete);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (assignmentReference?.Count > 0)
            {
                var dupData = assignmentReference.GroupBy(x => new { x.AssignmentId, x.ReferenceType })
                                                         .Where(group => group.Count() > 1)
                                                         .Select(group => group.Key);
                if (dupData?.ToList()?.Count != 0)
                    messages.Add(_messageDescriptions, dupData?.FirstOrDefault()?.ReferenceType, MessageType.AssignmentRefrenceDuplicateRecord, dupData?.FirstOrDefault()?.ReferenceType);
            }
            return messages?.Count <= 0;
        }

        private bool IsAssignmentReferenceExistInDb(IList<DomainModel.AssignmentReferenceType> assignmentReference,
                                                              IList<DbModel.AssignmentReference> dbAssignmentReferenceType,
                                                              ref List<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;
            if (assignmentReference?.Count > 0 && dbAssignmentReferenceType == null && assignmentReference.Select(x => new { x.ReferenceType, x.AssignmentId }).ContainsDuplicates())
            {
                assignmentReference?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x.ReferenceType, MessageType.AssignmentRefrenceAlreadyExist, x.ReferenceType);
                });
            }
            if (assignmentReference?.Count > 0 && dbAssignmentReferenceType?.Any() == true)
            {
                var assignmentReferenceExists = assignmentReference.Where(x => dbAssignmentReferenceType.Any(x1 => x1.Id != x.AssignmentReferenceTypeId &&
                                                                                                    x.AssignmentReferenceTypeId == x1.AssignmentReferenceTypeId && x.AssignmentId == x1.AssignmentId))?.ToList();
                assignmentReferenceExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x.ReferenceType, MessageType.AssignmentRefrenceAlreadyExist, x.ReferenceType);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }


        private void ValidateTaxonomy(IList<DomainModel.AssignmentTaxonomy> assignmentTaxonomy, ValidationType validationType, IList<DbModel.AssignmentTaxonomy> dbAssignmentTaxonomies, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection,
                                      ref List<ValidationMessage> validationMessages, bool IsAPIValidationRequired = false, params Expression<Func<DbModel.TaxonomyService, object>>[] includes)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (IsAPIValidationRequired ? IsValidAssignmentTaxonomyPayload(assignmentTaxonomy, validationType, ref validationMessages) : true)
            {
                if (assignmentDatabaseCollection?.DBAssignmentTaxonomy != null)
                {
                    assignmentTaxonomy = assignmentTaxonomy?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                    var notMatchedRecords = assignmentTaxonomy.Where(x => !dbAssignmentTaxonomies.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentTaxonomyId)).ToList();
                    notMatchedRecords.ForEach(x =>
                    {
                        messages.Add(_messageDescriptions, x.AssignmentTaxonomyId, MessageType.TaxonomyAlreadyUpdated, x.AssignmentTaxonomyId);
                    });
                }

                var taxonomyServiceId = assignmentTaxonomy.Where(x1 => x1.TaxonomyServiceId > 0).Select(x1 => x1.TaxonomyServiceId).ToList();
                if (taxonomyServiceId?.Count > 0)
                {
                    if (assignmentDatabaseCollection != null && (assignmentDatabaseCollection.DBService == null && taxonomyServiceId.Count > 0))
                        assignmentDatabaseCollection.DBService = _taxonomyRepository.FindBy(x => taxonomyServiceId.Contains(x.Id), includes)
                                                                                    ?.Select(x => new DbModel.TaxonomyService
                                                                                    {
                                                                                        Id = x.Id,
                                                                                        TaxonomyServiceName = x.TaxonomyServiceName,
                                                                                        TaxonomySubCategoryId = x.TaxonomySubCategoryId,
                                                                                        TaxonomySubCategory = new DbModel.TaxonomySubCategory
                                                                                        {
                                                                                            Id = x.TaxonomySubCategoryId,
                                                                                            TaxonomySubCategoryName = x.TaxonomySubCategory.TaxonomySubCategoryName,
                                                                                            TaxonomyCategory = new DbModel.Data
                                                                                            {
                                                                                                Id = x.TaxonomySubCategory.TaxonomyCategoryId,
                                                                                                Name = x.TaxonomySubCategory.TaxonomyCategory.Name,
                                                                                            },
                                                                                        }
                                                                                    }).ToList();

                    if (assignmentDatabaseCollection != null && IsAPIValidationRequired)
                    {
                        var dbData = assignmentDatabaseCollection.DBService;
                        var serviceNotExists = taxonomyServiceId.Where(x => dbData.All(x2 => x2.Id != x))?.ToList();
                        serviceNotExists?.ForEach(x =>
                        {
                            messages.Add(_messageDescriptions, x, MessageType.InvalidTaxonomyService, x);
                        });
                    }
                }
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);
        }

        private bool IsValidAssignmentTaxonomyPayload(IList<DomainModel.AssignmentTaxonomy> assignmentTaxonomy, ValidationType validationType, ref List<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();
            var filteredAddRecords = assignmentTaxonomy?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
            var filteredModifyRecords = assignmentTaxonomy?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
            var filteredDeleteRecords = assignmentTaxonomy?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();

            if (filteredAddRecords?.Any() == true)
            {
                var validationResult = _assignmentTaxonomyValidationService.Validate(JsonConvert.SerializeObject(filteredAddRecords), ValidationType.Add);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredModifyRecords?.Any() == true)
            {
                var validationResult = _assignmentTaxonomyValidationService.Validate(JsonConvert.SerializeObject(filteredModifyRecords), ValidationType.Update);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredDeleteRecords?.Any() == true)
            {
                var validationResult = _assignmentTaxonomyValidationService.Validate(JsonConvert.SerializeObject(filteredDeleteRecords), ValidationType.Delete);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            return messages?.Count <= 0;
        }

        private Response ValidateSubSupplier(int supplierPoId, ValidationType validationType, IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers, ref IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier,
                                          ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist, ref IList<DbModel.Assignment> dbAssignments, ref List<ValidationMessage> validationMessages, bool IsAPIValidationRequired = false)
        {
            Exception exception = null;
            var result = true;
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            List<int?> assignmentNotExists = null;
            try
            {
                var filterAddSubSupplier = assignmentSubSuppliers?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
                var filterModifySubSupplier = assignmentSubSuppliers?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
                var filterDeleteSubSupplier = assignmentSubSuppliers?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();
                if (IsAPIValidationRequired ? IsValidAssignmentSubSupplierPayload(filterAddSubSupplier, filterModifySubSupplier, filterDeleteSubSupplier, assignmentSubSuppliers, validationType, ref messages) : true)
                {
                    if (filterModifySubSupplier.Any() || filterDeleteSubSupplier.Any())
                    {
                        var assignmentSubSupplierIds = assignmentSubSuppliers?.ToList()?.Where(x => x.AssignmentSubSupplierId != null || x.AssignmentSubSupplierId != 0)
                                                                                    .Select(x => x.AssignmentSubSupplierId).ToList();

                        result = IsAssignmentSubSupplierExistInDb(assignmentSubSupplierIds,
                                                                  dbAssignmentSubSupplier,
                                                                  ref assignmentNotExists,
                                                                  ref messages);

                        if (result && filterDeleteSubSupplier.Any())
                            result = IsChildRecordExistsInDb(filterDeleteSubSupplier, dbAssignmentSubSupplier, ref messages);

                        else if (result && filterModifySubSupplier.Any())
                            result = IsRecordValidForUpdate(supplierPoId,
                                                            filterModifySubSupplier,
                                                            dbAssignmentSubSupplier,
                                                            ref dbTechnicalSpecialist,
                                                            ref messages,
                                                            ref dbAssignments);
                    }
                    else if (filterAddSubSupplier.Any())
                        result = IsRecordValidForAdd(supplierPoId,
                                                     filterAddSubSupplier,
                                                     ref dbAssignmentSubSupplier,
                                                     ref dbTechnicalSpecialist,
                                                     ref messages,
                                                     ref dbAssignments);
                }
                if (messages?.Count > 0)
                    validationMessages?.AddRange(messages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSuppliers);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsValidAssignmentSubSupplierPayload(IList<DomainModel.AssignmentSubSupplier> filteredAddRecords, IList<DomainModel.AssignmentSubSupplier> filteredUpdateRecords, IList<DomainModel.AssignmentSubSupplier> filteredDeleteRecords, IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers, ValidationType validationType, ref IList<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();
            if (filteredAddRecords?.Any() == true)
            {
                var validationResult = _assignmentSubSupplierValidationService.Validate(JsonConvert.SerializeObject(filteredAddRecords), ValidationType.Add);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredUpdateRecords?.Any() == true)
            {
                var validationResult = _assignmentSubSupplierValidationService.Validate(JsonConvert.SerializeObject(filteredUpdateRecords), ValidationType.Update);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredDeleteRecords?.Any() == true)
            {
                var validationResult = _assignmentSubSupplierValidationService.Validate(JsonConvert.SerializeObject(filteredDeleteRecords), ValidationType.Delete);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            var filteredAddAssignmentSunSupplierTSRecord = assignmentSubSuppliers?.SelectMany(x => x.AssignmentSubSupplierTS)?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
            var filteredModifyAssignmentSunSupplierTSRecord = assignmentSubSuppliers?.SelectMany(x => x.AssignmentSubSupplierTS)?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
            var filteredDeleteAssignmentSunSupplierTSRecord = assignmentSubSuppliers?.SelectMany(x => x.AssignmentSubSupplierTS)?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();
            if (filteredAddAssignmentSunSupplierTSRecord?.Any() == true)
            {
                var validationResult = _assignmentSubSupplierTSValidationService.Validate(JsonConvert.SerializeObject(filteredAddAssignmentSunSupplierTSRecord), ValidationType.Add);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredModifyAssignmentSunSupplierTSRecord?.Any() == true)
            {
                var validationResult = _assignmentSubSupplierTSValidationService.Validate(JsonConvert.SerializeObject(filteredModifyAssignmentSunSupplierTSRecord), ValidationType.Update);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredDeleteAssignmentSunSupplierTSRecord?.Any() == true)
            {
                var validationResult = _assignmentSubSupplierTSValidationService.Validate(JsonConvert.SerializeObject(filteredDeleteAssignmentSunSupplierTSRecord), ValidationType.Delete);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }

            return messages?.Count <= 0;
        }

        private void ValidateAdditionalExpense(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpenses, ValidationType validationType, IList<DbModel.AssignmentAdditionalExpense> dbAdditionalExpenses, ref List<ValidationMessage> validationMessages, bool IsAPIValidationRequired = false)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            if (IsAPIValidationRequired ? IsValidAssignmentAdditionalExpensePayload(assignmentAdditionalExpenses, validationType, ref messages) : true)
            {
                assignmentAdditionalExpenses = assignmentAdditionalExpenses.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
                if (assignmentAdditionalExpenses.Any())
                {
                    var notMatchedRecords = assignmentAdditionalExpenses.Where(x => !dbAdditionalExpenses.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentAdditionalExpenseId)).ToList();
                    notMatchedRecords.ForEach(x =>
                    {
                        messages.Add(_messageDescriptions, x.AssignmentAdditionalExpenseId, MessageType.AssignmentAdditionalExpenseUpdateCountMismatch, x.AssignmentAdditionalExpenseId);
                    });
                }
            }
            if (messages.Count > 0)
                validationMessages.AddRange(messages);
        }

        private bool IsValidAssignmentAdditionalExpensePayload(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpenses, ValidationType validationType, ref List<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();

            var filteredAddRecords = assignmentAdditionalExpenses?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
            var filteredModifyRecords = assignmentAdditionalExpenses?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
            var filteredDeleteRecords = assignmentAdditionalExpenses?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();

            if (filteredAddRecords?.Any() == true)
            {
                var validationResult = _assignmentAdditionalExpenseValidationService.Validate(JsonConvert.SerializeObject(filteredAddRecords), ValidationType.Add);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredModifyRecords?.Any() == true)
            {
                var validationResult = _assignmentAdditionalExpenseValidationService.Validate(JsonConvert.SerializeObject(filteredModifyRecords), ValidationType.Update);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredDeleteRecords?.Any() == true)
            {
                var validationResult = _assignmentAdditionalExpenseValidationService.Validate(JsonConvert.SerializeObject(filteredDeleteRecords), ValidationType.Delete);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            return messages?.Count <= 0;
        }

        //D661 issue 8 Start
        private Response ValidateAssignmentNote(IList<DomainModel.AssignmentNote> assignmentNotes, ValidationType validationType,
                                                ref IList<DbModel.AssignmentNote> dbAssignmentNote, ref IList<DbModel.Assignment> dbAssignments,
                                                ref List<ValidationMessage> validationMessage, bool IsAPIValidationRequired = false)
        {
            Exception exception = null;
            var result = true;
            IList<ValidationMessage> message = new List<ValidationMessage>();
            try
            {
                if (IsAPIValidationRequired) IsValidAssignmentNotePayload(assignmentNotes, validationType, ref message);
                if (message.Any())
                    validationMessage?.AddRange(message);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentNotes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessage?.ToList(), result, exception);
        }

        private bool IsValidAssignmentNotePayload(IList<DomainModel.AssignmentNote> assignmentNote, ValidationType validationType, ref IList<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();
            var filteredAddRecords = assignmentNote?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
            var filteredModifyRecords = assignmentNote?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
            var filteredDeleteRecords = assignmentNote?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();

            if (filteredAddRecords?.Any() == true)
            {
                var validationResult = _assignmentNoteValidationService.Validate(JsonConvert.SerializeObject(filteredAddRecords), ValidationType.Add);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredModifyRecords?.Any() == true)
            {
                var validationResult = _assignmentNoteValidationService.Validate(JsonConvert.SerializeObject(filteredModifyRecords), ValidationType.Update);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredDeleteRecords?.Any() == true)
            {
                var validationResult = _assignmentNoteValidationService.Validate(JsonConvert.SerializeObject(filteredDeleteRecords), ValidationType.Delete);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }

            return messages?.Count <= 0;
        }

        //D661 issue 8 End
        private bool ValidateContributionCalculator(IList<DomainModel.AssignmentContributionCalculation> filteredAssignmentContributionCalculation, ValidationType validationType, IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation,
                                                    ref List<ValidationMessage> validationMessages, bool IsAPIValidationRequired = false)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            if (IsAPIValidationRequired ? IsValidAssignmentContributionPayload(filteredAssignmentContributionCalculation, validationType, ref messages) : true)
            {
                filteredAssignmentContributionCalculation = filteredAssignmentContributionCalculation.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                if (filteredAssignmentContributionCalculation.Any())
                {
                    var notMatchedRecords = filteredAssignmentContributionCalculation.Where(x => !dbAssignmentContributionCalculation.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentContCalculationId)).ToList();
                    notMatchedRecords.ForEach(x =>
                    {
                        messages.Add(_messageDescriptions, x.AssignmentContCalculationId, MessageType.AssignmentContributionCalculationUpdateCountMismatch, x.AssignmentContCalculationId);
                    });
                }

                if (messages.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return messages?.Count <= 0;
        }

        private bool IsValidAssignmentContributionPayload(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculations, ValidationType validationType, ref List<ValidationMessage> messages)
        {
            if (messages == null)
                messages = new List<ValidationMessage>();

            var filteredAddRecords = assignmentContributionCalculations?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
            var filteredModifyRecords = assignmentContributionCalculations?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
            var filteredDeleteRecords = assignmentContributionCalculations?.ToList()?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();

            if (filteredAddRecords?.Any() == true)
            {
                var validationResult = _assignmentContributionCalculationValidationService.Validate(JsonConvert.SerializeObject(filteredAddRecords), ValidationType.Add);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredModifyRecords?.Any() == true)
            {
                var validationResult = _assignmentContributionCalculationValidationService.Validate(JsonConvert.SerializeObject(filteredAddRecords), ValidationType.Update);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }
            if (filteredDeleteRecords?.Any() == true)
            {
                var validationResult = _assignmentContributionCalculationValidationService.Validate(JsonConvert.SerializeObject(filteredAddRecords), ValidationType.Delete);
                if (validationResult?.Count > 0)
                    messages.Add(_messageDescriptions, ModuleType.Assignment, validationResult);
            }

            return messages?.Count <= 0;
        }

        private bool IsRecordValidForAdd(int supplierPoId, IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier, ref IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                         ref IList<ValidationMessage> validationMessages, ref IList<DbModel.Assignment> dbAssignments)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (IsValidSupplierContact(supplierPoId, assignmentSubSupplier, ref validationMessages))
                if (IsFirstVisitAlreadyAssociatedToAnotherSupplier(assignmentSubSupplier, dbAssignments, dbAssignmentSubSupplier, ref validationMessages))
                    return true;

            return false;
        }

        private bool IsValidSupplierContact(int supplierPoId,
                                          IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier,
                                          ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var supplierId = assignmentSubSupplier?.Where(x => x.MainSupplierId > 0)?.Select(x => x.MainSupplierId)?.ToList();
            var subSupplierId = assignmentSubSupplier?.Where(x => x.SubSupplierId > 0)?.Select(x => x.SubSupplierId)?.ToList();

            if (subSupplierId.Any())
                supplierId.AddRange(subSupplierId);

            var supplierContactId = assignmentSubSupplier?.Where(x => x.MainSupplierContactId > 0)?.Select(x => x.MainSupplierContactId)?.ToList();
            var subSupplierContactId = assignmentSubSupplier?.Where(x => x.SubSupplierContactId > 0)?.Select(x => x.SubSupplierContactId)?.ToList();

            if (subSupplierContactId.Any())
                supplierContactId.AddRange(subSupplierContactId);

            var validMessages = validationMessages;

            bool result = _supplierPoSubSupplierRepository.IsValidSupplierContact(supplierPoId, supplierId, supplierContactId);

            if (!result)
            {
                supplierContactId?.ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierIdAndContact, x);
                });
            }
            validationMessages = validMessages;
            return validationMessages.Count <= 0;
        }

        private bool IsFirstVisitAlreadyAssociatedToAnotherSupplier(IList<DomainModel.AssignmentSubSupplier> subSupplier, IList<DbModel.Assignment> dbAssignment, IList<DbModel.AssignmentSubSupplier> dbSubSupplier,
                                                                   ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validMessages = validationMessages;
            var assignmentIds = subSupplier.ToList().Where(x => x.AssignmentId > 0).Where(x => x.IsMainSupplierFirstVisit == true && x.IsSubSupplierFirstVisit == true)?.ToList();
            if (assignmentIds.Count > 0)
            {
                assignmentIds.ForEach(x =>
                {
                    validMessages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierHasFirstVisit, x);
                });
            }

            validationMessages = validMessages;
            return validationMessages.Count <= 0;
        }

        private bool IsRecordValidForUpdate(int supplierPoId, IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier, IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialist,
                                            ref IList<ValidationMessage> validationMessages, ref IList<DbModel.Assignment> dbAssignments)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (messages?.Count <= 0)
            {
                if (IsRecordUpdateCountMatching(assignmentSubSupplier, dbAssignmentSubSupplier, ref messages))
                {
                    var assignmentIds = assignmentSubSupplier.Where(x => x.AssignmentId > 0).Select(x => (int)x.AssignmentId).ToList();
                    if (IsChildRecordExistsInDb(assignmentSubSupplier, dbAssignmentSubSupplier, ref validationMessages))
                        if (IsChildRecordUpdateCountMatching(assignmentSubSupplier, dbAssignmentSubSupplier, ref validationMessages))
                            IsValidSupplierContact(supplierPoId, assignmentSubSupplier, ref validationMessages);
                }
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsChildRecordExistsInDb(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier, IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier, ref IList<ValidationMessage> validationMessages)

        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var messages = validationMessages;
            var subSupplierTsIds = assignmentSubSupplier.SelectMany(x => x.AssignmentSubSupplierTS)?
                                                               .Where(x => !x.RecordStatus.IsRecordStatusNew() && x.AssignmentSubSupplierTSId != null)
                                                               .Select(x => x.AssignmentSubSupplierTSId).ToList();

            var recordNotExists = subSupplierTsIds.Where(x => dbAssignmentSubSupplier.SelectMany(x1 => x1.AssignmentSubSupplierTechnicalSpecialist).All(x2 => x2.Id != x)).ToList();
            recordNotExists?.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierTechnicalSpecialistNotExists, x);
            });

            validationMessages = messages;
            return validationMessages?.Count <= 0;
        }

        private bool IsChildRecordUpdateCountMatching(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers, IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier, ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            var messages = validationMessages;
            var assignmentSubSupplierTs = assignmentSubSuppliers.SelectMany(x => x.AssignmentSubSupplierTS).ToList();
            var recordToValidate = assignmentSubSupplierTs.Where(x => x.RecordStatus.IsRecordStatusModified() && x.AssignmentSubSupplierTSId != null).ToList();
            var updateCountNotMatchingRecords = recordToValidate.Where(x => !dbAssignmentSubSupplier.SelectMany(x1 => x1.AssignmentSubSupplierTechnicalSpecialist).Any(x2 => x2.Id == x.AssignmentSubSupplierTSId && x2.UpdateCount == x.UpdateCount)).ToList();

            updateCountNotMatchingRecords?.ForEach(x =>
            {
                messages.Add(_messageDescriptions, x.AssignmentSubSupplierTSId, MessageType.AssignmentSubSupplierTechnicalSpecialistUpdateCountMisMatch, x.AssignmentSubSupplierTSId);
            });

            validationMessages = messages;

            return validationMessages?.Count <= 0;
        }

        private bool IsAssignmentSubSupplierExistInDb(List<int?> assignmentSubSupplierIds, IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier, ref List<int?> assignmentSubSupplierNotExists, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbAssignmentSubSupplier == null)
                dbAssignmentSubSupplier = new List<DbModel.AssignmentSubSupplier>();

            if (assignmentSubSupplierIds?.ToList()?.Count > 0)
            {
                assignmentSubSupplierNotExists = assignmentSubSupplierIds.Where(x => x > 0 && !dbAssignmentSubSupplier.Select(x1 => x1.Id).ToList().Contains((int)x)).ToList();
                assignmentSubSupplierNotExists?.ToList().ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierNotExists, x);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }
        //D661 issue 8 Start
        private bool IsAssignmentNotesExistInDb(List<int?> assignmentNotesIds, IList<DbModel.AssignmentNote> dbAssignmentNote, ref List<int?> assignmentNoteNotExists, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbAssignmentNote == null)
                dbAssignmentNote = new List<DbModel.AssignmentNote>();

            if (assignmentNotesIds?.Count > 0)
            {
                assignmentNoteNotExists = assignmentNotesIds.Where(x => !dbAssignmentNote.Select(x1 => x1.Id).ToList().Contains((int)x)).ToList();
                assignmentNoteNotExists?.ToList().ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x, MessageType.AssignmentNoteNotExists, x);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }

        private bool IsRecordUpdateCountMatchingForNotes(IList<DomainModel.AssignmentNote> assignmentNote, IList<DbModel.AssignmentNote> dbAssignmentNote, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            if ((assignmentNote?.Select(x => x.AssignmnetNoteId > 0)).Any())
            {
                var notMatchedRecords = assignmentNote.Where(x => !dbAssignmentNote.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmnetNoteId)).ToList();
                notMatchedRecords.ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierUpdateCountMisMatch, x.AssignmnetNoteId);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }
        //D661 issue 8 End
        private bool IsRecordUpdateCountMatching(IList<DomainModel.AssignmentSubSupplier> assignmentSubSupplier, IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier, ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();
            if ((assignmentSubSupplier?.Select(x => x.SubSupplierId > 0)).Any())//MS-TS Link
            {
                var notMatchedRecords = assignmentSubSupplier.Where(x => x.AssignmentSubSupplierId > 0 && !dbAssignmentSubSupplier.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.AssignmentSubSupplierId)).ToList();
                notMatchedRecords.ForEach(x =>
                {
                    messages.Add(_messageDescriptions, x, MessageType.AssignmentSubSupplierUpdateCountMisMatch, x.AssignmentSubSupplierId);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        private static void AssignAssignmentId(DomainModel.AssignmentDetail assignmentDetails, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection)
        {
            if (!(assignmentDatabaseCollection.DBAssignment?.FirstOrDefault()?.Id > 0)) return;
            {
                assignmentDetails.AssignmentInfo.AssignmentId = assignmentDatabaseCollection.DBAssignment?.FirstOrDefault()?.Id;
                assignmentDetails.AssignmentSubSuppliers?.ToList().ForEach(x =>
                {
                    x.AssignmentId = assignmentDetails.AssignmentInfo.AssignmentId;
                });
                assignmentDetails.AssignmentContractSchedules?.ToList().ForEach(x =>
                {
                    x.AssignmentId = assignmentDetails.AssignmentInfo.AssignmentId;
                });
                assignmentDetails.AssignmentReferences?.ToList().ForEach(x =>
                {
                    x.AssignmentId = assignmentDetails.AssignmentInfo.AssignmentId;
                });
                assignmentDetails.AssignmentTaxonomy?.ToList().ForEach(x =>
                {
                    x.AssignmentId = assignmentDetails.AssignmentInfo.AssignmentId;
                });
                assignmentDetails.AssignmentAdditionalExpenses?.ToList().ForEach(x =>
                {
                    x.AssignmentId = assignmentDetails.AssignmentInfo.AssignmentId;
                });
                assignmentDetails.AssignmentTechnicalSpecialists?.ToList().ForEach(x =>
                {
                    x.AssignmentId = assignmentDetails.AssignmentInfo.AssignmentId;
                });
                assignmentDetails.AssignmentNotes?.ToList().ForEach(x =>
                {
                    x.AssignmentId = assignmentDetails.AssignmentInfo.AssignmentId;
                });
                assignmentDetails.AssignmentDocuments?.ToList().ForEach(x =>
                {
                    x.ModuleRefCode = assignmentDetails.AssignmentInfo.AssignmentId.ToString();
                });
                if (assignmentDetails.AssignmentInstructions != null)
                    assignmentDetails.AssignmentInstructions.AssignmentId = assignmentDetails.AssignmentInfo.AssignmentId;
                if (assignmentDetails.AssignmentInterCompanyDiscounts != null)
                    assignmentDetails.AssignmentInterCompanyDiscounts.AssignmentId = assignmentDetails.AssignmentInfo.AssignmentId;

                assignmentDetails.AssignmentContributionCalculators?.ToList().ForEach(x =>
                {
                    x.AssignmentId = assignmentDetails.AssignmentInfo.AssignmentId;
                });

                assignmentDatabaseCollection.DBContractSchedule = assignmentDatabaseCollection.Assignment.DBProjects.SelectMany(x => x.Contract?.ContractSchedule)?.ToList();
                assignmentDatabaseCollection.DBInterCompanies = assignmentDatabaseCollection.Assignment.DBCompanies;
            }
        }

        private Response ProcessAssignmentDetail(DomainModel.AssignmentDetail assignmentDetail, DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ValidationType validationType, ref List<DbModel.Document> dbDocuments)
        {
            Response response = null;
            try
            {
                if (assignmentDetail != null)
                {
                    if (assignmentDetail.AssignmentReferences?.Where(x => !string.IsNullOrEmpty(x.RecordStatus))?.Any() == true)
                        response = this.ProcessAssignmentReference(assignmentDetail.AssignmentReferences, ref assignmentDatabaseCollection, validationType);

                    if (assignmentDetail.AssignmentContractSchedules?.Where(x => !string.IsNullOrEmpty(x.RecordStatus))?.Any() == true && (response == null || response?.Code == MessageType.Success.ToId()))
                        response = this.ProcessAssignmentContractRateSchedule(assignmentDetail.AssignmentContractSchedules, ref assignmentDatabaseCollection, validationType);

                    if (assignmentDetail.AssignmentTaxonomy?.Where(x => !string.IsNullOrEmpty(x.RecordStatus))?.Any() == true && (response == null || response?.Code == MessageType.Success.ToId()))
                        response = this.ProcessAssignmentTaxonomy(assignmentDetail.AssignmentTaxonomy, ref assignmentDatabaseCollection, validationType);

                    if (assignmentDetail.AssignmentTechnicalSpecialists?.Where(x => !string.IsNullOrEmpty(x.RecordStatus))?.Any() == true && (response == null || response?.Code == MessageType.Success.ToId()))
                        response = this.ProcessAssignmentTechnicalSpecialist(assignmentDetail.AssignmentTechnicalSpecialists, assignmentDetail.AssignmentSubSuppliers, ref assignmentDatabaseCollection, validationType, assignmentDetail);
                    if (assignmentDetail.AssignmentSubSuppliers?.Where(x => !string.IsNullOrEmpty(x.RecordStatus))?.Any() == true && (response == null || response?.Code == MessageType.Success.ToId()))
                        response = this.ProcessAssignmentSupplier(assignmentDetail.AssignmentSubSuppliers, assignmentDetail.AssignmentInfo.IsSupplierPOChanged, ref assignmentDatabaseCollection, validationType);     
                    if (assignmentDetail?.AssignmentAdditionalExpenses?.Where(x => !string.IsNullOrEmpty(x.RecordStatus))?.Any() == true && (response == null || response?.Code == MessageType.Success.ToId()))
                        response = this.ProcessAssignmentAdditionalExpense(assignmentDetail.AssignmentAdditionalExpenses, ref assignmentDatabaseCollection, validationType);

                    if (assignmentDetail.AssignmentInterCompanyDiscounts != null && (response == null || response?.Code == MessageType.Success.ToId()))
                        response = this.ProcessAssignmentInterCompany(assignmentDetail.AssignmentInterCompanyDiscounts, ref assignmentDatabaseCollection, validationType);

                    if (assignmentDetail.AssignmentContributionCalculators?.Where(x => !string.IsNullOrEmpty(x.RecordStatus))?.Any() == true && (response == null || response?.Code == MessageType.Success.ToId()))
                        response = this.ProcessAssignmentContributionCalculator(assignmentDetail.AssignmentContributionCalculators, ref assignmentDatabaseCollection, validationType);

                    if (assignmentDetail.AssignmentInstructions != null && (response == null || response?.Code == MessageType.Success.ToId()))
                        response = this.ProcessAssignmentInstruction(assignmentDetail.AssignmentInstructions, assignmentDatabaseCollection.DBAssignment.FirstOrDefault(), true);

                    if (assignmentDetail.AssignmentNotes?.Where(x => !string.IsNullOrEmpty(x.RecordStatus))?.Any() == true && (response == null || response?.Code == MessageType.Success.ToId()))
                        response = this.ProcessAssignmentNote(assignmentDetail.AssignmentNotes, ref assignmentDatabaseCollection);

                    if (assignmentDetail.AssignmentDocuments?.Where(x => !string.IsNullOrEmpty(x.RecordStatus))?.Any() == true && (response == null || response?.Code == MessageType.Success.ToId()))
                        response = this.ProcessAssignmentDocument(assignmentDetail.AssignmentDocuments, validationType, ref dbDocuments);

                    return response ?? new Response().ToPopulate(ResponseType.Success, null, null, null, null, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentDetail);
            }
            return response ?? new Response().ToPopulate(ResponseType.Success, null, null, null, null, null);
        }

        private Response ProcessEmailNotifications(DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection,
                                                   DomainModel.AssignmentDetail assignmentDetails,
                                                   int? assignmentNumber,
                                                   ref ModuleDocument moduleDocument,
                                                   string token)
        {
            Exception exception = null;
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            IList<KeyValuePair<string, string>> emailContentPlaceholders = null;
            string companyCode = string.Empty;
            string subject = string.Empty;
            try
            {


                if (assignmentDatabaseCollection != null)
                {
                    var emailTemplateContent = _assignmentRepository.GetCompanyMessage(assignmentDetails.AssignmentInfo?.AssignmentContractHoldingCompanyCode);
                    if (string.IsNullOrEmpty(emailTemplateContent))
                        emailTemplateContent = _assignmentRepository.MailTemplate()?.FirstOrDefault()?.KeyValue;
                    var fromEmails = new List<EmailAddress>();
                    var toEmails = new List<EmailAddress>();
                    var ccEmails = new List<EmailAddress>();
                    var bodyPlaceHolders = new List<EmailPlaceHolderItem>();
                    var subjectPlaceHolders = new List<EmailPlaceHolderItem>();
                    if (!string.IsNullOrEmpty(emailTemplateContent))
                        subject = AssignmentConstants.Email_Notification_Assignment_Creation_Subject_Company_Based;
                    else
                        subject = AssignmentConstants.Email_Notification_Assignment_Creation_Subject;

                    if (!string.IsNullOrEmpty(assignmentDetails.AssignmentInfo?.AssignmentContractCompanyCoordinatorEmail))
                        fromEmails?.Add(new EmailAddress() { Address = assignmentDetails.AssignmentInfo?.AssignmentContractCompanyCoordinatorEmail?.Trim(), DisplayName = assignmentDetails.AssignmentInfo?.AssignmentContractHoldingCompanyCoordinator?.ToString() });
                    if (!string.IsNullOrEmpty(assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinatorEmail))
                        toEmails?.Add(new EmailAddress() { Address = assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinatorEmail?.Trim(), DisplayName = assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinator?.ToString() });

                    if (string.IsNullOrEmpty(assignmentDetails.AssignmentInfo?.AssignmentContractCompanyCoordinatorEmail))
                    {
                        var contractEmail = this._userService.GetUsersByCompanyAndName(assignmentDetails?.AssignmentInfo?.AssignmentContractHoldingCompanyCode, assignmentDetails?.AssignmentInfo?.AssignmentContractHoldingCompanyCoordinator)?.FirstOrDefault()?.Email.Trim();
                        if (!string.IsNullOrEmpty(contractEmail))
                            fromEmails?.Add(new EmailAddress() { Address = contractEmail, DisplayName = assignmentDetails.AssignmentInfo?.AssignmentContractHoldingCompanyCoordinator?.ToString() });
                    }

                    if (string.IsNullOrEmpty(assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinatorEmail))
                    {
                        var operatingEmail = this._userService.GetUsersByCompanyAndName(assignmentDetails?.AssignmentInfo?.AssignmentOperatingCompanyCode, assignmentDetails?.AssignmentInfo?.AssignmentOperatingCompanyCoordinator)?.FirstOrDefault()?.Email?.Trim();
                        if (!string.IsNullOrEmpty(operatingEmail))
                            toEmails?.Add(new EmailAddress() { Address = operatingEmail, DisplayName = assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinator?.ToString() });
                    }

                    subjectPlaceHolders.AddRange(new List<EmailPlaceHolderItem>()
                            {
                                new EmailPlaceHolderItem()
                                {
                                    PlaceHolderName = AssignmentConstants.Assignment_Number.ToString(),
                                    PlaceHolderValue =assignmentNumber.ToString(),
                                }
                            });
                    if (toEmails?.Count > 0)
                    {
                        emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                            new KeyValuePair<string, string>(AssignmentConstants.Assignment_Number, string.Format("{0:D5}", assignmentNumber)),
                                            new KeyValuePair<string, string>(AssignmentConstants.Project_Number, assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.Customer_Name, assignmentDetails?.AssignmentInfo?.AssignmentCustomerName?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.Contract_Coordinator_Name, assignmentDetails?.AssignmentInfo?.AssignmentContractHoldingCompanyCoordinator?.Trim()?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.Operating_Coordinator_Name, assignmentDetails?.AssignmentInfo?.AssignmentOperatingCompanyCoordinator?.Trim()?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.Supplier_PO, assignmentDetails?.AssignmentInfo?.AssignmentSupplierPurchaseOrderNumber?.Trim()?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.Contract_Company, assignmentDetails?.AssignmentInfo?.AssignmentContractHoldingCompany?.Trim()?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.Operating_Company, assignmentDetails?.AssignmentInfo?.AssignmentOperatingCompany?.Trim()?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.Contract_Number, assignmentDetails?.AssignmentInfo?.AssignmentContractNumber?.Trim()?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.Customer_Project_Name, assignmentDetails?.AssignmentInfo?.AssignmentCustomerProjectName?.Trim()?.ToString()),
                        };

                        if (emailContentPlaceholders?.Count > 0 && !string.IsNullOrEmpty(emailTemplateContent))
                        {
                            emailContentPlaceholders.ToList().ForEach(x =>
                            {
                                emailTemplateContent = emailTemplateContent.Replace(x.Key, x.Value);
                            });
                        }

                        emailMessage.CreatedOn = DateTime.UtcNow;
                        emailMessage.EmailType = EmailType.ICA.ToString();
                        emailMessage.ModuleCode = ModuleCodeType.ASGMNT.ToString();
                        emailMessage.ModuleEmailRefCode = assignmentDatabaseCollection.DBAssignment?.FirstOrDefault()?.Id.ToString();
                        emailMessage.Subject = ParseSubject(subject, subjectPlaceHolders);
                        emailMessage.Content = emailTemplateContent + "<br/>Token = " + token;//"Message: " + emailTemplateContent;
                        emailMessage.FromAddresses = fromEmails;
                        emailMessage.ToAddresses = toEmails;
                        emailMessage.CcAddresses = ccEmails;
                        emailMessage.Token = token;
                        //emailMessage.BodyPlaceHolderAndValue = PopulateBodyPlaceHolder(bodyPlaceHolders);
                        if (emailMessage?.ToAddresses?.Count > 0 && !string.IsNullOrEmpty(emailMessage.Content))
                            this._emailService.Add(new List<EmailQueueMessage> { emailMessage });

                        /*Store as a document*/
                        DocumentUniqueNameDetail documentUniquename = new DocumentUniqueNameDetail();
                        EmailDocumentUpload emailDocumentUpload = new EmailDocumentUpload();
                        documentUniquename.DocumentName = VisitTimesheetConstants.ASSIGNMENT_EMAIL_LOG;
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
                        documentMessage.AppendLine(emailMessage.Content);

                        documentUniquename.ModuleCode = ModuleCodeType.ASGMNT.ToString();
                        documentUniquename.RequestedBy = string.Empty;
                        documentUniquename.ModuleCodeReference = Convert.ToString(assignmentDatabaseCollection.DBAssignment?.FirstOrDefault()?.Id);
                        documentUniquename.DocumentType = VisitTimesheetConstants.VISIT_TIMSHEET_EVOLUTION_EMAIL;
                        documentUniquename.SubModuleCodeReference = "0";

                        emailDocumentUpload.IsDocumentUpload = true;
                        emailDocumentUpload.DocumentUniqueName = documentUniquename;
                        emailDocumentUpload.DocumentMessage = documentMessage.ToString().Replace("<p>", "").Replace("</p>", "<br/>").Replace("\r\n", "<br/>").Replace("\n", "<br/>");
                        emailDocumentUpload.IsVisibleToCustomer = false;
                        emailDocumentUpload.IsVisibleToTS = _assignmentRepository.GetTsVisible();

                        moduleDocument.DocumentName = emailDocumentUpload?.DocumentUniqueName?.DocumentName;
                        moduleDocument.DocumentType = emailDocumentUpload?.DocumentUniqueName?.DocumentType;
                        moduleDocument.IsVisibleToTS = emailDocumentUpload?.IsVisibleToTS;
                        moduleDocument.IsVisibleToCustomer = emailDocumentUpload?.IsVisibleToCustomer;
                        moduleDocument.Status = emailDocumentUpload?.DocumentUniqueName?.Status;
                        moduleDocument.Status = emailDocumentUpload?.DocumentUniqueName?.Status;
                        moduleDocument.ModuleCode = emailDocumentUpload?.DocumentUniqueName?.ModuleCode;
                        moduleDocument.ModuleRefCode = emailDocumentUpload?.DocumentUniqueName?.ModuleCodeReference;
                        moduleDocument.CreatedOn = DateTime.UtcNow;
                        moduleDocument.RecordStatus = "N";

                        this.PostRequest(emailDocumentUpload);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), emailMessage);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }




        private Response ProcessEmailNotificationsForAmendmentReason(DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection,
                                                   DomainModel.AssignmentDetail assignmentDetails,
                                                   int? assignmentNumber,
                                                   ref ModuleDocument moduleDocument,
                                                   string token, IList<DomainModel.AssignmentDetail> originalAssignment)
        {
            Exception exception = null;
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            IList<KeyValuePair<string, string>> emailContentPlaceholders = null;
            string companyCode = string.Empty;
            string subject = string.Empty;
            try
            {
                DomainModel.AssignmentDetail oldAssignmentDetails = originalAssignment[0];
                if (!(assignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDiscount?.ToString()).Equals((oldAssignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDiscount?.ToString())) || !(assignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDescription?.ToString()).Equals((oldAssignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDescription?.ToString())))
                {

                    if (assignmentDatabaseCollection != null)
                    {
                       /* var emailTemplateContent = _assignmentRepository.GetCompanyMessage(assignmentDetails.AssignmentInfo?.AssignmentContractHoldingCompanyCode);
                        if (string.IsNullOrEmpty(emailTemplateContent))*/
                        var emailTemplateContent =  _assignmentRepository.MailTemplateForInterCompanyAmendment()?.FirstOrDefault()?.KeyValue;
                        var fromEmails = new List<EmailAddress>();
                        var toEmails = new List<EmailAddress>();
                        var ccEmails = new List<EmailAddress>();
                        var bodyPlaceHolders = new List<EmailPlaceHolderItem>();
                        var subjectPlaceHolders = new List<EmailPlaceHolderItem>();
                        if (!string.IsNullOrEmpty(emailTemplateContent))
                            subject = AssignmentConstants.AMENDMENTREASON_SUBJECT;
                        else
                            subject = AssignmentConstants.Email_Notification_Assignment_Creation_Subject;

                        if (!string.IsNullOrEmpty(assignmentDetails.AssignmentInfo?.AssignmentContractCompanyCoordinatorEmail))
                            fromEmails?.Add(new EmailAddress() { Address = assignmentDetails.AssignmentInfo?.AssignmentContractCompanyCoordinatorEmail?.Trim(), DisplayName = assignmentDetails.AssignmentInfo?.AssignmentContractHoldingCompanyCoordinator?.ToString() });
                        if (!string.IsNullOrEmpty(assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinatorEmail))
                            toEmails?.Add(new EmailAddress() { Address = assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinatorEmail?.Trim(), DisplayName = assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinator?.ToString() });

                        if (string.IsNullOrEmpty(assignmentDetails.AssignmentInfo?.AssignmentContractCompanyCoordinatorEmail))
                        {
                            var contractEmail = this._userService.GetUsersByCompanyAndName(assignmentDetails?.AssignmentInfo?.AssignmentContractHoldingCompanyCode, assignmentDetails?.AssignmentInfo?.AssignmentContractHoldingCompanyCoordinator)?.FirstOrDefault()?.Email.Trim();
                            if (!string.IsNullOrEmpty(contractEmail))
                                fromEmails?.Add(new EmailAddress() { Address = contractEmail, DisplayName = assignmentDetails.AssignmentInfo?.AssignmentContractHoldingCompanyCoordinator?.ToString() });
                        }

                        if (string.IsNullOrEmpty(assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinatorEmail))
                        {
                            var operatingEmail = this._userService.GetUsersByCompanyAndName(assignmentDetails?.AssignmentInfo?.AssignmentOperatingCompanyCode, assignmentDetails?.AssignmentInfo?.AssignmentOperatingCompanyCoordinator)?.FirstOrDefault()?.Email?.Trim();
                            if (!string.IsNullOrEmpty(operatingEmail))
                                toEmails?.Add(new EmailAddress() { Address = operatingEmail, DisplayName = assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinator?.ToString() });
                        }

                        subjectPlaceHolders.AddRange(new List<EmailPlaceHolderItem>()
                            {
                                new EmailPlaceHolderItem()
                                {
                                    PlaceHolderName = AssignmentConstants.AMENDMENTREASON.ToString(),
                                    PlaceHolderValue ="InterCompany Discount Amendment",
                                }
                            });
                        if (toEmails?.Count > 0)
                        {
                            emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                            new KeyValuePair<string, string>(AssignmentConstants.Customer_Name, assignmentDetails?.AssignmentInfo?.AssignmentCustomerName?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.Assignment_Number, string.Format("{0:D5}", assignmentNumber)),
                                            new KeyValuePair<string, string>(AssignmentConstants.Project_Number, assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.OldIntercompanyDiscount, oldAssignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDiscount.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.OldIntercompanyDescription, oldAssignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDescription.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.AMENDMENTEDPERCENTAGE, assignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDiscount?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.AMENDMENTED_DESCRIPTION, assignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDescription.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.AMENDMENT_REASON_DESC, assignmentDetails?.AssignmentInterCompanyDiscounts.AmendmentReason),
                                            new KeyValuePair<string, string>(AssignmentConstants.Contract_Coordinator_Name, assignmentDetails?.AssignmentInfo?.AssignmentContractHoldingCompanyCoordinator),
                                            new KeyValuePair<string, string>(AssignmentConstants.MAILTOKEN, DateTime.Now.ToString(VisitTimesheetConstants.TOKEN_DATE_FORMAT))
                            };

                            if (emailContentPlaceholders?.Count > 0 && !string.IsNullOrEmpty(emailTemplateContent))
                            {
                                emailContentPlaceholders.ToList().ForEach(x =>
                                {
                                    emailTemplateContent = emailTemplateContent.Replace(x.Key, x.Value);
                                });
                            }

                            emailMessage.CreatedOn = DateTime.UtcNow;
                            emailMessage.EmailType = EmailType.ICR.ToString();
                            emailMessage.ModuleCode = ModuleCodeType.ASGMNT.ToString();
                            emailMessage.ModuleEmailRefCode = assignmentDatabaseCollection.DBAssignment?.FirstOrDefault()?.Id.ToString();
                            emailMessage.Subject = ParseSubject(subject, subjectPlaceHolders);
                            emailMessage.Content = emailTemplateContent; //+ "<br/>Token = " + token + "</p>";//"Message: " + emailTemplateContent;
                            emailMessage.FromAddresses = fromEmails;
                            emailMessage.ToAddresses = toEmails;
                            //emailMessage.BodyPlaceHolderAndValue = PopulateBodyPlaceHolder(bodyPlaceHolders);
                            if (emailMessage?.ToAddresses?.Count > 0 && !string.IsNullOrEmpty(emailMessage.Content))
                                this._emailService.Add(new List<EmailQueueMessage> { emailMessage });

                            /*Store as a document*/
                            DocumentUniqueNameDetail documentUniquename = new DocumentUniqueNameDetail();
                            EmailDocumentUpload emailDocumentUpload = new EmailDocumentUpload();
                            documentUniquename.DocumentName = VisitTimesheetConstants.AMENDMENT_EMAIL_LOG;
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
                            documentMessage.AppendLine(emailMessage.Content);

                            documentUniquename.ModuleCode = ModuleCodeType.ASGMNT.ToString();
                            documentUniquename.RequestedBy = string.Empty;
                            documentUniquename.ModuleCodeReference = Convert.ToString(assignmentDatabaseCollection.DBAssignment?.FirstOrDefault()?.Id);
                            documentUniquename.DocumentType = VisitTimesheetConstants.VISIT_TIMSHEET_EVOLUTION_EMAIL;
                            documentUniquename.SubModuleCodeReference = "0";

                            emailDocumentUpload.IsDocumentUpload = true;
                            emailDocumentUpload.DocumentUniqueName = documentUniquename;
                            emailDocumentUpload.DocumentMessage = documentMessage.ToString().Replace("<p>", "").Replace("</p>", "<br/>").Replace("\r\n", "<br/>").Replace("\n", "<br/>");
                            emailDocumentUpload.IsVisibleToCustomer = false;
                            emailDocumentUpload.IsVisibleToTS = _assignmentRepository.GetTsVisible();

                            moduleDocument.DocumentName = emailDocumentUpload?.DocumentUniqueName?.DocumentName;
                            moduleDocument.DocumentType = emailDocumentUpload?.DocumentUniqueName?.DocumentType;
                            moduleDocument.IsVisibleToTS = emailDocumentUpload?.IsVisibleToTS;
                            moduleDocument.IsVisibleToCustomer = emailDocumentUpload?.IsVisibleToCustomer;
                            moduleDocument.Status = emailDocumentUpload?.DocumentUniqueName?.Status;
                            moduleDocument.Status = emailDocumentUpload?.DocumentUniqueName?.Status;
                            moduleDocument.ModuleCode = emailDocumentUpload?.DocumentUniqueName?.ModuleCode;
                            moduleDocument.ModuleRefCode = emailDocumentUpload?.DocumentUniqueName?.ModuleCodeReference;
                            moduleDocument.CreatedOn = DateTime.UtcNow;
                            moduleDocument.RecordStatus = "N";

                            this.PostRequest(emailDocumentUpload);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), emailMessage);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessEmailForAmendmentReasonForNewAssignment(DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection,
                                                   DomainModel.AssignmentDetail assignmentDetails,
                                                   int? assignmentNumber,
                                                   ref ModuleDocument moduleDocument,
                                                   string token, IList<DomainModel.AssignmentDetail> originalAssignment)
        {
            Exception exception = null;
            EmailQueueMessage emailMessage = new EmailQueueMessage();
            IList<KeyValuePair<string, string>> emailContentPlaceholders = null;
            string companyCode = string.Empty;
            string subject = string.Empty;
            try
            {

               /* if (!(assignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDiscount?.ToString()).Equals((oldAssignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDiscount?.ToString())) || !(assignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDescription?.ToString()).Equals((oldAssignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDescription?.ToString())))
                {*/

                    if (assignmentDatabaseCollection != null)
                    {
                        /* var emailTemplateContent = _assignmentRepository.GetCompanyMessage(assignmentDetails.AssignmentInfo?.AssignmentContractHoldingCompanyCode);
                         if (string.IsNullOrEmpty(emailTemplateContent))*/
                        var emailTemplateContent = _assignmentRepository.MailTemplateForInterCompanyAmendment()?.FirstOrDefault()?.KeyValue;
                        var fromEmails = new List<EmailAddress>();
                        var toEmails = new List<EmailAddress>();
                        var ccEmails = new List<EmailAddress>();
                        var bodyPlaceHolders = new List<EmailPlaceHolderItem>();
                        var subjectPlaceHolders = new List<EmailPlaceHolderItem>();
                        if (!string.IsNullOrEmpty(emailTemplateContent))
                            subject = AssignmentConstants.AMENDMENTREASON_SUBJECT;
                        else
                            subject = AssignmentConstants.Email_Notification_Assignment_Creation_Subject;

                        if (!string.IsNullOrEmpty(assignmentDetails.AssignmentInfo?.AssignmentContractCompanyCoordinatorEmail))
                            fromEmails?.Add(new EmailAddress() { Address = assignmentDetails.AssignmentInfo?.AssignmentContractCompanyCoordinatorEmail?.Trim(), DisplayName = assignmentDetails.AssignmentInfo?.AssignmentContractHoldingCompanyCoordinator?.ToString() });
                        if (!string.IsNullOrEmpty(assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinatorEmail))
                            toEmails?.Add(new EmailAddress() { Address = assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinatorEmail?.Trim(), DisplayName = assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinator?.ToString() });

                        if (string.IsNullOrEmpty(assignmentDetails.AssignmentInfo?.AssignmentContractCompanyCoordinatorEmail))
                        {
                            var contractEmail = this._userService.GetUsersByCompanyAndName(assignmentDetails?.AssignmentInfo?.AssignmentContractHoldingCompanyCode, assignmentDetails?.AssignmentInfo?.AssignmentContractHoldingCompanyCoordinator)?.FirstOrDefault()?.Email.Trim();
                            if (!string.IsNullOrEmpty(contractEmail))
                                fromEmails?.Add(new EmailAddress() { Address = contractEmail, DisplayName = assignmentDetails.AssignmentInfo?.AssignmentContractHoldingCompanyCoordinator?.ToString() });
                        }

                        if (string.IsNullOrEmpty(assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinatorEmail))
                        {
                            var operatingEmail = this._userService.GetUsersByCompanyAndName(assignmentDetails?.AssignmentInfo?.AssignmentOperatingCompanyCode, assignmentDetails?.AssignmentInfo?.AssignmentOperatingCompanyCoordinator)?.FirstOrDefault()?.Email?.Trim();
                            if (!string.IsNullOrEmpty(operatingEmail))
                                toEmails?.Add(new EmailAddress() { Address = operatingEmail, DisplayName = assignmentDetails.AssignmentInfo?.AssignmentOperatingCompanyCoordinator?.ToString() });
                        }
                        string defaultPercentage = "15.00";
                        string defaultDescription = "InterCo Discount";
                        subjectPlaceHolders.AddRange(new List<EmailPlaceHolderItem>()
                            {
                                new EmailPlaceHolderItem()
                                {
                                    PlaceHolderName = AssignmentConstants.AMENDMENTREASON.ToString(),
                                    PlaceHolderValue ="InterCompany Discount Amendment",
                                }
                            });
                        if (toEmails?.Count > 0)
                        {
                            emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                            new KeyValuePair<string, string>(AssignmentConstants.Customer_Name, assignmentDetails?.AssignmentInfo?.AssignmentCustomerName?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.Assignment_Number, string.Format("{0:D5}", assignmentNumber)),
                                            new KeyValuePair<string, string>(AssignmentConstants.Project_Number, assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.OldIntercompanyDiscount, defaultPercentage),
                                            new KeyValuePair<string, string>(AssignmentConstants.OldIntercompanyDescription, defaultDescription),
                                            new KeyValuePair<string, string>(AssignmentConstants.AMENDMENTEDPERCENTAGE, assignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDiscount?.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.AMENDMENTED_DESCRIPTION, assignmentDetails?.AssignmentInterCompanyDiscounts?.AssignmentContractHoldingCompanyDescription.ToString()),
                                            new KeyValuePair<string, string>(AssignmentConstants.AMENDMENT_REASON_DESC, assignmentDetails?.AssignmentInterCompanyDiscounts.AmendmentReason),
                                            new KeyValuePair<string, string>(AssignmentConstants.Contract_Coordinator_Name, assignmentDetails?.AssignmentInfo?.AssignmentContractHoldingCompanyCoordinator),
                                            new KeyValuePair<string, string>(AssignmentConstants.MAILTOKEN, DateTime.Now.ToString(VisitTimesheetConstants.TOKEN_DATE_FORMAT))
                            };

                            if (emailContentPlaceholders?.Count > 0 && !string.IsNullOrEmpty(emailTemplateContent))
                            {
                                emailContentPlaceholders.ToList().ForEach(x =>
                                {
                                    emailTemplateContent = emailTemplateContent.Replace(x.Key, x.Value);
                                });
                            }

                            emailMessage.CreatedOn = DateTime.UtcNow;
                            emailMessage.EmailType = EmailType.ICR.ToString();
                            emailMessage.ModuleCode = ModuleCodeType.ASGMNT.ToString();
                            emailMessage.ModuleEmailRefCode = assignmentDatabaseCollection.DBAssignment?.FirstOrDefault()?.Id.ToString();
                            emailMessage.Subject = ParseSubject(subject, subjectPlaceHolders);
                            emailMessage.Content = emailTemplateContent; //+ "<br/>Token = " + token + "</p>";//"Message: " + emailTemplateContent;
                            emailMessage.FromAddresses = fromEmails;
                            emailMessage.ToAddresses = toEmails;
                            //emailMessage.BodyPlaceHolderAndValue = PopulateBodyPlaceHolder(bodyPlaceHolders);
                            if (emailMessage?.ToAddresses?.Count > 0 && !string.IsNullOrEmpty(emailMessage.Content))
                                this._emailService.Add(new List<EmailQueueMessage> { emailMessage });

                            /*Store as a document*/
                            DocumentUniqueNameDetail documentUniquename = new DocumentUniqueNameDetail();
                            EmailDocumentUpload emailDocumentUpload = new EmailDocumentUpload();
                            documentUniquename.DocumentName = AssignmentConstants.AMENDMENT_EMAIL_LOG;
                            StringBuilder documentMessage = new StringBuilder();
                            if (emailMessage.FromAddresses != null && emailMessage.FromAddresses.Count > 0)
                            {
                                documentMessage.Append(AssignmentConstants.EMAIL_FROM);
                                documentMessage.AppendLine(emailMessage.FromAddresses[0].DisplayName + " <span><</span>" + emailMessage.FromAddresses[0].Address + "<span>></span>");
                            }
                            if (emailMessage.ToAddresses != null && emailMessage.ToAddresses.Count > 0)
                            {
                                documentMessage.Append(AssignmentConstants.EMAIL_TO);
                                documentMessage.AppendLine(emailMessage.ToAddresses[0].DisplayName + " <span><</span>" + emailMessage.ToAddresses[0].Address + "<span>></span>");
                            }
                            documentMessage.Append(AssignmentConstants.EMAIL_SUBJECT);
                            documentMessage.AppendLine(emailMessage.Subject);
                            documentMessage.AppendLine(emailMessage.Content);

                            documentUniquename.ModuleCode = ModuleCodeType.ASGMNT.ToString();
                            documentUniquename.RequestedBy = string.Empty;
                            documentUniquename.ModuleCodeReference = Convert.ToString(assignmentDatabaseCollection.DBAssignment?.FirstOrDefault()?.Id);
                            documentUniquename.DocumentType = AssignmentConstants.ASSIGNEMNT_EVOLUTION_EMAIL;
                            documentUniquename.SubModuleCodeReference = "0";

                            emailDocumentUpload.IsDocumentUpload = true;
                            emailDocumentUpload.DocumentUniqueName = documentUniquename;
                            emailDocumentUpload.DocumentMessage = documentMessage.ToString().Replace("<p>", "").Replace("</p>", "<br/>").Replace("\r\n", "<br/>").Replace("\n", "<br/>");
                            emailDocumentUpload.IsVisibleToCustomer = false;
                            emailDocumentUpload.IsVisibleToTS = _assignmentRepository.GetTsVisible();

                            moduleDocument.DocumentName = emailDocumentUpload?.DocumentUniqueName?.DocumentName;
                            moduleDocument.DocumentType = emailDocumentUpload?.DocumentUniqueName?.DocumentType;
                            moduleDocument.IsVisibleToTS = emailDocumentUpload?.IsVisibleToTS;
                            moduleDocument.IsVisibleToCustomer = emailDocumentUpload?.IsVisibleToCustomer;
                            moduleDocument.Status = emailDocumentUpload?.DocumentUniqueName?.Status;
                            moduleDocument.Status = emailDocumentUpload?.DocumentUniqueName?.Status;
                            moduleDocument.ModuleCode = emailDocumentUpload?.DocumentUniqueName?.ModuleCode;
                            moduleDocument.ModuleRefCode = emailDocumentUpload?.DocumentUniqueName?.ModuleCodeReference;
                            moduleDocument.CreatedOn = DateTime.UtcNow;
                            moduleDocument.RecordStatus = "N";

                            this.PostRequest(emailDocumentUpload);
                        }
                    }
             //   }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), emailMessage);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }


        private Response PostRequest(EmailDocumentUpload model)
        {
            Exception exception = null;
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                // Pass the handler to httpclient(to call api)
                using (var httpClient = new HttpClient(clientHandler))
                {
                    string url = _environment.ApplicationGatewayURL + _emailDocumentEndpoint;
                    var uri = new Uri(url);
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                    var response = httpClient.PostAsync(uri, content);
                    if (!response.Result.IsSuccessStatusCode)
                        _logger.LogError(ResponseType.Exception.ToId(), response?.Result?.ReasonPhrase, model);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ResourceAssignmentEmailNotification(IList<DbModel.Assignment> assignment, DomainModel.AssignmentDetail assignmentDetail, int? assignmentNumber, string token)
        {
            string emailSubject = string.Empty;
            Exception exception = null;
            EmailQueueMessage emailMessage = null;
            List<EmailAddress> toAddresses = null;
            List<EmailAddress> ccAddresses = null;
            List<EmailQueueMessage> emailQueueMessage = new List<EmailQueueMessage>();
            IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists = new List<DomainModel.AssignmentTechnicalSpecialist>();
            try
            {
                if (assignmentDetail != null)
                {
                    assignmentTechnicalSpecialists = assignmentDetail.AssignmentTechnicalSpecialists;
                    assignmentTechnicalSpecialists = assignmentDetail.AssignmentTechnicalSpecialists?.Where(x1 => x1.RecordStatus.IsRecordStatusNew())?.Select(x => { x.AssignmentTechnicalSplId = null; return x; })?.ToList();
                    if (assignmentTechnicalSpecialists != null && assignmentTechnicalSpecialists?.Count > 0)
                    {
                        var epins = assignmentTechnicalSpecialists.Select(x => x.Epin).Distinct().ToList();
                        if (assignment != null)
                        {
                            var tsTstContactInfos = _technicalSpecialistContactRepository.FindBy(x => epins.Contains(x.TechnicalSpecialist.Id) && x.ContactType == ContactType.PrimaryEmail.ToString()).ToList();
                            assignmentTechnicalSpecialists.ToList().ForEach(x =>
                            {
                                //Sending ResourceAssignmentNotification Emails 
                                var dbAssignment = assignment?.FirstOrDefault(x1 => x1.Id == x.AssignmentId);
                                if (dbAssignment != null)
                                {
                                    if (dbAssignment?.ContractCompanyId != dbAssignment.OperatingCompanyId)
                                    {
                                        ccAddresses = new List<EmailAddress> {
                                        new EmailAddress() { Address = dbAssignment?.ContractCompanyCoordinator?.Email }
                                    };
                                    }

                                    toAddresses = tsTstContactInfos?.Where(x1 => x1.TechnicalSpecialistId == x.Epin && !string.IsNullOrEmpty(x1.EmailAddress))?.Select(x1 => new EmailAddress() { Address = x1.EmailAddress }).ToList();
                                    emailSubject = string.Format(TechnicalSpecialistConstants.Email_Notification_ResourceAssignmentNotification_Subject, assignmentDetail.AssignmentInfo?.AssignmentProjectNumber?.ToString(), assignmentNumber?.ToString());

                                    var emailContentPlaceholders = new List<KeyValuePair<string, string>> {
                                        new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Customer_Name, assignmentDetail.AssignmentInfo?.AssignmentCustomerName),
                                        new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Project_Number, assignmentDetail.AssignmentInfo?.AssignmentProjectNumber?.ToString()),
                                        new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Assignment_Number, assignmentNumber?.ToString())
                                    };
                                    emailMessage = _emailService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, EmailTemplate.EmailResourceAssignmentNotification, EmailType.RAA, ModuleCodeType.ASGMNT, x?.AssignmentId.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses, IsMailSendAsGroup: true);
                                }
                                if (emailMessage != null)
                                    emailQueueMessage.Add(emailMessage);
                            });
                        }

                        return _emailService.Add(emailQueueMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
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

        private Response ProcessAssignmentSupplier(IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers, bool IsSupplierPOChanged, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ValidationType validationType)
        {
            Response response = null;
            bool IsMainSupplierSaved = false;
            try
            {
                var filterAddSubSuppliers = assignmentSubSuppliers?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var filterModifySubSuppliers = assignmentSubSuppliers?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var filterDeleteSubSuppliers = assignmentSubSuppliers?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (filterModifySubSuppliers?.Any() == true)
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && assignmentDatabaseCollection.DBAssignmentSubSupplier != null)//MS-TS Link
                        response = this.UpdateAssignmentSubSupplier(filterModifySubSuppliers, ref assignmentDatabaseCollection, ref filterAddSubSuppliers, true, ref IsMainSupplierSaved, IsSupplierPOChanged);

                /* This section is used only to update the main Supplier */
                int? assignmentsubSupplierId = assignmentDatabaseCollection.DBAssignmentSubSupplier?.FirstOrDefault(x => x.IsDeleted == false && x.SupplierType == SupplierType.MainSupplier.FirstChar() && x.AssignmentId == assignmentSubSuppliers.FirstOrDefault().AssignmentId)?.Id;

                if (assignmentsubSupplierId > 0 && filterModifySubSuppliers?.Any() == false) // && filterDeleteSubSuppliers?.Any() == true
                    response = this.UpdateMainSupplier(assignmentSubSuppliers.Where(x => x.MainSupplierId > 0).Take(1).ToList(), assignmentDatabaseCollection, IsSupplierPOChanged, true, ref IsMainSupplierSaved);

                if (filterDeleteSubSuppliers?.Any() == true)
                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && assignmentDatabaseCollection.DBAssignmentSubSupplier != null)//MS-TS Link
                    {
                        if (IsSupplierPOChanged == true)
                            response = this.UpdateAssignmentSubSupplier(filterDeleteSubSuppliers, ref assignmentDatabaseCollection, ref filterAddSubSuppliers, true, ref IsMainSupplierSaved, IsSupplierPOChanged);
                        else
                            response = this.DeleteAssignmentSubSupplier(filterDeleteSubSuppliers, assignmentDatabaseCollection, true, IsSupplierPOChanged, IsMainSupplierSaved);
                        //response = this.DeleteAssignmentSubSupplier(filterDeleteSubSuppliers, assignmentDatabaseCollection.DBAssignment, dbAssignmentSubSupplier, true, filterAddSubSuppliers);
                    }

                if (filterAddSubSuppliers?.Any() == true)
                    if (ValidationType.Delete != validationType)
                        response = this.AddAssignmentSubSupplier(filterAddSubSuppliers, ref assignmentDatabaseCollection, true, IsSupplierPOChanged, IsSupplierPOChanged == true ? false : IsMainSupplierSaved);

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSuppliers);
            }

            return response;
        }

        private Response AddAssignmentSubSupplier(List<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange, bool IsSupplierPOChanged, bool IsMainSupplierSaved)
        {
            Exception exception = null;
            List<DomainModel.AssignmentSubSupplier> addAssignmentSubSuppliers = null;
            try
            {
                List<DomainModel.AssignmentSubSupplier> mainSupplierDetailList = new List<DomainModel.AssignmentSubSupplier>();
                if (assignmentSubSuppliers?.Count > 0)
                {
                    var dbAssignmentTs = assignmentDatabaseCollection.DBAssignment.SelectMany(x => x.AssignmentTechnicalSpecialist).ToList();
                    _assignmentSubSupplierRepository.AutoSave = false;

                    /* This section is used only to update the main Supplier when it comes with N status only when we edit Tech Specialist for that Main Supplier*/
                    //int? assignmentsubSupplierId = assignmentDatabaseCollection.DBAssignmentSubSupplier?.FirstOrDefault(x => x.IsDeleted == false && x.SupplierType == SupplierType.MainSupplier.FirstChar() && x.AssignmentId == assignmentSubSuppliers.FirstOrDefault().AssignmentId)?.Id;
                    //if (assignmentsubSupplierId > 0) 
                    //    this.UpdateMainSupplier(assignmentSubSuppliers.Where(x => x.MainSupplierId > 0).Take(1).ToList().Select(y=> { y.RecordStatus = "M"; return y; }).ToList(), assignmentDatabaseCollection, IsSupplierPOChanged, true, ref IsMainSupplierSaved);

                    var result = AssignMainAndSubSupplier(assignmentSubSuppliers, assignmentDatabaseCollection, ref addAssignmentSubSuppliers, assignmentSubSuppliers?.FirstOrDefault()?.SubSupplierId != null ? true : false, IsSupplierPOChanged, ref IsMainSupplierSaved);
                    assignmentDatabaseCollection.DBAssignmentSubSupplier = _mapper.Map<IList<DbModel.AssignmentSubSupplier>>(result, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["AssignmentTsId"] = dbAssignmentTs;
                        opt.Items["IsDeleted"] = false;
                    });
                    assignmentDatabaseCollection.DBAssignmentSubSupplier = assignmentDatabaseCollection.DBAssignmentSubSupplier?.ToList().Select(x => { x.Id = 0; return x; })?.ToList();
                    _assignmentSubSupplierRepository.Add(assignmentDatabaseCollection.DBAssignmentSubSupplier);
                    if (commitChange && assignmentSubSuppliers.Count > 0)
                        _assignmentSubSupplierRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSuppliers);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response UpdateMainSupplier(List<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers, DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool IsSupplierPOChanged, bool commitChange, ref bool IsMainSupplierSaved)
        {
            Exception exception = null;
            try
            {
                List<DomainModel.AssignmentSubSupplier> mainSupplierDetailList = new List<DomainModel.AssignmentSubSupplier>();
                /*Note- Main Supplier manual mapping to accomodate in Domain Mapper(Domain To Db Model) */
                if (assignmentSubSuppliers?.Count > 0)
                {
                    int? assignmentsubSupplierId = assignmentDatabaseCollection.DBAssignmentSubSupplier?.FirstOrDefault(x => x.IsDeleted == false && x.SupplierType == SupplierType.MainSupplier.FirstChar() && x.AssignmentId == assignmentSubSuppliers.FirstOrDefault().AssignmentId)?.Id;
                    assignmentSubSuppliers.ToList().ForEach(x =>
                    {
                        DomainModel.AssignmentSubSupplier mainSupplier = new DomainModel.AssignmentSubSupplier
                        {
                            AssignmentSubSupplierId = assignmentsubSupplierId > 0 ? assignmentsubSupplierId : x.AssignmentSubSupplierId,
                            AssignmentSubSupplierTS = x.AssignmentSubSupplierTS?.Where(x1 => x1.IsAssignedToThisSubSupplier == false).ToList(),
                            SupplierType = SupplierType.MainSupplier.FirstChar(),
                            MainSupplierId = x.MainSupplierId,
                            AssignmentId = assignmentSubSuppliers.FirstOrDefault().AssignmentId,
                            MainSupplierContactId = assignmentSubSuppliers.FirstOrDefault().MainSupplierContactId,
                            IsMainSupplierFirstVisit = x.IsMainSupplierFirstVisit ?? false,
                            IsDeleted = IsSupplierPOChanged,
                            ModifiedBy = assignmentSubSuppliers.FirstOrDefault().ModifiedBy
                        };
                        mainSupplierDetailList.Add(mainSupplier);
                    });

                    var assignmentSubSupplierToUpdate = assignmentDatabaseCollection.DBAssignmentSubSupplier.ToList().Where(x => mainSupplierDetailList.Select(x1 => x1.MainSupplierId).Contains(x.SupplierId)).ToList();

                    assignmentSubSupplierToUpdate.ToList().ForEach(dbAssignmentSubSupplier =>
                    {
                        var assignmentSubSupplierToModify = mainSupplierDetailList?.FirstOrDefault(x => x.MainSupplierId == dbAssignmentSubSupplier.SupplierId);
                        if (assignmentSubSupplierToModify != null)
                        {
                            dbAssignmentSubSupplier.AssignmentId = (int)assignmentSubSupplierToModify.AssignmentId;
                            dbAssignmentSubSupplier.SupplierId = (int)assignmentSubSupplierToModify.MainSupplierId;
                            dbAssignmentSubSupplier.SupplierType = SupplierType.MainSupplier.FirstChar();
                            dbAssignmentSubSupplier.SupplierContactId = assignmentSubSupplierToModify.MainSupplierContactId;
                            dbAssignmentSubSupplier.IsFirstVisit = assignmentSubSupplierToModify.IsMainSupplierFirstVisit;
                            dbAssignmentSubSupplier.IsDeleted = IsSupplierPOChanged;
                            dbAssignmentSubSupplier.LastModification = DateTime.UtcNow;
                            dbAssignmentSubSupplier.UpdateCount = assignmentSubSupplierToModify.UpdateCount.CalculateUpdateCount();
                            dbAssignmentSubSupplier.ModifiedBy = assignmentSubSupplierToModify.ModifiedBy;
                            assignmentSubSupplierToModify?.AssignmentSubSupplierTS?.ForEach(x =>
                            {
                                ProcessSubSupplierTechnicalSpecialist(x,
                                                                         dbAssignmentSubSupplier.AssignmentSubSupplierTechnicalSpecialist.ToList(),
                                                                         assignmentDatabaseCollection.DBAssignment?.SelectMany(x1 => x1.AssignmentTechnicalSpecialist)?.ToList(),
                                                                         IsSupplierPOChanged
                                                                         );
                            });
                        }
                    });

                    _assignmentSubSupplierRepository.AutoSave = false;
                    _assignmentSubSupplierRepository.Update(assignmentSubSupplierToUpdate);
                    if (commitChange)
                    {
                        _assignmentSubSupplierRepository.ForceSave();
                        _assignmentSubSupplerTsRepository.ForceSave();
                        IsMainSupplierSaved = true;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSuppliers);
            }
            finally
            {
                _assignmentSubSupplierRepository.AutoSave = true;
                _assignmentSubSupplerTsRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private List<DomainModel.AssignmentSubSupplier> AssignMainAndSubSupplier(List<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers, DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ref List<DomainModel.AssignmentSubSupplier> filterAddSubSuppliers, bool isSubSupplierPresent, bool IsSupplierPOChanged, ref bool IsMainSupplierSaved)
        {
            List<DomainModel.AssignmentSubSupplier> mainSupplierDetailList = new List<DomainModel.AssignmentSubSupplier>();

            /*Note- Main Supplier manual mapping to accomodate in Domain Mapper(Domain To Db Model) */
            int? assignmentsubSupplierId = assignmentDatabaseCollection.DBAssignmentSubSupplier?.FirstOrDefault(x => x.IsDeleted == false && x.SupplierType == SupplierType.MainSupplier.FirstChar() && x.AssignmentId == assignmentSubSuppliers.FirstOrDefault().AssignmentId)?.Id;
            if (IsMainSupplierSaved == false) /* This flag will turn true when we have performed update on main Supplier when we have node where MainSupplier is already saved and !st SubSupplier is added newly */
            {
                assignmentSubSuppliers.Where(x => x.MainSupplierId > 0).Take(1).ToList().ForEach(x =>
                {
                    var techSpec = x.AssignmentSubSupplierTS?.Where(x1 => x1.IsAssignedToThisSubSupplier == false).ToList();
                    if (techSpec?.Count > 0)
                        techSpec.Select(y => { y.AssignmentSubSupplierId = assignmentsubSupplierId > 0 ? assignmentsubSupplierId : x.AssignmentSubSupplierId; return y; }).ToList();
                    DomainModel.AssignmentSubSupplier mainSupplier = new DomainModel.AssignmentSubSupplier
                    {
                        AssignmentSubSupplierId = assignmentsubSupplierId > 0 ? assignmentsubSupplierId : x.AssignmentSubSupplierId,
                        AssignmentSubSupplierTS = techSpec,//x.AssignmentSubSupplierTS?.Where(x1 => x1.IsAssignedToThisSubSupplier == false).ToList(),
                        SupplierType = SupplierType.MainSupplier.FirstChar(),
                        MainSupplierId = x.MainSupplierId,
                        AssignmentId = assignmentSubSuppliers.FirstOrDefault().AssignmentId,
                        MainSupplierContactId = assignmentSubSuppliers.FirstOrDefault().MainSupplierContactId,
                        IsMainSupplierFirstVisit = x.IsMainSupplierFirstVisit ?? false,
                        IsDeleted = IsSupplierPOChanged,
                        ModifiedBy = assignmentSubSuppliers.FirstOrDefault().ModifiedBy
                    };
                    mainSupplierDetailList.Add(mainSupplier);
                });
                if (!IsSupplierPOChanged)
                    IsMainSupplierSaved = true;
            }

            /*Note- All sub suppliers are mapped to Main Supplier line Items to accomodate mapping in Domain Mapper(Domain To Db Model) */
            if (isSubSupplierPresent == true)
            {
                var dbAssignmentSupplier = assignmentDatabaseCollection.DBAssignmentSubSupplier?.Where(x => x.IsDeleted == false && x.SupplierType == SupplierType.SubSupplier.FirstChar() && x.AssignmentId == assignmentSubSuppliers.FirstOrDefault().AssignmentId)?.ToList();
                //if (dbAssignmentSupplier?.Count > 0)
                //{
                assignmentSubSuppliers.ToList().ForEach(x =>
                {
                    var techSpec = x.AssignmentSubSupplierTS?.Where(x1 => x1.IsAssignedToThisSubSupplier == true).ToList();
                    if (techSpec?.Count > 0)
                        techSpec.Select(y => { y.AssignmentSubSupplierId = dbAssignmentSupplier?.FirstOrDefault(x1 => x1.SupplierId == x.SubSupplierId)?.Id; return y; }).ToList();

                    x.AssignmentSubSupplierTS = techSpec;
                    x.AssignmentSubSupplierId = dbAssignmentSupplier?.FirstOrDefault(x1 => x1.SupplierId == x.SubSupplierId)?.Id;
                    x.MainSupplierContactId = x.SubSupplierContactId;
                    x.MainSupplierId = x.SubSupplierId;
                    x.AssignmentId = assignmentSubSuppliers.FirstOrDefault().AssignmentId;
                    x.IsMainSupplierFirstVisit = x.IsSubSupplierFirstVisit ?? false;
                    x.SupplierType = SupplierType.SubSupplier.FirstChar();//MS-TS Link
                    x.IsDeleted = IsSupplierPOChanged;
                    x.ModifiedBy = assignmentSubSuppliers.FirstOrDefault().ModifiedBy;
                });
                //}

                if (assignmentSubSuppliers?.Count == 1 && dbAssignmentSupplier?.Count == 0)
                {
                    if (filterAddSubSuppliers?.Count == 0)
                        filterAddSubSuppliers = assignmentSubSuppliers?.Where(x1 => x1.RecordStatus.IsRecordStatusModified())?.ToList()?.Select(x2 => { x2.AssignmentSubSupplierId = null; x2.MainSupplierContactId = null; x2.MainSupplierId = null; x2.RecordStatus = "N"; return x2; }).ToList();
                    else if (filterAddSubSuppliers?.Count > 0)
                    {
                        filterAddSubSuppliers = filterAddSubSuppliers?.Select(x2 => { x2.AssignmentSubSupplierId = null; x2.MainSupplierContactId = null; x2.MainSupplierId = null; x2.RecordStatus = "N"; return x2; }).ToList();
                        filterAddSubSuppliers.AddRange(assignmentSubSuppliers?.Where(x1 => x1.RecordStatus.IsRecordStatusModified())?.ToList()?.Select(x2 => { x2.AssignmentSubSupplierId = null; x2.MainSupplierContactId = null; x2.MainSupplierId = null; x2.RecordStatus = "N"; return x2; }).ToList());

                    }
                }
            }

            if (mainSupplierDetailList?.Count > 0 && isSubSupplierPresent == true)
                assignmentSubSuppliers.AddRange(mainSupplierDetailList);

            return isSubSupplierPresent ? assignmentSubSuppliers : mainSupplierDetailList;
        }

        private Response DeleteAssignmentSubSupplier(List<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers, IList<DbModel.Assignment> dbAssignments, IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier, bool commitChange, List<DomainModel.AssignmentSubSupplier> assignmentSubSuppliersAdd)
        {
            Exception exception = null;
            try
            {
                if (assignmentSubSuppliers?.Count > 0)
                {
                    //IList<int> subSupplierIds = assignmentSubSuppliers?.Where(x => x.AssignmentSubSupplierId != null).Distinct().Select(x => (int)x.AssignmentSubSupplierId).ToList();
                    ////MS-TS Link - delete the MAin TS Techspecs when user change PO itself
                    //var allAssignmentSubSupplierslist = _assignmentSubSupplierRepository.FindBy(x => x.AssignmentId == assignmentSubSuppliers.Select(y => y.AssignmentId).FirstOrDefault()).ToList();
                    //if (allAssignmentSubSupplierslist.Count() == subSupplierIds.Count() + 1 && assignmentSubSuppliersAdd.Count > 0)
                    //{
                    //    int mainSupplierId = allAssignmentSubSupplierslist.Where(x => x.SupplierType == SupplierType.MainSupplier.FirstChar()).Select(x2 => x2.Id).FirstOrDefault();
                    //    if (mainSupplierId > 0)
                    //    {
                    //        subSupplierIds.Add(mainSupplierId);
                    //        dbAssignmentSubSupplier.Add(allAssignmentSubSupplierslist.Where(x => x.SupplierType == SupplierType.MainSupplier.FirstChar()).FirstOrDefault());
                    //        isPOchangedForThisAssignment = true;
                    //    }
                    //}
                    //MS-TS Link - delete the MAin TS Techspecs when user change PO itself
                    //var dbTsRecordsToDelete = _assignmentSubSupplerTsRepository.FindBy(x => subSupplierIds.Contains(x.AssignmentSubSupplierId)).ToList();
                    _assignmentSubSupplerTsRepository.AutoSave = false;
                    _assignmentSubSupplerTsRepository.Delete(dbAssignmentSubSupplier?.SelectMany(x => x.AssignmentSubSupplierTechnicalSpecialist)?.ToList());
                    _assignmentSubSupplierRepository.AutoSave = false;
                    _assignmentSubSupplierRepository.Delete(dbAssignmentSubSupplier);
                    if (commitChange)
                    {
                        _assignmentSubSupplerTsRepository.ForceSave();
                        _assignmentSubSupplierRepository.ForceSave();

                    }
                    // this.SaveMainSupplierContact(assignmentSubSuppliers, dbAssignments); -- no need to save in Assignment table - as per SMN 
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSuppliers);
            }
            finally
            {
                _assignmentSubSupplierRepository.AutoSave = true;
                // _assignmentSubSupplierRepository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response DeleteAssignmentSubSupplier(List<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers, DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange, bool IsSupplierPOChanged, bool IsMainSupplierSaved)
        {
            Exception exception = null;
            List<DomainModel.AssignmentSubSupplier> addAssignmentSubSuppliers = null;
            try
            {
                if (assignmentSubSuppliers?.Count > 0)
                {
                    var result = AssignMainAndSubSupplier(assignmentSubSuppliers, assignmentDatabaseCollection, ref addAssignmentSubSuppliers, assignmentSubSuppliers?.FirstOrDefault()?.SubSupplierId != null ? true : false, IsSupplierPOChanged, ref IsMainSupplierSaved);
                    IList<DbModel.AssignmentSubSupplier> dbAssignmentSubSupplier = assignmentDatabaseCollection.DBAssignmentSubSupplier?.Where(x => result
                                                                                                                .Any(x1 => x1.AssignmentSubSupplierId == x.Id &&
                                                                                                                     x1.RecordStatus.IsRecordStatusDeleted()))?.ToList();
                    _assignmentSubSupplerTsRepository.AutoSave = false;
                    _assignmentSubSupplerTsRepository.Delete(dbAssignmentSubSupplier?.SelectMany(x => x.AssignmentSubSupplierTechnicalSpecialist)?.ToList());
                    _assignmentSubSupplierRepository.AutoSave = false;
                    _assignmentSubSupplierRepository.Delete(dbAssignmentSubSupplier);
                    if (commitChange)
                    {
                        _assignmentSubSupplerTsRepository.ForceSave();
                        _assignmentSubSupplierRepository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSuppliers);
            }
            finally
            {
                _assignmentSubSupplierRepository.AutoSave = true;
                // _assignmentSubSupplierRepository.Dispose();
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response UpdateAssignmentSubSupplier(List<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ref List<DomainModel.AssignmentSubSupplier> filterAddSubSuppliers, bool commitChange, ref bool IsMainSupplierSaved, bool IsSupplierPOChanged = false)
        {
            Exception exception = null;
            try
            {
                if (assignmentSubSuppliers?.Count > 0)
                {
                    //var dbAssignmentTs = assignmentDatabaseCollection.DBAssignment.SelectMany(x => x.AssignmentTechnicalSpecialist).ToList();
                    var result = AssignMainAndSubSupplier(assignmentSubSuppliers, assignmentDatabaseCollection, ref filterAddSubSuppliers, assignmentSubSuppliers?.FirstOrDefault()?.SubSupplierId != null ? true : false, IsSupplierPOChanged, ref IsMainSupplierSaved);
                    var dbAssignmentTS = assignmentDatabaseCollection.DBAssignment?.SelectMany(x => x.AssignmentTechnicalSpecialist)?.ToList();
                    var assignmentSubSupplierToUpdate = assignmentDatabaseCollection.DBAssignmentSubSupplier?.ToList().Where(x => result.Select(x1 => x1.MainSupplierId).Contains(x.SupplierId))?.ToList();
                    assignmentSubSupplierToUpdate.ToList().ForEach(dbAssignmentSubSupplier =>
                    {
                        var assignmentSubSupplierToModify = result?.FirstOrDefault(x => x.AssignmentSubSupplierId == dbAssignmentSubSupplier.Id);
                        if (assignmentSubSupplierToModify != null)
                        {
                            dbAssignmentSubSupplier.AssignmentId = (int)assignmentSubSupplierToModify.AssignmentId;
                            dbAssignmentSubSupplier.SupplierId = (int)assignmentSubSupplierToModify.MainSupplierId;
                            dbAssignmentSubSupplier.SupplierType = assignmentSubSupplierToModify.SupplierType;
                            dbAssignmentSubSupplier.SupplierContactId = assignmentSubSupplierToModify.MainSupplierContactId;
                            dbAssignmentSubSupplier.IsFirstVisit = assignmentSubSupplierToModify.IsMainSupplierFirstVisit;
                            dbAssignmentSubSupplier.IsDeleted = IsSupplierPOChanged;
                            dbAssignmentSubSupplier.LastModification = DateTime.UtcNow;
                            dbAssignmentSubSupplier.UpdateCount = assignmentSubSupplierToModify.UpdateCount.CalculateUpdateCount();
                            dbAssignmentSubSupplier.ModifiedBy = assignmentSubSupplierToModify.ModifiedBy;
                            assignmentSubSupplierToModify?.AssignmentSubSupplierTS?.ForEach(x =>
                            {

                                ProcessSubSupplierTechnicalSpecialist(x,
                                                                         dbAssignmentSubSupplier.AssignmentSubSupplierTechnicalSpecialist.ToList(),
                                                                         dbAssignmentTS,
                                                                         IsSupplierPOChanged
                                                                         );

                            });
                        }
                    });

                    _assignmentSubSupplierRepository.AutoSave = false;
                    _assignmentSubSupplierRepository.Update(assignmentSubSupplierToUpdate);
                    if (commitChange)
                    {
                        _assignmentSubSupplierRepository.ForceSave();
                        _assignmentSubSupplerTsRepository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentSubSuppliers);
            }
            finally
            {
                _assignmentSubSupplierRepository.AutoSave = true;
                _assignmentSubSupplerTsRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private void ProcessSubSupplierTechnicalSpecialist(DomainModel.AssignmentSubSupplierTS assignmentSubSupplierTs, IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTs, List<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialists, bool IsSupplierPOChanged = false)
        {
            _assignmentSubSupplerTsRepository.AutoSave = false;
            if (assignmentSubSupplierTs.RecordStatus.IsRecordStatusNew())
            {
                if (dbAssignmentTechnicalSpecialists?.Count > 0)
                {
                    var dbNewTs = _mapper.Map<DbModel.AssignmentSubSupplierTechnicalSpecialist>(assignmentSubSupplierTs, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["AssignmentTsId"] = dbAssignmentTechnicalSpecialists;
                        opt.Items["IsDeleted"] = IsSupplierPOChanged;
                    });
                    var recordToDelete = dbAssignmentSubSupplierTs?.Where(x => x.TechnicalSpecialistId == dbNewTs?.TechnicalSpecialistId && x.IsDeleted == true)?.ToList();
                    if (recordToDelete != null && recordToDelete?.Count > 0)
                    {
                        _assignmentSubSupplerTsRepository.Delete(recordToDelete); //Changes for Live D743
                    }
                    if (dbNewTs != null)
                    {
                        _assignmentSubSupplerTsRepository.Add(dbNewTs);
                        _assignmentSubSupplerTsRepository.ForceSave();
                    }
                }
            }
            if (assignmentSubSupplierTs.RecordStatus.IsRecordStatusModified() || (assignmentSubSupplierTs.RecordStatus.IsRecordStatusDeleted() && IsSupplierPOChanged == true))
            {
                var recordToUpdate = dbAssignmentSubSupplierTs?.Where(x => x.Id == assignmentSubSupplierTs.AssignmentSubSupplierTSId)?.ToList();
                if (recordToUpdate != null)
                {
                    recordToUpdate.ForEach(ts =>
                    {
                        _mapper.Map(assignmentSubSupplierTs, ts, opt =>
                        {
                            opt.Items["isAssignId"] = true;
                            opt.Items["AssignmentTsId"] = dbAssignmentTechnicalSpecialists;
                            opt.Items["IsDeleted"] = IsSupplierPOChanged;
                        });
                        ts.LastModification = DateTime.UtcNow;
                        ts.UpdateCount = assignmentSubSupplierTs.UpdateCount.CalculateUpdateCount();
                        ts.ModifiedBy = assignmentSubSupplierTs.ModifiedBy;
                    });
                    _assignmentSubSupplerTsRepository.Update(recordToUpdate);
                }

                _assignmentSubSupplerTsRepository.ForceSave();
            }

            if (assignmentSubSupplierTs.RecordStatus.IsRecordStatusDeleted() && IsSupplierPOChanged == false)
            {
                var recordToDelete = dbAssignmentSubSupplierTs?.Where(x => x.Id == assignmentSubSupplierTs.AssignmentSubSupplierTSId)?.ToList();
                _assignmentSubSupplerTsRepository.Delete(recordToDelete);
                //if (recordToDelete != null)
                //{
                //    recordToDelete.ForEach(ts =>
                //    {
                //        ts.IsDeleted = true;
                //        ts.LastModification = DateTime.UtcNow;
                //        ts.UpdateCount = assignmentSubSupplierTs.UpdateCount.CalculateUpdateCount();
                //        ts.ModifiedBy = assignmentSubSupplierTs.ModifiedBy;
                //    });
                //    _assignmentSubSupplerTsRepository.Update(recordToDelete); //Changes for Live D743
                //}
                _assignmentSubSupplerTsRepository.ForceSave();
            }
        }

        private Response ProcessAssignmentReference(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ValidationType validationType)
        {
            Response response = null;
            try
            {
                var filterAddReference = assignmentReferenceTypes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var filterModifyReference = assignmentReferenceTypes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var filterDeleteReference = assignmentReferenceTypes?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                if (filterModifyReference?.Any() == true)
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) &&
                        assignmentDatabaseCollection.DBAssignmentReferenceTypes != null)
                        response = this.UpdateAssignmentReference(filterModifyReference,
                            ref assignmentDatabaseCollection, true);

                if (filterDeleteReference?.Any() == true)
                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) &&
                        assignmentDatabaseCollection.DBAssignmentReferenceTypes != null)
                    {
                        IList<DbModel.AssignmentReference> dbAssignmentReference = assignmentDatabaseCollection.DBAssignmentReferenceTypes
                                                                                                        .Where(x => filterDeleteReference
                                                                                                            .Any(x1 => x1.AssignmentReferenceTypeId == x.Id &&
                                                                                                                       x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this.DeleteAssignmentReference(filterDeleteReference, dbAssignmentReference, true);
                    }

                if (filterAddReference?.Any() == true)
                    if (ValidationType.Delete != validationType)
                        response = this.AddAssignmentReference(filterAddReference, ref assignmentDatabaseCollection, true);

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentReferenceTypes);
            }

            return response;
        }

        private Response AddAssignmentReference(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange = true)
        {
            Exception exception = null;
            IList<DbModel.Data> dbReferenceTypes = null;
            try
            {
                if (assignmentReferenceTypes?.Count > 0)
                {
                    _assignmentReferenceRepository.AutoSave = false;
                    dbReferenceTypes = assignmentDatabaseCollection.DBReferenceType;
                    assignmentDatabaseCollection.DBAssignmentReferenceTypes = _mapper.Map<IList<DbModel.AssignmentReference>>(assignmentReferenceTypes, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["ReferenceTypes"] = dbReferenceTypes;
                    });
                    _assignmentReferenceRepository.Add(assignmentDatabaseCollection.DBAssignmentReferenceTypes);
                    if (commitChange)
                        _assignmentReferenceRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentReferenceTypes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response UpdateAssignmentReference(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange = true)
        {
            Exception exception = null;
            try
            {
                if (assignmentReferenceTypes?.Count > 0)
                {
                    var dbReferenceTypes = assignmentDatabaseCollection.DBReferenceType;
                    assignmentDatabaseCollection.DBAssignmentReferenceTypes.ToList().ForEach(x =>
                    {
                        var assignmentReferenceToBeModify = assignmentReferenceTypes.FirstOrDefault(x1 => x1.AssignmentReferenceTypeId == x.Id);
                        if (assignmentReferenceToBeModify != null)
                        {
                            x.AssignmentReferenceTypeId = dbReferenceTypes.FirstOrDefault(x1 => x1.Name == assignmentReferenceToBeModify.ReferenceType).Id;
                            if (assignmentReferenceToBeModify.AssignmentId != null)
                                x.AssignmentId = (int)assignmentReferenceToBeModify.AssignmentId;
                            x.ReferenceValue = assignmentReferenceToBeModify.ReferenceValue;
                            x.LastModification = DateTime.UtcNow;
                            x.UpdateCount = assignmentReferenceToBeModify.UpdateCount.CalculateUpdateCount();
                            x.ModifiedBy = assignmentReferenceToBeModify.ModifiedBy;
                        }
                    });
                    _assignmentReferenceRepository.AutoSave = false;
                    _assignmentReferenceRepository.Update(assignmentDatabaseCollection.DBAssignmentReferenceTypes);
                    if (commitChange)
                        _assignmentReferenceRepository.ForceSave();

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentReferenceTypes);
            }
            finally
            {
                _assignmentReferenceRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response DeleteAssignmentReference(IList<DomainModel.AssignmentReferenceType> assignmentReferenceTypes, IList<DbModel.AssignmentReference> dbAssignmentReference, bool commitChange = true)
        {
            Exception exception = null;
            try
            {
                if (assignmentReferenceTypes?.Count > 0)
                {
                    _assignmentReferenceRepository.AutoSave = false;
                    _assignmentReferenceRepository.Delete(dbAssignmentReference);
                    if (commitChange)
                        _assignmentReferenceRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentReferenceTypes);
            }
            finally
            {
                _assignmentReferenceRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessAssignmentTaxonomy(IList<DomainModel.AssignmentTaxonomy> assignmentTaxonomies, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ValidationType validationType)
        {
            Response response = null;
            try
            {
                var filterAddTaxonomies = assignmentTaxonomies?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var filterModifyTaxonomies = assignmentTaxonomies?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var filterDeleteTaxonomies = assignmentTaxonomies?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();//MS-TS Link CR
                if (filterModifyTaxonomies?.Any() == true)
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) &&
                        assignmentDatabaseCollection.DBAssignmentTaxonomy != null)
                        response = this.UpdateAssignmentTaxonomy(filterModifyTaxonomies, ref assignmentDatabaseCollection, true);

                if (filterDeleteTaxonomies?.Any() == true)
                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) &&
                        assignmentDatabaseCollection.DBAssignmentTaxonomy != null)
                    {
                        IList<DbModel.AssignmentTaxonomy> dbAssignmentTaxonomy = assignmentDatabaseCollection.DBAssignmentTaxonomy.Where(x => filterDeleteTaxonomies
                                                                                                             .Any(x1 => x1.AssignmentTaxonomyId == x.Id &&
                                                                                                                        x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this.DeleteAssignmentTaxonomy(filterDeleteTaxonomies, dbAssignmentTaxonomy, true);
                    }

                if (filterAddTaxonomies?.Any() == true)
                    if (ValidationType.Delete != validationType)
                        response = this.AddAssignmentTaxonomy(filterAddTaxonomies, ref assignmentDatabaseCollection, true);

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTaxonomies);
            }

            return response;
        }

        private Response AddAssignmentTaxonomy(List<DomainModel.AssignmentTaxonomy> assignmentTaxonomies, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange)
        {
            Exception exception = null;
            try
            {
                IList<DbModel.TaxonomyService> dbTaxonomyServices = null;
                _assignmentTaxonomyRepository.AutoSave = false;
                dbTaxonomyServices = assignmentDatabaseCollection.DBService;
                assignmentDatabaseCollection.DBAssignmentTaxonomy = _mapper.Map<IList<DbModel.AssignmentTaxonomy>>(assignmentTaxonomies, opt =>
                {
                    opt.Items["isAssignId"] = false;
                    //opt.Items["TaxonomyService"] = dbTaxonomyServices;
                });
                _assignmentTaxonomyRepository.Add(assignmentDatabaseCollection.DBAssignmentTaxonomy);
                if (commitChange && assignmentTaxonomies.Count > 0)
                    _assignmentTaxonomyRepository.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTaxonomies);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response DeleteAssignmentTaxonomy(List<DomainModel.AssignmentTaxonomy> assignmentTaxonomies, IList<DbModel.AssignmentTaxonomy> dbAssignmentTaxonomy, bool commitChange)
        {
            Exception exception = null;
            try
            {
                var recordToDelete = dbAssignmentTaxonomy.Where(x => assignmentTaxonomies.Select(x1 => x1.AssignmentTaxonomyId).Contains(x.Id)).ToList();
                _assignmentTaxonomyRepository.AutoSave = false;
                _assignmentTaxonomyRepository.Delete(recordToDelete);

                if (commitChange)
                    _assignmentTaxonomyRepository.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTaxonomies);
            }
            finally
            {
                _assignmentTaxonomyRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response UpdateAssignmentTaxonomy(List<DomainModel.AssignmentTaxonomy> assignmentTaxonomies, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange)
        {
            Exception exception = null;
            try
            {
                IList<DbModel.AssignmentTaxonomy> recordToUpdate = new List<DbModel.AssignmentTaxonomy>();
                foreach (var record in assignmentTaxonomies)
                {
                    var dbrecord = assignmentDatabaseCollection.DBAssignmentTaxonomy.FirstOrDefault(x => x.Id == record.AssignmentTaxonomyId);
                    if (dbrecord != null)
                    {
                        dbrecord.TaxonomyServiceId = record.TaxonomyServiceId;//(int)assignmentDatabaseCollection.DBService?.FirstOrDefault(x => x.TaxonomyServiceName == record.TaxonomyService)?.Id;
                        dbrecord.LastModification = DateTime.UtcNow;
                        dbrecord.UpdateCount = record.UpdateCount.CalculateUpdateCount();
                        dbrecord.ModifiedBy = record.ModifiedBy;
                        recordToUpdate.Add(dbrecord);
                    }
                }
                _assignmentTaxonomyRepository.AutoSave = false;
                _assignmentTaxonomyRepository.Update(recordToUpdate);

                if (commitChange)
                    _assignmentTaxonomyRepository.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTaxonomies);
            }
            finally
            {
                _assignmentTaxonomyRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessAssignmentContractRateSchedule(IList<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ValidationType validationType)
        {
            Response response = null;
            try
            {
                var filterAddSchedules = assignmentContractRateSchedules?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var filterModifySchedules = assignmentContractRateSchedules?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var filterDeleteSchedules = assignmentContractRateSchedules?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                if (filterModifySchedules?.Any() == true)
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && assignmentDatabaseCollection.DBAssignmentContractSchedules != null)
                        response = this.UpdateAssignmentContractSchedule(filterModifySchedules, ref assignmentDatabaseCollection, true);

                if (filterDeleteSchedules?.Any() == true)
                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && assignmentDatabaseCollection.DBAssignmentContractSchedules != null)
                    {
                        IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedule = assignmentDatabaseCollection.DBAssignmentContractSchedules.Where(x => filterDeleteSchedules
                                                                                                             .Any(x1 => x1.AssignmentContractRateScheduleId == x.Id &&
                                                                                                                        x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this.DeleteAssignmentContractSchedule(filterDeleteSchedules, dbAssignmentContractSchedule, true);
                    }

                if (filterAddSchedules?.Any() == true)
                    if (ValidationType.Delete != validationType)
                        response = this.AddAssignmentContractSchedule(filterAddSchedules, ref assignmentDatabaseCollection, true);

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContractRateSchedules);
            }

            return response;
        }

        private Response AddAssignmentContractSchedule(List<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange)
        {
            Exception exception = null;
            try
            {
                if (assignmentContractRateSchedules?.Count > 0)
                {
                    _assignmentContractRateScheduleRepository.AutoSave = false;
                    assignmentDatabaseCollection.DBAssignmentContractSchedules = _mapper.Map<IList<DbModel.AssignmentContractSchedule>>(assignmentContractRateSchedules, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                    });
                    _assignmentContractRateScheduleRepository.Add(assignmentDatabaseCollection.DBAssignmentContractSchedules);
                    if (commitChange)
                        _assignmentContractRateScheduleRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContractRateSchedules);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response DeleteAssignmentContractSchedule(List<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules, IList<DbModel.AssignmentContractSchedule> dbAssignmentContractSchedule, bool commitChange)
        {
            Exception exception = null;
            try
            {
                if (assignmentContractRateSchedules?.Count > 0)
                {
                    _assignmentContractRateScheduleRepository.AutoSave = false;
                    _assignmentContractRateScheduleRepository.Delete(dbAssignmentContractSchedule);
                    if (commitChange)
                        _assignmentContractRateScheduleRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContractRateSchedules);
            }
            finally
            {
                _assignmentContractRateScheduleRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response UpdateAssignmentContractSchedule(List<DomainModel.AssignmentContractRateSchedule> assignmentContractRateSchedules, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange)
        {
            Exception exception = null;
            try
            {

                if (assignmentContractRateSchedules?.Count > 0)
                {
                    assignmentDatabaseCollection.DBAssignmentContractSchedules.ToList().ForEach(dbAssignmentContractSchedule =>
                    {
                        var asssignContractToBeModify = assignmentContractRateSchedules.FirstOrDefault(x => x.AssignmentContractRateScheduleId == dbAssignmentContractSchedule.Id);
                        if (asssignContractToBeModify != null)
                        {
                            dbAssignmentContractSchedule.AssignmentId = asssignContractToBeModify.AssignmentId.Value;
                            dbAssignmentContractSchedule.ContractScheduleId = asssignContractToBeModify.ContractScheduleId.Value;
                            dbAssignmentContractSchedule.LastModification = DateTime.UtcNow;
                            dbAssignmentContractSchedule.UpdateCount = asssignContractToBeModify.UpdateCount.CalculateUpdateCount();
                            dbAssignmentContractSchedule.ModifiedBy = asssignContractToBeModify.ModifiedBy;
                        }
                    });
                    _assignmentContractRateScheduleRepository.AutoSave = false;
                    _assignmentContractRateScheduleRepository.Update(assignmentDatabaseCollection.DBAssignmentContractSchedules);
                    if (commitChange)
                        _assignmentContractRateScheduleRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContractRateSchedules);
            }
            finally
            {
                _assignmentContractRateScheduleRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessAssignmentTechnicalSpecialist(IList<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists,
            IList<DomainModel.AssignmentSubSupplier> assignmentSubSuppliers,
            ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection,
            ValidationType validationType, DomainModel.AssignmentDetail assignmentDetail)
        {
            Response response = null;
            try
            {
                var filterAddTechnicalSpecialists = assignmentTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var filterModifyTechnicalSpecialists = assignmentTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var filterDeleteTechnicalSpecialists = assignmentTechnicalSpecialists?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                var filterDeleteSubSuppliers = assignmentSubSuppliers?.SelectMany(x => x.AssignmentSubSupplierTS).Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (filterModifyTechnicalSpecialists?.Any() == true)
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && assignmentDatabaseCollection.DBAssignmentTechnicalSpecialists != null)
                        response = this.UpdateAssignmentTechSpec(filterModifyTechnicalSpecialists, ref assignmentDatabaseCollection, true);

                if (filterDeleteTechnicalSpecialists?.Any() == true)
                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && assignmentDatabaseCollection.DBAssignmentTechnicalSpecialists != null)
                    {
                        if (filterDeleteSubSuppliers?.Any() == true)
                            if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && assignmentDatabaseCollection.DBAssignmentSubSupplier != null)//MS-TS Link
                            {
                                IList<DbModel.AssignmentSubSupplierTechnicalSpecialist> dbAssignmentSubSupplierTs = assignmentDatabaseCollection.DBAssignmentSubSupplier
                                                                                                                    .SelectMany(x => x.AssignmentSubSupplierTechnicalSpecialist)
                                                                                                                    .Where(x => filterDeleteSubSuppliers
                                                                                                                    .Any(x1 => x1.AssignmentSubSupplierTSId == x.Id &&
                                                                                                                                x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                                _assignmentSubSupplerTsRepository.Delete(dbAssignmentSubSupplierTs);
                                _assignmentSubSupplierRepository.ForceSave();
                            }

                        IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechSpec = assignmentDatabaseCollection.DBAssignmentTechnicalSpecialists.Where(x => filterDeleteTechnicalSpecialists
                                                                                                              .Any(x1 => x1.AssignmentTechnicalSplId == x.Id &&
                                                                                                                         x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this.DeleteAssignmentTechSpec(filterDeleteTechnicalSpecialists, dbAssignmentTechSpec, true);
                    }

                if (filterAddTechnicalSpecialists?.Any() == true)
                    if (ValidationType.Delete != validationType)
                        response = this.AddAssignmentTechSpec(filterAddTechnicalSpecialists, ref assignmentDatabaseCollection, true, assignmentDetail);

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialists);
            }

            return response;
        }

        private Response AddAssignmentTechSpec(List<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists,
            ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange,
            DomainModel.AssignmentDetail assignmentDetail)
        {
            Exception exception = null;
            try
            {
                if (assignmentTechnicalSpecialists?.Count > 0)
                {
                    _assignmentTechnicalSpecialistRepository.AutoSave = false;
                    assignmentTechnicalSpecialists?.ToList().ForEach(x =>
                    {
                        x.ModifiedBy = null;
                        x.AssignmentTechnicalSpecialistSchedules = x.AssignmentTechnicalSpecialistSchedules?.Where(x1 => x1.RecordStatus.IsRecordStatusNew()).ToList();
                    });

                    assignmentTechnicalSpecialists = assignmentTechnicalSpecialists?.Select(x => { x.AssignmentTechnicalSplId = null; return x; }).ToList();
                    var dbRecordToAdd = _mapper.Map<IList<DbModel.AssignmentTechnicalSpecialist>>(assignmentTechnicalSpecialists);
                    _assignmentTechnicalSpecialistRepository.Add(dbRecordToAdd);
                    // ResourceAssignmentEmailNotification(dbRecordToAdd, assignmentDatabaseCollection.DBAssignment);
                    if (commitChange)
                        _assignmentTechnicalSpecialistRepository.ForceSave();
                    int? assignmentID = assignmentDatabaseCollection?.DBAssignment?.FirstOrDefault()?.Id;
                    string modifiedBy = assignmentDetail.AssignmentInfo.ActionByUser;
                    this.AddAssignmentHistory(assignmentID,
                        assignmentDatabaseCollection?.Assignment?.DBMasterData?.Where(x => x.MasterDataTypeId == (int)MasterType.HistoryTable && x.Code == AssignmentConstants.ASSIGNMENT_SPECIALIST_ADDED)?.ToList()
                        , modifiedBy);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialists);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response DeleteAssignmentTechSpec(List<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists, IList<DbModel.AssignmentTechnicalSpecialist> dbAssignmentTechnicalSpecialist, bool commitChange)
        {
            Exception exception = null;
            try
            {
                if (assignmentTechnicalSpecialists?.Count > 0)
                {
                    _assignmentTechnicalSpecialistRepository.AutoSave = false;
                    _assignmentTechnicalSpecialistScheduleRepository.AutoSave = false;
                    var dbAssignmentTechSpecSchedule = dbAssignmentTechnicalSpecialist?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedule).ToList();
                    var techSpecSchedule = assignmentTechnicalSpecialists?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules.Where(x1 => x1.RecordStatus.IsRecordStatusDeleted())).ToList();

                    var tsScheduleToDelete = dbAssignmentTechSpecSchedule?.Where(x => techSpecSchedule.Any(x1 => x1.AssignmentTechnicalSpecilaistId == x.AssignmentTechnicalSpecialistId)).ToList();
                    _assignmentTechnicalSpecialistScheduleRepository.Delete(tsScheduleToDelete);

                    if (commitChange)
                    {
                        _assignmentTechnicalSpecialistScheduleRepository.ForceSave();
                        _assignmentTechnicalSpecialistRepository.Delete(dbAssignmentTechnicalSpecialist);
                        _assignmentTechnicalSpecialistRepository.ForceSave();

                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialists);
            }
            finally
            {
                _assignmentContractRateScheduleRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response UpdateAssignmentTechSpec(List<DomainModel.AssignmentTechnicalSpecialist> assignmentTechnicalSpecialists, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange)
        {
            Exception exception = null;
            try
            {

                if (assignmentTechnicalSpecialists?.Count > 0)
                {
                    var dbTechSpec = assignmentDatabaseCollection.DBTechnicalSpecialists;
                    var dbSpecialistSchedule = assignmentDatabaseCollection.DBAssignmentTechnicalSpecialists?.ToList().SelectMany(x => x.AssignmentTechnicalSpecialistSchedule).ToList();
                    assignmentDatabaseCollection.DBAssignmentTechnicalSpecialists?.ToList().ForEach(dbAssignTech =>
                    {
                        var assignmentTechnicalSpecialistToBeModify = assignmentTechnicalSpecialists?.FirstOrDefault(x => x.AssignmentTechnicalSplId == dbAssignTech.Id);
                        if (assignmentTechnicalSpecialistToBeModify != null)
                        {
                            dbAssignTech.TechnicalSpecialistId = (int)dbTechSpec?.ToList().FirstOrDefault(x1 => x1.Pin == assignmentTechnicalSpecialistToBeModify.Epin).Id;
                            dbAssignTech.AssignmentId = (int)assignmentTechnicalSpecialistToBeModify.AssignmentId;
                            dbAssignTech.IsSupervisor = assignmentTechnicalSpecialistToBeModify.IsSupervisor;
                            dbAssignTech.IsActive = assignmentTechnicalSpecialistToBeModify.IsActive;
                            dbAssignTech.LastModification = DateTime.UtcNow;
                            dbAssignTech.UpdateCount = assignmentTechnicalSpecialistToBeModify.UpdateCount.CalculateUpdateCount();
                            dbAssignTech.ModifiedBy = assignmentTechnicalSpecialistToBeModify.ModifiedBy;

                            assignmentTechnicalSpecialistToBeModify?.AssignmentTechnicalSpecialistSchedules?.ForEach(x =>
                            {
                                ProcessTechSpecSchedule((int)assignmentTechnicalSpecialistToBeModify.AssignmentTechnicalSplId,
                                                        x,
                                                        dbSpecialistSchedule);
                            });
                        }

                    });
                    _assignmentTechnicalSpecialistRepository.AutoSave = false;
                    _assignmentTechnicalSpecialistRepository.Update(assignmentDatabaseCollection.DBAssignmentTechnicalSpecialists);
                    if (commitChange)
                    {
                        _assignmentTechnicalSpecialistRepository.ForceSave();
                        _assignmentTechnicalSpecialistScheduleRepository.ForceSave();
                    }

                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentTechnicalSpecialists);
            }
            finally
            {
                _assignmentContractRateScheduleRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private void ProcessTechSpecSchedule(int techSpecId, DomainModel.AssignmentTechnicalSpecialistSchedule technicalSpecialistSchedule, IList<DbModel.AssignmentTechnicalSpecialistSchedule> dbAssignmentTechSpecSchedule)
        {
            _assignmentTechnicalSpecialistScheduleRepository.AutoSave = false;
            if (technicalSpecialistSchedule.RecordStatus.IsRecordStatusNew())
            {
                var recordToAdd = _mapper.Map<DbModel.AssignmentTechnicalSpecialistSchedule>(technicalSpecialistSchedule);
                recordToAdd.AssignmentTechnicalSpecialistId = techSpecId;
                _assignmentTechnicalSpecialistScheduleRepository.Add(recordToAdd);
            }
            if (technicalSpecialistSchedule.RecordStatus.IsRecordStatusModified())
            {
                var recordToUpdate = dbAssignmentTechSpecSchedule?.Where(x => x.Id == technicalSpecialistSchedule.AssignmentTechnicalSpecialistScheduleId)?.ToList();
                if (recordToUpdate != null)
                {
                    recordToUpdate.ForEach(ts =>
                    {
                        _mapper.Map(technicalSpecialistSchedule, ts);
                        ts.LastModification = DateTime.UtcNow;
                        ts.UpdateCount = technicalSpecialistSchedule.UpdateCount.CalculateUpdateCount();
                        ts.ModifiedBy = technicalSpecialistSchedule.ModifiedBy;
                    });
                    _assignmentTechnicalSpecialistScheduleRepository.Update(recordToUpdate);
                }

                _assignmentTechnicalSpecialistScheduleRepository.ForceSave();
            }

            if (!technicalSpecialistSchedule.RecordStatus.IsRecordStatusDeleted()) return;
            {
                var recordToDelete = dbAssignmentTechSpecSchedule.Where(x => x.Id == technicalSpecialistSchedule.AssignmentTechnicalSpecialistScheduleId)?.FirstOrDefault();
                _assignmentTechnicalSpecialistScheduleRepository.Delete(recordToDelete);
            }
        }

        private Response ProcessAssignmentNote(IList<DomainModel.AssignmentNote> assignmentNotes, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection)
        {
            Response response = null;
            try
            {
                var filterAddAssignmentNotes = assignmentNotes?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var filterUpdateAssignmentNotes = assignmentNotes?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                if (filterAddAssignmentNotes?.Any() == true)
                    response = this.AddAssignmentNote(filterAddAssignmentNotes, ref assignmentDatabaseCollection.DBAssignmentNotes, ref assignmentDatabaseCollection.DBAssignment, true, false);
                if (filterUpdateAssignmentNotes?.Any() == true)
                    response = this.UpdateAssignmentNote(filterUpdateAssignmentNotes, ref assignmentDatabaseCollection.DBAssignmentNotes, ref assignmentDatabaseCollection.DBAssignment, true, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentNotes);
            }

            return response;
        }

        private Response AddAssignmentNote(IList<DomainModel.AssignmentNote> assignmentNotes, ref IList<DbModel.AssignmentNote> dbAssignmentNotes, ref IList<DbModel.Assignment> dbAssignment, bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            try
            {
                _assignmentNoteRepository.AutoSave = false;
                _assignmentNoteRepository.Add(_mapper.Map<IList<DbModel.AssignmentNote>>(assignmentNotes));

                if (commitChange)
                    _assignmentNoteRepository.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentNotes);
            }
            finally
            {
                _assignmentNoteRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }
        //D661 issue 8 Start
        private Response UpdateAssignmentNote(IList<DomainModel.AssignmentNote> assignmentNotes, ref IList<DbModel.AssignmentNote> dbAssignmentNotes, ref IList<DbModel.Assignment> dbAssignment, bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            try
            {
                if (assignmentNotes?.Count > 0)
                {
                    dbAssignmentNotes.ToList().ForEach(dbAssignNote =>
                    {
                        var assignmentNotesToModify = assignmentNotes?.FirstOrDefault(x => x.AssignmnetNoteId == dbAssignNote.Id);
                        if (assignmentNotesToModify != null)
                        {
                            dbAssignNote.AssignmentId = (int)assignmentNotesToModify.AssignmentId;
                            dbAssignNote.Id = (int)assignmentNotesToModify.AssignmnetNoteId;
                            dbAssignNote.Note = assignmentNotesToModify.Note;
                            dbAssignNote.CreatedDate = (DateTime)assignmentNotesToModify.CreatedOn;
                            dbAssignNote.LastModification = DateTime.UtcNow;
                            dbAssignNote.UpdateCount = assignmentNotesToModify.UpdateCount.CalculateUpdateCount();
                            dbAssignNote.ModifiedBy = assignmentNotesToModify.ModifiedBy;
                        }
                    });
                    _assignmentNoteRepository.AutoSave = false;
                    _assignmentNoteRepository.Update(dbAssignmentNotes);
                    if (commitChange)
                    {
                        _assignmentNoteRepository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentNotes);
            }
            finally
            {
                _assignmentNoteRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);

        }
        //D661 issue 8 End
        private Response ProcessAssignmentInstruction(DomainModel.AssignmentInstructions assignmentInstructions, DbModel.Assignment dbAssignment, bool commitChange = true)
        {
            Exception exception = null;
            try
            {
                this._assignmentMessageRepository.AutoSave = false;
                var msgToBeInsert = this.ProcessNewAssignmentInstructions(assignmentInstructions, dbAssignment).ToList();
                var msgToBeUpdate = this.ProcessExistingAssignmentInstructions(assignmentInstructions, dbAssignment).ToList();
                if (msgToBeInsert?.Any() == true)
                    _assignmentMessageRepository.Add(msgToBeInsert);

                if (msgToBeUpdate?.Any() == true)
                    _assignmentMessageRepository.Update(msgToBeUpdate);

                if (commitChange && !_assignmentMessageRepository.AutoSave)
                    _assignmentMessageRepository.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentInstructions);
            }
            finally
            {
                _assignmentMessageRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private IList<DbModel.AssignmentMessage> ProcessNewAssignmentInstructions(DomainModel.AssignmentInstructions assignmentInstructions, DbModel.Assignment dbAssignment)
        {
            List<DbModel.AssignmentMessage> dbAssignmentMessage = new List<DbModel.AssignmentMessage>();
            DbModel.AssignmentMessage assignmentMessage = null;

            assignmentMessage = dbAssignment?.AssignmentMessage?.FirstOrDefault(x => x.MessageTypeId == (int)AssignmentMessageType.InterCompanyInstructions);
            if (assignmentMessage == null && !string.IsNullOrEmpty(assignmentInstructions.InterCompanyInstructions))
                dbAssignmentMessage.Add(ConvertToDbAssignmentInstructions(dbAssignment.Id, AssignmentMessageType.InterCompanyInstructions, assignmentInstructions.InterCompanyInstructions));

            assignmentMessage = dbAssignment?.AssignmentMessage?.FirstOrDefault(x => x.MessageTypeId == (int)AssignmentMessageType.OperationalNotes);
            if (assignmentMessage == null && !string.IsNullOrEmpty(assignmentInstructions.TechnicalSpecialistInstructions))
                dbAssignmentMessage.Add(ConvertToDbAssignmentInstructions(dbAssignment.Id, AssignmentMessageType.OperationalNotes, assignmentInstructions.TechnicalSpecialistInstructions));

            return dbAssignmentMessage;

        }

        private IList<DbModel.AssignmentMessage> ProcessExistingAssignmentInstructions(DomainModel.AssignmentInstructions assignmentInstructions, DbModel.Assignment dbAssignment)
        {
            List<DbModel.AssignmentMessage> recordToUpdate = new List<DbModel.AssignmentMessage>();
            DbModel.AssignmentMessage assignmentMessage = null;

            assignmentMessage = dbAssignment?.AssignmentMessage?.FirstOrDefault(x => x.MessageTypeId == (int)AssignmentMessageType.InterCompanyInstructions);
            if (assignmentMessage != null && !string.IsNullOrEmpty(assignmentMessage.Message))
            {
                if (assignmentMessage != null && !assignmentMessage.Message.Equals(assignmentInstructions.InterCompanyInstructions))
                    recordToUpdate.Add(ConvertToDbAssignmentInstructions(assignmentMessage,
                                                                         AssignmentMessageType.InterCompanyInstructions,
                                                                         assignmentInstructions.InterCompanyInstructions.IsEmptyReturnNull(),
                                                                         true,//!string.IsNullOrEmpty(assignmentInstructions.InterCompanyInstructions) ? true : false,
                                                                         assignmentInstructions.ModifiedBy,
                                                                         assignmentInstructions.UpdateCount));
            }
            else
            {
                if (assignmentMessage != null)
                {
                    recordToUpdate.Add(ConvertToDbAssignmentInstructions(assignmentMessage,
                                                                         AssignmentMessageType.InterCompanyInstructions,
                                                                         assignmentInstructions.InterCompanyInstructions.IsEmptyReturnNull(),
                                                                         true,//!string.IsNullOrEmpty(assignmentInstructions.InterCompanyInstructions) ? true : false,
                                                                         assignmentInstructions.ModifiedBy,
                                                                         assignmentInstructions.UpdateCount));
                }
            }
            assignmentMessage = dbAssignment?.AssignmentMessage?.FirstOrDefault(x => x.MessageTypeId == (int)AssignmentMessageType.OperationalNotes);
            if (assignmentMessage != null && !string.IsNullOrEmpty(assignmentMessage.Message))
            {

                if (assignmentMessage != null && !assignmentMessage.Message.Equals(assignmentInstructions.TechnicalSpecialistInstructions))
                    recordToUpdate.Add(ConvertToDbAssignmentInstructions(assignmentMessage,
                                                                        AssignmentMessageType.OperationalNotes,
                                                                        assignmentInstructions.TechnicalSpecialistInstructions.IsEmptyReturnNull(),
                                                                        true,//!string.IsNullOrEmpty(assignmentInstructions.TechnicalSpecialistInstructions) ? true : false,
                                                                        assignmentInstructions.ModifiedBy,
                                                                        assignmentInstructions.UpdateCount));
            }
            else
            {
                if (assignmentMessage != null)
                {
                    recordToUpdate.Add(ConvertToDbAssignmentInstructions(assignmentMessage,
                                                                         AssignmentMessageType.InterCompanyInstructions,
                                                                         assignmentInstructions.InterCompanyInstructions.IsEmptyReturnNull(),
                                                                         true,//!string.IsNullOrEmpty(assignmentInstructions.InterCompanyInstructions) ? true : false,
                                                                         assignmentInstructions.ModifiedBy,
                                                                         assignmentInstructions.UpdateCount));
                }
            }
            return recordToUpdate;
        }

        private DbModel.AssignmentMessage ConvertToDbAssignmentInstructions(int assignmentId, AssignmentMessageType type, string messageText, bool isActive = true)
        {
            return new DbModel.AssignmentMessage()
            {
                Id = 0,
                AssignmentId = assignmentId,
                Identifier = null,
                Message = messageText,
                IsActive = isActive,
                MessageTypeId = (int)type,
            };
        }

        private DbModel.AssignmentMessage ConvertToDbAssignmentInstructions(DbModel.AssignmentMessage dbAssignmentMessage, AssignmentMessageType type, string messageText, bool? isActive = true, string modifiedBy = null, int? updateCount = null)
        {
            dbAssignmentMessage.Message = messageText; //isActive == true ? messageText : dbAssignmentMessage.Message;
            dbAssignmentMessage.IsActive = isActive;
            dbAssignmentMessage.LastModification = DateTime.UtcNow;
            dbAssignmentMessage.ModifiedBy = modifiedBy;
            //dbAssignmentMessage.UpdateCount = Convert.ToByte(updateCount).CalculateUpdateCount(); 
            return dbAssignmentMessage;
        }

        private Response ProcessAssignmentContributionCalculator(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculations, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ValidationType validationType)
        {
            Response response = null;
            try
            {
                var filterAddContributionCalculations = assignmentContributionCalculations?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var filterModifyContributionCalculations = assignmentContributionCalculations?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var filterDeleteContributionCalculations = assignmentContributionCalculations?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                if (filterModifyContributionCalculations?.Any() == true)
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && assignmentDatabaseCollection.DBAssignmentTechnicalSpecialists != null)
                        response = this.UpdateAssignmentContributionCalculation(filterModifyContributionCalculations, ref assignmentDatabaseCollection, true);

                if (filterDeleteContributionCalculations?.Any() == true)
                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete) && assignmentDatabaseCollection.DBAssignmentTechnicalSpecialists != null)
                    {
                        IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculations = assignmentDatabaseCollection.DBAssignmentContributionCalculations.Where(x => filterDeleteContributionCalculations
                                                                                                              .Any(x1 => x1.AssignmentContCalculationId == x.Id &&
                                                                                                                         x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this.DeleteAssignmentContributionCalculation(filterDeleteContributionCalculations, dbAssignmentContributionCalculations, true);
                    }

                if (filterAddContributionCalculations?.Any() == true)
                    if (ValidationType.Delete != validationType)
                        response = this.AddAssignmentContributionCalculation(filterAddContributionCalculations, ref assignmentDatabaseCollection, true);

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionCalculations);
            }

            return response;
        }

        private Response AddAssignmentContributionCalculation(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange)
        {
            Exception exception = null;
            try
            {
                if (assignmentContributionCalculation?.Count > 0)
                {
                    _assignmentContributionCalculationRepository.AutoSave = false;
                    var dbRecordToAdd = _mapper.Map<IList<DbModel.AssignmentContributionCalculation>>(assignmentContributionCalculation, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["isContributionRevenueCostId"] = false;
                        opt.Items["isContributionCalculationId"] = false;
                    });

                    _assignmentContributionCalculationRepository.Add(dbRecordToAdd);
                    if (commitChange)
                        _assignmentContributionCalculationRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionCalculation);
            }
            finally { _assignmentContributionCalculationRepository.AutoSave = true; }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response UpdateAssignmentContributionCalculation(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange)
        {
            Exception exception = null;
            try
            {
                if (assignmentContributionCalculation?.Count > 0)
                {
                    assignmentDatabaseCollection.DBAssignmentContributionCalculations.ToList().ForEach(dbAssignmentContribution =>
                    {
                        var assignmentContributionCalculationToBeModify = assignmentContributionCalculation.FirstOrDefault(x => x.AssignmentContCalculationId == dbAssignmentContribution.Id);
                        if (assignmentContributionCalculationToBeModify != null)
                        {
                            dbAssignmentContribution.TotalContributionValue = (int)assignmentContributionCalculationToBeModify.TotalContributionValue;
                            dbAssignmentContribution.ContractHolderPercentage = (int)assignmentContributionCalculationToBeModify.ContractHolderPercentage;
                            dbAssignmentContribution.OperatingCompanyPercentage = assignmentContributionCalculationToBeModify.OperatingCompanyPercentage;
                            dbAssignmentContribution.CountryCompanyPercentage = assignmentContributionCalculationToBeModify.CountryCompanyPercentage;
                            dbAssignmentContribution.ContractHolderValue = (int)assignmentContributionCalculationToBeModify.ContractHolderValue;
                            dbAssignmentContribution.OperatingCompanyValue = assignmentContributionCalculationToBeModify.OperatingCompanyValue;
                            dbAssignmentContribution.CountryCompanyValue = assignmentContributionCalculationToBeModify.CountryCompanyValue;
                            dbAssignmentContribution.MarkupPercentage = assignmentContributionCalculationToBeModify.MarkupPercentage;
                            dbAssignmentContribution.LastModification = DateTime.UtcNow;
                            dbAssignmentContribution.UpdateCount = assignmentContributionCalculationToBeModify.UpdateCount.CalculateUpdateCount();
                            dbAssignmentContribution.ModifiedBy = assignmentContributionCalculationToBeModify.ModifiedBy;
                            assignmentContributionCalculationToBeModify.AssignmentContributionRevenueCosts?.ToList().ForEach(x =>
                            {
                                if (assignmentContributionCalculationToBeModify.AssignmentContCalculationId != null)
                                    ProcessContributionRevenueCost((int)assignmentContributionCalculationToBeModify.AssignmentContCalculationId, x, dbAssignmentContribution.AssignmentContributionRevenueCost?.ToList());
                            });
                        }
                    });
                    _assignmentContributionCalculationRepository.AutoSave = false;
                    _assignmentContributionCalculationRepository.Update(assignmentDatabaseCollection.DBAssignmentContributionCalculations);
                    if (commitChange)
                    {
                        _revenueCostRepository.ForceSave();
                        _assignmentContributionCalculationRepository.ForceSave();

                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionCalculation);
            }
            finally
            {
                _assignmentContributionCalculationRepository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response DeleteAssignmentContributionCalculation(IList<DomainModel.AssignmentContributionCalculation> assignmentContributionCalculation, IList<DbModel.AssignmentContributionCalculation> dbAssignmentContributionCalculation, bool commitChange)
        {
            Exception exception = null;
            try
            {
                if (assignmentContributionCalculation?.Count > 0)
                {
                    var dbRevenueCostToDelete = dbAssignmentContributionCalculation.SelectMany(x => x.AssignmentContributionRevenueCost).ToList();
                    _assignmentContributionCalculationRepository.AutoSave = false;
                    _revenueCostRepository.Delete(dbRevenueCostToDelete);

                    if (commitChange)
                    {
                        _revenueCostRepository.ForceSave();
                        _assignmentContributionCalculationRepository.Delete(dbAssignmentContributionCalculation);
                        _assignmentContributionCalculationRepository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentContributionCalculation);
            }
            finally
            {
                _assignmentContributionCalculationRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private void ProcessContributionRevenueCost(int contributionCalculationId, DomainModel.AssignmentContributionRevenueCost assignmentContributionRevenueCost, IList<DbModel.AssignmentContributionRevenueCost> dbAssignmentContributionRevenueCosts)
        {
            _revenueCostRepository.AutoSave = false;
            if (assignmentContributionRevenueCost.RecordStatus.IsRecordStatusNew())
            {
                var recordToAdd = _mapper.Map<DbModel.AssignmentContributionRevenueCost>(assignmentContributionRevenueCost);
                recordToAdd.Id = 0;
                recordToAdd.AssignmentContributionCalculationId = contributionCalculationId;
                _revenueCostRepository.Add(recordToAdd);
            }
            if (assignmentContributionRevenueCost.RecordStatus.IsRecordStatusModified())
            {
                var recordToUpdate = dbAssignmentContributionRevenueCosts.Where(x => x.Id == assignmentContributionRevenueCost.AssignmentContributionRevenueCostId)?.ToList();
                recordToUpdate.ForEach(cost =>
                {
                    _mapper.Map(assignmentContributionRevenueCost, cost);
                    cost.LastModification = DateTime.UtcNow;
                    cost.UpdateCount = assignmentContributionRevenueCost.UpdateCount.CalculateUpdateCount();
                    cost.ModifiedBy = assignmentContributionRevenueCost.ModifiedBy;
                });
                _revenueCostRepository.Update(recordToUpdate);
                _revenueCostRepository.ForceSave();
            }
            if (assignmentContributionRevenueCost.RecordStatus.IsRecordStatusDeleted())
            {
                var recordToDelete = dbAssignmentContributionRevenueCosts.Where(x => x.Id == assignmentContributionRevenueCost.AssignmentContributionRevenueCostId)?.FirstOrDefault();
                _revenueCostRepository.Delete(recordToDelete);
            }

        }

        private Response ProcessAssignmentAdditionalExpense(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpenses, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ValidationType validationType)
        {
            Response response = null;
            try
            {
                var filterAddExpenses = assignmentAdditionalExpenses?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var filterModifyExpenses = assignmentAdditionalExpenses?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var filterDeleteExpenses = assignmentAdditionalExpenses?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                if (filterModifyExpenses?.Any() == true)
                    if ((ValidationType.Delete != validationType || ValidationType.Add != validationType))
                        response = this.UpdateAssignmentAdditionalExpenses(filterModifyExpenses, ref assignmentDatabaseCollection, true);

                if (filterDeleteExpenses?.Any() == true)
                    if ((validationType == ValidationType.Update || validationType == ValidationType.Delete))
                    {
                        IList<DbModel.AssignmentAdditionalExpense> dbAssignmentContributionCalculations = assignmentDatabaseCollection.DBAssignmentAdditionalExpenses.Where(x => filterDeleteExpenses
                                                                                                              .Any(x1 => x1.AssignmentAdditionalExpenseId == x.Id &&
                                                                                                                         x1.RecordStatus.IsRecordStatusDeleted())).ToList();
                        response = this.DeleteAssignmentAdditionalExpense(filterDeleteExpenses, dbAssignmentContributionCalculations, true);
                    }

                if (filterAddExpenses?.Any() == true)
                    if (ValidationType.Delete != validationType)
                        response = this.AddAssignmentAdditionalExpense(filterAddExpenses, ref assignmentDatabaseCollection, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentAdditionalExpenses);
            }

            return response;
        }

        private Response AddAssignmentAdditionalExpense(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpenses, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange)
        {
            Exception exception = null;
            try
            {
                if (assignmentAdditionalExpenses?.Count > 0)
                {
                    _assignmentAdditionalExpenseRepository.AutoSave = false;
                    var dbExpenseType = assignmentDatabaseCollection.DBExpenseType;
                    var dbCompany = assignmentDatabaseCollection.Assignment.DBCompanies;
                    assignmentDatabaseCollection.DBAssignmentAdditionalExpenses = _mapper.Map<IList<DbModel.AssignmentAdditionalExpense>>(assignmentAdditionalExpenses, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["ExpenseTypes"] = dbExpenseType;
                        opt.Items["CompanyCodes"] = dbCompany;

                    });

                    _assignmentAdditionalExpenseRepository.Add(assignmentDatabaseCollection.DBAssignmentAdditionalExpenses);
                    if (commitChange)
                        _assignmentAdditionalExpenseRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentAdditionalExpenses);
            }
            finally { _assignmentAdditionalExpenseRepository.AutoSave = true; }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response UpdateAssignmentAdditionalExpenses(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpenses, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange)
        {
            Exception exception = null;
            try
            {
                if (assignmentAdditionalExpenses?.Count > 0)
                {
                    IList<string> companyCodes = assignmentAdditionalExpenses.Select(x => x.CompanyCode).ToList();
                    IList<string> expenseTypes = assignmentAdditionalExpenses.Select(x => x.ExpenseType).ToList();
                    var dbExpenseType = assignmentDatabaseCollection.DBExpenseType;
                    var dbCompany = assignmentDatabaseCollection.Assignment.DBCompanies;

                    assignmentDatabaseCollection.DBAssignmentAdditionalExpenses.ToList().ForEach(dbExpense =>
                    {
                        var expenseToBeModify =
                            assignmentAdditionalExpenses.FirstOrDefault(x =>
                                x.AssignmentAdditionalExpenseId == dbExpense.Id);
                        if (expenseToBeModify != null)
                        {
                            _mapper.Map(expenseToBeModify, dbExpense, opt =>
                            {
                                opt.Items["isAssignId"] = true;
                                opt.Items["ExpenseTypes"] = dbExpenseType;
                                opt.Items["CompanyCodes"] = dbCompany;

                            });
                            dbExpense.LastModification = DateTime.UtcNow;
                            dbExpense.UpdateCount = expenseToBeModify.UpdateCount.CalculateUpdateCount();
                            dbExpense.ModifiedBy = expenseToBeModify.ModifiedBy;
                        }
                    });
                    _assignmentAdditionalExpenseRepository.AutoSave = false;
                    _assignmentAdditionalExpenseRepository.Update(assignmentDatabaseCollection
                        .DBAssignmentAdditionalExpenses);
                    if (commitChange)
                        _assignmentAdditionalExpenseRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentAdditionalExpenses);
            }
            finally
            {
                _assignmentAdditionalExpenseRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response DeleteAssignmentAdditionalExpense(IList<DomainModel.AssignmentAdditionalExpense> assignmentAdditionalExpenses, IList<DbModel.AssignmentAdditionalExpense> dbAssignmentAdditionalExpenses, bool commitChange)
        {
            Exception exception = null;
            try
            {
                if (assignmentAdditionalExpenses?.Count > 0)
                {
                    _assignmentAdditionalExpenseRepository.AutoSave = false;
                    _assignmentAdditionalExpenseRepository.Delete(dbAssignmentAdditionalExpenses);
                    if (commitChange)
                        _assignmentAdditionalExpenseRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentAdditionalExpenses);
            }
            finally
            {
                _assignmentAdditionalExpenseRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response ProcessAssignmentInterCompany(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCoDiscounts, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, ValidationType validationType)
        {
            Response response = null;
            try
            {
                if (assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts?.Count > 0)
                    //if ((ValidationType.Delete != validationType || ValidationType.Add != validationType) && assignmentDatabaseCollection.DBAssignmentTechnicalSpecialists != null)
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                        response = this.UpdateInterCompanyDiscounts(assignmentInterCoDiscounts, ref assignmentDatabaseCollection, true);

                if (assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts?.Count > 0)
                    if (validationType == ValidationType.Update || validationType == ValidationType.Delete)
                        response = this.DeleteInterCompanyDiscounts(assignmentInterCoDiscounts, ref assignmentDatabaseCollection, true);

                if (assignmentInterCoDiscounts != null)
                    if (ValidationType.Delete != validationType)
                        response = this.AddInterCompanyDiscounts(assignmentInterCoDiscounts, ref assignmentDatabaseCollection, true);

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentInterCoDiscounts);
            }

            return response;
        }

        private Response AddInterCompanyDiscounts(DomainModel.AssignmentInterCoDiscountInfo interCompanyDiscounts, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange)
        {
            Exception exception = null;
            try
            {
                assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts = PopulateRecordsToAdd(interCompanyDiscounts, assignmentDatabaseCollection.DBAssignment.FirstOrDefault(), assignmentDatabaseCollection.Assignment.DBCompanies, assignmentDatabaseCollection.Assignment.DBContracts?.FirstOrDefault());
                if (assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts?.Count > 0)
                {
                    _assignmentInterCompanyDiscount.AutoSave = false;
                    _assignmentInterCompanyDiscount.Add(assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts);
                    if (commitChange)
                        _assignmentInterCompanyDiscount.ForceSave();

                    AddVisitTimesheetInterCompanyDiscounts(assignmentDatabaseCollection, commitChange);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), interCompanyDiscounts);
            }
            finally
            {
                _assignmentInterCompanyDiscount.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private void AddVisitTimesheetInterCompanyDiscounts(DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange)
        {
            Exception exception = null;
            try
            {
                if (assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts?.Any() == true && assignmentDatabaseCollection.dbVisit != null)
                {
                    if (assignmentDatabaseCollection.dbVisit.VisitInterCompanyDiscount?.Any() == null || assignmentDatabaseCollection.dbVisit.VisitInterCompanyDiscount?.Any() == false)
                    {
                        IList<DbModel.VisitInterCompanyDiscount> dbVisitInterCompanyDiscount = null;
                        long visitId = assignmentDatabaseCollection.dbVisit.Id;
                        dbVisitInterCompanyDiscount = assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts?.Select(x => new DbModel.VisitInterCompanyDiscount
                        {
                            VisitId = visitId,
                            DiscountType = x.DiscountType,
                            CompanyId = x.CompanyId,
                            Description = x.Description,
                            Percentage = x.Percentage,
                            ModifiedBy = x.ModifiedBy,
                            LastModification = x.LastModification,
                            UpdateCount = x.UpdateCount
                        }).ToList();
                        _visitInterCompanyRepository.Add(dbVisitInterCompanyDiscount);
                        if (commitChange)
                            _visitInterCompanyRepository.ForceSave();
                    }
                }
                if (assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts?.Any() == true && assignmentDatabaseCollection.dbTimesheet != null)
                {
                    if (assignmentDatabaseCollection.dbTimesheet?.TimesheetInterCompanyDiscount?.Any() == null || assignmentDatabaseCollection.dbTimesheet.TimesheetInterCompanyDiscount?.Any() == false)
                    {
                        IList<DbModel.TimesheetInterCompanyDiscount> dbTimesheetInterCompanyDiscount = null;
                        long timesheetId = assignmentDatabaseCollection.dbTimesheet.Id;
                        dbTimesheetInterCompanyDiscount = assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts?.Select(x => new DbModel.TimesheetInterCompanyDiscount
                        {
                            TimesheetId = timesheetId,
                            DiscountType = x.DiscountType,
                            CompanyId = x.CompanyId,
                            Description = x.Description,
                            Percentage = x.Percentage,
                            ModifiedBy = x.ModifiedBy,
                            LastModification = x.LastModification,
                            UpdateCount = x.UpdateCount
                        }).ToList();
                        _timesheetInterCompanyRepository.Add(dbTimesheetInterCompanyDiscount);
                        if (commitChange)
                            _timesheetInterCompanyRepository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }

        private Response UpdateInterCompanyDiscounts(DomainModel.AssignmentInterCoDiscountInfo intercompanyDiscounts, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange)

        {
            Exception exception = null;
            try
            {
                _assignmentInterCompanyDiscount.AutoSave = false;
                assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts = PopulateRecordsToUpdate(intercompanyDiscounts, assignmentDatabaseCollection.DBAssignment.FirstOrDefault());
                if (assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts?.Count > 0)
                    _assignmentInterCompanyDiscount.Update(assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts);
                if (commitChange)
                    _assignmentInterCompanyDiscount.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), intercompanyDiscounts);
            }
            finally
            {
                _assignmentInterCompanyDiscount.AutoSave = true;
                // _repository.Dispose();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response DeleteInterCompanyDiscounts(DomainModel.AssignmentInterCoDiscountInfo interCompanyDiscount, ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection, bool commitChange)
        {
            Exception exception = null;
            try
            {
                if (interCompanyDiscount != null && interCompanyDiscount.AssignmentId > 0)
                {
                    _assignmentInterCompanyDiscount.AutoSave = false;
                    assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts = PopulateRecordsToDelete(interCompanyDiscount, assignmentDatabaseCollection.DBAssignment.FirstOrDefault());
                    if (assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts?.Count > 0)
                        _assignmentInterCompanyDiscount.Delete(assignmentDatabaseCollection.DBAssignmentInterCompanyDiscounts);
                    if (commitChange)
                        _assignmentInterCompanyDiscount.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), interCompanyDiscount);
            }
            finally
            {
                _assignmentInterCompanyDiscount.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private static DbModel.AssignmentInterCompanyDiscount GetDbRecordsToAdd(int assignmentId, string discountType, int companyId, string description, decimal discountPercentage,string amendmentReason)
        {
            return new DbModel.AssignmentInterCompanyDiscount()
            {
                AssignmentId = assignmentId,
                DiscountType = discountType,
                CompanyId = companyId,
                Description = description,
                Percentage = discountPercentage,
                AmendmentReason = amendmentReason

            };
        }

        private static DbModel.AssignmentInterCompanyDiscount GetRecordsToUpdate(string discountType, decimal discountPercentage, string description, string modifiedBy, String amendmentReason, IList<DbModel.AssignmentInterCompanyDiscount> interCompanyDiscounts)
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

        private IList<DbModel.AssignmentInterCompanyDiscount> PopulateRecordsToAdd(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCoDiscountInfo, DbModel.Assignment dbAssignments, IList<DbModel.Company> dbCompanies,
                                                                            DbModel.Contract dbContract)
        {
            IList<DbModel.AssignmentInterCompanyDiscount> dbInterCompanyDiscounts = dbAssignments?.AssignmentInterCompanyDiscount?.ToList();
            IList<DbModel.AssignmentInterCompanyDiscount> recordToAdd = new List<DbModel.AssignmentInterCompanyDiscount>();
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.ParentContractHoldingCompanyCode) && assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDiscount != null && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDescription)
                && dbContract?.ContractType == ContractType.CHD.ToString() && (dbInterCompanyDiscounts?.Count > 0 ? dbInterCompanyDiscounts?.ToList()?.Select(x => x.DiscountType == AssignmentInterCompanyDiscountType.ParentContract.DisplayName()) : null) == null)
            {
                var companyId = dbCompanies?.ToList().FirstOrDefault(x => x.Code == assignmentInterCoDiscountInfo.ParentContractHoldingCompanyCode)?.Id;
                var data = GetDbRecordsToAdd(dbAssignments.Id, AssignmentInterCompanyDiscountType.ParentContract.DisplayName(), (int)companyId, assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDescription, (decimal)assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDiscount, assignmentInterCoDiscountInfo.AmendmentReason);
                if (data != null)
                    recordToAdd.Add(data);
            }

            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentOperatingCompanyCode) && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentHostcompanyCode) && assignmentInterCoDiscountInfo.AssignmentHostcompanyDiscount != null
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentHostcompanyDescription) && (dbInterCompanyDiscounts?.Count > 0 ? dbInterCompanyDiscounts?.ToList()?.Select(x => x.DiscountType == AssignmentInterCompanyDiscountType.OperatingCountryCompany.DisplayName()) : null) == null
                && assignmentInterCoDiscountInfo.AssignmentOperatingCompanyCode != assignmentInterCoDiscountInfo.AssignmentHostcompanyCode)
            {
                var companyId = dbCompanies?.ToList().FirstOrDefault(x => x.Code == assignmentInterCoDiscountInfo.AssignmentHostcompanyCode)?.Id;
                var data = GetDbRecordsToAdd(dbAssignments.Id, AssignmentInterCompanyDiscountType.OperatingCountryCompany.DisplayName(), (int)companyId, assignmentInterCoDiscountInfo.AssignmentHostcompanyDescription, (decimal)assignmentInterCoDiscountInfo.AssignmentHostcompanyDiscount, assignmentInterCoDiscountInfo.AmendmentReason);
                if (data != null)
                    recordToAdd.Add(data);
            }

            if (assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount != null && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code)
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Description) && (dbInterCompanyDiscounts?.Count > 0 ? dbInterCompanyDiscounts?.ToList()?.Select(x => x.DiscountType == AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId.DisplayName()) : null) == null)
            {
                var companyId = dbCompanies?.ToList().FirstOrDefault(x => x.Code == assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code)?.Id;
                var data = GetDbRecordsToAdd(dbAssignments.Id, AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId.DisplayName(), (int)companyId, assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Description, (decimal)assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount, assignmentInterCoDiscountInfo.AmendmentReason);
                if (data != null)
                    recordToAdd.Add(data);
            }

            if (assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount != null && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code) && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Description)
                && (dbInterCompanyDiscounts?.Count > 0 ? dbInterCompanyDiscounts?.ToList()?.Select(x => x.DiscountType == AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2.DisplayName()) : null) == null)

            {
                var companyId = dbCompanies?.ToList().FirstOrDefault(x => x.Code == assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code)?.Id;
                var data = GetDbRecordsToAdd(dbAssignments.Id, AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2.DisplayName(), (int)companyId, assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Description, (decimal)assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount, assignmentInterCoDiscountInfo.AmendmentReason);
                if (data != null)
                    recordToAdd.Add(data);
            }

            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyCode) && assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount != null
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDescription) && dbAssignments.ContractCompanyId != dbAssignments.OperatingCompanyId
                && (dbInterCompanyDiscounts?.Count > 0 ? dbInterCompanyDiscounts?.ToList()?.Select(x => x.DiscountType == AssignmentInterCompanyDiscountType.Contract.DisplayName()) : null) == null)
            {
                var companyId = dbCompanies?.ToList().FirstOrDefault(x => x.Code == assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyCode)?.Id;
                var data = GetDbRecordsToAdd(dbAssignments.Id, AssignmentInterCompanyDiscountType.Contract.DisplayName(), (int)companyId, assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDescription, (decimal)assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount, assignmentInterCoDiscountInfo.AmendmentReason);
                if (data != null)
                    recordToAdd.Add(data);
            }
            return recordToAdd;
        }

        private IList<DbModel.AssignmentInterCompanyDiscount> PopulateRecordsToUpdate(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCoDiscountInfo, DbModel.Assignment dbAssignments)
        {
            var dbRecordToUpdate = dbAssignments?.AssignmentInterCompanyDiscount?.ToList();
            IList<DbModel.AssignmentInterCompanyDiscount> recordToUpdate = new List<DbModel.AssignmentInterCompanyDiscount>();

            // Parent Contract
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.ParentContractHoldingCompanyCode) && assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDiscount != null
            && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDescription) && dbRecordToUpdate?.ToList()?.Select(x => x.DiscountType == AssignmentInterCompanyDiscountType.ParentContract.DisplayName()) != null)
                recordToUpdate.Add(GetRecordsToUpdate(AssignmentInterCompanyDiscountType.ParentContract.DisplayName(), (decimal)assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDiscount, assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDescription, assignmentInterCoDiscountInfo.ModifiedBy, assignmentInterCoDiscountInfo.AmendmentReason, dbRecordToUpdate));

            // Additional InterCompany Office 1
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code)
                && assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount != null
                && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Description)
                && dbRecordToUpdate?.ToList()?.Select(x => x.DiscountType == AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId.DisplayName()) != null)
            {
                var data = GetRecordsToUpdate(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId.DisplayName(), (decimal)assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount, assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Description, assignmentInterCoDiscountInfo.ModifiedBy, assignmentInterCoDiscountInfo.AmendmentReason , dbRecordToUpdate);
                if (data != null)
                    recordToUpdate.Add(data);
            }

            // Additional InterCompany Office 2
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code) && assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount != null && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Description)
                && dbRecordToUpdate?.ToList()?.Select(x => x.DiscountType == AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2.DisplayName()) != null)
            {
                var data = GetRecordsToUpdate(AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2.DisplayName(), (decimal)assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount, assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Description, assignmentInterCoDiscountInfo.ModifiedBy, assignmentInterCoDiscountInfo.AmendmentReason ,dbRecordToUpdate);
                if (data != null)
                    recordToUpdate.Add(data);
            }

            // Host Company
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentHostcompanyCode) && assignmentInterCoDiscountInfo.AssignmentHostcompanyDiscount != null && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentHostcompanyDescription)
                && dbRecordToUpdate?.ToList()?.Select(x => x.DiscountType == AssignmentInterCompanyDiscountType.OperatingCountryCompany.DisplayName()) != null)
            {
                var data = GetRecordsToUpdate(AssignmentInterCompanyDiscountType.OperatingCountryCompany.DisplayName(), (decimal)assignmentInterCoDiscountInfo.AssignmentHostcompanyDiscount, assignmentInterCoDiscountInfo.AssignmentHostcompanyDescription, assignmentInterCoDiscountInfo.ModifiedBy, assignmentInterCoDiscountInfo.AmendmentReason, dbRecordToUpdate);
                if (data != null)
                    recordToUpdate.Add(data);
            }

            // Contract Company
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyCode) && assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount != null && !string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDescription)
                  && dbRecordToUpdate?.ToList()?.Select(x => x.DiscountType == AssignmentInterCompanyDiscountType.Contract.DisplayName()) != null)
            {
                var data = GetRecordsToUpdate(AssignmentInterCompanyDiscountType.Contract.DisplayName(), (decimal)assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount, assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDescription, assignmentInterCoDiscountInfo.ModifiedBy, assignmentInterCoDiscountInfo.AmendmentReason, dbRecordToUpdate);
                if (data != null)
                    recordToUpdate.Add(data);
            }

            return recordToUpdate;
        }

        private IList<DbModel.AssignmentInterCompanyDiscount> PopulateRecordsToDelete(DomainModel.AssignmentInterCoDiscountInfo assignmentInterCoDiscountInfo, DbModel.Assignment dbAssignments)
        {
            IList<DbModel.AssignmentInterCompanyDiscount> dbInterCompanyDiscounts = dbAssignments?.AssignmentInterCompanyDiscount?.ToList();
            IList<DbModel.AssignmentInterCompanyDiscount> recordToDelete = new List<DbModel.AssignmentInterCompanyDiscount>();

            // Parent Contract
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.ParentContractHoldingCompanyCode) && assignmentInterCoDiscountInfo.ParentContractHoldingCompanyDiscount != null && dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == EnumExtension.DisplayName(AssignmentInterCompanyDiscountType.ParentContract)) != null)
                recordToDelete.Add(dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == AssignmentInterCompanyDiscountType.ParentContract.DisplayName()));

            // Additional InterCompany Office 1
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Code) && assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany1_Discount != null
                && dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId.DisplayName()) != null)
                recordToDelete.Add(dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId.DisplayName()));

            // Additional InterCompany Office 2
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Code) && assignmentInterCoDiscountInfo.AssignmentAdditionalIntercompany2_Discount != null
                && dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2.DisplayName()) != null)
                recordToDelete.Add(dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == AssignmentInterCompanyDiscountType.AdditionalIntercoOfficeCountryId2.DisplayName()));

            // Host Company
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentHostcompanyCode) && assignmentInterCoDiscountInfo.AssignmentHostcompanyDiscount != null
                && dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == AssignmentInterCompanyDiscountType.OperatingCountryCompany.DisplayName()) != null)
                recordToDelete.Add(dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == AssignmentInterCompanyDiscountType.OperatingCountryCompany.DisplayName()));

            // Contract Company
            if (!string.IsNullOrEmpty(assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyCode) && assignmentInterCoDiscountInfo.AssignmentContractHoldingCompanyDiscount != null
                  && dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == AssignmentInterCompanyDiscountType.Contract.DisplayName()) != null)
                recordToDelete.Add(dbInterCompanyDiscounts.FirstOrDefault(x => x.DiscountType == AssignmentInterCompanyDiscountType.Contract.DisplayName()));

            return recordToDelete;
        }

        private Response ProcessAssignmentDocument(IList<ModuleDocument> assignmentDocuments, ValidationType validationType, ref List<DbModel.Document> dbDocuments)
        {
            Response response = null;
            try
            {
                if (assignmentDocuments != null)
                {
                    if (ValidationType.Delete != validationType)
                        response = this._documentService.Save(assignmentDocuments, ref dbDocuments);
                    if (ValidationType.Delete != validationType || ValidationType.Add != validationType)
                        response = this._documentService.Modify(assignmentDocuments, ref dbDocuments);

                    if (validationType == ValidationType.Update || validationType == ValidationType.Delete)
                        response = this._documentService.Delete(assignmentDocuments);

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), assignmentDocuments);
            }

            return response;
        }

        private Response ProcessArsSearch(ResourceSearch.Domain.Models.ResourceSearch.ResourceSearch resourceSearch,
                                         ref DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection)
        {
            Response response = null;
            try
            {
                if (resourceSearch != null)
                {
                    assignmentDatabaseCollection.DBSubCategory = assignmentDatabaseCollection.DBService?.Select(x => x.TaxonomySubCategory)?.ToList();
                    assignmentDatabaseCollection.DBCategory = assignmentDatabaseCollection.DBService?.Select(x => x.TaxonomySubCategory)?.Select(x => x.TaxonomyCategory)?.ToList();
                    assignmentDatabaseCollection.DBARSCoordinators = assignmentDatabaseCollection.Assignment.DBOperatingUsers != null && assignmentDatabaseCollection.Assignment.DBOperatingUsers.Count > 0
                                                                    ? assignmentDatabaseCollection.Assignment.DBContractCoordinatorUsers?.Concat(assignmentDatabaseCollection.Assignment.DBOperatingUsers).ToList()
                                                                    : assignmentDatabaseCollection.Assignment.DBContractCoordinatorUsers.ToList();
                    if (resourceSearch.RecordStatus.IsRecordStatusNew() && resourceSearch.SearchAction != null)
                        response = this._resourceSearchService.Save(resourceSearch, ref assignmentDatabaseCollection.DBARSSearches, ref assignmentDatabaseCollection.Assignment.DBCustomers, ref assignmentDatabaseCollection.Assignment.DBCompanies,
                                                                    ref assignmentDatabaseCollection.DBARSCoordinators, ref assignmentDatabaseCollection.DBCategory, ref assignmentDatabaseCollection.DBSubCategory,
                                                                    ref assignmentDatabaseCollection.DBService, ref assignmentDatabaseCollection.DBOverrideResources, ref assignmentDatabaseCollection.DBTechnicalSpecialists,
                                                                    ref assignmentDatabaseCollection.DBARSAssignment, false, true, assignmentDatabaseCollection.dbModule);

                    if (resourceSearch.RecordStatus.IsRecordStatusModified())
                        response = this._resourceSearchService.Modify(resourceSearch,
                                                                    ref assignmentDatabaseCollection.DBARSSearches, ref assignmentDatabaseCollection.Assignment.DBCustomers, ref assignmentDatabaseCollection.Assignment.DBCompanies,
                                                                    ref assignmentDatabaseCollection.DBARSCoordinators, ref assignmentDatabaseCollection.DBCategory, ref assignmentDatabaseCollection.DBSubCategory,
                                                                    ref assignmentDatabaseCollection.DBService, ref assignmentDatabaseCollection.DBOverrideResources, ref assignmentDatabaseCollection.DBTechnicalSpecialists,
                                                                    ref assignmentDatabaseCollection.DBARSAssignment, false, true, assignmentDatabaseCollection.dbModule);

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), resourceSearch);
            }

            return response;
        }

        private void ProcessVisitTimesheetSkeletonDefaultLineItems(DomainModel.AssignmentDetail assignmentDetail, DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection)
        {
            try
            {
                if (assignmentDetail.AssignmentInfo.AssignmentProjectWorkFlow.Trim().IsVisitEntry())
                {
                    if (assignmentDatabaseCollection.dbVisit?.Id > 0 && assignmentDatabaseCollection.dbAddedVisitTS != null && assignmentDatabaseCollection.dbAddedVisitTS.Count > 0)
                        this.AddSkeletonVisitLineItems(assignmentDetail, assignmentDatabaseCollection);
                }
                else if (!assignmentDetail.AssignmentInfo.AssignmentProjectWorkFlow.Trim().IsVisitEntry())
                {
                    if (assignmentDatabaseCollection.dbTimesheet?.Id > 0 && assignmentDatabaseCollection.dbAddedTimesheetTS != null && assignmentDatabaseCollection.dbAddedTimesheetTS?.Count > 0)
                        this.AddSkeletonTimeSheetLineItems(assignmentDetail, assignmentDatabaseCollection);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
        }

        private void AddSkeletonTimeSheetLineItems(DomainModel.AssignmentDetail assignmentDetail, DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection)
        {
            List<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> dbTimesheetTechSpecCons = null;
            List<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> dbTimesheetTechSpecExpense = null;
            List<DbModel.TimesheetTechnicalSpecialistAccountItemTime> dbTimesheetTechSpecTime = null;
            List<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> dbTimesheetTechSpecTravel = null;
            try
            {
                var includes = new string[]
                {
                     "AssignmentTechnicalSpecialist",
                     "ContractChargeSchedule.ContractRate",
                     "TechnicalSpecialistPaySchedule.TechnicalSpecialistPayRate",
                };
                var assignTechSpecRateSchedules = this._assignmentTechnicalSpecialistScheduleRepository.GetAssignmentTechSpecRateSchedules(assignmentDatabaseCollection.dbTimesheet.AssignmentId, assignmentDatabaseCollection.dbLineItemExpense, includes);
                if (assignTechSpecRateSchedules == null) return;

                string expenseCurrency = assignmentDatabaseCollection.Assignment?.DBCompanies?.FirstOrDefault(x => x.Code == assignmentDetail.AssignmentInfo?.AssignmentOperatingCompanyCode)?.NativeCurrency;
                List<string> chargeCurrency = assignTechSpecRateSchedules.ChargeSchedules?.Select(x => x.ChargeScheduleCurrency)?.ToList();
                var payCurrency = assignTechSpecRateSchedules.PaySchedules?.Select(x => x.PayScheduleCurrency)?.ToList();
                var currencies = payCurrency != null && chargeCurrency != null ? chargeCurrency.Union(payCurrency) : chargeCurrency ?? payCurrency;

                IList<ExchangeRate> exchangeCurrencyRate = null;
                if (currencies?.Any() == true)
                {
                    exchangeCurrencyRate = currencies.Select(x => new ExchangeRate
                    {
                        CurrencyFrom = expenseCurrency,
                        CurrencyTo = x,
                        EffectiveDate = assignmentDetail.AssignmentInfo.TimesheetFromDate.GetValueOrDefault()
                    })?.ToList();
                }

                var exchangeRate = GetExpenseLineItemChargeExchangeRates(exchangeCurrencyRate, assignmentDatabaseCollection.Assignment.DBProjects.FirstOrDefault().ContractId);

                if (assignTechSpecRateSchedules.ChargeSchedules != null && assignTechSpecRateSchedules.ChargeSchedules.Count > 0)
                    ProcessSkeletonTimesheetChargeLineItems(assignTechSpecRateSchedules, assignmentDetail, assignmentDatabaseCollection, ref dbTimesheetTechSpecTime, ref dbTimesheetTechSpecTravel, ref dbTimesheetTechSpecExpense, ref dbTimesheetTechSpecCons, expenseCurrency, exchangeRate);
                if (assignTechSpecRateSchedules.PaySchedules != null && assignTechSpecRateSchedules.PaySchedules.Count > 0)
                    ProcessSkeletonTimesheetPayLineItems(assignTechSpecRateSchedules, assignmentDetail, assignmentDatabaseCollection, ref dbTimesheetTechSpecTime, ref dbTimesheetTechSpecTravel, ref dbTimesheetTechSpecExpense, ref dbTimesheetTechSpecCons, expenseCurrency, exchangeRate);

                if (dbTimesheetTechSpecTime?.Count > 0)
                {
                    _timesheetAccountItemTimeRepository.AutoSave = false;
                    _timesheetAccountItemTimeRepository.Add(dbTimesheetTechSpecTime);
                    _timesheetAccountItemTimeRepository.ForceSave();
                }

                if (dbTimesheetTechSpecTravel?.Count > 0)
                {
                    _timeAccountItemTravelRepository.AutoSave = false;
                    _timeAccountItemTravelRepository.Add(dbTimesheetTechSpecTravel);
                    _timeAccountItemTravelRepository.ForceSave();
                }

                if (dbTimesheetTechSpecExpense?.Count > 0)
                {
                    _timeAccountItemExpenseRepository.AutoSave = false;
                    _timeAccountItemExpenseRepository.Add(dbTimesheetTechSpecExpense);
                    _timeAccountItemExpenseRepository.ForceSave();
                }

                if (dbTimesheetTechSpecCons?.Count > 0)
                {
                    _timeAccountItemConsumableRepository.AutoSave = false;
                    _timeAccountItemConsumableRepository.Add(dbTimesheetTechSpecCons);
                    _timeAccountItemConsumableRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            finally
            {
                _timesheetAccountItemTimeRepository.AutoSave = true;
                _timeAccountItemExpenseRepository.AutoSave = true;
                _timeAccountItemConsumableRepository.AutoSave = true;
                _timeAccountItemTravelRepository.AutoSave = true;
            }

        }

        private void ProcessSkeletonTimesheetChargeLineItems(DomainModel.AssignmentTechSpecSchedules rateSchedules, DomainModel.AssignmentDetail assignmentDetail, DomainModel.AssignmentModuleDatabaseCollection assignmentDbCollection,
                        ref List<DbModel.TimesheetTechnicalSpecialistAccountItemTime> timesheetTechnicalSpecialistTimes, ref List<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> timesheetTechnicalSpecialistTravels,
                        ref List<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> timesheetTechnicalSpecialistExpenses, ref List<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> timesheetTechnicalSpecialistConsumables,
                        string expenseCurrency, Response exchangeRate)
        {
            string payScheduleCurrency = string.Empty;
            using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                for (int i = 0, len = rateSchedules.ChargeSchedules.Count; i < len; i++)
                {
                    var eachChargeSchedules = rateSchedules.ChargeSchedules[i];
                    if (eachChargeSchedules != null && eachChargeSchedules.ChargeScheduleRates != null)
                    {
                        var techSpecId = assignmentDbCollection.dbAddedTimesheetTS?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == eachChargeSchedules.TechnicalSpecialistId)?.Id;
                        payScheduleCurrency = rateSchedules.PaySchedules?.FirstOrDefault(x => x.TechnicalSpecialistId == eachChargeSchedules.TechnicalSpecialistId)?.PayScheduleCurrency;  //D-1311
                        for (int j = 0, ratesLen = eachChargeSchedules.ChargeScheduleRates.Count; j < ratesLen; j++)
                        {
                            DomainModel.ChargeScheduleRates eachCsRate = eachChargeSchedules.ChargeScheduleRates[j];
                            var payExchangeRate = !string.IsNullOrEmpty(expenseCurrency) && !string.IsNullOrEmpty(payScheduleCurrency) && expenseCurrency == payScheduleCurrency ?
                                             Convert.ToDecimal(String.Format("{0:0.000000}", 1)) :
                                             exchangeRate.Result?.Populate<List<ExchangeRate>>()?.FirstOrDefault(x => x.CurrencyFrom == expenseCurrency && x.CurrencyTo == payScheduleCurrency)?.Rate ?? Convert.ToDecimal(String.Format("{0:0.000000}", 0));

                            var chargeExchangeRate = !string.IsNullOrEmpty(expenseCurrency) && !string.IsNullOrEmpty(eachCsRate.Currency) && expenseCurrency == eachCsRate.Currency ?
                                                   Convert.ToDecimal(String.Format("{0:0.000000}", 1)) :
                                                   exchangeRate.Result?.Populate<List<ExchangeRate>>()?.FirstOrDefault(x => x.CurrencyFrom == expenseCurrency && x.CurrencyTo == eachCsRate.Currency)?.Rate ?? Convert.ToDecimal(String.Format("{0:0.000000}", 0));

                            if ((eachCsRate.EffectiveTo == null ||
                                eachCsRate.EffectiveTo >= assignmentDetail.AssignmentInfo.TimesheetFromDate) && eachCsRate.IsActive == true)
                            {
                                //Time
                                if (eachCsRate.Type == "R" && (timesheetTechnicalSpecialistTimes == null || !(timesheetTechnicalSpecialistTimes.Exists(x =>
                                        x.ExpenseChargeTypeId == eachCsRate.ChargeTypeId &&
                                        x.TimesheetTechnicalSpeciallistId == techSpecId))))
                                {
                                    if (timesheetTechnicalSpecialistTimes == null)
                                        timesheetTechnicalSpecialistTimes = new List<DbModel.TimesheetTechnicalSpecialistAccountItemTime>();
                                    timesheetTechnicalSpecialistTimes.Add(
                                        new DbModel.TimesheetTechnicalSpecialistAccountItemTime
                                        {
                                            AssignmentId = assignmentDbCollection.dbTimesheet.AssignmentId,
                                            TimesheetId = assignmentDbCollection.dbTimesheet.Id,
                                            ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                            ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                            TimesheetTechnicalSpeciallistId = (long)techSpecId,
                                            ExpenseDate = assignmentDetail.AssignmentInfo.TimesheetFromDate.GetValueOrDefault(),
                                            ExpenseChargeTypeId = eachCsRate.ChargeTypeId.GetValueOrDefault(),
                                            SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                            WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                            ChargeRateCurrency = eachCsRate.Currency,
                                            ChargeRateDescription = eachCsRate.Description,
                                            PayRateCurrency = payScheduleCurrency,//eachCsRate.Currency,
                                            ChargeRate = 0,
                                            ChargeTotalUnit = 0,
                                            PayTotalUnit = 0,
                                            PayRate = 0,
                                            InvoicingStatus = "N" //To be taken from Invoice Status enum when finance module comes into place
                                        });
                                    continue;
                                }

                                //Travel
                                if (eachCsRate.Type == "T" && (timesheetTechnicalSpecialistTravels == null || !(timesheetTechnicalSpecialistTravels.Exists(x =>
                                        x.ChargeExpenseTypeId == eachCsRate.ChargeTypeId &&
                                        x.TimesheetTechnicalSpecialistId == techSpecId))))
                                {
                                    if (timesheetTechnicalSpecialistTravels == null)
                                        timesheetTechnicalSpecialistTravels = new List<DbModel.TimesheetTechnicalSpecialistAccountItemTravel>();
                                    timesheetTechnicalSpecialistTravels.Add(
                                        new DbModel.TimesheetTechnicalSpecialistAccountItemTravel
                                        {
                                            AssignmentId = assignmentDbCollection.dbTimesheet.AssignmentId,
                                            TimesheetId = assignmentDbCollection.dbTimesheet.Id,
                                            ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                            ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                            TimesheetTechnicalSpecialistId = (long)techSpecId,
                                            ExpenceDate = assignmentDetail.AssignmentInfo.TimesheetFromDate.GetValueOrDefault(),
                                            ChargeExpenseTypeId = eachCsRate.ChargeTypeId.GetValueOrDefault(),
                                            SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                            WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                            ChargeRateCurrency = eachCsRate.Currency,
                                            PayRateCurrency = payScheduleCurrency,//eachCsRate.Currency,
                                            ChargeTotalUnit = 0,
                                            PayTotalUnit = 0,
                                            ChargeRate = 0,
                                            PayRate = 0,
                                            PayExpenseTypeId = eachCsRate.ChargeTypeId.GetValueOrDefault(),
                                            InvoicingStatus = "N"  //To be taken from Invoice Status enum when finance module comes into place
                                        });
                                    continue;
                                }

                                //Expense
                                if (eachCsRate.Type == "E" && (timesheetTechnicalSpecialistExpenses == null || !(timesheetTechnicalSpecialistExpenses.Exists(x =>
                                        x.ExpenseChargeTypeId == eachCsRate.ChargeTypeId &&
                                        x.TimesheetTechnicalSpeciallistId == techSpecId))))
                                {
                                    if (timesheetTechnicalSpecialistExpenses == null)
                                        timesheetTechnicalSpecialistExpenses = new List<DbModel.TimesheetTechnicalSpecialistAccountItemExpense>();
                                    timesheetTechnicalSpecialistExpenses.Add(
                                        new DbModel.TimesheetTechnicalSpecialistAccountItemExpense
                                        {
                                            AssignmentId = assignmentDbCollection.dbTimesheet.AssignmentId,
                                            TimesheetId = assignmentDbCollection.dbTimesheet.Id,
                                            ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                            ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                            TimesheetTechnicalSpeciallistId = (long)techSpecId,
                                            ExpenseDate = assignmentDetail.AssignmentInfo.TimesheetFromDate.GetValueOrDefault(),
                                            ExpenseChargeTypeId = eachCsRate.ChargeTypeId.GetValueOrDefault(),
                                            SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                            WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                            ChargeRateCurrency = eachCsRate.Currency,
                                            PayRateCurrency = payScheduleCurrency,//eachCsRate.Currency,
                                            ExpenceCurrency = expenseCurrency,//payScheduleCurrency,//eachCsRate.Currency,
                                            //ChargeExchangeRate = !string.IsNullOrEmpty(eachCsRate.Currency) ? Convert.ToDecimal(String.Format("{0:0.000000}", 1)) : Convert.ToDecimal(String.Format("{0:0.000000}", 0)),
                                            //PayExchangeRate = !string.IsNullOrEmpty(eachCsRate.Currency) ? Convert.ToDecimal(String.Format("{0:0.000000}", 1)) : Convert.ToDecimal(String.Format("{0:0.000000}", 0)),
                                            ChargeExchangeRate = chargeExchangeRate,
                                            PayExchangeRate = payExchangeRate,
                                            ChargeTotalUnit = 0,
                                            PayTotalUnit = 0,
                                            ChargeRate = 0,
                                            PayRate = 0,
                                            PayRateTax = 0,
                                            InvoicingStatus = "N"  //To be taken from Invoice Status enum when finance module comes into place
                                        });
                                    continue;
                                }

                                //Consumable
                                if ((eachCsRate.Type == "C" || eachCsRate.Type == "Q") && (timesheetTechnicalSpecialistConsumables == null || !(timesheetTechnicalSpecialistConsumables.Exists(x =>
                                        x.ChargeExpenseTypeId == eachCsRate.ChargeTypeId &&
                                        x.TimesheetTechnicalSpecialistId == techSpecId))))
                                {
                                    if (timesheetTechnicalSpecialistConsumables == null)
                                        timesheetTechnicalSpecialistConsumables = new List<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable>();
                                    timesheetTechnicalSpecialistConsumables.Add(
                                        new DbModel.TimesheetTechnicalSpecialistAccountItemConsumable()
                                        {
                                            AssignmentId = assignmentDbCollection.dbTimesheet.AssignmentId,
                                            TimesheetId = assignmentDbCollection.dbTimesheet.Id,
                                            ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                            ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                            TimesheetTechnicalSpecialistId = (long)techSpecId,
                                            ExpenceDate = assignmentDetail.AssignmentInfo.TimesheetFromDate.GetValueOrDefault(),
                                            ChargeExpenseTypeId = eachCsRate.ChargeTypeId.GetValueOrDefault(),
                                            SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                            WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                            ChargeRateCurrency = eachCsRate.Currency,
                                            ChargeDescription = eachCsRate.Description,
                                            PayRateCurrency = payScheduleCurrency,//eachCsRate.Currency,
                                            ChargeTotalUnit = 0,
                                            PayTotalUnit = 0,
                                            ChargeRate = 0,
                                            PayRate = 0,
                                            PayExpenseTypeId = eachCsRate.ChargeTypeId.GetValueOrDefault(),
                                            InvoicingStatus = "N" //To be taken from Invoice Status enum when finance module comes into place
                                        });
                                }

                            }
                        }
                    }
                }
                tranScope.Complete();
            }
        }

        private void ProcessSkeletonTimesheetPayLineItems(DomainModel.AssignmentTechSpecSchedules rateSchedules, DomainModel.AssignmentDetail assignmentDetail, DomainModel.AssignmentModuleDatabaseCollection assignmentDbCollection,
                        ref List<DbModel.TimesheetTechnicalSpecialistAccountItemTime> timesheetTechnicalSpecialistTimes, ref List<DbModel.TimesheetTechnicalSpecialistAccountItemTravel> timesheetTechnicalSpecialistTravels,
                        ref List<DbModel.TimesheetTechnicalSpecialistAccountItemExpense> timesheetTechnicalSpecialistExpenses, ref List<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable> timesheetTechnicalSpecialistConsumables,
                       string expenseCurrency, Response exchangeRate)
        {
            string chargeScheduleCurrency = string.Empty;
            using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                for (int i = 0, len = rateSchedules.PaySchedules.Count; i < len; i++)
                {
                    DomainModel.TechnicalSpecialistPaySchedule eachPaySchedules = rateSchedules.PaySchedules[i];
                    if (eachPaySchedules != null && eachPaySchedules.PayScheduleRates != null)
                    {
                        var techSpecId = assignmentDbCollection.dbAddedTimesheetTS?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == eachPaySchedules.TechnicalSpecialistId)?.Id;
                        chargeScheduleCurrency = rateSchedules.ChargeSchedules?.FirstOrDefault(x => x.TechnicalSpecialistId == eachPaySchedules.TechnicalSpecialistId)?.ChargeScheduleCurrency;  //D-1311
                        for (int j = 0, ratesLen = eachPaySchedules.PayScheduleRates.Count; j < ratesLen; j++)
                        {
                            DomainModel.PayScheduleRates eachCsRate = eachPaySchedules.PayScheduleRates[j];
                            var payExchangeRate = !string.IsNullOrEmpty(expenseCurrency) && !string.IsNullOrEmpty(eachCsRate.Currency) && expenseCurrency == eachCsRate.Currency ?
                                              Convert.ToDecimal(String.Format("{0:0.000000}", 1)) :
                                              exchangeRate.Result?.Populate<List<ExchangeRate>>()?.FirstOrDefault(x => x.CurrencyFrom == expenseCurrency && x.CurrencyTo == eachCsRate.Currency)?.Rate ?? Convert.ToDecimal(String.Format("{0:0.000000}", 0));

                            var chargeExchangeRate = !string.IsNullOrEmpty(expenseCurrency) && !string.IsNullOrEmpty(chargeScheduleCurrency) && expenseCurrency == chargeScheduleCurrency ?
                                                   Convert.ToDecimal(String.Format("{0:0.000000}", 1)) :
                                                   exchangeRate.Result?.Populate<List<ExchangeRate>>()?.FirstOrDefault(x => x.CurrencyFrom == expenseCurrency && x.CurrencyTo == chargeScheduleCurrency)?.Rate ?? Convert.ToDecimal(String.Format("{0:0.000000}", 0));

                            if ((eachCsRate.EffectiveTo == null ||
                                eachCsRate.EffectiveTo >= assignmentDetail.AssignmentInfo.TimesheetFromDate) && eachCsRate.IsActive == true)
                            {
                                //Time
                                if (eachCsRate.Type == "R" && (timesheetTechnicalSpecialistTimes == null || !(timesheetTechnicalSpecialistTimes.Exists(x =>
                                        x.ExpenseChargeTypeId == eachCsRate.ExpenseTypeId &&
                                        x.TimesheetTechnicalSpeciallistId == techSpecId))))
                                {
                                    if (timesheetTechnicalSpecialistTimes == null)
                                        timesheetTechnicalSpecialistTimes = new List<DbModel.TimesheetTechnicalSpecialistAccountItemTime>();
                                    timesheetTechnicalSpecialistTimes.Add(
                                        new DbModel.TimesheetTechnicalSpecialistAccountItemTime
                                        {
                                            AssignmentId = assignmentDbCollection.dbTimesheet.AssignmentId,
                                            TimesheetId = assignmentDbCollection.dbTimesheet.Id,
                                            ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                            ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                            TimesheetTechnicalSpeciallistId = (long)techSpecId,
                                            ExpenseDate = assignmentDetail.AssignmentInfo.TimesheetFromDate.GetValueOrDefault(),
                                            ExpenseChargeTypeId = eachCsRate.ExpenseTypeId.GetValueOrDefault(),
                                            SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                            WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                            PayRateCurrency = eachCsRate.Currency,
                                            ChargeRateCurrency = chargeScheduleCurrency, //eachCsRate.Currency,
                                            PayRateDescription = eachCsRate.Description,
                                            ChargeRate = 0,
                                            ChargeTotalUnit = 0,
                                            PayTotalUnit = 0,
                                            PayRate = 0,
                                            InvoicingStatus = "N"  //To be taken from Invoice Status enum when finance module comes into place
                                        });
                                }

                                //Travel
                                if (eachCsRate.Type == "T" && (timesheetTechnicalSpecialistTravels == null || !(timesheetTechnicalSpecialistTravels.Exists(x =>
                                         x.ChargeExpenseTypeId == eachCsRate.ExpenseTypeId &&
                                         x.TimesheetTechnicalSpecialistId == techSpecId))))
                                {
                                    if (timesheetTechnicalSpecialistTravels == null)
                                        timesheetTechnicalSpecialistTravels = new List<DbModel.TimesheetTechnicalSpecialistAccountItemTravel>();
                                    timesheetTechnicalSpecialistTravels.Add(
                                        new DbModel.TimesheetTechnicalSpecialistAccountItemTravel
                                        {
                                            AssignmentId = assignmentDbCollection.dbTimesheet.AssignmentId,
                                            TimesheetId = assignmentDbCollection.dbTimesheet.Id,
                                            ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                            ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                            TimesheetTechnicalSpecialistId = (long)techSpecId,
                                            ExpenceDate = assignmentDetail.AssignmentInfo.TimesheetFromDate.GetValueOrDefault(),
                                            ChargeExpenseTypeId = eachCsRate.ExpenseTypeId.GetValueOrDefault(),
                                            SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                            WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                            ChargeRateCurrency = chargeScheduleCurrency,//eachCsRate.Currency,
                                            PayRateCurrency = eachCsRate.Currency,
                                            ChargeTotalUnit = 0,
                                            PayTotalUnit = 0,
                                            ChargeRate = 0,
                                            PayRate = 0,
                                            PayExpenseTypeId = eachCsRate.ExpenseTypeId.GetValueOrDefault(),
                                            InvoicingStatus = "N"  //To be taken from Invoice Status enum when finance module comes into place
                                        });
                                }

                                //Expense
                                if (eachCsRate.Type == "E" && (timesheetTechnicalSpecialistExpenses == null || !(timesheetTechnicalSpecialistExpenses.Exists(x =>
                                        x.ExpenseChargeTypeId == eachCsRate.ExpenseTypeId &&
                                        x.TimesheetTechnicalSpeciallistId == techSpecId))))
                                {
                                    if (timesheetTechnicalSpecialistExpenses == null)
                                        timesheetTechnicalSpecialistExpenses = new List<DbModel.TimesheetTechnicalSpecialistAccountItemExpense>();
                                    timesheetTechnicalSpecialistExpenses.Add(
                                        new DbModel.TimesheetTechnicalSpecialistAccountItemExpense
                                        {
                                            AssignmentId = assignmentDbCollection.dbTimesheet.AssignmentId,
                                            TimesheetId = assignmentDbCollection.dbTimesheet.Id,
                                            ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                            ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                            TimesheetTechnicalSpeciallistId = (long)techSpecId,
                                            ExpenseDate = assignmentDetail.AssignmentInfo.TimesheetFromDate.GetValueOrDefault(),
                                            ExpenseChargeTypeId = eachCsRate.ExpenseTypeId.GetValueOrDefault(),
                                            SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                            WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                            ChargeRateCurrency = chargeScheduleCurrency,//eachCsRate.Currency,
                                            PayRateCurrency = eachCsRate.Currency,
                                            ExpenceCurrency = expenseCurrency,//chargeScheduleCurrency,  //eachCsRate.Currency,
                                                                              //ChargeExchangeRate = !string.IsNullOrEmpty(eachCsRate.Currency) ? Convert.ToDecimal(String.Format("{0:0.000000}", 1)) : Convert.ToDecimal(String.Format("{0:0.000000}", 0)),
                                                                              //PayExchangeRate = !string.IsNullOrEmpty(eachCsRate.Currency) ? Convert.ToDecimal(String.Format("{0:0.000000}", 1)) : Convert.ToDecimal(String.Format("{0:0.000000}", 0)),

                                            ChargeExchangeRate = chargeExchangeRate,
                                            PayExchangeRate = payExchangeRate,
                                            ChargeTotalUnit = 0,
                                            PayTotalUnit = 0,
                                            ChargeRate = 0,
                                            PayRate = 0,
                                            PayRateTax = 0,
                                            InvoicingStatus = "N"  //To be taken from Invoice Status enum when finance module comes into place
                                        });
                                }

                                //Consumable
                                if ((eachCsRate.Type == "C" || eachCsRate.Type == "Q") && (timesheetTechnicalSpecialistConsumables == null || !(timesheetTechnicalSpecialistConsumables.Exists(x =>
                                          x.ChargeExpenseTypeId == eachCsRate.ExpenseTypeId &&
                                          x.TimesheetTechnicalSpecialistId == techSpecId))))
                                {
                                    if (timesheetTechnicalSpecialistConsumables == null)
                                        timesheetTechnicalSpecialistConsumables = new List<DbModel.TimesheetTechnicalSpecialistAccountItemConsumable>();
                                    timesheetTechnicalSpecialistConsumables.Add(
                                        new DbModel.TimesheetTechnicalSpecialistAccountItemConsumable()
                                        {
                                            AssignmentId = assignmentDbCollection.dbTimesheet.AssignmentId,
                                            TimesheetId = assignmentDbCollection.dbTimesheet.Id,
                                            ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                            ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                            TimesheetTechnicalSpecialistId = (long)techSpecId,
                                            ExpenceDate = assignmentDetail.AssignmentInfo.TimesheetFromDate.GetValueOrDefault(),
                                            ChargeExpenseTypeId = eachCsRate.ExpenseTypeId.GetValueOrDefault(),
                                            SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                            WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                            ChargeRateCurrency = chargeScheduleCurrency,//eachCsRate.Currency,
                                            PayRateCurrency = eachCsRate.Currency,
                                            PayRateDescription = eachCsRate.Description,
                                            ChargeTotalUnit = 0,
                                            PayTotalUnit = 0,
                                            ChargeRate = 0,
                                            PayRate = 0,
                                            PayExpenseTypeId = eachCsRate.ExpenseTypeId.GetValueOrDefault(),
                                            InvoicingStatus = "N"  //To be taken from Invoice Status enum when finance module comes into place
                                        });

                                }

                            }
                        }
                    }
                }
                tranScope.Complete();
            }
        }

        private void AddSkeletonVisitLineItems(DomainModel.AssignmentDetail assignmentDetail, DomainModel.AssignmentModuleDatabaseCollection assignmentDatabaseCollection)
        {
            List<DbModel.VisitTechnicalSpecialistAccountItemConsumable> dbVistTechspecCons = null;
            List<DbModel.VisitTechnicalSpecialistAccountItemExpense> dbVistTechspecExpense = null;
            List<DbModel.VisitTechnicalSpecialistAccountItemTime> dbVistTechspecTime = null;
            List<DbModel.VisitTechnicalSpecialistAccountItemTravel> dbVistTechspecTravel = null;
            try
            {
                var includes = new string[]
                {
                    "AssignmentTechnicalSpecialist",
                    "ContractChargeSchedule.ContractRate",
                    "TechnicalSpecialistPaySchedule.TechnicalSpecialistPayRate",
                };

                var assignTechSpecRateSchedules = this._assignmentTechnicalSpecialistScheduleRepository.GetAssignmentTechSpecRateSchedules(assignmentDatabaseCollection.dbVisit.AssignmentId, assignmentDatabaseCollection.dbLineItemExpense, includes);
                var vstTs = assignmentDatabaseCollection.dbAddedVisitTS?.Select(x => x.TechnicalSpecialistId).ToList();
                if (assignTechSpecRateSchedules != null)
                {
                    string expenseCurrency = assignmentDatabaseCollection.Assignment?.DBCompanies?.FirstOrDefault(x => x.Code == assignmentDetail.AssignmentInfo?.AssignmentOperatingCompanyCode)?.NativeCurrency;
                    var chargeCurrency = assignTechSpecRateSchedules.ChargeSchedules?.Select(x => x.ChargeScheduleCurrency)?.ToList();
                    var payCurrency = assignTechSpecRateSchedules.PaySchedules?.Select(x => x.PayScheduleCurrency)?.ToList();
                    var currencies = payCurrency != null && chargeCurrency != null ? chargeCurrency.Union(payCurrency) : chargeCurrency ?? payCurrency;
                    IList<ExchangeRate> exchangeCurrencyRate = null;
                    if (currencies?.Any() == true)
                    {
                        exchangeCurrencyRate = currencies.Select(x => new ExchangeRate
                        {
                            CurrencyFrom = expenseCurrency,
                            CurrencyTo = x,
                            EffectiveDate = assignmentDetail.AssignmentInfo.VisitFromDate.GetValueOrDefault()
                        })?.ToList();
                    }

                    var exchangeRate = GetExpenseLineItemChargeExchangeRates(exchangeCurrencyRate, assignmentDatabaseCollection.Assignment.DBProjects.FirstOrDefault().ContractId);
                    if (assignTechSpecRateSchedules.ChargeSchedules != null && assignTechSpecRateSchedules.ChargeSchedules.Count > 0)
                    {
                        if (vstTs?.Count > 0)
                        {
                            var filteredTsChargeSchedules = assignTechSpecRateSchedules.ChargeSchedules.Where(ts => vstTs.Contains(ts.Epin ?? 0))?.Select(x => x).ToList();
                            if (filteredTsChargeSchedules?.Count > 0)
                                ProcessSkeletonVisitChargeLineItems(filteredTsChargeSchedules, assignTechSpecRateSchedules, assignmentDetail, assignmentDatabaseCollection, ref dbVistTechspecTime, ref dbVistTechspecTravel, ref dbVistTechspecExpense, ref dbVistTechspecCons, expenseCurrency, exchangeRate);
                        }
                    }
                    if (assignTechSpecRateSchedules.PaySchedules != null && assignTechSpecRateSchedules.PaySchedules.Count > 0)
                    {
                        if (vstTs?.Count > 0)
                        {
                            var filteredTsPaySchedules = assignTechSpecRateSchedules.PaySchedules.Where(ts => vstTs.Contains(ts.Epin ?? 0))?.Select(x => x).ToList();
                            if (filteredTsPaySchedules?.Count > 0)
                                ProcessSkeletonVisitPayLineItems(filteredTsPaySchedules, assignTechSpecRateSchedules, assignmentDetail, assignmentDatabaseCollection, ref dbVistTechspecTime, ref dbVistTechspecTravel, ref dbVistTechspecExpense, ref dbVistTechspecCons, expenseCurrency, exchangeRate);
                        }
                    }

                    if (dbVistTechspecTime?.Count > 0)
                    {
                        _visitAccountItemTimeRepository.AutoSave = false;
                        _visitAccountItemTimeRepository.Add(dbVistTechspecTime);
                        _visitAccountItemTimeRepository.ForceSave();
                    }

                    if (dbVistTechspecTravel?.Count > 0)
                    {
                        _visitAccountItemTravelRepository.AutoSave = false;
                        _visitAccountItemTravelRepository.Add(dbVistTechspecTravel);
                        _visitAccountItemTravelRepository.ForceSave();
                    }

                    if (dbVistTechspecExpense?.Count > 0)
                    {
                        _visitAccountItemExpenseRepository.AutoSave = false;
                        _visitAccountItemExpenseRepository.Add(dbVistTechspecExpense);
                        _visitAccountItemExpenseRepository.ForceSave();
                    }

                    if (dbVistTechspecCons?.Count > 0)
                    {
                        _visitAccountItemConsumableRepository.AutoSave = false;
                        _visitAccountItemConsumableRepository.Add(dbVistTechspecCons);
                        _visitAccountItemConsumableRepository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            finally
            {
                _visitAccountItemTimeRepository.AutoSave = true;
                _visitAccountItemTravelRepository.AutoSave = true;
                _visitAccountItemExpenseRepository.AutoSave = true;
                _visitAccountItemConsumableRepository.AutoSave = true;
            }
        }

        private void ProcessSkeletonVisitChargeLineItems(IList<DomainModel.TechnicalSpecialistChargeSchedule> chargeSchedules, DomainModel.AssignmentTechSpecSchedules rateSchedules, DomainModel.AssignmentDetail assignmentDetail, DomainModel.AssignmentModuleDatabaseCollection assignmentDbCollection,
                        ref List<DbModel.VisitTechnicalSpecialistAccountItemTime> visitTechnicalSpecialistTimes, ref List<DbModel.VisitTechnicalSpecialistAccountItemTravel> visitTechnicalSpecialistTravels,
                        ref List<DbModel.VisitTechnicalSpecialistAccountItemExpense> visitTechnicalSpecialistExpenses, ref List<DbModel.VisitTechnicalSpecialistAccountItemConsumable> visitTechnicalSpecialistConsumables,
                        string expenseCurrency, Response exchangeRate)
        {
            var payScheduleCurrency = string.Empty;
            using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                for (int i = 0, len = chargeSchedules.Count; i < len; i++)
                {
                    DomainModel.TechnicalSpecialistChargeSchedule eachChargeSchedule = chargeSchedules[i];
                    if (eachChargeSchedule != null && eachChargeSchedule.ChargeScheduleRates != null)
                    {
                        for (int j = 0, ratesLen = eachChargeSchedule.ChargeScheduleRates.Count; j < ratesLen; j++)
                        {
                            var techSpecId = assignmentDbCollection.dbAddedVisitTS?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == eachChargeSchedule.TechnicalSpecialistId)?.Id;
                            payScheduleCurrency = rateSchedules.PaySchedules?.FirstOrDefault(x => x.TechnicalSpecialistId == eachChargeSchedule.TechnicalSpecialistId)?.PayScheduleCurrency;  //D-1311
                            DomainModel.ChargeScheduleRates eachCsRate = eachChargeSchedule.ChargeScheduleRates[j];
                            var payExchangeRate = !string.IsNullOrEmpty(expenseCurrency) && !string.IsNullOrEmpty(payScheduleCurrency) && expenseCurrency == payScheduleCurrency ?
                                                    Convert.ToDecimal(String.Format("{0:0.000000}", 1)) :
                                                    exchangeRate.Result?.Populate<List<ExchangeRate>>()?.FirstOrDefault(x => x.CurrencyFrom == expenseCurrency && x.CurrencyTo == payScheduleCurrency)?.Rate ?? Convert.ToDecimal(String.Format("{0:0.000000}", 0));

                            var chargeExchangeRate = !string.IsNullOrEmpty(expenseCurrency) && !string.IsNullOrEmpty(eachCsRate.Currency) && expenseCurrency == eachCsRate.Currency ?
                                                   Convert.ToDecimal(String.Format("{0:0.000000}", 1)) :
                                                   exchangeRate.Result?.Populate<List<ExchangeRate>>()?.FirstOrDefault(x => x.CurrencyFrom == expenseCurrency && x.CurrencyTo == eachCsRate.Currency)?.Rate ?? Convert.ToDecimal(String.Format("{0:0.000000}", 0));

                            if ((eachCsRate.EffectiveTo == null || eachCsRate.EffectiveTo >= assignmentDetail.AssignmentInfo.VisitFromDate) && eachCsRate.IsActive == true)
                            {
                                //Time
                                if (eachCsRate.Type == "R" && (visitTechnicalSpecialistTimes == null || !(visitTechnicalSpecialistTimes.Exists(x =>
                                        x.ExpenseChargeTypeId == eachCsRate.ChargeTypeId && x.VisitTechnicalSpeciallistId == assignmentDbCollection.dbAddedVisitTS?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == eachChargeSchedule.TechnicalSpecialistId)?.Id
                                        ))))
                                {
                                    if (visitTechnicalSpecialistTimes == null)
                                        visitTechnicalSpecialistTimes = new List<DbModel.VisitTechnicalSpecialistAccountItemTime>();
                                    visitTechnicalSpecialistTimes.Add(new DbModel.VisitTechnicalSpecialistAccountItemTime
                                    {
                                        AssignmentId = assignmentDbCollection.dbVisit.AssignmentId,
                                        VisitId = assignmentDbCollection.dbVisit.Id,
                                        ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                        ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                        VisitTechnicalSpeciallistId =
                                            (long)techSpecId,
                                        ExpenseDate = assignmentDetail.AssignmentInfo.VisitFromDate.GetValueOrDefault(),
                                        ExpenseChargeTypeId = eachCsRate.ChargeTypeId.GetValueOrDefault(),
                                        ChargeRateCurrency = eachCsRate.Currency,
                                        PayRateCurrency = payScheduleCurrency,//eachCsRate.Currency,
                                        ChargeRateDescription = eachCsRate.Description,
                                        SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                        WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                        ChargeRate = 0,
                                        ChargeTotalUnit = 0,
                                        PayTotalUnit = 0,
                                        PayRate = 0,
                                        InvoicingStatus = "N" //To be taken from Invoice Status enum when finance module comes into place

                                    });
                                    continue;
                                }

                                //Travel
                                if (eachCsRate.Type == "T" && (visitTechnicalSpecialistTravels == null || !(visitTechnicalSpecialistTravels.Exists(x =>
                                        x.ChargeExpenseTypeId == eachCsRate.ChargeTypeId &&
                                        x.VisitTechnicalSpecialistId == assignmentDbCollection.dbAddedVisitTS?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == eachChargeSchedule.TechnicalSpecialistId)?.Id))))
                                {
                                    if (visitTechnicalSpecialistTravels == null)
                                        visitTechnicalSpecialistTravels = new List<DbModel.VisitTechnicalSpecialistAccountItemTravel>();
                                    visitTechnicalSpecialistTravels.Add(
                                        new DbModel.VisitTechnicalSpecialistAccountItemTravel
                                        {
                                            AssignmentId = assignmentDbCollection.dbVisit.AssignmentId,
                                            VisitId = assignmentDbCollection.dbVisit.Id,
                                            ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                            ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                            VisitTechnicalSpecialistId =
                                                (long)techSpecId,
                                            ExpenceDate = assignmentDetail.AssignmentInfo.VisitFromDate
                                                .GetValueOrDefault(),
                                            ChargeExpenseTypeId = eachCsRate.ChargeTypeId.GetValueOrDefault(),
                                            ChargeRateCurrency = eachCsRate.Currency,
                                            PayRateCurrency = payScheduleCurrency,//eachCsRate.Currency,
                                            SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                            WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                            ChargeTotalUnit = 0,
                                            PayTotalUnit = 0,
                                            ChargeRate = 0,
                                            PayRate = 0,
                                            PayExpenseTypeId = eachCsRate.ChargeTypeId.GetValueOrDefault(),
                                            InvoicingStatus = "N"  //To be taken from Invoice Status enum when finance module comes into place

                                        });

                                    continue;
                                }

                                //Expense
                                if (eachCsRate.Type == "E" && (visitTechnicalSpecialistExpenses == null || !(visitTechnicalSpecialistExpenses.Exists(x =>
                                        x.ExpenseChargeTypeId == eachCsRate.ChargeTypeId && x.VisitTechnicalSpeciallistId == assignmentDbCollection.dbAddedVisitTS?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == eachChargeSchedule.TechnicalSpecialistId)?.Id
                                        ))))
                                {
                                    if (visitTechnicalSpecialistExpenses == null)
                                        visitTechnicalSpecialistExpenses = new List<DbModel.VisitTechnicalSpecialistAccountItemExpense>();
                                    visitTechnicalSpecialistExpenses.Add(new DbModel.VisitTechnicalSpecialistAccountItemExpense()
                                    {
                                        AssignmentId = assignmentDbCollection.dbVisit.AssignmentId,
                                        VisitId = assignmentDbCollection.dbVisit.Id,
                                        ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                        ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                        VisitTechnicalSpeciallistId =
                                                (long)techSpecId,
                                        ExpenseDate = assignmentDetail.AssignmentInfo.VisitFromDate
                                                .GetValueOrDefault(),
                                        ExpenseChargeTypeId = eachCsRate.ChargeTypeId.GetValueOrDefault(),
                                        SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                        WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                        ChargeRateCurrency = eachCsRate.Currency,
                                        PayRateCurrency = payScheduleCurrency,//eachCsRate.Currency,
                                        ExpenceCurrency = expenseCurrency,//payScheduleCurrency,//eachCsRate.Currency,
                                        //ChargeExchangeRate = !string.IsNullOrEmpty(eachCsRate.Currency) ? Convert.ToDecimal(String.Format("{0:0.000000}", 1)) : Convert.ToDecimal(String.Format("{0:0.000000}", 0)),
                                        //PayExchangeRate = !string.IsNullOrEmpty(eachCsRate.Currency) ? Convert.ToDecimal(String.Format("{0:0.000000}", 1)) : Convert.ToDecimal(String.Format("{0:0.000000}", 0)),
                                        ChargeExchangeRate = chargeExchangeRate,
                                        PayExchangeRate = payExchangeRate, //To address Vsiit Defect
                                        ChargeTotalUnit = 0,
                                        PayTotalUnit = 0,
                                        ChargeRate = 0,
                                        PayRate = 0,
                                        PayRateTax = 0,
                                        InvoicingStatus = "N"  //To be taken from Invoice Status enum when finance module comes into place
                                    });
                                    continue;
                                }

                                //Consumable
                                if ((eachCsRate.Type == "C" || eachCsRate.Type == "Q") && (visitTechnicalSpecialistConsumables == null || !(visitTechnicalSpecialistConsumables.Exists(x =>
                                        x.ChargeExpenseTypeId == eachCsRate.ChargeTypeId && x.VisitTechnicalSpecialistId == assignmentDbCollection.dbAddedVisitTS?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == eachChargeSchedule.TechnicalSpecialistId)?.Id
                                       ))))
                                {
                                    if (visitTechnicalSpecialistConsumables == null)
                                        visitTechnicalSpecialistConsumables = new List<DbModel.VisitTechnicalSpecialistAccountItemConsumable>();
                                    visitTechnicalSpecialistConsumables.Add(
                                        new DbModel.VisitTechnicalSpecialistAccountItemConsumable()
                                        {
                                            AssignmentId = assignmentDbCollection.dbVisit.AssignmentId,
                                            VisitId = assignmentDbCollection.dbVisit.Id,
                                            ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                            ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                            VisitTechnicalSpecialistId =
                                                (long)techSpecId,
                                            ExpenceDate = assignmentDetail.AssignmentInfo.VisitFromDate
                                                .GetValueOrDefault(),
                                            ChargeExpenseTypeId = eachCsRate.ChargeTypeId.GetValueOrDefault(),
                                            SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                            WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                            ChargeRateCurrency = eachCsRate.Currency,
                                            PayRateCurrency = payScheduleCurrency,//eachCsRate.Currency,
                                            ChargeDescription = eachCsRate.Description,
                                            ChargeTotalUnit = 0,
                                            PayTotalUnit = 0,
                                            ChargeRate = 0,
                                            PayRate = 0,
                                            PayExpenseTypeId = eachCsRate.ChargeTypeId.GetValueOrDefault(),
                                            InvoicingStatus = "N"  //To be taken from Invoice Status enum when finance module comes into place                                            
                                        });
                                }
                            }
                        }
                    }
                }
                tranScope.Complete();
            }
        }

        private void ProcessSkeletonVisitPayLineItems(IList<DomainModel.TechnicalSpecialistPaySchedule> paySchedules, DomainModel.AssignmentTechSpecSchedules rateSchedules, DomainModel.AssignmentDetail assignmentDetail, DomainModel.AssignmentModuleDatabaseCollection assignmentDbCollection,
                ref List<DbModel.VisitTechnicalSpecialistAccountItemTime> visitTechnicalSpecialistTimes, ref List<DbModel.VisitTechnicalSpecialistAccountItemTravel> visitTechnicalSpecialistTravels,
                ref List<DbModel.VisitTechnicalSpecialistAccountItemExpense> visitTechnicalSpecialistExpenses, ref List<DbModel.VisitTechnicalSpecialistAccountItemConsumable> visitTechnicalSpecialistConsumables,
                string expenseCurrency, Response exchangeRate)
        {
            string chargeScheduleCurrency = string.Empty;
            using (var tranScope = new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                for (int i = 0, len = paySchedules.Count; i < len; i++)
                {
                    DomainModel.TechnicalSpecialistPaySchedule eachPaySchedule = paySchedules[i];
                    if (eachPaySchedule != null && eachPaySchedule.PayScheduleRates != null)
                    {
                        for (int j = 0, ratesLen = eachPaySchedule.PayScheduleRates.Count; j < ratesLen; j++)
                        {
                            var techSpecId = assignmentDbCollection.dbAddedVisitTS?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == eachPaySchedule.TechnicalSpecialistId)?.Id;
                            chargeScheduleCurrency = rateSchedules.ChargeSchedules?.FirstOrDefault(x => x.TechnicalSpecialistId == eachPaySchedule.TechnicalSpecialistId)?.ChargeScheduleCurrency;  //D-1311
                            DomainModel.PayScheduleRates eachCsRate = eachPaySchedule.PayScheduleRates[j];

                            var payExchangeRate = !string.IsNullOrEmpty(expenseCurrency) && !string.IsNullOrEmpty(eachCsRate.Currency) && expenseCurrency == eachCsRate.Currency ?
                                                  Convert.ToDecimal(String.Format("{0:0.000000}", 1)) :
                                                  exchangeRate.Result?.Populate<List<ExchangeRate>>()?.FirstOrDefault(x => x.CurrencyFrom == expenseCurrency && x.CurrencyTo == eachCsRate.Currency)?.Rate ?? Convert.ToDecimal(String.Format("{0:0.000000}", 0));

                            var chargeExchangeRate = !string.IsNullOrEmpty(expenseCurrency) && !string.IsNullOrEmpty(chargeScheduleCurrency) && expenseCurrency == chargeScheduleCurrency ?
                                                   Convert.ToDecimal(String.Format("{0:0.000000}", 1)) :
                                                   exchangeRate.Result?.Populate<List<ExchangeRate>>()?.FirstOrDefault(x => x.CurrencyFrom == expenseCurrency && x.CurrencyTo == chargeScheduleCurrency)?.Rate ?? Convert.ToDecimal(String.Format("{0:0.000000}", 0));

                            if ((eachCsRate.EffectiveTo == null ||
                                eachCsRate.EffectiveTo >= assignmentDetail.AssignmentInfo.VisitFromDate) && eachCsRate.IsActive == true)
                            {
                                //Time
                                if (eachCsRate.Type == "R" && (visitTechnicalSpecialistTimes == null || !(visitTechnicalSpecialistTimes.Exists(x =>
                                         x.ExpenseChargeTypeId == eachCsRate.ExpenseTypeId &&
                                         x.VisitTechnicalSpeciallistId == assignmentDbCollection.dbAddedVisitTS?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == eachPaySchedule.TechnicalSpecialistId)?.Id))))
                                {
                                    if (visitTechnicalSpecialistTimes == null)
                                        visitTechnicalSpecialistTimes = new List<DbModel.VisitTechnicalSpecialistAccountItemTime>();
                                    visitTechnicalSpecialistTimes.Add(new DbModel.VisitTechnicalSpecialistAccountItemTime()
                                    {
                                        AssignmentId = assignmentDbCollection.dbVisit.AssignmentId,
                                        VisitId = assignmentDbCollection.dbVisit.Id,
                                        ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                        ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                        VisitTechnicalSpeciallistId = (long)techSpecId,
                                        ExpenseDate = assignmentDetail.AssignmentInfo.VisitFromDate.GetValueOrDefault(),
                                        ExpenseChargeTypeId = eachCsRate.ExpenseTypeId.GetValueOrDefault(),
                                        SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                        WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                        ChargeRateCurrency = chargeScheduleCurrency,//eachCsRate.Currency,
                                        PayRateDescription = eachCsRate.Description,
                                        PayRateCurrency = eachCsRate.Currency,
                                        ChargeRate = 0,
                                        ChargeTotalUnit = 0,
                                        PayTotalUnit = 0,
                                        PayRate = 0,
                                        InvoicingStatus = "N"  //To be taken from Invoice Status enum when finance module comes into place
                                    });
                                    continue;
                                }

                                //Travel
                                if (eachCsRate.Type == "T" && (visitTechnicalSpecialistTravels == null || !(visitTechnicalSpecialistTravels.Exists(x =>
                                        x.ChargeExpenseTypeId == eachCsRate.ExpenseTypeId && x.VisitTechnicalSpecialistId ==
                                        assignmentDbCollection.dbAddedVisitTS?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == eachPaySchedule.TechnicalSpecialistId)?.Id))))
                                {
                                    if (visitTechnicalSpecialistTravels == null)
                                        visitTechnicalSpecialistTravels = new List<DbModel.VisitTechnicalSpecialistAccountItemTravel>();
                                    visitTechnicalSpecialistTravels.Add(
                                        new DbModel.VisitTechnicalSpecialistAccountItemTravel
                                        {
                                            AssignmentId = assignmentDbCollection.dbVisit.AssignmentId,
                                            VisitId = assignmentDbCollection.dbVisit.Id,
                                            ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                            ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                            VisitTechnicalSpecialistId =
                                                (long)techSpecId,
                                            ExpenceDate = assignmentDetail.AssignmentInfo.VisitFromDate
                                                .GetValueOrDefault(),
                                            ChargeExpenseTypeId = eachCsRate.ExpenseTypeId.GetValueOrDefault(),
                                            SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                            WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                            ChargeRateCurrency = chargeScheduleCurrency,//eachCsRate.Currency,
                                            PayRateCurrency = eachCsRate.Currency,
                                            ChargeTotalUnit = 0,
                                            PayTotalUnit = 0,
                                            ChargeRate = 0,
                                            PayRate = 0,
                                            PayExpenseTypeId = eachCsRate.ExpenseTypeId.GetValueOrDefault(),
                                            InvoicingStatus = "N"  //To be taken from Invoice Status enum when finance module comes into place
                                        });
                                    continue;
                                }

                                //Expense
                                if (eachCsRate.Type == "E" && (visitTechnicalSpecialistExpenses == null || !(visitTechnicalSpecialistExpenses.Exists(x =>
                                        x.ExpenseChargeTypeId == eachCsRate.ExpenseTypeId &&
                                        x.VisitTechnicalSpeciallistId == assignmentDbCollection.dbAddedVisitTS?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == eachPaySchedule.TechnicalSpecialistId)?.Id))))
                                {
                                    if (visitTechnicalSpecialistExpenses == null)
                                        visitTechnicalSpecialistExpenses = new List<DbModel.VisitTechnicalSpecialistAccountItemExpense>();
                                    visitTechnicalSpecialistExpenses.Add(
                                        new DbModel.VisitTechnicalSpecialistAccountItemExpense
                                        {
                                            AssignmentId = assignmentDbCollection.dbVisit.AssignmentId,
                                            VisitId = assignmentDbCollection.dbVisit.Id,
                                            ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                            ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                            VisitTechnicalSpeciallistId = (long)techSpecId,
                                            ExpenseDate = assignmentDetail.AssignmentInfo.VisitFromDate.GetValueOrDefault(),
                                            ExpenseChargeTypeId = eachCsRate.ExpenseTypeId.GetValueOrDefault(),
                                            SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                            WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                            ChargeRateCurrency = chargeScheduleCurrency,//eachCsRate.Currency,
                                            PayRateCurrency = eachCsRate.Currency,
                                            ExpenceCurrency = expenseCurrency,//chargeScheduleCurrency,//eachCsRate.Currency,
                                            //ChargeExchangeRate = !string.IsNullOrEmpty(eachCsRate.Currency) ? Convert.ToDecimal(String.Format("{0:0.000000}", 1)) : Convert.ToDecimal(String.Format("{0:0.000000}", 0)),
                                            //PayExchangeRate = !string.IsNullOrEmpty(eachCsRate.Currency) ? Convert.ToDecimal(String.Format("{0:0.000000}", 1)) : Convert.ToDecimal(String.Format("{0:0.000000}", 0)),
                                            ChargeExchangeRate = chargeExchangeRate,
                                            PayExchangeRate = payExchangeRate,
                                            ChargeTotalUnit = 0,
                                            PayTotalUnit = 0,
                                            ChargeRate = 0,
                                            PayRate = 0,
                                            PayRateTax = 0,
                                            InvoicingStatus = "N"  //To be taken from Invoice Status enum when finance module comes into place
                                        });
                                    continue;
                                }

                                //Consumable
                                if ((eachCsRate.Type == "C" || eachCsRate.Type == "Q") && (visitTechnicalSpecialistConsumables == null || !(visitTechnicalSpecialistConsumables.Exists(x =>
                                        x.ChargeExpenseTypeId == eachCsRate.ExpenseTypeId && x.VisitTechnicalSpecialistId ==
                                        assignmentDbCollection.dbAddedVisitTS?.FirstOrDefault(x1 => x1.TechnicalSpecialistId == eachPaySchedule.TechnicalSpecialistId)?.Id))))
                                {
                                    if (visitTechnicalSpecialistConsumables == null)
                                        visitTechnicalSpecialistConsumables = new List<DbModel.VisitTechnicalSpecialistAccountItemConsumable>();
                                    visitTechnicalSpecialistConsumables.Add(
                                    new DbModel.VisitTechnicalSpecialistAccountItemConsumable
                                    {
                                        AssignmentId = assignmentDbCollection.dbVisit.AssignmentId,
                                        VisitId = assignmentDbCollection.dbVisit.Id,
                                        ContractId = assignmentDbCollection.Assignment.DBContracts.FirstOrDefault().Id,
                                        ProjectId = assignmentDbCollection.DBAssignment.FirstOrDefault().ProjectId,
                                        VisitTechnicalSpecialistId = (long)techSpecId,
                                        ExpenceDate = assignmentDetail.AssignmentInfo.VisitFromDate.GetValueOrDefault(),
                                        ChargeExpenseTypeId = eachCsRate.ExpenseTypeId.GetValueOrDefault(),
                                        SalesTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceSalesTaxId,
                                        WithholdingTaxId = assignmentDbCollection.Assignment.DBProjects.FirstOrDefault().InvoiceWithholdingTaxId,
                                        ChargeRateCurrency = chargeScheduleCurrency,//eachCsRate.Currency,
                                        PayRateCurrency = eachCsRate.Currency,
                                        PayRateDescription = eachCsRate.Description,
                                        ChargeTotalUnit = 0,
                                        PayTotalUnit = 0,
                                        ChargeRate = 0,
                                        PayRate = 0,
                                        PayExpenseTypeId = eachCsRate.ExpenseTypeId.GetValueOrDefault(),
                                        InvoicingStatus = "N"  //To be taken from Invoice Status enum when finance module comes into place
                                    });
                                }
                            }
                        }
                    }
                }
                tranScope.Complete();
            }
        }


        public async Task<Response> Get(int assignmentId)
        {
            AssignmentAggregatorResponse aggregatorResponse = null;
            if (assignmentId > 0)
            {
                aggregatorResponse = new AssignmentAggregatorResponse
                {
                    //aggregatorResponse.AssignmentInfo = _assignmentRepository.SearchAssignments(new DomainModel.AssignmentSearch() {AssignmentId = assignmentId});
                    AssignmentContractSchedules = await Task.Run(() => _assignmentContractRateScheduleRepository.Search(new DomainModel.AssignmentContractRateSchedule() { AssignmentId = assignmentId })),
                    AssignmentReferences = await Task.Run(() => _assignmentReferenceRepository.Search(new DomainModel.AssignmentReferenceType() { AssignmentId = assignmentId })),
                    AssignmentTaxonomy = await Task.Run(() => _assignmentTaxonomyRepository.search(new DomainModel.AssignmentTaxonomy() { AssignmentId = assignmentId })),
                    AssignmentTechnicalSpecialists = await Task.Run(() => _assignmentTechnicalSpecialistRepository.Search(new DomainModel.AssignmentTechnicalSpecialist() { AssignmentId = assignmentId },
                                                                                                                                assign => assign.Assignment, techSpecialist => techSpecialist.TechnicalSpecialist)),
                    AssignmentSubSuppliers = await Task.Run(() => _assignmentSubSupplierRepository.Search(new DomainModel.AssignmentSubSupplier() { AssignmentId = assignmentId },
                                                                                                                        subSup => subSup.Supplier, supCon => supCon.SupplierContact, assign => assign.Assignment,
                                                                                                                        tech => tech.AssignmentSubSupplierTechnicalSpecialist)),//MS-TS Link scafold 
                    AssignmentAdditionalExpenses = await Task.Run(() => _assignmentAdditionalExpenseRepository.Search(new DomainModel.AssignmentAdditionalExpense() { AssignmentId = assignmentId })),
                    AssignmentInterCompanyDiscounts = await Task.Run(() => _assignmentInterCompanyDiscount.Search(assignmentId)),
                    AssignmentInstructions = await Task.Run(() => _assignmentMessageRepository.Search(assignmentId)),
                    AssignmentNotes = await Task.Run(() => _assignmentNoteRepository.Search(new DomainModel.AssignmentNote() { AssignmentId = assignmentId })),
                    AssignmentContributionCalculators = await Task.Run(() => _assignmentContributionCalculationRepository.Search(new DomainModel.AssignmentContributionCalculation() { AssignmentId = assignmentId })),
                    AssignmentDocuments = await Task.Run(() => _documentService.Get(new ModuleDocument() { ModuleRefCode = assignmentId.ToString(), ModuleCode = "ASGMNT" }))
                };
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, aggregatorResponse, null, null);
        }

        public Response GetAssignmentBudgetDetails(string companyCode = null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool isAssignmentOnly = true)
        {
            Exception exception = null;
            try
            {
                var contracts = _assignmentRepository.FindBy(x =>
                                         (string.IsNullOrEmpty(companyCode) || x.ContractCompany.Code == companyCode) &&
                                         (contractStatus == ContractStatus.All || x.Project.Contract.Status == contractStatus.FirstChar()) &&
                                         (x.BudgetValue > 0 || x.Project.Budget > 0 || x.Project.Contract.Budget > 0) &&
                                         ((isAssignmentOnly == false && (string.IsNullOrEmpty(userName) || x.Project.Coordinator.SamaccountName == userName)) ||
                                           (isAssignmentOnly == true && (string.IsNullOrEmpty(userName) || x.OperatingCompanyCoordinator.SamaccountName == userName) ||
                                            (string.IsNullOrEmpty(userName) || x.ContractCompanyCoordinator.SamaccountName == userName))
                                         )
                                         )
                                        .Select(x =>
                                        new
                                        {
                                            ContractId = x.Project.Contract.Id,
                                            ContractCustomerCode = x.Project.Contract.Customer.Code,
                                            ContractCustomerName = x.Project.Contract.Customer.Name,
                                            BudgetValue = x.Project.Contract.Budget,
                                            x.Project.Contract.CustomerContractNumber,
                                            x.Project.Contract.BudgetCurrency,
                                            x.BudgetWarning,
                                            x.BudgetHours,
                                            x.BudgetHoursWarning,
                                        }).Distinct().ToList();

                var contractIds = contracts?.Select(x => x.ContractId).Distinct().ToList();

                var projectInvoiceInfo = _contractRepository
                                              .GetBudgetAccountItemDetails(companyCode,
                                                                              contractIds,
                                                                              userName,
                                                                              contractStatus,
                                                                              isAssignmentOnly);

                return GetAssignmentBudgetDetails(projectInvoiceInfo);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response GetAssignmentBudgetDetails(IList<BudgetAccountItem> budgetAccountItems, IList<Common.Models.ExchangeRate.ContractExchangeRate> contractExchangeRates = null, IList<ExchangeRate> currencyExchangeRates = null)
        {
            IList<Budget> result = null;
            Exception exception = null;
            try
            {
                if (budgetAccountItems?.Count > 0)
                {
                    var contractIds = budgetAccountItems.Select(x => x.ContractId).Distinct().ToList();
                    var assignments = _assignmentRepository.FindBy(x => contractIds.Contains(x.Project.ContractId))
                                        .Select(x =>
                                        new
                                        {
                                            ContractId = x.Project.Contract.Id,
                                            AssignmentId = x.Id,
                                            ContractCustomerCode = x.Project.Contract.Customer.Code,
                                            ContractCustomerName = x.Project.Contract.Customer.Name,
                                            BudgetValue = x.BudgetValue ?? 0,
                                            x.Project.Contract.CustomerContractNumber,
                                            x.Project.Contract.BudgetCurrency,
                                            x.BudgetWarning,
                                            x.BudgetHours,
                                            x.BudgetHoursWarning,
                                        }).Distinct().ToList();

                    var assignmentInvoiceInfo = PopulateAssignmentInvoiceInfo(budgetAccountItems, contractExchangeRates, currencyExchangeRates).Result
                                                                                               .Populate<List<InvoicedInfo>>();
                    result = _mapper.Map<List<InvoicedInfo>, List<Budget>>(assignmentInvoiceInfo);

                    result.ToList().ForEach(x =>
                    {
                        var assignment = assignments?.FirstOrDefault(x1 => x1.AssignmentId == x.AssignmentId);
                        x.ContractCustomerCode = assignment.ContractCustomerCode;
                        x.ContractCustomerName = assignment.ContractCustomerName;
                        x.BudgetValue = assignment.BudgetValue;
                        x.CustomerContractNumber = assignment.CustomerContractNumber;
                        x.BudgetCurrency = assignment.BudgetCurrency;
                        x.BudgetWarning = assignment.BudgetWarning;
                        x.BudgetHours = assignment.BudgetHours;
                        x.BudgetHoursWarning = assignment.BudgetHoursWarning;
                    }
                    );
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }

        public Response GetAssignmentInvoiceInfo(string companyCode = null, List<int> contractIds = null, string userName = null, ContractStatus contractStatus = ContractStatus.All, bool showMyAssignmentsOnly = true)
        {
            Exception exception = null;
            try
            {
                var assignmetBudgetAccountItems = _contractRepository.GetBudgetAccountItemDetails(companyCode, contractIds, userName, contractStatus, showMyAssignmentsOnly);
                return PopulateAssignmentInvoiceInfo(assignmetBudgetAccountItems);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        public Response GetAssignmentInvoiceInfo(List<string> contractNumber = null, List<int> projectNumber = null, List<int> assignmentNumber = null)
        {
            List<int> contractIds = null;
            Exception exception = null;
            try
            {
                contractIds = new List<int>();
                if (contractNumber != null)
                {
                    contractIds.AddRange(_contractRepository.FindBy(x => contractNumber.Contains(x.ContractNumber)).Select(x => x.Id).ToList());
                }
                if (projectNumber != null)
                {
                    contractIds.AddRange(_projectRepository.FindBy(x => x.ProjectNumber != null && projectNumber.Contains(x.ProjectNumber.Value)).Select(x => x.ContractId).Distinct().ToList());
                }
                if (assignmentNumber != null)
                {
                    contractIds.AddRange(_assignmentRepository.FindBy(x => assignmentNumber.Contains((int)x.AssignmentNumber)).Select(x => x.Project.Contract.Id).Distinct().ToList());
                }

                contractIds = contractIds.Distinct().ToList();

                var assignmetBudgetAccountItems = _contractRepository.GetBudgetAccountItemDetails(contractIds: contractIds);

                return PopulateAssignmentInvoiceInfo(assignmetBudgetAccountItems);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), contractNumber);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private Response PopulateAssignmentInvoiceInfo(IList<BudgetAccountItem> budgetAccountItems, IList<ContractExchangeRate> contractExchangeRates = null, IList<ExchangeRate> currencyExchangeRates = null)
        {
            IList<InvoicedInfo> result = null;
            Exception exception = null;
            try
            {
                if (budgetAccountItems?.Count > 0)
                {
                    var distinctContractIds = budgetAccountItems.Select(x => x.ContractId).Distinct().ToList();

                    if (contractExchangeRates == null)
                        contractExchangeRates = _exchangeRateRepository.GetContractExchangeRates(distinctContractIds)
                                                                            .Populate<IList<Common.Models.ExchangeRate.ContractExchangeRate>>();
                    if (currencyExchangeRates == null)
                    {
                        var contractWithoutExchangeRate = budgetAccountItems?
                                                            .Where(x => !contractExchangeRates
                                                                        .Any(x1 => x.ContractId == x1.ContractId &&
                                                                             x.ChargeRateCurrency == x1.CurrencyFrom &&
                                                                             x.BudgetCurrency == x1.CurrencyTo)).ToList();

                        var currencyWithoutExchangeRate = contractWithoutExchangeRate?
                                                        .Select(x => new ExchangeRate()
                                                        {
                                                            CurrencyFrom = x.ChargeRateCurrency,
                                                            CurrencyTo = x.BudgetCurrency,
                                                            EffectiveDate = x.VisitDate
                                                        }).ToList();

                        currencyExchangeRates = _currencyExchangeRateService
                                                   .GetExchangeRates(currencyWithoutExchangeRate)
                                                   .Result
                                                   .Populate<IList<ExchangeRate>>();
                    }


                    var masterExpenceTypeToHour = _assignmentRepository.GetMasterData(null, null, new List<int>() { (int)MasterType.ExpenseTypeHour }, null).Populate<List<MasterData>>();

                    ExchangeRateClaculations.CalculateExchangeRate(contractExchangeRates, currencyExchangeRates, budgetAccountItems);

                    result = budgetAccountItems?
                         .GroupBy(x => new { x.ContractId, x.ProjectId, x.AssignmentId })
                         .Select(x =>
                         new InvoicedInfo
                         {
                             ContractId = x.Key.ContractId,
                             ProjectId = x.Key.ProjectId,
                             AssignmentId = x.Key.AssignmentId,
                             InvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0 : 0)),
                             UninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : x1.ContractExchangeRate != 0 ? ((x1.ChargeRate * x1.ChargeTotalUnit * (x1.ExpenseType == "E" ? x1.ChargeExchangeRate : 1)) / x1.ContractExchangeRate) : 0)),
                             HoursInvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? (x1.ChargeTotalUnit * (masterExpenceTypeToHour.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)) : 0)),
                             HoursUninvoicedToDate = x.Sum(x1 => (x1.Status == "C" ? 0 : (x1.ChargeTotalUnit * (masterExpenceTypeToHour.FirstOrDefault(x2 => x2.Name == x1.ExpenseName)?.Hour ?? 0)))),
                             ContractNumber = x?.FirstOrDefault().ContractNumber,
                             ProjectNumber = x?.FirstOrDefault()?.ProjectNumber ?? 0,
                             AssignmentNumber = x?.FirstOrDefault()?.AssignmentNumber ?? 0,
                         }).ToList();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), budgetAccountItems);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        private Response AuditLog(DomainModel.AssignmentDetail assignmentDetails,
                                     DbModel.Assignment dbAssignment,
                                     DomainModel.AssignmentModuleDatabaseCollection assignmentModuleDatabaseCollection,
                                     DbModel.Project dbProject,
                                     SqlAuditActionType sqlAuditActionType,
                                     SqlAuditModuleType sqlAuditModuleType,
                                     object oldData,
                                     object newData,
                                     ref long? eventId,
                                     DomainModel.AssignmentDetail dbExsistanceAssignmentDetails,
                                     List<DbModel.Document> dbDocuments = null)
        {
            Exception exception = null;
            Response result = new Response();
            result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), "{" + AuditSelectType.Id + ":" + dbAssignment?.Id + "}${" + AuditSelectType.ProjectNumber + ":" + assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber + "}${" + AuditSelectType.ProjectAssignment + ":" + assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber + "-" + assignmentDetails?.AssignmentInfo?.AssignmentNumber + "}", sqlAuditActionType, SqlAuditModuleType.Assignment, oldData, newData, assignmentModuleDatabaseCollection.dbModule);
            if (assignmentDetails.AssignmentInfo.RecordStatus.IsRecordStatusNew())
            {
                this.ProcessAuditSave(assignmentDetails, dbAssignment, sqlAuditActionType, sqlAuditModuleType, oldData, newData, ref eventId, dbExsistanceAssignmentDetails, assignmentModuleDatabaseCollection.dbModule, assignmentModuleDatabaseCollection, dbDocuments);
                if (assignmentDetails.AssignmentInfo.AssignmentContractHoldingCompanyCode == assignmentDetails.AssignmentInfo.AssignmentOperatingCompanyCode)
                    result = this.ProcessVisitAuditSave(assignmentDetails, dbAssignment, dbProject, assignmentModuleDatabaseCollection);

            }
            else
            {
                this.AuditUpdate(assignmentDetails, dbAssignment, sqlAuditActionType, sqlAuditModuleType, oldData, newData, ref eventId, dbExsistanceAssignmentDetails, assignmentModuleDatabaseCollection.dbModule, assignmentModuleDatabaseCollection, dbDocuments);
                if (assignmentDetails.AssignmentInfo.AssignmentContractHoldingCompanyCode == assignmentDetails.AssignmentInfo.AssignmentOperatingCompanyCode && dbExsistanceAssignmentDetails.AssignmentTechnicalSpecialists?.Count <= 0)
                    result = this.ProcessVisitAuditSaveResource(assignmentDetails, dbAssignment, dbProject, assignmentModuleDatabaseCollection);
                if (assignmentDetails.AssignmentInfo.AssignmentContractHoldingCompanyCode != assignmentDetails.AssignmentInfo.AssignmentOperatingCompanyCode && dbExsistanceAssignmentDetails.AssignmentInfo.IsFirstVisit == false)
                    result = this.ProcessVisitAuditSave(assignmentDetails, dbAssignment, dbProject, assignmentModuleDatabaseCollection);
                if (assignmentDetails.AssignmentInfo.AssignmentContractHoldingCompanyCode != assignmentDetails.AssignmentInfo.AssignmentOperatingCompanyCode && dbExsistanceAssignmentDetails.AssignmentInfo.IsFirstVisit == true && dbExsistanceAssignmentDetails.AssignmentTechnicalSpecialists?.Count <= 0)
                    result = this.ProcessVisitAuditSaveResource(assignmentDetails, dbAssignment, dbProject, assignmentModuleDatabaseCollection);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }
        /*This method is called to log Visit related line items & technical specialist after ARS done without resource in first Save */
        private Response ProcessVisitAuditSaveResource(DomainModel.AssignmentDetail assignmentDetails, DbModel.Assignment dbAssignment, DbModel.Project dbProject, DomainModel.AssignmentModuleDatabaseCollection assignmentModuleDatabaseCollection)
        {
            long? eventIDforVisit = null;
            object newData;
            Response result = new Response();
            if (dbAssignment?.Visit != null && dbAssignment?.Visit?.Count > 0)
            {
                dbAssignment?.Visit?.ToList()?.ForEach(visit =>
                {
                    LogEventGeneration logEventGeneration = new LogEventGeneration(_auditLogger);
                    eventIDforVisit = logEventGeneration.GetEventLogId(null,
                                                            ValidationType.Update.ToAuditActionType(),
                                                           assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(),
                                                            "{" + AuditSelectType.Id + ":" + visit?.Id + "}${" + AuditSelectType.ReportNumber + ":" + visit?.ReportNumber?.Trim() + "}${" + AuditSelectType.JobReferenceNumber + ":" + assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber + "-" + assignmentDetails?.AssignmentInfo?.AssignmentNumber + "-" + visit.VisitNumber + "}${" +
                                                           AuditSelectType.ProjectAssignment + ":" + assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber + "-" + assignmentDetails?.AssignmentInfo?.AssignmentNumber + "}",
                                                           SqlAuditModuleType.Visit.ToString(),
                                                           assignmentModuleDatabaseCollection.dbModule);
                    if (visit?.VisitTechnicalSpecialist != null && visit?.VisitTechnicalSpecialist.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitTechnicalSpecialist>>(visit.VisitTechnicalSpecialist);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitSpecialistAccount, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }

                    if (visit?.VisitTechnicalSpecialistAccountItemConsumable != null && visit?.VisitTechnicalSpecialistAccountItemConsumable.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitSpecialistAccountItemConsumable>>(visit.VisitTechnicalSpecialistAccountItemConsumable);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitTechnicalSpecialistAccountItemConsumable, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (visit?.VisitTechnicalSpecialistAccountItemExpense != null && visit?.VisitTechnicalSpecialistAccountItemExpense.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitSpecialistAccountItemExpense>>(visit.VisitTechnicalSpecialistAccountItemExpense);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitTechnicalSpecialistAccountItemExpense, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (visit?.VisitTechnicalSpecialistAccountItemTime != null && visit?.VisitTechnicalSpecialistAccountItemTime.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitSpecialistAccountItemTime>>(visit.VisitTechnicalSpecialistAccountItemTime);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTime, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (visit?.VisitTechnicalSpecialistAccountItemTravel != null && visit?.VisitTechnicalSpecialistAccountItemTravel.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitSpecialistAccountItemTravel>>(visit.VisitTechnicalSpecialistAccountItemTravel);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTravel, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                });

                if (dbAssignment?.Timesheet != null && dbAssignment?.Timesheet?.Count > 0)
                {
                    dbAssignment?.Timesheet?.ToList()?.ForEach(timesheet =>
                    {
                        LogEventGeneration logEventGeneration = new LogEventGeneration(_auditLogger);
                        eventIDforVisit = logEventGeneration.GetEventLogId(null,
                                                                ValidationType.Update.ToAuditActionType(),
                                                               assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(),
                                                               "{" + AuditSelectType.Id + ":" + timesheet.Id + "}${" +
                                                                AuditSelectType.TimesheetDescription + ":" + timesheet.TimesheetDescription?.Trim() + "}${" +
                                                                AuditSelectType.JobReferenceNumber + ":" + assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber + "-" + assignmentDetails?.AssignmentInfo?.AssignmentNumber + "-" + timesheet.TimesheetNumber + "}${" +
                                                                AuditSelectType.ProjectAssignment + ":" + assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber + "-" + assignmentDetails?.AssignmentInfo?.AssignmentNumber + "}",
                                                               SqlAuditModuleType.Timesheet.ToString(),
                                                               assignmentModuleDatabaseCollection.dbModule);

                        if (timesheet?.TimesheetTechnicalSpecialist != null && timesheet?.TimesheetTechnicalSpecialist.Count > 0)
                        {
                            newData = _mapper.Map<List<Evolution.Timesheet.Domain.Models.Timesheets.TimesheetTechnicalSpecialist>>(timesheet.TimesheetTechnicalSpecialist);
                            result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.TimesheetTechnicalSpecialist, null, newData, assignmentModuleDatabaseCollection.dbModule);
                        }
                        if (timesheet?.TimesheetTechnicalSpecialistAccountItemConsumable != null && timesheet?.TimesheetTechnicalSpecialistAccountItemConsumable.Count > 0)
                        {
                            newData = _mapper.Map<List<Evolution.Timesheet.Domain.Models.Timesheets.TimesheetSpecialistAccountItemConsumable>>(timesheet.TimesheetTechnicalSpecialistAccountItemConsumable);
                            result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemConsumable, null, newData, assignmentModuleDatabaseCollection.dbModule);
                        }
                        if (timesheet?.TimesheetTechnicalSpecialistAccountItemExpense != null && timesheet?.TimesheetTechnicalSpecialistAccountItemExpense.Count > 0)
                        {
                            newData = _mapper.Map<List<Evolution.Timesheet.Domain.Models.Timesheets.TimesheetSpecialistAccountItemExpense>>(timesheet?.TimesheetTechnicalSpecialistAccountItemExpense);
                            result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemExpense, null, newData, assignmentModuleDatabaseCollection.dbModule);
                        }
                        if (timesheet?.TimesheetTechnicalSpecialistAccountItemTime != null && timesheet?.TimesheetTechnicalSpecialistAccountItemTime.Count > 0)
                        {
                            newData = _mapper.Map<List<Evolution.Timesheet.Domain.Models.Timesheets.TimesheetSpecialistAccountItemTime>>(timesheet?.TimesheetTechnicalSpecialistAccountItemTime);
                            result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTime, null, newData, assignmentModuleDatabaseCollection.dbModule);
                        }
                        if (timesheet?.TimesheetTechnicalSpecialistAccountItemTravel != null && timesheet?.TimesheetTechnicalSpecialistAccountItemTravel.Count > 0)
                        {
                            newData = _mapper.Map<List<Evolution.Timesheet.Domain.Models.Timesheets.TimesheetSpecialistAccountItemTravel>>(timesheet?.TimesheetTechnicalSpecialistAccountItemTravel);
                            result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTravel, null, newData, assignmentModuleDatabaseCollection.dbModule);
                        }

                    });
                }
            }
            return result;
        }
        private Response ProcessVisitAuditSave(DomainModel.AssignmentDetail assignmentDetails, DbModel.Assignment dbAssignment, DbModel.Project dbProject, DomainModel.AssignmentModuleDatabaseCollection assignmentModuleDatabaseCollection)
        {
            long? eventIDforVisit = null;
            object newData;
            Response result = new Response();
            if (dbAssignment?.Visit != null && dbAssignment?.Visit?.Count > 0)
            {
                dbAssignment?.Visit?.ToList()?.ForEach(visit =>
                {
                    string mainSupplierName = visit.Supplier == null ? assignmentDetails.AssignmentSubSuppliers?.FirstOrDefault(x => x.MainSupplierId == visit.SupplierId)?.MainSupplierName : visit.Supplier.SupplierName;
                    string subSupplierName = visit.Supplier == null ? assignmentDetails.AssignmentSubSuppliers.FirstOrDefault(x => x.SubSupplierId == visit.SupplierId)?.SubSupplierName : visit.Supplier.SupplierName;
                    var domVisit = _mapper.Map<Evolution.Visit.Domain.Models.Visits.Visit>(visit);
                    domVisit.VisitProjectNumber = assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber;
                    domVisit.VisitSupplier = mainSupplierName ?? subSupplierName;
                    domVisit.VisitAssignmentNumber = Convert.ToInt32(assignmentDetails?.AssignmentInfo?.AssignmentNumber);

                    _auditSearchService.AuditLog(domVisit, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(),
                                                                                             "{" + AuditSelectType.Id + ":" + visit?.Id + "}${" + AuditSelectType.ReportNumber + ":" + visit?.ReportNumber?.Trim() + "}${" + AuditSelectType.JobReferenceNumber + ":" + assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber + "-" + assignmentDetails?.AssignmentInfo?.AssignmentNumber + "-" + visit.VisitNumber + "}${" +
                                                                                             AuditSelectType.ProjectAssignment + ":" + assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber + "-" + assignmentDetails?.AssignmentInfo?.AssignmentNumber + "}",
                                                                                              ValidationType.Add.ToAuditActionType(),
                                                                                              SqlAuditModuleType.Visit,
                                                                                              null,
                                                                                              domVisit,
                                                                                              assignmentModuleDatabaseCollection.dbModule
                                                                                               );

                    if (visit?.VisitReference != null && visit?.VisitReference.Count > 0)
                    {
                        var tempVisitReference = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitReference>>(visit.VisitReference);
                        tempVisitReference.ForEach(temp =>
                        {
                            temp.ReferenceType = assignmentModuleDatabaseCollection?.DBReferenceType?.ToList().FirstOrDefault(refType => refType.Id == temp.AssignmentReferenceTypeId)?.Name;
                        });
                        newData = tempVisitReference;
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitReference, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (visit?.VisitSupplierPerformance != null && visit?.VisitSupplierPerformance.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitSupplierPerformanceType>>(visit.VisitSupplierPerformance);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitSupplierPerformance, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (visit?.VisitTechnicalSpecialist != null && visit?.VisitTechnicalSpecialist.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitTechnicalSpecialist>>(visit.VisitTechnicalSpecialist);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitSpecialistAccount, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (visit?.VisitTechnicalSpecialistAccountItemConsumable != null && visit?.VisitTechnicalSpecialistAccountItemConsumable.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitSpecialistAccountItemConsumable>>(visit.VisitTechnicalSpecialistAccountItemConsumable);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitTechnicalSpecialistAccountItemConsumable, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (visit?.VisitTechnicalSpecialistAccountItemExpense != null && visit?.VisitTechnicalSpecialistAccountItemExpense.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitSpecialistAccountItemExpense>>(visit.VisitTechnicalSpecialistAccountItemExpense);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitTechnicalSpecialistAccountItemExpense, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (visit?.VisitTechnicalSpecialistAccountItemTime != null && visit?.VisitTechnicalSpecialistAccountItemTime.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitSpecialistAccountItemTime>>(visit.VisitTechnicalSpecialistAccountItemTime);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTime, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (visit?.VisitTechnicalSpecialistAccountItemTravel != null && visit?.VisitTechnicalSpecialistAccountItemTravel.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitSpecialistAccountItemTravel>>(visit.VisitTechnicalSpecialistAccountItemTravel);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitTechnicalSpecialistAccountItemTravel, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (visit?.VisitInterCompanyDiscount != null && visit?.VisitInterCompanyDiscount.Count > 0)
                    {
                        newData = _assignmentInterCompanyDiscount.SearchWithCompany((int)assignmentDetails.AssignmentInfo.AssignmentId);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitInterCompanyDiscount, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (visit?.VisitNote != null && visit?.VisitNote.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Visit.Domain.Models.Visits.VisitNote>>(visit.VisitNote);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.VisitNote, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }

                });

            }

            if (dbAssignment?.Timesheet != null && dbAssignment?.Timesheet?.Count > 0)
            {
                dbAssignment?.Timesheet?.ToList()?.ForEach(timesheet =>
                {
                    var domTimesheet = _mapper.Map<Evolution.Timesheet.Domain.Models.Timesheets.Timesheet>(timesheet);
                    domTimesheet.TimesheetProjectNumber = assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber;
                    domTimesheet.TimesheetAssignmentNumber = assignmentDetails?.AssignmentInfo?.AssignmentNumber;

                    _auditSearchService.AuditLog(domTimesheet, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(),
                                                                                               "{" + AuditSelectType.Id + ":" + timesheet.Id + "}${" +
                                                                                              AuditSelectType.TimesheetDescription + ":" + timesheet.TimesheetDescription?.Trim() + "}${" +
                                                                                              AuditSelectType.JobReferenceNumber + ":" + assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber + "-" + assignmentDetails?.AssignmentInfo?.AssignmentNumber + "-" + timesheet.TimesheetNumber + "}${" +
                                                                                              AuditSelectType.ProjectAssignment + ":" + assignmentDetails?.AssignmentInfo?.AssignmentProjectNumber + "-" + assignmentDetails?.AssignmentInfo?.AssignmentNumber + "}",
                                                                                              ValidationType.Add.ToAuditActionType(),
                                                                                              SqlAuditModuleType.Timesheet,
                                                                                              null,
                                                                                              domTimesheet,
                                                                                              assignmentModuleDatabaseCollection.dbModule
                                                                                              );

                    if (timesheet?.TimesheetReference != null && timesheet?.TimesheetReference.Count > 0)
                    {
                        var tempTimesheetReference = _mapper.Map<List<Evolution.Timesheet.Domain.Models.Timesheets.TimesheetReferenceType>>(timesheet.TimesheetReference);
                        tempTimesheetReference.ForEach(temp =>
                        {
                            temp.ReferenceType = assignmentModuleDatabaseCollection?.DBReferenceType?.ToList().FirstOrDefault(refType => refType.Id == temp.AssignmentReferenceTypeId)?.Name;
                        });
                        newData = tempTimesheetReference;
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.TimesheetReference, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (timesheet?.TimesheetTechnicalSpecialist != null && timesheet?.TimesheetTechnicalSpecialist.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Timesheet.Domain.Models.Timesheets.TimesheetTechnicalSpecialist>>(timesheet.TimesheetTechnicalSpecialist);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.TimesheetTechnicalSpecialist, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (timesheet?.TimesheetTechnicalSpecialistAccountItemConsumable != null && timesheet?.TimesheetTechnicalSpecialistAccountItemConsumable.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Timesheet.Domain.Models.Timesheets.TimesheetSpecialistAccountItemConsumable>>(timesheet.TimesheetTechnicalSpecialistAccountItemConsumable);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemConsumable, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (timesheet?.TimesheetTechnicalSpecialistAccountItemExpense != null && timesheet?.TimesheetTechnicalSpecialistAccountItemExpense.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Timesheet.Domain.Models.Timesheets.TimesheetSpecialistAccountItemExpense>>(timesheet?.TimesheetTechnicalSpecialistAccountItemExpense);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemExpense, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (timesheet?.TimesheetTechnicalSpecialistAccountItemTime != null && timesheet?.TimesheetTechnicalSpecialistAccountItemTime.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Timesheet.Domain.Models.Timesheets.TimesheetSpecialistAccountItemTime>>(timesheet?.TimesheetTechnicalSpecialistAccountItemTime);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTime, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }
                    if (timesheet?.TimesheetTechnicalSpecialistAccountItemTravel != null && timesheet?.TimesheetTechnicalSpecialistAccountItemTravel.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Timesheet.Domain.Models.Timesheets.TimesheetSpecialistAccountItemTravel>>(timesheet?.TimesheetTechnicalSpecialistAccountItemTravel);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.TimesheetTechnicalSpecialistAccountItemTravel, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }

                    if (timesheet?.TimesheetInterCompanyDiscount != null && timesheet?.TimesheetInterCompanyDiscount.Count > 0)
                    {
                        newData = _assignmentInterCompanyDiscount.Search((int)assignmentDetails.AssignmentInfo.AssignmentId);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.TimesheetInterCompanyDiscount, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }

                    if (timesheet?.TimesheetNote != null && timesheet?.TimesheetNote.Count > 0)
                    {
                        newData = _mapper.Map<List<Evolution.Timesheet.Domain.Models.Timesheets.TimesheetNote>>(timesheet.TimesheetNote);
                        result = _auditSearchService.AuditLog(newData, ref eventIDforVisit, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, ValidationType.Add.ToAuditActionType(), SqlAuditModuleType.TimesheetNote, null, newData, assignmentModuleDatabaseCollection.dbModule);
                    }

                });

            }
            return result;
        }

        private Response ProcessAuditSave(DomainModel.AssignmentDetail assignmentDetails,
                                DbModel.Assignment dbAssignment,
                                SqlAuditActionType sqlAuditActionType,
                                SqlAuditModuleType sqlAuditModuleType,
                                object oldData,
                                object newData,
                                ref long? eventId,
                                DomainModel.AssignmentDetail dbExsistanceAssignmentDetails,
                                IList<DbModel.SqlauditModule> dbModule,
                                DomainModel.AssignmentModuleDatabaseCollection assignmentModuleDatabaseCollection,
                                List<DbModel.Document> dbDocuments = null)
        {
            Exception exception = null;
            Response result = null;
            var assignTechSpecSchedules = assignmentDetails?.AssignmentTechnicalSpecialists?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules)?.ToList();
            var assignmentSubSupplierTs = assignmentDetails?.AssignmentSubSuppliers?.SelectMany(x => x.AssignmentSubSupplierTS)?.ToList();
            var assignmentContrRenuveCost = assignmentDetails?.AssignmentContributionCalculators?.SelectMany(x => x.AssignmentContributionRevenueCosts)?.ToList();
            var oldAssignTechSpecSchedules = dbExsistanceAssignmentDetails?.AssignmentTechnicalSpecialists?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules)?.ToList();
            var oldAssignmentSubSupplierTs = dbExsistanceAssignmentDetails?.AssignmentSubSuppliers?.SelectMany(x => x.AssignmentSubSupplierTS)?.ToList();
            var oldAssignmentContrRenuveCost = dbExsistanceAssignmentDetails?.AssignmentContributionCalculators?.SelectMany(x => x.AssignmentContributionRevenueCosts)?.ToList();

            if (assignmentDetails?.AssignmentInfo != null)
            {
                var newAssignmentInfo = assignmentDetails.AssignmentInfo.RecordStatus.IsRecordStatusNew();
                if (newAssignmentInfo)
                {
                    newData = assignmentDetails?.AssignmentInfo;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.Assignment, null, newData, dbModule);
                }
            }

            if (assignmentDetails?.AssignmentAdditionalExpenses?.Count > 0)
            {
                var newAdditionalExpenses = assignmentDetails.AssignmentAdditionalExpenses.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                if (newAdditionalExpenses.Count > 0)
                {
                    newData = newAdditionalExpenses;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentAdditionalExpense, null, newData, dbModule);
                }
            }

            if (assignmentDetails?.AssignmentNotes?.Count > 0)
            {
                var newNotes = assignmentDetails.AssignmentNotes.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                if (newNotes.Count > 0)
                {
                    newData = _mapper.Map<List<DomainModel.AssignmentNote>>(dbAssignment?.AssignmentNote);
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentNote, null, newData, dbModule);
                }
            }

            if (assignmentDetails?.AssignmentContractSchedules?.Count > 0)
            {
                var newContractSchedules = assignmentDetails.AssignmentContractSchedules.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (newContractSchedules.Count > 0)
                {
                    if (dbAssignment?.AssignmentContractSchedule?.Count > 0)
                    {
                        newData = dbAssignment?.AssignmentContractSchedule?.Select(x => new DomainModel.AssignmentContractRateSchedule
                        {
                            AssignmentContractRateScheduleId = x.Id,
                            AssignmentId = x.AssignmentId,
                            ContractScheduleId = x.ContractScheduleId,
                            ContractScheduleName = assignmentDetails?.AssignmentContractSchedules?.FirstOrDefault(x1 => x.ContractScheduleId == x1.ContractScheduleId)?.ContractScheduleName
                        }).ToList();
                    }
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentContractSchedule, null, newData, dbModule);
                }
            }

            if (assignmentDetails?.AssignmentContributionCalculators?.Count > 0)
            {
                var newContributionCalculators = assignmentDetails.AssignmentContractSchedules.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                if (newContributionCalculators.Count > 0)
                {
                    newData = _mapper.Map<List<DomainModel.AssignmentContributionCalculation>>(dbAssignment?.AssignmentContributionCalculation);
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentContributionCalculation, null, newData, dbModule);
                }
            }

            if (assignmentDetails?.AssignmentReferences?.Count > 0)
            {
                var newReferences = assignmentDetails.AssignmentReferences.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                if (newReferences.Count > 0)
                {
                    var tempAssignmentReference = _mapper.Map<List<DomainModel.AssignmentReferenceType>>(dbAssignment?.AssignmentReferenceNavigation);
                    tempAssignmentReference.ForEach(tempAssignment =>
                    {
                        tempAssignment.ReferenceType = assignmentModuleDatabaseCollection?.DBReferenceType?.ToList()?.FirstOrDefault(refType => refType.Id == tempAssignment.AssignmentReferenceTypeMasterId)?.Name;
                    });
                    newData = tempAssignmentReference;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentReference, null, newData, dbModule);
                }
            }

            if (assignmentDetails?.AssignmentSubSuppliers?.Count > 0)
            {
                var newSubSuppliers = assignmentDetails.AssignmentSubSuppliers.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                if (newSubSuppliers.Count > 0)
                {
                    newData = _mapper.Map<List<DomainModel.AssignmentSubSupplier>>(dbAssignment?.AssignmentSubSupplier);
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentSubSupplier, null, newData, dbModule);
                }
            }

            if (assignmentDetails?.AssignmentTaxonomy?.Count > 0)
            {
                var newTaxonomy = assignmentDetails.AssignmentTaxonomy.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                if (newTaxonomy.Count > 0)
                {
                    if (dbAssignment?.AssignmentTaxonomy?.Count > 0)
                    {
                        newData = dbAssignment?.AssignmentTaxonomy?.Select(x => new DomainModel.AssignmentTaxonomy
                        {
                            AssignmentTaxonomyId = x.Id,
                            AssignmentId = x.AssignmentId,
                            TaxonomyCategory = assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomyCategory,
                            TaxonomySubCategory = assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomySubCategory,
                            TaxonomyService = assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomyService,
                            TaxonomyCategoryId = assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomyCategoryId != null ? (int)assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomyCategoryId : 0,
                            TaxonomyServiceId = assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomyServiceId != null ? (int)assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomyServiceId : 0,
                            TaxonomySubCategoryId = assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomySubCategoryId != null ? (int)assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomySubCategoryId : 0,
                        })?.ToList();
                    }
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentTaxonomy, null, newData, dbModule);
                }
            }
            if (assignmentDetails?.AssignmentTechnicalSpecialists?.Count > 0)
            {
                var newTechnicalSpecialists = assignmentDetails.AssignmentTechnicalSpecialists.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (newTechnicalSpecialists.Count > 0)
                {
                    newData = _mapper.Map<List<DomainModel.AssignmentTechnicalSpecialist>>(dbAssignment?.AssignmentTechnicalSpecialist);
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentTechnicalSpecialist, null, newData, dbModule);
                }
            }
            if (assignmentDetails?.AssignmentInterCompanyDiscounts != null)
            {
                var AssignmentInterCompanyDiscounts = assignmentDetails.AssignmentInterCompanyDiscounts.RecordStatus.IsRecordStatusNew();
                if (AssignmentInterCompanyDiscounts)
                {

                    newData = assignmentDetails?.AssignmentInterCompanyDiscounts;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentInterCo, null, newData, dbModule);
                }
            }
            if (assignmentDetails?.AssignmentInstructions != null)
            {
                var newAssignmentInstructions = assignmentDetails.AssignmentInstructions.RecordStatus.IsRecordStatusNew();
                if (newAssignmentInstructions)
                {
                    newData = assignmentDetails?.AssignmentInstructions;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentInstructions, null, newData, dbModule);
                }
            }

            if (assignmentContrRenuveCost?.Count > 0)
            {
                var newContrRenuveCost = assignmentContrRenuveCost.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (newContrRenuveCost.Count > 0)
                {
                    newData = _mapper.Map<List<DomainModel.AssignmentContributionRevenueCost>>(dbAssignment?.AssignmentContributionCalculation.SelectMany(x => x.AssignmentContributionRevenueCost));
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentContributionRenuveCost, null, newData, dbModule);
                }
            }
            if (assignmentSubSupplierTs?.Count > 0)
            {
                var newSubSupplier = assignmentSubSupplierTs.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (newSubSupplier.Count > 0)
                {
                    newData = _mapper.Map<List<DomainModel.AssignmentSubSupplierTS>>(dbAssignment?.AssignmentSubSupplier.SelectMany(x => x.AssignmentSubSupplierTechnicalSpecialist));
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentSpecialistSubSupplier, null, newData, dbModule);
                }
            }

            if (assignTechSpecSchedules?.Count > 0)
            {
                var newTechSpecSchedules = assignTechSpecSchedules.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();

                if (newTechSpecSchedules.Count > 0)
                {
                    newData = _mapper.Map<List<DomainModel.AssignmentTechnicalSpecialistSchedule>>(dbAssignment?.AssignmentTechnicalSpecialist.SelectMany(x => x.AssignmentTechnicalSpecialistSchedule));
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentTechnicalSchedule, null, newData, dbModule);
                }
            }
            if (assignmentDetails?.AssignmentDocuments != null && assignmentDetails?.AssignmentDocuments?.Count > 0)
            {
                if (assignmentDetails?.AssignmentDocuments?.Count > 0)
                {
                    newData = assignmentDetails?.AssignmentDocuments.Select(x =>
                        {
                            x.CreatedOn = dbDocuments?.FirstOrDefault(y => y.DocumentUniqueName == x.DocumentUniqueName)?.CreatedDate;
                            return x;
                        }).ToList(); // audit create date issue fix

                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentDocument, oldData, newData, dbModule);
                }
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);

        }


        private Response AuditUpdate(DomainModel.AssignmentDetail assignmentDetails,
                                 DbModel.Assignment dbAssignment,
                                 SqlAuditActionType sqlAuditActionType,
                                 SqlAuditModuleType sqlAuditModuleType,
                                 object oldData,
                                 object newData,
                                 ref long? eventId,
                                 DomainModel.AssignmentDetail dbExsistanceAssignmentDetails,
                                 IList<DbModel.SqlauditModule> dbModule,
                                 DomainModel.AssignmentModuleDatabaseCollection assignmentModuleDatabaseCollection,
                                 List<DbModel.Document> dbDocuments = null
                                 )
        {
            Exception exception = null;
            Response result = null;

            var assignTechSpecSchedules = assignmentDetails?.AssignmentTechnicalSpecialists?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules)?.ToList();
            var assignmentSubSupplierTs = assignmentDetails?.AssignmentSubSuppliers?.SelectMany(x => x.AssignmentSubSupplierTS)?.ToList();
            var assignmentContrRenuveCost = assignmentDetails?.AssignmentContributionCalculators?.SelectMany(x => x.AssignmentContributionRevenueCosts)?.ToList();
            var oldAssignTechSpecSchedules = dbExsistanceAssignmentDetails?.AssignmentTechnicalSpecialists?.SelectMany(x => x.AssignmentTechnicalSpecialistSchedules)?.ToList();
            var oldAssignmentSubSupplierTs = dbExsistanceAssignmentDetails?.AssignmentSubSuppliers?.SelectMany(x => x.AssignmentSubSupplierTS)?.ToList();
            var oldAssignmentContrRenuveCost = dbExsistanceAssignmentDetails?.AssignmentContributionCalculators?.SelectMany(x => x.AssignmentContributionRevenueCosts)?.ToList();


            if (assignmentDetails.AssignmentInfo != null)
            {
                var ModifiedAssignmentInfo = assignmentDetails.AssignmentInfo.RecordStatus.IsRecordStatusModified();
                if (ModifiedAssignmentInfo)
                {
                    newData = assignmentDetails?.AssignmentInfo;
                    oldData = dbExsistanceAssignmentDetails?.AssignmentInfo;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.Assignment, oldData, newData, dbModule);
                }
            }

            if (assignmentDetails?.AssignmentAdditionalExpenses?.Count > 0)
            {
                var newAdditionalExpenses = assignmentDetails.AssignmentAdditionalExpenses.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modifieAdditionalExpenses = assignmentDetails.AssignmentAdditionalExpenses.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var deletedAdditionalExpenses = assignmentDetails.AssignmentAdditionalExpenses.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (newAdditionalExpenses.Count > 0)
                {
                    var newaddexp = _mapper.Map<List<DomainModel.AssignmentAdditionalExpense>>(newAdditionalExpenses);
                    newData = newaddexp?.Where(x => !dbExsistanceAssignmentDetails.AssignmentAdditionalExpenses.Any(x1 => x1.AssignmentAdditionalExpenseId == x.AssignmentAdditionalExpenseId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentAdditionalExpense, null, newData, dbModule);
                }
                if (modifieAdditionalExpenses.Count > 0)
                {
                    newData = modifieAdditionalExpenses;
                    var Ids = modifieAdditionalExpenses?.Select(x => x.AssignmentAdditionalExpenseId);
                    oldData = dbExsistanceAssignmentDetails?.AssignmentAdditionalExpenses?.Where(x => Ids.Contains(x.AssignmentAdditionalExpenseId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentAdditionalExpense, oldData, newData, dbModule);
                }
                if (deletedAdditionalExpenses.Count > 0)
                {
                    oldData = deletedAdditionalExpenses;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentAdditionalExpense, oldData, null, dbModule);
                }
            }

            if (assignmentDetails?.AssignmentNotes?.Count > 0)
            {
                var newNotes = assignmentDetails.AssignmentNotes.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modifiedNotes = assignmentDetails.AssignmentNotes.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var deletedNotes = assignmentDetails.AssignmentNotes.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (newNotes.Count > 0)
                {
                    var Notes = _mapper.Map<List<DomainModel.AssignmentNote>>(dbAssignment?.AssignmentNote);
                    newData = Notes?.Where(x => !dbExsistanceAssignmentDetails.AssignmentNotes.Any(x1 => x1.AssignmnetNoteId == x.AssignmnetNoteId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentNote, null, newData, dbModule);
                }
                if (modifiedNotes.Count > 0)
                {
                    newData = modifiedNotes;
                    var Ids = modifiedNotes?.Select(x => x.AssignmnetNoteId);
                    oldData = dbExsistanceAssignmentDetails?.AssignmentNotes?.Where(x => Ids.Contains(x.AssignmnetNoteId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentNote, oldData, newData, dbModule);
                }
                if (deletedNotes.Count > 0)
                {
                    oldData = deletedNotes;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentNote, oldData, null, dbModule);
                }
            }

            if (assignmentDetails?.AssignmentContractSchedules?.Count > 0)
            {
                var newContractSchedules = assignmentDetails.AssignmentContractSchedules.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modifiedContractSchedules = assignmentDetails.AssignmentContractSchedules.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var deletedContractSchedules = assignmentDetails.AssignmentContractSchedules.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (newContractSchedules.Count > 0)
                {
                    if (dbAssignment?.AssignmentContractSchedule?.Count > 0)
                    {
                        var assignmentContractSchedules = dbAssignment?.AssignmentContractSchedule?.Select(x => new DomainModel.AssignmentContractRateSchedule
                        {
                            AssignmentContractRateScheduleId = x.Id,
                            AssignmentId = x.AssignmentId,
                            ContractScheduleId = x.ContractScheduleId,
                            ContractScheduleName = assignmentDetails?.AssignmentContractSchedules?.FirstOrDefault(x1 => x.ContractScheduleId == x1.ContractScheduleId)?.ContractScheduleName
                        });
                        //var assignmentContractSchedules = _mapper.Map<List<DomainModel.AssignmentContractRateSchedule>>(dbAssignment?.AssignmentContractSchedule).ToList();
                        newData = assignmentContractSchedules?.Where(x => !dbExsistanceAssignmentDetails.AssignmentContractSchedules.Any(x1 => x1.AssignmentContractRateScheduleId == x.AssignmentContractRateScheduleId)).ToList();
                        result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentContractSchedule, null, newData, dbModule);
                    }
                }
                if (modifiedContractSchedules.Count > 0)
                {
                    newData = modifiedContractSchedules;
                    var Ids = modifiedContractSchedules?.Select(x => x.AssignmentContractRateScheduleId);
                    oldData = dbExsistanceAssignmentDetails?.AssignmentContractSchedules?.Where(x => Ids.Contains(x.AssignmentContractRateScheduleId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentContractSchedule, oldData, newData, dbModule);
                }
                if (deletedContractSchedules.Count > 0)
                {
                    oldData = deletedContractSchedules;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentContractSchedule, oldData, null, dbModule);
                }
            }
            if (assignmentDetails?.AssignmentContributionCalculators?.Count > 0)
            {
                var newContributionCalculators = assignmentDetails.AssignmentContractSchedules.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modifiedContributionCalculators = assignmentDetails.AssignmentContractSchedules.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var deletedContributionCalculators = assignmentDetails.AssignmentContractSchedules.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (newContributionCalculators.Count > 0)
                {
                    var ContributionCalculators = _mapper.Map<List<DomainModel.AssignmentContributionCalculation>>(dbAssignment?.AssignmentContributionCalculation);
                    newData = ContributionCalculators?.Where(x => !dbExsistanceAssignmentDetails.AssignmentContributionCalculators.Any(x1 => x1.AssignmentContCalculationId == x.AssignmentContCalculationId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentContributionCalculation, null, newData, dbModule);
                }
                if (modifiedContributionCalculators.Count > 0)
                {
                    newData = modifiedContributionCalculators;
                    var Ids = modifiedContributionCalculators?.Select(x => x.AssignmentContractRateScheduleId);
                    oldData = dbExsistanceAssignmentDetails?.AssignmentContributionCalculators?.Where(x => Ids.Contains(x.AssignmentContCalculationId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentContributionCalculation, oldData, newData, dbModule);
                }
                if (deletedContributionCalculators.Count > 0)
                {
                    oldData = deletedContributionCalculators;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentContributionCalculation, oldData, null, dbModule);
                }
            }

            if (assignmentDetails?.AssignmentReferences?.Count > 0)
            {
                var newReferences = assignmentDetails.AssignmentReferences.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modifiedReferences = assignmentDetails.AssignmentReferences.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var deletedReferences = assignmentDetails.AssignmentReferences.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (newReferences.Count > 0)
                {
                    var tempAssignmentReference = _mapper.Map<List<DomainModel.AssignmentReferenceType>>(dbAssignment?.AssignmentReferenceNavigation);
                    tempAssignmentReference.ForEach(tempAssignment =>
                    {
                        tempAssignment.ReferenceType = assignmentModuleDatabaseCollection?.DBReferenceType?.ToList()?.FirstOrDefault(refType => refType.Id == tempAssignment.AssignmentReferenceTypeMasterId)?.Name;
                    });
                    newData = tempAssignmentReference;
                    newData = tempAssignmentReference?.Where(x => !dbExsistanceAssignmentDetails.AssignmentReferences.Any(x1 => x1.AssignmentReferenceTypeId == x.AssignmentReferenceTypeId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentReference, null, newData, dbModule);
                }
                if (modifiedReferences.Count > 0)
                {
                    newData = modifiedReferences;
                    var Ids = modifiedReferences?.Select(x => x.AssignmentReferenceTypeId);
                    oldData = dbExsistanceAssignmentDetails?.AssignmentReferences?.Where(x => Ids.Contains(x.AssignmentReferenceTypeId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentReference, oldData, newData, dbModule);
                }
                if (deletedReferences.Count > 0)
                {
                    oldData = deletedReferences;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentReference, oldData, null, dbModule);
                }
            }

            if (assignmentDetails?.AssignmentSubSuppliers?.Count > 0)
            {
                var newSubSuppliers = assignmentDetails.AssignmentSubSuppliers.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modifiedSubSuppliers = assignmentDetails.AssignmentSubSuppliers.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var deletedSubSuppliers = assignmentDetails.AssignmentSubSuppliers.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (newSubSuppliers.Count > 0)
                {
                    var SubSuppliers = _mapper.Map<List<DomainModel.AssignmentSubSupplier>>(dbAssignment?.AssignmentSubSupplier);
                    newData = SubSuppliers?.Where(x => !dbExsistanceAssignmentDetails.AssignmentSubSuppliers.Any(x1 => x1.AssignmentSubSupplierId == x.AssignmentSubSupplierId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentSubSupplier, null, newData, dbModule);
                }
                if (modifiedSubSuppliers.Count > 0)
                {
                    newData = modifiedSubSuppliers;
                    var Ids = modifiedSubSuppliers?.Select(x => x.AssignmentSubSupplierId);
                    oldData = dbExsistanceAssignmentDetails?.AssignmentSubSuppliers?.Where(x => Ids.Contains(x.AssignmentSubSupplierId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentSubSupplier, oldData, newData, dbModule);
                }
                if (deletedSubSuppliers.Count > 0)
                {
                    oldData = deletedSubSuppliers;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentSubSupplier, oldData, null, dbModule);
                }
            }

            if (assignmentDetails?.AssignmentTaxonomy?.Count > 0)
            {
                var newTaxonomy = assignmentDetails.AssignmentTaxonomy.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modifiedTaxonomy = assignmentDetails.AssignmentTaxonomy.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var deletedTaxonomy = assignmentDetails.AssignmentTaxonomy.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (newTaxonomy.Count > 0)
                {
                    if (dbAssignment?.AssignmentTaxonomy?.Count > 0)
                    {
                        var taxonomy = dbAssignment?.AssignmentTaxonomy?.Select(x => new DomainModel.AssignmentTaxonomy
                        {
                            AssignmentTaxonomyId = x.Id,
                            AssignmentId = x.AssignmentId,
                            TaxonomyCategory = assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomyCategory,
                            TaxonomySubCategory = assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomySubCategory,
                            TaxonomyService = assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomyService,
                            TaxonomyCategoryId = assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomyCategoryId != null ? (int)assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomyCategoryId : 0,
                            TaxonomyServiceId = assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomyServiceId != null ? (int)assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomyServiceId : 0,
                            TaxonomySubCategoryId = assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomySubCategoryId != null ? (int)assignmentDetails?.AssignmentTaxonomy?.FirstOrDefault(x1 => x.TaxonomyServiceId == x1.TaxonomyServiceId)?.TaxonomySubCategoryId : 0,
                        })?.ToList();

                        //var taxonomy = _mapper.Map<List<DomainModel.AssignmentTaxonomy>>(dbAssignment?.AssignmentTaxonomy);
                        newData = taxonomy?.Where(x => !dbExsistanceAssignmentDetails.AssignmentTaxonomy.Any(x1 => x1.AssignmentTaxonomyId == x.AssignmentTaxonomyId)).ToList();
                        result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentTaxonomy, null, newData, dbModule);

                    }
                }
                if (modifiedTaxonomy.Count > 0)
                {
                    newData = modifiedTaxonomy;
                    var Ids = modifiedTaxonomy?.Select(x => x.AssignmentTaxonomyId);
                    oldData = dbExsistanceAssignmentDetails?.AssignmentTaxonomy?.Where(x => Ids.Contains(x.AssignmentTaxonomyId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentTaxonomy, oldData, newData, dbModule);
                }
                if (deletedTaxonomy.Count > 0)
                {
                    oldData = deletedTaxonomy;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentTaxonomy, oldData, null, dbModule);
                }

            }
            if (assignmentDetails?.AssignmentTechnicalSpecialists?.Count > 0)
            {
                var newTechnicalSpecialists = assignmentDetails.AssignmentTechnicalSpecialists.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modifiedTechnicalSpecialists = assignmentDetails.AssignmentTechnicalSpecialists.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var deletedTechnicalSpecialists = assignmentDetails.AssignmentTechnicalSpecialists.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (newTechnicalSpecialists.Count > 0)
                {
                    var technicalSpecialists = _mapper.Map<List<DomainModel.AssignmentTechnicalSpecialist>>(dbAssignment?.AssignmentTechnicalSpecialist);
                    newData = technicalSpecialists?.Where(x => !dbExsistanceAssignmentDetails.AssignmentTechnicalSpecialists.Any(x1 => x1.AssignmentTechnicalSplId == x.AssignmentTechnicalSplId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentTechnicalSpecialist, null, newData, dbModule);
                }
                if (modifiedTechnicalSpecialists.Count > 0)
                {
                    newData = modifiedTechnicalSpecialists;
                    var Ids = modifiedTechnicalSpecialists?.Select(x => x.AssignmentTechnicalSplId);
                    oldData = dbExsistanceAssignmentDetails?.AssignmentTechnicalSpecialists?.Where(x => Ids.Contains(x.AssignmentTechnicalSplId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentTechnicalSpecialist, oldData, newData, dbModule);
                }
                if (deletedTechnicalSpecialists.Count > 0)
                {
                    oldData = deletedTechnicalSpecialists;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentTechnicalSpecialist, oldData, null, dbModule);
                }
            }
            if (assignmentDetails.AssignmentInterCompanyDiscounts != null)
            {
                var newInterCompanyDiscounts = assignmentDetails.AssignmentInterCompanyDiscounts.RecordStatus.IsRecordStatusNew();
                var modifiedInterCompanyDiscounts = assignmentDetails.AssignmentInterCompanyDiscounts.RecordStatus.IsRecordStatusModified();
                var deletedInterCompanyDiscounts = assignmentDetails.AssignmentInterCompanyDiscounts.RecordStatus.IsRecordStatusDeleted();

                if (newInterCompanyDiscounts)
                {
                    newData = assignmentDetails?.AssignmentInterCompanyDiscounts;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentInterCo, null, newData, dbModule);
                }
                if (modifiedInterCompanyDiscounts)
                {
                    newData = assignmentDetails?.AssignmentInterCompanyDiscounts;

                    oldData = dbExsistanceAssignmentDetails?.AssignmentInterCompanyDiscounts;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentInterCo, oldData, newData, dbModule);
                }
                if (deletedInterCompanyDiscounts)
                {
                    oldData = deletedInterCompanyDiscounts;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentInterCo, oldData, null, dbModule);
                }
            }

            if (assignmentDetails.AssignmentInstructions != null)
            {
                var newAssignmentInstructions = assignmentDetails.AssignmentInstructions.RecordStatus.IsRecordStatusNew();
                var modifiedAssignmentInstructions = assignmentDetails.AssignmentInstructions.RecordStatus.IsRecordStatusModified();
                var deletedAssignmentInstructions = assignmentDetails.AssignmentInstructions.RecordStatus.IsRecordStatusDeleted();

                if (newAssignmentInstructions)
                {
                    newData = assignmentDetails?.AssignmentInstructions;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentInstructions, null, newData, dbModule);
                }
                if (modifiedAssignmentInstructions)
                {
                    newData = assignmentDetails?.AssignmentInstructions;

                    oldData = dbExsistanceAssignmentDetails?.AssignmentInstructions;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentInstructions, oldData, newData, dbModule);
                }
                if (deletedAssignmentInstructions)
                {
                    oldData = deletedAssignmentInstructions;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentInstructions, oldData, null, dbModule);
                }
            }
            if (assignTechSpecSchedules?.Count > 0)
            {
                var newTechSpecSchedules = assignTechSpecSchedules.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modify = assignTechSpecSchedules.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var Delete = assignTechSpecSchedules.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
                if (newTechSpecSchedules.Count > 0)
                {
                    var technicalSpecialists = _mapper.Map<List<DomainModel.AssignmentTechnicalSpecialistSchedule>>(dbAssignment?.AssignmentTechnicalSpecialist.SelectMany(x => x.AssignmentTechnicalSpecialistSchedule));

                    //var technicalSpecialists = _mapper.Map<List<DomainModel.AssignmentTechnicalSpecialist>>(dbAssignment.AssignmentTechnicalSpecialist);
                    newData = technicalSpecialists?.Where(x => !oldAssignTechSpecSchedules.Any(x1 => x1.AssignmentTechnicalSpecialistScheduleId == x.AssignmentTechnicalSpecialistScheduleId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentTechnicalSchedule, null, newData, dbModule);
                }
                if (modify.Count > 0)
                {
                    newData = modify;
                    var Ids = modify?.Select(x => x.AssignmentTechnicalSpecialistScheduleId);
                    oldData = oldAssignTechSpecSchedules?.Where(x => Ids.Contains(x.AssignmentTechnicalSpecialistScheduleId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentTechnicalSchedule, oldData, newData, dbModule);
                }
                if (Delete.Count > 0)
                {
                    oldData = Delete;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentTechnicalSchedule, oldData, null, dbModule);
                }
            }
            if (assignmentSubSupplierTs?.Count > 0)
            {
                var newSubSupplierTs = assignmentSubSupplierTs.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modify = assignmentSubSupplierTs.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var Delete = assignmentSubSupplierTs.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (newSubSupplierTs.Count > 0)
                {
                    var SubSupplierTS = _mapper.Map<List<DomainModel.AssignmentSubSupplierTS>>(dbAssignment?.AssignmentSubSupplier.SelectMany(x => x.AssignmentSubSupplierTechnicalSpecialist));

                    //var technicalSpecialists = _mapper.Map<List<DomainModel.AssignmentTechnicalSpecialist>>(dbAssignment.AssignmentTechnicalSpecialist);
                    newData = SubSupplierTS?.Where(x => !oldAssignmentSubSupplierTs.Any(x1 => x1.AssignmentSubSupplierTSId == x.AssignmentSubSupplierTSId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentSpecialistSubSupplier, null, newData, dbModule);
                }
                if (modify.Count > 0)
                {
                    newData = modify;
                    var Ids = modify?.Select(x => x.AssignmentSubSupplierTSId);
                    oldData = oldAssignmentSubSupplierTs?.Where(x => Ids.Contains(x.AssignmentSubSupplierTSId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentSpecialistSubSupplier, oldData, newData, dbModule);
                }
                if (Delete.Count > 0)
                {
                    oldData = Delete;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentSpecialistSubSupplier, oldData, null, dbModule);
                }
            }
            if (assignmentContrRenuveCost?.Count > 0)
            {
                var newContrRenuveCost = assignmentContrRenuveCost.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
                var modify = assignmentContrRenuveCost.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                var Delete = assignmentContrRenuveCost.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                if (newContrRenuveCost.Count > 0)
                {
                    var renuveCost = _mapper.Map<List<DomainModel.AssignmentContributionRevenueCost>>(dbAssignment?.AssignmentContributionCalculation.SelectMany(x => x.AssignmentContributionRevenueCost));

                    //var technicalSpecialists = _mapper.Map<List<DomainModel.AssignmentTechnicalSpecialist>>(dbAssignment.AssignmentTechnicalSpecialist);
                    newData = renuveCost?.Where(x => !oldAssignmentContrRenuveCost.Any(x1 => x1.AssignmentContributionRevenueCostId == x.AssignmentContributionRevenueCostId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentContributionRenuveCost, null, newData, dbModule);
                }
                if (modify.Count > 0)
                {
                    newData = modify;
                    var Ids = modify?.Select(x => x.AssignmentContributionRevenueCostId);
                    oldData = oldAssignmentContrRenuveCost?.Where(x => Ids.Contains(x.AssignmentContributionRevenueCostId)).ToList();
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentContributionRenuveCost, oldData, newData, dbModule);
                }
                if (Delete.Count > 0)
                {
                    oldData = Delete;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails.AssignmentInfo.ActionByUser.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentContributionRenuveCost, oldData, null, dbModule);
                }
            }

            //For Document Audit
            if (assignmentDetails?.AssignmentDocuments?.Count > 0)
            {
                var newDocument = assignmentDetails?.AssignmentDocuments?.Where(x => x.RecordStatus.IsRecordStatusNew())?.ToList();
                var modifiedDocument = assignmentDetails?.AssignmentDocuments?.Where(x => x.RecordStatus.IsRecordStatusModified())?.ToList();
                var deletedDocument = assignmentDetails?.AssignmentDocuments?.Where(x => x.RecordStatus.IsRecordStatusDeleted())?.ToList();
                if (newDocument.Count > 0)
                {
                    newData = newDocument.Select(x =>
                    {
                        x.CreatedOn = dbDocuments?.FirstOrDefault(y => y.DocumentUniqueName == x.DocumentUniqueName)?.CreatedDate;
                        return x;
                    }).ToList(); // audit create date issue fix
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentDocument, null, newData, dbModule);
                }
                if (modifiedDocument.Count > 0)
                {
                    newData = modifiedDocument?.OrderBy(x => x.Id)?.ToList();
                    oldData = (dbDocuments != null && dbDocuments.Count > 0) ? _mapper.Map<List<ModuleDocument>>(dbDocuments)?.OrderBy(x => x.Id)?.ToList() : null;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentDocument, oldData, newData, dbModule);
                }
                if (deletedDocument.Count > 0)
                {
                    oldData = deletedDocument;
                    result = _auditSearchService.AuditLog(assignmentDetails, ref eventId, assignmentDetails?.AssignmentInfo?.ActionByUser?.ToString(), null, sqlAuditActionType, SqlAuditModuleType.AssignmentDocument, oldData, null, dbModule);
                }
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }

        private class EmailPlaceHolderItem
        {
            public string PlaceHolderName { get; set; }

            public string PlaceHolderValue { get; set; }

            public string PlaceHolderForEmail { get; set; }
        }

        private void AddAssignmentHistory(int? assignmentId, IList<DbModel.Data> dbMasterData, string changedBy)
        {
            try
            {
                int id = assignmentId ?? 0;
                if (id > 0)
                {
                    _assignmentRepository.AddAssignmentHistory(id, dbMasterData, changedBy);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }

        public Response GetExpenseLineItemChargeExchangeRates(IList<ExchangeRate> models, int contractId)
        {
            List<ExchangeRate> result = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                var currenciesValueNeedToFetch = models?.Where(x => x.CurrencyFrom != x.CurrencyTo).ToList();
                if (currenciesValueNeedToFetch != null && currenciesValueNeedToFetch.Any())
                {
                    if (contractId > 0)
                    {
                        var listContractExchangeRate = _exchangeRateRepository.GetContractExchangeRates(new List<int> { contractId }).Populate<IList<DomainContractModel.ContractExchangeRate>>();
                        if (listContractExchangeRate != null && listContractExchangeRate.Any())
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
                    var fetchResult = this._currencyExchangeRateService.GetMiiwaExchangeRates(exchangeCurrenciesWithOutRates).Result.Populate<List<ExchangeRate>>();
                    if (fetchResult != null && fetchResult.Any())
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
                this._logger.LogError(responseType.ToId(), _messageDescriptions[responseType.ToId()].ToString(), ex);
            }
            return new Response().ToPopulate(responseType, result, result?.Count);
        }
    }
}