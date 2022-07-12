using Evolution.GenericDbRepository.Interfaces;
using DomainModel = Evolution.Company.Domain.Models.Companies;
using System;
using System.Collections.Generic;
using System.Text;
using Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Company.Domain.Interfaces.Data
{   
    //Company Division Cost Center
    public interface ICompanyCostCenterRepository : IGenericRepository<CompanyDivisionCostCenter>
    {
        IList<DomainModel.CompanyDivisionCostCenter> Search(DomainModel.CompanyDivisionCostCenter searchModel);
    }
}
