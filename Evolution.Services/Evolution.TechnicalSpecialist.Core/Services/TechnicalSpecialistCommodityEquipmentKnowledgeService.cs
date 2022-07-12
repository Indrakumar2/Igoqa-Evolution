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
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Core.Services
{
    public class TechnicalSpecialistCommodityEquipmentKnowledgeService : ITechnicalSpecialCommodityEquipmentKnowledgeService
    {
        private readonly IAppLogger<TechnicalSpecialistCommodityEquipmentKnowledgeService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ITechnicalSpecialistCommodityEquipmentKnowledgeRepository _repository = null;
        private readonly JObject _messages = null;
        private readonly ITechnicalSpecialistCommodityEquipmentKnowledgeValidationService _validationService = null;
        //private readonly ITechnicalSpecialistService _technSpecServices = null;
        private readonly ITechnicalSpecialistRepository _technicalSpecialistRepository = null;
        private readonly IMasterService _masterService = null;
        private readonly ICommodityService _commodityService = null;
        private readonly IEquipmentService _equipmentService = null;
        private readonly IAuditSearchService _auditSearchService = null;


        #region Constructor
        public TechnicalSpecialistCommodityEquipmentKnowledgeService(IMapper mapper,
                                                    ITechnicalSpecialistCommodityEquipmentKnowledgeRepository repository,
                                                    IAppLogger<TechnicalSpecialistCommodityEquipmentKnowledgeService> logger,
                                                    ITechnicalSpecialistCommodityEquipmentKnowledgeValidationService validationService,
                                                    JObject messages,
                                                    //ITechnicalSpecialistService technSpecServices,
                                                    ITechnicalSpecialistRepository technicalSpecialistRepository,
                                                    IMasterService masterService, ICommodityService commodityService,
                                                    IEquipmentService equipmentService, IAuditSearchService auditSearchService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _validationService = validationService;
            _messages = messages;
            //_technSpecServices = technSpecServices;
            _technicalSpecialistRepository = technicalSpecialistRepository;
            _masterService = masterService;
            _commodityService = commodityService;
            _equipmentService = equipmentService;
            _auditSearchService = auditSearchService;

        }
        #endregion

        #region Public Methods

        #region Get

        public Response Get(TechnicalSpecialistCommodityEquipmentKnowledgeInfo searchModel)
        {
            IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo>>(_repository.Search(searchModel));
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
            IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo>>(GetCommodityEquipmentInfoByPin(pins));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), pins);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GetByEquipmentKnowledge(IList<string> EquipmentKnowledge)
        {
            IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo>>(GetCommodityEquipmentKnowledgeInfotByEquipment(EquipmentKnowledge));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), EquipmentKnowledge);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<int> Ids)
        {
            IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo>>(GetCommodityEquipmentById(Ids));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), Ids);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> Commodity)
        {
            IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo>>(GetCommodityEquipmentKnowledgeInfotByCommodity(Commodity));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), Commodity);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        #endregion

        #region Add 

        public Response Add(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                            bool commitChange = true,
                            bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCommodities = null;
            IList<DbModel.Data> dbEquipments = null;

            return AddTechSpecialistComdEqipKnowledge(tsComdEqipKnowledge, ref dbTsComdEqipKnowledge, ref dbTechnicalSpecialists, ref dbCommodities, ref dbEquipments, commitChange, isDbValidationRequired);
        }

        public Response Add(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                        ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbCommodities,
                        ref IList<DbModel.Data> dbEquipments,
                        bool commitChange = true,
                        bool isDbValidationRequired = true)
        {
            return AddTechSpecialistComdEqipKnowledge(tsComdEqipKnowledge, ref dbTsComdEqipKnowledge, ref dbTechnicalSpecialists, ref dbCommodities, ref dbEquipments, commitChange, isDbValidationRequired);
        }


        #endregion

        #region Modify

        public Response Modify(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                        bool commitChange = true,
                        bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCommodities = null;
            IList<DbModel.Data> dbEquipments = null;

            return UpdateTechSpecialistComdEqipKnowledge(tsComdEqipKnowledge, ref dbTsComdEqipKnowledge, ref dbTechnicalSpecialists, ref dbCommodities, ref dbEquipments, commitChange, isDbValidationRequired);
        }

        public Response Modify(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                        ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                        ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                        ref IList<DbModel.Data> dbCommodities,
                        ref IList<DbModel.Data> dbEquipments,
                        bool commitChange = true,
                        bool isDbValidationRequired = true)
        {
            return UpdateTechSpecialistComdEqipKnowledge(tsComdEqipKnowledge, ref dbTsComdEqipKnowledge, ref dbTechnicalSpecialists, ref dbCommodities, ref dbEquipments, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Delete

        public Response Delete(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                       bool commitChange = true,
                       bool isDbValidationRequired = true)
        {
            IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsCommdEquipmentKnowledge = null;
            return RemoveTechSpecialistComdEqipKnowledge(tsComdEqipKnowledge, ref dbTsCommdEquipmentKnowledge, commitChange, isDbValidationRequired);
        }

        public Response Delete(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
            ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsCommdEquipmentKnowledge,
                        bool commitChange = true,
                        bool isDbValidationRequired = true)
        {
            return RemoveTechSpecialistComdEqipKnowledge(tsComdEqipKnowledge, ref dbTsCommdEquipmentKnowledge, commitChange, isDbValidationRequired);
        }

        #endregion

        #region Validation

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                            ValidationType validationType)
        {
            IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge = null;
            return IsRecordValidForProcess(tsComdEqipKnowledge, validationType, ref dbTsComdEqipKnowledge);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge, ValidationType validationType, ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge)
        {
            IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> filteredTSComdEqipKnowledge = null;
            IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists = null;
            IList<DbModel.Data> dbCommodities = null;
            IList<DbModel.Data> dbEquipments = null;

            return CheckRecordValidForProcess(tsComdEqipKnowledge, validationType, ref filteredTSComdEqipKnowledge, ref dbTsComdEqipKnowledge, ref dbTechnicalSpecialists, ref dbCommodities, ref dbEquipments);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                                ValidationType validationType,
                                                ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                ref IList<DbModel.Data> dbCommodities,
                                                ref IList<DbModel.Data> dbEquipments,
                                                bool isDraft = false)
        {
            IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> filteredTSComdEqipKnowledge = null;
            return CheckRecordValidForProcess(tsComdEqipKnowledge, validationType, ref filteredTSComdEqipKnowledge, ref dbTsComdEqipKnowledge, ref dbTechnicalSpecialists, ref dbCommodities, ref dbEquipments, isDraft);
        }

        public Response IsRecordValidForProcess(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                            ValidationType validationType,
                                            IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge)
        {
            return IsRecordValidForProcess(tsComdEqipKnowledge, validationType, ref dbTsComdEqipKnowledge);
        }

        public Response IsRecordExistInDb(IList<int> tsComdEqipKnowledgeIds,
                                        ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                        ref IList<ValidationMessage> validationMessages)
        {
            IList<int> tsComdEqipKnowledgeIdNotExists = null;
            return IsRecordExistInDb(tsComdEqipKnowledgeIds, ref dbTsComdEqipKnowledge, ref tsComdEqipKnowledgeIdNotExists, ref validationMessages);
        }

        public Response IsRecordExistInDb(IList<int> tsComdEqipKnowledgeIds,
                                          ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                          ref IList<int> tsComdEqipKnowledgeIdNotExists,
                                          ref IList<ValidationMessage> validationMessages)
        {

            Exception exception = null;
            bool result = true;
            try
            {
                if (dbTsComdEqipKnowledge == null && tsComdEqipKnowledgeIds != null && tsComdEqipKnowledgeIds.Any())
                    dbTsComdEqipKnowledge = GetCommodityEquipmentById(tsComdEqipKnowledgeIds);

                result = IsTSCommodityEquipmentExistInDb(tsComdEqipKnowledgeIds, dbTsComdEqipKnowledge, ref tsComdEqipKnowledgeIdNotExists, ref validationMessages);
            }
            catch (Exception ex)
            {
                result = false;
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsComdEqipKnowledgeIds);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        #endregion

        #endregion

        #region Private Methods

        #region Get
        private IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> GetCommodityEquipmentInfoByPin(IList<string> pins)
        {
            IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbtsCommodityEquipmentInfos = null;
            if (pins != null && pins.Any())
            {
                dbtsCommodityEquipmentInfos = _repository.FindBy(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString())).ToList();
            }
            return dbtsCommodityEquipmentInfos;
        }

        private IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> GetCommodityEquipmentById(IList<int> tsCommodityEquipmentInfosIds)
        {
            IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbtsCommodityEquipmentInfos = null;
            if (tsCommodityEquipmentInfosIds != null && tsCommodityEquipmentInfosIds.Any())
                dbtsCommodityEquipmentInfos = _repository.FindBy(x => tsCommodityEquipmentInfosIds.Contains(x.Id)).ToList();

            return dbtsCommodityEquipmentInfos;
        }

        private IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> GetCommodityEquipmentKnowledgeInfotByCommodity(IList<string> tsCommodity)
        {
            IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbtsCommodityInfos = null;
            if (tsCommodity != null && tsCommodity.Any())
                dbtsCommodityInfos = _repository.FindBy(x => tsCommodity.Contains(x.Commodity.Name)).ToList();

            return dbtsCommodityInfos;
        }
        private IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> GetCommodityEquipmentKnowledgeInfotByEquipment(IList<string> tsEquipment)
        {
            IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbtsEquipmentInfos = null;
            if (tsEquipment != null && tsEquipment.Any())
                dbtsEquipmentInfos = _repository.FindBy(x => tsEquipment.Contains(x.EquipmentKnowledge.Name)).ToList();

            return dbtsEquipmentInfos;
        }

        #endregion

        private Response AddTechSpecialistComdEqipKnowledge(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                                    ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                                    ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                    ref IList<DbModel.Data> dbCommodities,
                                                    ref IList<DbModel.Data> dbEquipments,
                                                    bool commitChange = true,
                                                    bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            long? eventId = 0;
            try
            {
                Response valdResponse = null;
                IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> recordToBeAdd = null;
                IList<DbModel.TechnicalSpecialist> dbTechSpecialists =null;
                IList<DbModel.Data> dbCommodity = null;
                IList<DbModel.Data> dbEquipment = null;
                eventId = tsComdEqipKnowledge?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsComdEqipKnowledge, ValidationType.Add, ref recordToBeAdd,
                                                              ref dbTsComdEqipKnowledge, ref dbTechnicalSpecialists, ref dbCommodities,
                                                              ref dbEquipments);
                }

                if (tsComdEqipKnowledge != null && tsComdEqipKnowledge.Any())
                {
                    recordToBeAdd = FilterRecord(tsComdEqipKnowledge, ValidationType.Add);
                }

                if (!isDbValidationRequired ||(recordToBeAdd?.Count>0 && Convert.ToBoolean(valdResponse.Result)))
                {
                    dbTechSpecialists = dbTechnicalSpecialists;
                    dbCommodity = dbCommodities;
                    dbEquipment = dbEquipments;

                    recordToBeAdd = recordToBeAdd.Select(x => { x.Id = 0; return x; }).ToList();
                    _repository.AutoSave = false;
                    var mappedRecords = _mapper.Map<IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge>>(recordToBeAdd, opt =>
                    {
                        opt.Items["isAssignId"] = false;
                        opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                        opt.Items["DBCommodities"] = dbCommodity;
                        opt.Items["DBEquipments"] = dbEquipment;

                    });
                    _repository.Add(mappedRecords);
                    if (commitChange)
                        _repository.ForceSave();
                    dbTsComdEqipKnowledge = mappedRecords;
                    if (mappedRecords != null && mappedRecords.Any())
                    {

                        mappedRecords?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, tsComdEqipKnowledge?.FirstOrDefault()?.ActionByUser,
                                                                                           null,
                                                                                            ValidationType.Add.ToAuditActionType(),
                                                                                            SqlAuditModuleType.TechnicalSpecialistCommodityEquipmentKnowledge,
                                                                                             null,
                                                                                               _mapper.Map<TechnicalSpecialistCommodityEquipmentKnowledgeInfo>(x1)
                                                                                             ));
                    }





                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsComdEqipKnowledge);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response UpdateTechSpecialistComdEqipKnowledge(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                                                ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                                                ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                                                ref IList<DbModel.Data> dbCommodities,
                                                                ref IList<DbModel.Data> dbEquipments,
                                                                bool commitChange = true,
                                                                bool isDbValidationRequired = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            IList<DbModel.TechnicalSpecialist> dbTechSpecialists = null;
            IList<DbModel.Data> dbCommdty = null;
            IList<DbModel.Data> dbEquip = null;
            Response valdResponse = null;
            IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> recordToBeModify = null;
            long? eventId = 0;
            bool valdResult = false;
            try
            {
                eventId = tsComdEqipKnowledge?.FirstOrDefault()?.EventId;
                if (isDbValidationRequired)
                {
                    valdResponse = CheckRecordValidForProcess(tsComdEqipKnowledge, ValidationType.Update, ref recordToBeModify, ref dbTsComdEqipKnowledge, ref dbTechnicalSpecialists, ref dbCommodities, ref dbEquipments);
                    valdResult = Convert.ToBoolean(valdResponse.Result);
                }

                if (tsComdEqipKnowledge != null && tsComdEqipKnowledge.Any())
                {
                    recordToBeModify = FilterRecord(tsComdEqipKnowledge, ValidationType.Update);
                }
                if (recordToBeModify != null && recordToBeModify.Any())
                {
                    if (dbTsComdEqipKnowledge == null || (dbTsComdEqipKnowledge?.Count <= 0 && !valdResult))
                    {
                        dbTsComdEqipKnowledge = _repository.Get(recordToBeModify?.Select(x => x.Id).ToList());
                    }

                    if (dbTechnicalSpecialists == null || (dbTechnicalSpecialists?.Count <= 0 && !valdResult))
                    {
                        //valdResult = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages).Result);
                        valdResult = IsTechSpecialistExistInDb(recordToBeModify.Select(x => x.Epin.ToString()).ToList(), ref dbTechnicalSpecialists, ref validationMessages);
                    }

                    if (dbCommodities == null || (dbCommodities?.Count <= 0 && !valdResult))
                    {
                        valdResult = _commodityService.IsValidCommodityName(recordToBeModify.Select(x => x.Commodity).ToList(), ref dbCommodities, ref validationMessages);
                    }

                    if (dbEquipments == null || (dbEquipments?.Count <= 0 && !valdResult))
                    {
                        valdResult = _equipmentService.IsValidEquipmentName(recordToBeModify.Select(x => x.EquipmentKnowledge).ToList(), ref dbEquipments, ref validationMessages);
                    }

                    if (!isDbValidationRequired || (valdResult && dbTsComdEqipKnowledge != null && dbTsComdEqipKnowledge.Any()))
                    {
                        dbTechSpecialists = dbTechnicalSpecialists;
                        dbCommdty = dbCommodities;
                        dbEquip = dbEquipments;
                        IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> domExisistingTecSplCommodityEquipmentKnowledge = new List<TechnicalSpecialistCommodityEquipmentKnowledgeInfo>();
                      
                        dbTsComdEqipKnowledge.ToList().ForEach(tsComdEqipKnowledgeInfo =>
                        {
                            domExisistingTecSplCommodityEquipmentKnowledge.Add(ObjectExtension.Clone(_mapper.Map<TechnicalSpecialistCommodityEquipmentKnowledgeInfo>(tsComdEqipKnowledgeInfo)));

                            var tsToBeModify = recordToBeModify.FirstOrDefault(x => x.Id == tsComdEqipKnowledgeInfo.Id);
                            if (tsToBeModify != null)
                            {
                                _mapper.Map(tsToBeModify, tsComdEqipKnowledgeInfo, opt =>
                                {
                                    opt.Items["isAssignId"] = true;
                                    opt.Items["DbTechSpecialist"] = dbTechSpecialists;
                                    opt.Items["DBCommodities"] = dbCommdty;
                                    opt.Items["DBEquipments"] = dbEquip;

                                });
                                tsComdEqipKnowledgeInfo.LastModification = DateTime.UtcNow;
                                tsComdEqipKnowledgeInfo.UpdateCount = tsComdEqipKnowledgeInfo.UpdateCount.CalculateUpdateCount();
                                tsComdEqipKnowledgeInfo.ModifiedBy = tsToBeModify.ModifiedBy;
                            }
                        });
                        _repository.AutoSave = false;
                        _repository.Update(dbTsComdEqipKnowledge);
                        if (commitChange)
                        {
                            _repository.ForceSave();
                            if (recordToBeModify != null && recordToBeModify.Any())
                            {
                                recordToBeModify?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                    null,
                                                                    ValidationType.Update.ToAuditActionType(),
                                                                  SqlAuditModuleType.TechnicalSpecialistCommodityEquipmentKnowledge,
                                                                   domExisistingTecSplCommodityEquipmentKnowledge?.FirstOrDefault(x2 => x2.Id == x1.Id),
                                                                   x1
                                                                   ));
                            }

                        }


                    }
                    else
                        return valdResponse;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsComdEqipKnowledge);
            }
            finally
            {
                _repository.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        private Response RemoveTechSpecialistComdEqipKnowledge(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
            ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsCommdEquipmentKnowledge,
                                                      bool commitChange = true,
                                                      bool isDbValidationRequire = true)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> recordToBeDeleted = null;
            long? eventId = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                eventId = tsComdEqipKnowledge?.FirstOrDefault()?.EventId;
                Response response = null;
                if (isDbValidationRequire)
                    response = IsRecordValidForProcess(tsComdEqipKnowledge, ValidationType.Delete, ref dbTsCommdEquipmentKnowledge);

                if (tsComdEqipKnowledge?.Count > 0)
                {
                    recordToBeDeleted = FilterRecord(tsComdEqipKnowledge, ValidationType.Delete);
                }
                 
                if (recordToBeDeleted != null && recordToBeDeleted.Any() && (response == null || Convert.ToBoolean(response.Result)) && dbTsCommdEquipmentKnowledge != null && dbTsCommdEquipmentKnowledge.Any())
                {
                    var dbTsCommdEquipmentKnowToBeDeleted = dbTsCommdEquipmentKnowledge?.Where(x => recordToBeDeleted.Any(x1 => x1.Id == x.Id)).ToList();
                    _repository.AutoSave = false;
                    _repository.Delete(dbTsCommdEquipmentKnowToBeDeleted);
                    if (commitChange)
                    {
                        _repository.ForceSave();

                        if (recordToBeDeleted != null && recordToBeDeleted.Any())
                        {
                            recordToBeDeleted?.ToList().ForEach(x1 => _auditSearchService.AuditLog(x1, ref eventId, x1.ActionByUser,
                                                                                                    null,
                                                                                                    ValidationType.Delete.ToAuditActionType(),
                                                                                                   SqlAuditModuleType.TechnicalSpecialistCommodityEquipmentKnowledge,
                                                                                                   x1,
                                                                                                    null
                                                                                                    ));
                        }
                    }

                }
                else
                    return response;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsComdEqipKnowledge);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response CheckRecordValidForProcess(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                            ValidationType validationType,
                                            ref IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> filteredTsComdEqipKnowledge,
                                            ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                            ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                            ref IList<DbModel.Data> dbCommodities,
                                            ref IList<DbModel.Data> dbEquipments,
                                             bool isDraft = false)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(tsComdEqipKnowledge, ref filteredTsComdEqipKnowledge, ref dbTsComdEqipKnowledge, ref dbTechnicalSpecialists, ref dbCommodities, ref dbEquipments, ref validationMessages);
                else if (validationType == ValidationType.Delete)
                    result = IsRecordValidForRemove(tsComdEqipKnowledge, ref filteredTsComdEqipKnowledge, ref dbTsComdEqipKnowledge, ref validationMessages);
                else if (validationType == ValidationType.Update)
                    result = IsRecordValidForUpdate(tsComdEqipKnowledge, ref filteredTsComdEqipKnowledge, ref dbTsComdEqipKnowledge, ref dbTechnicalSpecialists, ref dbCommodities, ref dbEquipments, ref validationMessages, isDraft);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), tsComdEqipKnowledge);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsRecordValidForAdd(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                     ref IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> filteredTsComdEqipKnowledge,
                                     ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                     ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                     ref IList<DbModel.Data> dbCommodities,
                                     ref IList<DbModel.Data> dbEquipments,
                                     ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsComdEqipKnowledge != null && tsComdEqipKnowledge.Any())
            {
                dbTechnicalSpecialists = dbTechnicalSpecialists ?? new List<DbModel.TechnicalSpecialist>();
                dbCommodities = dbCommodities ?? new List<DbModel.Data>();
                dbEquipments = dbEquipments ?? new List<DbModel.Data>();

                ValidationType validationType = ValidationType.Add;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsComdEqipKnowledge == null || filteredTsComdEqipKnowledge.Count <= 0)
                    filteredTsComdEqipKnowledge = FilterRecord(tsComdEqipKnowledge, validationType);

                if (filteredTsComdEqipKnowledge != null && filteredTsComdEqipKnowledge.Any() && IsValidPayload(filteredTsComdEqipKnowledge, validationType, ref validationMessages))
                {
                    IList<string> tsEpin = filteredTsComdEqipKnowledge.Select(x => x.Epin.ToString()).ToList();
                    IList<string> commodities = filteredTsComdEqipKnowledge.Select(x => x.Commodity).ToList();
                    IList<string> equipments = filteredTsComdEqipKnowledge.Select(x => x.EquipmentKnowledge).ToList();
                    GetMasterData(filteredTsComdEqipKnowledge, ref dbCommodities, ref dbEquipments);

                    //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                    result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                    if (result && commodities != null && commodities.Any())
                        result = _commodityService.IsValidCommodityName(commodities, ref dbCommodities, ref validationMessages);
                    if (result && equipments != null && equipments.Any())
                        result = _equipmentService.IsValidEquipmentName(equipments, ref dbEquipments, ref validationMessages);
                    if (result)
                        result = IsTSCommodityEquipmentUnique(filteredTsComdEqipKnowledge, ref validationMessages);
                }
            }
            return result;
        }
        private bool IsRecordValidForUpdate(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                          ref IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> filteredTsComdEqipKnowledge,
                                          ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                          ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,
                                          ref IList<DbModel.Data> dbCommodities,
                                          ref IList<DbModel.Data> dbEquipments,
                                          ref IList<ValidationMessage> validationMessages,
                                          bool isDraft = false)
        {
            bool result = false;
            if (tsComdEqipKnowledge != null && tsComdEqipKnowledge.Count > 0)
            {
                ValidationType validationType = ValidationType.Update;
                IList<ValidationMessage> messages = new List<ValidationMessage>();
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsComdEqipKnowledge == null || filteredTsComdEqipKnowledge.Count <= 0)
                    filteredTsComdEqipKnowledge = FilterRecord(tsComdEqipKnowledge, validationType);

                if (filteredTsComdEqipKnowledge != null && filteredTsComdEqipKnowledge.Any() && IsValidPayload(filteredTsComdEqipKnowledge, validationType, ref messages))
                {
                    GetTsCommodityEquipmentDbInfo(filteredTsComdEqipKnowledge, ref dbTsComdEqipKnowledge);
                    IList<int> tsCommodityEquipmentIds = filteredTsComdEqipKnowledge.Select(x => x.Id).ToList();
                    IList<int> tsDBCommodityEquipmentIds = dbTsComdEqipKnowledge.Select(x => x.Id).ToList();
                    if (tsCommodityEquipmentIds.Any(x => !tsDBCommodityEquipmentIds.Contains(x))) //Invalid TechSpecialist  Id found.
                    {
                        var dbTsInfosByIds = dbTsComdEqipKnowledge;
                        var idNotExists = tsCommodityEquipmentIds.Where(id => !dbTsInfosByIds.Any(tp => tp.Id == id)).ToList();
                        var techSpecialistCommodityEquipmentList = filteredTsComdEqipKnowledge;
                        idNotExists?.ForEach(tsId =>
                        {
                            var tsPaySchedule = techSpecialistCommodityEquipmentList.FirstOrDefault(x1 => x1.Id == tsId);
                            messages.Add(_messages, tsPaySchedule, MessageType.InvalidCommodityEquipmentKnowledgeInfo);
                        });
                    }
                    else
                    {
                        result = isDraft ? true : IsRecordUpdateCountMatching(filteredTsComdEqipKnowledge, dbTsComdEqipKnowledge, ref messages);
                        if (result)
                        {
                            IList<string> tsEpin = filteredTsComdEqipKnowledge.Select(x => x.Epin.ToString()).ToList();
                            IList<string> commodities = filteredTsComdEqipKnowledge.Select(x => x.Commodity).ToList();
                            IList<string> Equipments = filteredTsComdEqipKnowledge.Select(x => x.EquipmentKnowledge).ToList();

                            //result = Convert.ToBoolean(_technSpecServices.IsRecordExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages).Result);
                            result = IsTechSpecialistExistInDb(tsEpin, ref dbTechnicalSpecialists, ref validationMessages);
                            if (result && commodities != null && commodities.Any())
                                result = _commodityService.IsValidCommodityName(commodities, ref dbCommodities, ref validationMessages);
                            if (result && Equipments != null && Equipments.Any())
                                result = _equipmentService.IsValidEquipmentName(Equipments, ref dbEquipments, ref validationMessages);
                            if (result)
                                result = IsTSCommodityEquipmentUnique(filteredTsComdEqipKnowledge, ref validationMessages);
                        }

                    }
                }
                if (messages?.Count > 0)
                    validationMessages.AddRange(messages);
            }
            return result;
        }

        private bool IsRecordValidForRemove(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                           ref IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> filteredTsComdEqipKnowledge,
                                           ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                           ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (tsComdEqipKnowledge != null && tsComdEqipKnowledge.Any())
            {
                ValidationType validationType = ValidationType.Delete;
                validationMessages = validationMessages ?? new List<ValidationMessage>();

                if (filteredTsComdEqipKnowledge == null || filteredTsComdEqipKnowledge.Count <= 0)
                    filteredTsComdEqipKnowledge = FilterRecord(tsComdEqipKnowledge, validationType);

                if (filteredTsComdEqipKnowledge != null && filteredTsComdEqipKnowledge.Any() && IsValidPayload(filteredTsComdEqipKnowledge, validationType, ref validationMessages))
                {
                    GetTsCommodityEquipmentDbInfo(filteredTsComdEqipKnowledge, ref dbTsComdEqipKnowledge);
                    IList<int> tsPayScheduleIdNotExists = null;
                    var tsPayScheduleIds = filteredTsComdEqipKnowledge.Select(x => x.Id).Distinct().ToList();
                    result = IsTSCommodityEquipmentExistInDb(tsPayScheduleIds, dbTsComdEqipKnowledge, ref tsPayScheduleIdNotExists, ref validationMessages);

                }
            }
            return result;
        }


        private bool IsRecordUpdateCountMatching(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                                               IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                               ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var notMatchedRecords = tsComdEqipKnowledge.Where(x => !dbTsComdEqipKnowledge.ToList().Any(x1 => x1.UpdateCount.ToInt() == x.UpdateCount.ToInt() && x1.Id == x.Id)).ToList();
            notMatchedRecords.ForEach(x =>
            {
                messages.Add(_messages, x, MessageType.CommodityEquipmentKnowledgeInfoUpdatedByOtherUser, x.Commodity);
            });

            if (messages != null && messages.Any())
                validationMessages.AddRange(messages);

            return !messages.Any();
        }

        private IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> FilterRecord(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge, ValidationType filterType)
        {
            IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> filterTsInfos = null;

            if (filterType == ValidationType.Add)
                filterTsInfos = tsComdEqipKnowledge?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (filterType == ValidationType.Update)
                filterTsInfos = tsComdEqipKnowledge?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            else if (filterType == ValidationType.Delete)
                filterTsInfos = tsComdEqipKnowledge?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

            return filterTsInfos;
        }

        private bool IsValidPayload(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsComdEqipKnowledge,
                            ValidationType validationType,
                            ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(tsComdEqipKnowledge), validationType);
            if (validationResults != null && validationResults.Any())
            {
                messages.Add(_messages, ModuleType.Security, validationResults);
                validationMessages.AddRange(messages);
            }

            return !validationMessages.Any();
        }
        private void GetTsCommodityEquipmentDbInfo(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> filteredTsComdEqipKnowledge,
                                           ref IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge)
        {
            dbTsComdEqipKnowledge = dbTsComdEqipKnowledge ?? new List<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge>();
            IList<int> tsCommodityEquipmentIds = filteredTsComdEqipKnowledge?.Select(x => x.Id).Distinct().ToList();
            if (tsCommodityEquipmentIds != null && tsCommodityEquipmentIds.Any() && (dbTsComdEqipKnowledge.Count <= 0 || dbTsComdEqipKnowledge.Any(x => !tsCommodityEquipmentIds.Contains(x.Id))))
            {
                var tsComdEqipKnowledges = GetCommodityEquipmentById(tsCommodityEquipmentIds);
                if (tsComdEqipKnowledges != null && tsComdEqipKnowledges.Any())
                {
                    dbTsComdEqipKnowledge.AddRange(tsComdEqipKnowledges);
                }
            }
        }
        private bool IsTSCommodityEquipmentExistInDb(IList<int> tsCommodityEquipmentIds,
                                               IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> dbTsComdEqipKnowledge,
                                               ref IList<int> tsCommodityEquipmentIdNotExists,
                                               ref IList<ValidationMessage> validationMessages)
        {
            var validMessages = validationMessages = validationMessages ?? new List<ValidationMessage>();
            dbTsComdEqipKnowledge = dbTsComdEqipKnowledge ?? new List<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge>();

            if (tsCommodityEquipmentIds != null && tsCommodityEquipmentIds.Any())
            {
                tsCommodityEquipmentIdNotExists = tsCommodityEquipmentIds.Where(id => !dbTsComdEqipKnowledge.Any(x1 => x1.Id == id))
                                     .Select(id => id)
                                     .ToList();

                tsCommodityEquipmentIdNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.InvalidCommodityEquipmentKnowledgeInfo, x);
                });
            }

            if (validMessages.Any())
                validationMessages = validMessages;

            return !validMessages.Any();
        }
        private bool IsTSCommodityEquipmentUnique(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> filteredTsPaySchedules,
                                          ref IList<ValidationMessage> validationMessages)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            validationMessages = validationMessages ?? new List<ValidationMessage>();

            var tscomdtyEquipKnowledge = filteredTsPaySchedules.Select(x => new { x.Epin, x.Commodity, x.EquipmentKnowledge, x.EquipmentKnowledgeLevel, x.Id });
            var dbTscomdtyEquipKnowledge = _repository.FindBy(x => tscomdtyEquipKnowledge.Any(x1 => x1.Epin == x.TechnicalSpecialist.Pin && x1.Commodity == x.Commodity.Name && x1.EquipmentKnowledge == x.EquipmentKnowledge.Name && x1.EquipmentKnowledgeLevel == x.EquipmentKnowledgeLevel && x1.Id != x.Id)).ToList();
            if (dbTscomdtyEquipKnowledge?.Count > 0)
            {
                var tsComdtyEquipKnowledgeAlreadyExist = filteredTsPaySchedules.Where(x => dbTscomdtyEquipKnowledge.Any(x1 => x.Epin == x1.TechnicalSpecialist.Pin && x.Commodity == x1.Commodity.Name && x.EquipmentKnowledge == x1.EquipmentKnowledge.Name && x1.EquipmentKnowledgeLevel == x.EquipmentKnowledgeLevel));
                tsComdtyEquipKnowledgeAlreadyExist?.ToList().ForEach(x =>
                {
                    messages.Add(_messages, x, MessageType.TsComdtyEquipmentKnowledgeAlreadyExist, x.Epin, x.Commodity, x.EquipmentKnowledge, x.EquipmentKnowledgeLevel);
                });
            }

            if (messages.Count > 0)
                validationMessages.AddRange(messages);

            return !messages.Any();
        }

        private void GetMasterData(IList<TechnicalSpecialistCommodityEquipmentKnowledgeInfo> tsInfos,
                                              ref IList<DbModel.Data> dbCommodities, ref IList<DbModel.Data> dbEquipments)

        {
            IList<string> commodityNames = tsInfos.Select(x => x.Commodity).ToList();
            IList<string> equipmentsName = tsInfos.Select(x => x.EquipmentKnowledge).ToList();

            var masterNames = commodityNames.Union(equipmentsName).ToList();
            var dbMaster = _masterService.Get(new List<MasterType>()
                    {
                        MasterType.Commodity,
                        MasterType.Equipment

                    }, null, masterNames);
            if (dbMaster?.Count > 0)
            {
                var commoditiesAndequipments = dbMaster.Where(x => x.MasterDataTypeId == (int)MasterType.Commodity || x.MasterDataTypeId == (int)MasterType.Equipment).ToList();

                if (commoditiesAndequipments.Count > 0)
                {
                    var commodities = commoditiesAndequipments?.Where(x => x.MasterDataTypeId == (int)MasterType.Commodity).ToList();
                    if (commodities.Count > 0)
                    {
                        dbCommodities.AddRange(commodities);
                        dbCommodities = dbCommodities.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
                    }
                    var equipments = commoditiesAndequipments.Where(x => x.MasterDataTypeId == (int)MasterType.Equipment).ToList();
                    if (equipments?.Count > 0)
                    {
                        dbEquipments.AddRange(equipments);
                        dbEquipments = dbEquipments.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();
                    }
                }
            }
        }

        private bool IsTechSpecialistExistInDb(IList<string> tsPins, ref IList<DbModel.TechnicalSpecialist> dbTsInfos, ref IList<ValidationMessage> validationMessages)
        {
            if ((dbTsInfos?.Count == 0 || dbTsInfos == null) && tsPins?.Count > 0)
                dbTsInfos = _technicalSpecialistRepository.FindBy(x => tsPins.Contains(x.Pin.ToString())).ToList();

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

    }
}
