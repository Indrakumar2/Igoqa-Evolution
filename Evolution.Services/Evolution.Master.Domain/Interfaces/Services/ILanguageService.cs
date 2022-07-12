using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
  public  interface ILanguageService
    {
        Response Search(Models.Language search);
        bool IsValidLanguage(IList<string> languagecode, ref IList<DbModel.Data> dbCustomers, ref IList<ValidationMessage> messages);

    }
}
