using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Email.Domain.Interfaces.Email;
using Evolution.Email.Domain.Models;
using Evolution.Email.Models;
using Evolution.Home.Domain.Interfaces.Homes;
using Evolution.Home.Domain.Models.Homes;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
using Evolution.Security.Domain.Interfaces.Security;
using Evolution.Security.Domain.Models.Security;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Validations;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Evolution.Visit.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Net;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Threading.Tasks;
using System.Transactions;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Evolution.Visit.Domain.Models.Visits;


namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistService : ITechnicalSpecialistService
    {
        private readonly IAppLogger<TechnicalSpecialistService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ITechnicalSpecialistRepository _repository = null;
        private readonly JObject _messages = null;
        private readonly ITechnicalSpecialistValidationService _validationService = null;
        private readonly ICompanyService _companyService = null;
        private readonly IMasterService _masterService = null;
        private readonly ISubDivisionService _subDivisionService = null;
        private readonly IProfileActionService _profileActionService = null;
        private readonly IProfileStatusService _profileStatusService = null;
        private readonly IEmploymentTypeService _employmentService = null;
        private readonly ICompanyPayrollService _compPayrollService = null;
        private readonly ICountryService _countryService = null;
        private readonly IMyTaskService _myTaskService = null;
        private readonly IDocumentService _documentService = null;
        private readonly IUserService _userService = null;
        private readonly IMyTaskService _taskService = null;
        private readonly IEmailQueueService _emailService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly IVisitRepository _visitRepository = null;
        private readonly IMongoDocumentService _mongoDocumentService = null;

        #region Constructor
        public TechnicalSpecialistService(IMapper mapper,
                                              ITechnicalSpecialistRepository repository,
                                              IAppLogger<TechnicalSpecialistService> logger,
                                              ITechnicalSpecialistValidationService validationService,
                                              ICompanyService companyservice,
                                              IMasterService masterservice,
                                              ISubDivisionService subDivisionservice,
                                              IProfileActionService profileActionService,
                                              IProfileStatusService profileStatusService,
                                              IEmploymentTypeService employmentService,
                                              ICompanyPayrollService compPayrollService,
                                              ICountryService countryService,
                                              JObject messages,
                                              IMyTaskService myTaskService,
                                              IDocumentService documentService,
                                             IUserService userService,
                                              IMyTaskService taskService,
                                              IEmailQueueService emailService,
                                              IVisitRepository visitRepository,
                                              IOptions<AppEnvVariableBaseModel> environment,
                                                IAuditSearchService auditSearchService,
                                                IMongoDocumentService mongoDocumentService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _companyService = companyservice;
            _masterService = masterservice;
            _subDivisionService = subDivisionservice;
            _profileActionService = profileActionService;
            _profileStatusService = profileStatusService;
            _employmentService = employmentService;
            _compPayrollService = compPayrollService;
            _countryService = countryService;
            _messages = messages;
            _myTaskService = myTaskService;
            _documentService = documentService;
            _userService = userService;
            _taskService = taskService;
            _environment = environment.Value;
            _auditSearchService = auditSearchService;
            _emailService = emailService;
            _visitRepository = visitRepository;
            _mongoDocumentService = mongoDocumentService;
        }
        #endregion

        #region Public Methods

        #region Get
        public Response GetResourceBasicInfo(string companyCode, string logonName)
        {
            IList<BaseTechnicalSpecialistInfo> result = null;
            Exception exception = null;
            try
            {
                result = _repository.Search(companyCode, logonName);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyCode);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(BaseTechnicalSpecialistInfo searchModel)
        {
            IList<BaseTechnicalSpecialistInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<BaseTechnicalSpecialistInfo>>(_repository.Search(searchModel));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public async Task<Response> Get(SearchTechnicalSpecialist searchModel)
        {
            IList<TechnicalSpecialistSearchResult> result = null;
            List<int> tsIds = new List<int>();
            Exception exception = null;
            IList<string> mongoSearch = null;
            IList<DbModel.TechnicalSpecialist> tsDbResult = null;
            try
            {
                string[] includes = new string[]
                {
                    "Company",
                    "TechnicalSpecialistContact",
                    "TechnicalSpecialistContact.Country",
                    "TechnicalSpecialistContact.County",
                    "TechnicalSpecialistContact.City",
                    "TechnicalSpecialistTaxonomy",
                    "TechnicalSpecialistTaxonomy.TaxonomyCategory",
                    "TechnicalSpecialistTaxonomy.TaxonomyServices",
                    "TechnicalSpecialistTaxonomy.TaxonomySubCategory",
                    "ProfileStatus",
                    "EmploymentType"
                }; 
                var tsInfo = _mapper.Map<TechnicalSpecialistInfo>(searchModel); 

                //Mongo Doc Search
                if (!string.IsNullOrEmpty(searchModel.SearchDocumentType) || !string.IsNullOrEmpty(searchModel.DocumentSearchText))
                {
                    var evoMongoDocSearch = _mapper.Map<Document.Domain.Models.Document.EvolutionMongoDocumentSearch>(searchModel);
                    mongoSearch = await this._mongoDocumentService.SearchDistinctFieldAsync(evoMongoDocSearch);
                    if (mongoSearch != null && mongoSearch.Count > 0)
                    {
                        tsInfo.Epins = mongoSearch;
                        using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                        {
                            tsDbResult = _repository.Search(tsInfo, includes);
                            tranScope.Complete();
                        }
                        if (tsDbResult != null && tsDbResult.Count > 0)
                            tsDbResult = tsDbResult.Where(x => mongoSearch.Contains(x.Pin.ToString())).ToList();
                    }
                    else
                        result = new List<TechnicalSpecialistSearchResult>();
                } 
                else
                {
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                             new TransactionOptions
                                             {
                                                 IsolationLevel = IsolationLevel.ReadUncommitted
                                             }))
                    {
                        tsDbResult = _repository.Search(tsInfo, includes);
                    }
                }
                if (tsDbResult?.Count > 0)
                {
                    if (!string.IsNullOrEmpty(searchModel.Category))
                        tsDbResult = tsDbResult.Where(x => x.TechnicalSpecialistTaxonomy != null &&
                                                           x.TechnicalSpecialistTaxonomy.Any(x1 => x1.TaxonomyCategory.Name == searchModel.Category)).ToList();

                    if (!string.IsNullOrEmpty(searchModel.SubCategory))
                        tsDbResult = tsDbResult.Where(x => x.TechnicalSpecialistTaxonomy != null &&
                                                           x.TechnicalSpecialistTaxonomy.Any(x1 => x1.TaxonomySubCategory.TaxonomySubCategoryName == searchModel.SubCategory)).ToList();

                    if (!string.IsNullOrEmpty(searchModel.Service))
                        tsDbResult = tsDbResult.Where(x => x.TechnicalSpecialistTaxonomy != null &&
                                                           x.TechnicalSpecialistTaxonomy.Any(x1 => x1.TaxonomyServices.TaxonomyServiceName == searchModel.Service)).ToList();

                    if (!string.IsNullOrEmpty(searchModel.FullAddress))
                        tsDbResult = tsDbResult.Where(x => x.TechnicalSpecialistContact != null &&
                                                           x.TechnicalSpecialistContact.Any(x1 => !string.IsNullOrEmpty(x1.Address) && x1.Address.Contains(searchModel.FullAddress.Replace("*", string.Empty)) &&
                                                                                                  x1.ContactType == ContactType.PrimaryAddress.ToString())).ToList();
                    if (searchModel.CountryId.HasValue) //Added for ITK D1536
                        tsDbResult = tsDbResult.Where(x => x.TechnicalSpecialistContact != null &&
                                                           x.TechnicalSpecialistContact.Any(x1 => x1.Country != null &&
                                                                                                  x1.Country.Id == searchModel.CountryId &&
                                                                                                  x1.ContactType == ContactType.PrimaryAddress.ToString())).ToList();

                    if (searchModel.CountyId.HasValue) //Added for ITK D1536
                        tsDbResult = tsDbResult.Where(x => x.TechnicalSpecialistContact != null &&
                                                           x.TechnicalSpecialistContact.Any(x1 => x1.County != null &&
                                                                                                  x1.County.Id == searchModel.CountyId &&
                                                                                                  x1.ContactType == ContactType.PrimaryAddress.ToString())).ToList();

                    if (searchModel.CityId.HasValue) //Added for ITK D1536
                        tsDbResult = tsDbResult.Where(x => x.TechnicalSpecialistContact != null &&
                                                           x.TechnicalSpecialistContact.Any(x1 => x1.City != null &&
                                                                                                  x1.City.Id == searchModel.CityId &&
                                                                                                  x1.ContactType == ContactType.PrimaryAddress.ToString())).ToList();
                    if (!string.IsNullOrEmpty(searchModel.PinCode))
                        tsDbResult = tsDbResult.Where(x => x.TechnicalSpecialistContact != null &&
                                                           x.TechnicalSpecialistContact.Any(x1 => !string.IsNullOrEmpty(x1.PostalCode) && x1.PostalCode.Contains(searchModel.PinCode.Replace("*", string.Empty)) &&
                                                                                                  x1.ContactType == ContactType.PrimaryAddress.ToString())).ToList();

                    tsDbResult = tsDbResult.OrderBy(ts => ts.LastName).ToList();

                    result = _mapper.Map<IList<TechnicalSpecialistSearchResult>>(tsDbResult);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(TechnicalSpecialistInfo searchModel)
        {
            IList<TechnicalSpecialistInfo> result = null;
            Exception exception = null;
            try
            {
                var tsInfo = _mapper.Map<IList<TechnicalSpecialistInfo>>(_repository.Search(searchModel));
                var tsWithDoc = PopulateTsProfessionalDocument(tsInfo);
                // var tsWithCredInfo = PopulateTsLogInDetails(tsWithDoc); 
                // result = PopulateAssignedToTMDetails(tsWithCredInfo); //D946 :(AssignedTO Changes Handle in FrontEnd itself for All Profile Action)
                result = PopulateTsLogInDetails(tsWithDoc);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> pins)
        {
            IList<TechnicalSpecialistInfo> result = null;
            Exception exception = null;
            try
            {
                var tsInfo = _mapper.Map<IList<TechnicalSpecialistInfo>>(GetTechSpecialistByPin(pins));
                var tsWithDoc = PopulateTsProfessionalDocument(tsInfo);
                var tsWithCredInfo = PopulateTsLogInDetails(tsWithDoc);
                result = PopulateAssignedToTMDetails(tsWithCredInfo);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), pins);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public static string IntToLetters(int value)
        {
            string result = string.Empty;
            while (--value >= 0)
            {
                result = (char)('a' + value % 26) + result;
                value /= 26;
            }
            return result;
        }

        public Response GetTechnicalSpecialistChevronCV(long techspecialistEpin)
        {
            var data = _repository.FindBy((x => x.Pin == techspecialistEpin), new string[] { "TechnicalSpecialistContact", "TechnicalSpecialistEducationalQualification", "TechnicalSpecialistWorkHistory", "TechnicalSpecialistCommodityEquipmentKnowledge", "TechnicalSpecialistCertificationAndTraining", "TechnicalSpecialistTrainingAndCompetency" }).ToList();
            var tsData = data?.FirstOrDefault(ts => ts.Id == techspecialistEpin);
            var certificateTrainingData = data?.SelectMany(x => x.TechnicalSpecialistCertificationAndTraining)?.OrderBy(x => x.CertificationAndTraining?.Name)?.ToList();
            var equipmentData = data?.SelectMany(x => x.TechnicalSpecialistCommodityEquipmentKnowledge)?.ToList();
            var educationalData = data?.SelectMany(x => x.TechnicalSpecialistEducationalQualification)?.OrderByDescending(x => x.DateTo)?.ToList();
            var techContact = data?.SelectMany(x => x.TechnicalSpecialistContact)?.FirstOrDefault(x => x.CityId > 0);
            var trainingData = data?.SelectMany(x => x.TechnicalSpecialistTrainingAndCompetency)?.OrderBy(x => x.TechnicalSpecialistTrainingAndCompetencyType?.FirstOrDefault(s => s.TechnicalSpecialistTrainingAndCompetencyId == x?.Id)?.TrainingOrCompetencyData?.Name)?.ToList();
           // var workHistory = data?.SelectMany(x => x.TechnicalSpecialistWorkHistory)?.OrderByDescending(x => x.DateTo)?.OrderByDescending(y => y.DateFrom).ToList();
            var workHistoryData = data?.SelectMany(x => x.TechnicalSpecialistWorkHistory)?.OrderByDescending(x => x.DateTo)?.OrderByDescending(y => y.DateFrom)?.OrderBy(x => x.IsCurrentCompany == false).ToList();
            var compentency = data?.SelectMany(x => x.TechnicalSpecialistTrainingAndCompetency)?.Select(x => new KeyValuePair<string, object>(x.TechnicalSpecialistTrainingAndCompetencyType?.FirstOrDefault(x1 => x1.Id == x.Id)?.TrainingOrCompetencyData?.Name, x)).ToList();
            var training = data?.SelectMany(x => x.TechnicalSpecialistCertificationAndTraining)?.Select(x => new KeyValuePair<string, object>(x.CertificationAndTraining.Name, x)).ToList();
            var trainingCompentencyTrainingData = compentency?.Union(training).OrderBy(x => x.Key).ToList();
            StringBuilder educationalDivData = new StringBuilder();
            StringBuilder interTekHistoryDivData = new StringBuilder();
            Table interTekHistoryTable = new Table();
            char[] alphaCharList = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            educationalDivData.Append("<html><head></head><body>");

            //educationalDivData = string.Concat(educationalDivData, string.Format("<table style='width: 97%;' border='0'><tr><td style='text-align:center;'>{0}</td><td style='text-align:center;'>{1}</td></tr></table>", string.Concat(tsData?.FirstName, " ", tsData?.LastName), tsData?.DateOfBirth?.ToDateFormat("dd/MMM/yyyy")));

            //educationalDivData = string.Concat(educationalDivData, "<table style='width: 97%;' border='0'><tr><td style='text-align:left;'>QA-SPI-001 Attachment 4</td><td style='text-align:right;'>Rev 2</td></tr></table>");

            educationalDivData.Append("<table style='border-collapse:collapse;width:97%;'>");
            educationalDivData.Append("<tr><th colspan='3' style='border: 1px solid black;padding:4px;background-color:#CCCCCC;text-align:left;font-size:22px;'><b>1. Personal Data</b></th></tr>");
            educationalDivData.Append("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;width:4%;'>a </td>");
            educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'>Name</td>");
            educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'> {0} </td></tr>", string.Concat(tsData?.FirstName, " ", tsData?.LastName)?.ToUpper()));
            educationalDivData.Append("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;width:4%;'>b </td>");
            educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'>Nationality</td>");
            educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding:4px;text-align:left;'> {0} </td></tr>", techContact?.Country?.Name));
            educationalDivData.Append("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;width:4%;'>c </td>");
            educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'>Location (City)</td>");
            educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'> {0} </td></tr>", techContact?.City?.Name));
            educationalDivData.Append("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;width:4%;'>d </td>");
            educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'>Location (State)</td>");
            educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'> {0} </td></tr>", techContact?.County?.Name));
            educationalDivData.Append("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;width:4%;'>e </td>");
            educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'>Location (Country)</td>");
            educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'> {0} </td></tr></table>", techContact?.Country?.Name));

            //Educational Qualification Start
            educationalDivData.Append("<table style='border-collapse: collapse;width:97%;'>");
            educationalDivData.Append("<tr><th colspan='3' style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:left;font-size:22px;'><b>2. Educational Qualification</b></th></tr>");
            educationalDivData.Append("<tr><th colspan='2' style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:center;font-size:17px;width:49.7%;'><b>Institution &#38; Years of Attendance</b></th>");
            educationalDivData.Append("<th colspan='2' style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:center;font-size:17px;'><b>Qualification Achieved</b></th></tr>");
            if (educationalData != null && educationalData.Count > 0)
            {
                for (int i = 0; i < educationalData.Count; i++)
                {
                    educationalDivData.Append(string.Format("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;width:4%;'>{0}</td>", IntToLetters(i + 1)));
                    educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}, {1}</td>", WebUtility.HtmlEncode(educationalData[i]?.Institution), educationalData[i]?.DateTo?.Year));
                    educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td></tr>", WebUtility.HtmlEncode(educationalData[i]?.Qualification)));
                }
                educationalDivData.Append("</table>");
            }
            else
            {
                educationalDivData.Append("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td></tr>");
                educationalDivData.Append("</table>");
            }

            //Technical Certifications Start
            educationalDivData.Append("<table style='border-collapse: collapse;width:97%;'>");
            educationalDivData.Append("<tr><th colspan='5' style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:left;font-size:22px;'><b>3. Technical Certifications</b></th></tr>");
            educationalDivData.Append("<tr><th colspan='2' style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:center;font-size:17px;'><b>Certification</b></th>");
            educationalDivData.Append("<th style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:center;font-size:17px;'><b>Certificate Number</b></th>");
            educationalDivData.Append("<th style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:center;font-size:17px;'><b>Effective Date</b></th>");
            educationalDivData.Append("<th style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:center;font-size:17px;'><b>Expiry Date</b></th></tr>");

            if (certificateTrainingData != null && certificateTrainingData.Count > 0)
            {
                int count = 1;
                for (int i = 0; i < certificateTrainingData.Count; i++)
                {
                    //changes for D793 reffered email conversation
                    if (certificateTrainingData[i]?.RecordType == CompCertTrainingType.Ce.ToString())
                    {
                        educationalDivData.Append(string.Format("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;width:4%;'>{0}</td>", IntToLetters(count++)));
                        educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td>", certificateTrainingData[i]?.CertificationAndTraining?.Name?.Replace("&", "&#38;")));
                        educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td>", certificateTrainingData[i]?.CertificationAndTrainingRefId)?.Replace("&", "&#38;"));
                        educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td>", certificateTrainingData[i]?.EffeciveDate?.ToDateFormat("dd/MMM/yyyy")));
                        educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td></tr>", certificateTrainingData[i]?.ExpiryDate?.ToDateFormat("dd/MMM/yyyy")));
                    }
                }
                educationalDivData.Append("</table>");
            }
            else
            {
                educationalDivData.Append("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td></tr>");
                educationalDivData.Append("</table>");
            }

            //Training Section Start
            educationalDivData.Append("<table style='border-collapse: collapse;width:97%;'>");
            educationalDivData.Append("<tr><th colspan='4' style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:left;font-size:22px;'><b>4. Training</b></th></tr>");
            educationalDivData.Append("<tr><th colspan='2' style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:center;font-size:17px;'><b>Training</b></th>");
            educationalDivData.Append("<th style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:center;font-size:17px;'><b>Year(s) Attended</b></th>");
            educationalDivData.Append("<th style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:center;font-size:17px;'><b>Duration (days)</b></th></tr>");
            if (trainingCompentencyTrainingData != null && trainingCompentencyTrainingData.Count > 0)
            {
                int count = 1;
                for (int i = 0; i < trainingCompentencyTrainingData.Count; i++)
                {
                    if (trainingCompentencyTrainingData[i].Value is DbModel.TechnicalSpecialistTrainingAndCompetency)
                    {
                        var triData = (DbModel.TechnicalSpecialistTrainingAndCompetency)trainingCompentencyTrainingData[i].Value;
                        if (triData.RecordType == CompCertTrainingType.Co.ToString())
                        {

                            educationalDivData.Append(string.Format("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;width:4%;'>{0}</td>", IntToLetters(count++)));
                            educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td>", triData.TechnicalSpecialistTrainingAndCompetencyType?.FirstOrDefault(s => s.TechnicalSpecialistTrainingAndCompetencyId == triData.Id)?.TrainingOrCompetencyData?.Name)?.Replace("&", "&#38;"));
                            educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td>", triData?.TrainingDate?.Year));//D793 (Ref ALM Doc 10-03-2020)
                            educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td></tr>", triData?.Duration?.Replace("&", "&#38;")));
                        }

                    }
                    else if (trainingCompentencyTrainingData[i].Value is DbModel.TechnicalSpecialistCertificationAndTraining)
                    {
                        var certData = (DbModel.TechnicalSpecialistCertificationAndTraining)trainingCompentencyTrainingData[i].Value;
                        if (certData.RecordType == CompCertTrainingType.Tr.ToString())
                        {

                            educationalDivData.Append(string.Format("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;width:4%;'>{0}</td>", IntToLetters(count++)));
                            educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td>", certData?.CertificationAndTraining?.Name)?.Replace("&", "&#38;"));
                            educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td>", certData?.EffeciveDate?.Year));//D793 (Ref ALM Doc 10-03-2020)
                            educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td></tr>", certData?.Duration)?.Replace("&", "&#38;"));
                        }
                    }
                }
                educationalDivData.Append("</table>");
            }
            else
            {
                educationalDivData.Append("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td></tr>");
                educationalDivData.Append("</table>");
            }

            //Category of Inspector & High Level Summary of the specialization
            string ProfessionalSummary = tsData != null ? tsData.ProfessionalSummary : string.Empty;
            ProfessionalSummary = ProfessionalSummary.Replace("<ul>", "<ul><table border='0' style='border: 0px white solid' cellspacing='0' cellpadding='0'><tbody>");
            ProfessionalSummary = ProfessionalSummary.Replace("<ol>", "<ul><table border='0' style='border: 0px white solid' cellspacing='0' cellpadding='0'><tbody>");
            ProfessionalSummary = ProfessionalSummary.Replace("</ul>", "</tbody></table></ul>");
            ProfessionalSummary = ProfessionalSummary.Replace("</ol>", "</tbody></table></ul>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li>", "<tr><td><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("</li>", "</li></td></tr>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-1\">", "<tr><td style='padding-left: 4.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-2\">", "<tr><td style='padding-left: 7.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-3\">", "<tr><td style='padding-left: 10.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-4\">", "<tr><td style='padding-left: 13.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-5\">", "<tr><td style='padding-left: 16.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-6\">", "<tr><td style='padding-left: 19.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-7\">", "<tr><td style='padding-left: 22.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-8\">", "<tr><td style='padding-left: 25.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<ol>", "");
            string professionalBulletCharacter = string.Empty;
            educationalDivData.Append("<table style='border-collapse: collapse;width:97%;'><tr><th colspan='5' style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:left;font-size:22px;'><b>5. Category of Inspector &#38; High Level Summary of the specialization</b></th></tr>");
            if (CheckBulletContent(ProfessionalSummary, out professionalBulletCharacter))
            {
                string content = this.FormBulletContent(ProfessionalSummary, professionalBulletCharacter);
                educationalDivData.Append(string.Format("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td></tr></table>", content));
            }
            else
               educationalDivData.Append(string.Format("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td></tr></table>", WebUtility.HtmlDecode(ProfessionalSummary)?.Replace("&", "&#38;")));//PROD D 751 fix

            //Category of Inspector & High Level Summary Start
            educationalDivData.Append("<table style='border-collapse: collapse;width:97%;'>");
            educationalDivData.Append("<tr><th colspan='8' style='border: 1px solid black; padding: 4px; background-color:#CCCCCC;text-align:left;font-size:22px;'><b>6. Category of Inspector &#38; High Level Summary</b></th></tr>");
            educationalDivData.Append("<tr><th style='border: 1px solid black;  background-color:#CCCCCC;text-align:left;'><b>Period</b></th>");
            educationalDivData.Append("<th style='border: 1px solid black;  background-color:#CCCCCC;text-align:left;'><b>Inspection Company</b></th>");
            educationalDivData.Append("<th style='border: 1px solid black;  background-color:#CCCCCC;text-align:left;'><b>Customer</b></th>");
            educationalDivData.Append("<th style='border: 1px solid black;  background-color:#CCCCCC;text-align:left;'><b>Customer Project</b></th>");
            educationalDivData.Append("<th style='border: 1px solid black;  background-color:#CCCCCC;text-align:left;'><b>Supplier</b></th>");
            educationalDivData.Append("<th style='border: 1px solid black;  background-color:#CCCCCC;text-align:left;'><b>Supplier Location (City)</b></th>");
            educationalDivData.Append("<th style='border: 1px solid black;  background-color:#CCCCCC;text-align:left;'><b>Equipment Category Handled</b></th>");
            educationalDivData.Append("<th style='border: 1px solid black; background-color:#CCCCCC;text-align:left;'><b>Responsibility &#38; Inspection Details</b></th></tr>");

            if (workHistoryData != null && workHistoryData.Count > 0)
            {
                for (int i = 0; i < workHistoryData.Count; i++)
                {
                    var toDate = "Present";
                    if (workHistoryData[i]?.DateTo != null)
                    {
                        toDate = workHistoryData[i]?.DateTo?.ToDateFormat("dd/MMM/yyyy");
                    }
                    educationalDivData.Append(string.Format("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;width:35%;'>{0} - {1}</td>", workHistoryData[i]?.DateFrom?.ToDateFormat("dd/MMM/yyyy"), toDate));
                    educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td>", WebUtility.HtmlEncode(workHistoryData[i]?.ClientName)));
                    educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td>", WebUtility.HtmlEncode(workHistoryData[i]?.ClientName)));
                    educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td>", "Various"));
                    educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td>", "Various"));
                    educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td>", "Various"));
                    educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td>", ""));
                    educationalDivData.Append(string.Format("<td style='border: 1px solid black; padding: 4px;text-align:left;'>{0}</td></tr>", WebUtility.HtmlEncode(workHistoryData[i]?.JobTitle))); //changes for D793 reffered by ALM 08-02-2020 failed doc
                }
                educationalDivData.Append("</table>");
            }
            else
            {
                educationalDivData.Append("<tr><td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td>");
                educationalDivData.Append("<td style='border: 1px solid black; padding: 4px;text-align:left;'></td></tr>");
                educationalDivData.Append("</table>");
            }

            //Detailed Work Experience Start 
            educationalDivData.Append("<table style='border-collapse: collapse;width:97%;'><tr><th colspan='8' style='border: 1px solid black; background-color:#CCCCCC;text-align:left;padding: 4px;font-size:22px;'><b>7. Detailed Work Experience (free text to support the summary in Section 6)</b></th></tr></table>");

            educationalDivData.Append("<table style='border-collapse: collapse;width:97%;' border='0'><tr><th style='font-weight:bold;padding: 4px;text-align:left;width:25%;font-size:22px;'>PROFESSIONAL SUMMARY</th></tr>");

            if (workHistoryData != null && workHistoryData.Count > 0)
            {
                for (int i = 0; i < workHistoryData.Count; i++)
                {
                    var toDate = "Present";
                    if (workHistoryData[i]?.DateTo != null)
                       toDate = workHistoryData[i]?.DateTo?.ToDateFormat("dd/MMM/yyyy");
                    
                    educationalDivData.Append(string.Format("<tr><td style='font-weight:bold;text-align:left;width:97%;font-size:0.8rem;text-align:left;padding-top:20px;'>{0}</td></tr>", WebUtility.HtmlEncode(workHistoryData[i]?.ClientName)));
                    educationalDivData.Append(string.Format("<tr><td style='font-weight:bold;text-align:left;font-size:0.8rem;'>{0} to {1}</td></tr>", workHistoryData[i]?.DateFrom?.ToDateFormat("dd/MMM/yyyy"), toDate));
                    //educationalDivData.Append(string.Format("<tr><td style='font-weight:bold;text-align:left;font-size:0.8rem;padding-bottom:20px;'>{0}</td></tr>", WebUtility.HtmlEncode(workHistoryData[i]?.JobTitle)));
                    //educationalDivData.Append(string.Format("<tr><td style='text-align:left;'>{0}</td></tr>", WebUtility.HtmlDecode(workHistoryData[i]?.JobDescription)?.Replace("&", "&#38;")));//PROD def751 fix

                    string Description = WebUtility.HtmlDecode(workHistoryData[i]?.JobDescription)?.Replace("&", "&#38;"); //PROD D751 fix
                    Description = Description.Replace("<ul>", "<ul><table border='0' style='border: 0px white solid' cellspacing='0' cellpadding='0'><tbody>");
                    Description = Description.Replace("<ol>", "<ul><table border='0' style='border: 0px white solid' cellspacing='0' cellpadding='0'><tbody>");
                    Description = Description.Replace("</ul>", "</tbody></table></ul>");
                    Description = Description.Replace("</ol>", "</tbody></table></ul>");
                    Description = Description.Replace("<li>", "<tr><td><li>");
                    Description = Description.Replace("</li>", "</li></td></tr>");
                    Description = Description.Replace("<li class=\"ql-indent-1\">", "<tr><td style='padding-left: 4.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-2\">", "<tr><td style='padding-left: 7.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-3\">", "<tr><td style='padding-left: 10.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-4\">", "<tr><td style='padding-left: 13.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-5\">", "<tr><td style='padding-left: 16.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-6\">", "<tr><td style='padding-left: 19.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-7\">", "<tr><td style='padding-left: 22.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-8\">", "<tr><td style='padding-left: 25.5em;border: 1px white solid'><li>");

                    Description = !string.IsNullOrEmpty(Description) ? Description.TrimStart(Environment.NewLine.ToCharArray()) : "";
                    string firstThreeCharacters = !string.IsNullOrEmpty(Description) && Description?.Length > 3 ? Description.Substring(0, 3) : ""; //Changes for Live D734
                    if (!string.IsNullOrEmpty(Description) && firstThreeCharacters.ToLower().Equals("<p>"))
                        educationalDivData.Append(string.Format("<tr><td style='text-align:left;font-size:0.8rem;padding-bottom:20px;'><u><span style='font-weight:bold;'>{0}</span></u>{1}</td></tr>", WebUtility.HtmlEncode(workHistoryData[i]?.JobTitle), Description));
                    else
                        educationalDivData.Append(string.Format("<tr><td style='text-align:left;font-size:0.8rem;padding-bottom:20px;'><u><span style='font-weight:bold;'>{0}</span></u><p style='margin-top: 5px; '>{1}</p></td></tr>", WebUtility.HtmlEncode(workHistoryData[i]?.JobTitle), Description));//def IGOQC 915 fix

                    if (i < (workHistoryData.Count - 1))
                        educationalDivData.Append("<tr><td style='text-align:left;vertical-align:top;width:97%;'></td></tr>");
                    ////string description = WebUtility.HtmlEncode(workHistoryData[i]?.JobDescription);
                    ////if (!string.IsNullOrEmpty(description))
                    ////{
                    ////    string descriptionBulletCharacter = string.Empty;
                    ////    if (CheckBulletContent(description, out descriptionBulletCharacter))
                    ////    {
                    ////        string content = this.FormatContent(description, descriptionBulletCharacter);
                    ////        educationalDivData.Append(content);
                    ////    }
                    ////    else
                    ////    {
                    ////        educationalDivData.Append(string.Format("<tr><td style='text-align:left;'>{0}</td></tr>", description));
                    ////    }
                    ////}
                }
                educationalDivData.Append("</table>");
            }
            else
            {
                educationalDivData.Append("<tr><td style='font-weight:bold;text-align:left;width:97%;font-size:0.8rem;text-align:left;'></td></tr>");
                educationalDivData.Append("<tr><td style='font-weight:bold;text-align:left;font-size:0.8rem;'></td><td style='font-weight:bold;text-align:left;width:25%;font-size:0.8rem;'></td></tr>");
                educationalDivData.Append("<tr><td style='text-align:left;'></td></tr>");
                educationalDivData.Append("<tr><td style='text-align:left;'></td></tr>");
                educationalDivData.Append("</table>");
            }


            educationalDivData.Append("</body></html>");
            //D793
            //byte[] documentBytes = Common.Helpers.Utility.HtmlToWord(educationalDivData.ToString(), TechnicalSpecialistConstants.Chevron_Header_Text, string.Concat(tsData?.FirstName, " ", tsData?.LastName), tsData?.DateOfBirth?.ToDateFormat("dd/MMM/yyyy"), string.Concat(tsData?.Company?.Name, " ", tsData?.Company?.OperatingCountry), true);
            byte[] documentBytes = Common.Helpers.Utility.HtmlToWord(educationalDivData.ToString(),interTekHistoryDivData.Append(" ").ToString(),TechnicalSpecialistConstants.Chevron_Header_Text, string.Concat(tsData?.FirstName, " ", tsData?.LastName), DateTime.UtcNow.ToDateFormat("dd/MMM/yyyy"), string.Concat(tsData?.Company?.Name, " ", tsData?.Company?.OperatingCountry), tsData?.Company?.OperatingCountryNavigation?.Name, true);
            return new Response().ToPopulate(ResponseType.Success, null, null, null, documentBytes, null, null);
        }

        public Response GetTechnicalSpecialistExportCV(long techspecialistEpin)
        {
            var data = _repository.FindBy((x => x.Pin == techspecialistEpin), new string[] { "TechnicalSpecialistContact", "TechnicalSpecialistEducationalQualification", "TechnicalSpecialistWorkHistory", "TechnicalSpecialistLanguageCapability", "TechnicalSpecialistCertificationAndTraining", "TechnicalSpecialistTrainingAndCompetency", "TechnicalSpecialistCommodityEquipmentKnowledge" }).ToList();
            var tsData = data?.FirstOrDefault(ts => ts.Id == techspecialistEpin);
            var certificateTrainingData = data?.SelectMany(x => x.TechnicalSpecialistCertificationAndTraining)?.ToList();
            var equipmentData = data?.SelectMany(x => x.TechnicalSpecialistCommodityEquipmentKnowledge)?.ToList();
            var educationalData = data?.SelectMany(x => x.TechnicalSpecialistEducationalQualification)?.ToList();
            var techContact = data?.SelectMany(x => x.TechnicalSpecialistContact)?.FirstOrDefault(x => x.CityId > 0);
            var trainingData = data?.SelectMany(x => x.TechnicalSpecialistTrainingAndCompetency)?.ToList();
            var _workHistoryData = data?.SelectMany(x => x.TechnicalSpecialistWorkHistory)?.ToList();
          //  var workHistory = _workHistoryData != null ? _workHistoryData.OrderByDescending(a => a.DateTo)?.OrderByDescending(b => b.DateFrom).ToList() : _workHistoryData;
            var workHistoryData = _workHistoryData != null ? _workHistoryData.OrderByDescending(x => x.DateTo)?.OrderByDescending(y => y.DateFrom)?.OrderBy(x => x.IsCurrentCompany == false).ToList() : _workHistoryData;
            var langHistoryData = data?.SelectMany(x => x.TechnicalSpecialistLanguageCapability)?.ToList();
            var codeandStandards = data?.SelectMany(x => x.TechnicalSpecialistCodeAndStandard)?.ToList(); //D793
            var intertekWorkHistoryReportData = _visitRepository.GetStandardCVIntertekWorkHistoryReport(Convert.ToInt32(techspecialistEpin));

            StringBuilder educationalDivData = new StringBuilder();
            StringBuilder interTekHistoryDivData = new StringBuilder();
             Table interTekHistoryTable = new Table();
            educationalDivData.Append("<html><head></head><body style='border: 0.5rem solid #9c9107;padding: 1rem;'>");

            //educationalDivData = string.Concat(educationalDivData, string.Format("<table style='border-collapse: collapse;width: 100%; margin-bottom:1rem' border='0'><tr>{0}<td style='border: 1px solid black; padding: 8px; text-align:right; vertical-align:middle'><h3>CURRICULUM VITAE</h3></td></tr></table>", string.Empty));

            educationalDivData.Append(string.Format(@"<table style='width: 97%;' border='0' align='center'><tr>
            <th style='font-weight:bold;padding: 8px;font-size: 15pt;text-decoration:underline;'>{0}</th></tr>
            </table>", string.Concat(tsData?.FirstName, " ", tsData?.LastName)?.ToUpper()));

            string lstrTwoColwith3ColSpan = @"<tr> 
                                                <td style='font-weight:bold;width:20%;text-align:left'>{0}</td> 
                                                <td colspan='4'style='padding:8px;text-align:left;'>{1}</td> 
                                              </tr>";
            string lstrFiveColumnHeader = @"<tr> 
                                                <td style='font-weight:bold;text-align:left;vertical-align:top;'>{0}</td> 
                                                <td style='font-weight:bold;text-align:left;vertical-align:top;'>{1}</td> 
                                                <td style='font-weight:bold;text-align:left;vertical-align:top;'>{2}</td>
                                                <td style='font-weight:bold;text-align:left;vertical-align:top;'>{3}</td>
                                                <td style='font-weight:bold;text-align:left;vertical-align:top;'>{4}</td>
                                          </tr>";
            string lstrEmptyRow = @"<tr> 
                                          <td style='font-weight:bold;text-align:left;vertical-align:top;width:20%;'></td> 
                                          <td style='font-weight:bold;text-align:left;vertical-align:top;width:20%;'></td> 
                                          <td style='font-weight:bold;text-align:left;vertical-align:top;width:20%;'></td>
                                          <td style='font-weight:bold;text-align:left;vertical-align:top;width:20%;'></td>
                                          <td style='font-weight:bold;text-align:left;vertical-align:top;width:20%;'></td>
                                      </tr>";

            string lstrEmptyCell = @"<td style='width:25%;text-align:left;vertical-align:top;'></td>";

            string ProfessionalSummary = tsData != null ? tsData.ProfessionalSummary : string.Empty; //PROD D751 fix
            ProfessionalSummary = ProfessionalSummary.Replace("<ul>", "<ul><table border='0' style='border: 0px white solid' cellspacing='0' cellpadding='0'><tbody>");
            ProfessionalSummary = ProfessionalSummary.Replace("<ol>", "<ul><table border='0' style='border: 0px white solid' cellspacing='0' cellpadding='0'><tbody>");
            ProfessionalSummary = ProfessionalSummary.Replace("</ul>", "</tbody></table></ul>");
            ProfessionalSummary = ProfessionalSummary.Replace("</ol>", "</tbody></table></ul>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li>", "<tr><td><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("</li>", "</li></td></tr>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-1\">", "<tr><td style='padding-left: 4.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-2\">", "<tr><td style='padding-left: 7.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-3\">", "<tr><td style='padding-left: 10.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-4\">", "<tr><td style='padding-left: 13.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-5\">", "<tr><td style='padding-left: 16.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-6\">", "<tr><td style='padding-left: 19.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-7\">", "<tr><td style='padding-left: 22.5em;border: 1px white solid'><li>");
            ProfessionalSummary = ProfessionalSummary.Replace("<li class=\"ql-indent-8\">", "<tr><td style='padding-left: 25.5em;border: 1px white solid'><li>");
            string bulletCharacter = string.Empty;
            if (CheckBulletContent(ProfessionalSummary, out bulletCharacter))
            {
                educationalDivData.Append("<table style='width: 97%;' border='0' align='center'><tr><td style='font-weight:bold;width:20%;text-align:left'>SUMMARY</td></tr></table>");
                string contentProfessionalSummary = this.FormTableContent(ProfessionalSummary, bulletCharacter);
                educationalDivData.Append(string.Format("<table style='width: 97%;border-collapse: collapse;' border='0'>{0}</table>", contentProfessionalSummary));
            }
            else
            {
                educationalDivData.Append("<table style='width: 97%;' border='0'><tbody><tr><td style='font-weight:bold;width:20%;text-align:left'>SUMMARY</td></tr>");
                educationalDivData.Append(string.Format("<tr><td style='padding: 8px;text-align:left;'>{0}</td></tr></tbody></table>", WebUtility.HtmlDecode(ProfessionalSummary)?.Replace("&", "&#38;")));//PROD D751 fix
            }

            /* Starting of Table */

            educationalDivData.AppendLine("<table style='width: 97%;' border='0'>");
            if (techContact != null)
            {
                educationalDivData.Append(string.Format(lstrTwoColwith3ColSpan, "LOCATION", string.Format("{0}, {1}, {2}", techContact?.City?.Name, techContact?.County?.Name, techContact?.Country?.Name)));
            }
            if (educationalData != null && educationalData.Count > 0)
            {
                if(educationalData.Count == 1)
                    educationalDivData.Append(string.Format(lstrTwoColwith3ColSpan, "EDUCATION", string.Format("{0}, {1}, {2}", WebUtility.HtmlEncode(educationalData[0]?.Qualification), WebUtility.HtmlEncode(educationalData[0]?.Institution), educationalData[0]?.DateTo?.Year)));
                else
                {
                    string educationData = string.Empty;
                    for (int i = 0; i < educationalData.Count; i++)
                    {
                        educationData += string.Format("{0}, {1}, {2}", WebUtility.HtmlEncode(educationalData[i]?.Qualification), WebUtility.HtmlEncode(educationalData[i]?.Institution), educationalData[i]?.DateTo?.Year) + "<br />";
                    }
                    educationalDivData.Append("<tr><td style='font-weight:bold;text-align:left;vertical-align:top;'>EDUCATION</td>");
                    string finalEducationData = educationData.Substring(0, educationData.LastIndexOf("<br />"));
                    educationalDivData.Append(string.Format("<td colspan='4' style='width:20%;text-align:left;vertical-align:top;'>{0}</td>", finalEducationData));
                    educationalDivData.Append(string.Format("</tr>"));
                    //educationalDivData.Append(string.Format(lstrTwoColwith3ColSpan, "EDUCATION", educationData));
                }
            }
            if (langHistoryData != null && langHistoryData.Count > 0)
            {
                educationalDivData.Append(lstrEmptyRow);
                educationalDivData.Append(string.Format(lstrFiveColumnHeader, "LANGUAGES", "Languages", "Speaking Capability", "Writing Capability", "Comprehension Capability"));
                for (int i = 0; i < langHistoryData.Count; i++)
                {
                    educationalDivData.Append(string.Format("<tr>{0}", lstrEmptyCell));
                    educationalDivData.Append(string.Format("<td style='text-align:left;vertical-align:top;'>{0}</td>", WebUtility.HtmlEncode(langHistoryData[i]?.Language?.Name)));
                    educationalDivData.Append(string.Format("<td style='text-align:left;vertical-align:top;'>{0}</td>", langHistoryData[i]?.SpeakingCapabilityLevel));
                    educationalDivData.Append(string.Format("<td style='text-align:left;vertical-align:top;'>{0}</td>", langHistoryData[i]?.WritingCapabilityLevel));
                    educationalDivData.Append(string.Format("<td style='text-align:left;vertical-align:top;'>{0}</td></tr>", langHistoryData[i]?.ComprehensionCapabilityLevel));
                }
            }

            if (certificateTrainingData != null && certificateTrainingData.Count > 0)
            {
                educationalDivData.Append(lstrEmptyRow);
                educationalDivData.Append(string.Format(lstrFiveColumnHeader, 
                        "PROFESSIONAL QUALIFICATIONS", "Certification Details", "Verification Status", "Expiry Date",""));
                for (int i = 0; i < certificateTrainingData.Count; i++)
                {
                    if (certificateTrainingData[i]?.RecordType == CompCertTrainingType.Ce.ToString())
                    {
                        educationalDivData.Append(string.Format("<tr>{0}", lstrEmptyCell));
                        educationalDivData.Append(string.Format("<td style='text-align:left;vertical-align:top;'>{0} {1}</td>", certificateTrainingData[i]?.CertificationAndTraining?.Name?.Replace("&", "&#38;"),
                            string.IsNullOrEmpty(certificateTrainingData[i]?.CertificationAndTrainingRefId) ? "" : "#" + certificateTrainingData[i]?.CertificationAndTrainingRefId?.Replace("&", "&#38;")));                                                                                                                                                                                                                                 //CR#4 ME issue CertificateID Issue
                        educationalDivData.Append(string.Format("<td style='text-align:left;vertical-align:top;'>{0}</td>", certificateTrainingData[i]?.VerificationStatus));
                        educationalDivData.Append(string.Format("<td style='text-align:left;vertical-align:top;'>{0}</td>", certificateTrainingData[i]?.ExpiryDate?.ToDateFormat("dd/MMM/yyyy")));
                        educationalDivData.Append(string.Format("{0}</tr>", lstrEmptyCell));
                    }
                }
            }
            
            if (certificateTrainingData != null && certificateTrainingData.Count > 0)
            {
                var trainingCertificateData = certificateTrainingData?.Select(x => x.RecordType == CompCertTrainingType.Tr.ToString()).ToList();
                if (trainingCertificateData != null && trainingCertificateData.Count > 0)
                {
                    educationalDivData.Append(lstrEmptyRow);
                    educationalDivData.Append(string.Format(lstrFiveColumnHeader,
                        "", "Training Details", "", "", ""));
                    //educationalDivData.Append("<tr><td style='font-weight:bold;text-align:left;vertical-align: top;'></td><td colspan='4' style='font-weight:bold;width:25%;text-align:left;'></td></tr>");
                    for (int i = 0; i < certificateTrainingData.Count; i++)
                    {
                        if (certificateTrainingData[i]?.RecordType == CompCertTrainingType.Tr.ToString())
                        {
                            educationalDivData.Append(string.Format("<tr>{0}", lstrEmptyCell));
                            educationalDivData.Append(string.Format("<td style='width:20%;text-align:left;vertical-align:top;'>{0} {1}</td>", certificateTrainingData[i]?.CertificationAndTraining?.Name?.Replace("&", "&#38;"), 
                                string.IsNullOrEmpty(certificateTrainingData[i]?.CertificationAndTrainingRefId) ? "" : "#" + certificateTrainingData[i]?.CertificationAndTrainingRefId?.Replace("&", "&#38;")));  //CR#4 ME issue TrainingID Issue;
                            educationalDivData.Append(string.Format("{0}", lstrEmptyCell));
                            educationalDivData.Append(string.Format("{0}", lstrEmptyCell));
                            educationalDivData.Append(string.Format("{0}</tr>", lstrEmptyCell));
                        }
                    }
                    educationalDivData.Append(lstrEmptyRow);
                }
            }
            
            if (equipmentData != null && equipmentData.Count > 0)
            {
                educationalDivData.Append("<tr><td style='font-weight:bold;text-align:left;vertical-align:top;'>EQUIPMENT</td>");
                List<string> equipmentlist = equipmentData?.Select(x => WebUtility.HtmlEncode(x?.EquipmentKnowledge?.Name))?.ToList();
                educationalDivData.Append(string.Format("<td colspan='4' style='width:20%;text-align:left;vertical-align:top;'>{0}</td>", string.Join(", ", equipmentlist)));
                //educationalDivData.Append(string.Format("{0}", lstrEmptyCell));
                //educationalDivData.Append(string.Format("{0}", lstrEmptyCell));
                //educationalDivData.Append(string.Format("{0}</tr>", lstrEmptyCell));
                educationalDivData.Append(string.Format("</tr>"));
                //educationalDivData.Append(string.Format("<td colspan='4' style='text-align:left;vertical-align:top;'>{0}</td></tr>", string.Join(", ", equipmentlist)));
            }
            
            //Codes And Standards Start
            if (codeandStandards != null && codeandStandards.Count > 0)
            {
                educationalDivData.Append(lstrEmptyRow);
                educationalDivData.Append("<tr><td style='font-weight:bold;text-align:left;vertical-align:top;'>CODES/STANDARDS</td>");
                List<string> codeList = codeandStandards?.Select(x => WebUtility.HtmlEncode(x?.CodeStandard?.Name)).ToList();
                educationalDivData.Append(string.Format("<td colspan='4' style='width:20%;text-align:left;vertical-align:top;'>{0}</td>", string.Join(", ", codeList)));
                //educationalDivData.Append(string.Format("{0}", lstrEmptyCell));
                //educationalDivData.Append(string.Format("{0}", lstrEmptyCell));
                //educationalDivData.Append(string.Format("{0}</tr>", lstrEmptyCell));
                educationalDivData.Append(string.Format("</tr>"));
                //educationalDivData.Append(string.Format("<td colspan='4' style='text-align:left;vertical-align:top;'>{0}</td></tr>", string.Join(", ", codeList)));
            }

            //PROFESSIONAL AFFILIATIONS
            if (tsData?.ProfessionalAfiliation != null)
            {
                string content = WebUtility.HtmlEncode(tsData?.ProfessionalAfiliation);
                string breakRequired = content.Length < 89  ? "<br/>" : "";
                if(string.IsNullOrEmpty(breakRequired))
                 educationalDivData.Append(lstrEmptyRow);
                educationalDivData.Append(string.Format("<tr><td style='width:100px;font-weight:bold;text-align:left;vertical-align:top;'>{0}PROFESSIONAL AFFILIATIONS</td>", breakRequired));
                educationalDivData.Append(string.Format("<td colspan='4' style='width:20%;text-align:left;vertical-align:top;'>{0}</td>", content));
                educationalDivData.Append(string.Format("</tr>"));
            }

            educationalDivData.Append("</table>");
            if (workHistoryData != null && workHistoryData.Count > 0)
            {
                educationalDivData.Append("<table style='width: 97%;' border='0'><tr><th style='font-weight:bold;text-align:left;'>PROFESSIONAL EXPERIENCE</th></tr>");
                educationalDivData.Append("<tr><td style='text-align:left;vertical-align:top;width:97%;'></td></tr>");
                for (int i = 0; i < workHistoryData.Count; i++)
                {
                    var toDate = "Present";
                    if (workHistoryData[i].DateTo != null)
                    
                       toDate = workHistoryData[i]?.DateTo?.ToDateFormat("dd/MMM/yyyy");
                    educationalDivData.Append(string.Format("<tr><td style='font-weight:bold;text-align:left;width:97%;font-size:0.8rem;text-align:left;'>{0}</td></tr>", workHistoryData[i].ClientName?.Replace("&", "&#38;")));
                    educationalDivData.Append(string.Format("<tr><td style='font-weight:bold;text-align:left;font-size:0.8rem;text-align:left;'>{0} to {1}</td></tr>", workHistoryData[i]?.DateFrom?.ToDateFormat("dd/MMM/yyyy"), toDate));

                    string Description = WebUtility.HtmlDecode(workHistoryData[i]?.JobDescription)?.Replace("&", "&#38;"); //PROD D751 fix
                    Description = Description.Replace("<ul>", "<ul><table border='0' style='border: 0px white solid' cellspacing='0' cellpadding='0'><tbody>");
                    Description = Description.Replace("<ol>", "<ul><table border='0' style='border: 0px white solid' cellspacing='0' cellpadding='0'><tbody>");
                    Description = Description.Replace("</ul>", "</tbody></table></ul>");
                    Description = Description.Replace("</ol>", "</tbody></table></ul>");
                    Description = Description.Replace("<li>", "<tr><td><li>");
                    Description = Description.Replace("</li>", "</li></td></tr>");
                    Description = Description.Replace("<li class=\"ql-indent-1\">", "<tr><td style='padding-left: 4.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-2\">", "<tr><td style='padding-left: 7.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-3\">", "<tr><td style='padding-left: 10.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-4\">", "<tr><td style='padding-left: 13.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-5\">", "<tr><td style='padding-left: 16.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-6\">", "<tr><td style='padding-left: 19.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-7\">", "<tr><td style='padding-left: 22.5em;border: 1px white solid'><li>");
                    Description = Description.Replace("<li class=\"ql-indent-8\">", "<tr><td style='padding-left: 25.5em;border: 1px white solid'><li>");

                    Description = !string.IsNullOrEmpty(Description) ? Description.TrimStart(Environment.NewLine.ToCharArray()) : "";
                    string firstThreeCharacters = !string.IsNullOrEmpty(Description) && Description?.Length > 3 ? Description.Substring(0, 3): ""; //Changes for Live D734
                    if (!string.IsNullOrEmpty(Description) && firstThreeCharacters.ToLower().Equals("<p>"))                    
                       educationalDivData.Append(string.Format("<tr><td style='text-align:left;font-size:0.8rem;padding-bottom:20px;'><u><span style='font-weight:bold;'>{0}</span></u>{1}</td></tr>", WebUtility.HtmlEncode(workHistoryData[i]?.JobTitle), Description));
                    else
                        educationalDivData.Append(string.Format("<tr><td style='text-align:left;font-size:0.8rem;padding-bottom:20px;'><u><span style='font-weight:bold;'>{0}</span></u><p style='margin-top: 5px; '>{1}</p></td></tr>", WebUtility.HtmlEncode(workHistoryData[i]?.JobTitle), Description));//def IGOQC 915 fix

                    if (i < (workHistoryData.Count - 1))
                       educationalDivData.Append("<tr><td style='text-align:left;vertical-align:top;width:97%;'></td></tr>");
                    
                    //educationalDivData.Append(string.Format("<tr><td style='text-align:left;'>{0}</td></tr>", ));
                    // Below code added as part of D1187 formatting bullet points
                    ////string description = WebUtility.HtmlEncode(workHistoryData[i]?.JobDescription);
                    ////if (!string.IsNullOrEmpty(description))
                    ////{
                    ////    string descriptionBulletCharacter = string.Empty;
                    ////    if (CheckBulletContent(description, out descriptionBulletCharacter))
                    ////    {
                    ////        string content = this.FormatContent(description, descriptionBulletCharacter);
                    ////        educationalDivData.Append(content);
                    ////    }
                    ////    else
                    ////    {
                    ////        educationalDivData.Append(string.Format("<tr><td style='text-align:left;'>{0}</td></tr>", description));
                    ////    }
                    ////}
                }
                educationalDivData.Append("</table>");
            }
            
            if (intertekWorkHistoryReportData != null && intertekWorkHistoryReportData?.Count > 0)
            {

                 var uniqueIntertekWorkHistoryReportData = intertekWorkHistoryReportData?.GroupBy(x => new { x.SupplierName, x.SupplierCity, x.SupplierCounty, x.SupplierPostalCode, x.SupplierCountry, x.InspectedEquipment, x.Client })?.Select(x => x.Key).ToList();

                interTekHistoryDivData.Append("<html><body>");
                interTekHistoryDivData.Append("<p style='text-align:center;font-weight:bold;font-size:19px;'>INTERTEK WORK HISTORY</p>");             
                interTekHistoryDivData.Append("<table style='border-collapse:collapse;width:97%;' align='center' border='0'>");
                interTekHistoryDivData.Append("<tr style='border:1px; solid: black;border-style: dashed'><th style='border-top-style: solid;border-left-style: solid;border-bottom-style: solid;border-right-style: none;border-width: thin;text-align:left;font-weight:bold;background-color:#fbca1a;'>Manufacturer</th>");
                interTekHistoryDivData.Append("<th style='border-top-style: solid;border-bottom-style: solid;border-right-style: none;border-left-style: none;border-width: thin;text-align:left;font-weight:bold;background-color:#fbca1a;'>Location</th>");
                interTekHistoryDivData.Append("<th style='border-top-style: solid;border-bottom-style: solid;text-align:left;border-right-style: none;border-left-style: none;border-width: thin;font-weight:bold;background-color:#fbca1a;'>Inspected Equipment</th>");
                interTekHistoryDivData.Append("<th style='border-top-style: solid;border-bottom-style: solid;border-right-style: solid;text-align:left;border-left-style: none;border-width: thin;font-weight:bold;background-color:#fbca1a;'>Client</th></tr>");
                for (int i = 0; i < uniqueIntertekWorkHistoryReportData?.Count; i++)
                {
                    interTekHistoryDivData.Append(string.Format("<tr style='border:1px; solid: black;border-style: dashed'><td style='border-top-style: solid;border-left-style: solid;border-bottom-style: solid;border-right-style: none;border-width: thin;padding: 8px;text-align:left;'>{0}</td>", WebUtility.HtmlEncode(uniqueIntertekWorkHistoryReportData[i]?.SupplierName)));
                    interTekHistoryDivData.Append(string.Format("<td style='border-top-style: solid;border-left-style: none;border-bottom-style: solid;border-right-style: none;border-width: thin;padding: 8px;text-align:left;'>{0}</td>", (!string.IsNullOrEmpty(uniqueIntertekWorkHistoryReportData[i]?.SupplierCity) ? (uniqueIntertekWorkHistoryReportData[i]?.SupplierCity + ", ") : string.Empty) + (!string.IsNullOrEmpty(uniqueIntertekWorkHistoryReportData[i]?.SupplierCounty) ? (uniqueIntertekWorkHistoryReportData[i]?.SupplierCounty + ", ") : string.Empty) + (!string.IsNullOrEmpty(uniqueIntertekWorkHistoryReportData[i]?.SupplierPostalCode?.Trim()) ? (uniqueIntertekWorkHistoryReportData[i]?.SupplierPostalCode + ", ") : string.Empty) + (!string.IsNullOrEmpty(uniqueIntertekWorkHistoryReportData[i]?.SupplierCountry) ? (uniqueIntertekWorkHistoryReportData[i]?.SupplierCountry) : string.Empty)));
                    interTekHistoryDivData.Append(string.Format("<td style='border-top-style: solid;border-left-style: none;border-bottom-style: solid;border-right-style: none;border-width: thin;padding: 8px;text-align:left;'>{0}</td>", uniqueIntertekWorkHistoryReportData[i]?.InspectedEquipment));
                    interTekHistoryDivData.Append(string.Format("<td style='border-top-style: solid;border-left-style: none;border-bottom-style: solid;border-right-style: solid;border-width: thin;padding: 8px;text-align:left;'>{0}</td></tr>", WebUtility.HtmlEncode(uniqueIntertekWorkHistoryReportData[i]?.Client)));
                }
                interTekHistoryDivData.Append("</table>");
                interTekHistoryDivData.Append("</body></html>");
            }
            educationalDivData.Append("</body></html>");



            //D793
            // byte[] documentBytes = Common.Helpers.Utility.HtmlToWord(educationalDivData.ToString(), TechnicalSpecialistConstants.Header_Text, string.Concat(tsData?.FirstName, " ", tsData?.LastName), tsData?.DateOfBirth?.ToDateFormat("dd/MMM/yyyy"), string.Concat(tsData?.Company?.Name, " ", tsData?.Company?.OperatingCountry), false);
            byte[] documentBytes = Common.Helpers.Utility.HtmlToWord(educationalDivData.ToString(),interTekHistoryDivData.ToString(),TechnicalSpecialistConstants.Header_Text, string.Concat(tsData?.FirstName, " ", tsData?.LastName), tsData?.DateOfBirth?.ToDateFormat("dd/MMM/yyyy"), tsData?.Company?.Name, tsData?.Company?.OperatingCountryNavigation?.Name, false);
            return new Response().ToPopulate(ResponseType.Success, null, null, null, documentBytes, null, null);
        }
        
        private bool CheckBulletContent(string description, out string bulletCharacter)
        {
            bulletCharacter = string.Empty;
            bool isBullet = false;
            if (!string.IsNullOrEmpty(description) && description.Contains("•")) //string.IsNullOrEmpty For ITK QC D1334
            {
                bulletCharacter = "•";
                isBullet = true;
            }
            else if (!string.IsNullOrEmpty(description) && description.Contains("▪"))
            {
                bulletCharacter = "▪";
                isBullet = true;
            }
            else if (!string.IsNullOrEmpty(description) && description.Contains("●"))
            {
                bulletCharacter = "●";
                isBullet = true;
            }
            else if (!string.IsNullOrEmpty(description) && description.Contains("✓"))
            {
                bulletCharacter = "✓";
                isBullet = true;
            }
            else if (!string.IsNullOrEmpty(description) && description.Contains("✔"))
            {
                bulletCharacter = "✔";
                isBullet = true;
            }
            else if (!string.IsNullOrEmpty(description) && description.Contains("☑"))
            {
                bulletCharacter = "☑";
                isBullet = true;
            }
            else if (!string.IsNullOrEmpty(description) && description.Contains("○"))
            {
                bulletCharacter = "○";
                isBullet = true;
            }
            else if (!string.IsNullOrEmpty(description) && description.Contains("❖"))
            {
                bulletCharacter = "❖";
                isBullet = true;
            }
            else if (!string.IsNullOrEmpty(description) && description.Contains("◆"))
            {
                bulletCharacter = "◆";
                isBullet = true;
            }
            else if (!string.IsNullOrEmpty(description) && description.Contains("◈"))
            {
                bulletCharacter = "◈";
                isBullet = true;
            }
            else if (!string.IsNullOrEmpty(description) && description.Contains("✦"))
            {
                bulletCharacter = "✦";
                isBullet = true;
            }
            return isBullet;
        }

        private string FormBulletContent(string description, string bulletCharacter)
        {
            string content = description;
            string[] strArray = description.Split(bulletCharacter);
            if (strArray.Length > 0)
            {
                string modifiedDescription = string.Empty;
                for (int x = 0; x < strArray.Length; x++)
                {
                    if (!string.IsNullOrEmpty(strArray[x]))
                        modifiedDescription += bulletCharacter + " " + strArray[x].TrimStart() + "<br/>";

                }
                int length = modifiedDescription.Length - ("<br/>").Length;
                modifiedDescription = modifiedDescription.Substring(0, length);
                content = modifiedDescription;
            }
            return content;
        }

        private string FormTableContent(string description, string bulletCharacter)
        {
            string content = string.Empty;
            string[] strArray = description.Split(bulletCharacter);
            if (strArray.Length > 0)
            {
                for (int x = 0; x < strArray.Length; x++)
                {
                    if (!string.IsNullOrEmpty(strArray[x]))
                       content += string.Format("<tr><td style='vertical-align: top;width:3%,padding: 2px;text-align:left;'>{0}</td> <td style='vertical-align: top;'>{1}</td></tr>", bulletCharacter, strArray[x].TrimStart());
                    
                }
            }
            return content;
        }

        private string FormatContent(string description, string bulletCharacter)
        {
            string content = description;
            string[] strArray = description.Split(bulletCharacter);
            if (strArray.Length > 0)
            {
                string modifiedContent = string.Empty;
                for (int x = 0; x < strArray.Length; x++)
                {
                    if (!string.IsNullOrEmpty(strArray[x]))
                        modifiedContent += bulletCharacter + " " + strArray[x].TrimStart() + "<br/>";
                }
                int length = modifiedContent.Length - ("<br/>").Length;
                modifiedContent = modifiedContent.Substring(0, length);
                content = string.Format("<tr><td style='text-align:left;'>{0}</td></tr>", modifiedContent);
            }
            return content;
        }

        public Response Get(IList<int> tsIds)
        {
            IList<TechnicalSpecialistInfo> result = null;
            Exception exception = null;
            try
            {
                var tsInfo = _mapper.Map<IList<TechnicalSpecialistInfo>>(GetTechSpecialistById(tsIds));
                var tsWithDoc = PopulateTsProfessionalDocument(tsInfo);
                var tsWithCredInfo = PopulateTsLogInDetails(tsWithDoc);
                result = PopulateAssignedToTMDetails(tsWithCredInfo);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordExistInDb(IList<string> tsPins,
                                          ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                          ref IList<ValidationMessage> validationMessages,
                                          params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
        {
            IList<string> tsPinNotExists = null;
            return IsRecordExistInDb(tsPins, ref dbTsInfos, ref tsPinNotExists, ref validationMessages, includes);
        }

        public Response IsRecordExistInDb(IList<string> tsPins,
                                          ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                          ref IList<ValidationMessage> validationMessages,
                                          string[] includes)
        {
            IList<string> tsPinNotExists = null;
            return IsRecordExistInDb(tsPins, ref dbTsInfos, ref tsPinNotExists, ref validationMessages, includes);
        }

        public Response IsRecordExistInDbById(IList<int> tsIds,
                                          ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                          ref IList<ValidationMessage> validationMessages,
                                          params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
        {
            Exception exception = null;
            bool result = true;
            IList<int> tsIdNotExists = null;
            try
            {
                if ((dbTsInfos?.Count == 0 || dbTsInfos == null) && tsIds?.Count > 0)
                    dbTsInfos = GetTechSpecialistById(tsIds, includes);

                result = IsTechSpecialistExistInDbById(tsIds, dbTsInfos, ref tsIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        public Response IsRecordExistInDb(IList<string> tsPins,
                                         ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                         ref IList<string> tsPinNotExists,
                                         ref IList<ValidationMessage> validationMessages,
                                         string[] includes)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if ((dbTsInfos?.Count == 0 || dbTsInfos == null) && tsPins?.Count > 0)
                    dbTsInfos = GetTechSpecialistByPin(tsPins, includes);

                result = IsTechSpecialistExistInDb(tsPins, dbTsInfos, ref tsPinNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPins);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        public Response IsRecordExistInDb(IList<string> tsPins,
                                          ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                          ref IList<string> tsPinNotExists,
                                          ref IList<ValidationMessage> validationMessages,
                                          params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if ((dbTsInfos?.Count == 0 || dbTsInfos == null) && tsPins?.Count > 0)
                    dbTsInfos = GetTechSpecialistByPin(tsPins, includes);

                result = IsTechSpecialistExistInDb(tsPins, dbTsInfos, ref tsPinNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPins);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
        #endregion

        #region Add

        public Response Add(IList<TechnicalSpecialistInfo> tsInfos,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialist> dbTsInfos = null;

            return Add(tsInfos, ref dbTsInfos, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<TechnicalSpecialistInfo> tsInfos,

                            ref IList<DbModel.TechnicalSpecialist> dbTsInfos,

                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.CompanyPayroll> dbCompPayrolls = null;
            IList<DbModel.Data> dbSubDivisions = null;
            IList<DbModel.Data> dbStatuses = null;
            IList<DbModel.Data> dbActions = null;
            IList<DbModel.Data> dbEmploymentTypes = null;
            long? eventId = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.User> dbLoginUser = null;

            return AddTechSpecialist(tsInfos,
                                     ref dbTsInfos,
                                     dbCompanies,
                                     dbCompPayrolls,
                                     dbSubDivisions,
                                     dbStatuses,
                                     dbActions,
                                     dbEmploymentTypes,
                                     dbCountries,
                                     dbLoginUser,
                                     ref eventId,
                                     commitChange,
                                     isDbValidationRequire);
        }

        public Response Add(IList<TechnicalSpecialistInfo> tsInfos,
                            ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                            IList<DbModel.Company> dbCompanies,
                            IList<DbModel.CompanyPayroll> dbCompPayrolls,
                            IList<DbModel.Data> dbSubDivisions,
                            IList<DbModel.Data> dbStatuses,
                            IList<DbModel.Data> dbActions,
                            IList<DbModel.Data> dbEmploymentTypes,
                            IList<DbModel.Country> dbCountries,
                            IList<DbModel.User> dbLoginUser,
                               ref long? eventId,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            return AddTechSpecialist(tsInfos,
                                     ref dbTsInfos,
                                     dbCompanies,
                                     dbCompPayrolls,
                                     dbSubDivisions,
                                     dbStatuses,
                                     dbActions,
                                     dbEmploymentTypes,
                                     dbCountries,
                                     dbLoginUser,
                                     ref eventId,
                                     commitChange,
                                     isDbValidationRequire);
        }
        #endregion

        #region Modify
        public Response Modify(IList<TechnicalSpecialistInfo> tsInfos,
                                bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialist> dbTsInfos = null;

            return Modify(tsInfos, ref dbTsInfos, commitChange, isDbValidationRequire);
        }

        public Response Modify(IList<TechnicalSpecialistInfo> tsInfos,
                                ref IList<DbModel.TechnicalSpecialist> dbTsInfos,

                         bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.CompanyPayroll> dbCompPayrolls = null;
            IList<DbModel.Data> dbSubDivisions = null;
            IList<DbModel.Data> dbStatuses = null;
            IList<DbModel.Data> dbActions = null;
            long? eventId = null;
            IList<DbModel.Data> dbEmploymentTypes = null;
            IList<DbModel.Country> dbCountries = null;
            IList<DbModel.User> dbLoginUser = null;

            return UpdateTechSpecialist(tsInfos,
                                         ref dbTsInfos,
                                         dbCompanies,
                                         dbCompPayrolls,
                                         dbSubDivisions,
                                         dbStatuses,
                                         dbActions,
                                         dbEmploymentTypes,
                                         dbCountries,
                                         dbLoginUser,
                                         ref eventId,
                                         commitChange,
                                         isDbValidationRequire);
        }

        public Response Modify(IList<TechnicalSpecialistInfo> tsInfos,
                                ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                IList<DbModel.Company> dbCompanies,
                                IList<DbModel.CompanyPayroll> dbCompPayrolls,
                                IList<DbModel.Data> dbSubDivisions,
                                IList<DbModel.Data> dbStatuses,
                                IList<DbModel.Data> dbActions,
                                IList<DbModel.Data> dbEmploymentTypes,
                                IList<DbModel.Country> dbCountries,
                                IList<DbModel.User> dbLoginUser,
                                ref long? eventid,
                                bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            return UpdateTechSpecialist(tsInfos,
                                         ref dbTsInfos,
                                         dbCompanies,
                                         dbCompPayrolls,
                                         dbSubDivisions,
                                         dbStatuses,
                                         dbActions,
                                         dbEmploymentTypes,
                                         dbCountries,
                                         dbLoginUser,
                                         ref eventid,
                                         commitChange,
                                         isDbValidationRequire);
        }
        //D946 CR Start
        public Response Modify(List<KeyValuePair<DbModel.TechnicalSpecialist, List<KeyValuePair<string, object>>>> technicalSpecialist, params Expression<Func<DbModel.TechnicalSpecialist, object>>[] updatedProperties)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                _repository.Update(technicalSpecialist, updatedProperties);
                _repository.ForceSave();
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }//D946 CR Start
        #endregion

        #region Delete
        public Response Delete(IList<TechnicalSpecialistInfo> tsInfos,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialist> dbTsInfos = null;
            long? eventId = null;
            return RemoveTechSpecialist(tsInfos, ref dbTsInfos, ref eventId, commitChange, isDbValidationRequire);
        }

        public Response Delete(IList<TechnicalSpecialistInfo> tsInfos,
                                ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                ref long? eventId,
                                     bool commitChange = true,
                                  bool isDbValidationRequire = true)
        {
            return RemoveTechSpecialist(tsInfos, ref dbTsInfos, ref eventId, commitChange, isDbValidationRequire);
        }

        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<TechnicalSpecialistInfo> tsInfos,
                                                ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialist> dbTsInfos = null;
            return IsRecordValidForProcess(tsInfos, validationType, ref dbTsInfos);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistInfo> tsInfos,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialist> dbTsInfos)
        {
            IList<TechnicalSpecialistInfo> filteredTechSpecialist = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.CompanyPayroll> dbCompPayrolls = null;
            IList<DbModel.Data> dbSubDivisions = null;
            IList<DbModel.Data> dbStatuses = null;
            IList<DbModel.Data> dbActions = null;
            IList<DbModel.Data> dbEmploymentTypes = null;
            IList<DbModel.Country> dbCountries = null;

            return CheckRecordValidForProcess(tsInfos,
                                                    validationType,
                                                    ref filteredTechSpecialist,
                                                    ref dbTsInfos,
                                                    ref dbCompanies,
                                                    ref dbCompPayrolls,
                                                    ref dbSubDivisions,
                                                    ref dbStatuses,
                                                    ref dbActions,
                                                    ref dbEmploymentTypes,
                                                    ref dbCountries);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistInfo> tsInfos,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                                ref IList<DbModel.Company> dbCompanies,
                                                ref IList<DbModel.CompanyPayroll> dbCompPayrolls,
                                                ref IList<DbModel.Data> dbSubDivisions,
                                                ref IList<DbModel.Data> dbStatuses,
                                                ref IList<DbModel.Data> dbActions,
                                                ref IList<DbModel.Data> dbEmploymentTypes,
                                                ref IList<DbModel.Country> dbCountries,
                                                bool isDraft = false)
        {
            IList<TechnicalSpecialistInfo> filteredTechSpecialist = null;

            return CheckRecordValidForProcess(tsInfos,
                                                    validationType,
                                                    ref filteredTechSpecialist,
                                                    ref dbTsInfos,
                                                    ref dbCompanies,
                                                    ref dbCompPayrolls,
                                                    ref dbSubDivisions,
                                                    ref dbStatuses,
                                                    ref dbActions,
                                                    ref dbEmploymentTypes,
                                                    ref dbCountries,
                                                    isDraft);
        }
        #endregion

        #endregion

        #region Private Metods

        #region Get
        private IList<DbModel.TechnicalSpecialist> GetTechSpecialistByPin(IList<string> pins,
                                                                         string[] includes)
        {
            IList<DbModel.TechnicalSpecialist> dbTsInfos = null;
            if (pins?.Count > 0)
            {
                dbTsInfos = _repository.FindBy(x => pins.Contains(x.Pin.ToString()), includes).ToList();
            }
            return dbTsInfos;
        }
        private IList<DbModel.TechnicalSpecialist> GetTechSpecialistByPin(IList<string> pins,
                                                                          params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
        {
            IList<DbModel.TechnicalSpecialist> dbTsInfos = null;
            if (pins?.Count > 0)
            {
                dbTsInfos = _repository.FindBy(x => pins.Contains(x.Pin.ToString()), includes).ToList();
            }
            return dbTsInfos;
        }

        private IList<DbModel.TechnicalSpecialist> GetTechSpecialistById(IList<int> tsIds,
                                                                         params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
        {
            IList<DbModel.TechnicalSpecialist> dbTsInfos = null;
            if (tsIds?.Count > 0)
                dbTsInfos = _repository.FindBy(x => tsIds.Contains(x.Id), includes).ToList();

            return dbTsInfos;
        }
        public Response GetTSBasedOnCompany(IList<int> companyIds, bool isActive)
        {
            object result = null;
            Exception exception = null;
            try
            {
                if (isActive)
                {
                    if (companyIds.Count > 0)
                        result = _repository.FindBy(x => companyIds.Contains(x.Company.Id) && x.ProfileStatus.Name == ResourceSearchConstants.TS_Profile_Status_Active && x.EmploymentType.Name != ResourceSearchConstants.Employment_Type_OfficeStaff)?.Select(x1 => new { x1.Id, x1.FirstName, x1.LastName, x1.Pin, x1.CompanyId }).ToList(); //Changes for D1385
                }
                else
                {
                    if (companyIds.Count > 0)
                        result = _repository.FindBy(x => companyIds.Contains(x.Company.Id))?.Select(x1 => new { x1.Id, x1.FirstName, x1.LastName, x1.Pin, x1.CompanyId }).ToList();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), companyIds);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }
        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<TechnicalSpecialistInfo> tsInfos,
                                         ref IList<TechnicalSpecialistInfo> filteredTs,
                                         ref IList<DbModel.Company> dbCompanies,
                                         ref IList<DbModel.CompanyPayroll> dbCompanyPayrolls,
                                         ref IList<DbModel.Data> dbSubDivisions,
                                         ref IList<DbModel.Data> dbStatues,
                                         ref IList<DbModel.Data> dbActions,
                                         ref IList<DbModel.Data> dbEmploymentTypes,
                                         ref IList<DbModel.Country> dbCountries,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsInfos != null && tsInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTs == null || filteredTs.Count <= 0)
                    filteredTs = FilterRecord(tsInfos, validationType);

                if (IsValidPayload(filteredTs, validationType, ref validationMessages))
                {
                    result = IsMasterDataValid(filteredTs,
                                                ref dbCompanies,
                                                ref dbCompanyPayrolls,
                                                ref dbSubDivisions,
                                                ref dbStatues,
                                                ref dbActions,
                                                ref dbEmploymentTypes,
                                                ref dbCountries,
                                                ref validationMessages);
                }
            }
            return result;
        }

        private Response AddTechSpecialist(IList<TechnicalSpecialistInfo> tsInfos,
                                            ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                            IList<DbModel.Company> dbCompanies,
                                            IList<DbModel.CompanyPayroll> dbCompPayrolls,
                                            IList<DbModel.Data> dbSubDivisions,
                                            IList<DbModel.Data> dbStatuses,
                                            IList<DbModel.Data> dbActions,
                                            IList<DbModel.Data> dbEmploymentTypes,
                                            IList<DbModel.Country> dbCountries,
                                            IList<DbModel.User> dbLoginUser,
                                            ref long? eventId,
                                            bool commitChange = true,
                                            bool isDbValidationRequire = true)
        {
            Exception exception = null;
            long? eventID = 0;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.User> dbUser = null;
            try
            {
                Response valdResponse = null;
                IList<TechnicalSpecialistInfo> recordToBeAdd = null;

                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(tsInfos,
                                                                ValidationType.Add,
                                                                ref recordToBeAdd,
                                                                ref dbTsInfos,
                                                                ref dbCompanies,
                                                                ref dbCompPayrolls,
                                                                ref dbSubDivisions,
                                                                ref dbStatuses,
                                                                ref dbActions,
                                                                ref dbEmploymentTypes,
                                                                ref dbCountries);
                else
                    recordToBeAdd = tsInfos;

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    recordToBeAdd = recordToBeAdd.Select(x => { x.Id = 0; x.Epin = 0; return x; }).ToList();
                    GetPendingWithUserData(recordToBeAdd, ref dbUser); //Added for D946 CR
                    var mappedRecords = ConvertToDbModel(recordToBeAdd,
                                                              dbCompanies,
                                                              dbCompPayrolls,
                                                              dbSubDivisions,
                                                              dbStatuses,
                                                              dbActions,
                                                              dbEmploymentTypes,
                                                              dbCountries,
                                                              dbUser,
                                                              dbLoginUser);
                    _repository.AutoSave = false;
                    _repository.Add(mappedRecords);
                    if (commitChange)
                    {
                        var savedCnt = _repository.ForceSave();
                        dbTsInfos = mappedRecords;
                        if (savedCnt > 0)
                        {
                            var docRes = ProcessTsProfessionalDocuments(recordToBeAdd, mappedRecords, ref validationMessages);
                            if (dbTsInfos?.Count > 0 && recordToBeAdd?.Count > 0)
                            {
                                dbTsInfos?.ToList().ForEach(x =>
                                 recordToBeAdd?.ToList().ForEach(x1 =>
                                 {
                                     x1.Epin = x.Pin;
                                     var newDocuments = x1?.ProfessionalAfiliationDocuments?.Where(doc => doc.RecordStatus == "N")?.Select(doc => doc.DocumentName)?.ToList();
                                     if (newDocuments != null && newDocuments.Count > 0)
                                         x1.DocumentName = string.Join(",", newDocuments);

                                     _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                              "{" + AuditSelectType.Id + ":" + x.Pin + "}${" + AuditSelectType.PIN + ":" + x.Pin + "}${" + AuditSelectType.LastName + ":" + x.LastName + "}",
                                                               ValidationType.Add.ToAuditActionType(),
                                                               SqlAuditModuleType.TechnicalSpecialist,
                                                               null,
                                                                x1);
                                 }));
                                eventId = eventID;
                            }
                        }

                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        #endregion

        #region Modify
        private Response UpdateTechSpecialist(IList<TechnicalSpecialistInfo> tsInfos,
                                                ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                                IList<DbModel.Company> dbCompanies,
                                                IList<DbModel.CompanyPayroll> dbCompPayrolls,
                                                IList<DbModel.Data> dbSubDivisions,
                                                IList<DbModel.Data> dbStatuses,
                                                IList<DbModel.Data> dbActions,
                                                IList<DbModel.Data> dbEmploymentTypes,
                                                IList<DbModel.Country> dbCountries,
                                                IList<DbModel.User> dbLoginUser,
                                                ref long? eventId,
                                                bool commitChange = true,
                                                bool isDbValidationRequire = true)
        {
            Exception exception = null;
            long? eventID = 0;
            IList<ValidationMessage> validationMessages = null;
            IList<TechnicalSpecialistInfo> dbExistingTechnicalSpecialist = new List<TechnicalSpecialistInfo>();
            IList<DbModel.User> dbUser = null;
            try
            {
                Response valdResponse = null;
                var recordToBeModify = FilterRecord(tsInfos, ValidationType.Update);
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(tsInfos,
                                                              ValidationType.Update,
                                                              ref recordToBeModify,
                                                              ref dbTsInfos,
                                                              ref dbCompanies,
                                                              ref dbCompPayrolls,
                                                              ref dbSubDivisions,
                                                              ref dbStatuses,
                                                              ref dbActions,
                                                              ref dbEmploymentTypes,
                                                              ref dbCountries);

                if ((dbTsInfos == null || (dbTsInfos?.Count <= 0 && valdResponse == null)) && recordToBeModify?.Count > 0)
                    dbTsInfos = _repository.Get(recordToBeModify?.Select(x => Convert.ToInt32(x.Id)).ToList());

                if (!isDbValidationRequire || (Convert.ToBoolean(valdResponse.Result) && dbTsInfos?.Count > 0))
                {
                    GetPendingWithUserData(recordToBeModify, ref dbUser);
                    var tsToBeModify = ConvertToDbModel(recordToBeModify,
                                                             dbCompanies,
                                                             dbCompPayrolls,
                                                             dbSubDivisions,
                                                             dbStatuses,
                                                             dbActions,
                                                             dbEmploymentTypes,
                                                             dbCountries,
                                                             dbUser, dbLoginUser);

                    dbTsInfos.ToList().ForEach(x =>
                    {
                        dbExistingTechnicalSpecialist.Add(ObjectExtension.Clone(_mapper.Map<TechnicalSpecialistInfo>(x)));
                    });
                    dbExistingTechnicalSpecialist = PopulateTsLogInDetails(dbExistingTechnicalSpecialist);
                    dbExistingTechnicalSpecialist = PopulateTsProfessionalDocument(dbExistingTechnicalSpecialist);

                    var mstProfileStatus = _masterService.Get(new List<MasterType> { MasterType.ProfileStatus }).ToList();
                    dbTsInfos.ToList().ForEach(tsInfo =>
                    {
                        var ts = tsToBeModify.FirstOrDefault(x => x.Id == tsInfo.Id);
                        var statusFrom = mstProfileStatus?.FirstOrDefault(x1 => x1.Id == tsInfo.ProfileStatusId)?.Name;
                        var statusTo = mstProfileStatus?.FirstOrDefault(x1 => x1.Id == ts.ProfileStatusId)?.Name;

                        if (tsInfo.ProfileStatusId != ts.ProfileStatusId && ((statusFrom == ResourceProfileStatus.Pre_qualification.DisplayName() && statusTo == ResourceProfileStatus.Pending_TMR.DisplayName()) || (statusFrom == ResourceProfileStatus.Pending_TMR.DisplayName() && statusTo == ResourceProfileStatus.Active.DisplayName())))
                        {
                            var emailPlaceHolders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_First_Name, ts?.FirstName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Last_Name, ts?.LastName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_PROFILE_ID, ts?.Pin.ToString()),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Status_From, statusFrom),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Status_To, statusTo ),
                                };

                            ProcessEmailNotifications(recordToBeModify?.FirstOrDefault(x => x.Epin == tsInfo.Pin), EmailTemplate.EmailProfileStatusChange, emailPlaceHolders, null, null, tsInfo.TechnicalSpecialistContact.FirstOrDefault(x => x.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress);
                        }
                        if ((tsInfo.ProfileActionId != ts.ProfileActionId || dbActions.Any(x=>x.Id == ts.ProfileActionId && x.Id == tsInfo.ProfileActionId)) && statusFrom != statusTo && statusTo == ResourceProfileStatus.Active.DisplayName() &&  ts.ApprovalStatus!="I" )//Def 978 
                        {
                            var emailPlaceHolders = new List<KeyValuePair<string, string>> {
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_First_Name, ts?.FirstName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_Last_Name, ts?.LastName),
                                    new KeyValuePair<string, string>(TechnicalSpecialistConstants.Email_Content_PROFILE_ID, ts?.Pin.ToString())
                                };
                            ProcessEmailNotifications(recordToBeModify?.FirstOrDefault(x => x.Epin == tsInfo.Pin), EmailTemplate.EmailProfileActivation, emailPlaceHolders);
                        }
                         
                        tsInfo.BusinessInformationComment = ts.BusinessInformationComment;
                        tsInfo.CompanyId = ts.CompanyId;
                        tsInfo.CompanyPayrollId = ts.CompanyPayrollId;
                        tsInfo.PassportCountryOriginId = ts.PassportCountryOriginId;
                        tsInfo.DateOfBirth = ts.DateOfBirth;
                        tsInfo.DrivingLicenseNumber = ts.DrivingLicenseNumber;
                        tsInfo.DrivingLicenseExpiryDate = ts.DrivingLicenseExpiryDate;
                        tsInfo.EndDate = ts.EndDate;
                        tsInfo.EmploymentTypeId = ts.EmploymentTypeId;
                        tsInfo.FirstName = ts.FirstName;
                        tsInfo.IsReviewAndModerationProcess = ts.IsReviewAndModerationProcess;
                        tsInfo.IsActive = ts.IsActive;
                        tsInfo.IsEreportingQualified = ts.IsEreportingQualified;
                        tsInfo.LastName = ts.LastName;
                        tsInfo.MiddleName = ts.MiddleName;
                        tsInfo.ModeOfCommunication = ts.ModeOfCommunication;
                        tsInfo.PassportNumber = ts.PassportNumber;
                        tsInfo.PassportExpiryDate = ts.PassportExpiryDate;
                        tsInfo.PayrollReference = ts.PayrollReference;
                        tsInfo.PayrollNote = ts.PayrollNote;
                        tsInfo.ProfessionalAfiliation = ts.ProfessionalAfiliation;
                        tsInfo.ProfessionalSummary = ts.ProfessionalSummary;
                        tsInfo.ProfileActionId = ts.ProfileActionId;
                        tsInfo.ProfileStatusId = ts.ProfileStatusId;
                        tsInfo.HomePageComment = ts.HomePageComment;
                        tsInfo.ContactComment = ts.ContactComment;
                        //tsInfo.Pin =ts.Pin;
                        tsInfo.Salutation = ts.Salutation;
                        tsInfo.StartDate = ts.StartDate;
                        tsInfo.SubDivisionId = ts.SubDivisionId;
                        tsInfo.TaxReference = ts.TaxReference;
                        tsInfo.LastModification = DateTime.UtcNow;
                        tsInfo.UpdateCount = tsInfo.UpdateCount.CalculateUpdateCount();
                        tsInfo.ModifiedBy = ts.ModifiedBy;
                        tsInfo.ApprovalStatus = ts.ApprovalStatus;
                        tsInfo.Tqmcomment = ts.Tqmcomment;
                        tsInfo.AssignedByUser = ts.AssignedByUser;
                        tsInfo.AssignedToUser = ts.AssignedToUser;
                        tsInfo.PendingWithId = ts.PendingWithId;
                        tsInfo.LogInName = ts.LogInName;
                        tsInfo.IsTsCredSent = ts.IsTsCredSent;

                    });
                    _repository.AutoSave = false;
                    _repository.Update(dbTsInfos);
                    if (commitChange)
                    {
                        var savedCnt = _repository.ForceSave();
                        if (savedCnt > 0)
                        {
                            var docRes = ProcessTsProfessionalDocuments(recordToBeModify, dbTsInfos, ref validationMessages);

                        }



                        if (dbTsInfos != null && recordToBeModify?.Count > 0 && savedCnt > 0)
                        {

                            dbTsInfos?.ToList().ForEach(x =>
                           recordToBeModify?.ToList().ForEach(x1 =>
                           {
                               var oldValues = dbExistingTechnicalSpecialist?.FirstOrDefault(x2 => x2.Epin == x1.Epin);
                               var oldDocuments = oldValues?.ProfessionalAfiliationDocuments?.ToList();
                               var newDocuments = x1?.ProfessionalAfiliationDocuments?.ToList();
                               if (newDocuments != null && newDocuments.Count > 0)
                               {
                                   if (oldDocuments != null && oldDocuments.Count > 0)
                                   {
                                       oldValues.DocumentName = string.Join(",", oldDocuments?.Select(doc => doc.DocumentName));
                                   }

                                   var deletedDocIds = newDocuments?.Where(doc => doc.RecordStatus.IsRecordStatusDeleted())?.Select(doc => doc.Id)?.ToList();
                                   if (oldDocuments != null && oldDocuments.Count > 0)
                                   {
                                       var filteredOldDoc = new List<ModuleDocument>();
                                       filteredOldDoc = oldDocuments;
                                       if (deletedDocIds != null && deletedDocIds.Count > 0)
                                           filteredOldDoc = oldDocuments?.Where(doc => !deletedDocIds.Contains(doc.Id))?.ToList();
                                       var filteredNewDoc = newDocuments?.Where(doc => doc.RecordStatus.IsRecordStatusNew())?.ToList();

                                       if (filteredNewDoc != null && filteredNewDoc.Count > 0)
                                       {
                                           filteredOldDoc.AddRange(filteredNewDoc);
                                       }
                                       x1.DocumentName = string.Join(",", filteredOldDoc?.Select(doc => doc.DocumentName));
                                   }
                                   else
                                       x1.DocumentName = string.Join(",", newDocuments?.Select(doc => doc.DocumentName));
                               }

                               _auditSearchService.AuditLog(x1, ref eventID, x1.ActionByUser,
                                                          "{" + AuditSelectType.Id + ":" + x.Pin + "}${" + AuditSelectType.PIN + ":" + x.Pin + "}${" + AuditSelectType.LastName + ":" + x.LastName + "}",
                                                         ValidationType.Update.ToAuditActionType(),
                                                         SqlAuditModuleType.TechnicalSpecialist,
                                                         oldValues,
                                                          x1
                                                         );
                           }));
                        }
                        eventId = eventID;
                    }

                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistInfo> tsInfos,
                                            ref IList<TechnicalSpecialistInfo> filteredTsInfos,
                                            ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                            ref IList<DbModel.Company> dbCompanies,
                                            ref IList<DbModel.CompanyPayroll> dbCompanyPayrolls,
                                            ref IList<DbModel.Data> dbSubDivisions,
                                            ref IList<DbModel.Data> dbStatues,
                                            ref IList<DbModel.Data> dbActions,
                                            ref IList<DbModel.Data> dbEmploymentTypes,
                                            ref IList<DbModel.Country> dbCountries,
                                            ref IList<ValidationMessage> validationMessages,
                                            bool isDraft = false)
        {
            bool result = false;
            if (tsInfos != null && tsInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsInfos == null || filteredTsInfos.Count <= 0)
                    filteredTsInfos = FilterRecord(tsInfos, validationType);

                if (IsValidPayload(filteredTsInfos, validationType, ref validationMessages))
                {
                    if (dbTsInfos == null)
                        GetTsDbInfo(filteredTsInfos, true, ref dbTsInfos);

                    if (dbTsInfos?.Count != filteredTsInfos?.Count) //Invalid TechSpecialist Id found.
                    {
                        var dbTsInfosByIds = dbTsInfos;
                        var notExists = filteredTsInfos.Where(x => !dbTsInfosByIds.Any(x1 => x.Id == x.Id)).ToList();
                        notExists?.ForEach(ts =>
                        {
                            messages.Add(_messages, ts, MessageType.TsUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        result = IsMasterDataValid(filteredTsInfos,
                                                    ref dbCompanies,
                                                    ref dbCompanyPayrolls,
                                                    ref dbSubDivisions,
                                                    ref dbStatues,
                                                    ref dbActions,
                                                    ref dbEmploymentTypes,
                                                    ref dbCountries,
                                                    ref validationMessages);
                        if (result)
                        {
                            result = isDraft ? true : IsRecordUpdateCountMatching(filteredTsInfos, dbTsInfos, ref messages);
                            //if (result)
                            //    result = this.IsTechSpecialistEpinUnique(filteredTsInfos, ref validationMessages);
                        }
                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }

            return result;
        }

        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistInfo> tsInfos,
                                                IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                                ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var notMatchedRecords = tsInfos.Where(x => !dbTsInfos.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.Epin, MessageType.TsUpdatedByOther, x.Epin);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }

        //private bool IsTechSpecialistEpinUnique(IList<TechnicalSpecialistInfo> filteredTsInfos,
        //                                        ref IList<ValidationMessage> validationMessages)
        //{
        //    List<ValidationMessage> messages = new List<ValidationMessage>();
        //    if (validationMessages == null)
        //        validationMessages = new List<ValidationMessage>();

        //    var tsEpins = filteredTsInfos.Select(x => x.Epin);
        //    var dbTsInfos = _repository.FindBy(x => tsEpins.Contains(x.Pin)).ToList();
        //    if (dbTsInfos?.Count > 0)
        //    {
        //        var tsAlreadyExist = filteredTsInfos.Where(x => !dbTsInfos.Select(x1 => x1.Id).ToList().Contains(x.Id));
        //        tsAlreadyExist?.ToList().ForEach(x =>
        //        {
        //            messages.Add(_messages, x.Epin, MessageType.TsEPinAlreadyExist, x.Epin);
        //        });
        //    }

        //    if (messages.Count > 0)
        //        validationMessages.AddRange(messages);

        //    return messages?.Count <= 0;
        //}
        #endregion

        #region Remove
        private bool IsRecordValidForRemove(IList<TechnicalSpecialistInfo> tsInfos,
                                            ref IList<TechnicalSpecialistInfo> filteredTsInfos,
                                            ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsInfos != null && tsInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (filteredTsInfos == null || filteredTsInfos.Count <= 0)
                    filteredTsInfos = FilterRecord(tsInfos, validationType);

                if (filteredTsInfos?.Count > 0 && IsValidPayload(filteredTsInfos, validationType, ref validationMessages))
                {
                    GetTsDbInfo(filteredTsInfos, false, ref dbTsInfos);
                    IList<string> tsEpinNotExists = null;
                    var tsPins = filteredTsInfos.Select(x => x.Epin.ToString()).Distinct().ToList();
                    result = IsTechSpecialistExistInDb(tsPins, dbTsInfos, ref tsEpinNotExists, ref validationMessages);
                    if (result)
                        result = IsTechSpecialistCanBeRemove(dbTsInfos, ref validationMessages);
                }
            }
            return result;
        }

        private bool IsTechSpecialistCanBeRemove(IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                                 ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            dbTsInfos?.Where(x => x.IsAnyCollectionPropertyContainValue())
                 .ToList()
                 .ForEach(x =>
                 {
                     messages.Add(_messages, x.Pin, MessageType.TsEPinIsBeingUsed, x.Pin);
                 });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }
        #endregion

        #region Common
        private Response ProcessTsProfessionalDocuments(IList<TechnicalSpecialistInfo> tstechnicalspecialist, IList<DbModel.TechnicalSpecialist> dbTstechnicalspecialist, ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            try
            {
                if (tstechnicalspecialist?.Count == dbTstechnicalspecialist?.Count)
                {
                    //TODO : As we dont have any Unique values b/w viewModel data and Db model data . We are considering arry sequence to Update Id;
                    for (int comCnt = 0; comCnt < tstechnicalspecialist?.Count; comCnt++)
                    {
                        if (dbTstechnicalspecialist[comCnt] != null && tstechnicalspecialist[comCnt].ProfessionalAfiliationDocuments != null)
                        {
                            tstechnicalspecialist[comCnt]?.ProfessionalAfiliationDocuments?.Where(d => !string.IsNullOrEmpty(d.RecordStatus)).ToList().ForEach(x1 =>
                            {
                                if (x1.RecordStatus.IsRecordStatusDeleted())
                                    x1.RecordStatus = RecordStatus.Delete.FirstChar();
                                else
                                    x1.RecordStatus = RecordStatus.Modify.FirstChar();
                                x1.SubModuleRefCode = dbTstechnicalspecialist[comCnt].Id.ToString();
                                x1.ModuleRefCode = dbTstechnicalspecialist[comCnt]?.Pin.ToString();
                                x1.DocumentType = Evolution.Common.Enums.DocumentType.TS_ProfessionalAfiliation.ToString();
                                x1.Status = x1.Status.Trim();
                            });
                        }
                    }

                    var tsToBeProcess = tstechnicalspecialist?.Where(x => x.ProfessionalAfiliationDocuments != null &&
                                                                          x.ProfessionalAfiliationDocuments.Any(x1 => x1.RecordStatus != null))
                                                              .SelectMany(x => x.ProfessionalAfiliationDocuments)
                                                              .ToList();
                    if (tsToBeProcess?.Count > 0)
                    {
                        var docToModify = tsToBeProcess.Where(x1 => x1.RecordStatus.IsRecordStatusModified()).ToList();
                        var docToDelete = tsToBeProcess.Where(x1 => x1.RecordStatus.IsRecordStatusDeleted()).ToList();
                        if (docToDelete.Count > 0)
                            _documentService.Delete(docToDelete);

                        if (docToModify?.Count > 0)
                            return _documentService.Modify(docToModify);
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tstechnicalspecialist);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }

        private IList<TechnicalSpecialistInfo> PopulateTsProfessionalDocument(IList<TechnicalSpecialistInfo> tstechnicalspecialist)
        {
            try
            {
                if (tstechnicalspecialist?.Count > 0)
                {
                    var epins = tstechnicalspecialist?.Select(x => x.Epin.ToString())?.Distinct().ToList();

                    var tsCertificationDocs = _documentService.Get(ModuleCodeType.TS, epins, epins).Result?.Populate<IList<ModuleDocument>>();

                    if (tsCertificationDocs?.Count > 0)
                    {
                        return tstechnicalspecialist.GroupJoin(tsCertificationDocs,
                            tsc => new { moduleRefCode = tsc.Epin.ToString(), subModuleRefCode = tsc.Id.ToString() },
                            doc => new { moduleRefCode = doc.ModuleRefCode, subModuleRefCode = doc.SubModuleRefCode },
                            (tsc, doc) => new { tsProf = tsc, doc }).Select(x =>
                            {
                                x.tsProf.ProfessionalAfiliationDocuments = x?.doc.ToList();
                                return x.tsProf;
                            }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tstechnicalspecialist);
            }

            return tstechnicalspecialist;
        }

        private IList<TechnicalSpecialistInfo> PopulateTsLogInDetails(IList<TechnicalSpecialistInfo> tstechnicalspecialist)
        {
            try
            {
                if (tstechnicalspecialist?.Count > 0)
                {
                    var logonNames = tstechnicalspecialist?.Where(x => !string.IsNullOrEmpty(x.LogonName))?.Select(x => x.LogonName).ToList();

                    if (logonNames?.Count > 0)
                    {
                        logonNames = logonNames.Distinct().ToList();

                        var tsUserCredInfo = _userService.Get(_environment.SecurityAppName, logonNames).Result?.Populate<IList<UserInfo>>();

                        if (tsUserCredInfo?.Count > 0)
                        {
                            return tstechnicalspecialist.Join(tsUserCredInfo,
                                tsc => new { LogonName = tsc.LogonName.ToString() },
                                usrCrdIn => new { usrCrdIn.LogonName },
                                (tsc, usrCrdIn) => new { tsProf = tsc, usrCrdIn }).Select(x =>
                                {
                                    x.tsProf.LogonName = x?.usrCrdIn?.LogonName;
                                    x.tsProf.Password = x?.usrCrdIn?.Password;
                                    x.tsProf.SecurityQuestion = x?.usrCrdIn?.SecurityQuestion1;
                                    x.tsProf.SecurityAnswer = x?.usrCrdIn?.SecurityQuestion1Answer;
                                    x.tsProf.IsLockOut = x?.usrCrdIn?.IsAccountLocked ?? true;
                                    x.tsProf.IsEnableLogin = x?.usrCrdIn?.IsActive ?? false;
                                    return x.tsProf;
                                }).ToList();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tstechnicalspecialist);
            }

            return tstechnicalspecialist;
        }

        private IList<TechnicalSpecialistInfo> PopulateAssignedToTMDetails(IList<TechnicalSpecialistInfo> tstechnicalspecialist)
        {
            try
            {
                if (tstechnicalspecialist?.Count > 0)
                {
                    var ePins = tstechnicalspecialist.Select(x => x.Epin.ToString()).Distinct().ToList();

                    var assignedToTM = _taskService.Get(TechnicalSpecialistConstants.Task_Type_Taxonomy_Approval_Request, ePins).Result?.Populate<IList<MyTask>>();

                    if (assignedToTM?.Count > 0)
                    {
                        return tstechnicalspecialist.Join(assignedToTM,
                            tsc => new { refCode = tsc.Epin.ToString() },
                            tmU => new { refCode = tmU.TaskRefCode },
                            (tsc, tmU) => new { tsProf = tsc, tmU }).Select(x =>
                            {
                                x.tsProf.AssignedToUser = x.tmU.AssignedTo;
                                return x.tsProf;
                            }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tstechnicalspecialist);
            }

            return tstechnicalspecialist;
        }

        private void GetTsDbInfo(IList<TechnicalSpecialistInfo> filteredTsInfos,
                                 bool isTsInfoById,
                                 ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                 params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
        {
            var tsPins = !isTsInfoById ? filteredTsInfos.Select(x => x.Epin.ToString()).Distinct().ToList() : null;
            IList<int> tsIds = isTsInfoById ? filteredTsInfos.Select(x => (int)x.Id).Distinct().ToList() : null;
            if (dbTsInfos == null || dbTsInfos.Count <= 0)
                dbTsInfos = isTsInfoById ?
                       GetTechSpecialistById(tsIds, includes).ToList() :
                       GetTechSpecialistByPin(tsPins, includes).ToList();
        }

        private IList<TechnicalSpecialistInfo> FilterRecord(IList<TechnicalSpecialistInfo> tsInfos, ValidationType filterType)
        {
            IList<TechnicalSpecialistInfo> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }

        private bool IsTechSpecialistExistInDb(IList<string> tsPins,
                                        IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                        ref IList<string> tsPinNotExists,
                                        ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsInfos == null)
                dbTsInfos = new List<DbModel.TechnicalSpecialist>();

            var validMessages = validationMessages;

            if (tsPins?.Count > 0)
            {
                tsPinNotExists = tsPins.Where(pin => !dbTsInfos.Any(x1 => x1.Pin.ToString() == pin))
                                     .Select(pin => pin)
                                     .ToList();

                tsPinNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsEPinDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsTechSpecialistExistInDb(IList<string> tsPins,
                                                IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                                ref IList<string> tsPinNotExists,
                                                ref IList<ValidationMessage> validationMessages,
                                                params Expression<Func<DbModel.CompanyPayroll, object>>[] includes)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsInfos == null)
                dbTsInfos = new List<DbModel.TechnicalSpecialist>();

            var validMessages = validationMessages;

            if (tsPins?.Count > 0)
            {
                tsPinNotExists = tsPins.Where(pin => !dbTsInfos.Any(x1 => x1.Pin.ToString() == pin))
                                     .Select(pin => pin)
                                     .ToList();

                tsPinNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsEPinDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsTechSpecialistExistInDbById(IList<int> tsIds,
                                                IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                                ref IList<int> tsIdNotExists,
                                                ref IList<ValidationMessage> validationMessages,
                                                params Expression<Func<DbModel.CompanyPayroll, object>>[] includes)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsInfos == null)
                dbTsInfos = new List<DbModel.TechnicalSpecialist>();

            var validMessages = validationMessages;

            if (tsIds?.Count > 0)
            {
                tsIdNotExists = tsIds.Where(id => !dbTsInfos.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tsIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.InvalidTechSpecialist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private Response RemoveTechSpecialist(IList<TechnicalSpecialistInfo> tsInfos,
                                           ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                           ref long? eventId,
                                        bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            long? eventID = 0;
            IList<TechnicalSpecialistInfo> recordToBeDeleted = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(tsInfos, ValidationType.Delete, ref dbTsInfos);

                if (!isDbValidationRequire && tsInfos?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsInfos, ValidationType.Delete);
                }

                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbTsInfos?.Count > 0)
                {
                    var dbTsToBeDeleted = dbTsInfos?.Where(x => recordToBeDeleted.Any(x1 => x1.Epin == x.Pin)).ToList();

                    _repository.AutoSave = false;
                    _repository.Delete(dbTsToBeDeleted);
                    if (commitChange)
                    {
                        int value = _repository.ForceSave();
                        if (dbTsToBeDeleted?.Count > 0 && tsInfos?.Count > 0 && value > 0)
                        {

                            dbTsToBeDeleted?.ToList().ForEach(x =>
                            tsInfos?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventID, tsInfos?.FirstOrDefault()?.ActionByUser,
                                                                "{" + AuditSelectType.Id + ":" + x.Pin + "}${" + AuditSelectType.PIN + ":" + x.Pin + "}${" + AuditSelectType.LastName + ":" + x.LastName + "}",
                                                               ValidationType.Delete.ToAuditActionType(),
                                                               SqlAuditModuleType.TechnicalSpecialist,
                                                               x1,
                                                              null
                                                               )));


                            eventId = eventID;
                        }

                    }

                }

                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistInfo> tsInfos,
                                                    ValidationType validationType,
                                                    ref IList<TechnicalSpecialistInfo> filteredTsInfos,
                                                    ref IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                                    ref IList<DbModel.Company> dbCompanies,
                                                    ref IList<DbModel.CompanyPayroll> dbCompPayrolls,
                                                    ref IList<DbModel.Data> dbSubDivisions,
                                                    ref IList<DbModel.Data> dbStatuses,
                                                    ref IList<DbModel.Data> dbActions,
                                                    ref IList<DbModel.Data> dbEmploymentTypes,
                                                    ref IList<DbModel.Country> dbCountries,
                                                    bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsInfos,
                                                 ref filteredTsInfos,
                                                 ref dbCompanies,
                                                 ref dbCompPayrolls,
                                                 ref dbSubDivisions,
                                                 ref dbStatuses,
                                                 ref dbActions,
                                                 ref dbEmploymentTypes,
                                                 ref dbCountries,
                                                 ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsInfos,
                                                    ref filteredTsInfos,
                                                    ref dbTsInfos,
                                                    ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsInfos,
                                                    ref filteredTsInfos,
                                                    ref dbTsInfos,
                                                    ref dbCompanies,
                                                    ref dbCompPayrolls,
                                                    ref dbSubDivisions,
                                                    ref dbStatuses,
                                                    ref dbActions,
                                                    ref dbEmploymentTypes,
                                                    ref dbCountries,
                                                    ref validationMessages,
                                                    isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsInfos);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsTechSpecialistExistInDb(IList<int> tsIds,
                                               IList<DbModel.TechnicalSpecialist> dbTsInfos,
                                               ref IList<int> tsIdNotExists,
                                               ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsInfos == null)
                dbTsInfos = new List<DbModel.TechnicalSpecialist>();

            var validMessages = validationMessages;

            if (tsIds?.Count > 0)
            {
                tsIdNotExists = tsIds.Where(x => !dbTsInfos.Select(x1 => (int)x1.Id).ToList().Contains(x)).ToList();
                tsIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsEPinDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistInfo> ts,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(ts), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Security, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private IList<DbModel.Data> GetMasterData(IList<TechnicalSpecialistInfo> tsInfos,
                                                    ref IList<DbModel.Data> dbSubDivisions,
                                                    ref IList<DbModel.Data> dbActions,
                                                    ref IList<DbModel.Data> dbStatuses,
                                                    ref IList<DbModel.Data> dbEmploymentTypes)
        {
            IList<string> subDivisionNames = tsInfos.Select(x => x.SubDivisionName).ToList();
            IList<string> actionNames = tsInfos.Select(x => x.ProfileAction).ToList();
            IList<string> statusNames = tsInfos.Select(x => x.ProfileStatus).ToList();
            IList<string> employmentTypes = tsInfos.Select(x => x.EmploymentType).ToList();
            var masterNames = subDivisionNames.Union(actionNames)
                                              .Union(statusNames)
                                              .Union(employmentTypes)
                                              .ToList();
            var masterTypes = new List<MasterType>()
                    {
                        MasterType.SubDivision,
                        MasterType.ProfileAction,
                        MasterType.ProfileStatus,
                        MasterType.EmploymentType
                    };
            var dbMaster = _masterService.Get(masterTypes, null, masterNames);
            if (dbMaster?.Count > 0)
            {
                dbSubDivisions = dbMaster.Where(x => x.MasterDataTypeId == (int)MasterType.SubDivision).ToList();
                dbActions = dbMaster.Where(x => x.MasterDataTypeId == (int)MasterType.ProfileAction).ToList();
                dbStatuses = dbMaster.Where(x => x.MasterDataTypeId == (int)MasterType.ProfileStatus).ToList();
                dbEmploymentTypes = dbMaster.Where(x => x.MasterDataTypeId == (int)MasterType.EmploymentType).ToList();
            }

            return dbMaster;
        }

        private bool IsMasterDataValid(IList<TechnicalSpecialistInfo> filteredTs,
                                         ref IList<DbModel.Company> dbCompanies,
                                         ref IList<DbModel.CompanyPayroll> dbCompanyPayrolls,
                                         ref IList<DbModel.Data> dbSubDivisions,
                                         ref IList<DbModel.Data> dbStatues,
                                         ref IList<DbModel.Data> dbActions,
                                         ref IList<DbModel.Data> dbEmploymentTypes,
                                         ref IList<DbModel.Country> dbCountries,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (filteredTs != null && filteredTs.Count > 0)
            {
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                IList<string> companyCodes = filteredTs.Select(x => x.CompanyCode).ToList();
                var comPayrolls = filteredTs.Where(x => !string.IsNullOrEmpty(x.CompanyPayrollName))
                                            .Select(x => new KeyValuePair<string, string>(x.CompanyCode, x.CompanyPayrollName))
                                            .ToList();
                IList<string> subDivisionNames = filteredTs.Select(x => x.SubDivisionName).ToList();
                IList<string> actionNames = filteredTs.Select(x => x.ProfileAction).ToList();
                IList<string> statusNames = filteredTs.Select(x => x.ProfileStatus).ToList();
                IList<string> emplTypes = filteredTs.Select(x => x.EmploymentType).ToList();
                IList<string> countryNames = filteredTs.Where(x => !string.IsNullOrEmpty(x.PassportCountryName))
                                                       .Select(x => x.PassportCountryName)
                                                       .ToList();

                var dbMaster = GetMasterData(filteredTs, ref dbSubDivisions, ref dbActions, ref dbStatues, ref dbEmploymentTypes);

                result = _companyService.IsValidCompany(companyCodes,
                                                        ref dbCompanies,
                                                        ref validationMessages,
                                                        comp => comp.CompanyPayroll);

                if (result && comPayrolls?.Count > 0)
                {
                    dbCompanyPayrolls = dbCompanies?.SelectMany(x => x.CompanyPayroll).ToList();
                    result = _compPayrollService.IsValidCompanyPayroll(comPayrolls, ref dbCompanyPayrolls, ref validationMessages);
                }

                if (result && subDivisionNames?.Count > 0)
                    result = _subDivisionService.IsValidSubDivisionName(subDivisionNames, ref dbSubDivisions, ref validationMessages);

                if (result && actionNames?.Count > 0)
                    result = _profileActionService.IsValidProfileActionName(actionNames, ref dbActions, ref validationMessages);

                if (result && statusNames?.Count > 0)
                    result = _profileStatusService.IsValidProfileStatusName(statusNames, ref dbStatues, ref validationMessages);

                if (result && emplTypes?.Count > 0)
                    result = _employmentService.IsValidEmploymentName(emplTypes, ref dbEmploymentTypes, ref validationMessages);

                if (result && countryNames?.Count > 0)
                    result = _countryService.IsValidCountryName(countryNames, ref dbCountries, ref validationMessages);
            }
            return result;
        }

        private void GetPendingWithUserData(IList<TechnicalSpecialistInfo> filteredTs, ref IList<DbModel.User> dbUsers)
        {
            var userLogonName = filteredTs.Where(x => !string.IsNullOrEmpty(x.PendingWithUser))?.Select(x => x.PendingWithUser).ToList();
            IList<string> userNotExists = null;
            _userService.IsRecordExistInDb(userLogonName.Select(x =>
                                                                            new KeyValuePair<string, string>(_environment.SecurityAppName, x)).ToList(),
                                                                            ref dbUsers,
                                                                            ref userNotExists);

        }

        private IList<DbModel.TechnicalSpecialist> ConvertToDbModel(IList<TechnicalSpecialistInfo> tsInfos,
                                                             IList<DbModel.Company> dbCompanies,
                                                             IList<DbModel.CompanyPayroll> dbCompanyPayrolls,
                                                             IList<DbModel.Data> dbSubDivisions,
                                                             IList<DbModel.Data> dbStatuses,
                                                             IList<DbModel.Data> dbActions,
                                                             IList<DbModel.Data> dbEmploymentTypes,
                                                             IList<DbModel.Country> dbCountries,
                                                             IList<DbModel.User> dbUser,
                                                             IList<DbModel.User> dbLoginUser)
        {
            return _mapper.Map<IList<DbModel.TechnicalSpecialist>>(tsInfos, opt =>
             {
                 opt.Items["DbCompanies"] = dbCompanies;
                 opt.Items["DbCompPayrolls"] = dbCompanyPayrolls;
                 opt.Items["DbSubDivisions"] = dbSubDivisions;
                 opt.Items["DbStatuses"] = dbStatuses;
                 opt.Items["DbActions"] = dbActions;
                 opt.Items["DbEmploymentTypes"] = dbEmploymentTypes;
                 opt.Items["DbCountries"] = dbCountries;
                 opt.Items["DbUser"] = dbUser;
                 opt.Items["DbLoginUser"] = dbLoginUser; 
             }).Select(ts=> {
                 if ((ts.IsTsCredSent == false || ts.IsTsCredSent == null) && dbActions?.FirstOrDefault(x => x.Id == ts.ProfileActionId)?.Name == TechnicalSpecialistConstants.Profile_Action_Send_To_TS)
                 {
                     ts.IsTsCredSent = true;//def 978 SLNO 1 
                 }
                 return ts;
             }).ToList(); 
        }

        #endregion


        private Response ProcessEmailNotifications(TechnicalSpecialistInfo technicalSpecialistInfo, EmailTemplate emailTemplateType, List<KeyValuePair<string, string>> emailContentPlaceholders = null, string statusFrom = null, string statusTo = null, string tsEmail = null)
        {
            string emailSubject = string.Empty;
            Exception exception = null;
            EmailQueueMessage emailMessage = null;
            List<EmailAddress> fromAddresses = null;
            List<EmailAddress> toAddresses = null;
            List<EmailAddress> ccAddresses = null;
            List<string> userTypes = null;
            try
            {
                emailContentPlaceholders = emailContentPlaceholders ?? new List<KeyValuePair<string, string>>();
                if (technicalSpecialistInfo != null)
                {
                    switch (emailTemplateType)
                    {
                        case EmailTemplate.EmailProfileActivation:
                            userTypes = new List<string> { TechnicalSpecialistConstants.User_Type_OC, TechnicalSpecialistConstants.User_Type_OM, TechnicalSpecialistConstants.User_Type_RC, TechnicalSpecialistConstants.User_Type_TM };

                            var userUserTypeData = _userService.GetUsersByTypeAndCompany(technicalSpecialistInfo?.CompanyCode, userTypes);
                            var toUsers = userUserTypeData?.Where(x => x.UserTypeName == TechnicalSpecialistConstants.User_Type_OC)?
                                                                .Select(x1 => new DbModel.User { Id = x1.User.Id, Name = x1.User.Name, SamaccountName = x1.User.SamaccountName, Email = x1.User.Email })?.GroupBy(x2 => x2.Id)?.Select(x3 => x3.FirstOrDefault())?.ToList();
                            var ccUsers = userUserTypeData?.Where(x => x.UserTypeName != TechnicalSpecialistConstants.User_Type_OC)?
                                                                .Select(x1 => new DbModel.User { Id = x1.User.Id, Name = x1.User.Name, SamaccountName = x1.User.SamaccountName, Email = x1.User.Email })?.GroupBy(x2 => x2.Id)?.Select(x3 => x3.FirstOrDefault())?.ToList();
                            var fromUsers = userUserTypeData?.Where(x => x.UserTypeName == TechnicalSpecialistConstants.User_Type_RC && x.User.SamaccountName == technicalSpecialistInfo?.AssignedByUser)?
                                                                .Select(x1 => new DbModel.User { Id = x1.User.Id, Name = x1.User.Name, SamaccountName = x1.User.SamaccountName, Email = x1.User.Email })?.GroupBy(x2 => x2.Id)?.Select(x3 => x3.FirstOrDefault())?.ToList();

                            emailSubject = string.Format(TechnicalSpecialistConstants.Email_Notification_ProfileActivation_Subject, technicalSpecialistInfo?.Epin);

                            fromAddresses = fromUsers?.Select(x => new EmailAddress() { DisplayName = x.Name, Address = x.Email }).ToList();
                            toAddresses = toUsers?.Select(x => new EmailAddress() { DisplayName = x.Name, Address = x.Email }).ToList();
                            //ccAddresses = ccUsers?.Where(x => !toUsers.Any(y => y.Id == x.Id))?.Select(x1 => new EmailAddress() { DisplayName = x1.Name, Address = x1.Email }).ToList();
                            ccAddresses = ccUsers?.Select(x1 => new EmailAddress() { DisplayName = x1.Name, Address = x1.Email }).ToList();// Def 978 (cc count is less issue fix)
                            emailMessage = _emailService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, emailTemplateType, EmailType.NRP, ModuleCodeType.TS, technicalSpecialistInfo?.Epin.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses, fromAddresses: fromAddresses, IsMailSendAsGroup: true);//def 1163 fix
                            break;

                        case EmailTemplate.EmailProfileStatusChange:
                            emailSubject = TechnicalSpecialistConstants.Email_Notification_ProfileStatusChange_Subject;
                            toAddresses = new List<EmailAddress> { new EmailAddress { Address = tsEmail } };
                            emailMessage = _emailService.PopulateEmailQueueMessage(ModuleType.TechnicalSpecialist, emailTemplateType, EmailType.PSC, ModuleCodeType.TS, technicalSpecialistInfo?.Epin.ToString(), emailSubject, emailContentPlaceholders, toAddresses, ccAddresses);
                            break;

                    }

                    return _emailService.Add(new List<EmailQueueMessage> { emailMessage });

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
    }
}