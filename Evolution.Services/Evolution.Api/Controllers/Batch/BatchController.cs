using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evolution.Admin.Domain.Interfaces.Admins;
using Evolution.Admin.Domain.Interfaces.Data;
using Evolution.Admin.Domain.Models.Admins;
using Evolution.Api.Controllers.Base;
using Evolution.Common.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evolution.Api.Controllers.Batch
{
    [Route("api/Batch")]
    [ApiController]
    public class BatchController : BaseController
    {
        private readonly IBatchService _batchService;

        public BatchController(IBatchService batchService)
        {
            this._batchService = batchService;
        }

        [HttpPost]
        public Response Get([FromQuery]int aintBatchID, [FromQuery]int aintParamID)
        {
            Batches lobjBatches = new Batches
            {
                BatchID = aintBatchID,
                ParamID = aintParamID,
                ProcessStatus = 0,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = UserName
            };
            return _batchService.InsertBatch(lobjBatches);
        }

        [HttpGet]
        [Route("GeBatchStatus")]
        public Response GeBatchStatus([FromQuery] int aintBatchId)
        {
            return _batchService.GeBatchStatus(aintBatchId);
        }

        [HttpGet]
        [Route("GetBatch")]
        public Response CheckBatch(int aintBatchID, int aintParamID)
        {
            return _batchService.CheckBatch(aintBatchID, aintParamID);
        }

        [HttpGet]
        [Route("GetGeneratedReport")]
        public Response GetGeneratedReport(string astrUsername, int aintReportType)
        {
            return _batchService.GetGeneratedReport(astrUsername, aintReportType);
        }

        [HttpPost]
        [Route("DeleteReportFiles")]
        public Response DeleteReportFiles([FromBody]List<int> fileIds)
        {
            return _batchService.DeleteReportFiles(fileIds);
        }
    }
}
