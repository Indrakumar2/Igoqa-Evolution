using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IProfileStatusService
    {
        Response Search(Models.ProfileStatus search);
        
        bool IsValidProfileStatusName(IList<string> names, ref IList<DbModel.Data> dbStatuses, ref IList<ValidationMessage> valdMessages);
    }
}
