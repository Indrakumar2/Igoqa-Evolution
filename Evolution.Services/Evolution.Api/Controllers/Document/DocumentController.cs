//using Evolution.Common.Enums;
//using Evolution.Common.Extensions;
//using Evolution.Common.Helpers;
//using Evolution.Common.Models.Responses;
//using Evolution.Document.Domain.Interfaces.Documents;
//using Evolution.Document.Domain.Models.Document;
//using Evolution.Logging.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;


//namespace Evolution.Api.Controllers.Document
//{
//    [Route("api/documents/{moduleType}/")]
//    [ApiController]    
//    public class DocumentController : ControllerBase
//    {
//        private IDocumentService _documentService = null;
//        private readonly IAppLogger<DocumentController> _logger = null;
//        private readonly string _Document = "Documents";
//        private readonly JObject _messageDescriptions = null;
        
//        public DocumentController(IDocumentService documentService, IAppLogger<DocumentController> logger)
//        {
//            _documentService = documentService;
//            _logger = logger;
//            this._messageDescriptions = JObject.Parse(System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")));
//        }

//        [HttpPost]
//        public Response Post(IFormFileCollection files, [FromQuery]string code)
//        {
//            return this._documentService.Upload(files, code);
//        }

//        [HttpGet]
//        [Route("{Id}")]
//        public FileContentResult Get([FromRoute]string moduleType, [FromQuery]string code, [FromRoute]string Id)
//        {
//            string fileMimeType = "text/plain";
//            string fileName = "notfound.txt";
//            byte[] fileData = null;
//            try
//            {
//                if (!string.IsNullOrEmpty(moduleType) && !string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(Id))
//                {
//                    var document = this._documentService.Download(moduleType.ToEnum<ModuleCodeType>(), Id);
//                    var mimeType = document != null ? Utility.GetMimeTypes(Path.GetExtension(document?.DocumentName)) : "";
//                    if (!string.IsNullOrEmpty(mimeType) && document?.DocumentData != null && document?.DocumentData.Length > 0)
//                    {
//                        fileMimeType = mimeType;
//                        fileName = document.DocumentName;
//                        fileData = document.DocumentData;
//                    }
//                }
//                else
//                {
//                    fileData = Encoding.Unicode.GetBytes(this._messageDescriptions[MessageType.Document_NotExists.ToId()].ToString());
//                    _logger.LogError(MessageType.Document_NotExists.ToId(), this._messageDescriptions[MessageType.Document_NotExists.ToId()].ToString());
//                }
//            }
//            catch (Exception ex)
//            {
//                fileData = Encoding.Unicode.GetBytes(this._messageDescriptions[MessageType.Document_NotExists.ToId()].ToString());
//                _logger.LogError(ResponseType.Exception.ToId(), ex.Message);
//            }

//            return this.GenerateFileResult(fileName, fileMimeType, fileData);
//        }

//        [HttpGet]
//        [Route("{Id}/temp")]
//        public FileContentResult GetDocument([FromRoute]string moduleType, [FromQuery]string code, [FromRoute] string id)
//        {
//            string fileMimeType = "text/plain";
//            string fileName = "notfound.txt";
//            byte[] fileData = null;
//            try
//            {
//                if (!string.IsNullOrEmpty(moduleType) && !string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(id))
//                {
//                    var path = Path.Combine(Directory.GetCurrentDirectory(), _Document);
//                    if (!string.IsNullOrEmpty(code))
//                        path = Path.Combine(path, code);

//                    if (Directory.Exists(path))
//                    {
//                        foreach (var item in System.IO.Directory.GetFiles(path))
//                        {
//                            fileName = Path.GetFileName(item);
//                            if (fileName.Split('@').ToList()[0] == id)
//                            {
//                                if (System.IO.File.Exists(Path.Combine(path, fileName)))
//                                {
//                                    var mimeType = Utility.GetMimeTypes(Path.GetExtension(fileName));
//                                    if (!string.IsNullOrEmpty(mimeType))
//                                    {
//                                        fileData = System.IO.File.ReadAllBytes(Path.Combine(path, fileName));
//                                        fileMimeType = mimeType;
//                                        fileName = fileName.Split('@')[1];
//                                        break;
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                fileName = "notfound.txt";
//                                fileData = Encoding.Unicode.GetBytes(this._messageDescriptions[MessageType.Document_NotExists.ToId()].ToString());
//                            }
//                        }
//                    }
//                    else
//                    {
//                        fileData = Encoding.Unicode.GetBytes(this._messageDescriptions[MessageType.DocumentPath_NotExists.ToId()].ToString());
//                        _logger.LogError(MessageType.DocumentPath_NotExists.ToId(), this._messageDescriptions[MessageType.DocumentPath_NotExists.ToId()].ToString());
//                    }
//                }
//                else
//                {
//                    fileData = Encoding.Unicode.GetBytes(this._messageDescriptions[MessageType.Document_NotExists.ToId()].ToString());
//                    _logger.LogError(MessageType.Document_NotExists.ToId(), this._messageDescriptions[MessageType.Document_NotExists.ToId()].ToString());
//                }
//            }
//            catch (Exception ex)
//            {
//                fileData = Encoding.Unicode.GetBytes(this._messageDescriptions[MessageType.Document_NotExists.ToId()].ToString());
//                _logger.LogError(ResponseType.Exception.ToId(), ex.Message);
//            }
//            return this.GenerateFileResult(fileName, fileMimeType, fileData);
//        }

//        [HttpPost]
//        [Route("Copy")]
//        public Response GenerateCopyFile([FromBody] DocumentCopyModel copyModel, [FromRoute]string moduleType, [FromQuery]string code)
//        {
//            return this._documentService.Copy(moduleType.ToEnum<ModuleCodeType>(), code, copyModel.Id);
//        }

//        [HttpDelete]
//        [Route("Delete")]
//        public Response DeleteDocument([FromRoute]string moduleType, [FromQuery]string code, [FromBody] DocumentCopyModel copyModel)
//        {
//            var document = new List<ModuleDocument>()
//            {
//                new ModuleDocument()
//                {
//                    ModuleCode=moduleType,
//                    ModuleRefCode=code,


//                };
//            };
//            return this._documentService.Delete(moduleType.ToEnum<ModuleCodeType>(), code, copyModel.Id);
//        }
        
//        private FileContentResult GenerateFileResult(string fileName, string mimeType, byte[] data)
//        {
//            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
//            {
//                FileName = fileName,
//                Inline = false
//            };
//            Response.Headers.Add("Content-Disposition", cd.ToString());
//            Response.Headers.Add("X-Content-Type-Options", "nosniff");
//            return new FileContentResult(data, mimeType);
//        }
//    }

//    public class DocumentCopyModel
//    {
//        public string Id { get; set; }
//    }
//}