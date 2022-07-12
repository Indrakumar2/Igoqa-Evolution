using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.EntityExtensions.Bulk
{
    public interface IBulkOperationHelper
    {
        void BulkInsertOperation<T>(List<T> aobjData, BulkConfig aobjBulkConfig) where T : class;

        void BulkInsertOrUpdateOperation<T>(List<T> aobjData, BulkConfig aobjBulkConfig) where T : class;
    }
}
