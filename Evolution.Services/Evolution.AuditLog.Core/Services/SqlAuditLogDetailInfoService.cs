using AutoMapper;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.AuditLog.Domain.Interfaces.Data;
using Evolution.AuditLog.Domain.Interfaces.Validations;
using Evolution.AuditLog.Domain.Models.Audit;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Extensions;
using System.Linq;
using Newtonsoft.Json;

namespace Evolution.AuditLog.Core.Services
{
    public class SqlAuditLogDetailInfoService : ISqlAuditLogDetailInfoService
    {
        private readonly IAppLogger<SqlAuditLogDetailInfoService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly ISqlAuditLogDetailRepository _repository = null;
        private readonly JObject _messages = null;
        private readonly ISqlAditLogDetailValidationService _validationService = null;
        private readonly ISqlAuditModuleService _sqlAuditModuleService = null;

        #region Constructor
        public SqlAuditLogDetailInfoService(IMapper mapper,
                                             ISqlAuditLogDetailRepository repository,
                                             IAppLogger<SqlAuditLogDetailInfoService> logger,
                                             ISqlAditLogDetailValidationService validationService,
                                             ISqlAuditModuleService sqlAuditModuleService,
                                             JObject messgaes)
        {
            this._repository = repository;
            this._logger = logger;
            this._mapper = mapper;
            this._validationService = validationService;
            this._messages = messgaes;
            this._sqlAuditModuleService = sqlAuditModuleService;
        }
        #endregion

        public Response Add(IList<SqlAuditLogDetailInfo> auditLogDetailInfos, ref IList<DbModel.SqlauditModule> dbModules, bool commitChange = true)
        {
            return AddAuditLogDetail(auditLogDetailInfos,ref dbModules, commitChange);
        }

        public Response IsRecordValidForProcess(IList<SqlAuditLogDetailInfo> auditLogDetailInfos, ValidationType validationType, ref IList<DbModel.SqlauditModule> dbModules)
        {
            Exception exception = null;
            bool result = false;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                if (validationType == ValidationType.Add)
                    result = IsRecordValidForAdd(auditLogDetailInfos, ref dbModules, ref validationMessages);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), auditLogDetailInfos);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), result, exception);
        }

        private bool IsValidPayload(IList<SqlAuditLogDetailInfo> auditLogDetailInfos, ValidationType validationType, ref IList<ValidationMessage> validationMessages)
        {
            IList<ValidationMessage> messages = new List<ValidationMessage>();
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(auditLogDetailInfos), validationType);
            if (validationResults?.Count > 0)
                messages.Add(_messages, ModuleType.SqlAuditLog, validationResults);

            if (validationResults.Count > 0)
                validationMessages.AddRange(messages);

            return validationMessages?.Count <= 0;
        }

        private bool IsMasterDataValid(IList<SqlAuditLogDetailInfo> filteredAudit, ref IList<DbModel.SqlauditModule> dbModules, ref IList<ValidationMessage> validationMessages)
        {
            bool result = true;
            if (filteredAudit != null && filteredAudit.Count > 0)
            {
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                IList<string> modules = filteredAudit.Select(x => x.AuditSubModuleName).ToList();
                IList<string> moduleNameNotExist = null;
                result = _sqlAuditModuleService.IsValidModule(modules, ref dbModules,ref moduleNameNotExist, ref validationMessages);
            }
            return result;
        }

        private bool IsRecordValidForAdd(IList<SqlAuditLogDetailInfo> auditLogDetailInfos, ref IList<DbModel.SqlauditModule> dbModules, ref IList<ValidationMessage> validationMessages)
        {
            bool result = false;
            if (auditLogDetailInfos != null && auditLogDetailInfos.Count > 0)
            {
                ValidationType validationType = ValidationType.Add;
                if (validationMessages == null)
                    validationMessages = new List<ValidationMessage>();

                if (IsValidPayload(auditLogDetailInfos, validationType, ref validationMessages))
                    result = IsMasterDataValid(auditLogDetailInfos, ref dbModules, ref validationMessages);

            }
            return result;
        }

        private Response AddAuditLogDetail(IList<SqlAuditLogDetailInfo> auditLogDetailInfos, ref IList<DbModel.SqlauditModule> dbModules, bool commitChange = true)
        {
            Exception exception = null;
            IList<ValidationMessage> validationMessages = null;
            try
            {
                Response valdResponse = null;
                valdResponse = IsRecordValidForProcess(auditLogDetailInfos, ValidationType.Add, ref dbModules);
                var dbModule = dbModules;
                if (Convert.ToBoolean(valdResponse.Result))
                {
                    _repository.AutoSave = false;
                    _repository.Add(_mapper.Map<IList<DbModel.SqlauditLogDetail>>(auditLogDetailInfos, opt =>
                    {
                        opt.Items["dbModules"] = dbModule;
                    }));

                    //_repository.Add(_mapper.Map<IList<DbModel.SqlauditLogDetail>>(auditLogDetailInfos));
                    if (commitChange)
                        _repository.ForceSave();
                }
                else
                    return valdResponse;
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

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception);
        }

        public Response Get(SqlAuditLogDetailInfo searchModel)
        {
           IList<SqlAuditLogDetailInfo> result = null;
           Exception exception = null;
           try
           {
               result = _mapper.Map<IList<SqlAuditLogDetailInfo>>(this._repository.Search(searchModel));
           }
           catch (Exception ex)
           {
               exception = ex;
               _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
           }

           return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }
    }
}