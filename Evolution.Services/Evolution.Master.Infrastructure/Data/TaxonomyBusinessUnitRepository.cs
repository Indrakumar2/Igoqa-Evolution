using AutoMapper;
using Evolution.GenericDbRepository.Services;
using Evolution.Master.Domain.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Master.Domain.Models;
using Evolution.Master.Domain.Models;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;

namespace Evolution.Master.Infrastructure.Data
{
   public class TaxonomyBusinessUnitRepository : GenericRepository<DbModel.TaxonomyBusinessUnit>, ITaxonomyBusinessUnitRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TaxonomyBusinessUnitRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper):base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.TaxonomyBusinessUnit> Search(TaxonomyBusinessUnit model)
        {
            var dbSearchModel = _mapper.Map<DbModel.TaxonomyBusinessUnit>(model);
            IQueryable<DbModel.TaxonomyBusinessUnit> whereClause = _dbContext.TaxonomyBusinessUnit;
            var expression = dbSearchModel.ToExpression();

            //if (!string.IsNullOrEmpty(model.ProjectType))
            //    whereClause = whereClause.Where(x => x.ProjectType.Name.Trim() == model.ProjectType.Trim());

            if (model.ProjectTypeId> 0)
                whereClause = whereClause.Where(x => x.ProjectTypeId == model.ProjectTypeId);

            if (model.CategoryId > 0)
                whereClause = whereClause.Where(x => x.CategoryId == model.CategoryId);

            if (expression == null)
                return whereClause.ProjectTo< DomainModel.TaxonomyBusinessUnit>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.TaxonomyBusinessUnit>().ToList();
        }
    }
}
