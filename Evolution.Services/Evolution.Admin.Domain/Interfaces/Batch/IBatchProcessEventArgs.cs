using Evolution.Admin.Domain.Models.Admins;
using Evolution.Admin.Domain.Models.Batch;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Admin.Domain.Interfaces.Batch
{
    public interface IBatchProcessService
    {
        void StartBatch();

        void Processing(Batches aobjBatch);

        void Processed(Batches aobjBatch);

        void ErrorProcessing(Batches aobjBatch);
    }
}
