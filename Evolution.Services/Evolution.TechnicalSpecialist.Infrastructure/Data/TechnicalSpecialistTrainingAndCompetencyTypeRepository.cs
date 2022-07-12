using AutoMapper;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistTrainingAndCompetencyTypeRepository : GenericRepository<DbModel.TechnicalSpecialistTrainingAndCompetencyType>, ITechnicalSpecialistTrainingAndCompetencyTypeRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
      
        public TechnicalSpecialistTrainingAndCompetencyTypeRepository(EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> Search(TechnicalSpecialistInternalTrainingAndCompetencyType model)
        {
            var tsAsQueryable = this.PopulateInternalTrainingAndCompetencyTypeAsQuerable(model);
            return this.Get(tsAsQueryable,null,null);
        }

        public IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> Get(IList<int> typeIds)
        {
            return Get(null,typeIds,null,null);
        }
          
        public IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> Get(IList<string> typeNames)
        {
            return Get(typeNames : typeNames);
        }

        public IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> Get(IList<KeyValuePair<int, string>> trainingOrCompetencyIdAndTypeName)
        {
            var trainingOrCompetencyIds = trainingOrCompetencyIdAndTypeName?.Select(x => x.Key).ToList();
            var typeNames = trainingOrCompetencyIdAndTypeName?.Select(x => x.Value).ToList();

            return Get( typeNames: typeNames, trainingOrCompetencyIds: trainingOrCompetencyIds);
        }

        private IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType> Get(IQueryable<DbModel.TechnicalSpecialistTrainingAndCompetencyType> tsTrainingAndCompetencyTypeAsQuerable = null,
                                                            IList<int> typeIds = null, 
                                                            IList<string> typeNames = null,
                                                             IList<int> trainingOrCompetencyIds = null)
        {
            if (tsTrainingAndCompetencyTypeAsQuerable == null)
                tsTrainingAndCompetencyTypeAsQuerable = _dbContext.TechnicalSpecialistTrainingAndCompetencyType;

            if (typeIds?.Count > 0)
                tsTrainingAndCompetencyTypeAsQuerable = tsTrainingAndCompetencyTypeAsQuerable.Where(x => typeIds.Contains(x.Id));

            if (typeNames?.Count > 0)
                tsTrainingAndCompetencyTypeAsQuerable = tsTrainingAndCompetencyTypeAsQuerable.Where(x => typeNames.Contains(x.TrainingOrCompetencyData.Name));

            if (trainingOrCompetencyIds?.Count > 0)
                tsTrainingAndCompetencyTypeAsQuerable = tsTrainingAndCompetencyTypeAsQuerable.Where(x => trainingOrCompetencyIds.Contains(x.TechnicalSpecialistTrainingAndCompetencyId));

            return tsTrainingAndCompetencyTypeAsQuerable
                    .Include(x => x.TrainingOrCompetencyData)
                    .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistTrainingAndCompetencyType> PopulateInternalTrainingAndCompetencyTypeAsQuerable(TechnicalSpecialistInternalTrainingAndCompetencyType model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistTrainingAndCompetencyType>(model);
            IQueryable<DbModel.TechnicalSpecialistTrainingAndCompetencyType> tsPayScheduleAsQueryable = _dbContext.TechnicalSpecialistTrainingAndCompetencyType;
               
            return tsPayScheduleAsQueryable;
        }
    }
}
