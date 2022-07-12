using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Infrastructure.Data
{
    public class AssignmentContributionRevenueCostRepository : GenericRepository<DbModel.AssignmentContributionRevenueCost>, IAssignmentContributionRevenueCostRepository
    {
        private  DbModel.EvolutionSqlDbContext _dbContext = null;
        private  IMapper _mapper = null;

        public AssignmentContributionRevenueCostRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.AssignmentContributionRevenueCost> Search(DomainModel.AssignmentContributionRevenueCost model)
        {
            var dbSearchModel = _mapper.Map<DbModel.AssignmentContributionRevenueCost>(model);
            IQueryable<DbModel.AssignmentContributionRevenueCost> whereClause = _dbContext.AssignmentContributionRevenueCost;

            if (model.Type.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.SectionType, model.Type, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(model.Type) || x.SectionType == model.Type);

            if (model.Description.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Description, model.Type, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(model.Description) || x.Description == model.Description);

            //if (model.AssignmentId > 0)
            //    whereClause = whereClause.Where(x => x.AssignmentContributionCalculation.AssignmentId == model.AssignmentId);

            var expression = dbSearchModel.ToExpression();

            if (expression == null)
                return whereClause.ProjectTo<DomainModel.AssignmentContributionRevenueCost>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.AssignmentContributionRevenueCost>().ToList();
        }


        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
        }
    }
}
