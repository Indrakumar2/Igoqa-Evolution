using AutoMapper;
using Evolution.Admin.Domain.Interfaces.Data;
using Evolution.Admin.Domain.Models.Admins;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Admin.Infrastructure.Data
{
    public class BatchRepository : GenericRepository<BatchProcess>, IBatchProcess
    {
        private readonly EvolutionSqlDbContext _dbContext = null;

        private readonly IMapper _mapper = null;

        public BatchRepository(IMapper mapper, EvolutionSqlDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }


        public int GetLastReportID()
        {
            var maxId = _dbContext.BatchProcess.Max(item => item.Id);
            return maxId;
        }
        public Batches GetBatchByID(int aintID)
        {
            BatchProcess lobjBatchProcess = _dbContext.BatchProcess?.FirstOrDefault(a => a.Id == aintID);
            if (lobjBatchProcess != null)
                return _mapper.Map<Batches>(lobjBatchProcess);
            return null;
        }

        public List<Batches> GetPendingBatches()
        {
            List<BatchProcess> llstBatchProcess = _dbContext.BatchProcess?.Where(a => a.ProcessStatus == 0 && a.BatchId == 1)?.ToList();
            if (llstBatchProcess != null && llstBatchProcess.Count > 0)
                return _mapper.Map<List<Batches>>(llstBatchProcess);
            return null;
        }

        public int InsertBatch(Batches aobjBatches)
        {
            BatchProcess lobjBatchProcess = _mapper.Map<BatchProcess>(aobjBatches);
            _dbContext.BatchProcess.Add(lobjBatchProcess);
             _dbContext.SaveChanges(); 
             return lobjBatchProcess.Id; 
        }
    
        /// <summary>
        /// Gets the Batch Details based on the Id
        /// </summary>
        /// <param name="aintBatchId"></param>
        /// <returns></returns>
        public Batches GeBatchStatus(int aintBatchId)
        {
            BatchProcess batchProcess = _dbContext.BatchProcess.FirstOrDefault(item => item.Id == aintBatchId);
            return _mapper.Map<Batches>(batchProcess);
        }

        
        public Batches InsertBatchData(Batches aobjBatches)
        {
            //BatchProcess batchProcess = _dbContext.BatchProcess.Where(a => a.BatchId == aobjBatches.BatchID && a.ParamId == aobjBatches.ParamID && (a.ProcessStatus == 0 || a.ProcessStatus == 1)).FirstOrDefault();
           // if (batchProcess == null)
           // {
                BatchProcess lobjBatchProcess = _mapper.Map<BatchProcess>(aobjBatches);
                _dbContext.BatchProcess.Add(lobjBatchProcess);
                _dbContext.SaveChanges();
                return _mapper.Map<Batches>(lobjBatchProcess);
            //}
         //   return _mapper.Map<Batches>(batchProcess);
        }

        public int UpdateBatch(Batches aobjBatches)
        {
            BatchProcess lobjBatchProcess = _dbContext.BatchProcess?.FirstOrDefault(a => a.Id == aobjBatches.Id);
            if (lobjBatchProcess != null)
            {
                lobjBatchProcess.ProcessStatus = aobjBatches.ProcessStatus;
                lobjBatchProcess.UpdatedDate = DateTime.UtcNow;
                lobjBatchProcess.ErrorMessage = !string.IsNullOrEmpty(aobjBatches.ErrorMessage) ? aobjBatches.ErrorMessage : null;
                int saved = _dbContext.SaveChanges();
                return saved;
            }
            return -1;
        }

        public Batches CheckBatchExist(int aintBatchID, int aintParamID)
        {
            var lobjData = _dbContext.BatchProcess.Where(a => a.BatchId == aintBatchID && a.ParamId == aintParamID)?.OrderByDescending(a => a.Id)?.FirstOrDefault();
            return lobjData != null ? _mapper.Map<Batches>(lobjData) : null;
        }

        public List<Batches> GetGeneratedReport(string astrUserName, int aintReportType)
        {
            int? lintUserId = _dbContext.User.FirstOrDefault(a => a.SamaccountName == astrUserName)?.Id;
          if (lintUserId.HasValue)
           {
                if(aintReportType == 5)
                {
                    var GeneratedFile = _dbContext.BatchProcess.Where(a => a.ParamId == lintUserId && a.ReportType == 5 && a.IsDeleted == false)?.OrderByDescending(a => a.CreatedDate)?.ToList();
                    var GeneratedFile1 = _dbContext.BatchProcess.Where(a => a.ParamId == lintUserId && a.ReportType == 6 && a.IsDeleted == false)?.OrderByDescending(a => a.CreatedDate)?.ToList();
                    var GeneratedFile2 = _dbContext.BatchProcess.Where(a => a.ParamId == lintUserId && a.ReportType == 7 && a.IsDeleted == false)?.OrderByDescending(a => a.CreatedDate)?.ToList();
                    GeneratedFile2.AddRange(GeneratedFile);
                    GeneratedFile2.AddRange(GeneratedFile1);
                    var descendingOrder = GeneratedFile2.OrderByDescending(i => i.CreatedDate).ToList();

                    return descendingOrder != null ? _mapper.Map<List<Batches>>(descendingOrder) : null;
                }
                else
                {
                    var GeneratedFile = _dbContext.BatchProcess.Where(a => a.ParamId == lintUserId && a.ReportType == aintReportType && a.IsDeleted == false)?.OrderByDescending(a => a.CreatedDate)?.ToList();
                    return GeneratedFile != null ? _mapper.Map<List<Batches>>(GeneratedFile) : null;
                }
               
                
            }
            return null;
        }

        public string DeleteReportFiles(List<int> fileIds)
        {
            try
            {
                var FilesToBeDeleted = _dbContext.BatchProcess.Where(a => fileIds.Contains(a.Id)).ToList();
                FilesToBeDeleted.ForEach(item =>
                {
                    item.UpdatedDate = DateTime.UtcNow;
                    item.IsDeleted = true;
                });
                _dbContext.SaveChanges();
                return "Deleted Successfully";
            }
            catch (Exception ex)
            {
                return "Internal error, Please try again";
            }
        }
    }
}