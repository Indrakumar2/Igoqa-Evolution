using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ICityService
    {
        Response Save(IList<Models.City> datas);

        Response Search(Models.City search);

        Response Modify(IList<Models.City> datas);

        bool IsValidCity(IList<string> names,
                                    IList<DbModel.County> dbConties,
                                    ref IList<DbModel.City> dbCities,
                                    ref IList<ValidationMessage> valdMessages,
                                    params Expression<Func<DbModel.City, object>>[] includes);
    }
}
