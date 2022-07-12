using EFCore.BulkExtensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.EntityExtensions.Bulk
{
    public class BulkOperationHelper: IBulkOperationHelper
    {
        private readonly EvolutionSqlDbContext _dbContext = null;
        public BulkOperationHelper(EvolutionSqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void BulkInsertOperation<T>(List<T> aobjData, BulkConfig aobjBulkConfig) where T : class
        {
            _dbContext.BulkInsert(aobjData, aobjBulkConfig);
        }

        public void BulkInsertOrUpdateOperation<T>(List<T> aobjData, BulkConfig aobjBulkConfig) where T : class
        {
            _dbContext.BulkInsertOrUpdate(aobjData, aobjBulkConfig);
        }
    }
}
