using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Draft.Domain.Interfaces.Draft;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DomainModel = Evolution.Draft.Domain.Models;

namespace Evolution.Api.Controllers.TechnicalSpecialist
{
    [Route("/api/technicalSpecialists/{draftId}/draft")]
    public class DraftController : BaseController
    {
        private readonly IDraftService _tsDraftService = null;
        private readonly IAppLogger<DraftController> _logger = null;
        private readonly Newtonsoft.Json.Linq.JObject _messages = null;

        public DraftController(IDraftService tsDraftService, IAppLogger<DraftController> logger, Newtonsoft.Json.Linq.JObject messages)
        {
            _logger = logger;
            _tsDraftService = tsDraftService;
            _messages = messages;
        }
        [HttpGet]
        public Response Get([FromRoute] string draftId, [FromQuery] DomainModel.Draft searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.DraftId = draftId;
                return this._tsDraftService.GetDraft(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { draftId, searchModel });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromBody] TechnicalSpecialistDetail model, [FromQuery]string AssignedToUser, [FromQuery] DraftType draftType, [FromRoute] string draftId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (model != null)
                { 
                    if (model?.TechnicalSpecialistInfo != null)
                    {
                        model.TechnicalSpecialistInfo.LastModification = System.DateTime.UtcNow;
                    }

                    var draft = new DomainModel.Draft
                    {
                        Moduletype = ModuleCodeType.TS.ToString(),
                        SerilizableObject = model?.Serialize(SerializationType.JSON),
                        SerilizationType = SerializationType.JSON.ToString(),
                        ActionByUser = UserName,
                        AssignedBy = UserName,  // assigning logged in username
                        AssignedTo = AssignedToUser ?? UserName,
                        CreatedBy = UserName,  // assigning logged in username
                        DraftType = draftType.ToString(),
                        DraftId = !string.IsNullOrEmpty(draftId) ? draftId : string.Empty,
                        CompanyCode = model.TechnicalSpecialistInfo.CompanyCode, //D661 issue1 myTask CR
                        PendingWithUser = model.TechnicalSpecialistInfo.PendingWithUser
                    };

                    _tsDraftService.DeleteDraft(draft?.DraftId, draftType, true);
                    return _tsDraftService.SaveDraft(draft);
                }
                else
                {
                    return new Response().ToPopulate(ResponseType.Success, null, null, new List<ValidationMessage> { new ValidationMessage(model, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.InvalidPayLoad.ToId(), _messages[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { model, AssignedToUser, draftType, draftId });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromRoute] string draftId, [FromBody] TechnicalSpecialistDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (model != null)
                {
                     //AssignValues(model, ValidationType.Update);
                     if (model?.TechnicalSpecialistInfo != null)
                    {
                        model.TechnicalSpecialistInfo.LastModification = System.DateTime.UtcNow;
                    }
                    return _tsDraftService.ModifyDraft(model?.Serialize(SerializationType.JSON), draftId);
                }
                else
                {
                return new Response().ToPopulate(ResponseType.Success, null, null, new List<ValidationMessage> { new ValidationMessage(model, new List<MessageDetail> { new MessageDetail(ModuleType.TechnicalSpecialist, MessageType.InvalidPayLoad.ToId(), _messages[MessageType.InvalidPayLoad.ToId()].ToString()) }) }, null, null);
            }
        }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { draftId, model });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromRoute] string draftId, [FromQuery] DraftType draftType)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {

                return _tsDraftService.DeleteDraft(draftId, draftType);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { draftId, draftType });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private void AssignValues(TechnicalSpecialistDetail model, ValidationType validationType)
        {
            // model.ActionByUser = UserName;

            // if (validationType != ValidationType.Add)
            //     ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }
    }
}