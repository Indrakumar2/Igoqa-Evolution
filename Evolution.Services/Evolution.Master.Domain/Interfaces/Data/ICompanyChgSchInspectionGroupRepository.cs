using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Interfaces.Data
{
   public interface ICompanyChgSchInspectionGroupRepository:IGenericRepository<DbRepository.Models.SqlDatabaseContext.CompanyChgSchInspGroup>
    {
        IList<Models.CompanyChgSchInspGroup> Search(Models.CompanyChgSchInspGroup search);
    }
}
