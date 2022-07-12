using AutoMapper;
using Evolution.Admin.Domain.Interfaces.Admins;
using Evolution.Admin.Domain.Interfaces.Batch;
using Evolution.Admin.Domain.Models.Batch;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Documents;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Logging;
using Evolution.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Evolution.Batch
{
    public class BatchProcessor : IDisposable
    {
        private readonly IBatchProcessService _batchProcessService = null;
        private readonly IBatchService _batchService = null;
        public BatchProcessor(IBatchProcessService batchProcessService, IBatchService batchService)
        {
            _batchProcessService = batchProcessService;
            _batchService = batchService;
        }

        public void StartBatch()
        {
            _batchProcessService.StartBatch();
        }

        private void _batchProcessService_BatchProcessError(object sender, BatchProcessEventArgs e)
        {
            string message = string.Format("Error Occurred while Processing BatchID {0} DateTime :- {1}", e.BatchData.BatchID, DateTime.Now.ToString());
            _batchService.Update(e.BatchData);
            this.PrintMessage(message);
        }

        private void _batchProcessService_BatchProcessCompleted(object sender, BatchProcessEventArgs e)
        {
            string message = string.Empty;
            if (e.BatchData != null)
            {
                _batchService.Update(e.BatchData);
                message = string.Format("Processed BatchID {0} DateTime :- {1}", e.BatchData.BatchID, DateTime.Now.ToString());
            }
            else
                message = string.Format("No Batches to be Processed DateTime :- {0}", DateTime.Now.ToString());

            this.PrintMessage(message);
        }

        private void _batchProcessService_BatchProcessing(object sender, BatchProcessEventArgs e)
        {
            if (e.BatchData != null && e.BatchData.Id > 0)
            {
                e.BatchData.ProcessStatus = 1;
                _batchService.Update(e.BatchData);
                string message = string.Format("Processing BatchID {0} DateTime :- {1}", e.BatchData.BatchID, DateTime.Now.ToString());
                this.PrintMessage(message);
            }
        }

        private void PrintMessage(string message)
        {
            Console.WriteLine("");
            Console.WriteLine(string.Format("{0}  DateTime :- {1}", message, DateTime.Now.ToString()));
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}