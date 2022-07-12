using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface INativeCurrencyService:IMasterService
    {
        Response Search(Models.Currency search);

        bool IsValidCurrency(IList<string> currencyCodes,
                                         ref IList<DbModel.Data> dbCurrencies,
                                         ref IList<ValidationMessage> messages);
    }
}
