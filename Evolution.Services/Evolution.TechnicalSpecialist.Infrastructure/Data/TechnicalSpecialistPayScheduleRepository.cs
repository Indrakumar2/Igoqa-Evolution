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
  public  class TechnicalSpecialistPayScheduleRepository : GenericRepository<DbModel.TechnicalSpecialistPaySchedule>, ITechnicalSpecialistPayScheduleRepository
    {

        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistPayScheduleRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistPaySchedule> Search(TechnicalSpecialistPayScheduleInfo model)
        {
            var tsAsQueryable = this.PopulateTsPayScheduleAsQuerable(model);
            return this.Get(tsAsQueryable, null, null, null);
        }

        public IList<DbModel.TechnicalSpecialistPaySchedule> Get(IList<int> payScheduleIds)
        {
            return Get(null, payScheduleIds, null, null);
        }

        public IList<DbModel.TechnicalSpecialistPaySchedule> GetByPinId(IList<string> pins)
        {
            return Get(null, null, pins, null);
        }

        public IList<DbModel.TechnicalSpecialistPaySchedule> Get(IList<string> payScheduleNames)
        {
            return Get(null, null, null, payScheduleNames);
        }

        public IList<DbModel.TechnicalSpecialistPaySchedule> Get(IList<KeyValuePair<string, string>> ePinAndPayScheduleName)
        {
            var pins = ePinAndPayScheduleName?.Select(x => x.Key).ToList();
            var payScheduleNames = ePinAndPayScheduleName?.Select(x => x.Value).ToList();

            return Get(null, null, pins, payScheduleNames);
        }

        private IList<DbModel.TechnicalSpecialistPaySchedule> Get(IQueryable<DbModel.TechnicalSpecialistPaySchedule> tsPayScheduleAsQuerable = null,
                                                            IList<int> payscheduleIds = null,
                                                            IList<string> pins = null,
                                                            IList<string> payScheduleName = null)
        {
            if (tsPayScheduleAsQuerable == null)
                tsPayScheduleAsQuerable = _dbContext.TechnicalSpecialistPaySchedule;

            if (payscheduleIds?.Count > 0)
                tsPayScheduleAsQuerable = tsPayScheduleAsQuerable.Where(x => payscheduleIds.Contains(x.Id));

            if (payScheduleName?.Count > 0)
                tsPayScheduleAsQuerable = tsPayScheduleAsQuerable.Where(x => payScheduleName.Contains(x.PayScheduleName));

            if (pins?.Count > 0)
                tsPayScheduleAsQuerable = tsPayScheduleAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tsPayScheduleAsQuerable.Include(x => x.TechnicalSpecialist)                                   
                                    .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistPaySchedule> PopulateTsPayScheduleAsQuerable(TechnicalSpecialistPayScheduleInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistPaySchedule>(model);
            IQueryable<DbModel.TechnicalSpecialistPaySchedule> tsPayScheduleAsQueryable = _dbContext.TechnicalSpecialistPaySchedule;

            #region Wildcard Search for TS EPin
            if (model.Epin > 0)
                tsPayScheduleAsQueryable = tsPayScheduleAsQueryable.Where(x => x.TechnicalSpecialist.Pin == model.Epin);
            #endregion
         
            
            var defWhereExpr = dbSearchModel.ToExpression(new List<string> { nameof(dbSearchModel.IsActive) });
            if (defWhereExpr != null)
                tsPayScheduleAsQueryable = tsPayScheduleAsQueryable.Where(defWhereExpr);

            return tsPayScheduleAsQueryable;
        }
    }
}
