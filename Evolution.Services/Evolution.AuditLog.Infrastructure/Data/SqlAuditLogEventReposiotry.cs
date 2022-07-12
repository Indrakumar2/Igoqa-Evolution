using AutoMapper;
using Evolution.AuditLog.Domain.Interfaces.Data;
using Evolution.AuditLog.Domain.Models.Audit;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.AuditLog.Infrastructure.Data
{
    public class SqlAuditLogEventReposiotry : GenericRepository<DbModel.SqlauditLogEvent>, ISqlAuditLogEventReposiotry
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<SqlAuditLogEventReposiotry> _logger = null;

        public SqlAuditLogEventReposiotry(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper,
                                          IAppLogger<SqlAuditLogEventReposiotry> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            _logger = logger;
        }

        public IList<DbModel.SqlauditLogEvent> Get(IList<string> moduleNames)
        {
            if (moduleNames?.Count > 0)
                //return _dbContext.SqlauditLogEvent.Where(x => moduleNames.Contains(x.SqlAuditModule.ModuleName)).ToList();
                return this.FindBy(x => moduleNames.Contains(x.SqlAuditModule.ModuleName)).ToList();
            else
                return null;
        }

        public List<SqlAuditLogDetailInfo> GetAuditEvent(SqlAuditLogEventSearchInfo model)
        {
            List<SqlAuditLogDetailInfo> sqlAuditLogDetailInfo = null;
            if (model.AuditModuleId > 0)
            {
                var searchText = "\"" + model.SelectType + ":" + model.SearchReference + "\"";
                model.ToDate = model.ToDate.Value.Date.Add(DateTime.Parse(DateTime.UtcNow.ToShortTimeString()).TimeOfDay);

                //model.ToDate = model.ToDate.Value.Date.Add(TimeSpan.Parse(DateTime.Now.ToShortTimeString()));
                sqlAuditLogDetailInfo = _dbContext.SqlauditLogEvent.Where(x => x.SqlAuditModuleId == model.AuditModuleId &&
                                              x.ActionOn >= model.FromDate.Value &&
                                              x.ActionOn <= model.ToDate.Value &&
                                              EF.Functions.Contains(x.SearchReference, searchText ))
                                .Select(x => new SqlAuditLogDetailInfo
                                {
                                    ActionBy = x.ActionBy,
                                    ActionOn = x.ActionOn,
                                    ActionType = x.ActionType,
                                    AuditModuleName = model.AuditModuleName,
                                    AuditEventId = x.Id,
                                    SearchReference = x.SearchReference
                                })?
                                .OrderByDescending(x=>x.AuditEventId)?
                                .ToList();
            }
            if(AuditSelectType.JobReferenceNumber.ToString().ToLower() == model.SelectType.ToLower()  || AuditSelectType.ProjectAssignment.ToString().ToLower() == model.SelectType.ToLower())
                sqlAuditLogDetailInfo = sqlAuditLogDetailInfo?.Where(x => EF.Functions.Like(x.SearchReference, 
                                                                    "%{" + model.SelectType + ":" + model.SearchReference + "}%"))
                                                         ?.ToList();

            return sqlAuditLogDetailInfo; 
        }

        public List<SqlAuditLogDetailInfo> GetAuditLog(SqlAuditLogEventSearchInfo model)
        {
            List<SqlAuditLogDetailInfo> sqlAuditLogDetailInfo = null;
            sqlAuditLogDetailInfo = _dbContext.SqlauditLogDetail.Include("SQLAuditLogEvent").Include("SQLAuditModule").Where(x => x.SqlAuditLogId == model.LogId)
                            .Select(x => new SqlAuditLogDetailInfo
                            {
                                LogId = x.Id,
                                ActionBy = x.SqlAuditLog.ActionBy,
                                ActionOn = x.SqlAuditLog.ActionOn,
                                ActionType = x.SqlAuditLog.ActionType,
                                AuditSubModuleName = x.SqlAuditSubModule.ModuleName,
                                AuditEventId = x.Id,
                                OldValue = x.OldValue,
                                NewValue = x.NewValue
                            })?
                            .ToList();
            return sqlAuditLogDetailInfo;
        }

        public List<SqlAuditLogDetailInfo> GetAuditData(SqlAuditLogEventSearchInfo model)
        {
            List<SqlAuditLogDetailInfo> sqlAuditLogDetailInfos = null;
            try
            {
                model.SearchReference = model.SearchReference.Contains('*') ? model.SearchReference.Replace('*', '%') : model.SearchReference;
                var searchText = model.SelectType + ":" + model.SearchReference;
                sqlAuditLogDetailInfos = _dbContext.SqlauditLogDetail
                           .Join(_dbContext.SqlauditLogEvent,
                                 ad => new { SqlAuditLogId = ad.SqlAuditLogId },
                                 ade => new { SqlAuditLogId = ade.Id },
                                 (ad, ade) => new { ad, ade })
                           .Join(_dbContext.SqlauditModule,
                                 am => new { ModuleId = am.ade.SqlAuditModuleId },
                                 ac => new { ModuleId = ac.Id },
                                 (am, ac) => new { am, ac })
                            .Where(x => x.ac.ModuleName == model.AuditModuleName &&
                                        (Convert.ToDateTime(x.am.ade.ActionOn).Date >= Convert.ToDateTime(model.FromDate).Date && Convert.ToDateTime(x.am.ade.ActionOn).Date <= Convert.ToDateTime(model.ToDate).Date ||
                                        ((Convert.ToDateTime(x.am.ade.ActionOn).Date >= Convert.ToDateTime(model.FromDate).Date) && Convert.ToDateTime(x.am.ade.ActionOn).Date <= Convert.ToDateTime(model.ToDate).Date) ||
                    (((Convert.ToDateTime(x.am.ade.ActionOn).Date <= Convert.ToDateTime(model.FromDate).Date) && (Convert.ToDateTime(x.am.ade.ActionOn).Date <= Convert.ToDateTime(model.ToDate).Date)) &&
                    (Convert.ToDateTime(x.am.ade.ActionOn).Date >= Convert.ToDateTime(model.FromDate).Date) && (Convert.ToDateTime(x.am.ade.ActionOn).Date >= Convert.ToDateTime(model.ToDate).Date)))
                      //x.am.ade.ActionOn >= model.FromDate.Value.Date && x.am.ade.ActionOn <= model.ToDate.Value.Date &&
                                       && EF.Functions.Contains(x.am.ade.SearchReference, searchText))
                           //EF.Functions.FreeText(x.am.ade.SearchReference, "%{" + model.SelectType + ":" + model.SearchReference + "}%"))
                           .Select(x => new SqlAuditLogDetailInfo
                           {
                               ActionBy = x.am.ade.ActionBy,
                               ActionOn = x.am.ade.ActionOn,
                               ActionType = x.am.ade.ActionType,
                               AuditModuleName = x.ac.ModuleName,
                               AuditSubModuleName = x.am.ad.SqlAuditSubModule.ModuleName,
                               LogId = x.am.ad.SqlAuditLogId,
                               NewValue = x.am.ad.NewValue,
                               OldValue = x.am.ad.OldValue,
                               SearchReference = x.am.ade.SearchReference
                           })?.

                           ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            return sqlAuditLogDetailInfos;
        }

        public IQueryable<DbModel.SqlauditLogEvent> Get(SqlAuditLogEventSearchInfo model)
        {
            var whereClause = _dbContext.SqlauditLogEvent.AsQueryable().Include("SqlauditLogDetail").Include("SqlAuditModule");
            DateTime myToTime = Convert.ToDateTime(model.ToDate).Add(DateTime.UtcNow.TimeOfDay);
            if (!string.IsNullOrEmpty(model.AuditModuleName))
            {
                if (model.AuditModuleName.HasEvoWildCardChar())
                    whereClause = whereClause.WhereLike(x => x.SqlAuditModule.ModuleName, model.AuditModuleName, '*');
                else
                    whereClause = whereClause.Where(x => x.SqlAuditModule.ModuleName == model.AuditModuleName);
            }
            if (model.FromDate != null && model.ToDate != null)
                whereClause = whereClause.Where(x => x.ActionOn >= model.FromDate.Value.Date && x.ActionOn <= model.ToDate.Value.Date);

            if (!string.IsNullOrEmpty(model.SearchReference))
            {
                if (model.SearchReference.HasEvoWildCardChar())
                    whereClause = whereClause.WhereLike(x => x.SearchReference, "{" + model.SelectType + ":" + model.SearchReference + "}", '*');
                else
                    whereClause = whereClause.Where(x => EF.Functions.Like(x.SearchReference, "%{" + model.SelectType + ":" + model.SearchReference + "}%"));
                //whereClause = whereClause.Where(x => x.SearchReference.Contains(model.SearchReference.Trim().ToLower()));
            }

            return whereClause;
        }

        public IList<DbModel.SqlauditLogEvent> Get(SqlAuditLogEventInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.SqlauditLogEvent>(model);
            var whereClause = this.GetAll();

            if (!string.IsNullOrEmpty(model.AuditModuleName))
                whereClause = whereClause.Where(x => x.SqlAuditModule.ModuleName == model.AuditModuleName);

            if (!string.IsNullOrEmpty(model.SearchReference))
            {
                if (model.SearchReference.HasEvoWildCardChar())
                    whereClause = whereClause.WhereLike(x => x.SearchReference, model.SearchReference, '*');
                else
                {
                    var searchRef = model.SearchReference.Split(",").ToString().Trim().ToLower();
                    whereClause = whereClause.Where(x => searchRef.Contains(x.SearchReference.ToLower()));
                    //whereClause = whereClause.Where(x => x.SearchReference == model.SearchReference);
                }

                dbSearchModel.SearchReference = string.Empty;
            }


            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ToList();
            else
                return whereClause.Where(expression).ToList();
        }

    }
}
