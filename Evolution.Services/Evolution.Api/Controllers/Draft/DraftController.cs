using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Draft.Domain.Interfaces.Draft;
using Evolution.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using DomainModel = Evolution.Draft.Domain.Models;

namespace Evolution.Api.Controllers.Draft
{
    [Route("/api/{Moduletype}/{DraftType}/drafts")]
    public class DraftController : BaseController
    {
        private readonly IDraftService _tsDraftService = null;
        private readonly IAppLogger<DraftController> _logger = null;

        public DraftController(IDraftService tsDraftService, IAppLogger<DraftController> logger)
        {
            _tsDraftService = tsDraftService;
            _logger = logger;
        }
        [HttpGet]
        public Response Get([FromQuery] DomainModel.Draft searchModel, [FromRoute] string moduleType, [FromRoute] string DraftType)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Moduletype = moduleType;
                return this._tsDraftService.GetDraft(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), searchModel);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromBody] object model, [FromQuery]string AssignedToUser, [FromRoute] string moduleType, [FromRoute] string DraftType)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                var draft = new DomainModel.Draft
                {
                    Moduletype = moduleType,
                    SerilizableObject = model?.Serialize(SerializationType.JSON),
                    SerilizationType = SerializationType.JSON.ToString(),
                    AssignedBy = UserName,  // assigning logged in username
                    AssignedTo = AssignedToUser,
                    CreatedBy = UserName,  // assigning logged in username
                    DraftType = DraftType
                };

                return _tsDraftService.SaveDraft(draft);
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
        public Response Put([FromRoute] string draftId, [FromBody] object model)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _tsDraftService.ModifyDraft(model?.Serialize(SerializationType.JSON), draftId);
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
        public Response Delete([FromRoute] string draftId)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                return _tsDraftService.DeleteDraft(draftId);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), draftId);
            }

            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
    }
}