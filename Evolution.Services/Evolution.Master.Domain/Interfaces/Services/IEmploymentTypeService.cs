using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IEmploymentTypeService
    {
        Response Search(Models.EmploymentType search);

        bool IsValidEmploymentName(IList<string> names, ref IList<DbModel.Data> dbEmploymentTypes, ref IList<ValidationMessage> valdMessages);
    }
}
