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
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.EntityFrameworkCore;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistCodeAndStandardRepository : GenericRepository<DbModel.TechnicalSpecialistCodeAndStandard>, ITechnicalSpecialistCodeAndStandardRepository
    {

        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistCodeAndStandardRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistCodeAndStandard> Search(TechnicalSpecialistCodeAndStandardinfo model)
        {
            var tsAsQueryable = this.PopulateTsCodeAndStandardAsQuerable(model);
            return this.Get(tsAsQueryable, null, null, null);
        }

        public IList<DbModel.TechnicalSpecialistCodeAndStandard> Get(IList<int> stampIds)
        {
            return Get(null, stampIds, null, null);
        }

        public IList<DbModel.TechnicalSpecialistCodeAndStandard> GetByPinId(IList<string> pins)
        {
            return Get(null, null, pins, null);
        }

        public IList<DbModel.TechnicalSpecialistCodeAndStandard> Get(IList<string> CodeandStandard)
        {
            return Get(null, null, null, CodeandStandard);
        }

        public IList<DbModel.TechnicalSpecialistCodeAndStandard> Get(IList<KeyValuePair<string, string>> ePinAndCodeandStandard)
        {
            var pins = ePinAndCodeandStandard?.Select(x => x.Key).ToList();
            var CodeStandardNames = ePinAndCodeandStandard?.Select(x => x.Value).ToList();

            return Get(null, null, pins, CodeStandardNames);
        }

        private IList<DbModel.TechnicalSpecialistCodeAndStandard> Get(IQueryable<DbModel.TechnicalSpecialistCodeAndStandard> tsCodeAndstandardpAsQuerable = null,
                                                            IList<int> CodeStandardIds = null,
                                                            IList<string> pins = null,
                                                            IList<string> CodeStandardName = null)
        {
            if (tsCodeAndstandardpAsQuerable == null)
                tsCodeAndstandardpAsQuerable = _dbContext.TechnicalSpecialistCodeAndStandard;

            if (CodeStandardIds?.Count > 0)
                tsCodeAndstandardpAsQuerable = tsCodeAndstandardpAsQuerable.Where(x => CodeStandardIds.Contains(x.Id));

            if (CodeStandardName?.Count > 0)
                tsCodeAndstandardpAsQuerable = tsCodeAndstandardpAsQuerable.Where(x => CodeStandardName.Contains(x.CodeStandard.Name));

            if (pins?.Count > 0)
                tsCodeAndstandardpAsQuerable = tsCodeAndstandardpAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tsCodeAndstandardpAsQuerable.Include(x => x.TechnicalSpecialist)
                                    .Include(x => x.CodeStandard)
                                    .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistCodeAndStandard> PopulateTsCodeAndStandardAsQuerable(TechnicalSpecialistCodeAndStandardinfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistCodeAndStandard>(model);
            IQueryable<DbModel.TechnicalSpecialistCodeAndStandard> tsCodeAndStandardAsQueryable = _dbContext.TechnicalSpecialistCodeAndStandard;

            #region Wildcard Search for TS EPin
            if (model.Epin > 0)
                tsCodeAndStandardAsQueryable = tsCodeAndStandardAsQueryable.Where(x => x.TechnicalSpecialist.Pin == model.Epin);
            #endregion

            #region Wildcard Search for Country Name
            if (model.CodeStandardName.HasEvoWildCardChar())
                tsCodeAndStandardAsQueryable = tsCodeAndStandardAsQueryable.WhereLike(x => x.CodeStandard.Name.ToString(), model.CodeStandardName, '*');
            else if (!string.IsNullOrEmpty(model.CodeStandardName))
                tsCodeAndStandardAsQueryable = tsCodeAndStandardAsQueryable.Where(x => x.CodeStandard.Name.ToString() == model.CodeStandardName);
            #endregion



            var defWhereExpr = dbSearchModel.ToExpression();
            if (defWhereExpr != null)
                tsCodeAndStandardAsQueryable = tsCodeAndStandardAsQueryable.Where(defWhereExpr);

            return tsCodeAndStandardAsQueryable;
        }


    }
}

