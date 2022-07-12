using Evolution.Admin.Domain.Interfaces.Batch;
using Evolution.Admin.Domain.Interfaces.Data;
using Evolution.Admin.Domain.Models.Admins;
using Evolution.Admin.Domain.Models.Batch;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Evolution.Admin.Core.Services
{
    public class BatchProcessService : IBatchProcessService
    {
        private readonly IBatchProcess _batchRepository = null;
        public static bool isProcessing = false;
        private readonly IContractScheduleService _contractScheduleService;

        public BatchProcessService(IBatchProcess batchRepository, IContractScheduleService contractScheduleService)
        {
            _contractScheduleService = contractScheduleService;
            _batchRepository = batchRepository;
        }

        private List<Batches> GetPendingBatches()
        {
            return _batchRepository.GetPendingBatches();
        }

        public void StartBatch()
        {
            List<Batches> batches = this.GetPendingBatches();
            if (batches != null && batches.Count > 0)
            {
                isProcessing = true;
                foreach (var item in batches)
                    this.Start(item);
            }
            else
                isProcessing = false;
        }

        public void Processing(Batches aobjBatch)
        {
            _batchRepository.UpdateBatch(aobjBatch);
            string message = string.Format("Processing Batch {0}", Convert.ToString(aobjBatch.ParamID));
            Console.WriteLine(message);
        }

        public void Processed(Batches aobjBatch)
        {
            _batchRepository.UpdateBatch(aobjBatch);
            string message = string.Format("Batch process completed {0}", Convert.ToString(aobjBatch.ParamID));
            Console.WriteLine(message);
        }

        public void ErrorProcessing(Batches aobjBatch)
        {
            _batchRepository.UpdateBatch(aobjBatch);
            string message = string.Format("Error occurred while processing Batch {0}", Convert.ToString(aobjBatch.ParamID));
            Console.WriteLine(message);
        }

        private void Start(Batches aobjBatch)
        {
            try
            {
                if (aobjBatch.BatchID == 1)
                {
                    if (aobjBatch.FailCount == 0)
                    {
                        aobjBatch.ProcessStatus = 1;
                        this.Processing(aobjBatch);
                    }
                    string lstrMessage = _contractScheduleService.RelatedFrameworkContractBatch(aobjBatch.ParamID, aobjBatch.CreatedBy);
                    if (lstrMessage.ToLower().Contains("completed"))
                        aobjBatch.ProcessStatus = 2;
                    else
                    {
                        aobjBatch.ProcessStatus = 3;
                        aobjBatch.ErrorMessage = lstrMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                aobjBatch.ProcessStatus = 3;
                if (!string.IsNullOrEmpty(ex.Message)
                    && (ex.Message.ToLower().Contains("timed out")
                    || (ex.Message.ToLower().Contains("network"))))
                    aobjBatch.ErrorMessage = "Copy has failed due to server not responding, please contact support";
                else
                    aobjBatch.ErrorMessage = "Failed due to Data mismatch Issue, Please contact Support";
            }
            finally
            {
                if (aobjBatch.ProcessStatus == 3)
                    this.ErrorProcessing(aobjBatch);
                else if (aobjBatch.ProcessStatus == 2)
                    this.Processed(aobjBatch);
            }
        }
    }
}