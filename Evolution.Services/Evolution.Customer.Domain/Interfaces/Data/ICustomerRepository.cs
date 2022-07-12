using Evolution.GenericDbRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Customer.Domain.Models.Customers;

namespace Evolution.Customer.Domain.Interfaces.Data
{    
    public interface ICustomerRepository : IGenericRepository<DbModel.Customer>
    {
        IList<DomainModel.Customer> Search(DomainModel.CustomerSearch searchModel);

        int? GetCityIdForName(string cityName,string countyName);

        int GetAssignmentReferenceIdForAssignmentRefferenceType(string AssignmentReferenceType);

        int GetCompanyIdForCompanyCode(string companyCode);

        bool GetIsPortalUser(string logonName);

        IList<DomainModel.Customer> SearchCustomersBasedOnCoordinators(DomainModel.CustomerSearch searchModel);

        IList<DomainModel.CustomerSearch> SearchApprovedVisitCustomers(int ContractHolderCompanyId, bool isVisit, bool isNDT);

        IList<DomainModel.CustomerSearch> SearchUnApprovedVisitCustomers(int ContractHolderCompanyId, int? CoordinatorId, bool isVisit, bool isOperating);

        IList<DomainModel.CustomerSearch> GetVisitTimesheetKPICustomers(int ContractHolderCompanyId, bool isVisit, bool isOperating);
    }
}