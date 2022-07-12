using Evolution.Admin.Domain.Models.Admins;
using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext; 

namespace Evolution.Admin.Domain.Interfaces.Data
{
    public interface IBatchProcess:IGenericRepository<DbModels.BatchProcess>
    {
        int InsertBatch(Batches aobjBatches);
        int GetLastReportID();
        int UpdateBatch(Batches aobjBatches);
        Batches GetBatchByID(int aintID);
        List<Batches> GetPendingBatches();
        Batches CheckBatchExist(int aintBatchID, int aintParamID);
        Batches InsertBatchData(Batches aobjBatches);
        List<Batches> GetGeneratedReport(string astrUserName, int aintReportType);
        string DeleteReportFiles(List<int> fileIds);
        Batches GeBatchStatus(int aintBatchId);
    }
}
