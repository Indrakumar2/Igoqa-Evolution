using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Models;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ICommodityEquipmentService 
    {
        Response Search(CommodityEquipment search);
    }
}
