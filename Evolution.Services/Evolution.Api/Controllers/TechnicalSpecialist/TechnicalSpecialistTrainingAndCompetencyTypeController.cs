using System.Collections.Generic;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.AspNetCore.Mvc;

namespace Evolution.Api.Controllers.TechnicalSpecialist
{
    [Route("api/technicalSpecialists/{ePin}/TechnicalSpecialistTrainingAndCompetency/{recordType}/techTrainingAndCompetency/{TechnicalSpecialistTrainingAndCompetencyId}/TechnicalSpecialistTrainingAndCompetencyType")]
    [ApiController]
    public class TechnicalSpecialistTrainingAndCompetencyTypeController : ControllerBase
    {
        //private readonly ITechnicalSpecialistTrainingAndCompetencyTypeService _technicalSpecialistTrainingAndCompetencyTypeService = null;

        //public TechnicalSpecialistTrainingAndCompetencyTypeController(ITechnicalSpecialistTrainingAndCompetencyTypeService technicalSpecialistTrainingAndCompetencyTypeService)
        //{
        //    _technicalSpecialistTrainingAndCompetencyTypeService = technicalSpecialistTrainingAndCompetencyTypeService;

        //}
        //[HttpGet]
        //public Response Get([FromRoute] int ePin, [FromRoute] int TechnicalSpecialistTrainingAndCompetencyId, [FromQuery] TechnicalSpecialistTrainingAndCompetencyType searchModel)
        //{
        //    searchModel.TechnicalSpecialistTrainingAndCompetencyId = TechnicalSpecialistTrainingAndCompetencyId;
        //    searchModel.Epin = ePin;
        //    return this._technicalSpecialistTrainingAndCompetencyTypeService.GetTechnicalSpecialistTrainingAndCompetencyType(searchModel);
        //}
        //[HttpPost]
        //public Response SaveTechnicalSpecialistTrainingAndCompetencyType([FromRoute] int ePin, [FromBody] IList<TechnicalSpecialistTrainingAndCompetencyType> technicalSpecialistTrainingAndCompetencyTypes, [FromRoute] string recordType)
        //{
        //    return this._technicalSpecialistTrainingAndCompetencyTypeService.SaveTechnicalSpecialistTrainingAndCompetencyType(ePin,technicalSpecialistTrainingAndCompetencyTypes, recordType);
          
        //}
        //[HttpPut]
        //public Response ModifyTechnicalSpecialistTrainingAndCompetencyType([FromRoute] int ePin, [FromBody] IList<TechnicalSpecialistTrainingAndCompetencyType> technicalSpecialistTrainingAndCompetencyTypes, [FromRoute] string recordType)
        //{
        //    return this._technicalSpecialistTrainingAndCompetencyTypeService.ModifyTechnicalSpecialistTrainingAndCompetencyType(ePin,technicalSpecialistTrainingAndCompetencyTypes, recordType);

        //}

        //[HttpDelete]
        //public Response DeleteTechnicalSpecialistTrainingAndCompetencyType([FromRoute] int ePin, [FromBody] IList<TechnicalSpecialistTrainingAndCompetencyType> technicalSpecialistTrainingAndCompetencyTypes, [FromRoute] string recordType)
        //{
        //    return this._technicalSpecialistTrainingAndCompetencyTypeService.DeleteTechnicalSpecialistTrainingAndCompetencyType(ePin,technicalSpecialistTrainingAndCompetencyTypes, recordType);

        //}
    }
}