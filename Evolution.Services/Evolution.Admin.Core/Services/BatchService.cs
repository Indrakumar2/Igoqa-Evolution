using Evolution.Admin.Domain.Interfaces.Admins;
using DomainModels = Evolution.Admin.Domain.Models.Admins;
using DbModels = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using Evolution.Admin.Domain.Interfaces.Data;
using Evolution.Logging.Interfaces;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Admin.Domain.Models.Admins;
using AutoMapper;
using System.Linq.Expressions;
using Evolution.Common.Models.Messages;
using System.IO;
using System.IO.Compression;
using Evolution.Document.Domain.Interfaces.Documents;
using Newtonsoft.Json.Linq;

namespace Evolution.Admin.Core.Services
{
    public class BatchService : IBatchService
    {
        private readonly IBatchProcess _repsoitory = null;
        private readonly IAppLogger<BatchService> _logger = null;
        private readonly IMapper _mapper = null;
        private readonly IDocumentService _documentService = null;
        private readonly JObject _messageDescriptions = null;
        public BatchService(IBatchProcess repository, 
            IAppLogger<BatchService> logger, 
            IMapper mapper, 
            IDocumentService documentService,
            JObject message)
        {
            _repsoitory = repository;
            _logger = logger;
            _mapper= mapper;
            _documentService = documentService;
            _messageDescriptions = message;
        }

        public Response GetBatch(int aintID)
        {
            Exception exception = null;
            Batches result = null;
            try
            {
                result = _repsoitory.GetBatchByID(aintID);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), aintID);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, 0);
        }
        public int GetLastReportID()
        {
            int result = _repsoitory.GetLastReportID();
            return result;
        }


        public int Insert(Batches aobjBatches)
        {
            Exception exception = null;
            int result = 0;
            try
            {
                result = _repsoitory.InsertBatch(aobjBatches);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), aobjBatches);
            }
            return result;
        }

        public Response InsertBatch(Batches aobjBatches)
        {
            Exception exception = null;
            Batches lobjBatches = null;
            try
            {
                lobjBatches = _repsoitory.InsertBatchData(aobjBatches);
                // Add your response message here the above method will return batch value which is inserted or it will return the existing batch record.
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), aobjBatches);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, lobjBatches, exception, 0);
        }

        public Response Update(Batches aobjBatches)
        {
            Exception exception = null;
            int result = 0;
            try
            {
                result = _repsoitory.UpdateBatch(aobjBatches);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), aobjBatches);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, 0);
        }

        public void Update(DbModels.BatchProcess objDbBatchPrcInfo, params Expression<Func<DbModels.BatchProcess, object>>[] updatedProperties)
        { 
            try
            { 
                _repsoitory.Update(objDbBatchPrcInfo, updatedProperties);
            }
            catch (Exception ex)
            { 
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), objDbBatchPrcInfo);
            } 
        }

        public Response CheckBatch(int aintBatchID, int aintParamID)
        {
            Exception exception = null;
            Batches lobjData = null;
            try
            {
                lobjData = _repsoitory.CheckBatchExist(aintBatchID, aintParamID);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), aintParamID);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, lobjData, exception, 0);
        }

        public Response GetGeneratedReport(string astrUserName, int aintReportType)
        {
            Exception exception = null;
            List<Batches> result = null;
            try
            {
                result = _repsoitory.GetGeneratedReport(astrUserName, aintReportType);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), astrUserName);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response DeleteReportFiles(List<int> fileIds)
        {
            Exception exception = null;
            string message = string.Empty;
            try
            {
                message = _repsoitory.DeleteReportFiles(fileIds);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), String.Join(',', fileIds));
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, message, exception, 1);
        }

        public Response GeBatchStatus(int aintBatchId)
        {
            Exception exception = null;
            Batches lobjData = null;
            try
            {
                lobjData = _repsoitory.GeBatchStatus(aintBatchId);
            }
            catch (Exception ex)
            {
                exception = ex;
                lobjData = new Batches()
                {
                    ProcessStatus = 1,
                    BatchID = 1,
                    ErrorMessage = "Failed due to server not responding, please contact support"
                };
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), aintBatchId);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, lobjData, exception, 0);
        }

        public Response GenerateResourceCVs(IList<KeyValuePair<string, byte[]>> resourceCVFileContent, Batches batches)
        {
            byte[] zipFileBytes;
            Exception exception = null;
            DbModels.BatchProcess objDbBatchnfo = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            List<Expression<Func<DbModels.BatchProcess, object>>> updatedProperties = new List<Expression<Func<DbModels.BatchProcess, object>>>();
            try
            {
                if (resourceCVFileContent?.Count > 0 && batches != null)
                {
                    using (var compressedFileStream = new MemoryStream())
                    {
                        //Create an archive and store the stream in memory.
                        using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, false))
                        {
                            foreach (var attachmentData in resourceCVFileContent)
                            {
                                //Create a zip entry for each attachment
                                var zipEntry = zipArchive.CreateEntry(attachmentData.Key);

                                //Get the stream of the attachment
                                using (var originalFileStream = new MemoryStream(attachmentData.Value))
                                {
                                    using (var zipEntryStream = zipEntry.Open())
                                    {
                                        //Copy the attachment stream to the zip entry stream
                                        originalFileStream.CopyTo(zipEntryStream);
                                    }
                                }
                            }
                        }
                        zipFileBytes = compressedFileStream.ToArray();
                    }

                    string upldFolderPath = _documentService.CheckAndUpdateDocumentUploadPath();
                    objDbBatchnfo = _mapper.Map<DbModels.BatchProcess>(batches);
                    if (string.IsNullOrEmpty(upldFolderPath)) // To handle all isFull=true folder path full scenario and no access exception
                    {
                        string errorCode = MessageType.AllDocumentUploadFilepathFull.ToId();
                        errorMessages.Add(new MessageDetail(ModuleType.Document, errorCode, _messageDescriptions[errorCode].ToString()));
                        _logger.LogError(ResponseType.Warning.ToId(), _messageDescriptions[errorCode].ToString(), null);

                        objDbBatchnfo.ProcessStatus = 3;//Failed
                        objDbBatchnfo.ErrorMessage = _messageDescriptions[errorCode].ToString();
                        updatedProperties.Add(a => a.ProcessStatus);
                        updatedProperties.Add(a => a.ErrorMessage);
                    }
                    else
                    {
                        objDbBatchnfo.ReportFilePath = upldFolderPath;
                        objDbBatchnfo.ReportFileName = string.Format("{0}-{1}-{2}-{3}-{4}.{5}", "Technical_specialist_cv", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, Guid.NewGuid(), "zip");

                        using (var fileStream = new FileStream(Path.Combine(upldFolderPath, objDbBatchnfo.ReportFileName), FileMode.OpenOrCreate))
                        {
                            fileStream.Write(zipFileBytes, 0, zipFileBytes.Length);
                        }

                        objDbBatchnfo.ProcessStatus = 2;//Completed 
                        updatedProperties.Add(a => a.ProcessStatus);
                        updatedProperties.Add(a => a.ReportFilePath);
                        updatedProperties.Add(a => a.ReportFileName);
                    }
                    Update(objDbBatchnfo, updatedProperties.ToArray());
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());

                objDbBatchnfo = _mapper.Map<DbModels.BatchProcess>(batches);

                objDbBatchnfo.ProcessStatus = 3;//Failed
                objDbBatchnfo.ErrorMessage = ex.Message;
                //objDbBatchnfo.FailCount = batches.FailCount + 1;

                updatedProperties.Add(a => a.ProcessStatus);
                updatedProperties.Add(a => a.ErrorMessage);
                // updatedProperties.Add(a => a.FailCount);

                Update(objDbBatchnfo, updatedProperties.ToArray());
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }



Response IBatchService.Insert(Batches aobjBatches)
        {
            throw new NotImplementedException();
        }
    }
}