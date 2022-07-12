using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Evolution.AuditLog.Domain.Interfaces.Data;
using DomainModel = Evolution.AuditLog.Domain.Models.Audit;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Interfaces;
using Evolution.GenericDbRepository.Services;
using Microsoft.EntityFrameworkCore;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using AutoMapper.QueryableExtensions;

namespace Evolution.AuditLog.Infrastructure.Data
{
    public class AuditSearchReposiotry : GenericRepository<DbModel.AuditSearch>, IAuditSearchRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public AuditSearchReposiotry(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.AuditSearch> Search(DomainModel.AuditSearch searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.AuditSearch>(searchModel);
            var whereClause = this.GetAll();

            if (!string.IsNullOrEmpty(searchModel.Module))
                whereClause = whereClause?.Where(x => x.Module.ModuleName == searchModel.Module);

            if (!string.IsNullOrEmpty(searchModel.SearchName))
                whereClause = whereClause?.Where(x => x.SearchName == searchModel.SearchName);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause?.ProjectTo<DomainModel.AuditSearch>().ToList();
            else
                return whereClause?.Where(expression)?.ProjectTo<DomainModel.AuditSearch>().ToList();
        }

        public IList<DbModel.SqlauditModule> GetAuditModule(IList<string> moduleList)
        {
           return _dbContext.SqlauditModule.Where(x => moduleList.Contains(x.ModuleName))?.ToList();
        }
        
    }
}