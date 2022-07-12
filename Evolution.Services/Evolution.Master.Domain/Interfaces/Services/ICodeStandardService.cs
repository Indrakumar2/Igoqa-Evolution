using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ICodeStandardService
    {
        Response Search(Models.CodeStandard search);
        bool IsValidCodeAndStandardName(IList<string> names, ref IList<DbModel.Data> dbcodeAndStandard, ref IList<ValidationMessage> validMessages);
    }
}
