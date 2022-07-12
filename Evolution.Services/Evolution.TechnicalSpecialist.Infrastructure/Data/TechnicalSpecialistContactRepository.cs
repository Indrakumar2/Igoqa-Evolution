using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.EntityFrameworkCore;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistContactRepository : GenericRepository<DbModel.TechnicalSpecialistContact>, ITechnicalSpecialistContactRepository
    {

        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistContactRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistContact> Search(DomainModel.TechnicalSpecialistContactInfo model )
        {
            var tsAsQueryable = PopulateTsContactAsQuerable(model);
            return Get(tsAsQueryable, null, null);
        }

        public IList<DbModel.TechnicalSpecialistContact> Search(DomainModel.TechnicalSpecialistContactInfo model , int takeCount)
        {
            var tsAsQueryable = PopulateTsContactAsQuerable(model);
            return Get(tsAsQueryable,takeCount: takeCount);
        }

        public IList<DbModel.TechnicalSpecialistContact> Get(IList<int> contactIds)
        {
            return Get(null, contactIds, null);
        }

        public IList<DbModel.TechnicalSpecialistContact> GetByPinId(IList<string> pins)
        {
            return Get(null, null, pins);
        }

        public bool UpdateContactSyncStatus(IList<int> tsContactIds)
        {
            if (tsContactIds?.Count > 0)
                _dbContext.Database.ExecuteSqlCommand(string.Format("UPDATE [techspecialist].TechnicalSpecialistContact SET IsGeoCordinateSync=1 WHERE Id IN ({0})", string.Join(',', tsContactIds)));

            return true;
        }

        private IList<DbModel.TechnicalSpecialistContact> Get(IQueryable<DbModel.TechnicalSpecialistContact> tsContactAsQuerable = null,
                                                              IList<int> contactIds = null,
                                                              IList<string> pins = null,
                                                              int takeCount=0)
        {
            if (tsContactAsQuerable == null)
                tsContactAsQuerable = _dbContext.TechnicalSpecialistContact;

            if (contactIds?.Count > 0)
                tsContactAsQuerable = tsContactAsQuerable.Where(x => contactIds.Contains(x.Id));

            if (pins?.Count > 0)
                tsContactAsQuerable = tsContactAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            if (takeCount > 0)
                tsContactAsQuerable = tsContactAsQuerable.Take(takeCount);

            return tsContactAsQuerable.Include(x => x.TechnicalSpecialist)
                                    .Include(x => x.Country)
                                    .Include(x => x.County)
                                    .Include(x => x.City)
                                    .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistContact> PopulateTsContactAsQuerable(DomainModel.TechnicalSpecialistContactInfo model)
        {
            var dbSearchModel = _mapper.Map<DbModel.TechnicalSpecialistContact>(model);
            
            IQueryable<DbModel.TechnicalSpecialistContact> tsContactAsQueryable = _dbContext.TechnicalSpecialistContact;

            #region Wildcard Search for TS EPin
            if (model.Epin > 0)
                tsContactAsQueryable = tsContactAsQueryable.Where(x => x.TechnicalSpecialist.Pin == model.Epin);
            #endregion

            #region Wildcard Search for CountryName
            if (model.Country.HasEvoWildCardChar())
                tsContactAsQueryable = tsContactAsQueryable.WhereLike(x => x.Country.Name.ToString(), model.Country, '*');
            else if (!string.IsNullOrEmpty(model.Country))
                tsContactAsQueryable = tsContactAsQueryable.Where(x => x.Country.Name.ToString() == model.Country);
            #endregion

            #region Wildcard Search for county
            if (model.County.HasEvoWildCardChar())
                tsContactAsQueryable = tsContactAsQueryable.WhereLike(x => x.County.Name.ToString(), model.County, '*');
            else if (!string.IsNullOrEmpty(model.County))
                tsContactAsQueryable = tsContactAsQueryable.Where(x => x.County.Name.ToString() == model.County);
            #endregion

            #region Wildcard Search for city
            if (model.City.HasEvoWildCardChar())
                tsContactAsQueryable = tsContactAsQueryable.WhereLike(x => x.City.Name.ToString(), model.City, '*');
            else if (!string.IsNullOrEmpty(model.City))
                tsContactAsQueryable = tsContactAsQueryable.Where(x => x.City.Name.ToString() == model.City);
            #endregion

            var defWhereExpr = dbSearchModel.ToExpression();
            if (defWhereExpr != null)
                tsContactAsQueryable = tsContactAsQueryable.Where(defWhereExpr);

            return tsContactAsQueryable;
        }
    }
}
