using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.Visit.Domain.Interfaces.Visits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Domain = Evolution.Visit.Domain.Models.Visits;
using System;
using System.Collections.Generic;

namespace Evolution.Api.Controllers.Visit
{
    [Route("api/visits/{visitId}/detail")]
    public class VisitDetailController : BaseController
    {
        //TO be removed later once figured out 500 issue
        private readonly string LoggerPrefix = "Test_500_In_35";
        private readonly IVisitDetailService _service = null;
        private readonly IAppLogger<VisitDetailController> _logger = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly Newtonsoft.Json.Linq.JObject _messages = null;

        public VisitDetailController(IVisitDetailService service,
                                    IAppLogger<VisitDetailController> logger, IOptions<AppEnvVariableBaseModel> environment,
                                    Newtonsoft.Json.Linq.JObject messages)
        {
            this._logger = logger;
            this._service = service;
            _environment = environment.Value;
            _messages = messages;
        }
        [HttpGet]
        [Route("TechnicalSpecialistWithGrossMargin")]
        public Task<Response> GetTechnicalSpecialistWithGrossMargin([FromRoute]long visitId, [FromQuery]Domain.VisitTechnicalSpecialist searchModel)
        {
            //Exception exception = null;
            //ResponseType responseType = ResponseType.Success;
            //try
            //{
                searchModel.VisitId = visitId;
                return this._service.GetTechnicalSpecialistWithGrossMargin(searchModel);
            //}
            //catch (Exception ex)
            //{
            //    exception = ex;
            //    responseType = ResponseType.Exception;
            //    //Console.WriteLine(ex.ToFullString());
            //    _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            //}

            //return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromBody]Domain.VisitDetail visitDetail)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (visitDetail != null && visitDetail.VisitInfo != null)
                {
                    AssignValues(visitDetail, ValidationType.Add);
                    return _service.Add(visitDetail, ApplicationAudienceCode != _environment.Evolution2_SPA_Client_Audience_code);
                }
                else
                {
                    return new Response().ToPopulate(ResponseType.Success, null, null, new List<ValidationMessage> { new ValidationMessage(visitDetail, new List<MessageDetail> { new MessageDetail(ModuleType.Visit, MessageType.InvalidPayLoad.ToId(), _messages[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), visitDetail);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody]Domain.VisitDetail visitDetail)
        {
            Response putResponse = null;
            try
            {
                if (visitDetail != null && visitDetail.VisitInfo != null)
                {
                    AssignValues(visitDetail, ValidationType.Update);
                    putResponse = _service.Modify(visitDetail, ApplicationAudienceCode != _environment.Evolution2_SPA_Client_Audience_code);
                }
                else
                {
                    putResponse = new Response().ToPopulate(ResponseType.Success, null, null, new List<ValidationMessage> { new ValidationMessage(visitDetail, new List<MessageDetail> { new MessageDetail(ModuleType.Visit, MessageType.InvalidPayLoad.ToId(), _messages[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);
                }
            }
            catch (Exception ex)
            {
                putResponse = new Response().ToPopulate(ResponseType.Exception, null, null, null, null, ex);
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), LoggerPrefix + ex.ToFullString(), visitDetail);
            }
            return putResponse;

        }

        [HttpDelete]
        public Response Delete([FromBody]Domain.VisitDetail visitDetail)
        {

            Response delResponse = null;
            try
            {
                if (visitDetail != null && visitDetail.VisitInfo != null)
                {
                    AssignValues(visitDetail, ValidationType.Delete);
                    delResponse = _service.Delete(visitDetail, ApplicationAudienceCode != _environment.Evolution2_SPA_Client_Audience_code);
                }
                else
                {
                    delResponse = new Response().ToPopulate(ResponseType.Success, null, null, new List<ValidationMessage> { new ValidationMessage(visitDetail, new List<MessageDetail> { new MessageDetail(ModuleType.Visit, MessageType.InvalidPayLoad.ToId(), _messages[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);
                }
            }
            catch (Exception ex)
            {
                delResponse = new Response().ToPopulate(ResponseType.Exception, null, null, null, null, ex);
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), LoggerPrefix + ex.ToFullString(), visitDetail);
            }
            return delResponse;
        }

        [HttpPut]
        [Route("approve")]
        public Response ApproveVisit([FromBody]Domain.VisitEmailData visitEmailData)
        {
            Response response = null;
            try
            {
                //if status is A and isSendClientReportingNotification === true
                // Then start process Client Reporting Email notifications
                response = _service.ApproveVisit(visitEmailData);
            }
            catch (Exception ex)
            {
                response = new Response().ToPopulate(ResponseType.Exception, null, null, null, null, ex);
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), LoggerPrefix + ex.ToFullString(), visitEmailData);
            }
            return response;
        }

        [HttpPut]
        [Route("reject")]
        public Response RejectVisit([FromBody]Domain.VisitEmailData visitEmailData)
        {
            Response response = null;
            try
            {
                //if status is A and isSendClientReportingNotification === true
                // Then start process Client Reporting Email notifications
                response = _service.RejectVisit(visitEmailData);
            }
            catch (Exception ex)
            {
                response = new Response().ToPopulate(ResponseType.Exception, null, null, null, null, ex);
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), LoggerPrefix + ex.ToFullString(), visitEmailData);
            }
            return response;
        }

        [HttpPost]
        [Route("CustomerReportNotification")]
        public Response CustomerReportNotification([FromBody]Domain.CustomerReportingNotification notificationData)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.ApprovalCustomerReportNotification(notificationData);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), notificationData);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        private void AssignValues(Domain.VisitDetail model, ValidationType validationType)
        {
            if (model != null)
            {
                ObjectExtension.SetPropertyValue(model.VisitInfo, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.VisitNotes, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.VisitSupplierPerformances, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.VisitReferences, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.VisitTechnicalSpecialistConsumables, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.VisitTechnicalSpecialistExpenses, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.VisitTechnicalSpecialists, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.VisitTechnicalSpecialistTimes, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.VisitTechnicalSpecialistTravels, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistCalendarList, "ActionByUser", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.VisitInfo, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.VisitNotes, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.VisitSupplierPerformances, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.VisitReferences, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.VisitTechnicalSpecialistConsumables, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.VisitTechnicalSpecialistExpenses, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.VisitTechnicalSpecialists, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.VisitTechnicalSpecialistTimes, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.VisitTechnicalSpecialistTravels, "ModifiedBy", UserName);
            }
        }

    }
}
