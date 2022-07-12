using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IEquipmentService
    {
        Response Search(Models.Equipment search);

        bool IsValidEquipmentName(IList<string> names, ref IList<DbModel.Data> dbEquipments, ref IList<ValidationMessage> validMessages);
    }
}
