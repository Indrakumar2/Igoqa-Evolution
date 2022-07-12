using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IProfileActionService
    {
        Response Search(Models.ProfileAction search);

        bool IsValidProfileActionName(IList<string> names, ref IList<DbModel.Data> dbActions, ref IList<ValidationMessage> valdMessages);
    }
}
