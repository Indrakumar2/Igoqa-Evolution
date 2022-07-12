using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ITechnicalSpecialistStampCountryCodeService : IMasterService
    {
        Response Search(Models.TechnicalSpecialistStampCountryCode search);

        bool IsValidTechnicalSpecialistStampCountryCode(IList<string> stampCountries,
                                         ref IList<DbModel.Data> dbTechSpecStampCountries,
                                         ref IList<ValidationMessage> messages);
    }
}
