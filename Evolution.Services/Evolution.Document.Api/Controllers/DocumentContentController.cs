using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Evolution.Document.Api.Controllers
{
    [Route("api/DocumentContent/")]
    public class DocumentContent : BaseController
    {
        private readonly IDocumentService _documentService = null;
        private readonly IAppLogger<DocumentController> _logger = null;
        private readonly IDocumentMongoSyncService _documentMongoSyncService = null;
        private readonly IMongoDocumentService _mongoDocumentService = null;
        private readonly JObject _messageDescriptions = null;

        public DocumentContent(IDocumentService documentService,
                                    IAppLogger<DocumentController> logger,
                                    IDocumentMongoSyncService documentMongoSyncService,
                                    IMongoDocumentService mongoDocumentService)
        {
            _documentService = documentService;
            _logger = logger;
            _documentMongoSyncService = documentMongoSyncService;
            _mongoDocumentService = mongoDocumentService;
            _messageDescriptions = JObject.Parse(System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")));
        }

        [HttpPost]
        [Route("DeleteOrphandDocuments")]
        public Response DeleteOrphandDocuments([FromBody]List<string> documentUiqueNames)
        {
            ResponseType responseType = ResponseType.Success;
            Exception exception = null;
            try
            {
                if (documentUiqueNames?.Count <= 0)
                {
                    return new Response().ToPopulate(ResponseType.Success, null, null, new List<ValidationMessage> { new ValidationMessage(documentUiqueNames, new List<MessageDetail> { new MessageDetail(ModuleType.Document, MessageType.InvalidPayLoad.ToId(), _messageDescriptions[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);
                } 
                _documentService.DeletOrphandDocumentFiles(documentUiqueNames);//delete file from path
                _documentService.DeletOrphandDocuments(documentUiqueNames);//delete from comman.document Table 
                _documentMongoSyncService.DeleteOrphandDocuments(documentUiqueNames); //delete from comman.documentMongosync Table
                _mongoDocumentService.RemoveByUniqueNameAsync(documentUiqueNames);//delete from mongo DB Table

            }
            catch (Exception ex)
            {
                exception = ex;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { documentUiqueNames });
                responseType = ResponseType.Exception;
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("FetchOnlyTextFromFile")]
        public Response FeatchOnlyTextFromFile([FromBody]string filePath)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            string fileText = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    return new Response().ToPopulate(ResponseType.Error, null, null, new List<ValidationMessage> { new ValidationMessage(filePath, new List<MessageDetail> { new MessageDetail(ModuleType.Document, MessageType.InvalidPayLoad.ToId(), _messageDescriptions[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);
                }

                fileText = _documentMongoSyncService.ExtractOnlyTextFromFile(filePath);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(responseType.ToId(), ex.ToFullString(), new { filePath });
            }
            return new Response().ToPopulate(responseType, null, null, null, fileText, exception);
        }
    }
}