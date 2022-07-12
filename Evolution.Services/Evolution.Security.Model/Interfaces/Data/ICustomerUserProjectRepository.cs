using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using DomainModel = Evolution.Security.Domain.Models.Security;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Interfaces;

namespace Evolution.Security.Domain.Interfaces.Data
{
    public interface ICustomerUserProjectRepository : IGenericRepository<DbModel.CustomerUserProjectAccess>
    {
        //Response Get(IList<DomainModel.CustomerUserProject> userInfo);
    }
}
