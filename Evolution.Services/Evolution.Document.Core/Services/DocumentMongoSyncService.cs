using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Documents;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Document.Domain.Interfaces.Data;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models;
using Evolution.Document.Domain.Models.Document;
using Evolution.FileExtractor.Interfaces;
using Evolution.Logging.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace Evolution.Document.Core.Services
{
    public class DocumentMongoSyncService : IDocumentMongoSyncService
    {
        private readonly IDocumentRepository _documentRepository = null;
        private readonly IDocumentMongoSyncRepository _syncRepository = null;
        private readonly IMongoDocumentService _mongoDocumentService = null;
        private readonly IDocumentService _documentService = null;
        //private readonly IEnumerable<IFileExtractorService> _fileExtractorService = null;
        private readonly IMongoDocumentExtractor _fileExtractorService = null;
        private readonly IAppLogger<DocumentMongoSyncService> _logger = null;
        private readonly JObject _messages = null;
        private readonly MongoSetting _mongoSetting = null;
        private readonly DocumentUploadSetting _documentUploadSetting = null;


        public DocumentMongoSyncService(IDocumentRepository documentRepository,
                                        IDocumentMongoSyncRepository documentMongoSyncRepository,
                                        IDocumentService documentService,
                                        IMongoDocumentService mongoDocumentService,
                                        IMongoDocumentExtractor fileExtractorService,
                                        IOptions<MongoSetting> mongoSetting,
                                        IOptions<DocumentUploadSetting> documentUploadSetting,
                                        IAppLogger<DocumentMongoSyncService> logger,
                                        JObject messages)
        {
            _documentRepository = documentRepository;
            _documentService = documentService;
            _syncRepository = documentMongoSyncRepository;
            _mongoDocumentService = mongoDocumentService;
            _fileExtractorService = fileExtractorService;
            _mongoSetting = mongoSetting.Value;
            _logger = logger;
            _messages = messages;
            _documentUploadSetting = documentUploadSetting.Value;
        }

        public ModuleDocument FetchTop1DocumentToBeProcessed(bool isFetchNewDoc)
        {
            try
            {
                return _documentRepository.GetDocumentsToBeProcessed(isFetchNewDoc)?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            return null;
        }

        public ModuleDocument FetchTop1DocumentToBeProcessed(string documentTitle, string modifiedBy)
        {
            try
            {
                return _documentRepository.GetDocumentsToBeProcessed(documentTitle, modifiedBy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            return null;
        }

        public void ProcessDocument(ModuleDocument document, string fileText)
        {
            try
            {
                PushTextToMongo(document, fileText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
        }

        public void ProcessDocumentMigratedDocuments(ModuleDocument document, string fileText)
        {
            try
            {
                PushTextToMongoMigratedDocuments(document, fileText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }

        }

        public bool IsDocumentValidForSync(string documentUniqueName, out string message)
        {
            message = string.Empty;
            try
            {
                if (!IsValidForSync(documentUniqueName))
                {
                    message = string.Format("Only [{0}] file types are allowed", _mongoSetting.DocumentTypes);
                    return false;
                }

                bool searchResult = _mongoDocumentService.Any(documentUniqueName);

                if (searchResult)
                {
                    message = string.Format("Document with DocumentUniqueName [{0}] already synched to mongo DB. ", documentUniqueName);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
                message = string.Format("Exception : {0}", ex.Message);
                return false;
            }
            return true;
        }

        public void SaveSyncDocumentAuditData(string documentUniqueName, MongoSyncStatusType status, bool isCompleted = true, string reason = null)
        {
            try
            {
                var existingSyncDocument = _syncRepository.FindBy(x => x.DocumentUniqueName == documentUniqueName)?.FirstOrDefault();
                if (existingSyncDocument != null)
                {
                    existingSyncDocument.IsCompleted = isCompleted;
                    existingSyncDocument.Reason = reason;
                    existingSyncDocument.Status = status.ToString();
                    if (isCompleted == false)
                    {
                        existingSyncDocument.FailCount = existingSyncDocument.FailCount.CalculateUpdateCount();
                    }
                    _syncRepository.Update(existingSyncDocument);
                }
                else
                {
                    _syncRepository.Add(new DocumentMongoSync()
                    {
                        Id = 0,
                        DocumentUniqueName = documentUniqueName,
                        Reason = reason,
                        CreatedBy = "SyncProcess",
                        CreatedOn = DateTime.UtcNow,
                        IsCompleted = isCompleted,
                        Status = status.ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }

        public void SaveSyncDocumentAuditDataUpdateDocumentStatus(string documentUniqueName, MongoSyncStatusType status, bool isCompleted = true, string reason = null)
        {
            try
            {
                //TODO: After Completing status should be updated as Completed or Done
                _syncRepository.Add(new DocumentMongoSync()
                {
                    Id = 0,
                    DocumentUniqueName = documentUniqueName,
                    Reason = reason,
                    CreatedBy = "Migration",
                    CreatedOn = DateTime.UtcNow,
                    IsCompleted = true,
                    Status = MongoSyncStatusType.Synced.ToString(),
                });

                var document = _documentRepository.FindBy(a => a.DocumentUniqueName == documentUniqueName)?.FirstOrDefault();
                if (document != null)
                {
                    document.Status = DocumentStatusType.EXT.ToString();
                    _documentRepository.Update(document);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }

        public string ExtractOnlyTextFromFile(string filePath)
        {
            try
            {
                if (_fileExtractorService.CanExtractText(filePath, out string validationMessage))
                {
                    return _fileExtractorService.GetContent(filePath);
                }
                else
                {
                    throw new Exception(validationMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                throw;
            }
        }

        private void PushTextToMongo(ModuleDocument document, string fileText)
        {
            try
            {
                if (string.IsNullOrEmpty(fileText))
                {
                    SaveSyncDocumentAuditData(document.DocumentUniqueName, MongoSyncStatusType.Failed_Retryable, false);
                    return;
                }
                _mongoDocumentService.Add(new EvolutionMongoDocument()
                {
                    UniqueName = document.DocumentUniqueName,
                    DocumentType = document.DocumentType,
                    ModuleCode = document.ModuleCode,
                    ReferenceCode = document.ModuleRefCode,
                    SubReferenceCode = document.SubModuleRefCode,
                    Text = fileText
                });
                SaveSyncDocumentAuditData(document.DocumentUniqueName, MongoSyncStatusType.Synced, true, MongoSyncStatusType.Synced.ToString());

            }
            catch (Exception ex)
            {
                SaveSyncDocumentAuditData(document.DocumentUniqueName, MongoSyncStatusType.Failed_Retryable, false, ex.Message);
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }

        private void PushTextToMongoMigratedDocuments(ModuleDocument document, string fileText)
        {
            try
            {
                if (string.IsNullOrEmpty(fileText))
                {
                    SaveSyncDocumentAuditDataUpdateDocumentStatus(document.DocumentUniqueName, MongoSyncStatusType.Failed_Retryable, false);
                    return;
                }
                _mongoDocumentService.Add(new EvolutionMongoDocument()
                {
                    UniqueName = document.DocumentUniqueName,
                    DocumentType = document.DocumentType,
                    ModuleCode = document.ModuleCode,
                    ReferenceCode = document.ModuleRefCode,
                    SubReferenceCode = document.SubModuleRefCode,
                    Text = fileText
                });
                SaveSyncDocumentAuditDataUpdateDocumentStatus(document.DocumentUniqueName, MongoSyncStatusType.Synced, true, MongoSyncStatusType.Synced.ToString());

            }
            catch (Exception ex)
            {
                SaveSyncDocumentAuditDataUpdateDocumentStatus(document.DocumentUniqueName, MongoSyncStatusType.Failed_Retryable, false, ex.Message);
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }

        public Response DeleteOrphandDocuments(IList<string> uniqueNames)
        {
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    _syncRepository.BulkDelete(x => uniqueNames.Contains(x.DocumentUniqueName));
                    tranScope.Complete();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success);
        }

        public async void DeleteOrphandDocumentsAsync(IList<string> uniqueNames)
        {
            try
            {
                await _syncRepository.DeleteAsync(x => uniqueNames.Contains(x.DocumentUniqueName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }

        private bool IsValidForSync(string fileName)
        {
            bool isValidType = false;
            if (!string.IsNullOrEmpty(_mongoSetting.DocumentTypes))
            {
                var extension = new FileInfo(fileName).Extension;
                if (!string.IsNullOrEmpty(extension))
                {
                    var type = _mongoSetting.DocumentTypes.Split(',');
                    isValidType = type.Any(x => x.ToLower() == extension.ToLower());
                }
            }
            return isValidType;
        }
    }
}