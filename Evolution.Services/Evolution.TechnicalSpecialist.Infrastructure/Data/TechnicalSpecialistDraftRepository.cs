using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistDraftRepository : GenericRepository<DbModel.Draft>, ITechnicalSpecialistDraftRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        /// <summary>
        /// TODO :  Replace Object to DBContext
        /// </summary>
        /// <param name="dbContext"></param>
        /// 
        public TechnicalSpecialistDraftRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.TechnicalSpecialistDraft> Search(DomainModel.TechnicalSpecialistDraft searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.Draft>(searchModel);
           
            IQueryable<DbModel.Draft> whereClause = _dbContext.Draft;

            if (!string.IsNullOrEmpty(searchModel.DraftId))
                whereClause = whereClause.Where(x => x.DraftId == searchModel.DraftId);

            if (searchModel.Moduletype.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Moduletype, searchModel.Moduletype, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.Moduletype) || x.Moduletype == searchModel.Moduletype);

            if (searchModel.AssignedBy.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.AssignedBy, searchModel.AssignedBy, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignedBy) || x.AssignedBy == searchModel.AssignedBy);

            if (searchModel.AssignedTo.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.AssignedTo, searchModel.AssignedTo, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.AssignedTo) || x.AssignedTo == searchModel.AssignedTo);

            var expression = dbSearchModel.ToExpression();

            if (expression == null)
                return whereClause.ProjectTo<DomainModel.TechnicalSpecialistDraft>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.TechnicalSpecialistDraft>().ToList();
        }
    }
}
