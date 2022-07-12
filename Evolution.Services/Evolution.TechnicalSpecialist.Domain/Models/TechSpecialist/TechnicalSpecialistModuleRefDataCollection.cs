using System;
using System.Collections.Generic;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistModuleRefDataCollection : IDisposable
    {
        // Flag: Has Dispose already been called?
        bool disposed = false;

        public IList<DbModel.Company> DbCompanies = null;
        public IList<DbModel.CompanyPayroll> DbCompPayrolls = null;
        public IList<DbModel.Data> DbSubDivisions = null;
        public IList<DbModel.Data> DbStatuses = null;
        public IList<DbModel.Data> DbActions = null;
        public IList<DbModel.Data> DbEmploymentTypes = null;
        public IList<DbModel.Country> DbCountries = null;
        public IList<DbModel.County> DbCounties = null;
        public IList<DbModel.City> DbCities = null;
        public IList<DbModel.Data> DbCertificationTypes = null;
        public IList<DbModel.Data> DbTrainingTypes = null;
        public IList<DbModel.User> DbVarifiedByUsers = null;
        public IList<DbModel.Data> DbCodeAndStandards = null;
        public IList<DbModel.Data> DbCommodities = null;
        public IList<DbModel.Data> DbEquipments = null;
        public IList<DbModel.Data> DbComputerElectronicsKnowledges = null;
        public IList<DbModel.TechnicalSpecialistCustomers> DbTechSpecCustomers = null;
        public IList<DbModel.Data> DbLanguages = null;
        public IList<DbModel.Data> DbCurrencies = null;
        public IList<DbModel.Data> DbExpenseTypes = null;
        public IList<DbModel.Data> DbCategories = null;
        public IList<DbModel.Data> DbTsStampCountries =null;
        public IList<DbModel.TaxonomySubCategory> DbSubCategories = null;
        public IList<DbModel.TaxonomyService> DbTaxonomyService = null;
        public IList<DbModel.User> DbUsers = null; 

        public IList<DbModel.TechnicalSpecialist> DbTechnicalSpecialists = null;
        public IList<DbModel.TechnicalSpecialistCertificationAndTraining> DbTsCertifications = null;
        public IList<DbModel.TechnicalSpecialistCertificationAndTraining> DbTsTrainings = null;
        public IList<DbModel.TechnicalSpecialistCodeAndStandard> DbTsCodeAndStandardInfos = null;
        public IList<DbModel.TechnicalSpecialistCommodityEquipmentKnowledge> DbTsComdEqipKnowledges = null;
        public IList<DbModel.TechnicalSpecialistComputerElectronicKnowledge> DbTsCompElecKnowledges = null;
        public IList<DbModel.TechnicalSpecialistCustomerApproval> DbTsCustomerApprovalInfos = null;
        public IList<DbModel.TechnicalSpecialistLanguageCapability> DbTsLanguageCapabilities = null;
        public IList<DbModel.TechnicalSpecialistNote> DbTsNotes = null;
        public IList<DbModel.TechnicalSpecialistPaySchedule> DbTsPaySchedules = null;
        public IList<DbModel.TechnicalSpecialistPayRate> DbTsPayRates = null;
        public IList<DbModel.TechnicalSpecialistStamp> DbTsStampInfos = null;
        public IList<DbModel.TechnicalSpecialistTaxonomy> DbTsTaxonomies = null;
        public IList<DbModel.TechnicalSpecialistWorkHistory> DbTsWorkHistoryInfos = null;
        public IList<DbModel.TechnicalSpecialistTrainingAndCompetency> DbTsCompetencies = null;
        public IList<DbModel.TechnicalSpecialistTrainingAndCompetency> DbTsInternalTrainings = null;
        public IList<DbModel.TechnicalSpecialistContact> DbTsContacts = null;
        public IList<DbModel.TechnicalSpecialistEducationalQualification> DbTsEducationQulifications = null;
        
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
                DbCompanies?.Clear();
                DbCompanies = null;

                DbCompPayrolls?.Clear();
                DbCompPayrolls = null;

                DbSubDivisions?.Clear();
                DbSubDivisions = null;

                DbStatuses?.Clear();
                DbStatuses = null;

                DbActions?.Clear();
                DbActions = null;

                DbEmploymentTypes?.Clear();
                DbEmploymentTypes = null;

                DbCountries?.Clear();
                DbCountries = null;

                DbCounties?.Clear();
                DbCounties = null;

                DbCities?.Clear();
                DbCities = null;

                DbCertificationTypes?.Clear();
                DbCertificationTypes = null;

                DbTrainingTypes?.Clear();
                DbTrainingTypes = null;

                DbVarifiedByUsers?.Clear();
                DbVarifiedByUsers = null;

                DbCodeAndStandards?.Clear();
                DbCodeAndStandards = null;

                DbCommodities?.Clear();
                DbCommodities = null;

                DbEquipments?.Clear();
                DbEquipments = null;

                DbComputerElectronicsKnowledges?.Clear();
                DbComputerElectronicsKnowledges = null;

                DbTechSpecCustomers?.Clear();
                DbTechSpecCustomers = null;

                DbLanguages?.Clear();
                DbLanguages = null;

                DbCurrencies?.Clear();
                DbStatuses = null;

                DbExpenseTypes?.Clear();
                DbExpenseTypes = null;

                DbCategories?.Clear();
                DbCategories = null;

                DbSubCategories?.Clear();
                DbSubCategories = null;

                DbTaxonomyService?.Clear();
                DbTaxonomyService = null;

                DbUsers?.Clear();
                DbUsers = null;

                DbTechnicalSpecialists?.Clear();
                DbTechnicalSpecialists = null;

                DbTsCertifications?.Clear();
                DbTsCertifications = null;

                DbTsTrainings?.Clear();
                DbTsTrainings = null;

                DbTsCodeAndStandardInfos?.Clear();
                DbTsCodeAndStandardInfos = null;

                DbTsComdEqipKnowledges?.Clear();
                DbTsComdEqipKnowledges = null;

                DbTsCompElecKnowledges?.Clear();
                DbTsCompElecKnowledges = null;

                DbTsCustomerApprovalInfos?.Clear();
                DbTsCustomerApprovalInfos = null;

                DbTsLanguageCapabilities?.Clear();
                DbTsLanguageCapabilities = null;

                DbTsNotes?.Clear();
                DbTsNotes = null;

                DbTsPaySchedules?.Clear();
                DbTsPaySchedules = null;

                DbTsPayRates?.Clear();
                DbTsPayRates = null;

                DbTsStampInfos?.Clear();
                DbTsStampInfos = null;

                DbTsTaxonomies?.Clear();
                DbTsTaxonomies = null;

                DbTsWorkHistoryInfos?.Clear();
                DbTsWorkHistoryInfos = null;

                DbTsCompetencies?.Clear();
                DbTsCompetencies = null;

                DbTsInternalTrainings?.Clear();
                DbTsInternalTrainings = null;

                DbTsContacts?.Clear();
                DbTsContacts = null;

                DbTsEducationQulifications?.Clear();
                DbTsEducationQulifications = null;
                 
            }
            disposed = true;
        }
    }
}
