using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IStateService
    {
        Response Save(IList<Models.State> counties);

        Response Search(Models.State search);

        Response Modify(IList<Models.State> counties);

        Response Delete(IList<Models.State> counties);

        bool IsValidCounty(IList<string> names,
                           ref IList<DbModel.County> dbCounties,
                           ref IList<ValidationMessage> valdMessages,
                           params Expression<Func<DbModel.County, object>>[] includes);
    }
}
