using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.Admin.Domain.Models.Admins;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
 public   interface ITechnicalSpecialistUsersRepository : IGenericRepository<DbModel.User>
    {
        IList<User> Search(User searchModel);
    }

}
