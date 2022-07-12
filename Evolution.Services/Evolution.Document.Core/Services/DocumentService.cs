using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Documents;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Data;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Document.Domain.Validations;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DBModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using TransactionOptions = System.Transactions.TransactionOptions;

namespace Evolution.Document.Core.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IAppLogger<DocumentService> _logger = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IMapper _mapper = null;
        private readonly DocumentUploadSetting _docUploadSetting = null;
        private readonly IDocumentRepository _documentRepository = null;
        private readonly IDocumentValidationService _validationService = null;

        public DocumentService(IDocumentRepository documentRepository,
                                IDocumentValidationService validationService,
                                IOptions<DocumentUploadSetting> docUploadSetting,
                                IAppLogger<DocumentService> logger,
                                IMapper mapper,
                                JObject message)
        {
            this._documentRepository = documentRepository;
            this._validationService = validationService;
            this._docUploadSetting = docUploadSetting.Value;
            this._logger = logger;
            this._mapper = mapper;
            this._messageDescriptions = message;
        }

        #region Public Methods

        public Response ChangeDocumentStatus(IList<ModuleDocument> documents)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();

                if (documents == null || documents?.Count == 0)
                {
                    validationMessages.Add(new ValidationMessage(documents, new List<MessageDetail> { new MessageDetail(ModuleType.Document, MessageType.InvalidPayLoad.ToId(), _messageDescriptions[MessageType.InvalidPayLoad.ToId()].ToString()) }));
                }
                else if (documents != null && documents.Any(x => string.IsNullOrEmpty(x.DocumentUniqueName)))
                {
                    documents.Where(x => string.IsNullOrEmpty(x.DocumentUniqueName)).ToList().ForEach(x =>
                    {
                        validationMessages.Add(new ValidationMessage(new { x.DocumentUniqueName }, new List<MessageDetail> { new MessageDetail(ModuleType.Document, MessageType.Madatory_Param.ToId(), _messageDescriptions[MessageType.Madatory_Param.ToId()].ToString()) }));
                    }
                    );
                }
                else
                {
                    // Search documents based on below fields only
                    var searchDocuments = documents?.Select(x => new ModuleDocument()
                    {
                        ModuleCode = x.ModuleCode,
                        ModuleRefCode = x.ModuleRefCode,
                        DocumentUniqueName = x.DocumentUniqueName,
                        Status = (x.Status == DocumentStatusType.C.ToString() ? DocumentStatusType.CR.ToString() : x.Status) // sanity def issue fix
                    })?.Distinct()?.ToList();

                    var dbDocuments = _mapper.Map<IList<DBModel.Document>>(this.Get(searchDocuments).Result.Populate<IList<ModuleDocument>>());

                    if (dbDocuments?.Count > 0)
                    {
                        var recordToBeUpdate = dbDocuments.Join(documents,
                                                                db => db.DocumentUniqueName,
                                                                model => model.DocumentUniqueName,
                                                                (db, model) => db.DocumentUniqueName).ToList();
                        var recordToInsert = documents.Select(x => x.DocumentUniqueName)
                                                      .Where(x => !recordToBeUpdate.Contains(x)).ToList();

                        _documentRepository.AutoSave = false;

                        if (recordToInsert?.Count > 0)
                        {
                            recordToInsert.ForEach(x =>
                            {
                                var domainDoc = documents.FirstOrDefault(x1 => x1.DocumentUniqueName == x);
                                if (domainDoc != null)
                                    _documentRepository.Add(_mapper.Map<DBModel.Document>(domainDoc));
                            });
                        }

                        if (recordToBeUpdate?.Count > 0)
                        {
                            dbDocuments = dbDocuments.Where(x => recordToBeUpdate.Contains(x.DocumentUniqueName)).ToList();
                            dbDocuments.ToList()
                                       .ForEach(x =>
                                       {
                                           var doc = documents.FirstOrDefault(x1 => x1.DocumentUniqueName == x.DocumentUniqueName);
                                           x.Status = doc.Status.ToString();
                                           x.Size = doc.DocumentSize;
                                           x.LastModification = DateTime.UtcNow;
                                           x.UpdateCount = x.UpdateCount.CalculateUpdateCount();
                                           x.ModifiedBy = doc.ModifiedBy;//TODO: Pass user name from Token by Controller.
                                       });
                            _documentRepository.Update(dbDocuments.ToList());
                        }

                        if (recordToBeUpdate?.Count > 0 || recordToInsert?.Count > 0)
                            _documentRepository.ForceSave();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documents);
            }
            finally
            {
                _documentRepository.AutoSave = true;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, documents, exception);
        }

        public Response ChangeDocumentStatus(ModuleDocument document)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();
                validationMessages = new List<ValidationMessage>();
                // Search documents based on below fields only
                var dbDocument = _documentRepository.FindBy(x => x.DocumentUniqueName == document.DocumentUniqueName).ToList();

                if (dbDocument != null)
                {
                    var recordToBeUpdate = dbDocument.Where(x => x.DocumentUniqueName == document.DocumentUniqueName).Select(x => x.DocumentUniqueName).ToList();

                    _documentRepository.AutoSave = false;

                    if (recordToBeUpdate?.Count > 0)
                    {
                        dbDocument.ToList()
                                   .ForEach(x =>
                                   {
                                       x.Status = document.Status.ToString();
                                       x.Size = document.DocumentSize;
                                       x.LastModification = DateTime.UtcNow;
                                       x.UpdateCount = x.UpdateCount.CalculateUpdateCount();
                                       x.ModifiedBy = document.ModifiedBy;//TODO: Pass user name from Token by Controller.
                                   });
                        _documentRepository.Update(dbDocument.ToList());
                    }
                    else
                    {
                        _documentRepository.Add(_mapper.Map<DBModel.Document>(document));
                    }

                    _documentRepository.ForceSave();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), document);
            }
            finally
            {
                _documentRepository.AutoSave = true;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        public Response UploadEmailDocuments(EmailDocument emailDocument)
        {
            Response response = null;
            try
            {
                if (emailDocument != null && emailDocument.EmailDocumentUpload != null && emailDocument.EmailDocumentUpload.Count > 0)
                {
                    foreach (var emailDocumentUpload in emailDocument.EmailDocumentUpload)
                    {
                        response = this.UploadEmailDocument(emailDocumentUpload.DocumentUniqueName, emailDocumentUpload.DocumentMessage, emailDocumentUpload.IsVisibleToCustomer, emailDocumentUpload.IsVisibleToTS);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), emailDocument);
            }
            return response;
        }

        public Response UploadEmailDocuments(EmailDocumentUpload emailDocument)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                if (emailDocument != null)
                    return this.UploadEmailDocument(emailDocument.DocumentUniqueName, emailDocument.DocumentMessage, emailDocument.IsVisibleToCustomer, emailDocument.IsVisibleToTS);
                else
                    validationMessages.Add(new ValidationMessage(emailDocument, new List<MessageDetail> { new MessageDetail(ModuleType.Document, MessageType.InvalidPayLoad.ToId(), _messageDescriptions[MessageType.InvalidPayLoad.ToId()].ToString()) }));

            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), emailDocument);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages, null, null);
        }

        public Response UploadEmailDocument(DocumentUniqueNameDetail model, string emailSubject, bool IsVisibleToCustomer, bool IsVisibleToTS)
        {
            string filePath = string.Empty;
            Exception exception = null;
            IList<DocumentUniqueNameDetail> result = null;
            try
            {
                string documentUniqueName = this.GenerateDocumentUniqueName(model.ModuleCode, model.DocumentName);
                string folderPath = this.CheckAndUpdateDocumentUploadPath();
                if (string.IsNullOrEmpty(folderPath)) // To handle all isFull=true folder path full scenario and no access exception
                {
                    string errorCode = MessageType.AllDocumentUploadFilepathFull.ToId();
                    _logger.LogError(ResponseType.Warning.ToId(), _messageDescriptions[errorCode].ToString(), null);
                    return new Response().ToPopulate(ResponseType.Success, null, new List<MessageDetail> { new MessageDetail(ModuleType.Document, errorCode, _messageDescriptions[errorCode].ToString()) }, null, null, null);
                }
                else
                {
                    filePath = Path.Combine(folderPath, documentUniqueName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Byte[] title = new UTF8Encoding(true).GetBytes(emailSubject);
                        stream.Write(title, 0, title.Length);
                        var uploadedDoc = new List<ModuleDocument>()
                    {
                        new ModuleDocument()
                        {
                            DocumentName = model.DocumentName,
                            DocumentType = model.DocumentType,
                            DocumentUniqueName = documentUniqueName,
                            Status = DocumentStatusType.C.ToString(),
                            CreatedOn = DateTime.UtcNow,
                            RecordStatus = RecordStatus.New.FirstChar(),
                            IsVisibleToCustomer = IsVisibleToCustomer,
                            IsVisibleToTS = IsVisibleToTS,
                            DocumentSize= stream.Length > 0 ? Convert.ToInt32(stream.Length/1024) < 1 ? 1 : (long?)Math.Round(Convert.ToDouble(stream.Length/1024),0, MidpointRounding.AwayFromZero) : 0,
                            ModuleCode = model.ModuleCode,
                            CreatedBy = model.RequestedBy,
                            ModuleRefCode = model.ModuleCodeReference,
                            SubModuleRefCode = "0"
                        }
                    };
                        var response = this.SaveDocument(uploadedDoc, true, true, true);
                        if (response.Code == ResponseType.Success.ToId() && response.Result != null)
                            result = _mapper.Map<IList<DocumentUniqueNameDetail>>(response.Result.Populate<IList<ModuleDocument>>());
                        else
                            return response;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response GenerateDocumentUniqueName(IList<DocumentUniqueNameDetail> model)
        {
            Exception exception = null;
            IList<DocumentUniqueNameDetail> result = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                if (model == null || model.Count == 0)
                {
                    validationMessages.Add(new ValidationMessage(model, new List<MessageDetail> { new MessageDetail(ModuleType.Document, MessageType.InvalidPayLoad.ToId(), _messageDescriptions[MessageType.InvalidPayLoad.ToId()].ToString()) }));
                }
                else if (model != null && model.Any(x => string.IsNullOrEmpty(x.DocumentName) || string.IsNullOrEmpty(x.ModuleCode)))
                {
                    model.Where(x => string.IsNullOrEmpty(x.DocumentName) || string.IsNullOrEmpty(x.ModuleCode)).ToList().ForEach(x =>
                    {
                        validationMessages.Add(new ValidationMessage(new { x.DocumentName, x.ModuleCode }, new List<MessageDetail> { new MessageDetail(ModuleType.Document, MessageType.Madatory_Param.ToId(), _messageDescriptions[MessageType.Madatory_Param.ToId()].ToString()) }));
                    });
                }
                else
                {
                    var param = _mapper.Map<IList<ModuleDocument>>(model);
                    if (param?.Count > 0)
                    {
                        param = param.Select(x =>
                        {
                            x.DocumentUniqueName = this.GenerateDocumentUniqueName(x.ModuleCode, x.DocumentName);
                            x.Status = DocumentStatusType.CR.ToString();
                            x.CreatedOn = DateTime.UtcNow;
                            x.RecordStatus = RecordStatus.New.FirstChar();
                            return x;
                        }).ToList();
                    }

                    var response = this.SaveDocument(param, true, true, true);
                    if (response.Code == ResponseType.Success.ToId() && response.Result != null)
                        result = _mapper.Map<IList<DocumentUniqueNameDetail>>(response.Result.Populate<IList<ModuleDocument>>());
                    else
                        return response;
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, result, exception, result?.Count);
        }

        public Response GetOrphandDocuments(int returnRowCount = 0)
        {
            Exception exception = null;  
            IList<string> orphandDocumentUniqueNames = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    if (returnRowCount > 0)
                        orphandDocumentUniqueNames = _documentRepository.FindBy(this.GetOrphandDocumentExpression())?.Select(x=>x.DocumentUniqueName)?.Take(returnRowCount)?.ToList();
                    else
                        orphandDocumentUniqueNames = _documentRepository.FindBy(this.GetOrphandDocumentExpression())?.Select(x => x.DocumentUniqueName)?.ToList();
                    tranScope.Complete();
                } 
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, orphandDocumentUniqueNames, exception, orphandDocumentUniqueNames?.Count);
        }

        public Response Get(IList<string> documentUniqueNames)
        {
            Exception exception = null;
            IList<ModuleDocument> result = null;
            try
            {
                result = _documentRepository.Get(documentUniqueNames);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documentUniqueNames);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(ModuleDocument model)
        {
            Exception exception = null;
            IList<ModuleDocument> result = null;
            try
            {
                result = new List<ModuleDocument>();
                if (model != null)
                {
                    //result = _documentRepository.Get(model);
                    using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        result = _documentRepository.FilterDocumentsByCompany(model);
                        tranScope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(IList<ModuleDocument> searchModel)
        {
            Exception exception = null;
            List<ModuleDocument> result = null;
            try
            {
                result = new List<ModuleDocument>();
                if (searchModel?.Count > 0)
                {
                    //TODO : Need to change below logic later.
                    foreach (var model in searchModel)
                    {
                        using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                                              new TransactionOptions
                                              {
                                                  IsolationLevel = IsolationLevel.ReadUncommitted
                                              }))
                        {
                            var searchResult = _documentRepository.Get(model);
                            if (searchResult?.Count > 0)
                            {
                                result.AddRange(searchResult);
                            }
                            tranScope.Complete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public Response Get(ModuleCodeType moduleCodeType, IList<string> moduleRefCodes = null, IList<string> subModuleRefCodes = null)
        {
            Exception exception = null;
            IList<ModuleDocument> result = null;
            try
            {
                if (moduleRefCodes?.Count == 0)
                {
                    return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception);
                }
                result = _documentRepository.Get(moduleCodeType, moduleRefCodes, subModuleRefCodes);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), moduleCodeType, moduleRefCodes, subModuleRefCodes);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, result, exception, result?.Count);
        }

        public DocumentDownloadResult Get(string documentUniqueName)
        {
            string filePath = string.Empty;
            DocumentDownloadResult result = new DocumentDownloadResult();
            try
            {
                var documentInfo = this.Get(new List<string> { documentUniqueName }).Result.Populate<IList<ModuleDocument>>().FirstOrDefault();
                if (documentInfo != null)
                {
                    result.DocumentName = documentInfo.DocumentName;
                    result.FileContent = File.ReadAllBytes(documentInfo.FilePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documentUniqueName);
            }
            return result;
        }
        //def 1289
        public IList<DocumentDownloadResult> GetEmailAttachmentDocuments(IList<string> documentUniqueNames)
        {
            string filePath = string.Empty;
            IList<DocumentDownloadResult> result = null;
            Exception exception = null;
            try
            {
                var documentInfos = this.Get(documentUniqueNames).Result.Populate<IList<ModuleDocument>>();
                if (documentInfos != null && documentInfos.Any())
                {
                    result = documentInfos.Select(x => { return new DocumentDownloadResult {
                        DocumentUniqueName=x.DocumentUniqueName,
                        DocumentName = x.DocumentName,
                       FileContent = File.ReadAllBytes(x.FilePath)
                   }; }).ToList();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documentUniqueNames);
            }
            return result;
        }

        public Response Modify(IList<ModuleDocument> documents, bool commitChange = true, bool isResultSetRequired = false)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (documents == null)
            {
                validationMessages.Add(new ValidationMessage(documents, new List<MessageDetail> { new MessageDetail(ModuleType.Document, MessageType.InvalidPayLoad.ToId(), _messageDescriptions[MessageType.InvalidPayLoad.ToId()].ToString()) }));
            }
            else if (documents?.Count > 0)
            {
                var recordTobeUpdate = documents?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
                //Changin status to New as Save will take care of New and modified records
                recordTobeUpdate.ForEach(x =>
                {
                    x.RecordStatus = RecordStatus.New.FirstChar();
                });

                return this.SaveDocument(recordTobeUpdate, isUniqueDocNameRequest: true);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages, null, null);
        }

        public Response Save(IList<ModuleDocument> documents, bool commitChange = true, bool isResultSetRequired = false)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (documents == null)
            {
                validationMessages.Add(new ValidationMessage(documents, new List<MessageDetail> { new MessageDetail(ModuleType.Document, MessageType.InvalidPayLoad.ToId(), _messageDescriptions[MessageType.InvalidPayLoad.ToId()].ToString()) }));
            }
            else if (documents?.Count > 0)
            {
                documents?.ToList().ForEach(x =>
                {
                    x.Id = null;
                    x.CreatedOn = DateTime.UtcNow;
                    x.CreatedBy = string.IsNullOrEmpty(x.CreatedBy) ? "" : x.CreatedBy;
                });
                return this.SaveDocument(documents, commitChange, isResultSetRequired);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages, null, null);
        }

        //Audit
        public Response Modify(IList<ModuleDocument> documents, ref List<DBModel.Document> dbDocuments, bool commitChange = true, bool isResultSetRequired = false)
        {
            var recordTobeUpdate = documents?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();
            //Changin status to New as Save will take care of New and modified records
            recordTobeUpdate.ForEach(x =>
            {
                x.RecordStatus = RecordStatus.New.FirstChar();
            });

            return this.SaveDocument(recordTobeUpdate, ref dbDocuments, isUniqueDocNameRequest: true);
        }

        public Response Save(IList<ModuleDocument> documents, ref List<DBModel.Document> dbDocuments, bool commitChange = true, bool isResultSetRequired = false)
        {
            documents?.ToList().ForEach(x =>
            {
                x.Id = null;
                x.CreatedOn = DateTime.UtcNow;
                x.CreatedBy = x.CreatedBy ?? "";
            });
            return this.SaveDocument(documents, ref dbDocuments, commitChange, isResultSetRequired);
        }

        public Response Get(string moduleCode, string moduleRefCode)
        {
            Exception exception = null;
            string[] result = null;
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            if (!string.IsNullOrEmpty(moduleCode) && !string.IsNullOrEmpty(moduleRefCode))
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    result = _documentRepository.GetCustomerName(moduleCode, moduleRefCode);
                    tranScope.Complete();
                }
            }
            else
                validationMessages.Add(new ValidationMessage(new { moduleCode, moduleRefCode }, new List<MessageDetail> { new MessageDetail(ModuleType.Document, MessageType.Madatory_Param.ToId(), _messageDescriptions[MessageType.Madatory_Param.ToId()].ToString()) }));

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages, result, exception, null);
        }

        public Response Paste(ModuleCodeType moduleCodeType, string referenceCode, string copyDocumentUniqueName, string logInUsername = "")
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            try
            {
                errorMessages = new List<MessageDetail>();

                // Search using documentUniqueName only as user can copy paste documents across defferent modules (ex :  project to contract)
                var result = this.Get(new List<string> { copyDocumentUniqueName });
                if (result.Code == MessageType.Success.ToId() && result.Result != null)
                {
                    var document = _mapper.Map<IList<DocumentUniqueNameDetail>>(result.Result.Populate<List<ModuleDocument>>()).ToList();

                    if (document?.Count > 0)
                    {
                        document.ForEach(x =>
                        {
                            x.UniqueName = null;
                            x.ModuleCode = moduleCodeType.ToString();
                            x.ModuleCodeReference = string.IsNullOrEmpty(referenceCode) ? null : referenceCode;
                            x.RequestedBy = string.IsNullOrEmpty(x.RequestedBy) ? logInUsername : x.RequestedBy;//TODO: Pass user name from Token by Controller.
                        });

                        var uniqueNameResult = GenerateDocumentUniqueName(document);
                        if (uniqueNameResult.Code == MessageType.Success.ToId() && uniqueNameResult.Result != null)
                        {
                            var pasteDocumentUniqueName = uniqueNameResult.Result.Populate<List<DocumentUniqueNameDetail>>().FirstOrDefault();
                            if (pasteDocumentUniqueName != null)
                            {
                                var copyFilePath = this._documentRepository.GetDocumentPathByName(copyDocumentUniqueName);

                                if (File.Exists(copyFilePath))
                                {
                                    var folderPath = this.CheckAndUpdateDocumentUploadPath();
                                    if (string.IsNullOrEmpty(folderPath)) // To handle all isFull=true folder path full scenario and no access exception
                                    {
                                        string errorCode = MessageType.AllDocumentUploadFilepathFull.ToId();
                                        _logger.LogError(ResponseType.Warning.ToId(), _messageDescriptions[errorCode].ToString(), null);
                                        return new Response().ToPopulate(ResponseType.Success, null, new List<MessageDetail> { new MessageDetail(ModuleType.Document, errorCode, _messageDescriptions[errorCode].ToString()) }, null, null, null);
                                    }
                                    else
                                    {
                                        var PasteFilePath = Path.Combine(folderPath, pasteDocumentUniqueName.UniqueName);
                                        // Copy paste existing file with new name
                                        File.Copy(copyFilePath, PasteFilePath);
                                        var copyPasteDoc = result.Result.Populate<List<ModuleDocument>>().Select(x => new ModuleDocument()
                                        {
                                            DocumentUniqueName = pasteDocumentUniqueName.UniqueName,
                                            Status = DocumentStatusType.C.ToString()
                                        }).FirstOrDefault();

                                        var statusResponse = this.ChangeDocumentStatus(copyPasteDoc);
                                        if (statusResponse.Code == ResponseType.Success.ToId())
                                        {
                                            pasteDocumentUniqueName.Status = DocumentStatusType.C.ToString();
                                            return new Response().ToPopulate(ResponseType.Success, pasteDocumentUniqueName);
                                        }
                                        else
                                        {
                                            return statusResponse;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            errorMessages.Add(new MessageDetail(ModuleType.Document, MessageType.Document_ChangeStatusFailed.ToId(), _messageDescriptions[MessageType.Document_ChangeStatusFailed.ToId()].ToString()));
                        }
                    }
                }
                errorMessages.Add(new MessageDetail(ModuleType.Document, MessageType.Document_NotExists.ToId(), _messageDescriptions[MessageType.Document_NotExists.ToId()].ToString()));

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), copyDocumentUniqueName);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, null, null, exception);
        }

        public Response Delete(IList<ModuleDocument> documents, bool commitChange = true, bool isResultSetRequired = false)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>(); ;
            List<ModuleDocument> result = null;
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                if (documents == null)
                {
                    validationMessages.Add(new ValidationMessage(documents, new List<MessageDetail> { new MessageDetail(ModuleType.Document, MessageType.InvalidPayLoad.ToId(), _messageDescriptions[MessageType.InvalidPayLoad.ToId()].ToString()) }));
                }
                else if (documents?.Count > 0)
                {
                    IList<ModuleDocument> recordTobeDeleted = documents?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();

                    if (this.IsRecordValidForProcess(documents,
                                                     ValidationType.Delete,
                                                     ref recordTobeDeleted,
                                                     ref errorMessages,
                                                     ref validationMessages))
                    {
                        if (recordTobeDeleted?.Count > 0)
                        {
                            _documentRepository.AutoSave = false;

                            var uniqueDocumentNames = recordTobeDeleted?.Where(x => !string.IsNullOrEmpty(x.DocumentUniqueName))?
                                                                                         .Select(x1 => x1.DocumentUniqueName)?
                                                                                         .ToList();

                            var dbDocuments = _documentRepository?.FindBy(x => uniqueDocumentNames.Contains(x.DocumentUniqueName))?.ToList();

                            dbDocuments.ForEach(x =>
                            {
                                var document = recordTobeDeleted?.FirstOrDefault(x1 => x1.DocumentUniqueName == x.DocumentUniqueName);
                                x.IsDeleted = true;
                                x.LastModification = DateTime.UtcNow;
                                x.ModifiedBy = document.ModifiedBy;
                            });

                            _documentRepository.Update(dbDocuments);

                            if (commitChange && !_documentRepository.AutoSave && recordTobeDeleted?.Count > 0 && errorMessages.Count <= 0)
                            {
                                _documentRepository.ForceSave();
                                if (isResultSetRequired)
                                    result = _mapper.Map<List<ModuleDocument>>(dbDocuments);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documents);
            }
            finally
            {
                _documentRepository.AutoSave = true;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, result, exception);
        }

        public Response DeletOrphandDocuments()
        {
            try
            {
                _documentRepository.Delete(this.GetOrphandDocumentExpression());
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return new Response().ToPopulate(ResponseType.Success);
        }

        public void DeletOrphandDocuments(List<string> documentUniqueNames)
        {
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    _documentRepository.BulkDelete(x => documentUniqueNames.Contains(x.DocumentUniqueName));
                    tranScope.Complete();
                } 
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            } 
        }
        public void DeletOrphandDocumentFiles(List<string> documentUniqueNames)
        {
            List<string> OrphandDocumentFilePath = null;
            try
            {
                using (var tranScope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    OrphandDocumentFilePath = _documentRepository.FindBy(x => documentUniqueNames.Contains(x.DocumentUniqueName)).Select(x => x.FilePath).Distinct().ToList();
                    tranScope.Complete();
                }
                OrphandDocumentFilePath = OrphandDocumentFilePath?.Where(x => !string.IsNullOrEmpty(x)).ToList();
                foreach (string filepath in OrphandDocumentFilePath)
                {
                    try
                    { 
                        File.Delete(filepath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                        continue;
                    } 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
        }
         
        public DocumentDownloadResult Download(ModuleCodeType moduleCodeType, string documentUniqueName)
        {
            DocumentDownloadResult result = new DocumentDownloadResult();
            try
            {
                var documentInfo = this.Get(new List<string> { documentUniqueName }).Result.Populate<IList<ModuleDocument>>().FirstOrDefault();
                if (documentInfo != null)
                {
                    result.DocumentName = documentInfo.DocumentName;
                    result.FileContent = File.ReadAllBytes(documentInfo.FilePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documentUniqueName);
            }
            return result;
        }

        public Response Upload(IFormFileCollection files, string code)
        {
            throw new NotImplementedException();
        }

        public Response UploadByStream(HttpRequest httpRequest, ModuleCodeType moduleCodeType, string referenceCode, string documentUniqueName)
        {
            return this.ProcessStreamUpload(httpRequest, moduleCodeType, referenceCode, documentUniqueName).Result;
        }

        public async Task<Response> UploadByStreamAsync(HttpRequest httpRequest, ModuleCodeType moduleCodeType, string referenceCode, string documentUniqueName)
        {
            return await this.ProcessStreamUpload(httpRequest, moduleCodeType, referenceCode, documentUniqueName);
        }

        #endregion

        #region Private Methods

        private Response SaveDocument(IList<ModuleDocument> documents, bool commitChange = true, bool isResultSetRequired = false, bool isUniqueDocNameRequest = false)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = new List<MessageDetail>();
            List<ValidationMessage> validationMessages = null;
            try
            {
                _documentRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ModuleDocument> recordToBeInserted = null;

                if (this.IsRecordValidForProcess(documents,
                                                 ValidationType.Add,
                                                 ref recordToBeInserted,
                                                 ref errorMessages,
                                                 ref validationMessages))
                {
                    if (recordToBeInserted?.Count > 0)
                    {
                        var uniqueDocumentNames = recordToBeInserted?.Where(x => !string.IsNullOrEmpty(x.DocumentUniqueName))?
                                                                         .Select(x1 => x1.DocumentUniqueName)?
                                                                         .ToList();

                        var dbDocuments = _documentRepository?.FindBy(x => uniqueDocumentNames.Contains(x.DocumentUniqueName))?
                                                              .ToList();
                        if (!isUniqueDocNameRequest)
                            this.IsValidDocument(recordToBeInserted, ref errorMessages, dbDocuments);

                        if (errorMessages?.Count <= 0)
                        {
                            recordToBeInserted?.ToList().ForEach(x =>
                            {
                                var document = dbDocuments?.FirstOrDefault(x1 => x1.DocumentUniqueName == x.DocumentUniqueName);
                                if (document != null)
                                {
                                    document.DocumentName = x.DocumentName;
                                    document.DocumentType = x.DocumentType;
                                    document.IsVisibleToCustomer = x.IsVisibleToCustomer;
                                    document.IsVisibleToTechSpecialist = x.IsVisibleToTS;
                                    document.IsVisibleToOutsideOfCompany = x.IsVisibleOutOfCompany;
                                    document.Status = x.Status;
                                    document.DocumentUniqueName = x.DocumentUniqueName;
                                    document.ModuleCode = x.ModuleCode;
                                    document.ModuleRefCode = x.ModuleRefCode;
                                    document.SubModuleRefCode = x.SubModuleRefCode ?? "0";
                                    document.Size = x.DocumentSize;
                                    document.LastModification = DateTime.UtcNow;
                                    document.UpdateCount = x.UpdateCount.CalculateUpdateCount();
                                    document.ModifiedBy = x.ModifiedBy;
                                    document.Comments = x.Comments;
                                    document.ExpiryDate = x.ExpiryDate;
                                    document.DisplayOrder = x.DisplayOrder;
                                    document.ApprovalDate = x.ApprovalDate;
                                    document.ApprovedBy = x.ApprovedBy;
                                    document.IsForApproval = x.IsForApproval;
                                    document.DocumentTitle = x.DocumentTitle;
                                    if (x.IsDeleted != null)//Sanity defect 148 fix
                                        document.IsDeleted = x.IsDeleted;

                                    _documentRepository.Update(document);
                                }
                                else
                                {
                                    string folderPath = this.CheckAndUpdateDocumentUploadPath();
                                    if (string.IsNullOrEmpty(folderPath)) // To handle all isFull=true folder path full scenario and no access exception
                                    {
                                        string errorCode = MessageType.AllDocumentUploadFilepathFull.ToId();
                                        errorMessages.Add(new MessageDetail(ModuleType.Document, errorCode, _messageDescriptions[errorCode].ToString()));
                                        _logger.LogError(ResponseType.Warning.ToId(), _messageDescriptions[errorCode].ToString(), null);
                                    }
                                    else
                                    {
                                        x.FilePath = Path.Combine(folderPath, x.DocumentUniqueName);
                                        _documentRepository.Add(_mapper.Map<DBModel.Document>(x));
                                    }
                                }
                            });

                            if (commitChange && !_documentRepository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                            {
                                _documentRepository.ForceSave();
                                if (isResultSetRequired)
                                {
                                    var unqName = documents?.Select(x => x.DocumentUniqueName)?.Distinct().ToList();
                                    return this.Get(unqName);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documents);
            }
            finally
            {
                _documentRepository.AutoSave = true;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private Response SaveDocument(IList<ModuleDocument> documents, ref List<DBModel.Document> dbDocuments, bool commitChange = true, bool isResultSetRequired = false, bool isUniqueDocNameRequest = false)
        {
            Exception exception = null;
            List<MessageDetail> errorMessages = null;
            List<ValidationMessage> validationMessages = null;
            try
            {
                _documentRepository.AutoSave = false;
                errorMessages = new List<MessageDetail>();
                IList<ModuleDocument> recordToBeInserted = null;

                if (this.IsRecordValidForProcess(documents,
                                                 ValidationType.Add,
                                                 ref recordToBeInserted,
                                                 ref errorMessages,
                                                 ref validationMessages))
                {
                    if (recordToBeInserted?.Count > 0)
                    {
                        var uniqueDocumentNames = recordToBeInserted?.Where(x => !string.IsNullOrEmpty(x.DocumentUniqueName))?
                                                                         .Select(x1 => x1.DocumentUniqueName)?
                                                                         .ToList();

                        var dbDocumentsOld = _documentRepository?.FindBy(x => uniqueDocumentNames.Contains(x.DocumentUniqueName))?
                                                              .ToList();
                        var filteredDocuments = dbDocumentsOld?.ToList()?.Select(x => new DBModel.Document
                        {
                            Id = x.Id,
                            DocumentName = x.DocumentName,
                            DocumentType = x.DocumentType,
                            Size = x.Size,
                            IsVisibleToTechSpecialist = x.IsVisibleToTechSpecialist,
                            IsVisibleToCustomer = x.IsVisibleToCustomer,
                            IsVisibleToOutsideOfCompany = x.IsVisibleToOutsideOfCompany,
                            Status = x.Status,
                            ModuleCode = x.ModuleCode,
                            ModuleRefCode = x.ModuleRefCode,
                            DocumentUniqueName = x.DocumentUniqueName,
                            CreatedDate = x.CreatedDate, // To show created date in Audit
                        })?.ToList();
                        dbDocuments = ObjectExtension.Clone(filteredDocuments); // Need keep dbDocument as a ref variable to avoid one fetch
                        if (!isUniqueDocNameRequest)
                            this.IsValidDocument(recordToBeInserted, ref errorMessages, dbDocumentsOld);

                        if (errorMessages?.Count <= 0)
                        {
                            recordToBeInserted?.ToList().ForEach(x =>
                            {
                                var document = dbDocumentsOld?.FirstOrDefault(x1 => x1.DocumentUniqueName == x.DocumentUniqueName);
                                if (document != null)
                                {
                                    document.DocumentName = x.DocumentName;
                                    document.DocumentType = x.DocumentType;
                                    document.IsVisibleToCustomer = x.IsVisibleToCustomer;
                                    document.IsVisibleToTechSpecialist = x.IsVisibleToTS;
                                    document.IsVisibleToOutsideOfCompany = x.IsVisibleOutOfCompany;
                                    document.Status = x.Status;
                                    document.DocumentUniqueName = x.DocumentUniqueName;
                                    document.ModuleCode = x.ModuleCode;
                                    document.ModuleRefCode = x.ModuleRefCode;
                                    document.SubModuleRefCode = x.SubModuleRefCode ?? "0";
                                    document.Size = x.DocumentSize;
                                    document.LastModification = DateTime.UtcNow;
                                    document.UpdateCount = x.UpdateCount.CalculateUpdateCount();
                                    document.ModifiedBy = x.ModifiedBy;
                                    document.Comments = x.Comments;
                                    document.ExpiryDate = x.ExpiryDate;
                                    document.DisplayOrder = x.DisplayOrder;
                                    document.ApprovalDate = x.ApprovalDate;
                                    document.ApprovedBy = x.ApprovedBy;
                                    document.IsForApproval = x.IsForApproval;
                                    document.DocumentTitle = x.DocumentTitle;
                                    if (x.IsDeleted != null)//def 1306 Issue 1 :  document delete issue fix
                                        document.IsDeleted = x.IsDeleted; 

                                    _documentRepository.Update(document);
                                }
                                else
                                {
                                    string folderPath = this.CheckAndUpdateDocumentUploadPath();
                                    if (string.IsNullOrEmpty(folderPath))// To handle all isFull=true folder path full scenario and no access exception
                                    {
                                        string errorCode = MessageType.AllDocumentUploadFilepathFull.ToId();
                                        errorMessages.Add(new MessageDetail(ModuleType.Document, errorCode, _messageDescriptions[errorCode].ToString()));
                                        _logger.LogError(ResponseType.Warning.ToId(), _messageDescriptions[errorCode].ToString(), null);
                                    }
                                    else
                                    {
                                        x.FilePath = Path.Combine(folderPath, x.DocumentUniqueName);
                                        _documentRepository.Add(_mapper.Map<DBModel.Document>(x));
                                    }
                                }
                            });

                            if (commitChange && !_documentRepository.AutoSave && recordToBeInserted?.Count > 0 && errorMessages.Count <= 0)
                            {
                                _documentRepository.ForceSave();
                                if (isResultSetRequired)
                                {
                                    var unqName = documents?.Select(x => x.DocumentUniqueName)?.Distinct().ToList();
                                    return this.Get(unqName);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), documents);
            }
            finally
            {
                _documentRepository.AutoSave = true;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return new Response().ToPopulate(ResponseType.Success, null, errorMessages, validationMessages, null, exception);
        }

        private async Task<Response> ProcessStreamUpload(HttpRequest httpRequest,
                                                         ModuleCodeType moduleCodeType,
                                                         string referenceCode,
                                                         string documentUniqueName)
        {
            List<MessageDetail> errorMessages = null;
            ResponseType responseType = ResponseType.Success;
            DocumentUploadResponse result = null;
            string filePath = string.Empty;
            IList<ModuleDocument> dbDocuments = null;
            try
            {
                errorMessages = new List<MessageDetail>();

                if (moduleCodeType == ModuleCodeType.None)
                    throw new Exception("Invalid Module Code Passed.");
                else if (string.IsNullOrEmpty(documentUniqueName))
                    throw new Exception("Document Unique Name Not Passed.");
                if (!IsValidDocumentExtension(documentUniqueName, ref errorMessages))
                {
                    return new Response().ToPopulate(ResponseType.Validation, errorMessages, null, 0);
                }
                if (!this.ValidateDocumentUniqueName(moduleCodeType, referenceCode, documentUniqueName, ref dbDocuments, ref errorMessages))
                {
                    return new Response().ToPopulate(ResponseType.Validation, errorMessages, null, 0);
                }
                filePath = this._documentRepository.GetDocumentPathByName(documentUniqueName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    bool streamResult = await httpRequest.StreamFile(stream);
                    if (streamResult)
                    {
                        var uploadedDoc = new List<ModuleDocument>()
                            {
                                new ModuleDocument()
                                {
                                    ModuleCode = moduleCodeType.ToString(),
                                    ModuleRefCode = referenceCode,
                                    DocumentUniqueName = documentUniqueName,
                                    Status=DocumentStatusType.C.ToString(),
                                    DocumentSize=stream.Length > 0 ? Convert.ToInt32(stream.Length/1024) < 1 ? 1 : (long?)Math.Round(Convert.ToDouble(stream.Length/1024),0, MidpointRounding.AwayFromZero) : 0,
                                }
                            };
                        var statusResponse = this.ChangeDocumentStatus(uploadedDoc);
                        if (statusResponse.Code.ToEnum<ResponseType>() != ResponseType.Success)
                            return statusResponse;

                        result = new DocumentUploadResponse()
                        {
                            ModuleCode = moduleCodeType.ToString(),
                            ModuleReferenceCode = referenceCode,
                            UploadedFileName = documentUniqueName,
                            FileName = dbDocuments?.FirstOrDefault(x => x.DocumentUniqueName == documentUniqueName).DocumentName
                        };
                    }
                    else
                    {
                        errorMessages.Add(new MessageDetail(MessageType.Document_UploadFailed.ToId(), this._messageDescriptions[MessageType.Document_UploadFailed.ToId()].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
                responseType = ResponseType.Exception;
                errorMessages.Add(new MessageDetail(ModuleType.Document, ResponseType.Exception.ToId(), ex.Message));
            }
            return new Response().ToPopulate(responseType, errorMessages, result, (result == null ? 0 : 1));
        }

        private bool ValidateDocumentUniqueName(ModuleCodeType moduleCodeType,
                                                string moduleCodeReference,
                                                string documentUniqueName,
                                                ref List<MessageDetail> errorMessage)
        {
            errorMessage = errorMessage ?? new List<MessageDetail>();

            var response = this._documentRepository.Get(new ModuleDocument()
            {
                DocumentUniqueName = documentUniqueName,
                ModuleCode = moduleCodeType.ToString(),
                ModuleRefCode = moduleCodeReference,
                Status = DocumentStatusType.CR.ToString()
            });

            if (response?.Count > 0)
                return true;
            else
            {
                string errorCode = MessageType.Document_InvalidUniqueName.ToId();
                errorMessage.Add(new MessageDetail(errorCode, _messageDescriptions[errorCode].ToString()));
                return false;
            }
        }

        private bool ValidateDocumentUniqueName(ModuleCodeType moduleCodeType,
                                               string moduleCodeReference,
                                               string documentUniqueName,
                                               ref IList<ModuleDocument> dbDocuments,
                                               ref List<MessageDetail> errorMessage)
        {
            errorMessage = errorMessage ?? new List<MessageDetail>();

            dbDocuments = this._documentRepository.Get(new ModuleDocument()
            {
                DocumentUniqueName = documentUniqueName,
                ModuleCode = moduleCodeType.ToString(),
                ModuleRefCode = moduleCodeReference,
                Status = DocumentStatusType.CR.ToString()
            });

            if (dbDocuments?.Count > 0)
                return true;
            else
            {
                string errorCode = MessageType.Document_InvalidUniqueName.ToId();
                errorMessage.Add(new MessageDetail(errorCode, _messageDescriptions[errorCode].ToString()));
                return false;
            }
        }

        private string GetDateQurter()
        {
            int currentMonth = DateTime.Now.Month;
            string currentYear = Convert.ToString(DateTime.Now.Year);
            if (currentMonth >= 1 && currentMonth <= 3)
                return string.Concat("Q1", currentYear);
            else if (currentMonth >= 4 && currentMonth <= 6)
                return string.Concat("Q2", currentYear);
            else if (currentMonth >= 7 && currentMonth <= 9)
                return string.Concat("Q3", currentYear);
            else
                return string.Concat("Q4", currentYear);
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
           out ulong lpFreeBytesAvailable,
           out ulong lpTotalNumberOfBytes,
           out ulong lpTotalNumberOfFreeBytes);

        public string CheckAndUpdateDocumentUploadPath()
        {
            try
            {
                var response = this._documentRepository.GetDocumentUploadPath();
                if (response != null && !string.IsNullOrEmpty(response.FolderPath))
                {
                    bool success = GetDiskFreeSpaceEx(response.FolderPath,
                                                        out ulong FreeBytesAvailable,
                                                        out ulong TotalNumberOfBytes,
                                                        out ulong TotalNumberOfFreeBytes);
                    ulong freeGigaBytesAvailable = FreeBytesAvailable / 1024 / 1024 / 1024;
                    if (success && freeGigaBytesAvailable < 10)
                    {
                        response = this._documentRepository.UpdateDocumentUploadFull(response.Id);
                    }
                    if (success && response != null && !string.IsNullOrEmpty(response.FolderPath))
                    {
                        var path = Path.Combine(response.FolderPath, this.GetDateQurter());
                        if (Utility.CheckAndCreateFolder(path))
                            return path;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString());
            }
            return string.Empty;
        }

        private string GetDocumentUploadPath()
        {
            var path = Path.Combine(System.AppContext.BaseDirectory, this._docUploadSetting.FolderName);
            if (Utility.CheckAndCreateFolder(path))
                return path;
            else
                return string.Empty;
        }

        private bool IsRecordValidForProcess(IList<ModuleDocument> documents,
                                                ValidationType validationType,
                                                ref IList<ModuleDocument> filteredDocument,
                                                ref List<MessageDetail> errorMessages,
                                                ref List<ValidationMessage> validationMessages)
        {
            if (validationType == ValidationType.Add)
                filteredDocument = documents?.Where(x => x.RecordStatus.IsRecordStatusNew()).ToList();
            else if (validationType == ValidationType.Delete)
                filteredDocument = documents?.Where(x => x.RecordStatus.IsRecordStatusDeleted()).ToList();
            else if (validationType == ValidationType.Update)
                filteredDocument = documents?.Where(x => x.RecordStatus.IsRecordStatusModified()).ToList();

            if (filteredDocument?.Count <= 0)
                return false;

            return IsDocumentHasValidSchema(filteredDocument, validationType, ref errorMessages, ref validationMessages);
        }

        private bool IsDocumentHasValidSchema(IList<ModuleDocument> models,
                                                ValidationType validationType,
                                                ref List<MessageDetail> errorMessages,
                                                ref List<ValidationMessage> validationMessages)
        {
            var messages = new List<ValidationMessage>();

            if (validationMessages == null)
                validationMessages = new List<ValidationMessage>();

            var validationResults = _validationService.Validate(JsonConvert.SerializeObject(models), validationType);
            validationResults.ToList().ForEach(x =>
            {
                messages.Add(new ValidationMessage(x.Code, new List<MessageDetail> { new MessageDetail(ModuleType.Document, x.Code, x.Message) }));
            });

            models?.Where(x => !x.RecordStatus.IsRecordStatusDeleted() && x.Status == DocumentStatusType.C.ToString() && !string.IsNullOrEmpty(x.ModuleRefCode) && string.IsNullOrEmpty(x.DocumentType)).ToList().ForEach(x => //def 1344 fix
            {
                messages.Add(new ValidationMessage(new { x.ModuleRefCode, x.DocumentType }, new List<MessageDetail> { new MessageDetail(ModuleType.Document, MessageType.Madatory_Param_DocumentType.ToId(), _messageDescriptions[MessageType.Madatory_Param_DocumentType.ToId()].ToString()) }));
            });

            if (messages.Count > 0)
            {
                validationMessages.AddRange(messages);
            }
            return validationMessages?.Count <= 0;
        }

        private bool IsValidDocument(IList<ModuleDocument> documents,
                                     ref List<MessageDetail> errorMessages,
                                     List<DBModel.Document> dbDocuments = null)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            if (errorMessages == null)
                errorMessages = new List<MessageDetail>();

            IList<string> uniqueDocumentNames = null;
            if (dbDocuments == null)
            {
                uniqueDocumentNames = documents?.Where(x => !string.IsNullOrEmpty(x.DocumentUniqueName))?
                                                .Select(x1 => x1.DocumentUniqueName)?
                                                .ToList();

                dbDocuments = _documentRepository?.FindBy(x => uniqueDocumentNames.Contains(x.DocumentUniqueName))?
                                                  .ToList();
            }

            IList<ModuleDocument> docAlreadyExist = new List<ModuleDocument>();
            documents.ToList().ForEach(x =>
            {
                DBModel.Document document = null;
                if (x.Id > 0) //Edit
                    document = dbDocuments.FirstOrDefault(x1 => x1.DocumentUniqueName == x.DocumentUniqueName && x1.Id != x.Id);

                if (document != null)
                    docAlreadyExist.Add(_mapper.Map<ModuleDocument>(document));
            });

            if (docAlreadyExist?.Count > 0)
            {
                dbDocuments?.ForEach(x =>
                {
                    string errorCode = MessageType.DocumentUniqueNameExist.ToId();
                    messages.Add(new MessageDetail(ModuleType.Document, errorCode, string.Format(_messageDescriptions[errorCode].ToString(), x.DocumentUniqueName)));
                });
            }

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count == 0;
        }

        private string GenerateDocumentUniqueName(string moduleCode, string documentName = null)
        {
            var extension = string.Empty;

            if (!string.IsNullOrEmpty(documentName))
                extension = Path.GetExtension(documentName);
            if (string.IsNullOrEmpty(extension))
                extension = this._docUploadSetting.FileDefaultExtension;

            return string.Format("{0}-{1}-{2}-{3}-{4}.{5}", moduleCode, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, Guid.NewGuid(), extension.Replace(".", ""));
        }

        private Expression<Func<DBModel.Document, bool>> GetOrphandDocumentExpression()
        {
            return (x) => (x.IsDeleted == true) || (DateTime.UtcNow.Date != x.CreatedDate.Date && x.Status == DocumentStatusType.CR.ToString()) || (DateTime.UtcNow.Date != x.CreatedDate.Date && x.Status == DocumentStatusType.C.ToString() && string.IsNullOrEmpty(x.ModuleRefCode)) || (x.Status == DocumentStatusType.R.ToString());//D1186 & D1188
        }

        private bool IsValidDocumentExtension(string fileName, ref List<MessageDetail> errorMessages)
        {
            List<MessageDetail> messages = new List<MessageDetail>();
            bool? isValidType = false;

            int i = fileName.LastIndexOf('.');
            string extension = i > 0 ? fileName.Substring(i) : null;
            // var extension = new FileInfo(fileName).Extension;
            if (string.IsNullOrEmpty(extension))
            {
                messages.Add(new MessageDetail(ModuleType.Document, MessageType.InvalidFileExtention.ToId(), string.Format(_messageDescriptions[MessageType.InvalidFileExtention.ToId()].ToString(), extension)));
            }
            else if (string.IsNullOrEmpty(_docUploadSetting.DocumentTypes))
            {
                messages.Add(new MessageDetail(ModuleType.Document, MessageType.MasterDocumentTypesListNotFound.ToId(), string.Format(_messageDescriptions[MessageType.MasterDocumentTypesListNotFound.ToId()].ToString(), extension)));
            }
            else
            {
                var type = _docUploadSetting.DocumentTypes.Split(',');
                isValidType = type?.Any(x => x.ToLower() == extension.ToLower());

                if (isValidType == false)
                    messages.Add(new MessageDetail(ModuleType.Document, MessageType.NotSupportedDocument.ToId(), string.Format(_messageDescriptions[MessageType.NotSupportedDocument.ToId()].ToString(), extension)));
            }

            if (messages.Count > 0)
                errorMessages.AddRange(messages);

            return errorMessages?.Count == 0;
        }


        #endregion
    }
}