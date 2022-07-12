using Evolution.GenericDbRepository.Interfaces;
using Evolution.Master.Domain.Models;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Master.Domain.Interfaces.Data
{
    public interface ICommodityEquipmentRepository : IGenericRepository<DbModel.CommodityEquipment>
    {
        IList<CommodityEquipment> Search(CommodityEquipment search);
    }
}
