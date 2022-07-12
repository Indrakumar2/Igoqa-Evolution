using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Company.Domain.Interfaces.Companies;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Validations;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistCalendarService : ITechnicalSpecialistCalendarService
    {
        private readonly IMapper _mapper = null;
        private readonly JObject _messages = null;
        private readonly IAppLogger<TechnicalSpecialistCalendarService> _logger = null;
        private readonly ITechnicalSpecialistCalendarRepository _tsCalendarRepository = null;
        private readonly ITechnicalSpecialistTimeOffRequestRepository _tsCalendarTimeOffRequestRepository = null;
        private readonly ITechnicalSpecialistCalendarValidationService _validationService = null;
        private readonly ITechnicalSpecialistService _technSpecServices = null;
        private readonly ICompanyService _companyServices = null;

        //private readonly IVisitService _visitService = null;

        #region Constructor

        public TechnicalSpecialistCalendarService(IMapper mapper,
                                                        JObject messages,
                                                        IAppLogger<TechnicalSpecialistCalendarService> logger,
                                                        ITechnicalSpecialistCalendarRepository tsCalendarRepository,
                                                        ITechnicalSpecialistCalendarValidationService validationService,
                                                        ITechnicalSpecialistService technSpecServices,
                                                        ICompanyService companyServices,
                                                        ITechnicalSpecialistTimeOffRequestRepository tsCalendarTimeOffRequestRepository)
        //IVisitService visitService)
        {
            _mapper = mapper;
            _messages = messages;
            _logger = logger;
            _tsCalendarRepository = tsCalendarRepository;
            _validationService = validationService;
            _technSpecServices = technSpecServices;
            _companyServices = companyServices;
            _tsCalendarTimeOffRequestRepository = tsCalendarTimeOffRequestRepository;
            // _visitService = visitService;
        }

        #endregion

        #region Get

        public IList<DbModel.TechnicalSpecialistCalendar> GetCalendar(TechnicalSpecialistCalendar technicalSpecialistCalendarModel)
        {
            IList<DbModel.TechnicalSpecialistCalendar> tsCalendarResult = null;
            try
            {
                tsCalendarResult = _tsCalendarRepository.Get(technicalSpecialistCalendarModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), technicalSpecialistCalendarModel);
            }

            return tsCalendarResult;
        }

        public Response Get(TechnicalSpecialistCalendar technicalSpecialistCalendarModel, bool isView = true)
        {
            IList<TechnicalSpecialistCalendar> tsCalendarResult = null;
            TechnicalSpecialistCalendarView tsCalendarViewResult = null;
            Exception exception = null;
            IList<BaseTechnicalSpecialistInfo> tsInfo = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.TechnicalSpecialistTimeOffRequest> tsTimeOffRequestInfo = null;
            IList<long> ptoIds = null;
            IList<int?> companyIds = null;
            IList<ValidationMessage> validationMessages = null;

            try
            {
                tsCalendarResult = _mapper.Map<IList<TechnicalSpecialistCalendar>>(_tsCalendarRepository.Get(technicalSpecialistCalendarModel));
                //To get PTO Leave Type
                ptoIds = tsCalendarResult.Where(s => s.CalendarType == CalendarType.PTO.ToString()).Select(id => id.CalendarRefCode).ToList();
                string[] includes = new string[] { "LeaveType" };
                tsTimeOffRequestInfo = _tsCalendarTimeOffRequestRepository.FindBy(x => ptoIds.Contains(x.Id), includes).ToList();
                if (isView)
                {
                    BaseTechnicalSpecialistInfo bTsSearchModel = new BaseTechnicalSpecialistInfo
                    {
                        CompanyCode = technicalSpecialistCalendarModel.CompanyCode
                    };
                    tsInfo = ((technicalSpecialistCalendarModel.CalendarType != CalendarType.VISIT.ToString()) && (technicalSpecialistCalendarModel.CalendarType != CalendarType.TIMESHEET.ToString())) ? _technSpecServices.Get(bTsSearchModel).Result.Populate<IList<BaseTechnicalSpecialistInfo>>() : null;
                    companyIds = tsCalendarResult.Select(id => id.CompanyId).ToList();
                    bool isCompanyValid = _companyServices.IsValidCompanyById(companyIds, ref dbCompanies, ref validationMessages);
                    if (isCompanyValid)
                    {
                        tsCalendarViewResult = _mapper.Map<TechnicalSpecialistCalendarView>(tsCalendarResult, opts =>
                        {
                            opts.Items["tsInfo"] = tsInfo;
                            opts.Items["tsTimeOffRequestInfo"] = tsTimeOffRequestInfo;
                            opts.Items["dbCompanies"] = dbCompanies;
                        });
                    }
                    else
                    {
                        tsCalendarViewResult = _mapper.Map<TechnicalSpecialistCalendarView>(tsCalendarResult, opts =>
                            {
                                opts.Items["tsInfo"] = tsInfo;
                            });
                    }
                    if (tsCalendarViewResult != null)
                    {
                        tsCalendarViewResult.Resources = tsCalendarViewResult.Resources?.OrderBy(resource => resource.Name).ToList();
                        tsCalendarViewResult.Events = tsCalendarViewResult.Events?.OrderBy(events => events.Start).ToList();
                    }
                    return new Response().ToPopulate(ResponseType.Success, null, null, null, tsCalendarViewResult, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), technicalSpecialistCalendarModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, tsCalendarResult, exception, tsCalendarResult?.Count);
        }

        public Response SearchGet(TechnicalSpecialistCalendar technicalSpecialistCalendarModel, bool isView = true)
        {
            IList<TechnicalSpecialistCalendar> tsCalendarResult = null;
            TechnicalSpecialistCalendarView tsCalendarViewResult = null;
            Exception exception = null;
            IList<BaseTechnicalSpecialistInfo> tsInfo = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.TechnicalSpecialistTimeOffRequest> tsTimeOffRequestInfo = null;
            IList<int?> companyIds = null;
            IList<long> ptoIds = null;
            IList<ValidationMessage> validationMessages = null;

            try
            {
                tsCalendarResult = _mapper.Map<IList<TechnicalSpecialistCalendar>>(_tsCalendarRepository.CalendarSearchGet(technicalSpecialistCalendarModel));
                if (isView)
                {
                    //To get PTO Leave Type {Afetr discussing with Ramesh this is included inide}
                    ptoIds = tsCalendarResult.Where(s => s.CalendarType == CalendarType.PTO.ToString()).Select(id => id.CalendarRefCode).ToList();
                    string[] includes = new string[] { "LeaveType" };
                    tsTimeOffRequestInfo = _tsCalendarTimeOffRequestRepository.FindBy(x => ptoIds.Contains(x.Id), includes).ToList();

                    BaseTechnicalSpecialistInfo bTsSearchModel = new BaseTechnicalSpecialistInfo
                    {
                        CompanyCode = technicalSpecialistCalendarModel.CompanyCode
                    };
                    tsInfo = _technSpecServices.Get(bTsSearchModel).Result.Populate<IList<BaseTechnicalSpecialistInfo>>();
                    if (tsInfo != null && tsInfo.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(technicalSpecialistCalendarModel.ResourceName))
                        {
                            tsInfo = tsInfo.Where(ts => string.Format("{0} {1}", !string.IsNullOrEmpty(ts.LastName) ? ts.LastName.ToLower().Trim() : string.Empty, !string.IsNullOrEmpty(ts.FirstName) ? ts.FirstName.ToLower().Trim() : string.Empty).Contains(technicalSpecialistCalendarModel.ResourceName.ToLower()))?.ToList();
                        }
                        if (!string.IsNullOrEmpty(technicalSpecialistCalendarModel.CustomerName) || !string.IsNullOrEmpty(technicalSpecialistCalendarModel.SupplierName) || !string.IsNullOrEmpty(technicalSpecialistCalendarModel.SupplierLocation))
                        {
                            tsInfo = tsInfo.Join(tsCalendarResult,
                                tsInfo1 => new { techId = Convert.ToInt32(tsInfo1.Id) },
                            tsCalendarResult1 => new { techId = tsCalendarResult1.TechnicalSpecialistId },
                            (tsInfo1, tsCalendarResult1) => new { tsInfo1, tsCalendarResult1 }).Select(tsCalendarInfo => tsCalendarInfo.tsInfo1).Distinct().ToList();
                        }
                        if (tsCalendarResult.Count > 0)
                        {
                            companyIds = tsCalendarResult.Select(id => id.CompanyId).ToList();
                            bool isCompanyValid = _companyServices.IsValidCompanyById(companyIds, ref dbCompanies, ref validationMessages);
                            if (isCompanyValid)
                            {
                                tsCalendarViewResult = _mapper.Map<TechnicalSpecialistCalendarView>(tsCalendarResult, opts =>
                                {
                                    opts.Items["tsInfo"] = tsInfo;
                                    opts.Items["tsTimeOffRequestInfo"] = tsTimeOffRequestInfo;
                                    opts.Items["dbCompanies"] = dbCompanies;
                                });
                            }
                        }
                        else
                        {
                            tsCalendarViewResult = _mapper.Map<TechnicalSpecialistCalendarView>(tsCalendarResult, opts =>
                                {
                                    opts.Items["tsInfo"] = tsInfo;
                                });
                        }
                        if (tsCalendarViewResult != null)
                        {
                            tsCalendarViewResult.Resources = tsCalendarViewResult.Resources?.OrderBy(resource => resource.Name).ToList();
                            tsCalendarViewResult.Events = tsCalendarViewResult.Events?.OrderBy(events => events.Start).ToList();
                        }
                    }
                    return new Response().ToPopulate(ResponseType.Success, null, null, null, tsCalendarViewResult, exception, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), technicalSpecialistCalendarModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, tsCalendarResult, exception, tsCalendarResult?.Count);
        }

        public string GetJobReference(string calendarType, long calendarRefCode)
        {
            return _tsCalendarRepository.GetJobReference(calendarType, calendarRefCode);
        }

        public Response GetCalendarByTechnicalSpecialistId(TechnicalSpecialistCalendar technicalSpecialistCalendarModel, bool isCalendarDataOnly = false)
        {
            IList<TechnicalSpecialistCalendar> dbTsCalendarInfos = null;
            TechnicalSpecialistCalendarView tsCalendarViewResult = null;
            IList<BaseTechnicalSpecialistInfo> tsInfo = null;
            IList<DbModel.Company> dbCompanies = null;
            IList<DbModel.TechnicalSpecialistTimeOffRequest> tsTimeOffRequestInfo = null;
            IList<long> ptoIds = null;
            IList<int?> companyIds = null;
            IList<ValidationMessage> validationMessages = null;

            if (technicalSpecialistCalendarModel?.TechnicalSpecialistIds?.Count > 0)
            {
                if (isCalendarDataOnly)
                {
                    dbTsCalendarInfos = _mapper.Map<IList<TechnicalSpecialistCalendar>>(_tsCalendarRepository.FindBy(x => (technicalSpecialistCalendarModel.TechnicalSpecialistIds.Contains(x.TechnicalSpecialistId) && technicalSpecialistCalendarModel.CalendarRefCode == x.CalendarRefCode) && x.IsActive).ToList());
                    return new Response().ToPopulate(ResponseType.Success, null, null, null, dbTsCalendarInfos, null, null);
                }
                else
                {
                    dbTsCalendarInfos = _mapper.Map<IList<TechnicalSpecialistCalendar>>(_tsCalendarRepository.FindBy(x => technicalSpecialistCalendarModel.TechnicalSpecialistIds.Contains(x.TechnicalSpecialistId))?.Where(y => ((Convert.ToDateTime(y.StartDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date && Convert.ToDateTime(y.StartDateTime).Date <= Convert.ToDateTime(technicalSpecialistCalendarModel.EndDateTime).Date) ||
                        (Convert.ToDateTime(y.EndDateTime).Date >= Convert.ToDateTime(technicalSpecialistCalendarModel.StartDateTime).Date)) && y.IsActive).ToList());
                    //To get PTO Leave Type
                    ptoIds = dbTsCalendarInfos.Where(s => s.CalendarType == CalendarType.PTO.ToString()).Select(id => Convert.ToInt64(id.CalendarRefCode)).ToList();
                    string[] includes = new string[] { "LeaveType" };
                    tsTimeOffRequestInfo = _tsCalendarTimeOffRequestRepository.FindBy(x => ptoIds.Contains(x.Id), includes).ToList();
                    BaseTechnicalSpecialistInfo bTsSearchModel = new BaseTechnicalSpecialistInfo
                    {
                        CompanyCode = technicalSpecialistCalendarModel.CompanyCode
                    };
                    tsInfo = ((technicalSpecialistCalendarModel.CalendarType != CalendarType.VISIT.ToString()) || (technicalSpecialistCalendarModel.CalendarType != CalendarType.TIMESHEET.ToString())) ? null : _technSpecServices.Get(bTsSearchModel).Result.Populate<IList<BaseTechnicalSpecialistInfo>>();
                    companyIds = dbTsCalendarInfos.Select(id => id.CompanyId).Distinct().ToList();
                    bool isCompanyValid = _companyServices.IsValidCompanyById(companyIds, ref dbCompanies, ref validationMessages);
                    if (isCompanyValid)
                    {
                        tsCalendarViewResult = _mapper.Map<TechnicalSpecialistCalendarView>(dbTsCalendarInfos, opts =>
                        {
                            opts.Items["tsInfo"] = tsInfo;
                            opts.Items["tsTimeOffRequestInfo"] = tsTimeOffRequestInfo;
                            opts.Items["dbCompanies"] = dbCompanies;
                        });
                    }
                    else
                    {
                        tsCalendarViewResult = _mapper.Map<TechnicalSpecialistCalendarView>(dbTsCalendarInfos, opts =>
                        {
                            opts.Items["tsInfo"] = tsInfo;
                        });
                    }
                    if (tsCalendarViewResult != null)
                    {
                        tsCalendarViewResult.Resources = tsCalendarViewResult.Resources?.OrderBy(resource => resource.Name).ToList();
                        tsCalendarViewResult.Events = tsCalendarViewResult.Events?.OrderBy(events => events.Start).ToList();
                    }
                }
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, tsCalendarViewResult, null, null);
        }

        #endregion

        #region Post

        public Response Save(IList<TechnicalSpecialistCalendar> technicalSpecialistCalendarModel, bool commitChange = true, bool isVisitSave = false, IList<DbModel.Company> dbCompanies = null)
        {
            Response tsCalendarResult = null;
            Exception exception = null;
            try
            {
                tsCalendarResult = AddTsCalendar(technicalSpecialistCalendarModel, dbCompanies, commitChange, isVisitSave);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), technicalSpecialistCalendarModel);
            }

            return tsCalendarResult;
        }


        public Response Save(IList<DbModel.TechnicalSpecialistCalendar> technicalSpecialistCalendars, bool commitChange = true)
        {
            Exception exception = null;
            try
            {
                if (technicalSpecialistCalendars != null && technicalSpecialistCalendars?.Count > 0)
                {
                    _tsCalendarRepository.AutoSave = false;
                    technicalSpecialistCalendars = technicalSpecialistCalendars.Select(x =>
                    {
                        x.CreatedDate = DateTime.UtcNow;
                        return x;
                    }).ToList();
                    _tsCalendarRepository.Add(technicalSpecialistCalendars);
                    if (commitChange)
                    {
                        _tsCalendarRepository.ForceSave();
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), technicalSpecialistCalendars);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception, null);
        }

        //public Response Save(IList<TechnicalSpecialistCalendar> technicalSpecialistCalendarModel, bool commitChange = true,
        //                 bool isDbValidationRequire = true)
        //{ }
        #endregion

        #region Put

        public Response Update(IList<TechnicalSpecialistCalendar> technicalSpecialistCalendarModel, bool commitChange = true, bool isVisitSave = false, IList<DbModel.Company> dbCompanies = null)
        {
            Response tsCalendarResult = null;
            Exception exception = null;
            try
            {
                tsCalendarResult = UpdateTsCalendar(technicalSpecialistCalendarModel, dbCompanies, commitChange, isVisitSave);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), technicalSpecialistCalendarModel);
            }

            return tsCalendarResult;
        }
        #endregion

        #region Delete

        public Response Delete(IList<TechnicalSpecialistCalendar> technicalSpecialistCalendarModel, bool commitChange = true, bool isVisitSave = false)
        {
            Response tsCalendarResult = null;
            Exception exception = null;
            try
            {
                tsCalendarResult = Remove(technicalSpecialistCalendarModel, commitChange, isVisitSave);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), technicalSpecialistCalendarModel);
            }

            return tsCalendarResult;
        }
        #endregion

        #region Private Methods

        private Response AddTsCalendar(IList<TechnicalSpecialistCalendar> technicalSpecialistCalendarModel, IList<DbModel.Company> dbCompanies, bool commitChange = true, bool isVisitSave = false)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialistCalendar> tsCalendarInfo = null;
            try
            {

                IList<TechnicalSpecialistCalendar> recordToBeAdd = null;
                if (!isVisitSave)
                {
                    isVisitSave = CheckRecordValidForProcess(technicalSpecialistCalendarModel, ValidationType.Add, ref recordToBeAdd, ref dbTechnicalSpecialists, ref validationMessages, ref tsCalendarInfo, ref dbCompanies);
                }
                if (isVisitSave)
                {
                    _tsCalendarRepository.AutoSave = false;
                    var recordToBeInserted = _mapper.Map<IList<DbModel.TechnicalSpecialistCalendar>>(technicalSpecialistCalendarModel, opt =>
                    {
                        opt.Items["dbCompanies"] = dbCompanies;
                    }
                    );
                    recordToBeInserted.ToList().ForEach(calendar =>
                    {
                        calendar.Id = 0;
                    });
                    _tsCalendarRepository.Add(recordToBeInserted);
                    if (commitChange)
                    {
                        var savedCnt = _tsCalendarRepository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), technicalSpecialistCalendarModel);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, validationMessages, exception, null);
        }

        private Response UpdateTsCalendar(IList<TechnicalSpecialistCalendar> technicalSpecialistCalendarModel, IList<DbModel.Company> dbCompanies, bool commitChange = true,
                           bool isDbValidationRequire = true, bool isVisitSave = false)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.TechnicalSpecialistCalendar> tsCalendarInfo = null;
            try
            {
                IList<TechnicalSpecialistCalendar> recordToBeAdd = null;
                if (!isVisitSave)
                {
                    isVisitSave = CheckRecordValidForProcess(technicalSpecialistCalendarModel, ValidationType.Update, ref recordToBeAdd, ref dbTechnicalSpecialists, ref validationMessages, ref tsCalendarInfo, ref dbCompanies);
                }
                if (isVisitSave)
                {
                    _tsCalendarRepository.AutoSave = false;
                    tsCalendarInfo.ToList().ForEach(technicalSpecialistCalendar =>
                    {
                        var tsToBeModify = technicalSpecialistCalendarModel.FirstOrDefault(x => x.Id == technicalSpecialistCalendar.Id);
                        _mapper.Map(tsToBeModify, technicalSpecialistCalendar, opt =>
                    {
                        opt.Items["dbCompanies"] = dbCompanies;
                    });
                        technicalSpecialistCalendar.LastModification = DateTime.UtcNow;
                        technicalSpecialistCalendar.UpdateCount = tsToBeModify.UpdateCount.CalculateUpdateCount();
                        technicalSpecialistCalendar.ModifiedBy = tsToBeModify.ModifiedBy;
                    });
                    _tsCalendarRepository.Update(tsCalendarInfo);
                    if (commitChange)
                    {
                        var savedCnt = _tsCalendarRepository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), technicalSpecialistCalendarModel);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, validationMessages, exception, null);
        }

        private Response Remove(IList<TechnicalSpecialistCalendar> technicalSpecialistCalendarModel, bool commitChange = true,
                         bool isDbValidationRequire = true, bool isVisitSave = false)
        {
            Exception exception = null;
            IList<DbModel.TechnicalSpecialistCalendar> tsCalendarInfo = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.Company> dbCompanies = null;
            try
            {
                IList<TechnicalSpecialistCalendar> recordToBeAdd = null;
                if (!isVisitSave)
                {
                    isVisitSave = CheckRecordValidForProcess(technicalSpecialistCalendarModel, ValidationType.Delete, ref recordToBeAdd, ref dbTechnicalSpecialists, ref validationMessages, ref tsCalendarInfo, ref dbCompanies);
                }

                if (isVisitSave)
                {
                    IList<long?> tsCalendarIds = technicalSpecialistCalendarModel.Select(x => x.Id).ToList();
                    bool result = Convert.ToBoolean(IsRecordExistInDbById(tsCalendarIds, ref tsCalendarInfo, ref validationMessages).Result);
                    if (result)
                    {
                        _tsCalendarRepository.AutoSave = false;
                        tsCalendarInfo = tsCalendarInfo.Select(technicalSpecialistCalendar =>
                         {
                             technicalSpecialistCalendar.IsActive = false;
                             technicalSpecialistCalendar.LastModification = DateTime.UtcNow;
                             technicalSpecialistCalendar.UpdateCount = technicalSpecialistCalendar.UpdateCount.CalculateUpdateCount();
                             return technicalSpecialistCalendar;
                         }).ToList();

                        _tsCalendarRepository.Update(tsCalendarInfo);
                        if (commitChange)
                        {
                            var savedCnt = _tsCalendarRepository.ForceSave();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), technicalSpecialistCalendarModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, validationMessages, exception, null); ;
        }

        public Response UpdateCalendar(IList<DbModel.TechnicalSpecialistCalendar> dbTechSpecialistCalender, bool commitChange = true)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            IList<DbModel.TechnicalSpecialistCalendar> tsCalendarInfo = null;
            try
            {
                if (dbTechSpecialistCalender?.Any() == true)
                {
                    _tsCalendarRepository.AutoSave = false;
                    tsCalendarInfo = dbTechSpecialistCalender.Select(technicalSpecialistCalendar =>
                    {
                        technicalSpecialistCalendar.IsActive = false;
                        technicalSpecialistCalendar.LastModification = DateTime.UtcNow;
                        technicalSpecialistCalendar.UpdateCount = technicalSpecialistCalendar.UpdateCount.CalculateUpdateCount();
                        return technicalSpecialistCalendar;
                    }).ToList();
                    _tsCalendarRepository.Update(tsCalendarInfo, a => a.IsActive, b => b.LastModification, c => c.UpdateCount);
                    if (commitChange)
                        _tsCalendarRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception, null); ;
        }

        public bool CheckRecordValidForProcess(IList<TechnicalSpecialistCalendar> technicalSpecialistCalendarModel, ValidationType validationType, ref IList<TechnicalSpecialistCalendar> filteredTsInfos, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, ref IList<ValidationMessage> validationMessages, ref IList<DbModel.TechnicalSpecialistCalendar> tsCalendarInfo, ref IList<DbModel.Company> dbCompanies)
        {
            Exception exception = null;
            bool result = false;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(technicalSpecialistCalendarModel, ref validationMessages, ref filteredTsInfos, ref dbTechnicalSpecialists, ref dbCompanies);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(technicalSpecialistCalendarModel, ref validationMessages, ref filteredTsInfos, ref dbTechnicalSpecialists, ref dbCompanies);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(technicalSpecialistCalendarModel, ref validationMessages, ref filteredTsInfos, ref dbTechnicalSpecialists, ref tsCalendarInfo, ref dbCompanies);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), technicalSpecialistCalendarModel);
            }
            return result;
        }

        private bool IsRecordValidForAdd(IList<TechnicalSpecialistCalendar> technicalSpecialistCalendarModel, ref IList<ValidationMessage> validationMessages, ref IList<TechnicalSpecialistCalendar> filteredTsInfos, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, ref IList<DbModel.Company> dbCompanies)
        {
            bool result = true;
            if (technicalSpecialistCalendarModel != null && technicalSpecialistCalendarModel.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;

                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsInfos?.Count <= 0 || filteredTsInfos == null)
                    filteredTsInfos = FilterRecord(technicalSpecialistCalendarModel, validationType);

                if (filteredTsInfos?.Count > 0 && IsValidPayload(filteredTsInfos, validationType, ref validationMessages))
                {
                    IList<int> tsId = filteredTsInfos.Select(x => (int)x.TechnicalSpecialistId).ToList();
                    result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDbById(tsId, ref dbTechnicalSpecialists, ref validationMessages).Result);
                    if (result)
                    {
                        IList<string> companyCodes = filteredTsInfos.Select(x => x.CompanyCode).ToList();
                        if (dbCompanies == null || dbCompanies.Count == 0)
                            result = _companyServices.IsValidCompany(companyCodes, ref dbCompanies, ref validationMessages);
                    }
                }
            }
            return result;
        }

        private bool IsRecordValidForRemove(IList<TechnicalSpecialistCalendar> technicalSpecialistCalendarModel, ref IList<ValidationMessage> validationMessages, ref IList<TechnicalSpecialistCalendar> filteredTsInfos, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, ref IList<DbModel.Company> dbCompanies)
        {
            bool result = true;
            if (technicalSpecialistCalendarModel != null && technicalSpecialistCalendarModel.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;

                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsInfos?.Count <= 0 || filteredTsInfos == null)
                    filteredTsInfos = FilterRecord(technicalSpecialistCalendarModel, validationType);

                if (filteredTsInfos?.Count > 0 && IsValidPayload(filteredTsInfos, validationType, ref validationMessages))
                {
                    return true;
                }

            }
            return result;
        }
        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistCalendar> technicalSpecialistCalendarModel, ref IList<ValidationMessage> validationMessages, ref IList<TechnicalSpecialistCalendar> filteredTsInfos, ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists, ref IList<DbModel.TechnicalSpecialistCalendar> tsCalendarInfo, ref IList<DbModel.Company> dbCompanies)
        {
            bool result = true;
            if (technicalSpecialistCalendarModel != null && technicalSpecialistCalendarModel.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;

                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsInfos?.Count <= 0 || filteredTsInfos == null)
                    filteredTsInfos = FilterRecord(technicalSpecialistCalendarModel, validationType);

                if (filteredTsInfos?.Count > 0 && IsValidPayload(filteredTsInfos, validationType, ref validationMessages))
                {
                    IList<long?> calendarIds = filteredTsInfos.Select(x => x.Id).ToList();
                    result = Convert.ToBoolean(IsRecordExistInDbById(calendarIds, ref tsCalendarInfo, ref validationMessages).Result);
                    if (result)
                    {
                        IList<int> tsId = filteredTsInfos.Select(x => x.TechnicalSpecialistId).ToList();
                        result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDbById(tsId, ref dbTechnicalSpecialists, ref validationMessages).Result);
                        if (result)
                        {
                            IList<string> companyCodes = filteredTsInfos.Select(x => x.CompanyCode).ToList();
                            result = _companyServices.IsValidCompany(companyCodes, ref dbCompanies, ref validationMessages);
                        }
                    }
                }
            }
            return result;
        }

        public Response IsRecordExistInDbById(IList<long?> calendarIds,
                                          ref IList<DbModel.TechnicalSpecialistCalendar> tsCalendarInfo,
                                          ref IList<ValidationMessage> validationMessages,
                                          params Expression<Func<DbModel.TechnicalSpecialistCalendar, object>>[] includes)
        {
            Exception exception = null;
            bool result = true;
            IList<long?> calendarIdNotExists = null;
            try
            {
                if (tsCalendarInfo?.Count == 0 || tsCalendarInfo == null)
                    tsCalendarInfo = GetTechSpecialistCalendarById(calendarIds, includes);

                result = IsTechSpecialistCalendarExistInDbById(calendarIds, tsCalendarInfo, ref calendarIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), calendarIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private IList<DbModel.TechnicalSpecialistCalendar> GetTechSpecialistCalendarById(IList<long?> calendarIds,
                                                                         params Expression<Func<DbModel.TechnicalSpecialistCalendar, object>>[] includes)
        {
            IList<DbModel.TechnicalSpecialistCalendar> dbTsInfos = null;
            if (calendarIds?.Count > 0)
                dbTsInfos = _tsCalendarRepository.FindBy(x => calendarIds.Contains(x.Id), includes).ToList();

            return dbTsInfos;
        }

        private bool IsTechSpecialistCalendarExistInDbById(IList<long?> calendarIds,
                                                IList<DbModel.TechnicalSpecialistCalendar> tsCalendarInfo,
                                                ref IList<long?> calendarIdNotExists,
                                                ref IList<ValidationMessage> validationMessages,
                                                params Expression<Func<DbModel.TechnicalSpecialistCalendar, object>>[] includes)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (tsCalendarInfo == null)
                tsCalendarInfo = new List<DbModel.TechnicalSpecialistCalendar>();

            var validMessages = validationMessages;

            if (calendarIds?.Count > 0)
            {
                calendarIdNotExists = calendarIds.Where(id => !tsCalendarInfo.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                calendarIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.InvalidTechSpecialist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private IList<TechnicalSpecialistCalendar> FilterRecord(IList<TechnicalSpecialistCalendar> tsInfos, ValidationType filterType)
        {
            IList<TechnicalSpecialistCalendar> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistCalendar> technicalSpecialistCalendarModel,
                                    ValidationType validationType,
                                    ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(technicalSpecialistCalendarModel), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.Calendar, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        #endregion
    }
}
