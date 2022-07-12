using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Timesheet.Domain.Interfaces.Timesheets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Model = Evolution.Timesheet.Domain.Models.Timesheets;
using System.Collections.Generic;
using System;
using Evolution.Logging.Interfaces;

namespace Evolution.Api.Controllers.Timesheet
{
    [Route("api/timesheets/{timesheetId}/detail")]
    public class TimesheetDetailController : BaseController
    {
        private readonly ITimesheetDetailService _service = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly Newtonsoft.Json.Linq.JObject _messages = null;
        private readonly IAppLogger<TimesheetDetailController> _logger = null;

        public TimesheetDetailController(ITimesheetDetailService service, IOptions<AppEnvVariableBaseModel> environment,
                                        Newtonsoft.Json.Linq.JObject messages, IAppLogger<TimesheetDetailController> logger)
        {
            this._service = service;
            _environment = environment.Value;
            _messages = messages;
            _logger = logger;
        }

        [HttpGet]
        [Route("TechnicalSpecialistWithGrossMargin")]
        public Task<Response> GetTechnicalSpecialistWithGrossMargin([FromRoute]long timesheetId, [FromQuery]Model.TimesheetTechnicalSpecialist searchModel)
        {
            //Exception exception = null;
            //ResponseType responseType = ResponseType.Success;
            //try
            //{
            searchModel.TimesheetId = timesheetId;
            return this._service.GetTechnicalSpecialistWithGrossMargin(searchModel);
            //}
            //catch (Exception ex)
            //{
            //    exception = ex;
            //    responseType = ResponseType.Exception;
            //    _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), timesheetId);
            //}
            //return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromBody]Model.TimesheetDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (model != null && model.TimesheetInfo != null)
                {
                    AssignValues(model, ValidationType.Add);
                    return _service.Add(model, ApplicationAudienceCode != _environment.Evolution2_SPA_Client_Audience_code);
                }
                else
                {
                    return new Response().ToPopulate(ResponseType.Success, null, null, new List<ValidationMessage> { new ValidationMessage(model, new List<MessageDetail> { new MessageDetail(ModuleType.Timesheet, MessageType.InvalidPayLoad.ToId(), _messages[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody]Model.TimesheetDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (model != null && model.TimesheetInfo != null)
                {
                    AssignValues(model, ValidationType.Update);
                    return _service.Modify(model, ApplicationAudienceCode != _environment.Evolution2_SPA_Client_Audience_code);
                }
                else
                {
                    return new Response().ToPopulate(ResponseType.Success, null, null, new List<ValidationMessage> { new ValidationMessage(model, new List<MessageDetail> { new MessageDetail(ModuleType.Timesheet, MessageType.InvalidPayLoad.ToId(), _messages[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody]Model.TimesheetDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (model != null && model.TimesheetInfo != null)
                {
                    AssignValues(model, ValidationType.Delete);
                    return _service.Delete(model, ApplicationAudienceCode != _environment.Evolution2_SPA_Client_Audience_code);
                }
                else
                {
                    return new Response().ToPopulate(ResponseType.Success, null, null, new List<ValidationMessage> { new ValidationMessage(model, new List<MessageDetail> { new MessageDetail(ModuleType.Timesheet, MessageType.InvalidPayLoad.ToId(), _messages[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), model);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        [Route("approve")]
        public Response ApproveTimesheet([FromBody]Model.TimesheetEmailData TimesheetEmailData)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.ApproveTimesheet(TimesheetEmailData);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), TimesheetEmailData);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        [Route("reject")]
        public Response RejectTimesheet([FromBody]Model.TimesheetEmailData TimesheetEmailData)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _service.RejectTimesheet(TimesheetEmailData);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), TimesheetEmailData);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        [HttpPost]
        [Route("customerReportNotification")]
        public Response CustomerReportNotification([FromBody]Model.CustomerReportingNotification notificationData)
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

        private void AssignValues(Model.TimesheetDetail model, ValidationType validationType)
        {
            if (model != null)
            {
                ObjectExtension.SetPropertyValue(model.TimesheetInfo, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.TimesheetNotes, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.TimesheetReferences, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.TimesheetTechnicalSpecialistConsumables, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.TimesheetTechnicalSpecialistExpenses, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.TimesheetTechnicalSpecialists, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.TimesheetTechnicalSpecialistTimes, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.TimesheetTechnicalSpecialistTravels, "ActionByUser", UserName);
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistCalendarList, "ActionByUser", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.TimesheetInfo, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.TimesheetNotes, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.TimesheetReferences, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.TimesheetTechnicalSpecialistConsumables, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.TimesheetTechnicalSpecialistExpenses, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.TimesheetTechnicalSpecialists, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.TimesheetTechnicalSpecialistTimes, "ModifiedBy", UserName);
                if (validationType != ValidationType.Add)
                    ObjectExtension.SetPropertyValue(model.TimesheetTechnicalSpecialistTravels, "ModifiedBy", UserName);
            }
        }
    }
}