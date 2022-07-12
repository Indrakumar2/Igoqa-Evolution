using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Document.Domain.Models.Document;
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
    public class TechnicalSpecialistEducationalQualificationRepository : GenericRepository<DbModel.TechnicalSpecialistEducationalQualification>, ITechnicalSpecialistEducationalQualificationRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistEducationalQualificationRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistEducationalQualification> Search(TechnicalSpecialistEducationalQualificationInfo model)
        {
            var tsAsQueryable = this.PopulateTsWorkHistoryAsQuerable(model);
            return this.Get(tsAsQueryable, null, null);
        }

        public IList<DbModel.TechnicalSpecialistEducationalQualification> Get(IList<int> EduQulificationIds)
        {
            return Get(null, EduQulificationIds, null);
        }

        public IList<DbModel.TechnicalSpecialistEducationalQualification> GetByPinId(IList<string> pins)
        {
            return Get(null, null, pins);
        }


        private IList<DbModel.TechnicalSpecialistEducationalQualification> Get(IQueryable<DbModel.TechnicalSpecialistEducationalQualification> tsQulificationAsQuerable = null,
                                                            IList<int> WorkhistoryIds = null,
                                                            IList<string> pins = null)

        {
            if (tsQulificationAsQuerable == null)
                tsQulificationAsQuerable = _dbContext.TechnicalSpecialistEducationalQualification;

            if (WorkhistoryIds?.Count > 0)
                tsQulificationAsQuerable = tsQulificationAsQuerable.Where(x => WorkhistoryIds.Contains(x.Id));


            if (pins?.Count > 0)
                tsQulificationAsQuerable = tsQulificationAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tsQulificationAsQuerable.Include(x => x.TechnicalSpecialist)
                                    .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistEducationalQualification> PopulateTsWorkHistoryAsQuerable(TechnicalSpecialistEducationalQualificationInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistEducationalQualification>(model);
            IQueryable<DbModel.TechnicalSpecialistEducationalQualification> tsEducationalQulificationAsQueryable = _dbContext.TechnicalSpecialistEducationalQualification;

            #region Wildcard Search for TS EPin
            if (model.Epin > 0)
                tsEducationalQulificationAsQueryable = tsEducationalQulificationAsQueryable.Where(x => x.TechnicalSpecialist.Pin == model.Epin);
            #endregion

            var defWhereExpr = dbSearchModel.ToExpression(new List<string> { nameof(dbSearchModel.ModifiedBy) });
            if (defWhereExpr != null)
                tsEducationalQulificationAsQueryable = tsEducationalQulificationAsQueryable.Where(defWhereExpr);

            return tsEducationalQulificationAsQueryable;
        }

    }
}
