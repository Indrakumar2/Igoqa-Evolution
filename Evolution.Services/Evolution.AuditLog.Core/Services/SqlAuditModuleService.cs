using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Functions;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.AuditLog.Domain.Interfaces.Data;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.AuditLog.Core.Services
{
    public class SqlAuditModuleService : ISqlAuditModuleService
    {
        private readonly ISqlAuditModuleRepository _repository = null;
        private readonly JObject _messages = null;
        //private readonly IAuditLogger _auditLogger = null;

        #region Constructor
        public SqlAuditModuleService(ISqlAuditModuleRepository repository,
                                    // IAuditLogger auditLogger,
                                     JObject messgaes)
        {
            this._repository = repository;
            this._messages = messgaes;
            //this._auditLogger = auditLogger;
        }
        #endregion

        public bool IsValidModule(IList<string> moduleNames, ref IList<DbModel.SqlauditModule> dbModules, ref IList<string> moduleNameNotExists, ref IList<ValidationMessage> validationMessages)
        {
            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            if (dbModules == null || dbModules?.Count == 0)
                dbModules = this.GetModuleByName(moduleNames);

            if (dbModules == null)
                dbModules = new List<DbModel.SqlauditModule>();

            var validMessages = validationMessages;
            var dbModulesCopy = dbModules;
            if (moduleNames?.Count > 0)
            {
                moduleNameNotExists = moduleNames
                                     .Where(x => !dbModulesCopy.Any(x1 => x1.ModuleName == x))
                                     .Select(x => x).ToList();

                moduleNameNotExists?.ToList().ForEach(x =>
                {
                    validMessages.Add(_messages, x, MessageType.AuditModuleNotExists, x);
                });
            }

            if (validMessages.Count > 0)
                validationMessages = validMessages;

            return validationMessages.Count <= 0;
        }

        private IList<DbModel.SqlauditModule> GetModuleByName(IList<string> modules)
        {
            IList<DbModel.SqlauditModule> dbModules = null;
            if (modules?.Count > 0)
                dbModules = _repository.FindBy(x => modules.Contains(x.ModuleName)).ToList();

            return dbModules;
        }

        //public Response AuditLog(object Models,
        //                        ref long? eventId,
        //                        string actionByUser,
        //                        string ModuleId,
        //                        SqlAuditActionType sqlAuditActionType,
        //                        SqlAuditModuleType sqlAuditModuleType,
        //                         object oldData,
        //                         object newData)
        //{
        //    LogEventGeneration logEventGeneration = new LogEventGeneration(_auditLogger);
        //    Exception exception = null;
        //    long? logEventId = 0;
        //    if (Models != null)
        //    {

        //        string actionBy = actionByUser;
        //        if (eventId > 0)
        //            logEventId = eventId;
        //        else
        //            eventId = logEventGeneration.GetEventLogId(logEventId,
        //                                                          sqlAuditActionType,
        //                                                          actionBy,
        //                                                          ModuleId ?? null,
        //                                                          sqlAuditModuleType.ToString());

        //        return _auditLogger.LogAuditData((long)logEventId, sqlAuditModuleType, oldData, newData);
        //    }
        //    return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        //}
    }
}
