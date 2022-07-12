using AutoMapper;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel=Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
   public class TechnicalSpecialistTimeOffRequestRepository :GenericRepository<DbModel.TechnicalSpecialistTimeOffRequest>,ITechnicalSpecialistTimeOffRequestRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistTimeOffRequestRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) :base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.TechnicalSpecialistTimeOffRequest> Search(DomainModel.TechnicalSpecialistTimeOffRequest model, params Expression<Func<DbModel.TechnicalSpecialistTimeOffRequest, object>>[] includes)
        {
            var dbSearchModel = _mapper.Map<DbModel.TechnicalSpecialistTimeOffRequest>(model);
            var technicalSpecialistTimeOffRequest = _dbContext.TechnicalSpecialistTimeOffRequest;

            IQueryable<DbModel.TechnicalSpecialistTimeOffRequest> whereClause = null;

            if (model.ResourceName.HasEvoWildCardChar())
                whereClause = _dbContext.TechnicalSpecialistTimeOffRequest.WhereLike(x => x.TechnicalSpecialist.LastName + x.TechnicalSpecialist.FirstName, model.ResourceName, '*');
            else
            whereClause = _dbContext.TechnicalSpecialistTimeOffRequest.Where(x => string.IsNullOrEmpty(model.ResourceName) || x.TechnicalSpecialist.LastName + x.TechnicalSpecialist.FirstName == model.ResourceName);

            var expression = dbSearchModel.ToExpression();
            if (includes.Any())
                whereClause = includes.Aggregate(whereClause, (current, include) => current.Include(include));


            if (expression == null)
                    return technicalSpecialistTimeOffRequest.ProjectTo<DomainModel.TechnicalSpecialistTimeOffRequest>().ToList();
                else
                    return technicalSpecialistTimeOffRequest.Where(expression).ProjectTo<DomainModel.TechnicalSpecialistTimeOffRequest>().ToList();
        }
    }
}
