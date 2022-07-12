using System;
using System.Collections.Generic;
using DBModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentModuleDatabaseCollection : IDisposable
    {
        // Flag: Has Dispose already been called?
        bool disposed = false;
        public AssignmentDatabaseCollection Assignment = null;
        public DBModel.Assignment DbAssignment = null;
        public IList<DBModel.Assignment> DBAssignment = null;
        public IList<DBModel.SupplierPurchaseOrderSubSupplier> DBSubSupplier = null;
        public IList<DBModel.AssignmentSubSupplier> DBAssignmentSubSupplier = null;
        public IList<DBModel.TechnicalSpecialist> DBTechnicalSpecialists = null;
        public IList<DBModel.TechnicalSpecialist> DBMainSupplierTechnicalSpecialists = null;
        public IList<DBModel.TechnicalSpecialist> DBSubSupplierTechnicalSpecialists = null;
        public IList<DBModel.AssignmentContractSchedule> DBAssignmentContractSchedules = null;
        public IList<DBModel.AssignmentReference> DBAssignmentReferenceTypes = null;
        public IList<DBModel.AssignmentTaxonomy> DBAssignmentTaxonomy = null;
        public IList<DBModel.AssignmentTechnicalSpecialist> DBAssignmentTechnicalSpecialists = null;
        public IList<DBModel.AssignmentAdditionalExpense> DBAssignmentAdditionalExpenses = null;
        public IList<DBModel.AssignmentInterCompanyDiscount> DBAssignmentInterCompanyDiscounts = null;
        public IList<DBModel.AssignmentContributionCalculation> DBAssignmentContributionCalculations = null;
        public IList<DBModel.AssignmentNote> DBAssignmentNotes = null;
        public IList<DBModel.Data> DBReferenceType = null;
        public IList<DBModel.Data> DBExpenseType = null;
        public IList<DBModel.Data> DBCategory = null;
        public IList<DBModel.TaxonomySubCategory> DBSubCategory = null;
        public IList<DBModel.TaxonomyService> DBService = null;
        public IList<DBModel.ContractSchedule> DBContractSchedule = null;
        public IList<DBModel.Company> DBInterCompanies = null;
        public DBModel.Assignment DBARSAssignment = null;
        public IList<DBModel.ResourceSearch> DBARSSearches = null;
        public IList<DBModel.User> DBARSCoordinators = null;
        public IList<DBModel.OverrideResource> DBOverrideResources = null;
        public IList<DBModel.VisitTechnicalSpecialist> dbAddedVisitTS = null;
        public IList<DBModel.TimesheetTechnicalSpecialist> dbAddedTimesheetTS = null;
        public DBModel.Visit dbVisit = null;
        public DBModel.Timesheet dbTimesheet = null;
        public IList<DBModel.Data> dbLineItemExpense = null;
        public IList<DBModel.TechnicalSpecialistPaySchedule> dbTechPaySchedule = null;
        public IList<DBModel.TechnicalSpecialistPayRate> dbTechPayRate = null;
        public IList<DBModel.SqlauditModule> dbModule = null;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Assignment = null;

                DBAssignment?.Clear();
                DBAssignment = null;

                DBSubSupplier?.Clear();
                DBSubSupplier = null;

                DBAssignmentSubSupplier?.Clear();
                DBAssignmentSubSupplier = null;

                DBTechnicalSpecialists?.Clear();
                DBTechnicalSpecialists = null;

                DBAssignmentContractSchedules?.Clear();
                DBAssignmentContractSchedules = null;

                DBAssignmentReferenceTypes?.Clear();
                DBAssignmentReferenceTypes = null;

                DBAssignmentTaxonomy?.Clear();
                DBAssignmentTaxonomy = null;

                DBAssignmentTechnicalSpecialists?.Clear();
                DBAssignmentTechnicalSpecialists = null;

                DBAssignmentAdditionalExpenses?.Clear();
                DBAssignmentAdditionalExpenses = null;

                DBAssignmentInterCompanyDiscounts?.Clear();
                DBAssignmentInterCompanyDiscounts = null;

                DBAssignmentContributionCalculations?.Clear();
                DBAssignmentContributionCalculations = null;

                DBAssignmentNotes?.Clear();
                DBAssignmentNotes = null;

                DBReferenceType?.Clear();
                DBReferenceType = null;

                DBExpenseType?.Clear();
                DBExpenseType = null;

                DBCategory?.Clear();
                DBCategory = null;

                DBSubCategory?.Clear();
                DBSubCategory = null;

                DBService?.Clear();
                DBService = null;

                DBContractSchedule?.Clear();
                DBContractSchedule = null;

                DBInterCompanies?.Clear();
                DBInterCompanies = null;

                DBARSAssignment = null;

                DBARSSearches?.Clear();
                DBARSSearches = null;

                DBARSCoordinators?.Clear();
                DBARSCoordinators = null;

                DBOverrideResources?.Clear();
                DBOverrideResources = null;

                dbAddedVisitTS?.Clear();
                dbAddedVisitTS = null;

                dbAddedTimesheetTS?.Clear();
                dbAddedTimesheetTS = null;

                dbVisit = null;
                dbTimesheet = null;

                dbLineItemExpense?.Clear();
                dbLineItemExpense = null;

                dbTechPaySchedule?.Clear();
                dbTechPaySchedule = null;

                dbTechPayRate?.Clear();
                dbTechPayRate = null;

                dbModule?.Clear();
                dbModule = null;
            }
            disposed = true;
        }
    }

    public class AssignmentDatabaseCollection : IDisposable
    {
        // Flag: Has Dispose already been called?
        bool disposed = false;
        public IList<DBModel.Project> DBProjects = null;
        public IList<DBModel.Contract> DBContracts = null;
        public IList<DBModel.Customer> DBCustomers = null;
        public IList<DBModel.Company> DBCompanies = null;
        public IList<DBModel.Company> DBHostCompanies = null;
        public IList<DBModel.User> DBContractCoordinatorUsers = null;
        public IList<DBModel.User> DBOperatingUsers = null;
        public List<DBModel.Data> DBAssignmentStatus = null;
        public List<DBModel.Data> DBAssignmentType = null;
        public List<DBModel.Data> DBAssignmentLifeCycle = null;
        public List<DBModel.Data> DBAssignmentReviewAndModeration = null;
        public IList<DBModel.CustomerContact> DBCustomerContacts = null;
        public IList<DBModel.CustomerAddress> DBCustomerOffices = null;
        public IList<DBModel.SupplierPurchaseOrder> DBSupplierPO = null;
        public IList<DBModel.Country> DBCountry = null;
        public IList<DBModel.County> DBCounty = null;
        public IList<DBModel.City> DBCity = null;
        public IList<DBModel.Data> DBMasterData = null;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                DBProjects?.Clear();
                DBProjects = null;

                DBContracts?.Clear();
                DBContracts = null;

                DBCustomers?.Clear();
                DBCustomers = null;

                DBCompanies?.Clear();
                DBCompanies = null;

                DBHostCompanies?.Clear();
                DBHostCompanies = null;

                DBContractCoordinatorUsers?.Clear();
                DBContractCoordinatorUsers = null;

                DBOperatingUsers?.Clear();
                DBOperatingUsers = null;

                DBAssignmentStatus?.Clear();
                DBAssignmentStatus = null;

                DBAssignmentType?.Clear();
                DBAssignmentType = null;

                DBAssignmentLifeCycle?.Clear();
                DBAssignmentLifeCycle = null;

                DBAssignmentReviewAndModeration?.Clear();
                DBAssignmentReviewAndModeration = null;

                DBCustomerContacts?.Clear();
                DBCustomerContacts = null;

                DBCustomerOffices?.Clear();
                DBCustomerOffices = null;

                DBSupplierPO?.Clear();
                DBSupplierPO = null;

                DBCountry?.Clear();
                DBCountry = null;

                DBCounty?.Clear();
                DBCounty = null;

                DBCity?.Clear();
                DBCity = null;

                DBMasterData?.Clear();
                DBMasterData = null;
            }
            disposed = true;
        }

    }
}
