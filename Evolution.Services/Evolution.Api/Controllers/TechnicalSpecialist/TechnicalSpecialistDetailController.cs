using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Base;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Evolution.Api.Controllers.TechnicalSpecialist
{
    [Route("/api/TechnicalSpecialists/{ePin}/detail")]
    public class TechnicalSpecialistDetailController : BaseController
    {
        private readonly ITechnicalSpecialistDetailService _tsDetailService = null;
        private readonly AppEnvVariableBaseModel _environment = null;
        private readonly Newtonsoft.Json.Linq.JObject _messages = null;
        private readonly IAppLogger<TechnicalSpecialistDetailController> _logger = null;

        public TechnicalSpecialistDetailController(ITechnicalSpecialistDetailService service,
            IOptions<AppEnvVariableBaseModel> environment,
            Newtonsoft.Json.Linq.JObject messages,
            IAppLogger<TechnicalSpecialistDetailController> logger)
        {
            _logger = logger;
            _tsDetailService = service;
            _environment = environment.Value;
            _messages = messages;
        }

        [HttpPost]
        public Response Post([FromBody] TechnicalSpecialistDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (model != null && model?.TechnicalSpecialistInfo != null)
                {
                    AssignValuesFromToken(ref model);
                    AssignValues(model, ValidationType.Add);
                    return _tsDetailService.Add(model, isPayloadValidationRequired: !string.Equals(ApplicationAudienceCode, _environment.Evolution2_SPA_Client_Audience_code));
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { model });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPut]
        public Response Put([FromBody] TechnicalSpecialistDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                if (model != null && model?.TechnicalSpecialistInfo != null)
                {
                    //AssignValuesFromToken(ref model); --- Commented for audit functionality 
                    model.TechnicalSpecialistInfo.AssignedByUser = UserName;
                    model.TechnicalSpecialistInfo.ActionByUser = UserName;
                    //model.TechnicalSpecialistInfo.CreatedBy = UserName; ---Not needed for update 

                    AssignValues(model, ValidationType.Update);
                    return _tsDetailService.Modify(model, isPayloadValidationRequired: !string.Equals(ApplicationAudienceCode, _environment.Evolution2_SPA_Client_Audience_code)); ;
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
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { model });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpDelete]
        public Response Delete([FromBody] TechnicalSpecialistDetail model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                AssignValuesFromToken(ref model);
                AssignValues(model, ValidationType.Delete);
                return _tsDetailService.Delete(model, isPayloadValidationRequired: !string.Equals(ApplicationAudienceCode, _environment.Evolution2_SPA_Client_Audience_code));
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

        private void AssignValuesFromToken(ref TechnicalSpecialistDetail model)
        {
            model.TechnicalSpecialistInfo.AssignedByUser = UserName;
            model.TechnicalSpecialistInfo.ActionByUser = UserName;
            model.TechnicalSpecialistInfo.CreatedBy = UserName;
            model.TechnicalSpecialistInfo.ActionByUser = UserName;

        }
        private void AssignValues(TechnicalSpecialistDetail model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistCertification, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistCodeAndStandard, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistCommodityAndEquipment, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistCompetancy, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistComputerElectronicKnowledge, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistContact, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistCustomerApproval, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistDocuments, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistEducation, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistInfo, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistInternalTraining, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistLanguageCapabilities, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistNotes, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistPayRate, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistPaySchedule, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistStamp, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistTaxonomy, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistTraining, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistWorkHistory, "ActionByUser", UserName);
            ObjectExtension.SetPropertyValue(model.TechnicalSpecialistSensitiveDocuments, "ActionByUser", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistCertification, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistCodeAndStandard, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistCommodityAndEquipment, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistCompetancy, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistComputerElectronicKnowledge, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistContact, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistCustomerApproval, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistDocuments, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistSensitiveDocuments, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistEducation, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistInfo, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistInternalTraining, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistLanguageCapabilities, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistNotes, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistPayRate, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistPaySchedule, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistStamp, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistTaxonomy, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistTraining, "ModifiedBy", UserName);
            if (validationType != ValidationType.Add)
                ObjectExtension.SetPropertyValue(model.TechnicalSpecialistWorkHistory, "ModifiedBy", UserName);
        }
    }
}