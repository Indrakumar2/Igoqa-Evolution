using Evolution.Common.Models.Responses;
using System.Collections.Generic;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface IModuleTypeService
    {
        Response Search(Models.MasterModuleTypes search);
    }
}
