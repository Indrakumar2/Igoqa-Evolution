using Evolution.Common.Models.Responses;
using Evolution.Master.Domain.Models;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ITaxTypeService:IMasterService
    {
        Response Search(TaxType search);
    }
}
