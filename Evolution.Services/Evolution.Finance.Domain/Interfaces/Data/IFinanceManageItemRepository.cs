using Evolution.GenericDbRepository.Interfaces;
using System.Collections.Generic;
using DBModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Finance.Domain.Models.Finance;

namespace Evolution.Finance.Domain.Interfaces.Data
{
    public interface IFinanceManageItemRepository : IGenericRepository<DBModel.Invoice>
    {
        //IList<DomainModel.Project> Search(DomainModel.ProjectSearch searchModel);
    }
}
