using Evolution.Common.Models.Responses;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IDispositionTypeService : IMasterService
    {
        Response Search(Models.DispositionType search);
    }
}
