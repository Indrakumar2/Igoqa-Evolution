using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.AuditLog.Domain.Interfaces.Data;
using Evolution.AuditLog.Domain.Models.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Newtonsoft.Json.Linq;
using Evolution.AuditLog.Domain.Enums;
using Evolution.AuditLog.Domain.Functions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System.Transactions;

namespace Evolution.AuditLog.Core.Services
{
    public class AuditSearchService : IAuditSearchService
    {
        private readonly IAppLogger<AuditSearchService> _logger = null;
        private readonly IAuditSearchRepository _repository = null;
        private readonly IAuditLogger _auditLogger = null;

        public AuditSearchService(IAppLogger<AuditSearchService> logger,
                                  IAuditSearchRepository repository, 
                                  IAuditLogger auditLogger)
        {
            this._logger = logger;
            _repository = repository;
            _auditLogger = auditLogger;
        }

        public Response GetModuleAndSearchType(string module)
        {
            object result = null;
            Exception exception = null;
            try
            {
                var searchResult = this._repository.Search(new AuditSearch { Module = module });

                result = searchResult.GroupJoin(searchResult,
                                            a => a.Module,
                                            b => b.Module,
                                            (a, b) => new { a, b })
                                            .Select(x => new
                                            {
                                                x.a.ModuleId,
                                                x.a.Module,
                                                x.a.ModuleName,
                                                DispalyName = x.b.Select(x1 => new { Value = x1.SearchName, Name = x1.DispalyName })
                                            })
                                            .GroupBy(x => x.Module)
                                            .Select(x => x?.ToList().FirstOrDefault()).ToList();
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), module);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
        }
        public Response AuditLog(object Models,
                                ref long? eventId,
                                string actionByUser,
                                string searchRef,
                                SqlAuditActionType sqlAuditActionType,
                                SqlAuditModuleType sqlAuditModuleType,
                                object oldData,
                                object newData,
                                IList<DbModel.SqlauditModule> dbModules = null)
        {
            Response result = null;

            try
            {
                if (Models != null && !string.IsNullOrEmpty(actionByUser))
                {
                    if ((!eventId.HasValue || eventId == 0) && !string.IsNullOrEmpty(searchRef))
                    {
                        LogEventGeneration logEventGeneration = new LogEventGeneration(_auditLogger);
                        eventId = logEventGeneration.GetEventLogId(eventId,
                                                                   sqlAuditActionType,
                                                                   actionByUser,
                                                                    searchRef,
                                                                    sqlAuditModuleType.ToString(),
                                                                    dbModules);

                    }
                    if (eventId > 0 && eventId != null)
                        result = _auditLogger.LogAuditData((long)eventId, sqlAuditModuleType, oldData, newData, dbModules);
                }
                else
                    result = new Response().ToPopulate(ResponseType.Success, null, null, null, null, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

            return result;
        }

        /// <summary>
        /// Get Audit Modules 
        /// </summary>
        /// <returns></returns>
        public IList<DbModel.SqlauditModule> GetAuditModule(IList<string> moduleList)
        {
            return this._repository.GetAuditModule(moduleList);
            //using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            //{
            //var modules= this._repository.GetAuditModule(moduleList);
            //tranScope.Complete();

            //return modules;
            // }
        }
    }
}