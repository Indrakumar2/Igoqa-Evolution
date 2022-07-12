using Evolution.Common.Enums;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public  interface ICountryService
    {
        Response Save(IList<Models.Country> datas);

        Response Search(Models.Country search);

        Response Modify(IList<Models.Country> datas);

        Response Delete(IList<Models.Country> datas);

        Response GetCountryEUVatPrefix();

        bool IsValidCountryName(IList<string> names,
                                ref IList<DbModel.Country> dbCountries,
                                ref IList<ValidationMessage> valdMessages,
                                params Expression<Func<DbModel.Country, object>>[] includes);

        bool IsValidCountryName(IList<string> names,
                                        ref IList<DbModel.Country> dbCountries,
                                        ref IList<ValidationMessage> valdMessages,
                                        string[] includes);
        Response IsRecordValidForProcess(IList<Models.Country> datas, ValidationType validationType,IList<Models.Country> filterData,ref IList<DbModel.Country> dbCountries,ref IList<DbModel.Data>dbRegions);
    }
}
