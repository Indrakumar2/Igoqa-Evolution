using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Interfaces.Data
{
    public interface ICompanyChgSchInspGrpInspectionTypeRepository : IGenericRepository<DbRepository.Models.SqlDatabaseContext.CompanyChgSchInspGrpInspectionType>
    
    {
        IList<Models.CompanyChgSchInspGrpInspectionType> Search(Models.CompanyChgSchInspGrpInspectionType search);
    }
}
