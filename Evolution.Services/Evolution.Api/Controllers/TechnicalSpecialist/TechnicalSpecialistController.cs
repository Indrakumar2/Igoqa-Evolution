using Evolution.Admin.Domain.Interfaces.Admins;
using Evolution.Admin.Domain.Models.Admins;
using Evolution.Api.Controllers.Base;
using Evolution.Common.Constants;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;


namespace Evolution.Api.Controllers.TechnicalSpecialist
{
    [Route("api/TechnicalSpecialists")]
    [ApiController]
    public class TechnicalSpecialistController : BaseController
    {
        private readonly ITechnicalSpecialistService _technicalSpecialistService = null;
        private readonly IAppLogger<TechnicalSpecialistController> _logger = null;
        private readonly IBatchService _batchService;
       
        private readonly IUserService _userService;
        private readonly AppEnvVariableBaseModel _environment;
        private readonly Newtonsoft.Json.Linq.JObject _messages = null;
        public readonly string _batchProcessEndpoint = "documents/GenerateCV";


        public TechnicalSpecialistController(ITechnicalSpecialistService technicalSpecialistService,
            IBatchService batchService,
            IUserService userService,
            Newtonsoft.Json.Linq.JObject messages,
            IOptions<AppEnvVariableBaseModel> environment,
            IAppLogger<TechnicalSpecialistController> logger)
        {
            _logger = logger;
            _technicalSpecialistService = technicalSpecialistService;
            _batchService = batchService;
            _userService = userService;
            _environment = environment.Value;
            _messages = messages;
        }

        [HttpGet]
        public Response Get([FromQuery] DomainModel.TechnicalSpecialistInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._technicalSpecialistService.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { searchModel });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("Info")]
        public Response Get([FromQuery] DomainModel.BaseTechnicalSpecialistInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._technicalSpecialistService.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { searchModel });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("BasicInfo")]
        public Response Get([FromQuery] string companyCode, [FromQuery] string logonName)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return this._technicalSpecialistService.GetResourceBasicInfo(companyCode, logonName);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { companyCode });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("Search")]
        public Response GetTechnicalSpecialist([FromQuery] DomainModel.SearchTechnicalSpecialist searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return Task.Run<Response>(async () => await this._technicalSpecialistService.Get(searchModel)).Result;
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { searchModel });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpGet]
        [Route("ExportCV")]
        public FileResult GetTechnicalSpecialistCV(long techspecialistEpin, bool isChevron = false)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                byte[] techSpecDetails = null;
                string fileName = string.Empty;

                if (isChevron)
                {
                    techSpecDetails = (byte[])this._technicalSpecialistService.GetTechnicalSpecialistChevronCV(techspecialistEpin).Result;
                    fileName = string.Format("Evolution2-{0}_Chevron_CV.doc", techspecialistEpin);//D793
                }
                else
                {
                    techSpecDetails = (byte[])this._technicalSpecialistService.GetTechnicalSpecialistExportCV(techspecialistEpin).Result;
                    fileName = string.Format("Evolution2-{0}_Exported_CV.doc", techspecialistEpin);//D793
                }
                Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition,X-Suggested-Filename");
                return File(techSpecDetails, TechnicalSpecialistConstants.File_Export_Format, fileName);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { techspecialistEpin, isChevron });
            }
            return File(ByteExtension.ObjectToByteArray("Unable to export file there is an exception. Please contact to administrator."), TechnicalSpecialistConstants.File_Export_Format, responseType.ToString());
        }

        [HttpPost]
        public Response Post([FromBody] IList<DomainModel.TechnicalSpecialistInfo> technicalSpecialists)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref technicalSpecialists);
                return this._technicalSpecialistService.Add(technicalSpecialists);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { technicalSpecialists });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);

        }

        [HttpPut]
        public Response Put([FromBody] IList<DomainModel.TechnicalSpecialistInfo> technicalSpecialists)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref technicalSpecialists);
                return this._technicalSpecialistService.Modify(technicalSpecialists);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { technicalSpecialists });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody] IList<DomainModel.TechnicalSpecialistInfo> technicalSpecialists)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref technicalSpecialists);
                return this._technicalSpecialistService.Delete(technicalSpecialists);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { technicalSpecialists });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        [HttpPost]
        [Route("GetTSBasedOnCompany")]
        public Response GetTSBasedOnCompany([FromBody] IList<int> companyCodes, [FromQuery] bool isActive)
        {
            return this._technicalSpecialistService.GetTSBasedOnCompany(companyCodes, isActive);
        }



        [HttpPost]
        [Route("ExportCV")]
        public void GenerateTechnicalSpecialistCVZip([FromBody] IEnumerable<long> epins, [FromQuery] bool isChevron, [FromQuery] int exportCVFrom)
        {

            ResponseType responseType = ResponseType.Success;
            IList<KeyValuePair<string, byte[]>> techSpecDetails = new List<KeyValuePair<string, byte[]>>();

            if (epins == null || epins?.Count() == 0 || (exportCVFrom >= 5 || exportCVFrom <= 7))
            {

                var userId = (int)_userService.GeUser(UserName).Result;

                var reportID = _batchService.GetLastReportID();
                reportID++;
                Batches objBatches = new Batches
                {
                    BatchID = 3,//Generate Resorce cv zip in background
                    ParamID = userId,
                    ProcessStatus = 0,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = UserName,
                    ReportType = exportCVFrom,
                    IsDeleted = false,
                    ReportParam = new { Epins = epins, IsChevron = isChevron }.Serialize(SerializationType.JSON)
                };
                var result = _batchService.InsertBatch(objBatches);
                if (result.Code == MessageType.Success.ToId())
                {

                    InitiateBatchProcess(result.Result.Populate<Batches>());

                 }
               // return result;
            }
            }

    /*[HttpPost]
        [Route("GenerateBatchForExportCV")]
        public Response GenerateTechnicalSpecialistCVZip([FromBody] IEnumerable<long> epins, [FromQuery] bool isChevron, [FromQuery] int exportCVFrom)
        {

            string cv_fileName = string.Empty;
            string zip_fileName = string.Empty;
            //exportCVFrom : 5 : for quick search export, 6 :for preassignment search export 0, 7 :for ARS search export
            if (exportCVFrom == 5)
            {
                cv_fileName = "Quick Search CV Export";
                zip_fileName = "Quick Search CV Export";
            }
            else if (exportCVFrom == 6)
            {
                cv_fileName = "Evolution2-PreAssignment-";
                zip_fileName = "Pre Assignment CV Export";
            }
            else if (exportCVFrom == 7)
            {
                cv_fileName = "Evolution2-ARS-";
                zip_fileName = "ARS CV Export";
            }
            if (epins == null || epins?.Count() == 0 || (exportCVFrom >= 5 || exportCVFrom <= 7))
            {

                var userId = (int)_userService.GeUser(UserName).Result;

                var reportID = _batchService.GetLastReportID();
                reportID++;
              Batches objBatches = new Batches
            {
                BatchID = 3,//Generate Resorce cv zip in background
                ParamID = userId,
                ProcessStatus = 0,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = UserName,
                ReportType = exportCVFrom,
                ReportFileName = zip_fileName + " " + reportID + ".",
                FileExtension = "zip",
                IsDeleted = false,
                ReportParam = new { Epins = epins, IsChevron = isChevron, ExportCVFrom = exportCVFrom }.Serialize(SerializationType.JSON)
            };
                var result = _batchService.InsertBatch(objBatches);
               *//* if (result.Code == MessageType.Success.ToId())
                {
                    InitiateBatchProcess(result.Result.Populate<Batches>());
            }*//*

                    return result;
        }
        else
            {
               
                return new Response().ToPopulate(ResponseType.Validation, null, null, new List<ValidationMessage> { new ValidationMessage(new { epins, isChevron, exportCVFrom }, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.InvalidPayLoad.ToId(), _messages[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);
         }
  }*/

        [HttpPost]
        [Route("exportCVasZIP")]
        public FileResult GetTechnicalSpecalitCVasZip([FromBody] IEnumerable<long> epins, [FromQuery] bool isChevron, [FromQuery] int exportCVFrom, [FromQuery] int reportid)
        {
            ResponseType responseType = ResponseType.Success;
            IList<KeyValuePair<string, byte[]>> techSpecDetails = new List<KeyValuePair<string, byte[]>>();
            try
            {

                string cv_fileName = string.Empty;
                string zip_fileName = string.Empty;
                //exportCVFrom : 5 : for quick search export, 6 :for preassignment search export 0, 7 :for ARS search export
                if (exportCVFrom == 5)
                {
                    cv_fileName = "Evolution2-Quick Search";
                    zip_fileName = "Quick Search CV Export";
                }
                else if (exportCVFrom == 6)
                {
                    cv_fileName = "Evolution2-PreAssignment-";
                    zip_fileName = "Pre-Assignment CV Export";
                }
                else if (exportCVFrom == 7)
                {
                    cv_fileName = "Evolution2-ARS-";
                    zip_fileName = "ARS CV Export";
                }
                if (epins?.Count() > 0)
                    foreach (long pin in epins)
                    {
                        if (isChevron)
                        {

                            var cvdata = (byte[])this._technicalSpecialistService.GetTechnicalSpecialistChevronCV(pin).Result;
                            techSpecDetails.Add(new KeyValuePair<string, byte[]>(
                                string.Format(cv_fileName+" {0}_Chevron_CV.doc", pin),
                                cvdata
                                ));
                        }
                        else
                        {
                            techSpecDetails.Add(new KeyValuePair<string, byte[]>(
                               string.Format(cv_fileName + "{0}_Exported_CV.doc", pin),
                               (byte[])this._technicalSpecialistService.GetTechnicalSpecialistExportCV(pin).Result
                               ));
                        }
                    }

                if (techSpecDetails?.Count() > 0)
                    using (var compressedFileStream = new MemoryStream())
                    {
                        //Create an archive and store the stream in memory.
                        using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, false))
                        {
                            foreach (var attachmentData in techSpecDetails)
                            {
                                //Create a zip entry for each attachment
                                var zipEntry = zipArchive.CreateEntry(attachmentData.Key);

                                //Get the stream of the attachment
                                using (var originalFileStream = new MemoryStream(attachmentData.Value))
                                using (var zipEntryStream = zipEntry.Open())
                                {
                                    //Copy the attachment stream to the zip entry stream
                                    originalFileStream.CopyTo(zipEntryStream);
                                }
                            }
                        }
                        Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition,X-Suggested-Filename");
                        return new FileContentResult(compressedFileStream.ToArray(), "application/zip") { FileDownloadName = zip_fileName+" "+ reportid +".zip" };
                    }
            }
            catch (Exception ex)
            {
                responseType = ResponseType.Exception;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new
                {
                    epins,
                    isChevron
                });
            }
            return File(ByteExtension.ObjectToByteArray("Unable to export file there is an exception. Please contact to administrator."), TechnicalSpecialistConstants.File_Export_Format, responseType.ToString());

        }


        private void AssignValuesFromToken(ref IList<DomainModel.TechnicalSpecialistInfo> technicalSpecialistInfos)
        {
            technicalSpecialistInfos.ToList().ForEach(x =>
            {
                x.AssignedByUser = UserName;
                x.ActionByUser = UserName;
                x.CreatedBy = UserName;
            });
        }

        private void InitiateBatchProcess(Batches batchProInfo)
        {
            Exception exception = null;
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                // Pass the handler to httpclient(to call api)
                using (var httpClient = new HttpClient(clientHandler))
                {
                    string url = _environment.ApplicationGatewayURL + _batchProcessEndpoint;
                    //string url = "http://localhost:5104/api/" + _batchProcessEndpoint;
                    var uri = new Uri(url);
                    HttpContent content = new StringContent(batchProInfo.Serialize(SerializationType.JSON), Encoding.UTF8, "application/json");

                    var response = httpClient.PostAsync(uri, content);
                    if (!response.Result.IsSuccessStatusCode)
                        _logger.LogError(ResponseType.Exception.ToId(), response?.Result?.ReasonPhrase, batchProInfo);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), batchProInfo);

                batchProInfo.ProcessStatus = 3;//Failed
                batchProInfo.ErrorMessage = ex.Message;
                _batchService.Update(batchProInfo);
            }
        }
    }
}
