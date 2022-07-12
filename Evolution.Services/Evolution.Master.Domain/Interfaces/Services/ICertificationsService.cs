using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ICertificationsService
    {
        Response Search(Models.Certifications search);
        bool IsValidCertification(IList<string> certificationNames,
                                         ref IList<DbModel.Data> dbCertifications,
                                         ref IList<ValidationMessage> messages);
    }
}
