
using System.Collections.Generic;
using Evolution.AuditLog.Domain.Models.Audit;
using Evolution.GenericDbRepository.Interfaces;
using DbModel=Evolution.DbRepository.Models.SqlDatabaseContext;
namespace Evolution.AuditLog.Domain.Interfaces.Data
{
    public interface IAuditSearchRepository: IGenericRepository<DbModel.AuditSearch> 
    {
        IList<AuditSearch> Search (AuditSearch searchModel);
        IList<DbModel.SqlauditModule> GetAuditModule(IList<string> moduleList);
    }
}


