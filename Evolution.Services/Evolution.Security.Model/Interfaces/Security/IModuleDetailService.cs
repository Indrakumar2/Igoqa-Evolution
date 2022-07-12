using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Security.Domain.Interfaces.Security
{
    public interface IModuleDetailService
    {
        Response Get(Models.Security.ModuleInfo searchModel);
        
        Response Add(IList<Models.Security.ModuleDetail> moduleDetails);

        Response Modify(IList<Models.Security.ModuleDetail> moduleDetails);

        Response Delete(IList<Models.Security.ModuleDetail> moduleDetails);
    }
}
