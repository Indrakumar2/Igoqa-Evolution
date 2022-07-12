using Evolution.Api.Controllers.Base;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Responses;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evolution.Api.Controllers.TechnicalSpecialist
{
    [Route("api/technicalSpecialists/{ePin}/Notes")]
    [ApiController]
    public class TechnicalSpecialistNotesController : BaseController
    {
        private readonly ITechnicalSpecialistNoteService _technicalSpecialistNoteService = null;
        private readonly IAppLogger<TechnicalSpecialistNotesController> _logger = null;

        public TechnicalSpecialistNotesController(ITechnicalSpecialistNoteService technicalSpecialistNoteService, IAppLogger<TechnicalSpecialistNotesController> logger)
        {
            _logger = logger;
            _technicalSpecialistNoteService = technicalSpecialistNoteService;
        }

        [HttpGet]
        public Response Get([FromRoute] int ePin, [FromQuery] TechnicalSpecialistNoteInfo searchModel)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                searchModel.Epin = ePin;
                return this._technicalSpecialistNoteService.Get(searchModel);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin, searchModel });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }

        [HttpPost]
        public Response Post([FromRoute] int ePin, [FromBody]IList<TechnicalSpecialistNoteInfo> notes)
        {
            Exception exception = null;
            ResponseType responseType = ResponseType.Success;
            try
            {
                notes.ToList().ForEach(x => x.Epin = ePin);
                AssignValues(notes, ValidationType.Add);
                return this._technicalSpecialistNoteService.Add(notes);
            }
            catch (Exception ex)
            {
                exception = ex;
                responseType = ResponseType.Exception;
                //Console.WriteLine(ex.ToFullString());
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), new { ePin , notes });
            }
            return new Response().ToPopulate(responseType, null, null, null, null, exception);
        }
        private void AssignValues(IList<TechnicalSpecialistNoteInfo> model, ValidationType validationType)
        {
            ObjectExtension.SetPropertyValue(model, "ActionByUser", UserName);
            //if (validationType != ValidationType.Add)
            //    ObjectExtension.SetPropertyValue(model, "ModifiedBy", UserName);
        }

    }
}
