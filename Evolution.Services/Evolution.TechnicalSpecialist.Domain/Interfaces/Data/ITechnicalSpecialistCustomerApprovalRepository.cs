using Evolution.GenericDbRepository.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.Data
{
   public interface ITechnicalSpecialistCustomerApprovalRepository : IGenericRepository<DbModel.TechnicalSpecialistCustomerApproval>
    {
       
        IList<DbModel.TechnicalSpecialistCustomerApproval> Search(TechnicalSpecialistCustomerApprovalInfo model);

        IList<DbModel.TechnicalSpecialistCustomerApproval> Get(IList<int> Ids);

        IList<DbModel.TechnicalSpecialistCustomerApproval> GetByPinId(IList<string> pinIds);

        IList<DbModel.TechnicalSpecialistCustomerApproval> Get(IList<string> CustomerName);

        IList<DbModel.TechnicalSpecialistCustomerApproval> Get(IList<KeyValuePair<string, string>> ePinAndCustomerName);
    }
}
