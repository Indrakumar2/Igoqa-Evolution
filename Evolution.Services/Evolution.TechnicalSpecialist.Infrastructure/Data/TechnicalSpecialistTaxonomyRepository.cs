using AutoMapper;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistTaxonomyRepository : GenericRepository<DbModel.TechnicalSpecialistTaxonomy>, ITechnicalSpecialistTaxonomyRepository
    {

        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistTaxonomyRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistTaxonomy> Search(DomainModel.TechnicalSpecialistTaxonomyInfo model)
        {
            var tsAsQueryable = this.PopulateTsStampAsQuerable(model);
            return this.Get(tsAsQueryable, null, null);
        }
        //D684
        public bool IsTaxonomyHistoryExists(int epin)
        { 
            if (epin > 0)
               return _dbContext.TechnicalSpecialistTaxonomyHistory.Count(x => x.TechnicalSpecialistId == epin) > 0;
            return false;
        }

        public IList<DbModel.TechnicalSpecialistTaxonomy> Get(IList<int> taxonomyIds)
        {
            return Get(null, taxonomyIds, null);
        }

        public IList<DbModel.TechnicalSpecialistTaxonomy> GetByPinId(IList<string> pins)
        {
            return Get(null, null, pins);
        }      
     

        private IList<DbModel.TechnicalSpecialistTaxonomy> Get(IQueryable<DbModel.TechnicalSpecialistTaxonomy> tsTaxonomyAsQuerable = null,
                                                            IList<int> taxonomyIds = null,
                                                            IList<string> pins = null                                                            
                                                           )
        {
            if (tsTaxonomyAsQuerable == null)
                tsTaxonomyAsQuerable = _dbContext.TechnicalSpecialistTaxonomy;

            if (taxonomyIds?.Count > 0)
                tsTaxonomyAsQuerable = tsTaxonomyAsQuerable.Where(x => taxonomyIds.Contains(x.Id));          

            if (pins?.Count > 0)
                tsTaxonomyAsQuerable = tsTaxonomyAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tsTaxonomyAsQuerable.Include(x => x.TechnicalSpecialist)
                                    .Include(x => x.TaxonomyCategory)
                                    .Include(x => x.TaxonomySubCategory)
                                    .Include(x => x.TaxonomyServices)
                                    .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistTaxonomy> PopulateTsStampAsQuerable(DomainModel.TechnicalSpecialistTaxonomyInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistTaxonomy>(model);
            IQueryable<DbModel.TechnicalSpecialistTaxonomy> tstaxonomyAsQueryable = _dbContext.TechnicalSpecialistTaxonomy;

            #region Wildcard Search for TS EPin
            if (model.Epin > 0)
                tstaxonomyAsQueryable = tstaxonomyAsQueryable.Where(x => x.TechnicalSpecialist.Pin == model.Epin);
            #endregion

            #region Wildcard Search for TaxonomyCategoryName
            if (model.TaxonomyCategoryName.HasEvoWildCardChar())
                tstaxonomyAsQueryable = tstaxonomyAsQueryable.WhereLike(x => x.TaxonomyCategory.Name.ToString(), model.TaxonomyCategoryName, '*');
            else if (!string.IsNullOrEmpty(model.TaxonomyCategoryName))
                tstaxonomyAsQueryable = tstaxonomyAsQueryable.Where(x => x.TaxonomyCategory.Name.ToString() == model.TaxonomyCategoryName);
            #endregion

            #region Wildcard Search for TaxonomySubCategoryName
            if (model.TaxonomySubCategoryName.HasEvoWildCardChar())
                tstaxonomyAsQueryable = tstaxonomyAsQueryable.WhereLike(x => x.TaxonomySubCategory.TaxonomySubCategoryName.ToString(), model.TaxonomySubCategoryName, '*');
            else if (!string.IsNullOrEmpty(model.TaxonomySubCategoryName))
                tstaxonomyAsQueryable = tstaxonomyAsQueryable.Where(x => x.TaxonomySubCategory.TaxonomySubCategoryName.ToString() == model.TaxonomySubCategoryName);
            #endregion

            #region Wildcard Search for TaxonomyServices
            if (model.TaxonomyServices.HasEvoWildCardChar())
                tstaxonomyAsQueryable = tstaxonomyAsQueryable.WhereLike(x => x.TaxonomyServices.TaxonomyServiceName.ToString(), model.TaxonomyServices, '*');
            else if (!string.IsNullOrEmpty(model.TaxonomyServices))
                tstaxonomyAsQueryable = tstaxonomyAsQueryable.Where(x => x.TaxonomyServices.TaxonomyServiceName.ToString() == model.TaxonomyServices);
            #endregion

            var defWhereExpr = dbSearchModel.ToExpression();
            if (defWhereExpr != null)
                tstaxonomyAsQueryable = tstaxonomyAsQueryable.Where(defWhereExpr);

            return tstaxonomyAsQueryable;
        }
    }
}
