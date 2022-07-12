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
    public class TechnicalSpecialistCommodityEquipmentKnowledgeRepository : GenericRepository<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge>, ITechnicalSpecialistCommodityEquipmentKnowledgeRepository
    {

        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistCommodityEquipmentKnowledgeRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> Search(TechnicalSpecialistCommodityEquipmentKnowledgeInfo model)
        {
            var tsAsQueryable = this.PopulateTsCommodityEquipmentAsQuerable(model);
            return this.Get(tsAsQueryable, null, null, null, null);
        }

        public IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> Get(IList<int> Ids)
        {
            return Get(null, Ids, null, null, null);
        }

        public IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> GetByPinId(IList<string> pins)
        {
            return Get(null, null, pins, null, null);
        }

        public IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> Get(IList<string> Commodity)
        {
            return Get(null, null, null, Commodity, null);
        }
        public IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> GetByEquipmentKnowladge(IList<string> EquipmentKnowladge)
        {
            return Get(null, null, null, null, EquipmentKnowladge);
        }

        public IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> Get(IList<KeyValuePair<string, string>> ePinAndCommodity)
        {
            var pins = ePinAndCommodity?.Select(x => x.Key).ToList();
            var CommodityName = ePinAndCommodity?.Select(x => x.Value).ToList();

            return Get(null, null, pins, CommodityName);
        }

        private IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> Get(IQueryable<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> tsCommodityEqupipmentAsQuerable = null,
                                                            IList<int> Ids = null,
                                                            IList<string> pins = null,
                                                            IList<string> Commodity = null,
                                                             IList<string> Equipmentknowledge = null)
        {
            if (tsCommodityEqupipmentAsQuerable == null)
                tsCommodityEqupipmentAsQuerable = _dbContext.TechnicalSpecialistCommodityEquipmentKnowledge;

            if (Ids?.Count > 0)
                tsCommodityEqupipmentAsQuerable = tsCommodityEqupipmentAsQuerable.Where(x => Ids.Contains(x.Id));

            if (Commodity?.Count > 0)
                tsCommodityEqupipmentAsQuerable = tsCommodityEqupipmentAsQuerable.Where(x => Commodity.Contains(x.Commodity.Name));

            if (Equipmentknowledge?.Count > 0)
                tsCommodityEqupipmentAsQuerable = tsCommodityEqupipmentAsQuerable.Where(x => Equipmentknowledge.Contains(x.EquipmentKnowledge.Name));


            if (pins?.Count > 0)
                tsCommodityEqupipmentAsQuerable = tsCommodityEqupipmentAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tsCommodityEqupipmentAsQuerable.Include(x => x.TechnicalSpecialist)
                                    .Include(x => x.Commodity)
                                    .Include(x => x.EquipmentKnowledge)
                                    .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> PopulateTsCommodityEquipmentAsQuerable(TechnicalSpecialistCommodityEquipmentKnowledgeInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge>(model);
            IQueryable<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> tsCommodityEquipmentAsQueryable = _dbContext.TechnicalSpecialistCommodityEquipmentKnowledge;

            #region Wildcard Search for TS EPin
            if (model.Epin > 0)
                tsCommodityEquipmentAsQueryable = tsCommodityEquipmentAsQueryable.Where(x => x.TechnicalSpecialist.Pin == model.Epin);
            #endregion

            #region Wildcard Search for Commodity Name
            if (model.Commodity.HasEvoWildCardChar())
                tsCommodityEquipmentAsQueryable = tsCommodityEquipmentAsQueryable.WhereLike(x => x.Commodity.Name.ToString(), model.Commodity, '*');
            else if (!string.IsNullOrEmpty(model.Commodity))
                tsCommodityEquipmentAsQueryable = tsCommodityEquipmentAsQueryable.Where(x => x.Commodity.Name.ToString() == model.Commodity);
            #endregion
            #region Wildcard Search for EquipmentKnowledge Name
            if (model.EquipmentKnowledge.HasEvoWildCardChar())
                tsCommodityEquipmentAsQueryable = tsCommodityEquipmentAsQueryable.WhereLike(x => x.EquipmentKnowledge.Name.ToString(), model.EquipmentKnowledge, '*');
            else if (!string.IsNullOrEmpty(model.EquipmentKnowledge))
                tsCommodityEquipmentAsQueryable = tsCommodityEquipmentAsQueryable.Where(x => x.EquipmentKnowledge.Name.ToString() == model.EquipmentKnowledge);
            #endregion


            var defWhereExpr = dbSearchModel.ToExpression();
            if (defWhereExpr != null)
                tsCommodityEquipmentAsQueryable = tsCommodityEquipmentAsQueryable.Where(defWhereExpr);

            return tsCommodityEquipmentAsQueryable;
        }

    }
}
