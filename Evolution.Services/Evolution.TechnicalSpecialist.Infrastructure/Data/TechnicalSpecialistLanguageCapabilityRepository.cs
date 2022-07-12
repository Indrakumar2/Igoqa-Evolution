using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistLanguageCapabilityRepository : GenericRepository<DbModel.TechnicalSpecialistLanguageCapability>, ITechnicalSpecialistLanguageCapabilityRepository
    {

        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistLanguageCapabilityRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistLanguageCapability> Search(TechnicalSpecialistLanguageCapabilityInfo model)
        {
            var tsAsQueryable = this.PopulateTslcAsQuerable(model);
            return this.Get(tsAsQueryable, null, null, null);
        }

        public IList<DbModel.TechnicalSpecialistLanguageCapability> Get(IList<int> stampIds)
        {
            return Get(null, stampIds, null, null);
        }

        public IList<DbModel.TechnicalSpecialistLanguageCapability> GetByPinId(IList<string> pins)
        {
            return Get(null, null, pins, null);
        }

        public IList<DbModel.TechnicalSpecialistLanguageCapability> Get(IList<string> Language)
        {
            return Get(null, null, null, Language);
        }

        public IList<DbModel.TechnicalSpecialistLanguageCapability> Get(IList<KeyValuePair<string, string>> ePinAndLanguage)
        {
            var pins = ePinAndLanguage?.Select(x => x.Key).ToList();
            var LanguageCapabilityNames = ePinAndLanguage?.Select(x => x.Value).ToList();

            return Get(null, null, pins, LanguageCapabilityNames);
        }

        private IList<DbModel.TechnicalSpecialistLanguageCapability> Get(IQueryable<DbModel.TechnicalSpecialistLanguageCapability> tslanaguageCapabilityAsQuerable = null,
                                                            IList<int> LanguageIds = null,
                                                            IList<string> pins = null,
                                                            IList<string> CodeStandardName = null)
        {
            if (tslanaguageCapabilityAsQuerable == null)
                tslanaguageCapabilityAsQuerable = _dbContext.TechnicalSpecialistLanguageCapability;

            if (LanguageIds?.Count > 0)
                tslanaguageCapabilityAsQuerable = tslanaguageCapabilityAsQuerable.Where(x => LanguageIds.Contains(x.Id));

            if (CodeStandardName?.Count > 0)
                tslanaguageCapabilityAsQuerable = tslanaguageCapabilityAsQuerable.Where(x => CodeStandardName.Contains(x.Language.Name));

            if (pins?.Count > 0)
                tslanaguageCapabilityAsQuerable = tslanaguageCapabilityAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tslanaguageCapabilityAsQuerable.Include(x => x.TechnicalSpecialist)
                                    .Include(x => x.Language)
                                    .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistLanguageCapability> PopulateTslcAsQuerable(TechnicalSpecialistLanguageCapabilityInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistLanguageCapability>(model);
            IQueryable<DbModel.TechnicalSpecialistLanguageCapability> tslangaugeCapabilityAsQueryable = _dbContext.TechnicalSpecialistLanguageCapability;

            #region Wildcard Search for TS EPin
            if (model.Epin > 0)
                tslangaugeCapabilityAsQueryable = tslangaugeCapabilityAsQueryable.Where(x => x.TechnicalSpecialist.Pin == model.Epin);
            #endregion

            #region Wildcard Search for Country Name
            if (model.Language.HasEvoWildCardChar())
                tslangaugeCapabilityAsQueryable = tslangaugeCapabilityAsQueryable.WhereLike(x => x.Language.Name.ToString(), model.Language, '*');
            else if (!string.IsNullOrEmpty(model.Language))
                tslangaugeCapabilityAsQueryable = tslangaugeCapabilityAsQueryable.Where(x => x.Language.Name.ToString() == model.Language);
            #endregion



            var defWhereExpr = dbSearchModel.ToExpression();
            if (defWhereExpr != null)
                tslangaugeCapabilityAsQueryable = tslangaugeCapabilityAsQueryable.Where(defWhereExpr);

            return tslangaugeCapabilityAsQueryable;
        }

    }

}
