using Evolution.Common.Models.Responses;
using System.Collections.Generic;

namespace Evolution.Security.Domain.Interfaces.Security
{
    public interface IRoleDetailService
    {
        Response Get(Models.Security.RoleInfo searchModel);

        Response Get(IList<string> roleNames);

        Response Add(IList<Models.Security.RoleDetail> roleDetails);

        Response Modify(IList<Models.Security.RoleDetail> roleDetails);

        Response Delete(IList<Models.Security.RoleDetail> roleDetails);
    }
}
