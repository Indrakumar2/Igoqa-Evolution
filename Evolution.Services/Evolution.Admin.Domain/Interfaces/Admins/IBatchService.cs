using DomModel = Evolution.Admin.Domain.Models.Admins;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Admin.Domain.Models.Admins;
using System.Linq.Expressions;
using System;

namespace Evolution.Admin.Domain.Interfaces.Admins
{
    public interface IBatchService
    {
        Response GetBatch(int aintID);
        int GetLastReportID();
        Response Insert(Batches aobjBatches);
        Response Update(Batches aobjBatches);
        Response CheckBatch(int aintBatchID, int aintParamID);
        Response InsertBatch(Batches aobjBatches);
        Response GetGeneratedReport(string astrUserName, int aintReportType);
        Response DeleteReportFiles(List<int> fileIds);
        Response GeBatchStatus(int aintBatchId);
        void Update(BatchProcess objDbBatchPrcInfo, params Expression<Func<BatchProcess, object>>[] updatedProperties);
        
        Response GenerateResourceCVs(IList<KeyValuePair<string, byte[]>> resourceCVFileContent, Batches batches);
    }
}
