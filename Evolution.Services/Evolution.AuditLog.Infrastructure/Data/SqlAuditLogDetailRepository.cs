using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Evolution.AuditLog.Domain.Interfaces.Data;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.AuditLog.Domain.Models.Audit;
using Microsoft.EntityFrameworkCore;
using System;

namespace Evolution.AuditLog.Infrastructure.Data
{
    public class SqlAuditLogDetailRepository : GenericRepository<SqlauditLogDetail>, ISqlAuditLogDetailRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public SqlAuditLogDetailRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<SqlauditLogDetail> Search(DomainModel.SqlAuditLogDetailInfo model)
        {
            var dbSearchModel = this._mapper.Map<SqlauditLogDetail>(model);
            var whereClause = dbSearchModel.ToExpression();
            if (whereClause == null)
                return this._dbContext.SqlauditLogDetail.ToList();
            else
                return this._dbContext.SqlauditLogDetail.Where(whereClause).ToList();
        }

        IList<SqlauditLogDetail> ISqlAuditLogDetailRepository.Get(IList<long> auditLogIds)
        {
            if (auditLogIds?.Count > 0)
            {
                return this._dbContext.SqlauditLogDetail
                    .Include("SqlAuditLog")
                    .Include("SqlAuditSubModule")
                    .Where(x => auditLogIds.Contains(x.SqlAuditLogId))
                    .OrderByDescending(x => x.SqlAuditLog.ActionOn)
                    .ToList();
            }
            else
                return null;
        }

        IList<SqlauditLogDetail> ISqlAuditLogDetailRepository.Get(DomainModel.SqlAuditLogEventSearchInfo model)
        {
            if (model != null)
            {
                IQueryable<DbModel.SqlauditLogEvent> whereClause = _dbContext.SqlauditLogEvent;
                if (!string.IsNullOrEmpty(model.AuditModuleName))
                    whereClause = whereClause.Where(x => x.SqlAuditModule.ModuleName == model.AuditModuleName);

                if (!string.IsNullOrEmpty(model.SearchReference))
                    whereClause = whereClause.Where(x => EF.Functions.Like(x.SearchReference, "%" + model.SearchReference.Trim() + "%"));

                // DateTime myFromTime = model.FromDate.Value.Date.Add(DateTime.Now.TimeOfDay);
                DateTime myToTime = Convert.ToDateTime(model.ToDate).Add(DateTime.UtcNow.TimeOfDay);

                if (model.FromDate.HasValue)
                    whereClause = whereClause.Where(x => x.ActionOn.Date.ToUniversalTime() >= Convert.ToDateTime(model.FromDate).Date.ToUniversalTime());
                if (model.ToDate.HasValue)
                    whereClause = whereClause.Where(x => x.ActionOn.Date.ToUniversalTime() <= myToTime.Date.ToUniversalTime());

                var result = _dbContext.SqlauditLogDetail.Join(whereClause,
                                        dbSqlLogDetail => new { dbSqlLogDetail.SqlAuditLogId },
                                        dbSqlLogEvent => new { SqlAuditLogId = dbSqlLogEvent.Id },
                                        (dbSqlLogDetail, dbSqlLogEvent) => new { dbSqlLogDetail, dbSqlLogEvent })
                                        .Join(_dbContext.SqlauditModule,
                                        dbSqlLogDetail1 => new { dbSqlLogDetail1.dbSqlLogDetail.SqlAuditSubModuleId },
                                        dbSqlModule => new { SqlAuditSubModuleId = dbSqlModule.Id },
                                        (dbSqlLogDetail1, dbSqlModule) => new { dbSqlLogDetail1, dbSqlModule })
                                        .OrderByDescending(x=>x.dbSqlLogDetail1.dbSqlLogEvent.ActionOn)
                                        .Select(x => x.dbSqlLogDetail1.dbSqlLogDetail)?
                                        
                                        .ToList();

                return result;
            }
            else
                return null;
        }

    }
}