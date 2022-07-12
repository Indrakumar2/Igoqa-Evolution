using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ICommodityService
    {
        Response Search(Models.Commodity search);

        bool IsValidCommodityName(IList<string> names, ref IList<DbModel.Data> dbCommodities, ref IList<ValidationMessage> validMessages);
    }
}
