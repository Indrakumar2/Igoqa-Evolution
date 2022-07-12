using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Assignment.Domain.Interfaces.Data;
using Evolution.Assignment.Domain.Models.Assignments;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Assignment.Domain.Models.Assignments;

namespace Evolution.Assignment.Infrastructure.Data
{
    public class AssignmentTaxonomyRepository : GenericRepository<DbModel.AssignmentTaxonomy>, IAssignmentTaxonomyRepository
    {
        private DbModel.EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public AssignmentTaxonomyRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.AssignmentTaxonomy> search(AssignmentTaxonomy searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.AssignmentTaxonomy>(searchModel);
            var expression = dbSearchModel.ToExpression();
            IQueryable<DbModel.AssignmentTaxonomy> whereClause = FilterRecord(searchModel);
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.AssignmentTaxonomy>().ToList();
            return whereClause.Where(expression).ProjectTo<DomainModel.AssignmentTaxonomy>().ToList();

        }

        private IQueryable<DbModel.AssignmentTaxonomy> FilterRecord(AssignmentTaxonomy searchModel)
        {
            var dbTaxonomy = _dbContext.AssignmentTaxonomy;
            IQueryable<DbModel.AssignmentTaxonomy> whereClause = null;

          
            // Taxonomy Categroy
            if (searchModel.TaxonomyCategory.HasEvoWildCardChar())
                whereClause = dbTaxonomy.WhereLike(x => x.TaxonomyService.TaxonomySubCategory.TaxonomyCategory.Name, searchModel.TaxonomyCategory, '*');
            else
                whereClause = dbTaxonomy.Where(x => string.IsNullOrEmpty(searchModel.TaxonomyCategory) || x.TaxonomyService.TaxonomySubCategory.TaxonomyCategory.Name == searchModel.TaxonomyCategory);

            // Taxonomy SubCategory
            if (searchModel.TaxonomySubCategory.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.TaxonomyService.TaxonomySubCategory.TaxonomySubCategoryName, searchModel.TaxonomyCategory, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.TaxonomySubCategory) || x.TaxonomyService.TaxonomySubCategory.TaxonomySubCategoryName == searchModel.TaxonomySubCategory);

            // Taxonomy Service
            if (searchModel.TaxonomyService.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.TaxonomyService.TaxonomyServiceName, searchModel.TaxonomyService, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.TaxonomyService) || x.TaxonomyService.TaxonomyServiceName == searchModel.TaxonomyService);

            return whereClause;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
        }
    }
}
