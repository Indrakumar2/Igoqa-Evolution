using AutoMapper;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistStampRepository : GenericRepository<DbModel.TechnicalSpecialistStamp>, ITechnicalSpecialistStampInfoRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistStampRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistStamp> Search(TechnicalSpecialistStampInfo model)
        {
            var tsAsQueryable = this.PopulateTsStampAsQuerable(model);
            return this.Get(tsAsQueryable, null, null,null);
        }

        public IList<DbModel.TechnicalSpecialistStamp> Get(IList<int> stampIds)
        {
            return Get(null,stampIds,null, null);
        }

        public IList<DbModel.TechnicalSpecialistStamp> GetByPinId(IList<string> pins)
        {
            return Get(null, null,pins,null);
        }

        public IList<DbModel.TechnicalSpecialistStamp> Get(IList<string> stampNumbers)
        {
            return Get(null, null,null,stampNumbers);
        }

        public IList<DbModel.TechnicalSpecialistStamp> Get(IList<KeyValuePair<string, string>> ePinAndStampNumbers)
        {
            var pins = ePinAndStampNumbers?.Select(x => x.Key).ToList();
            var stampNums = ePinAndStampNumbers?.Select(x => x.Value).ToList();

            return Get(null, null, pins, stampNums);
        }

        private IList<DbModel.TechnicalSpecialistStamp> Get(IQueryable<DbModel.TechnicalSpecialistStamp> tsStampAsQuerable = null,
                                                            IList<int> stampIds = null,
                                                            IList<string> pins = null,
                                                            IList<string> stampNumbers = null)
        {
            if (tsStampAsQuerable == null)
                tsStampAsQuerable = _dbContext.TechnicalSpecialistStamp;

            if (stampIds?.Count > 0)
                tsStampAsQuerable = tsStampAsQuerable.Where(x => stampIds.Contains(x.Id));

            if (stampNumbers?.Count > 0)
                tsStampAsQuerable = tsStampAsQuerable.Where(x => stampNumbers.Contains(x.StampNumber));

            if (pins?.Count > 0)
                tsStampAsQuerable = tsStampAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tsStampAsQuerable.Include(x => x.TechnicalSpecialist)
                                    .Include(x => x.Country)
                                    .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistStamp> PopulateTsStampAsQuerable(TechnicalSpecialistStampInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistStamp>(model);
            IQueryable<DbModel.TechnicalSpecialistStamp> tsStampAsQueryable = _dbContext.TechnicalSpecialistStamp;

            #region Wildcard Search for TS EPin
            if (model.Epin > 0)
                tsStampAsQueryable = tsStampAsQueryable.Where(x => x.TechnicalSpecialist.Pin == model.Epin);
            #endregion

            #region Wildcard Search for Country Name
            if (model.CountryName.HasEvoWildCardChar())
                tsStampAsQueryable = tsStampAsQueryable.WhereLike(x => x.Country.Name.ToString(), model.CountryName, '*');
            else if (!string.IsNullOrEmpty(model.CountryName))
                tsStampAsQueryable = tsStampAsQueryable.Where(x => x.Country.Name.ToString() == model.CountryName);
            #endregion

            #region Wildcard Search for Country Code
            if (model.CountryCode.HasEvoWildCardChar())
                tsStampAsQueryable = tsStampAsQueryable.WhereLike(x => x.Country.Code.ToString(), model.CountryCode, '*');
            else if (!string.IsNullOrEmpty(model.CountryCode))
                tsStampAsQueryable = tsStampAsQueryable.Where(x => x.Country.Code.ToString() == model.CountryCode);
            #endregion

            //var defWhereExpr = dbSearchModel.ToExpression() ;
            //if (defWhereExpr != null)
            //    tsStampAsQueryable = tsStampAsQueryable.Where(defWhereExpr);

            return tsStampAsQueryable;
        }
    }
}
