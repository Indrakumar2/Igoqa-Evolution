using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Security.Domain.Models.Security;

namespace Evolution.Security.Domain.Interfaces.Data
{
    public interface IUserTypeRepository : IGenericRepository<DbModel.UserType>
    { 
        IList<DbModel.UserType> Search(DomainModel.UserTypeInfo searchModel, params string[] includes);

        IList<DbModel.UserType> Get(string companyCode, IList<string> userTypes, params string[] includes);

        IList<DbModel.UserType> Get(int companyId, IList<string> userTypes, params string[] includes);
    }
}
