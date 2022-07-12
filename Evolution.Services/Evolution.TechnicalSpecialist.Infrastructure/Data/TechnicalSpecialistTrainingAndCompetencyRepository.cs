using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions; 
using Evolution.Document.Domain.Models.Document;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistTrainingAndCompetencyRepository : GenericRepository<DbModel.TechnicalSpecialistTrainingAndCompetency>, ITechnicalSpecialistTrainingAndCompetencyRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistTrainingAndCompetencyRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }


        #region Get 

        public IList<DbModel.TechnicalSpecialistTrainingAndCompetency> Get(IList<int> Ids)
        {
            return Get(trainingAndCompetencyIds: Ids);
        }

        public IList<DbModel.TechnicalSpecialistTrainingAndCompetency> Get(IList<string> trainingOrCompetencyNames, CompCertTrainingType trainingAndCompetencyType)
        {
            return Get(trainingAndCompetencyType, null, null, null, trainingOrCompetencyNames);
        }

        public IList<DbModel.TechnicalSpecialistTrainingAndCompetency> Get(IList<KeyValuePair<string, string>> ePinAndTrainingOrCompetencyNames, CompCertTrainingType trainingAndCompetencyType)
        {
            var pins = ePinAndTrainingOrCompetencyNames?.Select(x => x.Key).ToList();
            var trainingOrCompetencyNames = ePinAndTrainingOrCompetencyNames?.Select(x => x.Value).ToList();

            return Get(trainingAndCompetencyType, null, null, pins, trainingOrCompetencyNames);
        }

        public IList<DbModel.TechnicalSpecialistTrainingAndCompetency> GetByPinId(IList<string> pinIds)
        {
            return Get(pins: pinIds);
        }

        #endregion

        #region Search

        public IList<DbModel.TechnicalSpecialistTrainingAndCompetency> Search(TechnicalSpecialistCompetency searchModel)
        {
            var tsCertificateAsQueryable = this.PopulateTsTrainingAndCompetencyAsQuerable(competencySearchModel : searchModel);
            return this.Get(CompCertTrainingType.Co, tsCertificateAsQueryable, null, null, null);
        }

        public IList<DbModel.TechnicalSpecialistTrainingAndCompetency> Search(TechnicalSpecialistInternalTraining searchModel)
        {
            var tsTrainingAsQueryable = this.PopulateTsTrainingAndCompetencyAsQuerable(internalTrainingSearchModel: searchModel);
            return this.Get(CompCertTrainingType.IT, tsTrainingAsQueryable, null, null, null);
        }

        #endregion

        #region Private Methods

        private IList<DbModel.TechnicalSpecialistTrainingAndCompetency> Get(CompCertTrainingType trainingAndCompetencyType, IQueryable<DbModel.TechnicalSpecialistTrainingAndCompetency> tsTrainingAndCompetencyAsQuerable = null,
                                                            IList<int> trainingAndCompetencyIds = null,
                                                            IList<string> pins = null,
                                                            IList<string> trainingAndCompetencyName = null)
        {
            if (tsTrainingAndCompetencyAsQuerable != null)
                tsTrainingAndCompetencyAsQuerable = tsTrainingAndCompetencyAsQuerable.Where(x => x.RecordType == trainingAndCompetencyType.ToString());

            return Get(tsTrainingAndCompetencyAsQuerable, trainingAndCompetencyIds, pins, trainingAndCompetencyName);
        }

        private IList<DbModel.TechnicalSpecialistTrainingAndCompetency> Get(IQueryable<DbModel.TechnicalSpecialistTrainingAndCompetency> tsTrainingAndCompetencyAsQuerable = null,
                                                    IList<int> trainingAndCompetencyIds = null,
                                                    IList<string> pins = null,
                                                    IList<string> trainingAndCompetencyName = null)
        {
            if (tsTrainingAndCompetencyAsQuerable == null)
                tsTrainingAndCompetencyAsQuerable = _dbContext.TechnicalSpecialistTrainingAndCompetency;

            if (trainingAndCompetencyIds?.Count > 0)
                tsTrainingAndCompetencyAsQuerable = tsTrainingAndCompetencyAsQuerable.Where(x => trainingAndCompetencyIds.Contains(x.Id));

            if (trainingAndCompetencyName?.Count > 0)
                tsTrainingAndCompetencyAsQuerable = tsTrainingAndCompetencyAsQuerable.Where(x => x.TechnicalSpecialistTrainingAndCompetencyType.Any(x1=> trainingAndCompetencyName.Contains(x1.TrainingOrCompetencyData.Name)));

            if (pins?.Count > 0)
                tsTrainingAndCompetencyAsQuerable = tsTrainingAndCompetencyAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tsTrainingAndCompetencyAsQuerable.Include(x => x.TechnicalSpecialist)
                                                .Include(x => x.TechnicalSpecialistTrainingAndCompetencyType) 
                                                .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistTrainingAndCompetency> PopulateTsTrainingAndCompetencyAsQuerable(TechnicalSpecialistCompetency competencySearchModel = null, TechnicalSpecialistInternalTraining internalTrainingSearchModel = null)
        {
            DbModel.TechnicalSpecialistTrainingAndCompetency dbSearchModel = null;

            IQueryable<DbModel.TechnicalSpecialistTrainingAndCompetency> tsTrainingAndCompetencyAsQueryable = _dbContext.TechnicalSpecialistTrainingAndCompetency;

            if (competencySearchModel != null)
            {
                dbSearchModel = _mapper.Map<DbModel.TechnicalSpecialistTrainingAndCompetency>(competencySearchModel);

                if (competencySearchModel.Epin > 0 )
                {
                    tsTrainingAndCompetencyAsQueryable = tsTrainingAndCompetencyAsQueryable.Where(x => x.TechnicalSpecialist.Pin == competencySearchModel.Epin);
                }
            }
            if (internalTrainingSearchModel != null)
            {
                dbSearchModel = _mapper.Map<DbModel.TechnicalSpecialistTrainingAndCompetency>(internalTrainingSearchModel);

                if (internalTrainingSearchModel!=null && internalTrainingSearchModel.Epin > 0)
                {
                    tsTrainingAndCompetencyAsQueryable = tsTrainingAndCompetencyAsQueryable.Where(x => x.TechnicalSpecialist.Pin == internalTrainingSearchModel.Epin);
                }

            }
             
            var defWhereExpr = dbSearchModel.ToExpression();
            if (defWhereExpr != null)
                tsTrainingAndCompetencyAsQueryable = tsTrainingAndCompetencyAsQueryable.Where(defWhereExpr);

            return tsTrainingAndCompetencyAsQueryable;
        }

        #endregion

    }
}
