using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistDetailService
    { 
        Response Get(TechnicalSpecialistDetail techSpecialistDetail);

        Response Add(TechnicalSpecialistDetail techSpecialistDetail, bool commitChange = true, bool isPayloadValidationRequired = false);

        Response Modify(TechnicalSpecialistDetail techSpecialistDetail, bool commitChange = true, bool isPayloadValidationRequired = false);

        Response Delete(TechnicalSpecialistDetail techSpecialistDetail, bool commitChange = true, bool isPayloadValidationRequired = false);
    }
}
