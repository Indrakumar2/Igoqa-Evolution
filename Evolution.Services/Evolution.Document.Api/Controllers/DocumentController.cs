using Evolution.Admin.Domain.Interfaces.Admins;
using Evolution.Admin.Domain.Models.Admins;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Filters;
using Evolution.Common.Helpers;
using Evolution.Common.Models.Responses;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.Document.Domain.Models.Document;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Evolution.Document.Api.Controllers
{
    [Route("api/documents/")]
    public class DocumentController : BaseController
    {
        private readonly IDocumentService _documentService = null;
        private readonly IAppLogger<DocumentController> _logger = null;
        private readonly JObject _messageDescriptions = null;
        private readonly IBatchService _batchService = null;
        private readonly ITechnicalSpecialistService _technicalSpecialistService = null;

        public DocumentController(IDocumentService documentService,
                                    IAppLogger<DocumentController> logger,
                                    IBatchService batchService,
                                    ITechnicalSpecialistService technicalSpecialistService)
        {
            _documentService = documentService;
            _logger = logger;
            _batchService = batchService;
            _technicalSpecialistService = technicalSpecialistService;
            this._messageDescriptions = JObject.Parse(System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Message_EN.json")));
        }


        [HttpPost]
        [Route("GenerateCV")]
        public Response GetTechnicalSpecialistCVs([FromBody] Batches batchProcInfo)
        {
            Exception exception = null;
            IList<KeyValuePair<string, byte[]>> techSpecDetails = new List<KeyValuePair<string, byte[]>>();
            try
            {
                if (batchProcInfo != null)
                {
                    batchProcInfo.ProcessStatus = 1;
                    _batchService.Update(batchProcInfo);
                    BatchProcessParam reportParam = batchProcInfo.ReportParam.DeSerialize<BatchProcessParam>(SerializationType.JSON);
                    foreach (long pin in reportParam?.Epins)
                    {
                        if (reportParam.IsChevron)
                        {
                            techSpecDetails.Add(new KeyValuePair<string, byte[]>(
                                string.Format("Evolution2-{0}_Chevron_CV.doc", pin),
                                (byte[])this._technicalSpecialistService.GetTechnicalSpecialistChevronCV(pin).Result
                                ));
                        }
                        else
                        {
                            techSpecDetails.Add(new KeyValuePair<string, byte[]>(
                               string.Format("Evolution2-{0}_Exported_CV.doc", pin),
                               (byte[])this._technicalSpecialistService.GetTechnicalSpecialistExportCV(pin).Result
                               ));
                        }
                    }
                }

                if (techSpecDetails?.Count() > 0)
                {
                    return _batchService.GenerateResourceCVs(techSpecDetails, batchProcInfo);
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), batchProcInfo);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception);
        }




        [HttpPost]
        [DisableFormValueModelBinding]
        [Route("{moduleType}/UploadFileAsStream")]
        public async Task<Response> UploadStreamFile([FromRoute]string moduleType, [FromQuery]string referenceCode, [FromQuery]string documentUniqueName)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return await this._documentService.UploadByStreamAsync(Request, moduleType.ToEnum<ModuleCodeType>(ModuleCodeType.None, false), referenceCode?.Trim(), documentUniqueName?.Trim());
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { moduleType, referenceCode, documentUniqueName });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("UniqueName")]
        public Response DocumentUniqueName([FromBody]IList<DocumentUniqueNameDetail> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                model?.SetPropertyValue(nameof(DocumentUniqueNameDetail.RequestedBy), UserName);
                return this._documentService.GenerateDocumentUniqueName(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { model });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("{moduleType}/Paste")]
        public Response DocumentPaste([FromRoute]string moduleType, [FromQuery]string referenceCode, [FromQuery]string copyDocumentUniqueName)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._documentService.Paste(moduleType.ToEnum<ModuleCodeType>(), referenceCode, copyDocumentUniqueName);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { moduleType, referenceCode, copyDocumentUniqueName });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        [Route("ChangeStatus")]
        public Response ChangeDocumentStatus([FromBody] IList<ModuleDocument> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                model?.SetPropertyValue(nameof(ModuleDocument.ModifiedBy), UserName);
                return this._documentService.ChangeDocumentStatus(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { model });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("{moduleType}/download")]
        public FileContentResult DownloadDocument([FromRoute]string moduleType, [FromQuery]string documentUniqueName)
        {
            string fileName = "notfound.txt";
            byte[] fileData = null;
            try
            {
                fileData = System.Text.Encoding.Unicode.GetBytes(this._messageDescriptions[MessageType.Document_NotExists.ToId()].ToString());

                if (!string.IsNullOrEmpty(moduleType) && !string.IsNullOrEmpty(documentUniqueName))
                {
                    var downloadDocData = this._documentService.Download(moduleType.ToEnum<ModuleCodeType>(), documentUniqueName);
                    if (downloadDocData?.FileContent != null)
                    {
                        fileName = downloadDocData.DocumentName;
                        fileData = downloadDocData.FileContent;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.Message);
            }

            return this.GenerateFileResult(fileName, fileData);
        }

        [HttpGet]
        public Response Get([FromQuery]ModuleDocument model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._documentService.Get(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { model });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("GetCustomerName")]
        public Response Get([FromQuery]string moduleCode, [FromQuery]string moduleRefCode)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._documentService.Get(moduleCode, moduleRefCode);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { moduleCode, moduleRefCode });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromBody]IList<ModuleDocument> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                model?.SetPropertyValue(nameof(ModuleDocument.CreatedBy), UserName);
                return this._documentService.Save(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { model });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody] IList<ModuleDocument> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                model?.SetPropertyValue(nameof(ModuleDocument.ModifiedBy), UserName);
                return this._documentService.Modify(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { model });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody] IList<ModuleDocument> model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._documentService.Delete(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { model });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("UploadEmailDocuments")]
        public Response UploadEmailDocuments([FromBody]EmailDocument model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._documentService.UploadEmailDocuments(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { model });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        [Route("UploadDocuments")]
        public Response UploadDocuments([FromBody]EmailDocumentUpload model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._documentService.UploadEmailDocuments(model);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { model });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        //def 1289
        [HttpPost]
        [Route("EmailAttachmentDocuments")]
        public IList<DocumentDownloadResult> UploadDocuments([FromBody]IList<string> documentUniqueNames)
        {
            Exception exception = null; 
            try
            {
                return this._documentService.GetEmailAttachmentDocuments(documentUniqueNames);
            }
            catch (Exception ex)
            {
                exception = ex; 
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { documentUniqueNames });
            }
            return null;
        }

       /* [HttpPost]
        [Route("GenerateCV")]
        public Response GetTechnicalSpecialistCVs([FromBody] Batches batchProcInfo)
        { 
            Exception exception = null;
            IList<KeyValuePair<string, byte[]>> techSpecDetails = new List<KeyValuePair<string, byte[]>>();
            try
            {
                if (batchProcInfo != null)
                {
                    batchProcInfo.ProcessStatus = 1;
                    _batchService.Update(batchProcInfo);
                    BatchProcessParam reportParam =  batchProcInfo.ReportParam.DeSerialize<BatchProcessParam>(SerializationType.JSON);
                    foreach (long pin in reportParam?.Epins)
                    {
                        if (reportParam.IsChevron)
                        {
                            techSpecDetails.Add(new KeyValuePair<string, byte[]>(
                                string.Format("Evolution2-{0}_Chevron_CV.doc", pin),
                                (byte[])this._technicalSpecialistService.GetTechnicalSpecialistChevronCV(pin).Result
                                ));
                        }
                        else
                        {
                            techSpecDetails.Add(new KeyValuePair<string, byte[]>(
                               string.Format("Evolution2-{0}_Exported_CV.doc", pin),
                               (byte[])this._technicalSpecialistService.GetTechnicalSpecialistExportCV(pin).Result
                               ));
                        }
                    }
                }
                   
                if (techSpecDetails?.Count() > 0)
                {
                    return _batchService.GenerateResourceCVs(techSpecDetails, batchProcInfo);
                }
                    
            }
            catch (Exception ex)
            {
                exception = ex; 
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), batchProcInfo);
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, null, null, exception); 
        }*/

        private FileContentResult GenerateFileResult(string fileName, byte[] data)
        {
            try
            {
                string mimeType = Utility.GetMimeTypes(new FileInfo(fileName)?.Extension);
                if (string.IsNullOrEmpty(mimeType))//def205
                {
                    data = System.Text.Encoding.Unicode.GetBytes(this._messageDescriptions[MessageType.Document_Not_Supported.ToId()].ToString());
                    fileName = "FileNotSupported.txt";
                    mimeType = Utility.GetMimeTypes(new FileInfo(fileName)?.Extension);
                }
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = Uri.EscapeDataString(fileName),//def 1357- non ascii char in file name issue fix
                    Inline = false
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());
                Response.Headers.Add("X-Content-Type-Options", "nosniff");
                Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition,X-Suggested-Filename");
                return new FileContentResult(data, mimeType);
            }
            catch (Exception)
            { 
                throw;
            }
        }
    }

    public class BatchProcessParam
    {
        public IEnumerable<long> Epins { get; set; }
        public bool IsChevron { get; set; }
    }
}