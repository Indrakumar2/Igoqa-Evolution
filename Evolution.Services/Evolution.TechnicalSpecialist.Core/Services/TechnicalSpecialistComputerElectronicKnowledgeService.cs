using AutoMapper;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Extensions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Master.Domain.Interfaces.Services;
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
    public class TechnicalSpecialistComputerElectronicKnowledgeService : ITechnicalSpecialistComputerElectronicKnowledgeService
    {
        private readonly IAppLogger<TechnicalSpecialistComputerElectronicKnowledgeService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ITechnicalSpecialistComputerElectronicKnowledgeServiceRepository _repository = null;
        private readonly JObject _messages = null;
        private readonly ITechnicalSpecialistComputerElectronicKnowledgeValidationService _validationService = null;
        private readonly IMasterService _masterService = null;
        private readonly IComputerKnowledgeService _computerKnowledgeService = null;
        private readonly IAuditSearchService _auditSearchService = null;
        //private readonly ITechnicalSpecialistService _technicalSpecialistInfoServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;

        #region Constructor
        public TechnicalSpecialistComputerElectronicKnowledgeService(IMapper mapper,
                                                    ITechnicalSpecialistComputerElectronicKnowledgeServiceRepository repository,
                                                    IAppLogger<TechnicalSpecialistComputerElectronicKnowledgeService> logger,
                                                    ITechnicalSpecialistComputerElectronicKnowledgeValidationService validationService,
                                                    IMasterService masterService,
                                                    IComputerKnowledgeService computerKnowledgeService,
                                                    //ITechnicalSpecialistService technicalSpecialistInfoServices,
                                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                    JObject messages,
                                                     IAuditSearchService auditSearchService

                                                     )
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _masterService = masterService;
            _computerKnowledgeService = computerKnowledgeService;
            //_technicalSpecialistInfoServices = technicalSpecialistInfoServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _messages = messages;
            _auditSearchService = auditSearchService;

        }
        #endregion

        #region Public Methods

        #region Get

        public Response Get(TechnicalSpecialistComputerElectronicKnowledgeInfo searchModel)
        {
            IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistComputerElectronicKnowledgeInfo>>(_repository.Search(searchModel));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByPinId(IList<string> pins)
        {
            IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistComputerElectronicKnowledgeInfo>>(GetComputerKnowledgeInfoByPin(pins));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), pins);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> Ids)
        {
            IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistComputerElectronicKnowledgeInfo>>(GetComputerKnowladgeById(Ids));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), Ids);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> ComputerKnowledge)
        {
            IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistComputerElectronicKnowledgeInfo>>(GetComputerKnowledgeInfotByComputerKnowledge(ComputerKnowledge));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), ComputerKnowledge);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndComputerKnowledge,
                                    ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsComputerKnowledgeInfos,
                                    ref IList<ValidationMessage> validationMessages)
        {
            IList<KeyValuePair<string, string>> tsPinAndComputerKnowledgeNotExists = null;
            return IsRecordExistInDb(tsPinAndComputerKnowledge, ref dbtsComputerKnowledgeInfos, ref tsPinAndComputerKnowledgeNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<KeyValuePair<string, string>> tsPinAndComputerKnowledge,
                                    ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsComputerKnowledgeInfos,
                                    ref IList<KeyValuePair<string, string>> tsPinAndComputerKnowledgeNotExists,
                                    ref IList<ValidationMessage> validationMessages)
        {
            Exception exception = null;
            bool result = true;
            try
            {
                if (dbtsComputerKnowledgeInfos == null && tsPinAndComputerKnowledge?.Count > 0)
                    dbtsComputerKnowledgeInfos = GetComputerKnowledgeInfoByPin(tsPinAndComputerKnowledge.Select(x => x.Key).ToList());

                result = IsComputerElectronicKnowledgeInfoExistInDb(tsPinAndComputerKnowledge, dbtsComputerKnowledgeInfos, ref tsPinAndComputerKnowledgeNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsPinAndComputerKnowledge);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }


        #endregion

        #region Add

        public Response Add(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComElecKnowledgeInfos = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbComputerElectronicKnowledge = null;
            return AddTechSpecialistKnowledge(tsCompElecKnowledgeInfos, ref dbTsComElecKnowledgeInfos, ref dbTechnicalSpecialists, ref dbComputerElectronicKnowledge, commitChange, isDbValidationRequire);
        }

        public Response Add(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
                        ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsCompElecKnowledgeInfos,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbComputerElectronicKnowledge,
                        bool commitChange = true,
                        bool isDbValidationRequire = true)
        {
            return AddTechSpecialistKnowledge(tsCompElecKnowledgeInfos, ref dbtsCompElecKnowledgeInfos, ref dbTechnicalSpecialists, ref dbComputerElectronicKnowledge, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Modify
        public Response Modify(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
                                bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsCompElecKnowledgeInfos = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbComputerElectronicKnowledge = null;

            return UpdateTechSpecialistKnowledge(tsCompElecKnowledgeInfos, ref dbtsCompElecKnowledgeInfos, ref dbTechnicalSpecialists, ref dbComputerElectronicKnowledge, commitChange, isDbValidationRequire);
        }

        public Response Modify(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
                                ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsCompElecKnowledgeInfos,
                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                ref IList<DbModel.Data> dbComputerElectronicKnowledge,
                                bool commitChange = true,
                                bool isDbValidationRequire = true)
        {
            return UpdateTechSpecialistKnowledge(tsCompElecKnowledgeInfos, ref dbtsCompElecKnowledgeInfos, ref dbTechnicalSpecialists, ref dbComputerElectronicKnowledge, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Delete
        public Response Delete(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
                               bool commitChange = true,
                               bool isDbValidationRequire = true)
        {
            IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComputerElectronicKnowledgeInfos = null;
            return RemoveTechSpecialistisComputerElectronicKnowledge(tsCompElecKnowledgeInfos, ref dbTsComputerElectronicKnowledgeInfos, commitChange, isDbValidationRequire);
        }

        public Response Delete(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
             ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComputerElectronicKnowledgeInfos,
                            bool commitChange = true,
                            bool isDbValidationRequire = true)
        {
            return RemoveTechSpecialistisComputerElectronicKnowledge(tsCompElecKnowledgeInfos, ref dbTsComputerElectronicKnowledgeInfos, commitChange, isDbValidationRequire);
        }
        #endregion

        #region Record Valid Check
        public Response IsRecordValidForProcess(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
                                                ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsCompElecKnowledgeInfos = null;
            IList<DbModel.Data> dbCompElecKnowledges = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalpecialist = null;
            return IsRecordValidForProcess(tsCompElecKnowledgeInfos, validationType, ref dbTechnicalpecialist, ref dbCompElecKnowledges, ref dbtsCompElecKnowledgeInfos);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbComputerElectronicKnowledge,
                                                ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsCompElecKnowledgeInfos,
                                                bool isDraft = false)
        {
            IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> filteredTsCompElecKnowledge = null;
            return CheckRecordValidForProcess(tsCompElecKnowledgeInfos, validationType, ref filteredTsCompElecKnowledge, ref dbtsCompElecKnowledgeInfos, ref dbComputerElectronicKnowledge, ref dbTechnicalSpecialists, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsCompElecKnowledgeInfos,
                                                ref IList<DbModel.Data> dbcCompElecKnowledges,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalpecialist,
                                                bool isDraft = false)
        {
            IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> filteredTsCompElecKnowledge = null;
            return CheckRecordValidForProcess(tsCompElecKnowledgeInfos, validationType, ref filteredTsCompElecKnowledge, ref dbtsCompElecKnowledgeInfos, ref dbcCompElecKnowledges, ref dbTechnicalpecialist, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsCompElecKnowledgeInfos,
                                                ValidationType validationType,
                                                IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsCompElecKnowledgeInfos)
        {
            IList<DbModel.Data> dbCompElecKnowledges = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalpecialist = null;
            return IsRecordValidForProcess(tsCompElecKnowledgeInfos, validationType, ref dbTechnicalpecialist, ref dbCompElecKnowledges, ref dbtsCompElecKnowledgeInfos);
        }
        #endregion

        #endregion

        #region Private Methods

        #region Get
        private IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> GetComputerKnowledgeInfoByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsComputerKnowladgeInfos = null;
            if (pins?.Count > 0)
            {
                dbtsComputerKnowladgeInfos = _repository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbtsComputerKnowladgeInfos;
        }

        private IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> GetComputerKnowladgeById(IList<int> tsComputerKnowladgeIds)
        {
            IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsComputerKnowladgeInfos = null;
            if (tsComputerKnowladgeIds?.Count > 0)
                dbtsComputerKnowladgeInfos = _repository.FindBy(x => tsComputerKnowladgeIds.Contains(x.Id)).ToList();

            return dbtsComputerKnowladgeInfos;
        }

        private IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> GetComputerKnowledgeInfotByComputerKnowledge(IList<string> tsComputerKnowladge)
        {
            IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbtsComputerKnowladgeInfos = null;
            if (tsComputerKnowladge?.Count > 0)
                dbtsComputerKnowladgeInfos = _repository.FindBy(x => tsComputerKnowladge.Contains(x.ComputerKnowledge.ToString())).ToList();

            return dbtsComputerKnowladgeInfos;
        }


        #endregion

        #region Add
        private bool IsRecordValidForAdd(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsComputerElectronicKnowledgeInfos,
                                         ref IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> filteredTechSpecialistComputerElectronicKnowledge,
                                         ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbComputerElectronicKnowledgeInfos,
                                         ref IList<DbModel.Data> dbComputerKnowledge,
                                         ref IList<DbModel.TechnicalSpecialist> dbtechnicalspecialist,
                                         ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsComputerElectronicKnowledgeInfos != null && tsComputerElectronicKnowledgeInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTechSpecialistComputerElectronicKnowledge == null || filteredTechSpecialistComputerElectronicKnowledge.Count <= 0)
                    filteredTechSpecialistComputerElectronicKnowledge = FilterRecord(tsComputerElectronicKnowledgeInfos, validationType);

                if (filteredTechSpecialistComputerElectronicKnowledge?.Count > 0 && IsValidPayload(filteredTechSpecialistComputerElectronicKnowledge, validationType, ref validationMessages))
                {
                    IList<KeyValuePair<string, string>> tsComputerKnowledgeNotExists = null;
                    IList<string> computerKnowledgeName = filteredTechSpecialistComputerElectronicKnowledge.Select(x => x.ComputerKnowledge).ToList();
                    IList<string> epins = filteredTechSpecialistComputerElectronicKnowledge.Select(x => x.Epin.ToString()).ToList();

                    GetMasterData(filteredTechSpecialistComputerElectronicKnowledge, ref dbComputerKnowledge);

                    result = _computerKnowledgeService.IsValidComputerKnowledgeName(computerKnowledgeName, ref dbComputerKnowledge, ref validationMessages);
                    if (result && epins?.Count > 0)
                    {
                        //result = Convert.ToBoolean(_technicalSpecialistInfoServices.IsRecordExistInDb(epins, ref dbtechnicalspecialist, ref validationMessages, ts => ts.TechnicalSpecialistComputerElectronicKnowledge).Result);
                        result = IsTechSpecialistExistInDb(epins, ref dbtechnicalspecialist, ref validationMessages, ts => ts.TechnicalSpecialistComputerElectronicKnowledge);
                    }
                    if (result)
                    {
                        dbComputerElectronicKnowledgeInfos = dbtechnicalspecialist.SelectMany(x => x.TechnicalSpecialistComputerElectronicKnowledge).ToList();
                        var tsPinAnComputerKnowledge = tsComputerElectronicKnowledgeInfos.Select(x => new KeyValuePair<string, string>(x.Epin.ToString(), x.ComputerKnowledge))
                                                        .Distinct()
                                                        .ToList();
                        result = (IsComputerElectronicKnowledgeInfoExistInDb(tsPinAnComputerKnowledge, dbComputerElectronicKnowledgeInfos, ref tsComputerKnowledgeNotExists, ref validationMessages));
                    }
                }
            }
            return result;
        }

        private Response AddTechSpecialistKnowledge(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsComputerElectronicKnowledgeInfos,
                                                    ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComputerElectronicKnowledgeInfos,
                                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                    ref IList<DbModel.Data> dbComputerElectronicKnowledge,
                                                    bool commitChange = true,
                                                    bool isDbValidationRequire = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                IList<DbModel.Data> dbComputerKnowledge = null;
                IList<DbModel.TechnicalSpecialist> dbTechnicalspecialist = null;
                IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> recordToBeAdd = null;
                eventId = tsComputerElectronicKnowledgeInfos?.FirstOrDefault()?.EventId;
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(tsComputerElectronicKnowledgeInfos, ValidationType.Add, ref recordToBeAdd, ref dbTsComputerElectronicKnowledgeInfos, ref dbComputerElectronicKnowledge, ref dbTechnicalSpecialists);

                if (!isDbValidationRequire && tsComputerElectronicKnowledgeInfos?.Count > 0)
                {
                    recordToBeAdd = FilterRecord(tsComputerElectronicKnowledgeInfos, ValidationType.Add);
                }

                if (!isDbValidationRequire || Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    dbComputerKnowledge = dbComputerElectronicKnowledge;
                    dbTechnicalspecialist = dbTechnicalSpecialists;
                    recordToBeAdd = recordToBeAdd.Select(x => { x.Id = 0; return x; }).ToList();
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbComputerKnowledges"] = dbComputerKnowledge;
                        opt.Items["DbTechnicalspecialists"] = dbTechnicalspecialist;
                    });

                    _repository.Add(mappedRecords);

                    if (commitChange)
                        _repository.ForceSave();

                    dbTsComputerElectronicKnowledgeInfos = mappedRecords;
                    if (mappedRecords?.Count > 0)
                    {
                        mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, tsComputerElectronicKnowledgeInfos?.FirstOrDefault()?.ActionByUser,
                                                                                           null,
                                                                                           ValidationType.Add.ToAuditActionType(),
                                                                                           SqlAuditModuleType.TechnicalSpecialistComputerElectronicKnowledge,
                                                                                            null,
                                                                                            _mapper.Map<TechnicalSpecialistComputerElectronicKnowledgeInfo>(x1)
                                                                                                ));
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsComputerElectronicKnowledgeInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }
        #endregion

        #region Modify
        private Response UpdateTechSpecialistKnowledge(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsComputerElectronicKnowledgeInfos,
                                                ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbComputerElectronicKnowledgeInfos,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbComputerElectronicKnowledge,
                                                bool commitChange = true,
                                                bool isDbValidationRequire = true)
        {
            Exception exception = null;
            long? eventId = 0;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                IList<DbModel.Data> dbComputerKnowledge = null;
                IList<DbModel.TechnicalSpecialist> dbTechnicalspecialist = null;
                var recordToBeModify = FilterRecord(tsComputerElectronicKnowledgeInfos, ValidationType.Update);
                bool valdResult = false;
                eventId = tsComputerElectronicKnowledgeInfos?.FirstOrDefault().EventId;
                if (isDbValidationRequire)
                    valdResponse = CheckRecordValidForProcess(tsComputerElectronicKnowledgeInfos, ValidationType.Update, ref recordToBeModify, ref dbComputerElectronicKnowledgeInfos, ref dbComputerKnowledge, ref dbTechnicalspecialist);

                if (tsComputerElectronicKnowledgeInfos?.Count > 0)
                {
                    recordToBeModify = FilterRecord(tsComputerElectronicKnowledgeInfos, ValidationType.Update);
                }
                if (recordToBeModify?.Count > 0)
                {
                    if (dbComputerElectronicKnowledgeInfos == null || dbComputerElectronicKnowledgeInfos?.Count <= 0)
                        dbComputerElectronicKnowledgeInfos = _repository.Get(recordToBeModify?.Select(x => x.Id).ToList());

                    if (dbTechnicalSpecialists == null || (dbTechnicalSpecialists?.Count <= 0 && !valdResult))
                    {
                        //valdResult = Convert.ToBoolean(_technicalSpecialistInfoServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages).Result);
                        valdResult = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }
                    if (dbComputerElectronicKnowledge == null || (dbComputerElectronicKnowledge?.Count <= 0 && !valdResult))
                    {
                        _computerKnowledgeService.IsValidComputerKnowledgeName(recordToBeModify.Select(x => x.ComputerKnowledge).ToList(), ref dbComputerElectronicKnowledge, ref validationMessages);
                    }

                    if ((valdResponse == null || Convert.ToBoolean(valdResponse.Result) && dbComputerElectronicKnowledgeInfos?.Count > 0))
                    {
                        dbTechnicalspecialist = dbTechnicalSpecialists;
                        IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> domExsistingTechSplCompElectKnoweledge = new List<TechnicalSpecialistComputerElectronicKnowledgeInfo>();
                        
                        dbComputerKnowledge = dbComputerElectronicKnowledge;

                        dbComputerElectronicKnowledgeInfos.ToList().ForEach(dbTsComputerKnowledgeInfos =>
                        {
                            domExsistingTechSplCompElectKnoweledge.Add(ObjectExtension.Clone(_mapper.Map<TechnicalSpecialistComputerElectronicKnowledgeInfo>(dbTsComputerKnowledgeInfos)));
                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == dbTsComputerKnowledgeInfos.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, dbTsComputerKnowledgeInfos, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DbComputerKnowledges"] = dbComputerKnowledge;
                                    opt.Items["DbTechnicalspecialists"] = dbTechnicalspecialist;

                                });
                                dbTsComputerKnowledgeInfos.LastModification = DateTime.UtcNow;
                                dbTsComputerKnowledgeInfos.UpdateCount = dbTsComputerKnowledgeInfos.UpdateCount.CalculateUpdateCount();
                                dbTsComputerKnowledgeInfos.ModifiedBy = tsToBeModify.ModifiedBy;
                            }

                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbComputerElectronicKnowledgeInfos);
                        if (commitChange)
                        {
                            _repository.ForceSave();

                            if (recordToBeModify?.Count > 0)
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                  null,
                                                                                                   ValidationType.Update.ToAuditActionType(),
                                                                                                   SqlAuditModuleType.TechnicalSpecialistComputerElectronicKnowledge,
                                                                                                  domExsistingTechSplCompElectKnoweledge?.FirstOrDefault(x2 => x2.Id == x1.Id),
                                                                                                    x1
                                                                                                      ));
                        }
                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), dbComputerElectronicKnowledgeInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsComputerElectronicKnowledgeInfos,
                                            ref IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> filteredTsInfos,
                                            ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComputerElectronicKnowledgeInfos,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Data> dbcompKnowledge,
                                            ref IList<ValidationMessage> validationMessages,
                                            bool isDraft = false)
        {

            bool result = false;
            if (tsComputerElectronicKnowledgeInfos != null && tsComputerElectronicKnowledgeInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsInfos == null || filteredTsInfos.Count <= 0)
                    filteredTsInfos = FilterRecord(tsComputerElectronicKnowledgeInfos, validationType);

                if (filteredTsInfos?.Count > 0 && IsValidPayload(filteredTsInfos, validationType, ref messages))
                {
                    GetTsComputerKnowledgeDbInfo(filteredTsInfos, true, ref dbTsComputerElectronicKnowledgeInfos);
                    IList<int> tsIds = filteredTsInfos.Select(x => x.Id).ToList();
                    IList<int> tsDBCompElectrIds = dbTsComputerElectronicKnowledgeInfos.Select(x => x.Id).ToList();
                    if (tsIds.Any(x => !tsDBCompElectrIds.Contains(x))) //Invalid Id found.
                    {
                        var dbTsComElecKnowledgeInfosByIds = dbTsComputerElectronicKnowledgeInfos;
                        var idNotExists = tsIds.Where(id => !dbTsComElecKnowledgeInfosByIds.Any(ts => ts.Id == id)).ToList();
                        var tsKnowledgeList = filteredTsInfos;
                        idNotExists?.ForEach(tsId =>
                        {
                            var ts = tsKnowledgeList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, ts, MessageType.TsComputerElectronicKnowledgeUpdateRequestNotExist);
                        });
                    }
                    else
                    {
                        result = isDraft ? true : IsRecordUpdateCountMatching(filteredTsInfos, dbTsComputerElectronicKnowledgeInfos, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredTsInfos.Select(x => x.Epin.ToString()).ToList();
                            IList<string> computerknowledges = filteredTsInfos.Select(x => x.ComputerKnowledge).ToList();

                            if (tsEpin?.Count > 0)
                            {
                                //result = Convert.ToBoolean(_technicalSpecialistInfoServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages, tsca => tsca.TechnicalSpecialistComputerElectronicKnowledge).Result);
                                result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages, tsca => tsca.TechnicalSpecialistComputerElectronicKnowledge);
                            }

                            if (result && computerknowledges?.Count > 0)
                                result = _computerKnowledgeService.IsValidComputerKnowledgeName(computerknowledges, ref dbcompKnowledge, ref validationMessages);

                            if (result)
                            {
                                IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbComputerElectKnowledgeInfos = null;
                                IList<KeyValuePair<string, string>> tsComputerKnowledgeNotExists = null;
                                dbComputerElectKnowledgeInfos = dbTechnicalSpecialists.SelectMany(x => x.TechnicalSpecialistComputerElectronicKnowledge).ToList();
                                var tsPinAndComputerKnowledge = filteredTsInfos.Select(x => new KeyValuePair<string, string>(x.Epin.ToString(), x.ComputerKnowledge))
                                                                .Distinct()
                                                                .ToList();
                                result = IsComputerElectronicKnowledgeInfoExistInDb(tsPinAndComputerKnowledge, dbComputerElectKnowledgeInfos, ref tsComputerKnowledgeNotExists, ref validationMessages);
                            }

                        }
                    }
                }
                if (isDraft && result == false) //To handle reject TS changes and duplicate value validation
                {
                    result = true;
                    validationMessages?.Clear();
                    messages?.Clear();
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }

        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsComputerElectronicKnowledgeInfos,
                                                    IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComputerElectronicKnowledgeInfos,
                                                    ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var notMatchedRecords = tsComputerElectronicKnowledgeInfos.Where(x => !dbTsComputerElectronicKnowledgeInfos.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x.ComputerKnowledge, MessageType.TsUpdatedByOther, x.ComputerKnowledge);
            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages?.Count <= 0;
        }


        #endregion

        #region Remove
        private bool IsRecordValidForRemove(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsComputerElectronicKnowledgeInfos,
                                            ref IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> filteredTsComputerKnowledgeInfos,
                                            ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComputerElectInfos,
                                            ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsComputerElectronicKnowledgeInfos != null && tsComputerElectronicKnowledgeInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Delete;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsComputerKnowledgeInfos == null || filteredTsComputerKnowledgeInfos.Count <= 0)
                    filteredTsComputerKnowledgeInfos = FilterRecord(tsComputerElectronicKnowledgeInfos, validationType);

                if (filteredTsComputerKnowledgeInfos?.Count > 0 && IsValidPayload(filteredTsComputerKnowledgeInfos, validationType, ref validationMessages))
                {
                    GetTsComputerKnowledgeDbInfo(filteredTsComputerKnowledgeInfos, false, ref dbTsComputerElectInfos);

                    IList<int> tsIdNotExists = null;
                    IList<int> tsids = filteredTsComputerKnowledgeInfos.Select(x => x.Id).Distinct().ToList();

                    result = IsComputerElectronicKnowledgeInfoExistInDb(tsids, dbTsComputerElectInfos, ref tsIdNotExists, ref validationMessages);
                    if (result)
                        result = IsTsCodeCanBeRemove(dbTsComputerElectInfos, ref validationMessages);


                }
            }
            return result;
        }

        private bool IsTsCodeCanBeRemove(IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComputerElectInfos,
                                          ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            dbTsComputerElectInfos?.Where(x => x.IsAnyCollectionPropertyContainValue())
                            .ToList()
                            .ForEach(x =>
                            {
                                messages.Add(_messages, x.ComputerKnowledge.Name, MessageType.TsCoderIsBeingUsed, x.ComputerKnowledge.Name);
                            });

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return messages.Count <= 0;
        }
        #endregion     

        #region Common

        private bool IsComputerElectronicKnowledgeInfoExistInDb(IList<KeyValuePair<string, string>> tsPinAndComputerElectronicKnowledge,
                                          IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComputerElectronicKnowledgeInfos,
                                          ref IList<KeyValuePair<string, string>> tsComputerElectronicKnowledgeExists,
                                          ref IList<ValidationMessage> validationMessages)
        {
            var validMessages = validationMessages = validationMessages ?? new List<ValidationMessage>();
            dbTsComputerElectronicKnowledgeInfos = dbTsComputerElectronicKnowledgeInfos ?? new List<DbModel.TechnicalSpecialistComputerElectronicKnowledge>();

            if (tsPinAndComputerElectronicKnowledge?.Count > 0)
            {
                tsComputerElectronicKnowledgeExists = tsPinAndComputerElectronicKnowledge.Where(info => dbTsComputerElectronicKnowledgeInfos.Any(x1 => x1.TechnicalSpecialist.Pin == Convert.ToInt64(info.Key) && x1.ComputerKnowledge.Name == info.Value))
                                                        .Select(x => x).ToList();

                tsComputerElectronicKnowledgeExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsComputerElectronicKnowledgeExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }


        private bool IsComputerElectronicKnowledgeInfoExistInDb(IList<int> tsComputerKnowledgeIds,
                                           IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComputerKnowledgeInfos,
                                           ref IList<int> tsomputerKnowledgeIdsNotExists,
                                           ref IList<ValidationMessage> validationMessages)
        {
            var validMessages = validationMessages = validationMessages ?? new List<ValidationMessage>();
            dbTsComputerKnowledgeInfos = dbTsComputerKnowledgeInfos ?? new List<DbModel.TechnicalSpecialistComputerElectronicKnowledge>();

            if (tsComputerKnowledgeIds?.Count > 0)
            {
                tsomputerKnowledgeIdsNotExists = tsComputerKnowledgeIds.Where(x => !dbTsComputerKnowledgeInfos.Select(x1 => x1.Id).ToList().Contains(x)).ToList();
                tsomputerKnowledgeIdsNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.TsComputerElectronicKnowledgeDoesNotExist, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private void GetTsComputerKnowledgeDbInfo(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> filteredTsCodesInfos,
                                       bool isTsComputerElectronicKnowledgeInfoById,
                                       ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComputerElectronicKnowledgeInfos)
        {
            IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> tsCompElectr = null;
            dbTsComputerElectronicKnowledgeInfos = dbTsComputerElectronicKnowledgeInfos ?? new List<DbModel.TechnicalSpecialistComputerElectronicKnowledge>();
            var tsPins = !isTsComputerElectronicKnowledgeInfoById ?
                            filteredTsCodesInfos.Select(x => x.Epin.ToString()).Distinct().ToList() :
                            null;
            IList<int> tsIds = isTsComputerElectronicKnowledgeInfoById ?
                                filteredTsCodesInfos.Select(x => x.Id).Distinct().ToList() :
                                null;

            if (tsPins?.Count > 0 && (dbTsComputerElectronicKnowledgeInfos.Count <= 0 || dbTsComputerElectronicKnowledgeInfos.Any(x => !tsPins.Contains(x.TechnicalSpecialist.Pin.ToString()))))
            {
                tsCompElectr = GetComputerKnowledgeInfoByPin(tsPins).ToList();
            }
            if (tsIds?.Count > 0 && (dbTsComputerElectronicKnowledgeInfos.Count <= 0 || dbTsComputerElectronicKnowledgeInfos.Any(x => !tsIds.Contains(x.Id))))
            {
                tsCompElectr = GetComputerKnowladgeById(tsIds).ToList();
            }
            if (tsCompElectr != null && tsCompElectr.Any())
            {
                dbTsComputerElectronicKnowledgeInfos.AddRange(tsCompElectr);
            }
        }

        private IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> FilterRecord(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsComputerElectronicKnowledgeInfos,
                                                                 ValidationType filterType)
        {
            IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsComputerElectronicKnowledgeInfos?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsComputerElectronicKnowledgeInfos?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsComputerElectronicKnowledgeInfos?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> ts,
                                 ValidationType validationType,
                                 ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(ts), validationType);
            if (validationResults?.Count > 0)
            {
                messages.Add(_messages, ModuleType.Security, validationResults);
                validationMessages.AddRange(messages);
            }

            return validationMessages?.Count <= 0;
        }

        private Response RemoveTechSpecialistisComputerElectronicKnowledge(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsComputerElectronicKnowledgeInfos,
             ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComputerElectronicKnowledgeInfos,
        bool commitChange, bool isDbValidationRequire)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            IList<DbModel.Data> dbCompElecKnowledges = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalpecialist = null;
            IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> recordToBeDeleted = null;
            long? eventId = 0;
            Response response = null;
            try
            {
                eventId = tsComputerElectronicKnowledgeInfos?.FirstOrDefault()?.EventId;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(tsComputerElectronicKnowledgeInfos, ValidationType.Delete, ref dbTechnicalpecialist, ref dbCompElecKnowledges, ref dbTsComputerElectronicKnowledgeInfos);

                if (tsComputerElectronicKnowledgeInfos?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsComputerElectronicKnowledgeInfos, ValidationType.Delete);
                }

                if (recordToBeDeleted?.Count > 0 && (response == null || Convert.ToBoolean(response.Result)) && dbTsComputerElectronicKnowledgeInfos?.Count > 0)
                {
                    var dbTsCompElectKnowledgeToBeDeleted = dbTsComputerElectronicKnowledgeInfos?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _repository.AutoSave = false;
                    _repository.Delete(dbTsCompElectKnowledgeToBeDeleted);
                    if (commitChange)
                    {
                        _repository.ForceSave();
                        if (recordToBeDeleted.Count > 0)

                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                        null,
                                                                                                         ValidationType.Delete.ToAuditActionType(),
                                                                                                           SqlAuditModuleType.TechnicalSpecialistComputerElectronicKnowledge,
                                                                                                          x1,
                                                                                                           null
                                                                                                          ));
                    }

                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsComputerElectronicKnowledgeInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsComputerElectronicKnowledgeInfos,
                                                   ValidationType validationType,
                                                   ref IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> filteredTsInfos,
                                                   ref IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> dbTsComputerKnowledgeInfos,
                                                   ref IList<DbModel.Data> dbComputerElectronicKnowledges,
                                                   ref IList<DbModel.TechnicalSpecialist> dbtechnicalspecialists,
                                                   bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsComputerElectronicKnowledgeInfos, ref filteredTsInfos, ref dbTsComputerKnowledgeInfos, ref dbComputerElectronicKnowledges, ref dbtechnicalspecialists, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsComputerElectronicKnowledgeInfos, ref filteredTsInfos, ref dbTsComputerKnowledgeInfos, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsComputerElectronicKnowledgeInfos, ref filteredTsInfos, ref dbTsComputerKnowledgeInfos, ref dbtechnicalspecialists, ref dbComputerElectronicKnowledges, ref validationMessages, isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsComputerElectronicKnowledgeInfos);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private void GetMasterData(IList<TechnicalSpecialistComputerElectronicKnowledgeInfo> tsInfos,
                                                 ref IList<DbModel.Data> computerElectronicKnowledge)

        {
            IList<string> ComputerElectronicKnowledgeName = tsInfos.Select(x => x.ComputerKnowledge).ToList();

            var masterNames = ComputerElectronicKnowledgeName.ToList();
            var masterTypes = new List<MasterType>()
                    {
                        MasterType.ComputerKnowledge

                    };
            var dbMaster = _masterService.Get(masterTypes, null, masterNames);
            if (dbMaster?.Count > 0)
            {
                computerElectronicKnowledge = dbMaster.Where(x => x.MasterDataTypeId == (int)MasterType.ComputerKnowledge).ToList();
            }

        }


        private bool IsTechSpecialistExistInDb(IList<string> tsPins, ref IList<DbModel.TechnicalSpecialist> dbTsInfos, ref IList<ValidationMessage> validationMessages, params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
        {
            if ((dbTsInfos?.Count == 0 || dbTsInfos == null) && tsPins?.Count > 0)
                dbTsInfos = _technicalSpecialistRepository.FindBy(x => tsPins.Contains(x.Pin.ToString()), includes).ToList();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbTsInfos == null)
                dbTsInfos = new List<DbModel.TechnicalSpecialist>();

            var validMessages = validationMessages;
            var dbTechSpecs = dbTsInfos;

            if (tsPins?.Count > 0)
            {
                IList<string> tsPinNotExists = tsPins.Where(pin => !dbTechSpecs.Any(x1 => x1.Pin.ToString() == pin))
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
        #endregion

        #endregion




    }
}
