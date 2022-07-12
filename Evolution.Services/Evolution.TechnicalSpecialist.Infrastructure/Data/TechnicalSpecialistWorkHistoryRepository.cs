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
    public class TechnicalSpecialistWorkHistoryRepository : GenericRepository<DbModel.TechnicalSpecialistWorkHistory>, ITechnicalSpecialistWorkHistoryRepository

    {

        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistWorkHistoryRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistWorkHistory> Search(TechnicalSpecialistWorkHistoryInfo model)
        {
            var tsAsQueryable = this.PopulateTsWorkHistoryAsQuerable(model);
            return this.Get(tsAsQueryable, null,null);
        }

        public IList<DbModel.TechnicalSpecialistWorkHistory> Get(IList<int> workHistoryIds)
        {
            return Get(null, workHistoryIds, null);
        }

        public IList<DbModel.TechnicalSpecialistWorkHistory> GetByPinId(IList<string> pins)
        {
            return Get(null, null, pins);
        }


        private IList<DbModel.TechnicalSpecialistWorkHistory> Get(IQueryable<DbModel.TechnicalSpecialistWorkHistory> tsWorkHistoryAsQuerable = null,
                                                            IList<int> WorkhistoryIds = null,
                                                            IList<string> pins = null)

        {
            if (tsWorkHistoryAsQuerable == null)
                tsWorkHistoryAsQuerable = _dbContext.TechnicalSpecialistWorkHistory;

            if (WorkhistoryIds?.Count > 0)
                tsWorkHistoryAsQuerable = tsWorkHistoryAsQuerable.Where(x => WorkhistoryIds.Contains(x.Id));


            if (pins?.Count > 0)
                tsWorkHistoryAsQuerable = tsWorkHistoryAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tsWorkHistoryAsQuerable.Include(x => x.TechnicalSpecialist)
                                    .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistWorkHistory> PopulateTsWorkHistoryAsQuerable(TechnicalSpecialistWorkHistoryInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistWorkHistory>(model);
            IQueryable<DbModel.TechnicalSpecialistWorkHistory> tsWorkHistoryAsQueryable = _dbContext.TechnicalSpecialistWorkHistory;

            #region Wildcard Search for TS EPin
            if (model.Epin > 0)
                tsWorkHistoryAsQueryable = tsWorkHistoryAsQueryable.Where(x => x.TechnicalSpecialist.Pin == model.Epin);
            #endregion

            var defWhereExpr = dbSearchModel.ToExpression(new List<string> { nameof(dbSearchModel.IsCurrentCompany) });
            if (defWhereExpr != null)
                tsWorkHistoryAsQueryable = tsWorkHistoryAsQueryable.Where(defWhereExpr);

            return tsWorkHistoryAsQueryable;
        }


    }
}