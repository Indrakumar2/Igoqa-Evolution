using AutoMapper;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public  class TechnicalSpecialistPayRateRepository : GenericRepository<DbModel.TechnicalSpecialistPayRate>, ITechnicalSpecialistPayRateRepository
    {

        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistPayRateRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistPayRate> Search(TechnicalSpecialistPayRateInfo model)
        {
            var tsAsQueryable = this.PopulateTsPayRateAsQuerable(model);
            return this.Get(tsAsQueryable, null, null, null);
        }

        public IList<DbModel.TechnicalSpecialistPayRate> Get(IList<int> payRateIds)
        {
            return Get(null, payRateIds, null, null);
        }

        public IList<DbModel.TechnicalSpecialistPayRate> GetByPinId(IList<string> pins)
        {
            return Get(null, null, pins, null);
        }

        public IList<DbModel.TechnicalSpecialistPayRate> Get(IList<string> payScheduleNames)
        {
            return Get(null, null, null, payScheduleNames);
        }

        public IList<DbModel.TechnicalSpecialistPayRate> Get(IList<KeyValuePair<string, string>> ePinAndPayScheduleName)
        {
            var pins = ePinAndPayScheduleName?.Select(x => x.Key).ToList();
            var payScheduleNames = ePinAndPayScheduleName?.Select(x => x.Value).ToList();

            return Get(null, null, pins, payScheduleNames);
        }

        private IList<DbModel.TechnicalSpecialistPayRate> Get(IQueryable<DbModel.TechnicalSpecialistPayRate> tsPayRateAsQuerable = null,
                                                            IList<int> payRateIds = null,
                                                            IList<string> pins = null,
                                                            IList<string> payScheduleNames = null)
        {
            if (tsPayRateAsQuerable == null)
                tsPayRateAsQuerable = _dbContext.TechnicalSpecialistPayRate;

            if (payRateIds?.Count > 0)
                tsPayRateAsQuerable = tsPayRateAsQuerable.Where(x => payRateIds.Contains(x.Id));

            if (payScheduleNames?.Count > 0)
                tsPayRateAsQuerable = tsPayRateAsQuerable.Where(x => payScheduleNames.Contains(x.PaySchedule.PayScheduleName.ToString()));

            if (pins?.Count > 0)
                tsPayRateAsQuerable = tsPayRateAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tsPayRateAsQuerable.Include(x => x.TechnicalSpecialist)
                                      .Include(x => x.ExpenseType)
                                      .Include(x => x.PaySchedule)                            
                                      .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistPayRate> PopulateTsPayRateAsQuerable(TechnicalSpecialistPayRateInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistPayRate>(model);
            dbSearchModel.PayScheduleId =model.PayScheduleId ?? 0 ;
            IQueryable<DbModel.TechnicalSpecialistPayRate> tsPayRateAsQueryable = _dbContext.TechnicalSpecialistPayRate;

            #region Wildcard Search for TS EPin

            if (model.Epin > 0)
                tsPayRateAsQueryable = tsPayRateAsQueryable.Where(x => x.TechnicalSpecialist.Pin == model.Epin);

            #endregion
             

            var defWhereExpr = dbSearchModel.ToExpression(new List<string> { nameof(dbSearchModel.IsActive),nameof(dbSearchModel.IsDefaultPayRate), nameof(dbSearchModel.IsHideOnTsExtranet) });
            if (defWhereExpr != null)
                tsPayRateAsQueryable = tsPayRateAsQueryable.Where(defWhereExpr);

                return model.IsActive != null ? tsPayRateAsQueryable.Where(x => x.IsActive == model.IsActive): tsPayRateAsQueryable;
        }
    }
}
