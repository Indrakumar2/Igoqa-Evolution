using AutoMapper;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.AuditLog.Domain.Interfaces.Data;
using Evolution.AuditLog.Domain.Interfaces.Validations;
using Evolution.AuditLog.Domain.Models.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.AuditLog.Core.Services
{
    public class SqlAuditLogEventInfoService : ISqlAuditLogEventInfoService
    {
        private readonly IAppLogger<SqlAuditLogEventInfoService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ISqlAuditLogEventReposiotry _repository = null;
        private readonly JObject _messages = null;
        private readonly ISqlAuditLogEventValidationService _validationService = null;
        private readonly ISqlAuditModuleService _sqlAuditModuleService = null;
        private readonly ISqlAuditLogDetailInfoService _sqlAuditLogDetailInfoService = null;
        private readonly ISqlAuditLogDetailRepository _sqlAuditLogDetailRepository = null;
        // private readonly IAuditLogger _auditLogger = null;

        #region Constructor
        public SqlAuditLogEventInfoService(IMapper mapper,
                                        ISqlAuditLogEventReposiotry repository,
                                        IAppLogger<SqlAuditLogEventInfoService> logger,
                                        ISqlAuditLogEventValidationService validationService,
                                        ISqlAuditModuleService sqlAuditModuleService,
                                        ISqlAuditLogDetailInfoService sqlAuditLogDetailInfoService,
                                        JObject messgaes, ISqlAuditLogDetailRepository sqlAuditLogDetailRepository)

        // IAuditLogger auditLogger)
        {
            this._repository = repository;
            this._logger = logger;
            this._mapper = mapper;
            this._validationService = validationService;
            this._messages = messgaes;
            _sqlAuditModuleService = sqlAuditModuleService;
            _sqlAuditLogDetailInfoService = sqlAuditLogDetailInfoService;
            _sqlAuditLogDetailRepository = sqlAuditLogDetailRepository;
            //_auditLogger = auditLogger;
        }
        #endregion


        public Response Add(IList<SqlAuditLogEventInfo> auditLogInfos, bool commitChange = true)
        {
            IList<DbModel.SqlauditModule> dbModules = null;
            return AddAuditLogEvent(auditLogInfos, ref dbModules, commitChange);
        }

        public Response Add(IList<SqlAuditLogEventInfo> auditLogInfos, IList<DbModel.SqlauditModule> dbModules = null, bool commitChange = true)
        {
            return AddAuditLogEvent(auditLogInfos, ref dbModules, commitChange);
        }

        public Response Add(IList<SqlAuditLogDetailInfo> auditLogDetailInfos, bool commitChange = true)
        {
            IList<DbModel.SqlauditModule> dbModules = null;
            return AddAuditLog(auditLogDetailInfos, ref dbModules, commitChange);
        }

        public Response Add(IList<SqlAuditLogDetailInfo> auditLogDetailInfos, IList<DbModel.SqlauditModule> dbModules = null, bool commitChange = true)
        {
            return AddAuditLog(auditLogDetailInfos, ref dbModules, commitChange);
        }

        public Response Get(SqlAuditLogEventInfo searchModel)
        {
            IList<SqlAuditLogEventInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<SqlAuditLogEventInfo>>(this._repository.Get(searchModel));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<string> moduleNames)
        {
            IList<SqlAuditLogDetailInfo> result = null;
            Exception exception = null;
            try
            {
                result = _mapper.Map<IList<SqlAuditLogDetailInfo>>(this._repository.Get(moduleNames));
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), moduleNames);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(SqlAuditLogEventSearchInfo searchModel)
        {
            IList<SqlAuditLogDetailInfo> result = null;
            Exception exception = null;
            try
            {
                result = new List<SqlAuditLogDetailInfo>();

                var searchResponse = this._repository.Get(searchModel);
                var logEvents = _mapper.Map<IList<SqlAuditLogEventInfo>>(searchResponse).ToList();
                var logEventDetails = searchResponse.SelectMany(x => x.SqlauditLogDetail).ToList();

                logEvents.ForEach(x =>
                {
                    var detail = _mapper.Map<SqlAuditLogDetailInfo>(x);
                    var dbDetail = logEventDetails.FirstOrDefault(x1 => x1.SqlAuditLogId == x.LogId);
                    if (dbDetail != null)
                    {
                        detail.AuditSubModuleName = dbDetail.SqlAuditSubModule.ModuleName;
                        detail.OldValue = dbDetail.OldValue;
                        detail.NewValue = dbDetail.NewValue;
                    }
                    result.Add(detail);
                });
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response IsRecordValidForProcess(IList<SqlAuditLogEventInfo> auditLogEventInfos, ValidationType validationType, ref IList<DbModel.SqlauditModule> dbModules)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(auditLogEventInfos, ref dbModules, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), auditLogEventInfos);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsValidPayload(IList<SqlAuditLogEventInfo> auditLogEventInfos, ValidationType validationType, ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(auditLogEventInfos), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.SqlAuditLog, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsMasterDataValid(IList<SqlAuditLogEventInfo> auditLogEventInfos, ref IList<DbModel.SqlauditModule> dbModules, ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            if (auditLogEventInfos != null && auditLogEventInfos.Count > 0)
            {
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                IList<string> modules = auditLogEventInfos.Select(x => x.AuditModuleName).ToList();
                IList<string> moduleNameNotExist = null;
                result = _sqlAuditModuleService.IsValidModule(modules, ref dbModules, ref moduleNameNotExist, ref validationMessages);
            }
            return result;
        }

        private bool IsRecordValidForAdd(IList<SqlAuditLogEventInfo> auditLogEventInfos, ref IList<DbModel.SqlauditModule> dbModules, ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            if (auditLogEventInfos != null && auditLogEventInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                result = IsValidPayload(auditLogEventInfos, validationType, ref validationMessages);
                if (result)
                    result = IsMasterDataValid(auditLogEventInfos, ref dbModules, ref validationMessages);

            }
            return result;
        }

        private Response AddAuditLogEvent(IList<SqlAuditLogEventInfo> auditLogEventInfos, ref IList<DbModel.SqlauditModule> dbModules, bool commitChange = true)
        {
            Exception exception = null;
            IList<long> result = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                valdResponse = IsRecordValidForProcess(auditLogEventInfos, ValidationType.Add, ref dbModules);
                if (Convert.ToBoolean(valdResponse.Result))
                {
                    auditLogEventInfos = auditLogEventInfos.Select(x => { x.LogId = null; return x; }).ToList();
                    var dbModuleCopy = dbModules;
                    _repository.AutoSave = false;
                    var mappedData = _mapper.Map<IList<DbModel.SqlauditLogEvent>>(auditLogEventInfos, opt =>
                      {
                          opt.Items["dbModules"] = dbModuleCopy;
                      });
                    _repository.Add(mappedData);
                    if (commitChange)
                    {
                        _repository.ForceSave();
                        result = mappedData.Select(x => x.Id).ToList();
                    }
                }
                else
                    return valdResponse;
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), auditLogEventInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        public Response GetAll(SqlAuditLogEventSearchInfo searchModel)
        {
            IList<SqlAuditLogDetailInfo> result = null;
            Exception exception = null;
            try
            {
                result = new List<SqlAuditLogDetailInfo>();
                result = _repository.GetAuditData(searchModel);
                if (result != null)
                {
                    if (result?.Count() > 0)
                    {
                        result?.ToList().ForEach(data =>
                        {
                            if (!string.IsNullOrEmpty(data.OldValue) && !string.IsNullOrEmpty(data.NewValue))
                            {
                                data.DiffValue = CompareJson.CompareObjects(data.OldValue, data.NewValue);
                                if (data.DiffValue == "{}")
                                {
                                    data.DiffValue = data?.DiffValue?.ToString().Replace("{}", null);
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
        public Response GetAuditEvent(SqlAuditLogEventSearchInfo searchModel)
        {
            IList<SqlAuditLogDetailInfo> result = null;
            Exception exception = null;
            try
            {
                result = _repository.GetAuditEvent(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
        public Response GetAuditLog(SqlAuditLogEventSearchInfo searchModel)
        {
            IList<SqlAuditLogDetailInfo> result = null;
            Exception exception = null;
            try
            {
                result = _repository.GetAuditLog(searchModel);
                if (result?.Count() > 0)
                {
                    result?.ToList().ForEach(data =>
                    {
                        if (!string.IsNullOrEmpty(data.OldValue) && !string.IsNullOrEmpty(data.NewValue))
                        {
                            data.DiffValue = CompareJson.CompareObjects(data.OldValue, data.NewValue);
                            if (data.DiffValue == "{}")
                            {
                                data.DiffValue = data?.DiffValue?.ToString().Replace("{}", null);
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
        private Response AddAuditLog(IList<SqlAuditLogDetailInfo> auditLogDetailInfos, ref IList<DbModel.SqlauditModule> dbModules, bool commitChange = true)
        {
            Exception exception = null;
            IList<long> result = null;
            IList<ValidationMessage> validationMessages = null;
            IList<string> moduleNames = null;
            bool isValidModule = true;
            try
            {
                Response valdResponse = null;
                IList<string> moduleNotExists = null;
                if (auditLogDetailInfos?.Count > 0)
                {
                    //auditLogDetailInfos = auditLogDetailInfos.Select(x => { x.LogId = null; return x; }).ToList();
                    moduleNames = auditLogDetailInfos.Select(x => x.AuditModuleName).Where(x => !string.IsNullOrEmpty(x))
                                                     .Union(auditLogDetailInfos.Select(x => x.AuditSubModuleName).Where(x => !string.IsNullOrEmpty(x))).ToList();
                    if (dbModules?.Count == 0)
                        isValidModule = _sqlAuditModuleService.IsValidModule(moduleNames, ref dbModules, ref moduleNotExists, ref validationMessages);
                    if (isValidModule)
                    {
                        IList<SqlAuditLogEventInfo> auditLogEvents = _mapper.Map<IList<SqlAuditLogEventInfo>>(auditLogDetailInfos);
                        valdResponse = IsRecordValidForProcess(auditLogEvents.Where(x => !x.LogId.HasValue).ToList(), ValidationType.Add, ref dbModules);
                        if (Convert.ToBoolean(valdResponse.Result))
                        {
                            valdResponse = _sqlAuditLogDetailInfoService.IsRecordValidForProcess(auditLogDetailInfos, ValidationType.Add, ref dbModules);
                            if (Convert.ToBoolean(valdResponse.Result))
                            {
                                foreach (var logEvent in auditLogDetailInfos.Where(x => !x.LogId.HasValue).ToList())
                                {
                                    valdResponse = this.AddAuditLogEvent(new List<SqlAuditLogEventInfo>() { _mapper.Map<SqlAuditLogEventInfo>(logEvent) }, ref dbModules);
                                    if (valdResponse.Code == ResponseType.Success.ToId())
                                    {
                                        var logIds = valdResponse.Result?.Populate<IList<long>>();
                                        if (logIds?.Count > 0)
                                            logEvent.LogId = logIds.FirstOrDefault();
                                    }
                                    else
                                        break;
                                }

                                if (valdResponse.Code == ResponseType.Success.ToId())
                                    valdResponse = _sqlAuditLogDetailInfoService.Add(auditLogDetailInfos, ref dbModules);
                            }
                        }
                        else
                            return valdResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), auditLogDetailInfos);
            }
            finally
            {
                _repository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }
    }
}