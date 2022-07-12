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
    public class AssignmentContributionCalculationRepository : GenericRepository<DbModel.AssignmentContributionCalculation>,IAssignmentContributionCalculationRepository
    {
        private  DbModel.EvolutionSqlDbContext _dbContext = null;
        private  IMapper _mapper = null;

        public AssignmentContributionCalculationRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DomainModel.AssignmentContributionCalculation> Search(DomainModel.AssignmentContributionCalculation model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.AssignmentContributionCalculation>(model);
            var whereClause = dbSearchModel.ToExpression();
            if (whereClause == null)
            {
                return this._dbContext.AssignmentContributionCalculation.ProjectTo<DomainModel.AssignmentContributionCalculation>().ToList();
            }
            else
                return this._dbContext.AssignmentContributionCalculation.Where(whereClause).ProjectTo<DomainModel.AssignmentContributionCalculation>().ToList();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
        }
    }
}
