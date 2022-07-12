using Evolution.GenericDbRepository.Interfaces;
using Evolution.Security.Domain.Models.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Security.Domain.Interfaces.Data
{
    public interface IRoleDetailRepository : IGenericRepository<Evolution.DbRepository.Models.SqlDatabaseContext.RoleActivity>
    {
        IList<RoleDetail> Search(RoleInfo model);

        IList<RoleDetail> Search(IList<string> roleNames);
    }
}
