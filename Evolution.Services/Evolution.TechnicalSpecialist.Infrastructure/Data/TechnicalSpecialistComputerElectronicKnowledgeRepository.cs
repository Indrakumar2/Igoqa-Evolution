using AutoMapper;
using Evolution.Common.Extensions;
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
    public class TechnicalSpecialistComputerElectronicKnowledgeRepository : GenericRepository<DbModel.TechnicalSpecialistComputerElectronicKnowledge>, ITechnicalSpecialistComputerElectronicKnowledgeServiceRepository
    {

        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistComputerElectronicKnowledgeRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> Get(IList<int> Ids)
        {
            return Get(null, Ids, null, null);
        }

        public IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> Get(IList<string> ComputerKnowledge)
        {
            return Get(null, null, null, ComputerKnowledge);
        }

        public IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> Get(IList<KeyValuePair<string, string>> ePinAndComputerKnowledge)
        {
            var pins = ePinAndComputerKnowledge?.Select(x => x.Key).ToList();
            var ComputerElectronicKnowledge = ePinAndComputerKnowledge?.Select(x => x.Value).ToList();

            return Get(null, null, pins, ComputerElectronicKnowledge);
        }

        public IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> GetByPinId(IList<string> pinIds)
        {
            return Get(null, null, pinIds, null);
        }

        public IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> Search(TechnicalSpecialistComputerElectronicKnowledgeInfo model)
        {
            var tsAsQueryable = this.PopulateTsComputerElectronicKnowledgeAsQuerable(model);
            return this.Get(tsAsQueryable, null, null, null);
        }
        private IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> Get(IQueryable<DbModel.TechnicalSpecialistComputerElectronicKnowledge> tsComputerElectronicKnowledgeAsQuerable = null,
                                                          IList<int> Ids = null,
                                                          IList<string> pins = null,
                                                          IList<string> ComputerKnowledge = null)
        {
            if (tsComputerElectronicKnowledgeAsQuerable == null)
                tsComputerElectronicKnowledgeAsQuerable = _dbContext.TechnicalSpecialistComputerElectronicKnowledge;

            if ( Ids?.Count > 0)
                tsComputerElectronicKnowledgeAsQuerable = tsComputerElectronicKnowledgeAsQuerable.Where(x => Ids.Contains(x.Id));

            if (ComputerKnowledge?.Count > 0)
                tsComputerElectronicKnowledgeAsQuerable = tsComputerElectronicKnowledgeAsQuerable.Where(x => ComputerKnowledge.Contains(x.ComputerKnowledge.Name));

            if (pins?.Count > 0)
                tsComputerElectronicKnowledgeAsQuerable = tsComputerElectronicKnowledgeAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tsComputerElectronicKnowledgeAsQuerable.Include(x => x.TechnicalSpecialist)
                                    .Include(x =>x.ComputerKnowledge)
                                    .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistComputerElectronicKnowledge> PopulateTsComputerElectronicKnowledgeAsQuerable(TechnicalSpecialistComputerElectronicKnowledgeInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistComputerElectronicKnowledge>(model);
            IQueryable<DbModel.TechnicalSpecialistComputerElectronicKnowledge> tsComputerElectronicKnowledgeAsQueryable = _dbContext.TechnicalSpecialistComputerElectronicKnowledge;

            #region Wildcard Search for TS EPin
            if (model.Epin > 0)
                tsComputerElectronicKnowledgeAsQueryable = tsComputerElectronicKnowledgeAsQueryable.Where(x => x.TechnicalSpecialist.Pin == model.Epin);
            #endregion

            #region Wildcard Search for ComputerKnowledge 
            if (model.ComputerKnowledge.HasEvoWildCardChar())
                tsComputerElectronicKnowledgeAsQueryable = tsComputerElectronicKnowledgeAsQueryable.WhereLike(x => x.ComputerKnowledge.Name.ToString(), model.ComputerKnowledge, '*');
            else if (!string.IsNullOrEmpty(model.ComputerKnowledge))
                tsComputerElectronicKnowledgeAsQueryable = tsComputerElectronicKnowledgeAsQueryable.Where(x => x.ComputerKnowledge.Name.ToString() == model.ComputerKnowledge);
            #endregion
            var defWhereExpr = dbSearchModel.ToExpression();
            if (defWhereExpr != null)
                tsComputerElectronicKnowledgeAsQueryable = tsComputerElectronicKnowledgeAsQueryable.Where(defWhereExpr);

            return tsComputerElectronicKnowledgeAsQueryable;
        }

       
    }
}

