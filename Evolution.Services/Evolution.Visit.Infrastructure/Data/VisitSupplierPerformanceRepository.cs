using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Visit.Domain.Enums;
using Evolution.Visit.Domain.Interfaces.Data;
using Evolution.Visit.Domain.Models.Visits;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Visit.Domain.Models.Visits;
using AutoMapper;
using Evolution.Logging.Interfaces;
using Evolution.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Evolution.Visit.Infrastructure.Data
{
    public class VisitSupplierPerformanceRepository : GenericRepository<DbModel.VisitSupplierPerformance>, IVisitSupplierPerformanceRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<VisitSupplierPerformanceRepository> _logger = null;

        public VisitSupplierPerformanceRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper, IAppLogger<VisitSupplierPerformanceRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._logger = logger;
        }

        public IList<DomainModel.VisitSupplierPerformanceType> Search(DomainModel.VisitSupplierPerformanceType searchModel)
        {
            var dbSearchModel = this._mapper.Map<DbModel.VisitSupplierPerformance>(searchModel);
            IQueryable<DbModel.VisitSupplierPerformance> whereClause = null;

            whereClause = _dbContext.VisitSupplierPerformance.Where(x => x.VisitId == searchModel.VisitId);

            var expression = dbSearchModel.ToExpression();
            if (expression == null)
                return whereClause.ProjectTo<DomainModel.VisitSupplierPerformanceType>().ToList();
            else
                return whereClause.Where(expression).ProjectTo<DomainModel.VisitSupplierPerformanceType>().ToList();
        }

        public int DeleteVisitSupplierPerformanceType(List<DomainModel.VisitSupplierPerformanceType> visitReferences)
        {
            var supplierPerformanceTypeIds = visitReferences?.Where(x => x.VisitSupplierPerformanceTypeId != null && x.VisitSupplierPerformanceTypeId > 0)?.Select(x => Convert.ToInt32(x.VisitSupplierPerformanceTypeId))?.Distinct().ToList();
            return DeleteVisitSupplierPerformance(supplierPerformanceTypeIds);
        }

        public int DeleteVisitSupplierPerformance(List<int> supplierPerformanceTypeIds)
        {
            int count = 0;
            try
            {
                if (supplierPerformanceTypeIds.Count > 0)
                {
                    var deleteStatement = string.Format(Utility.GetSqlQuery(SQLModuleType.Visit_Reference, SQLModuleActionType.Delete), string.Join(",", supplierPerformanceTypeIds.ToString<int>()));
                    count = _dbContext.Database.ExecuteSqlCommand(deleteStatement);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), "VisitSupplierPerformanceTypeIds=" + supplierPerformanceTypeIds.ToString<int>());
            }

            return count;
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
