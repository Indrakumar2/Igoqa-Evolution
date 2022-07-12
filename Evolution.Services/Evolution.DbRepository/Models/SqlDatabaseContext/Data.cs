using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Data
    {
        public Data()
        {
            AssignmentAdditionalExpense = new HashSet<AssignmentAdditionalExpense>();
            AssignmentAssignmentLifecycle = new HashSet<Assignment>();
            AssignmentHistory = new HashSet<AssignmentHistory>();
            AssignmentReference = new HashSet<AssignmentReference>();
            AssignmentReviewAndModerationProcess = new HashSet<Assignment>();
            CommodityEquipmentCommodity = new HashSet<CommodityEquipment>();
            CommodityEquipmentEquipment = new HashSet<CommodityEquipment>();
            Company = new HashSet<Company>();
            CompanyChargeSchedule = new HashSet<CompanyChargeSchedule>();
            CompanyChgSchInspGroup = new HashSet<CompanyChgSchInspGroup>();
            CompanyChgSchInspGrpInspectionType = new HashSet<CompanyChgSchInspGrpInspectionType>();
            CompanyDivision = new HashSet<CompanyDivision>();
            CompanyExpectedMargin = new HashSet<CompanyExpectedMargin>();
            CompanyInspectionTypeChargeRateExpenseType = new HashSet<CompanyInspectionTypeChargeRate>();
            CompanyInspectionTypeChargeRateFilmSize = new HashSet<CompanyInspectionTypeChargeRate>();
            CompanyInspectionTypeChargeRateFilmType = new HashSet<CompanyInspectionTypeChargeRate>();
            CompanyInspectionTypeChargeRateItemSize = new HashSet<CompanyInspectionTypeChargeRate>();
            CompanyInspectionTypeChargeRateItemThickness = new HashSet<CompanyInspectionTypeChargeRate>();
            Contract = new HashSet<Contract>();
            ContractInvoiceAttachment = new HashSet<ContractInvoiceAttachment>();
            ContractInvoiceReference = new HashSet<ContractInvoiceReference>();
            ContractRate = new HashSet<ContractRate>();
            Country = new HashSet<Country>();
            CurrencyExchangeRate = new HashSet<CurrencyExchangeRate>();
            CustomerAssignmentReferenceType = new HashSet<CustomerAssignmentReferenceType>();
            CustomerCommodity = new HashSet<CustomerCommodity>();
            Invoice = new HashSet<Invoice>();
            InvoiceAssignmentReferenceType = new HashSet<InvoiceAssignmentReferenceType>();
            LanguageInvoicePaymentTermInvoicePaymentTerm = new HashSet<LanguageInvoicePaymentTerm>();
            LanguageInvoicePaymentTermLanguage = new HashSet<LanguageInvoicePaymentTerm>();
            LanguageReferenceTypeLanguage = new HashSet<LanguageReferenceType>();
            LanguageReferenceTypeReferenceType = new HashSet<LanguageReferenceType>();
            ModuleDocumentTypeDocumentType = new HashSet<ModuleDocumentType>();
            ModuleDocumentTypeModule = new HashSet<ModuleDocumentType>();
            ProjectInvoiceAssignmentReference = new HashSet<ProjectInvoiceAssignmentReference>();
            ProjectInvoiceAttachment = new HashSet<ProjectInvoiceAttachment>();
            ProjectInvoicePaymentTerms = new HashSet<Project>();
            ProjectLogo = new HashSet<Project>();
            ProjectManagedServicesTypeNavigation = new HashSet<Project>();
            ProjectProjectType = new HashSet<Project>();
            ResourceSearch = new HashSet<ResourceSearch>();
            TaxonomyBusinessUnitCategory = new HashSet<TaxonomyBusinessUnit>();
            TaxonomyBusinessUnitProjectType = new HashSet<TaxonomyBusinessUnit>();
            TaxonomySubCategory = new HashSet<TaxonomySubCategory>();
            TechnicalSpecialistCertificationAndTraining = new HashSet<TechnicalSpecialistCertificationAndTraining>();
            TechnicalSpecialistCodeAndStandard = new HashSet<TechnicalSpecialistCodeAndStandard>();
            TechnicalSpecialistCommodityEquipmentKnowledgeCommodity = new HashSet<TechnicalSpecialistCommodityEquipmentKnowledge>();
            TechnicalSpecialistCommodityEquipmentKnowledgeEquipmentKnowledge = new HashSet<TechnicalSpecialistCommodityEquipmentKnowledge>();
            TechnicalSpecialistComputerElectronicKnowledge = new HashSet<TechnicalSpecialistComputerElectronicKnowledge>();
            TechnicalSpecialistCustomerApproval = new HashSet<TechnicalSpecialistCustomerApproval>();
            TechnicalSpecialistEmploymentType = new HashSet<TechnicalSpecialist>();
            TechnicalSpecialistLanguageCapability = new HashSet<TechnicalSpecialistLanguageCapability>();
            TechnicalSpecialistPayRate = new HashSet<TechnicalSpecialistPayRate>();
            TechnicalSpecialistProfileAction = new HashSet<TechnicalSpecialist>();
            TechnicalSpecialistProfileStatus = new HashSet<TechnicalSpecialist>();
            TechnicalSpecialistStamp = new HashSet<TechnicalSpecialistStamp>();
            TechnicalSpecialistSubDivision = new HashSet<TechnicalSpecialist>();
            TechnicalSpecialistTaxonomy = new HashSet<TechnicalSpecialistTaxonomy>();
            TechnicalSpecialistTimeOffRequest = new HashSet<TechnicalSpecialistTimeOffRequest>();
            TechnicalSpecialistTrainingAndCompetencyType = new HashSet<TechnicalSpecialistTrainingAndCompetencyType>();
            TimeOffRequestCategoryEmploymentType = new HashSet<TimeOffRequestCategory>();
            TimeOffRequestCategoryLeaveCategoryType = new HashSet<TimeOffRequestCategory>();
            TimesheetHistory = new HashSet<TimesheetHistory>();
            TimesheetReference = new HashSet<TimesheetReference>();
            TimesheetTechnicalSpecialistAccountItemConsumableChargeExpenseType = new HashSet<TimesheetTechnicalSpecialistAccountItemConsumable>();
            TimesheetTechnicalSpecialistAccountItemConsumablePayExpenseType = new HashSet<TimesheetTechnicalSpecialistAccountItemConsumable>();
            TimesheetTechnicalSpecialistAccountItemConsumableUnpaidStatus = new HashSet<TimesheetTechnicalSpecialistAccountItemConsumable>();
            TimesheetTechnicalSpecialistAccountItemExpenseExpenseChargeType = new HashSet<TimesheetTechnicalSpecialistAccountItemExpense>();
            TimesheetTechnicalSpecialistAccountItemExpenseUnpaidStatus = new HashSet<TimesheetTechnicalSpecialistAccountItemExpense>();
            TimesheetTechnicalSpecialistAccountItemTimeExpenseChargeType = new HashSet<TimesheetTechnicalSpecialistAccountItemTime>();
            TimesheetTechnicalSpecialistAccountItemTimeUnpaidStatus = new HashSet<TimesheetTechnicalSpecialistAccountItemTime>();
            TimesheetTechnicalSpecialistAccountItemTravelChargeExpenseType = new HashSet<TimesheetTechnicalSpecialistAccountItemTravel>();
            TimesheetTechnicalSpecialistAccountItemTravelPayExpenseType = new HashSet<TimesheetTechnicalSpecialistAccountItemTravel>();
            TimesheetTechnicalSpecialistAccountItemTravelUnpaidStatus = new HashSet<TimesheetTechnicalSpecialistAccountItemTravel>();
            UnpaidStatusReason = new HashSet<UnpaidStatusReason>();
            VisitHistory = new HashSet<VisitHistory>();
            VisitReference = new HashSet<VisitReference>();
            VisitTechnicalSpecialistAccountItemConsumableChargeExpenseType = new HashSet<VisitTechnicalSpecialistAccountItemConsumable>();
            VisitTechnicalSpecialistAccountItemConsumablePayExpenseType = new HashSet<VisitTechnicalSpecialistAccountItemConsumable>();
            VisitTechnicalSpecialistAccountItemConsumableUnpaidStatus = new HashSet<VisitTechnicalSpecialistAccountItemConsumable>();
            VisitTechnicalSpecialistAccountItemExpenseExpenseChargeType = new HashSet<VisitTechnicalSpecialistAccountItemExpense>();
            VisitTechnicalSpecialistAccountItemExpenseUnpaidStatus = new HashSet<VisitTechnicalSpecialistAccountItemExpense>();
            VisitTechnicalSpecialistAccountItemTimeExpenseChargeType = new HashSet<VisitTechnicalSpecialistAccountItemTime>();
            VisitTechnicalSpecialistAccountItemTimeUnpaidStatus = new HashSet<VisitTechnicalSpecialistAccountItemTime>();
            VisitTechnicalSpecialistAccountItemTravelChargeExpenseType = new HashSet<VisitTechnicalSpecialistAccountItemTravel>();
            VisitTechnicalSpecialistAccountItemTravelPayExpenseType = new HashSet<VisitTechnicalSpecialistAccountItemTravel>();
            VisitTechnicalSpecialistAccountItemTravelUnpaidStatus = new HashSet<VisitTechnicalSpecialistAccountItemTravel>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int MasterDataTypeId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAlchiddenForNewFacility { get; set; }
        public bool? IsEmployed { get; set; }
        public string Type { get; set; }
        public string InterCompanyType { get; set; }
        public string ChargeReference { get; set; }
        public string PayReference { get; set; }
        public string InvoiceType { get; set; }
        public int? Precedence { get; set; }
        public int? Hour { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public int? Evolution1Id { get; set; }
        public string PayrollExportPrefix { get; set; }
        public bool? IsArs { get; set; }
        public bool? IsTsVisible { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsDefaultSpecialistExtranetReadOnly { get; set; }

        public virtual DataType MasterDataType { get; set; }
        public virtual ICollection<AssignmentAdditionalExpense> AssignmentAdditionalExpense { get; set; }
        public virtual ICollection<Assignment> AssignmentAssignmentLifecycle { get; set; }
        public virtual ICollection<AssignmentHistory> AssignmentHistory { get; set; }
        public virtual ICollection<AssignmentReference> AssignmentReference { get; set; }
        public virtual ICollection<Assignment> AssignmentReviewAndModerationProcess { get; set; }
        public virtual ICollection<CommodityEquipment> CommodityEquipmentCommodity { get; set; }
        public virtual ICollection<CommodityEquipment> CommodityEquipmentEquipment { get; set; }
        public virtual ICollection<Company> Company { get; set; }
        public virtual ICollection<CompanyChargeSchedule> CompanyChargeSchedule { get; set; }
        public virtual ICollection<CompanyChgSchInspGroup> CompanyChgSchInspGroup { get; set; }
        public virtual ICollection<CompanyChgSchInspGrpInspectionType> CompanyChgSchInspGrpInspectionType { get; set; }
        public virtual ICollection<CompanyDivision> CompanyDivision { get; set; }
        public virtual ICollection<CompanyExpectedMargin> CompanyExpectedMargin { get; set; }
        public virtual ICollection<CompanyInspectionTypeChargeRate> CompanyInspectionTypeChargeRateExpenseType { get; set; }
        public virtual ICollection<CompanyInspectionTypeChargeRate> CompanyInspectionTypeChargeRateFilmSize { get; set; }
        public virtual ICollection<CompanyInspectionTypeChargeRate> CompanyInspectionTypeChargeRateFilmType { get; set; }
        public virtual ICollection<CompanyInspectionTypeChargeRate> CompanyInspectionTypeChargeRateItemSize { get; set; }
        public virtual ICollection<CompanyInspectionTypeChargeRate> CompanyInspectionTypeChargeRateItemThickness { get; set; }
        public virtual ICollection<Contract> Contract { get; set; }
        public virtual ICollection<ContractInvoiceAttachment> ContractInvoiceAttachment { get; set; }
        public virtual ICollection<ContractInvoiceReference> ContractInvoiceReference { get; set; }
        public virtual ICollection<ContractRate> ContractRate { get; set; }
        public virtual ICollection<Country> Country { get; set; }
        public virtual ICollection<CurrencyExchangeRate> CurrencyExchangeRate { get; set; }
        public virtual ICollection<CustomerAssignmentReferenceType> CustomerAssignmentReferenceType { get; set; }
        public virtual ICollection<CustomerCommodity> CustomerCommodity { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<InvoiceAssignmentReferenceType> InvoiceAssignmentReferenceType { get; set; }
        public virtual ICollection<LanguageInvoicePaymentTerm> LanguageInvoicePaymentTermInvoicePaymentTerm { get; set; }
        public virtual ICollection<LanguageInvoicePaymentTerm> LanguageInvoicePaymentTermLanguage { get; set; }
        public virtual ICollection<LanguageReferenceType> LanguageReferenceTypeLanguage { get; set; }
        public virtual ICollection<LanguageReferenceType> LanguageReferenceTypeReferenceType { get; set; }
        public virtual ICollection<ModuleDocumentType> ModuleDocumentTypeDocumentType { get; set; }
        public virtual ICollection<ModuleDocumentType> ModuleDocumentTypeModule { get; set; }
        public virtual ICollection<ProjectInvoiceAssignmentReference> ProjectInvoiceAssignmentReference { get; set; }
        public virtual ICollection<ProjectInvoiceAttachment> ProjectInvoiceAttachment { get; set; }
        public virtual ICollection<Project> ProjectInvoicePaymentTerms { get; set; }
        public virtual ICollection<Project> ProjectLogo { get; set; }
        public virtual ICollection<Project> ProjectManagedServicesTypeNavigation { get; set; }
        public virtual ICollection<Project> ProjectProjectType { get; set; }
        public virtual ICollection<ResourceSearch> ResourceSearch { get; set; }
        public virtual ICollection<TaxonomyBusinessUnit> TaxonomyBusinessUnitCategory { get; set; }
        public virtual ICollection<TaxonomyBusinessUnit> TaxonomyBusinessUnitProjectType { get; set; }
        public virtual ICollection<TaxonomySubCategory> TaxonomySubCategory { get; set; }
        public virtual ICollection<TechnicalSpecialistCertificationAndTraining> TechnicalSpecialistCertificationAndTraining { get; set; }
        public virtual ICollection<TechnicalSpecialistCodeAndStandard> TechnicalSpecialistCodeAndStandard { get; set; }
        public virtual ICollection<TechnicalSpecialistCommodityEquipmentKnowledge> TechnicalSpecialistCommodityEquipmentKnowledgeCommodity { get; set; }
        public virtual ICollection<TechnicalSpecialistCommodityEquipmentKnowledge> TechnicalSpecialistCommodityEquipmentKnowledgeEquipmentKnowledge { get; set; }
        public virtual ICollection<TechnicalSpecialistComputerElectronicKnowledge> TechnicalSpecialistComputerElectronicKnowledge { get; set; }
        public virtual ICollection<TechnicalSpecialistCustomerApproval> TechnicalSpecialistCustomerApproval { get; set; }
        public virtual ICollection<TechnicalSpecialist> TechnicalSpecialistEmploymentType { get; set; }
        public virtual ICollection<TechnicalSpecialistLanguageCapability> TechnicalSpecialistLanguageCapability { get; set; }
        public virtual ICollection<TechnicalSpecialistPayRate> TechnicalSpecialistPayRate { get; set; }
        public virtual ICollection<TechnicalSpecialist> TechnicalSpecialistProfileAction { get; set; }
        public virtual ICollection<TechnicalSpecialist> TechnicalSpecialistProfileStatus { get; set; }
        public virtual ICollection<TechnicalSpecialistStamp> TechnicalSpecialistStamp { get; set; }
        public virtual ICollection<TechnicalSpecialist> TechnicalSpecialistSubDivision { get; set; }
        public virtual ICollection<TechnicalSpecialistTaxonomy> TechnicalSpecialistTaxonomy { get; set; }
        public virtual ICollection<TechnicalSpecialistTimeOffRequest> TechnicalSpecialistTimeOffRequest { get; set; }
        public virtual ICollection<TechnicalSpecialistTrainingAndCompetencyType> TechnicalSpecialistTrainingAndCompetencyType { get; set; }
        public virtual ICollection<TimeOffRequestCategory> TimeOffRequestCategoryEmploymentType { get; set; }
        public virtual ICollection<TimeOffRequestCategory> TimeOffRequestCategoryLeaveCategoryType { get; set; }
        public virtual ICollection<TimesheetHistory> TimesheetHistory { get; set; }
        public virtual ICollection<TimesheetReference> TimesheetReference { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemConsumable> TimesheetTechnicalSpecialistAccountItemConsumableChargeExpenseType { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemConsumable> TimesheetTechnicalSpecialistAccountItemConsumablePayExpenseType { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemConsumable> TimesheetTechnicalSpecialistAccountItemConsumableUnpaidStatus { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemExpense> TimesheetTechnicalSpecialistAccountItemExpenseExpenseChargeType { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemExpense> TimesheetTechnicalSpecialistAccountItemExpenseUnpaidStatus { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTime> TimesheetTechnicalSpecialistAccountItemTimeExpenseChargeType { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTime> TimesheetTechnicalSpecialistAccountItemTimeUnpaidStatus { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTravel> TimesheetTechnicalSpecialistAccountItemTravelChargeExpenseType { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTravel> TimesheetTechnicalSpecialistAccountItemTravelPayExpenseType { get; set; }
        public virtual ICollection<TimesheetTechnicalSpecialistAccountItemTravel> TimesheetTechnicalSpecialistAccountItemTravelUnpaidStatus { get; set; }
        public virtual ICollection<UnpaidStatusReason> UnpaidStatusReason { get; set; }
        public virtual ICollection<VisitHistory> VisitHistory { get; set; }
        public virtual ICollection<VisitReference> VisitReference { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemConsumable> VisitTechnicalSpecialistAccountItemConsumableChargeExpenseType { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemConsumable> VisitTechnicalSpecialistAccountItemConsumablePayExpenseType { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemConsumable> VisitTechnicalSpecialistAccountItemConsumableUnpaidStatus { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemExpense> VisitTechnicalSpecialistAccountItemExpenseExpenseChargeType { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemExpense> VisitTechnicalSpecialistAccountItemExpenseUnpaidStatus { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTime> VisitTechnicalSpecialistAccountItemTimeExpenseChargeType { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTime> VisitTechnicalSpecialistAccountItemTimeUnpaidStatus { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTravel> VisitTechnicalSpecialistAccountItemTravelChargeExpenseType { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTravel> VisitTechnicalSpecialistAccountItemTravelPayExpenseType { get; set; }
        public virtual ICollection<VisitTechnicalSpecialistAccountItemTravel> VisitTechnicalSpecialistAccountItemTravelUnpaidStatus { get; set; }
    }
}
