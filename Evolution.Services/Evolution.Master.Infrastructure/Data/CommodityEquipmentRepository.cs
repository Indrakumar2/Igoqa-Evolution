using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Master.Domain.Interfaces.Data;
using DomainModel =Evolution.Master.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Infrastructure.Data
{
    public class CommodityEquipmentRepository : GenericRepository<DbModel.CommodityEquipment>, ICommodityEquipmentRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public CommodityEquipmentRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.CommodityEquipment> Search(DomainModel.CommodityEquipment searchModel)
        {
            var dbSearchModel = _mapper.Map<DbModel.CommodityEquipment>(searchModel);
            var expression = dbSearchModel.ToExpression();
            IQueryable <DbModel.CommodityEquipment> whereClause = _dbContext.CommodityEquipment;

            //Wildcard Search for Commodity
            if (searchModel.Commodity.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Commodity.Name, searchModel.Commodity, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.Commodity) || x.Commodity.Name == searchModel.Commodity);

            //Wildcard Search for Equipment
            if (searchModel.Equipment.HasEvoWildCardChar())
                whereClause = whereClause.WhereLike(x => x.Equipment.Name, searchModel.Equipment, '*');
            else
                whereClause = whereClause.Where(x => string.IsNullOrEmpty(searchModel.Equipment) || x.Equipment.Name == searchModel.Equipment);

            if (expression == null)
                return whereClause.ProjectTo<DomainModel.CommodityEquipment>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.CommodityEquipment>().ToList();
        }
    }
}