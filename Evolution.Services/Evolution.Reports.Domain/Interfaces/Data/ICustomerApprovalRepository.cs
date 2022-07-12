using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Reports.Domain.Models.Reports;
namespace Evolution.Reports.Domain.Interfaces.Data
{
        public interface ICustomerApprovalRepository : IGenericRepository<DbModel.TechnicalSpecialistCustomerApproval>
        {
            IList<DomainModel.CustomerApproval> Search(DomainModel.CustomerApprovalSearch searchModel);
        }
    
}
