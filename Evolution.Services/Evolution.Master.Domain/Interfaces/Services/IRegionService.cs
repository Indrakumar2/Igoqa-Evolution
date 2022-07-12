using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
   public interface IRegionService : IMasterService
    {
        Response Search(Models.Region search);

        bool IsValidRegion(IList<string> names, ref IList<DbModel.Data> dbRegions, ref IList<ValidationMessage> valdMessages);
    }
}
