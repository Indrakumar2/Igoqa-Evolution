using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class EvolutionSqlDbContext : DbContext
    {
        //public EvolutionSqlDbContext()
        //{
        //}

        public EvolutionSqlDbContext(DbContextOptions<EvolutionSqlDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<Announcement> Announcement { get; set; }
        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<ApplicationMenu> ApplicationMenu { get; set; }
        public virtual DbSet<Assignment> Assignment { get; set; }
        public virtual DbSet<AssignmentAdditionalExpense> AssignmentAdditionalExpense { get; set; }
        public virtual DbSet<AssignmentContractSchedule> AssignmentContractSchedule { get; set; }
        public virtual DbSet<AssignmentContributionCalculation> AssignmentContributionCalculation { get; set; }
        public virtual DbSet<AssignmentContributionRevenueCost> AssignmentContributionRevenueCost { get; set; }
        public virtual DbSet<AssignmentHistory> AssignmentHistory { get; set; }
        public virtual DbSet<AssignmentInterCompanyDiscount> AssignmentInterCompanyDiscount { get; set; }
        public virtual DbSet<AssignmentMessage> AssignmentMessage { get; set; }
        public virtual DbSet<AssignmentMessageType> AssignmentMessageType { get; set; }
        public virtual DbSet<AssignmentNote> AssignmentNote { get; set; }
        public virtual DbSet<AssignmentReference> AssignmentReference { get; set; }
        public virtual DbSet<AssignmentSubSupplier> AssignmentSubSupplier { get; set; }
        public virtual DbSet<AssignmentSubSupplierTechnicalSpecialist> AssignmentSubSupplierTechnicalSpecialist { get; set; }
        public virtual DbSet<AssignmentTaxonomy> AssignmentTaxonomy { get; set; }
        public virtual DbSet<AssignmentTechnicalSpecialist> AssignmentTechnicalSpecialist { get; set; }
        public virtual DbSet<AssignmentTechnicalSpecialistSchedule> AssignmentTechnicalSpecialistSchedule { get; set; }
        public virtual DbSet<Audience> Audience { get; set; }
        public virtual DbSet<AuditSearch> AuditSearch { get; set; }
        public virtual DbSet<BatchProcess> BatchProcess { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientAudience> ClientAudience { get; set; }
        public virtual DbSet<CommodityEquipment> CommodityEquipment { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<CompanyChargeSchedule> CompanyChargeSchedule { get; set; }
        public virtual DbSet<CompanyChgSchInspGroup> CompanyChgSchInspGroup { get; set; }
        public virtual DbSet<CompanyChgSchInspGrpInspectionType> CompanyChgSchInspGrpInspectionType { get; set; }
        public virtual DbSet<CompanyDivision> CompanyDivision { get; set; }
        public virtual DbSet<CompanyDivisionCostCenter> CompanyDivisionCostCenter { get; set; }
        public virtual DbSet<CompanyExpectedMargin> CompanyExpectedMargin { get; set; }
        public virtual DbSet<CompanyInspectionTypeChargeRate> CompanyInspectionTypeChargeRate { get; set; }
        public virtual DbSet<CompanyMessage> CompanyMessage { get; set; }
        public virtual DbSet<CompanyMessageType> CompanyMessageType { get; set; }
        public virtual DbSet<CompanyNote> CompanyNote { get; set; }
        public virtual DbSet<CompanyOffice> CompanyOffice { get; set; }
        public virtual DbSet<CompanyPayroll> CompanyPayroll { get; set; }
        public virtual DbSet<CompanyPayrollPeriod> CompanyPayrollPeriod { get; set; }
        public virtual DbSet<CompanyTax> CompanyTax { get; set; }
        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<ContractExchangeRate> ContractExchangeRate { get; set; }
        public virtual DbSet<ContractInvoiceAttachment> ContractInvoiceAttachment { get; set; }
        public virtual DbSet<ContractInvoiceReference> ContractInvoiceReference { get; set; }
        public virtual DbSet<ContractMessage> ContractMessage { get; set; }
        public virtual DbSet<ContractMessageType> ContractMessageType { get; set; }
        public virtual DbSet<ContractNote> ContractNote { get; set; }
        public virtual DbSet<ContractRate> ContractRate { get; set; }
        public virtual DbSet<ContractSchedule> ContractSchedule { get; set; }
        public virtual DbSet<CostAccrual> CostAccrual { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<CurrencyExchangeRate> CurrencyExchangeRate { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAddress { get; set; }
        public virtual DbSet<CustomerAssignmentReferenceType> CustomerAssignmentReferenceType { get; set; }
        public virtual DbSet<CustomerCommodity> CustomerCommodity { get; set; }
        public virtual DbSet<CustomerCompanyAccountReference> CustomerCompanyAccountReference { get; set; }
        public virtual DbSet<CustomerContact> CustomerContact { get; set; }
        public virtual DbSet<CustomerNote> CustomerNote { get; set; }
        public virtual DbSet<CustomerUserProjectAccess> CustomerUserProjectAccess { get; set; }
        public virtual DbSet<Data> Data { get; set; }
        public virtual DbSet<DataType> DataType { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<DocumentLibrary> DocumentLibrary { get; set; }
        public virtual DbSet<DocumentMongoSync> DocumentMongoSync { get; set; }
        public virtual DbSet<DocumentUploadPath> DocumentUploadPath { get; set; }
        public virtual DbSet<Draft> Draft { get; set; }
        public virtual DbSet<Email> Email { get; set; }
        public virtual DbSet<EmailConfiguration> EmailConfiguration { get; set; }
        public virtual DbSet<EmailPlaceHolder> EmailPlaceHolder { get; set; }
        public virtual DbSet<ILearnMapping> ILearnMapping { get; set; }
        public virtual DbSet<IlearnData> IlearnData { get; set; }
        public virtual DbSet<IlearnLogData> IlearnLogData { get; set; }
        public virtual DbSet<InterCompanyInvoice> InterCompanyInvoice { get; set; }
        public virtual DbSet<InterCompanyInvoiceBatch> InterCompanyInvoiceBatch { get; set; }
        public virtual DbSet<InterCompanyInvoiceItem> InterCompanyInvoiceItem { get; set; }
        public virtual DbSet<InterCompanyInvoiceItemBackup> InterCompanyInvoiceItemBackup { get; set; }
        public virtual DbSet<InterCompanyTransfer> InterCompanyTransfer { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceAssignmentReferenceType> InvoiceAssignmentReferenceType { get; set; }
        public virtual DbSet<InvoiceBatch> InvoiceBatch { get; set; }
        public virtual DbSet<InvoiceExchangeRate> InvoiceExchangeRate { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItem { get; set; }
        public virtual DbSet<InvoiceItemBackup> InvoiceItemBackup { get; set; }
        public virtual DbSet<InvoiceMessage> InvoiceMessage { get; set; }
        public virtual DbSet<InvoiceMessageType> InvoiceMessageType { get; set; }
        public virtual DbSet<InvoiceNumberRange> InvoiceNumberRange { get; set; }
        public virtual DbSet<InvoiceNumberRangeDetail> InvoiceNumberRangeDetail { get; set; }
        public virtual DbSet<LanguageInvoicePaymentTerm> LanguageInvoicePaymentTerm { get; set; }
        public virtual DbSet<LanguageReferenceType> LanguageReferenceType { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<LogData> LogData { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<ModuleActivity> ModuleActivity { get; set; }
        public virtual DbSet<ModuleDocumentType> ModuleDocumentType { get; set; }
        public virtual DbSet<NumberSequence> NumberSequence { get; set; }
        public virtual DbSet<OverrideResource> OverrideResource { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectClientNotification> ProjectClientNotification { get; set; }
        public virtual DbSet<ProjectInvoiceAssignmentReference> ProjectInvoiceAssignmentReference { get; set; }
        public virtual DbSet<ProjectInvoiceAttachment> ProjectInvoiceAttachment { get; set; }
        public virtual DbSet<ProjectMessage> ProjectMessage { get; set; }
        public virtual DbSet<ProjectMessageType> ProjectMessageType { get; set; }
        public virtual DbSet<ProjectNote> ProjectNote { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public virtual DbSet<ResourceSearch> ResourceSearch { get; set; }
        public virtual DbSet<ResourceSearchNote> ResourceSearchNote { get; set; }
        public virtual DbSet<RevenueAccrual> RevenueAccrual { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleActivity> RoleActivity { get; set; }
        public virtual DbSet<SqlauditLogDetail> SqlauditLogDetail { get; set; }
        public virtual DbSet<SqlauditLogEvent> SqlauditLogEvent { get; set; }
        public virtual DbSet<SqlauditModule> SqlauditModule { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<SupplierContact> SupplierContact { get; set; }
        public virtual DbSet<SupplierNote> SupplierNote { get; set; }
        public virtual DbSet<SupplierPurchaseOrder> SupplierPurchaseOrder { get; set; }
        public virtual DbSet<SupplierPurchaseOrderNote> SupplierPurchaseOrderNote { get; set; }
        public virtual DbSet<SupplierPurchaseOrderSubSupplier> SupplierPurchaseOrderSubSupplier { get; set; }
        public virtual DbSet<SystemSetting> SystemSetting { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<TaxonomyBusinessUnit> TaxonomyBusinessUnit { get; set; }
        public virtual DbSet<TaxonomyService> TaxonomyService { get; set; }
        public virtual DbSet<TaxonomySubCategory> TaxonomySubCategory { get; set; }
        public virtual DbSet<TechnicalSpecialist> TechnicalSpecialist { get; set; }
        public virtual DbSet<TechnicalSpecialistCalendar> TechnicalSpecialistCalendar { get; set; }
        public virtual DbSet<TechnicalSpecialistCertificationAndTraining> TechnicalSpecialistCertificationAndTraining { get; set; }
        public virtual DbSet<TechnicalSpecialistCodeAndStandard> TechnicalSpecialistCodeAndStandard { get; set; }
        public virtual DbSet<TechnicalSpecialistCommodityEquipmentKnowledge> TechnicalSpecialistCommodityEquipmentKnowledge { get; set; }
        public virtual DbSet<TechnicalSpecialistComputerElectronicKnowledge> TechnicalSpecialistComputerElectronicKnowledge { get; set; }
        public virtual DbSet<TechnicalSpecialistContact> TechnicalSpecialistContact { get; set; }
        public virtual DbSet<TechnicalSpecialistCustomerApproval> TechnicalSpecialistCustomerApproval { get; set; }
        public virtual DbSet<TechnicalSpecialistCustomers> TechnicalSpecialistCustomers { get; set; }
        public virtual DbSet<TechnicalSpecialistEducationalQualification> TechnicalSpecialistEducationalQualification { get; set; }
        public virtual DbSet<TechnicalSpecialistLanguageCapability> TechnicalSpecialistLanguageCapability { get; set; }
        public virtual DbSet<TechnicalSpecialistNote> TechnicalSpecialistNote { get; set; }
        public virtual DbSet<TechnicalSpecialistPayRate> TechnicalSpecialistPayRate { get; set; }
        public virtual DbSet<TechnicalSpecialistPaySchedule> TechnicalSpecialistPaySchedule { get; set; }
        public virtual DbSet<TechnicalSpecialistStamp> TechnicalSpecialistStamp { get; set; }
        public virtual DbSet<TechnicalSpecialistTaxonomy> TechnicalSpecialistTaxonomy { get; set; }
        public virtual DbSet<TechnicalSpecialistTaxonomyHistory> TechnicalSpecialistTaxonomyHistory { get; set; }
        public virtual DbSet<TechnicalSpecialistTimeOffRequest> TechnicalSpecialistTimeOffRequest { get; set; }
        public virtual DbSet<TechnicalSpecialistTrainingAndCompetency> TechnicalSpecialistTrainingAndCompetency { get; set; }
        public virtual DbSet<TechnicalSpecialistTrainingAndCompetencyType> TechnicalSpecialistTrainingAndCompetencyType { get; set; }
        public virtual DbSet<TechnicalSpecialistWorkHistory> TechnicalSpecialistWorkHistory { get; set; }
        public virtual DbSet<TimeOffRequestCategory> TimeOffRequestCategory { get; set; }
        public virtual DbSet<Timesheet> Timesheet { get; set; }
        public virtual DbSet<TimesheetHistory> TimesheetHistory { get; set; }
        public virtual DbSet<TimesheetInterCompanyDiscount> TimesheetInterCompanyDiscount { get; set; }
        public virtual DbSet<TimesheetNote> TimesheetNote { get; set; }
        public virtual DbSet<TimesheetReference> TimesheetReference { get; set; }
        public virtual DbSet<TimesheetTechnicalSpecialist> TimesheetTechnicalSpecialist { get; set; }
        public virtual DbSet<TimesheetTechnicalSpecialistAccountItemConsumable> TimesheetTechnicalSpecialistAccountItemConsumable { get; set; }
        public virtual DbSet<TimesheetTechnicalSpecialistAccountItemExpense> TimesheetTechnicalSpecialistAccountItemExpense { get; set; }
        public virtual DbSet<TimesheetTechnicalSpecialistAccountItemTime> TimesheetTechnicalSpecialistAccountItemTime { get; set; }
        public virtual DbSet<TimesheetTechnicalSpecialistAccountItemTravel> TimesheetTechnicalSpecialistAccountItemTravel { get; set; }
        public virtual DbSet<Token> Token { get; set; }
        public virtual DbSet<TokenMessage> TokenMessage { get; set; }
        public virtual DbSet<UnpaidStatusReason> UnpaidStatusReason { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
        public virtual DbSet<Visit> Visit { get; set; }
        public virtual DbSet<VisitHistory> VisitHistory { get; set; }
        public virtual DbSet<VisitInterCompanyDiscount> VisitInterCompanyDiscount { get; set; }
        public virtual DbSet<VisitNote> VisitNote { get; set; }
        public virtual DbSet<VisitReference> VisitReference { get; set; }
        public virtual DbSet<VisitRejectedCount> VisitRejectedCount { get; set; }
        public virtual DbSet<VisitSupplierPerformance> VisitSupplierPerformance { get; set; }
        public virtual DbSet<VisitTechnicalSpecialist> VisitTechnicalSpecialist { get; set; }
        public virtual DbSet<VisitTechnicalSpecialistAccountItemConsumable> VisitTechnicalSpecialistAccountItemConsumable { get; set; }
        public virtual DbSet<VisitTechnicalSpecialistAccountItemExpense> VisitTechnicalSpecialistAccountItemExpense { get; set; }
        public virtual DbSet<VisitTechnicalSpecialistAccountItemTime> VisitTechnicalSpecialistAccountItemTime { get; set; }
        public virtual DbSet<VisitTechnicalSpecialistAccountItemTravel> VisitTechnicalSpecialistAccountItemTravel { get; set; }

        // Unable to generate entity type for table 'admin.History'. Please see the warning messages.
        // Unable to generate entity type for table 'finance.CreditNoteReasonIDforCreditNoteReasonDelete'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=192.168.54.108;Database=Evo2DecUAT;user=sa;password=Ev02sql16;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("Activity", "security");

                entity.HasIndex(e => new { e.ApplicationId, e.Code, e.Name })
                    .HasName("IX_Activity_AppId_Code_Name")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Activity_Activity_ApplicationId");
            });

            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.ToTable("Announcement", "common");

                entity.Property(e => e.BackgroundColour).HasMaxLength(20);

                entity.Property(e => e.DisplayFrom).HasColumnType("datetime");

                entity.Property(e => e.DisplayTill).HasColumnType("datetime");

                entity.Property(e => e.EvolutionLockMessage).HasMaxLength(1000);

                entity.Property(e => e.Header)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.Text).HasMaxLength(1000);

                entity.Property(e => e.TextColour).HasMaxLength(20);
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application", "security");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ApplicationMenu>(entity =>
            {
                entity.ToTable("ApplicationMenu", "security");

                entity.HasIndex(e => new { e.ApplicationId, e.ModuleId, e.MenuName })
                    .HasName("IX_ApplicationMenu_Application_Module_MenuName")
                    .IsUnique();

                entity.Property(e => e.ActivitiesCode).HasMaxLength(500);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.MenuName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationMenu)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.ApplicationMenu)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.ToTable("Assignment", "assignment");

                entity.HasIndex(e => e.ContractCompanyCoordinatorId);

                entity.HasIndex(e => e.CustomerAssignmentContactId);

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.OperatingCompanyCoordinatorId);

                entity.HasIndex(e => e.SupplierPurchaseOrderId);

                entity.HasIndex(e => new { e.OperatingCompanyCoordinatorId, e.OperatingCompanyId })
                    .HasName("IX_ASSIGNMENT_OPERATINGCOMPANYID");

                entity.HasIndex(e => new { e.Id, e.AssignmentNumber, e.SupplierPurchaseOrderId, e.ContractCompanyCoordinatorId, e.OperatingCompanyCoordinatorId, e.ProjectId })
                    .HasName("IX_Project_User");

                entity.HasIndex(e => new { e.Id, e.ContractCompanyId, e.ContractCompanyCoordinatorId, e.OperatingCompanyCoordinatorId, e.AssignmentStatus, e.OperatingCompanyId })
                    .HasName("IX_Assignment_AssignmentStatus_OperatingCompanyId");

                entity.HasIndex(e => new { e.Id, e.AssignmentNumber, e.ContractCompanyId, e.ContractCompanyCoordinatorId, e.OperatingCompanyId, e.OperatingCompanyCoordinatorId, e.ProjectId })
                    .HasName("IX_Assignment_ProjectId");

                entity.HasIndex(e => new { e.Id, e.ProjectId, e.SupplierPurchaseOrderId, e.AssignmentNumber, e.ContractCompanyCoordinatorId, e.OperatingCompanyId, e.OperatingCompanyCoordinatorId, e.IsFirstVisit, e.AssignmentStatus, e.CreatedDate, e.AssignmentReference, e.IsAssignmentComplete })
                    .HasName("IX_ASSIGNMENTCOMPLETE");

                entity.HasIndex(e => new { e.Id, e.ProjectId, e.AssignmentReference, e.AssignmentNumber, e.IsAssignmentComplete, e.AssignmentType, e.SupplierPurchaseOrderId, e.ContractCompanyId, e.ContractCompanyCoordinatorId, e.OperatingCompanyId, e.OperatingCompanyCoordinatorId, e.IsCustomerFormatReportRequired, e.CreatedDate, e.FirstVisitTimesheetStartDate, e.AssignmentStatus })
                    .HasName("IX_AssignmentStatus_Search");

                entity.Property(e => e.AssignmentReference).HasMaxLength(75);

                entity.Property(e => e.AssignmentStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((2))");

                entity.Property(e => e.AssignmentType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.BudgetHours).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BudgetHoursWarning).HasDefaultValueSql("((0))");

                entity.Property(e => e.BudgetValue).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DocumentStatus)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.FirstVisitTimesheetEndDate).HasColumnType("datetime");

                entity.Property(e => e.FirstVisitTimesheetStartDate).HasColumnType("datetime");

                entity.Property(e => e.FirstVisitTimesheetStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.IsCustomerFormatReportRequired).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsEformReportRequired).HasColumnName("IsEFormReportRequired");

                entity.Property(e => e.IsOverrideOrPlo)
                    .HasColumnName("IsOverrideOrPLO")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.OperatingCompanyPinCode)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.AssignmentCompanyAddress)
                    .WithMany(p => p.Assignment)
                    .HasForeignKey(d => d.AssignmentCompanyAddressId)
                    .HasConstraintName("FK_Assignment_CompanyOffice_AssignmentCompAddressId");

                entity.HasOne(d => d.AssignmentLifecycle)
                    .WithMany(p => p.AssignmentAssignmentLifecycle)
                    .HasForeignKey(d => d.AssignmentLifecycleId)
                    .HasConstraintName("FK_Assignment_Data_AssignmentLifeCycleId");

                entity.HasOne(d => d.ContractCompanyCoordinator)
                    .WithMany(p => p.AssignmentContractCompanyCoordinator)
                    .HasForeignKey(d => d.ContractCompanyCoordinatorId)
                    .HasConstraintName("FK_Assignment_User_ContractCompCoordId");

                entity.HasOne(d => d.ContractCompany)
                    .WithMany(p => p.AssignmentContractCompany)
                    .HasForeignKey(d => d.ContractCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CustomerAssignmentContact)
                    .WithMany(p => p.Assignment)
                    .HasForeignKey(d => d.CustomerAssignmentContactId)
                    .HasConstraintName("FK_Assignment_CustomerContact");

                entity.HasOne(d => d.HostCompany)
                    .WithMany(p => p.AssignmentHostCompany)
                    .HasForeignKey(d => d.HostCompanyId)
                    .HasConstraintName("FK_Assignment_HostCompany");

                entity.HasOne(d => d.OperatingCompanyCoordinator)
                    .WithMany(p => p.AssignmentOperatingCompanyCoordinator)
                    .HasForeignKey(d => d.OperatingCompanyCoordinatorId)
                    .HasConstraintName("FK_Assignment_User_OperatingCompCoordId");

                entity.HasOne(d => d.OperatingCompanyCounty)
                    .WithMany(p => p.Assignment)
                    .HasForeignKey(d => d.OperatingCompanyCountyId)
                    .HasConstraintName("FK_Assignment_OperatingcompanyCountyId");

                entity.HasOne(d => d.OperatingCompany)
                    .WithMany(p => p.AssignmentOperatingCompany)
                    .HasForeignKey(d => d.OperatingCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assignment_OperatingCompany");

                entity.HasOne(d => d.OperatingCompanyLocation)
                    .WithMany(p => p.Assignment)
                    .HasForeignKey(d => d.OperatingCompanyLocationId)
                    .HasConstraintName("FK_Assignment_OperatingCompanyLocationId");

                entity.HasOne(d => d.OperationCompanyCountry)
                    .WithMany(p => p.Assignment)
                    .HasForeignKey(d => d.OperationCompanyCountryId)
                    .HasConstraintName("FK_Assignment_OperatingCompanyCountryId");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Assignment)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assignment_Project");

                entity.HasOne(d => d.ReviewAndModerationProcess)
                    .WithMany(p => p.AssignmentReviewAndModerationProcess)
                    .HasForeignKey(d => d.ReviewAndModerationProcessId)
                    .HasConstraintName("FK_Assignment_Data_ReviewAndModeration");

                entity.HasOne(d => d.SupplierPurchaseOrder)
                    .WithMany(p => p.Assignment)
                    .HasForeignKey(d => d.SupplierPurchaseOrderId)
                    .HasConstraintName("FK_Assignment_Assignment_SupplierPurchaseOrderId");
            });

            modelBuilder.Entity<AssignmentAdditionalExpense>(entity =>
            {
                entity.ToTable("AssignmentAdditionalExpense", "assignment");

                entity.HasIndex(e => e.AssignmentId);

                entity.HasIndex(e => e.CompanyId);

                entity.HasIndex(e => e.Currency)
                    .HasName("IX_AssignmentAdditionalExpense_CurrencyId");

                entity.HasIndex(e => e.ExpenseTypeId);

                entity.HasIndex(e => e.LastModification);

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.TotalUnit).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.AssignmentAdditionalExpense)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.AssignmentAdditionalExpense)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ExpenseType)
                    .WithMany(p => p.AssignmentAdditionalExpense)
                    .HasForeignKey(d => d.ExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AssignmentContractSchedule>(entity =>
            {
                entity.ToTable("AssignmentContractSchedule", "assignment");

                entity.HasIndex(e => e.ContractScheduleId)
                    .HasName("IX_Assignment_ContractScheduleId");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => new { e.AssignmentId, e.ContractScheduleId })
                    .HasName("IX_AssignmentContractSchedule_AssignmentId_ContractScheduleID")
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.AssignmentContractSchedule)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ContractSchedule)
                    .WithMany(p => p.AssignmentContractSchedule)
                    .HasForeignKey(d => d.ContractScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AssignmentContributionCalculation>(entity =>
            {
                entity.ToTable("AssignmentContributionCalculation", "assignment");

                entity.Property(e => e.ContractHolderPercentage).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ContractHolderValue).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CountryCompanyPercentage).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.CountryCompanyValue).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.MarkupPercentage).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.OperatingCompanyPercentage).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.OperatingCompanyValue).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.TotalContributionValue).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.AssignmentContributionCalculation)
                    .HasForeignKey(d => d.AssignmentId)
                    .HasConstraintName("FK_AssignmentContributionCalculation_Assignment");
            });

            modelBuilder.Entity<AssignmentContributionRevenueCost>(entity =>
            {
                entity.ToTable("AssignmentContributionRevenueCost", "assignment");

                entity.HasIndex(e => e.AssignmentContributionCalculationId);

                entity.HasIndex(e => e.LastModification);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.SectionType)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.Value).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.AssignmentContributionCalculation)
                    .WithMany(p => p.AssignmentContributionRevenueCost)
                    .HasForeignKey(d => d.AssignmentContributionCalculationId)
                    .HasConstraintName("FK_AssignmentContributionRevenueCost_AssignmentContributionCalculation");
            });

            modelBuilder.Entity<AssignmentHistory>(entity =>
            {
                entity.ToTable("AssignmentHistory", "assignment");

                entity.HasIndex(e => e.AssignmentId);

                entity.Property(e => e.AssignmentHistoryDateTime).HasColumnType("datetime");

                entity.Property(e => e.ChangedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.AssignmentHistory)
                    .HasForeignKey(d => d.AssignmentId)
                    .HasConstraintName("FK_AssignmentHistory_Assignment");

                entity.HasOne(d => d.HistoryItem)
                    .WithMany(p => p.AssignmentHistory)
                    .HasForeignKey(d => d.HistoryItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignmentHistory_HistoryItem");
            });

            modelBuilder.Entity<AssignmentInterCompanyDiscount>(entity =>
            {
                entity.ToTable("AssignmentInterCompanyDiscount", "assignment");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => new { e.AssignmentId, e.DiscountType, e.CompanyId });

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.DiscountType)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.AssignmentInterCompanyDiscount)
                    .HasForeignKey(d => d.AssignmentId)
                    .HasConstraintName("FK_AssignmentDefaultInterCompanyDiscount_Assignment");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.AssignmentInterCompanyDiscount)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_AssignmentDefaultInterCompanyDiscount_Company");
            });

            modelBuilder.Entity<AssignmentMessage>(entity =>
            {
                entity.ToTable("AssignmentMessage", "assignment");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => new { e.AssignmentId, e.MessageTypeId })
                    .HasName("IX_AssignmentMessage_MessageTypeId");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.AssignmentMessage)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignmentMessage_Assignment_assignmentId");

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.AssignmentMessage)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignmentMessage_AssignmentMessageType");
            });

            modelBuilder.Entity<AssignmentMessageType>(entity =>
            {
                entity.ToTable("AssignmentMessageType", "assignment");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AssignmentNote>(entity =>
            {
                entity.ToTable("AssignmentNote", "assignment");

                entity.HasIndex(e => e.AssignmentId)
                    .HasName("IX_Assignment_Note");

                entity.HasIndex(e => e.LastModification);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.AssignmentNote)
                    .HasForeignKey(d => d.AssignmentId)
                    .HasConstraintName("FK_AssignmentNote_Assignment");
            });

            modelBuilder.Entity<AssignmentReference>(entity =>
            {
                entity.ToTable("AssignmentReference", "assignment");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => new { e.AssignmentId, e.AssignmentReferenceTypeId })
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ReferenceValue)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.AssignmentReferenceNavigation)
                    .HasForeignKey(d => d.AssignmentId)
                    .HasConstraintName("FK_AssignmentReference_Assignment");

                entity.HasOne(d => d.AssignmentReferenceType)
                    .WithMany(p => p.AssignmentReference)
                    .HasForeignKey(d => d.AssignmentReferenceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AssignmentSubSupplier>(entity =>
            {
                entity.ToTable("AssignmentSubSupplier", "assignment");

                entity.HasIndex(e => e.AssignmentId);

                entity.HasIndex(e => e.LastModification);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.SupplierType).HasMaxLength(1);

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.AssignmentSubSupplier)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignmentSubSupplier_Assignment");

                entity.HasOne(d => d.SupplierContact)
                    .WithMany(p => p.AssignmentSubSupplier)
                    .HasForeignKey(d => d.SupplierContactId)
                    .HasConstraintName("FK_AssignmentSubSupplier_SupplierContact");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.AssignmentSubSupplier)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_AssignmentSubSupplier_SupplierId");
            });

            modelBuilder.Entity<AssignmentSubSupplierTechnicalSpecialist>(entity =>
            {
                entity.ToTable("AssignmentSubSupplierTechnicalSpecialist", "assignment");

                entity.HasIndex(e => e.AssignmentSubSupplierId);

                entity.HasIndex(e => e.TechnicalSpecialistId)
                    .HasName("IX_AssignmentSubSupplierTechSpeialist_TechSpecialistId");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.AssignmentSubSupplier)
                    .WithMany(p => p.AssignmentSubSupplierTechnicalSpecialist)
                    .HasForeignKey(d => d.AssignmentSubSupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignmentSubSupplier_AssignmentSubSupplierId");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.AssignmentSubSupplierTechnicalSpecialist)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assignment_AssignmentTechnicalSpecialist_Id");
            });

            modelBuilder.Entity<AssignmentTaxonomy>(entity =>
            {
                entity.ToTable("AssignmentTaxonomy", "assignment");

                entity.HasIndex(e => e.AssignmentId);

                entity.HasIndex(e => e.LastModification);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.AssignmentTaxonomy)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignmentTaxonomy_Assignment");

                entity.HasOne(d => d.TaxonomyService)
                    .WithMany(p => p.AssignmentTaxonomy)
                    .HasForeignKey(d => d.TaxonomyServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignmentTaxonomy_AssignmentTaxonomyService");
            });

            modelBuilder.Entity<AssignmentTechnicalSpecialist>(entity =>
            {
                entity.ToTable("AssignmentTechnicalSpecialist", "assignment");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.TechnicalSpecialistId)
                    .HasName("IX_PK_AssignmentTechnicalSpecialist");

                entity.HasIndex(e => new { e.AssignmentId, e.TechnicalSpecialistId })
                    .HasName("IX_AssignmentTechnicalSpecialist_AssignmentId_SpecialistId")
                    .IsUnique();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSupervisor).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.AssignmentTechnicalSpecialist)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.AssignmentTechnicalSpecialist)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AssignmentTechnicalSpecialistSchedule>(entity =>
            {
                entity.ToTable("AssignmentTechnicalSpecialistSchedule", "assignment");

                entity.HasIndex(e => e.ContractChargeScheduleId)
                    .HasName("IX_Assignment_ContractChargeScheduleId");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => new { e.TechnicalSpecialistPayScheduleId, e.ContractChargeScheduleId, e.AssignmentTechnicalSpecialistId })
                    .HasName("IX_ASSIGNMENTTECHSPECSCHEDULE_SPECID");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ScheduleNoteToPrintOnInvoice).HasMaxLength(30);

                entity.HasOne(d => d.AssignmentTechnicalSpecialist)
                    .WithMany(p => p.AssignmentTechnicalSpecialistSchedule)
                    .HasForeignKey(d => d.AssignmentTechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignmentTechnicalSpecialistSchedule_AssignmentTechnicalSpecialist");

                entity.HasOne(d => d.ContractChargeSchedule)
                    .WithMany(p => p.AssignmentTechnicalSpecialistSchedule)
                    .HasForeignKey(d => d.ContractChargeScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TechnicalSpecialistPaySchedule)
                    .WithMany(p => p.AssignmentTechnicalSpecialistSchedule)
                    .HasForeignKey(d => d.TechnicalSpecialistPayScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AssignmentTechnicalSpecialistSchedule_TechnicalSpecialistPaySchedule");
            });

            modelBuilder.Entity<Audience>(entity =>
            {
                entity.ToTable("Audience", "auth");

                entity.Property(e => e.AudienceCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AudienceName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<AuditSearch>(entity =>
            {
                entity.ToTable("AuditSearch", "audit");

                entity.Property(e => e.DisplayName).HasMaxLength(50);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ModuleName).HasMaxLength(50);

                entity.Property(e => e.SearchName).HasMaxLength(50);

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.AuditSearch)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("Fk_AuditSearch_Data_ModuleTypeId");
            });

            modelBuilder.Entity<BatchProcess>(entity =>
            {
                entity.ToTable("BatchProcess", "common");

                entity.Property(e => e.BatchId).HasColumnName("BatchID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ErrorMessage).IsUnicode(false);

                entity.Property(e => e.FileExtension)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ParamId).HasColumnName("ParamID");

                entity.Property(e => e.ReportFileName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ReportFilePath)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City", "master");

                entity.HasIndex(e => e.Name);

                entity.HasIndex(e => new { e.CountyId, e.Name })
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.County)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.CountyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client", "auth");

                entity.Property(e => e.AccessTokenExpMins).HasDefaultValueSql("((5))");

                entity.Property(e => e.ClientCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.RefreshTokenExpMins).HasDefaultValueSql("((30))");

                entity.Property(e => e.SeckretKey)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.TokenIssuer)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(N'Demo')");
            });

            modelBuilder.Entity<ClientAudience>(entity =>
            {
                entity.ToTable("ClientAudience", "auth");

                entity.HasOne(d => d.Audience)
                    .WithMany(p => p.ClientAudience)
                    .HasForeignKey(d => d.AudienceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientAudience)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CommodityEquipment>(entity =>
            {
                entity.ToTable("CommodityEquipment", "master");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Commodity)
                    .WithMany(p => p.CommodityEquipmentCommodity)
                    .HasForeignKey(d => d.CommodityId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.CommodityEquipmentEquipment)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company", "company");

                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.HasIndex(e => e.CompanyMiiwaid)
                    .HasName("IX_COMPANY_MIIWAID");

                entity.HasIndex(e => e.InvoiceCompanyName);

                entity.HasIndex(e => e.Name);

                entity.HasIndex(e => new { e.Code, e.Name });

                entity.Property(e => e.AverageTshourlycost)
                    .HasColumnName("AverageTShourlycost")
                    .HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Code).HasMaxLength(6);

                entity.Property(e => e.CognosNumber).HasMaxLength(10);

                entity.Property(e => e.CompanyMiiwaid).HasColumnName("CompanyMIIWAId");

                entity.Property(e => e.CompanyMiiwaref).HasColumnName("CompanyMIIWARef");

                entity.Property(e => e.Euvatprefix)
                    .HasColumnName("EUVATPrefix")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.GfsBu)
                    .HasColumnName("GFS_BU")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.GfsCoa)
                    .HasColumnName("GFS_COA")
                    .HasMaxLength(10);

                entity.Property(e => e.Iaregion)
                    .HasColumnName("IARegion")
                    .HasMaxLength(100);

                entity.Property(e => e.InterCompanyExpenseAccRef).HasMaxLength(20);

                entity.Property(e => e.InterCompanyRateAccRef).HasMaxLength(20);

                entity.Property(e => e.InterCompanyRoyaltyAccRef).HasMaxLength(20);

                entity.Property(e => e.InvoiceCompanyName).HasMaxLength(60);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsUseIctms).HasColumnName("IsUseICTMS");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.NativeCurrency)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Region).HasMaxLength(60);

                entity.Property(e => e.SalesTaxDescription).HasMaxLength(20);

                entity.Property(e => e.VatTaxRegistrationNo).HasMaxLength(60);

                entity.Property(e => e.VatregTextOutsideEc)
                    .HasColumnName("VATRegTextOutsideEC")
                    .HasMaxLength(135);

                entity.Property(e => e.VatregTextWithinEc)
                    .HasColumnName("VATRegTextWithinEC")
                    .HasMaxLength(135);

                entity.Property(e => e.WithholdingTaxDescription).HasMaxLength(20);

                entity.HasOne(d => d.Logo)
                    .WithMany(p => p.Company)
                    .HasForeignKey(d => d.LogoId)
                    .HasConstraintName("FK_Company_Logo_Master_Data");

                entity.HasOne(d => d.OperatingCountryNavigation)
                    .WithMany(p => p.Company)
                    .HasForeignKey(d => d.OperatingCountry)
                    .HasConstraintName("FK_Company_OpertingCountryId");
            });

            modelBuilder.Entity<CompanyChargeSchedule>(entity =>
            {
                entity.ToTable("CompanyChargeSchedule", "master");

                entity.HasIndex(e => new { e.CompanyId, e.StandardChargeScheduleId });

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyChargeSchedule)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.StandardChargeSchedule)
                    .WithMany(p => p.CompanyChargeSchedule)
                    .HasForeignKey(d => d.StandardChargeScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CompanyChgSchInspGroup>(entity =>
            {
                entity.ToTable("CompanyChgSchInspGroup", "master");

                entity.HasIndex(e => new { e.CompanyChargeScheduleId, e.StandardInspectionGroupId })
                    .HasName("IX_CompanyChgSchInspGroup")
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.CompanyChargeSchedule)
                    .WithMany(p => p.CompanyChgSchInspGroup)
                    .HasForeignKey(d => d.CompanyChargeScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyChgSchInspGroup_CompanyChargeSchedule_CompChgSchId");

                entity.HasOne(d => d.StandardInspectionGroup)
                    .WithMany(p => p.CompanyChgSchInspGroup)
                    .HasForeignKey(d => d.StandardInspectionGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyChgSchInspGroup_Data_StandardInspGrpId");
            });

            modelBuilder.Entity<CompanyChgSchInspGrpInspectionType>(entity =>
            {
                entity.ToTable("CompanyChgSchInspGrpInspectionType", "master");

                entity.HasIndex(e => new { e.CompanyChgSchInspGroupId, e.StandardInspectionTypeId })
                    .HasName("IX_CompanyChgSchInspGrpInspectionType")
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.CompanyChgSchInspGroup)
                    .WithMany(p => p.CompanyChgSchInspGrpInspectionType)
                    .HasForeignKey(d => d.CompanyChgSchInspGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyChgSchInspGrpInspectionType_CompanyChgSchInspGroup");

                entity.HasOne(d => d.StandardInspectionType)
                    .WithMany(p => p.CompanyChgSchInspGrpInspectionType)
                    .HasForeignKey(d => d.StandardInspectionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyChgSchInspGrpInspectionType_Data_StandardInspType");
            });

            modelBuilder.Entity<CompanyDivision>(entity =>
            {
                entity.ToTable("CompanyDivision", "company");

                entity.HasIndex(e => new { e.CompanyId, e.DivisionId })
                    .IsUnique();

                entity.Property(e => e.AccountReference)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyDivision)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Division)
                    .WithMany(p => p.CompanyDivision)
                    .HasForeignKey(d => d.DivisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CompanyDivisionCostCenter>(entity =>
            {
                entity.ToTable("CompanyDivisionCostCenter", "company");

                entity.HasIndex(e => e.CompanyDivisionId)
                    .HasName("IX_CompanyDivisionCostCenter_DivisionId");

                entity.HasIndex(e => new { e.CompanyDivisionId, e.Name, e.Code })
                    .HasName("IX_CompanyDivisionCostCenter_CompanyDivisionId_Code_Name");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.CompanyDivision)
                    .WithMany(p => p.CompanyDivisionCostCenter)
                    .HasForeignKey(d => d.CompanyDivisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CompanyExpectedMargin>(entity =>
            {
                entity.ToTable("CompanyExpectedMargin", "company");

                entity.HasIndex(e => new { e.CompanyId, e.MarginTypeId })
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.MinimumMargin).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyExpectedMargin)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyExpectedMargin_CompanyExpectedMargin_CompanyId");

                entity.HasOne(d => d.MarginType)
                    .WithMany(p => p.CompanyExpectedMargin)
                    .HasForeignKey(d => d.MarginTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CompanyInspectionTypeChargeRate>(entity =>
            {
                entity.ToTable("CompanyInspectionTypeChargeRate", "master");

                entity.HasIndex(e => e.CompanyChgSchInspGrpInspectionTypeId);

                entity.Property(e => e.ItemDescription).HasMaxLength(150);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.RateOffShoreOil).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.RateOnShoreNonOil).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.RateOnShoreOil).HasColumnType("decimal(16, 4)");

                entity.HasOne(d => d.CompanyChgSchInspGrpInspectionType)
                    .WithMany(p => p.CompanyInspectionTypeChargeRate)
                    .HasForeignKey(d => d.CompanyChgSchInspGrpInspectionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StandardInspectionTypeChargeRate_CompanyChgSchInspGrpInspectionType_CompChgSchInspGrpInspTypeId");

                entity.HasOne(d => d.ExpenseType)
                    .WithMany(p => p.CompanyInspectionTypeChargeRateExpenseType)
                    .HasForeignKey(d => d.ExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StandardInspectionTypeChargeRates_Data_ExpenseTypeId");

                entity.HasOne(d => d.FilmSize)
                    .WithMany(p => p.CompanyInspectionTypeChargeRateFilmSize)
                    .HasForeignKey(d => d.FilmSizeId)
                    .HasConstraintName("FK_StandardInspectionTypeChargeRates_Data_FilmSizeId");

                entity.HasOne(d => d.FilmType)
                    .WithMany(p => p.CompanyInspectionTypeChargeRateFilmType)
                    .HasForeignKey(d => d.FilmTypeId)
                    .HasConstraintName("FK_StandardInspectionTypeChargeRates_Data_FilmTypeId");

                entity.HasOne(d => d.ItemSize)
                    .WithMany(p => p.CompanyInspectionTypeChargeRateItemSize)
                    .HasForeignKey(d => d.ItemSizeId)
                    .HasConstraintName("FK_StandardInspectionTypeChargeRates_Data_ItemsizeId");

                entity.HasOne(d => d.ItemThickness)
                    .WithMany(p => p.CompanyInspectionTypeChargeRateItemThickness)
                    .HasForeignKey(d => d.ItemThicknessId)
                    .HasConstraintName("FK_StandardInspectionTypeChargeRates_Data_ItemThicknessId");
            });

            modelBuilder.Entity<CompanyMessage>(entity =>
            {
                entity.ToTable("CompanyMessage", "company");

                entity.HasIndex(e => e.CompanyId);

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.MessageTypeId);

                entity.HasIndex(e => new { e.MessageTypeId, e.Evo1Id });

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDefaultMessage).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyMessage)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.CompanyMessage)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CompanyMessageType>(entity =>
            {
                entity.ToTable("CompanyMessageType", "company");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CompanyNote>(entity =>
            {
                entity.ToTable("CompanyNote", "company");

                entity.HasIndex(e => e.CompanyId);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyNote)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompanyNote_Company");
            });

            modelBuilder.Entity<CompanyOffice>(entity =>
            {
                entity.ToTable("CompanyOffice", "company");

                entity.HasIndex(e => e.CompanyId);

                entity.Property(e => e.AccountRef)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.OfficeName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.PostalCode).HasMaxLength(15);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CompanyOffice)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_CompanyOffice_CityId");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyOffice)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CompanyOffice)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_CompanyOffice_Country");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.CompanyOffice)
                    .HasForeignKey(d => d.CountyId)
                    .HasConstraintName("FK_CompanyOffice_County");
            });

            modelBuilder.Entity<CompanyPayroll>(entity =>
            {
                entity.ToTable("CompanyPayroll", "company");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.ExportPrefix)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyPayroll)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CompanyPayrollPeriod>(entity =>
            {
                entity.ToTable("CompanyPayrollPeriod", "company");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => new { e.CompanyPayrollId, e.PeriodName });

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PeriodName)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.PeriodStatus).HasMaxLength(1);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.CompanyPayroll)
                    .WithMany(p => p.CompanyPayrollPeriod)
                    .HasForeignKey(d => d.CompanyPayrollId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CompanyTax>(entity =>
            {
                entity.ToTable("CompanyTax", "company");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.IsIcinv).HasColumnName("IsICInv");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Rate).HasColumnType("decimal(9, 4)");

                entity.Property(e => e.TaxType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyTax)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contract", "contract");

                entity.HasIndex(e => e.ContractHolderCompanyId);

                entity.HasIndex(e => e.ContractNumber);

                entity.HasIndex(e => e.ContractType);

                entity.HasIndex(e => e.CreatedDate);

                entity.HasIndex(e => e.DefaultCustomerContractContactId);

                entity.HasIndex(e => e.DefaultCustomerInvoiceContactId);

                entity.HasIndex(e => e.InvoicingCompanyId)
                    .HasName("IX_Contract_InvoiceCompanyId");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.ParentContractId);

                entity.HasIndex(e => e.Status);

                entity.HasIndex(e => new { e.StartDate, e.EndDate });

                entity.HasIndex(e => new { e.ContractHolderCompanyId, e.Status, e.EndDate })
                    .HasName("IX_Contract_Status_EndDate");

                entity.HasIndex(e => new { e.Id, e.ContractNumber, e.CustomerContractNumber, e.ContractHolderCompanyId, e.ContractType, e.CustomerId })
                    .HasName("IX_Contract_CustomerId");

                entity.Property(e => e.Budget).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.BudgetCurrency)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BudgetHours).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.BudgetHoursWarning).HasDefaultValueSql("((0))");

                entity.Property(e => e.ContractNumber).HasMaxLength(12);

                entity.Property(e => e.ContractType)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Crmreason)
                    .HasColumnName("CRMReason")
                    .HasMaxLength(50);

                entity.Property(e => e.Crmreference)
                    .HasColumnName("CRMReference")
                    .HasColumnType("numeric(10, 0)");

                entity.Property(e => e.CustomerContractNumber)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.DefaultInvoiceCurrency)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DefaultInvoiceGrouping)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IsCrmstatus)
                    .HasColumnName("IsCRMStatus")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsUseInvoiceDetailsFromParentContract).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ParentContractDiscountPercentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.CompanyOffice)
                    .WithMany(p => p.ContractCompanyOffice)
                    .HasForeignKey(d => d.CompanyOfficeId);

                entity.HasOne(d => d.ContractHolderCompany)
                    .WithMany(p => p.ContractContractHolderCompany)
                    .HasForeignKey(d => d.ContractHolderCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_Contract_ContractHolderCompanyId");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Contract)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DefaultCustomerContractAddress)
                    .WithMany(p => p.ContractDefaultCustomerContractAddress)
                    .HasForeignKey(d => d.DefaultCustomerContractAddressId)
                    .HasConstraintName("FK_Contract_CustomerContractAddress");

                entity.HasOne(d => d.DefaultCustomerContractContact)
                    .WithMany(p => p.ContractDefaultCustomerContractContact)
                    .HasForeignKey(d => d.DefaultCustomerContractContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DefaultCustomerInvoiceAddress)
                    .WithMany(p => p.ContractDefaultCustomerInvoiceAddress)
                    .HasForeignKey(d => d.DefaultCustomerInvoiceAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contract_CustomerInvoiceAddress");

                entity.HasOne(d => d.DefaultCustomerInvoiceContact)
                    .WithMany(p => p.ContractDefaultCustomerInvoiceContact)
                    .HasForeignKey(d => d.DefaultCustomerInvoiceContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DefaultFooterText)
                    .WithMany(p => p.ContractDefaultFooterText)
                    .HasForeignKey(d => d.DefaultFooterTextId);

                entity.HasOne(d => d.DefaultRemittanceText)
                    .WithMany(p => p.ContractDefaultRemittanceText)
                    .HasForeignKey(d => d.DefaultRemittanceTextId);

                entity.HasOne(d => d.DefaultSalesTax)
                    .WithMany(p => p.ContractDefaultSalesTax)
                    .HasForeignKey(d => d.DefaultSalesTaxId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DefaultWithholdingTax)
                    .WithMany(p => p.ContractDefaultWithholdingTax)
                    .HasForeignKey(d => d.DefaultWithholdingTaxId)
                    .HasConstraintName("FK_Contract_CompanyTax_WithholdTaxId");

                entity.HasOne(d => d.FrameworkCompanyOffice)
                    .WithMany(p => p.ContractFrameworkCompanyOffice)
                    .HasForeignKey(d => d.FrameworkCompanyOfficeId)
                    .HasConstraintName("FK_Contract_FrameworkCompanyOffice");

                entity.HasOne(d => d.FrameworkContract)
                    .WithMany(p => p.InverseFrameworkContract)
                    .HasForeignKey(d => d.FrameworkContractId);

                entity.HasOne(d => d.InvoicePaymentTerms)
                    .WithMany(p => p.Contract)
                    .HasForeignKey(d => d.InvoicePaymentTermsId)
                    .HasConstraintName("FK_Contract_Data_InvoicePaymentTermId");

                entity.HasOne(d => d.InvoicingCompany)
                    .WithMany(p => p.ContractInvoicingCompany)
                    .HasForeignKey(d => d.InvoicingCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ParentContract)
                    .WithMany(p => p.InverseParentContract)
                    .HasForeignKey(d => d.ParentContractId)
                    .HasConstraintName("FK_Contract_ParentContract");
            });

            modelBuilder.Entity<ContractExchangeRate>(entity =>
            {
                entity.ToTable("ContractExchangeRate", "contract");

                entity.HasIndex(e => new { e.ContractId, e.CurrencyFrom, e.CurrencyTo, e.EffectiveFrom })
                    .HasName("IX_ContractExchangeRate");

                entity.Property(e => e.CurrencyFrom)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CurrencyTo)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.EffectiveFrom).HasColumnType("datetime");

                entity.Property(e => e.ExchangeRate).HasColumnType("numeric(12, 6)");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.ContractExchangeRate)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ContractInvoiceAttachment>(entity =>
            {
                entity.ToTable("ContractInvoiceAttachment", "contract");

                entity.HasIndex(e => e.ContractId);

                entity.HasIndex(e => e.LastModification);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.ContractInvoiceAttachment)
                    .HasForeignKey(d => d.ContractId);

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.ContractInvoiceAttachment)
                    .HasForeignKey(d => d.DocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ContractInvoiceReference>(entity =>
            {
                entity.ToTable("ContractInvoiceReference", "contract");

                entity.HasIndex(e => e.AssignmentReferenceTypeId);

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => new { e.ContractId, e.AssignmentReferenceTypeId })
                    .HasName("IX_ContractAssignmentReference_ContractReference")
                    .IsUnique();

                entity.Property(e => e.IsAssignment).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsTimesheet).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsVisit).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.AssignmentReferenceType)
                    .WithMany(p => p.ContractInvoiceReference)
                    .HasForeignKey(d => d.AssignmentReferenceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.ContractInvoiceReference)
                    .HasForeignKey(d => d.ContractId);
            });

            modelBuilder.Entity<ContractMessage>(entity =>
            {
                entity.ToTable("ContractMessage", "contract");

                entity.HasIndex(e => new { e.ContractId, e.MessageTypeId });

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDefaultMessage).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.ContractMessage)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.ContractMessage)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ContractMessageType>(entity =>
            {
                entity.ToTable("ContractMessageType", "contract");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ContractNote>(entity =>
            {
                entity.ToTable("ContractNote", "contract");

                entity.HasIndex(e => e.ContractId);

                entity.HasIndex(e => e.LastModification);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(4000);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.ContractNote)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContractNote_Contract");
            });

            modelBuilder.Entity<ContractRate>(entity =>
            {
                entity.ToTable("ContractRate", "contract");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => new { e.Id, e.ContractScheduleId })
                    .HasName("IX_ContractRate_ContractScheduleId");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.DiscountApplied).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsPrintDescriptionOnInvoice).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Percentage).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Rate).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.StandardValue).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.HasOne(d => d.ContractSchedule)
                    .WithMany(p => p.ContractRate)
                    .HasForeignKey(d => d.ContractScheduleId);

                entity.HasOne(d => d.ExpenseType)
                    .WithMany(p => p.ContractRate)
                    .HasForeignKey(d => d.ExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContractRate_Data_ExpenceTypeId");

                entity.HasOne(d => d.StandardInspectionTypeChargeRate)
                    .WithMany(p => p.ContractRate)
                    .HasForeignKey(d => d.StandardInspectionTypeChargeRateId)
                    .HasConstraintName("FK_ContractRates_StandardInspectionTypeChargeRates");
            });

            modelBuilder.Entity<ContractSchedule>(entity =>
            {
                entity.ToTable("ContractSchedule", "contract");

                entity.HasIndex(e => e.ContractId)
                    .HasName("IX_ContractSchedule_ContractID");

                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ScheduleNoteForInvoice).HasMaxLength(20);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.ContractSchedule)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContractSchedule_ContractSchedule_ContractId");
            });

            modelBuilder.Entity<CostAccrual>(entity =>
            {
                entity.ToTable("CostAccrual", "finance");

                entity.Property(e => e.AdditionalCompany1).HasMaxLength(60);

                entity.Property(e => e.AdditionalCompany2).HasMaxLength(60);

                entity.Property(e => e.BusinessUnit).HasMaxLength(50);

                entity.Property(e => e.ChargeReference).HasMaxLength(50);

                entity.Property(e => e.ContractHoldingCompany).HasMaxLength(60);

                entity.Property(e => e.ContractHoldingCoordinator).HasMaxLength(50);

                entity.Property(e => e.CostCentre).HasMaxLength(100);

                entity.Property(e => e.Currency).HasMaxLength(3);

                entity.Property(e => e.CurrentInvoicingStatus).HasMaxLength(1);

                entity.Property(e => e.Customer).HasMaxLength(60);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DiscountType).HasMaxLength(1);

                entity.Property(e => e.Division).HasMaxLength(30);

                entity.Property(e => e.DivisionRef).HasMaxLength(15);

                entity.Property(e => e.Etype)
                    .HasColumnName("EType")
                    .HasMaxLength(1);

                entity.Property(e => e.ExchangeRate).HasColumnType("decimal(16, 6)");

                entity.Property(e => e.HostCompany).HasMaxLength(60);

                entity.Property(e => e.Invoicedate).HasColumnType("datetime");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.MiassignmentNo).HasColumnName("MIAssignmentNo");

                entity.Property(e => e.MicontractNumber)
                    .HasColumnName("MIContractNumber")
                    .HasMaxLength(12);

                entity.Property(e => e.MiprojectNumber).HasColumnName("MIProjectNumber");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.NativeCurrency).HasMaxLength(3);

                entity.Property(e => e.NetExpenseTotal).HasColumnType("decimal(38, 6)");

                entity.Property(e => e.NetFeeTotal).HasColumnType("numeric(38, 6)");

                entity.Property(e => e.Office).HasMaxLength(30);

                entity.Property(e => e.OperatingCompany).HasMaxLength(60);

                entity.Property(e => e.OperatingCoordinator).HasMaxLength(50);

                entity.Property(e => e.ParentCompany).HasMaxLength(60);

                entity.Property(e => e.Ptype)
                    .IsRequired()
                    .HasColumnName("PType")
                    .HasMaxLength(1);

                entity.Property(e => e.Rate).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.ReportNumber).HasMaxLength(65);

                entity.Property(e => e.SupplierNumber).HasMaxLength(150);

                entity.Property(e => e.TechnicalSpecialistStatus).HasMaxLength(60);

                entity.Property(e => e.Type).HasMaxLength(15);

                entity.Property(e => e.Units).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.VisitDatePeriod).HasMaxLength(30);

                entity.Property(e => e.VisitStatus).HasMaxLength(28);

                entity.HasOne(d => d.Token)
                    .WithMany(p => p.CostAccrual)
                    .HasForeignKey(d => d.TokenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CostAccrual_Token");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country", "master");

                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.Euvatprefix)
                    .HasColumnName("EUVATPrefix")
                    .HasMaxLength(6);

                entity.Property(e => e.IsEumember)
                    .HasColumnName("IsEUMember")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsGccmember)
                    .HasColumnName("IsGCCMember")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.HasOne(d => d.Regon)
                    .WithMany(p => p.Country)
                    .HasForeignKey(d => d.RegonId)
                    .HasConstraintName("FK_Country_Data_RegionId");
            });

            modelBuilder.Entity<County>(entity =>
            {
                entity.ToTable("County", "master");

                entity.HasIndex(e => new { e.Name, e.CountryId })
                    .HasName("IX_County_Name_Country_Id")
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.County)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_County_County_CountryId");
            });

            modelBuilder.Entity<CurrencyExchangeRate>(entity =>
            {
                entity.ToTable("CurrencyExchangeRate", "master");

                entity.HasIndex(e => new { e.CurrencyId, e.EffectiveDate })
                    .HasName("IX_CurrencyExchangeRate");

                entity.HasIndex(e => new { e.EffectiveDate, e.ExchangeRateUsd, e.CurrencyId })
                    .HasName("IX_CurrencyExchangeRate_CurrencyId");

                entity.Property(e => e.AverageGbp)
                    .HasColumnName("AverageGBP")
                    .HasColumnType("decimal(16, 6)");

                entity.Property(e => e.AverageUsd)
                    .HasColumnName("AverageUSD")
                    .HasColumnType("decimal(16, 6)");

                entity.Property(e => e.EffectiveDate).HasColumnType("datetime");

                entity.Property(e => e.ExchangeRateGbp)
                    .HasColumnName("ExchangeRateGBP")
                    .HasColumnType("decimal(16, 6)");

                entity.Property(e => e.ExchangeRateUsd)
                    .HasColumnName("ExchangeRateUSD")
                    .HasColumnType("decimal(16, 6)");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.CurrencyExchangeRate)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencyExchangeRate_Currency");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer", "customer");

                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.Miiwaid);

                entity.HasIndex(e => e.MiiwaparentId);

                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Code).HasMaxLength(7);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.Miiwaid).HasColumnName("MIIWAId");

                entity.Property(e => e.MiiwaparentId).HasColumnName("MIIWAParentId");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.ParentName)
                    .IsRequired()
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.ToTable("CustomerAddress", "customer");

                entity.HasIndex(e => e.CityId)
                    .HasName("IX_CustomerAddress_City");

                entity.HasIndex(e => e.CustomerId);

                entity.HasIndex(e => e.LastModification);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Euvatprefix)
                    .HasColumnName("EUVATPrefix")
                    .HasMaxLength(6);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PostalCode).HasMaxLength(15);

                entity.Property(e => e.VatTaxRegistrationNo).HasMaxLength(60);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CustomerAddress)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_CustomerAddress_City");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerAddress)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CustomerAssignmentReferenceType>(entity =>
            {
                entity.ToTable("CustomerAssignmentReferenceType", "customer");

                entity.HasIndex(e => e.AssignmentReferenceId)
                    .HasName("IX_Customer_AssignmentReferenceType_LNK_AssignmentReferenceId");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("IX_Customer_AssignmentReferenceType_LNK_CustomerId");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.AssignmentReference)
                    .WithMany(p => p.CustomerAssignmentReferenceType)
                    .HasForeignKey(d => d.AssignmentReferenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerAssignmentReferenceType)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CustomerCommodity>(entity =>
            {
                entity.ToTable("CustomerCommodity", "master");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Commodity)
                    .WithMany(p => p.CustomerCommodity)
                    .HasForeignKey(d => d.CommodityId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerCommodity)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CustomerCompanyAccountReference>(entity =>
            {
                entity.ToTable("CustomerCompanyAccountReference", "customer");

                entity.HasIndex(e => e.CompanyId);

                entity.HasIndex(e => e.CustomerId);

                entity.HasIndex(e => e.LastModification);

                entity.Property(e => e.AccountReference)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CustomerCompanyAccountReference)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerCompanyAccountReference)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerCompanyAccountRefrence_Customer_CustomerId");
            });

            modelBuilder.Entity<CustomerContact>(entity =>
            {
                entity.ToTable("CustomerContact", "customer");

                entity.HasIndex(e => e.ContactName);

                entity.HasIndex(e => e.EmailAddress);

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => new { e.Id, e.Salutation, e.Position, e.ContactName, e.TelephoneNumber, e.FaxNumber, e.MobileNumber, e.EmailAddress, e.OtherContactDetails, e.LastModification, e.UpdateCount, e.ModifiedBy, e.LoginName, e.CustomerAddressId })
                    .HasName("IX_Customer_CustomerAddress");

                entity.Property(e => e.ContactName).HasMaxLength(60);

                entity.Property(e => e.EmailAddress).HasMaxLength(60);

                entity.Property(e => e.FaxNumber).HasMaxLength(60);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.LoginName).HasMaxLength(50);

                entity.Property(e => e.MobileNumber).HasMaxLength(60);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.OtherContactDetails).HasMaxLength(150);

                entity.Property(e => e.Position).HasMaxLength(60);

                entity.Property(e => e.Salutation).HasMaxLength(50);

                entity.Property(e => e.TelephoneNumber)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.HasOne(d => d.CustomerAddress)
                    .WithMany(p => p.CustomerContact)
                    .HasForeignKey(d => d.CustomerAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerContact_AddressId");
            });

            modelBuilder.Entity<CustomerNote>(entity =>
            {
                entity.ToTable("CustomerNote", "customer");

                entity.HasIndex(e => e.CustomerId);

                entity.HasIndex(e => e.LastModification);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(4000);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerNote)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<CustomerUserProjectAccess>(entity =>
            {
                entity.ToTable("CustomerUserProjectAccess", "security");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.CustomerUserProjectAccess)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_CustomerUserProjectAccess_Project");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CustomerUserProjectAccess)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_CustomerUserProjectAccess_User");
            });

            modelBuilder.Entity<Data>(entity =>
            {
                entity.ToTable("Data", "master");

                entity.HasIndex(e => e.Name);

                entity.HasIndex(e => new { e.Id, e.Code })
                    .HasName("IX_Data_Code");

                entity.HasIndex(e => new { e.IsActive, e.MasterDataTypeId })
                    .HasName("IX_Data_MasterDataTypeId");

                entity.HasIndex(e => new { e.MasterDataTypeId, e.Code, e.Name })
                    .HasName("IX_Data_MasterDataTypeID_Code_Name")
                    .IsUnique();

                entity.Property(e => e.ChargeReference).HasMaxLength(50);

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.InterCompanyType)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceType)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsAlchiddenForNewFacility)
                    .HasColumnName("IsALCHiddenForNewFacility")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsArs).HasColumnName("IsARS");

                entity.Property(e => e.IsEmployed).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PayReference).HasMaxLength(50);

                entity.Property(e => e.PayrollExportPrefix)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.MasterDataType)
                    .WithMany(p => p.Data)
                    .HasForeignKey(d => d.MasterDataTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DataType>(entity =>
            {
                entity.ToTable("DataType", "master");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_DataType_Name")
                    .IsUnique();

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("Document", "common");

                entity.HasIndex(e => e.DocumentUniqueName)
                    .HasName("IX_Document_DocumentUniqueName1")
                    .IsUnique();

                entity.HasIndex(e => e.Evoid)
                    .HasName("idx_DocumentEvoId");

                entity.HasIndex(e => e.ModuleRefCode);

                entity.HasIndex(e => e.SubModuleRefCode);

                entity.HasIndex(e => new { e.FilePath, e.DocumentUniqueName, e.IsDeleted })
                    .HasName("IX_DocumentUniqueName_IsDeleted");

                entity.HasIndex(e => new { e.ModuleCode, e.ModuleRefCode, e.SubModuleRefCode });

                entity.HasIndex(e => new { e.DocumentType, e.ModuleRefCode, e.CreatedBy, e.ModuleCode })
                    .HasName("IX_Document_ModuleTypeCode");

                entity.HasIndex(e => new { e.Id, e.DocumentUniqueName, e.CreatedDate, e.Status, e.ModuleRefCode })
                    .HasName("IX_Document_CreatedDate_Status_ModuleREfCode");

                entity.Property(e => e.ApprovalDate).HasColumnType("datetime");

                entity.Property(e => e.ApprovedBy).HasMaxLength(50);

                entity.Property(e => e.Comments).HasMaxLength(4000);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DocumentName).HasMaxLength(255);

                entity.Property(e => e.DocumentTitle).HasMaxLength(4000);

                entity.Property(e => e.DocumentType).HasMaxLength(50);

                entity.Property(e => e.DocumentUniqueName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Evoid).HasColumnName("evoid");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ModuleCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ModuleRefCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.SubModuleRefCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Coordinator)
                    .WithMany(p => p.Document)
                    .HasForeignKey(d => d.CoordinatorId);
            });

            modelBuilder.Entity<DocumentLibrary>(entity =>
            {
                entity.ToTable("DocumentLibrary", "document");

                entity.Property(e => e.LastEmailSentOn).HasColumnType("datetime");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Owner).HasMaxLength(255);

                entity.Property(e => e.ReviewDate).HasColumnType("datetime");

                entity.Property(e => e.ReviewStatus).HasMaxLength(255);

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentLibrary)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentLibrary_DocumentId");
            });

            modelBuilder.Entity<DocumentMongoSync>(entity =>
            {
                entity.ToTable("DocumentMongoSync", "common");

                entity.HasIndex(e => e.DocumentUniqueName)
                    .HasName("IX_Document_DocumentUniqueName")
                    .IsUnique();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DocumentUniqueName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Reason).HasMaxLength(4000);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DocumentUploadPath>(entity =>
            {
                entity.ToTable("DocumentUploadPath", "common");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FolderPath)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ServerName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Draft>(entity =>
            {
                entity.ToTable("Draft", "common");

                entity.Property(e => e.AssignedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AssignedOn).HasColumnType("datetime");

                entity.Property(e => e.AssignedTo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTo).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DraftId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Moduletype)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SerilizationType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Draft)
                    .HasForeignKey(d => d.CompanyId);
            });

            modelBuilder.Entity<Email>(entity =>
            {
                entity.ToTable("Email", "alert");

                entity.Property(e => e.BodyContent)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EmailStatus).HasMaxLength(25);

                entity.Property(e => e.EmailType)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.FromEmail).HasMaxLength(255);

                entity.Property(e => e.LastAttemptOn).HasColumnType("datetime");

                entity.Property(e => e.ModuleCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ModuleEmailRefCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PrivateKey).HasMaxLength(500);

                entity.Property(e => e.StatusReason).HasColumnType("ntext");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(160);

                entity.Property(e => e.ToEmail).IsRequired();

                entity.Property(e => e.Token).HasMaxLength(50);
            });

            modelBuilder.Entity<EmailConfiguration>(entity =>
            {
                entity.ToTable("EmailConfiguration", "admin");

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.Property(e => e.ConfigType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Server)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ServerUser)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ServerUserPassword)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<EmailPlaceHolder>(entity =>
            {
                entity.ToTable("EmailPlaceHolder", "admin");

                entity.Property(e => e.DisplayName).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ModuleName).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ILearnMapping>(entity =>
            {
                entity.ToTable("iLearnMapping", "techspecialist");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IlearnId)
                    .HasColumnName("ILearnID")
                    .HasMaxLength(255);

                entity.Property(e => e.IlearnObjectId)
                    .HasColumnName("ILearnObjectID")
                    .HasMaxLength(255);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.TrainingType).HasMaxLength(255);
            });

            modelBuilder.Entity<IlearnData>(entity =>
            {
                entity.ToTable("ILearnData", "techspecialist");

                entity.Property(e => e.CompletedDate).HasColumnType("datetime");

                entity.Property(e => e.Score).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.TrainingHours).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.TrainingObjectId)
                    .HasColumnName("TrainingObjectID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TrainingTitle)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IlearnLogData>(entity =>
            {
                entity.ToTable("ILearnLogData", "techspecialist");

                entity.Property(e => e.CompletedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ErrorMessage).HasMaxLength(3000);

                entity.Property(e => e.IlearnId)
                    .IsRequired()
                    .HasColumnName("ILearnID")
                    .HasMaxLength(510);

                entity.Property(e => e.IlearnObjectId)
                    .HasColumnName("ILearnObjectID")
                    .HasMaxLength(255);

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.TrainingType).HasMaxLength(255);
            });

            modelBuilder.Entity<InterCompanyInvoice>(entity =>
            {
                entity.ToTable("InterCompanyInvoice", "finance");

                entity.Property(e => e.Currency).HasMaxLength(20);

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNumber).HasMaxLength(20);

                entity.Property(e => e.InvoiceTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceType)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.IsSettledThroughIctms).HasColumnName("IsSettledThroughICTMS");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.SalesTaxCode).HasMaxLength(10);

                entity.Property(e => e.SalesTaxId).HasDefaultValueSql("((0))");

                entity.Property(e => e.SalesTaxName).HasMaxLength(50);

                entity.Property(e => e.SalesTaxTotal).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.SalesTaxValue).HasColumnType("decimal(9, 4)");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.TotalInvoiceAmount).HasColumnType("decimal(20, 4)");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.InterCompanyInvoice)
                    .HasForeignKey(d => d.BatchId)
                    .HasConstraintName("FK_InterCompanyInvoice_InterCompanyInvoiceBatch");

                entity.HasOne(d => d.CustomerInvoice)
                    .WithMany(p => p.InterCompanyInvoice)
                    .HasForeignKey(d => d.CustomerInvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterCoInvoice_Invoice_CustomerInvoiceId");

                entity.HasOne(d => d.InterCompanyTransfer)
                    .WithMany(p => p.InterCompanyInvoice)
                    .HasForeignKey(d => d.InterCompanyTransferId)
                    .HasConstraintName("FK_InterCoInvoice_InterCompanyTransfer");

                entity.HasOne(d => d.RaisedAgainstCompany)
                    .WithMany(p => p.InterCompanyInvoiceRaisedAgainstCompany)
                    .HasForeignKey(d => d.RaisedAgainstCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterCoInvoice_RaisedAgainstCompany");

                entity.HasOne(d => d.RaisedByCompany)
                    .WithMany(p => p.InterCompanyInvoiceRaisedByCompany)
                    .HasForeignKey(d => d.RaisedByCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterCoInvoice_RaisedByCompany");

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.InterCompanyInvoice)
                    .HasForeignKey(d => d.SalesTaxId);
            });

            modelBuilder.Entity<InterCompanyInvoiceBatch>(entity =>
            {
                entity.ToTable("InterCompanyInvoiceBatch", "finance");

                entity.Property(e => e.BatchReference)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.BatchStatus)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.InterCompanyInvoiceBatch)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterCompanyInvoiceBatch_Company");
            });

            modelBuilder.Entity<InterCompanyInvoiceItem>(entity =>
            {
                entity.ToTable("InterCompanyInvoiceItem", "finance");

                entity.Property(e => e.Contribution).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.ItemTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ReferenceValue).HasMaxLength(150);

                entity.Property(e => e.SalesTaxCode).HasMaxLength(10);

                entity.Property(e => e.SalesTaxId).HasDefaultValueSql("((0))");

                entity.Property(e => e.SalesTaxName).HasMaxLength(50);

                entity.Property(e => e.SalesTaxTotal).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.SalesTaxValue).HasColumnType("decimal(9, 4)");

                entity.Property(e => e.TotalInterCompanyInvoiceAmount).HasColumnType("decimal(20, 4)");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.InterCompanyInvoiceItem)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterCoInvoiceItem_Assignment");

                entity.HasOne(d => d.InterCompanyInvoice)
                    .WithMany(p => p.InterCompanyInvoiceItem)
                    .HasForeignKey(d => d.InterCompanyInvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterCoInvoiceItem_InterCoInvoice");

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.InterCompanyInvoiceItem)
                    .HasForeignKey(d => d.SalesTaxId);
            });

            modelBuilder.Entity<InterCompanyInvoiceItemBackup>(entity =>
            {
                entity.ToTable("InterCompanyInvoiceItemBackup", "finance");

                entity.HasIndex(e => new { e.InterCompanyInvoiceItemId, e.SpecialistAccountItemId })
                    .HasName("IX_InterCoInvoiceItemid");

                entity.Property(e => e.Contribution).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ConvertedNet).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.ItemTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ItemType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Net).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SalesTaxCode).HasMaxLength(10);

                entity.Property(e => e.SalesTaxName).HasMaxLength(50);

                entity.Property(e => e.SalesTaxTotal).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.SalesTaxValue).HasColumnType("decimal(9, 4)");

                entity.Property(e => e.Units).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.InterCompanyInvoiceItem)
                    .WithMany(p => p.InterCompanyInvoiceItemBackup)
                    .HasForeignKey(d => d.InterCompanyInvoiceItemId)
                    .HasConstraintName("FK_InterCoInvoiceItemBackup_InterCoInvoiceItem");

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.InterCompanyInvoiceItemBackup)
                    .HasForeignKey(d => d.SalesTaxId);
            });

            modelBuilder.Entity<InterCompanyTransfer>(entity =>
            {
                entity.ToTable("InterCompanyTransfer", "finance");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.FromCompanyMiiwaref).HasColumnName("FromCompanyMIIWARef");

                entity.Property(e => e.InterCompanyInvoiceStatus).HasMaxLength(1);

                entity.Property(e => e.InterCompanyTransferType)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.NativeCurrency)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.NativeTotalValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NativeTransferValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ToCompanyAccountRef).HasMaxLength(20);

                entity.Property(e => e.ToCompanyMiiwaref).HasColumnName("ToCompanyMIIWARef");

                entity.Property(e => e.TotalValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransferDate).HasColumnType("datetime");

                entity.Property(e => e.TransferValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.WorkFromDate).HasColumnType("datetime");

                entity.Property(e => e.WorkToDate).HasColumnType("datetime");

                entity.HasOne(d => d.FromCompany)
                    .WithMany(p => p.InterCompanyTransferFromCompany)
                    .HasForeignKey(d => d.FromCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterCompanyTransfer_FromCompany");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InterCompanyTransfer)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ToCompany)
                    .WithMany(p => p.InterCompanyTransferToCompany)
                    .HasForeignKey(d => d.ToCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InterCompanyTransfer_ToCompany");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoice", "finance");

                entity.HasIndex(e => e.ContractId)
                    .HasName("IX_Invoice_Contract");

                entity.HasIndex(e => new { e.InvoiceStatus, e.InterCompanyTransferStatus, e.IsInterCompanyTransferRequired, e.ProjectId })
                    .HasName("IX_INVOICE_INVOICEID");

                entity.Property(e => e.CommittedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Ictmsreference)
                    .HasColumnName("ICTMSReference")
                    .HasMaxLength(50);

                entity.Property(e => e.InterCompanyTransferStatus).HasMaxLength(1);

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNumber).HasMaxLength(20);

                entity.Property(e => e.InvoiceStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasDefaultValueSql("(N'D')");

                entity.Property(e => e.InvoiceTotal).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.Lang).HasMaxLength(50);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Net).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.OriginalInvoiceNumber).HasMaxLength(20);

                entity.Property(e => e.SalesTaxCode).HasMaxLength(10);

                entity.Property(e => e.SalesTaxName).HasMaxLength(50);

                entity.Property(e => e.SalesTaxTotal).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.SalesTaxValue).HasColumnType("decimal(9, 4)");

                entity.Property(e => e.WithholdingTax).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Batch)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.BatchId);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.InvoiceContract)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CreditNoteReason)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.CreditNoteReasonId);

                entity.HasOne(d => d.InvoicingCompany)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.InvoicingCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.OriginalInvoice)
                    .WithMany(p => p.InverseOriginalInvoice)
                    .HasForeignKey(d => d.OriginalInvoiceId);

                entity.HasOne(d => d.ParentContract)
                    .WithMany(p => p.InvoiceParentContract)
                    .HasForeignKey(d => d.ParentContractId);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.InvoiceSalesTax)
                    .HasForeignKey(d => d.SalesTaxId);

                entity.HasOne(d => d.WithholdingTaxNavigation)
                    .WithMany(p => p.InvoiceWithholdingTaxNavigation)
                    .HasForeignKey(d => d.WithholdingTaxId);
            });

            modelBuilder.Entity<InvoiceAssignmentReferenceType>(entity =>
            {
                entity.ToTable("InvoiceAssignmentReferenceType", "finance");

                entity.HasIndex(e => e.InvoiceId);

                entity.HasIndex(e => new { e.InvoiceId, e.InvoiceReferenceTypeId })
                    .HasName("IX_Invoice_InvoiceReference")
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceAssignmentReferenceType)
                    .HasForeignKey(d => d.InvoiceId);

                entity.HasOne(d => d.InvoiceReferenceType)
                    .WithMany(p => p.InvoiceAssignmentReferenceType)
                    .HasForeignKey(d => d.InvoiceReferenceTypeId)
                    .HasConstraintName("FK_InvoiceAssignmentReferenceType_InvoiceAssignmentReferenceType");
            });

            modelBuilder.Entity<InvoiceBatch>(entity =>
            {
                entity.ToTable("InvoiceBatch", "finance");

                entity.Property(e => e.BatchReference)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.BatchStatus)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.InvoiceBatch)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceBatch_Company");
            });

            modelBuilder.Entity<InvoiceExchangeRate>(entity =>
            {
                entity.ToTable("InvoiceExchangeRate", "finance");

                entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.FromCurrency)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ToCurrency)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceExchangeRate)
                    .HasForeignKey(d => d.InvoiceId);
            });

            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.ToTable("InvoiceItem", "finance");

                entity.HasIndex(e => e.InvoiceId)
                    .HasName("IX_INVOICEITEM_INVOICEID");

                entity.Property(e => e.ItemTotal).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Net).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.ReferenceValue).HasMaxLength(150);

                entity.Property(e => e.SalesTaxCode).HasMaxLength(10);

                entity.Property(e => e.SalesTaxName).HasMaxLength(50);

                entity.Property(e => e.SalesTaxTotal).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.SalesTaxValue).HasColumnType("decimal(9, 4)");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.InvoiceItem)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceItem_Assignment");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceItem)
                    .HasForeignKey(d => d.InvoiceId);

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.InvoiceItem)
                    .HasForeignKey(d => d.SalesTaxId);
            });

            modelBuilder.Entity<InvoiceItemBackup>(entity =>
            {
                entity.ToTable("InvoiceItemBackup", "finance");

                entity.Property(e => e.ConvertedItemTotal).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.ConvertedNet).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.ConvertedSalesTax).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.ItemTotal).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.ItemType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Net).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SalesTaxCode).HasMaxLength(10);

                entity.Property(e => e.SalesTaxName).HasMaxLength(50);

                entity.Property(e => e.SalesTaxTotal).HasColumnType("decimal(20, 4)");

                entity.Property(e => e.SalesTaxValue).HasColumnType("decimal(9, 4)");

                entity.Property(e => e.Units).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.InvoiceItem)
                    .WithMany(p => p.InvoiceItemBackup)
                    .HasForeignKey(d => d.InvoiceItemId)
                    .HasConstraintName("FK_InvoiceItemBackup_InvoiceItem");

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.InvoiceItemBackup)
                    .HasForeignKey(d => d.SalesTaxId)
                    .HasConstraintName("FK_InvoiceItemBackup_CompanyTaxSalesTax");
            });

            modelBuilder.Entity<InvoiceMessage>(entity =>
            {
                entity.ToTable("InvoiceMessage", "finance");

                entity.HasIndex(e => new { e.InvoiceId, e.MessageTypeId })
                    .HasName("IX_InvoiceMessage_MessageTypeId");

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceMessage)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.InvoiceMessage)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<InvoiceMessageType>(entity =>
            {
                entity.ToTable("InvoiceMessageType", "finance");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<InvoiceNumberRange>(entity =>
            {
                entity.ToTable("InvoiceNumberRange", "master");

                entity.Property(e => e.Language).HasMaxLength(50);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.Notes).HasMaxLength(500);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.InvoiceNumberRange)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceNumberRange_Company");
            });

            modelBuilder.Entity<InvoiceNumberRangeDetail>(entity =>
            {
                entity.ToTable("InvoiceNumberRangeDetail", "master");

                entity.Property(e => e.Language).HasMaxLength(50);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.RangeType).HasMaxLength(1);

                entity.HasOne(d => d.InvoiceNumberRange)
                    .WithMany(p => p.InvoiceNumberRangeDetail)
                    .HasForeignKey(d => d.InvoiceNumberRangeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceNumberRangeDetail_InvoiceNumberRange");
            });

            modelBuilder.Entity<LanguageInvoicePaymentTerm>(entity =>
            {
                entity.ToTable("LanguageInvoicePaymentTerm", "master");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.InvoicePaymentTerm)
                    .WithMany(p => p.LanguageInvoicePaymentTermInvoicePaymentTerm)
                    .HasForeignKey(d => d.InvoicePaymentTermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageInvoicePaymentTerm_InvoicePaymentTermId");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.LanguageInvoicePaymentTermLanguage)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageInvoicePaymentTerm_LanguageId");
            });

            modelBuilder.Entity<LanguageReferenceType>(entity =>
            {
                entity.ToTable("LanguageReferenceType", "master");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.LanguageReferenceTypeLanguage)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageReference_LanguageId");

                entity.HasOne(d => d.ReferenceType)
                    .WithMany(p => p.LanguageReferenceTypeReferenceType)
                    .HasForeignKey(d => d.ReferenceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageReference_ReferencetypeId");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Log", "admin");

                entity.Property(e => e.Application)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Logged).HasColumnType("datetime");

                entity.Property(e => e.Logger).HasMaxLength(250);

                entity.Property(e => e.Message).IsRequired();
            });

            modelBuilder.Entity<LogData>(entity =>
            {
                entity.ToTable("LogData", "admin");

                entity.Property(e => e.LogReason)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LoggedOn)
                    .HasColumnName("loggedOn")
                    .HasColumnType("date");

                entity.Property(e => e.Module)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Object).IsUnicode(false);

                entity.Property(e => e.ObjectType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.ToTable("Module", "security");

                entity.HasIndex(e => new { e.ApplicationId, e.Name })
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.Module)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ModuleActivity>(entity =>
            {
                entity.ToTable("ModuleActivity", "security");

                entity.HasIndex(e => new { e.ActivityId, e.MouduleId })
                    .HasName("IX_Module_Activity_ActivityId_ModuleId")
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ModuleActivity)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuleActivity_ActivityId");

                entity.HasOne(d => d.Moudule)
                    .WithMany(p => p.ModuleActivity)
                    .HasForeignKey(d => d.MouduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuleActivity_Module_ModuleId");
            });

            modelBuilder.Entity<ModuleDocumentType>(entity =>
            {
                entity.ToTable("ModuleDocumentType", "master");

                entity.HasIndex(e => new { e.ModuleId, e.DocumentTypeId });

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Tsvisible).HasColumnName("TSVisible");

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.ModuleDocumentTypeDocumentType)
                    .HasForeignKey(d => d.DocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuleDocumentType_Data_DocumentId");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.ModuleDocumentTypeModule)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<NumberSequence>(entity =>
            {
                entity.ToTable("NumberSequence", "common");

                entity.HasIndex(e => e.ModuleData);

                entity.HasIndex(e => e.ModuleId);

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.NumberSequenceModule)
                    .HasForeignKey(d => d.ModuleId)
                    .HasConstraintName("FK__NumberSeq__Modul__61BB7BD9");

                //entity.HasOne(d => d.ModuleRef)
                //    .WithMany(p => p.NumberSequenceModuleRef)
                //    .HasForeignKey(d => d.ModuleRefId)
                //    .HasConstraintName("FK__NumberSeq__Modul__62AFA012");
            });

            modelBuilder.Entity<OverrideResource>(entity =>
            {
                entity.ToTable("OverrideResource", "search");

                entity.HasIndex(e => e.ResourceSearchId);

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.ResourceSearch)
                    .WithMany(p => p.OverrideResource)
                    .HasForeignKey(d => d.ResourceSearchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OverrideResource_ResourceSearch");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.OverrideResource)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OverrideResource_TechnicalSpecialist_Id");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project", "project");

                entity.HasIndex(e => e.CoordinatorId);

                entity.HasIndex(e => e.CustomerContactId);

                entity.HasIndex(e => e.CustomerProjectContactId);

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.ManagedServicesCoordinatorId);

                entity.HasIndex(e => e.ProjectNumber);

                entity.HasIndex(e => new { e.Status, e.CustomerProjectName });

                entity.HasIndex(e => new { e.Status, e.CustomerProjectNumber });

                entity.HasIndex(e => new { e.Id, e.ProjectNumber, e.CoordinatorId, e.ContractId })
                    .HasName("IX_Project_ContractId");

                entity.HasIndex(e => new { e.ContractId, e.CompanyDivisionId, e.CompanyanyDivCostCentreId, e.CompanyOfficeId, e.InvoiceSalesTaxId })
                    .HasName("IX_Project_InvoiceSalesTaxId");

                entity.Property(e => e.Budget).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.BudgetHours).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.BudgetHoursWarning).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerDirectReportingEmailAddress).HasMaxLength(1000);

                entity.Property(e => e.CustomerProjectName)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.CustomerProjectNumber)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IndustrySector).HasMaxLength(50);

                entity.Property(e => e.InvoiceCurrency)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.InvoiceGrouping)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IsEreportProjectMapped).HasColumnName("IsEReportProjectMapped");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.WorkFlowType)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.CompanyDivision)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.CompanyDivisionId);

                entity.HasOne(d => d.CompanyOffice)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.CompanyOfficeId);

                entity.HasOne(d => d.CompanyanyDivCostCentre)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.CompanyanyDivCostCentreId)
                    .HasConstraintName("FK_Project_CompanyDivisionCostCenter_CompanyCostCenterId");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_Project_ContractId");

                entity.HasOne(d => d.Coordinator)
                    .WithMany(p => p.ProjectCoordinator)
                    .HasForeignKey(d => d.CoordinatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CustomerContact)
                    .WithMany(p => p.ProjectCustomerContact)
                    .HasForeignKey(d => d.CustomerContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CustomerInvoiceAddress)
                    .WithMany(p => p.ProjectCustomerInvoiceAddress)
                    .HasForeignKey(d => d.CustomerInvoiceAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CustomerProjectAddress)
                    .WithMany(p => p.ProjectCustomerProjectAddress)
                    .HasForeignKey(d => d.CustomerProjectAddressId);

                entity.HasOne(d => d.CustomerProjectContact)
                    .WithMany(p => p.ProjectCustomerProjectContact)
                    .HasForeignKey(d => d.CustomerProjectContactId);

                entity.HasOne(d => d.InvoiceFooterText)
                    .WithMany(p => p.ProjectInvoiceFooterText)
                    .HasForeignKey(d => d.InvoiceFooterTextId);

                entity.HasOne(d => d.InvoicePaymentTerms)
                    .WithMany(p => p.ProjectInvoicePaymentTerms)
                    .HasForeignKey(d => d.InvoicePaymentTermsId);

                entity.HasOne(d => d.InvoiceRemittanceText)
                    .WithMany(p => p.ProjectInvoiceRemittanceText)
                    .HasForeignKey(d => d.InvoiceRemittanceTextId);

                entity.HasOne(d => d.InvoiceSalesTax)
                    .WithMany(p => p.ProjectInvoiceSalesTax)
                    .HasForeignKey(d => d.InvoiceSalesTaxId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_MasterTax_SalesTax");

                entity.HasOne(d => d.InvoiceWithholdingTax)
                    .WithMany(p => p.ProjectInvoiceWithholdingTax)
                    .HasForeignKey(d => d.InvoiceWithholdingTaxId)
                    .HasConstraintName("FK_Project_MasterTax_WithholdingTax");

                entity.HasOne(d => d.Logo)
                    .WithMany(p => p.ProjectLogo)
                    .HasForeignKey(d => d.LogoId)
                    .HasConstraintName("FK_Project_Logo_Master_Data");

                entity.HasOne(d => d.ManagedServicesCoordinator)
                    .WithMany(p => p.ProjectManagedServicesCoordinator)
                    .HasForeignKey(d => d.ManagedServicesCoordinatorId)
                    .HasConstraintName("FK_Project_User_ManagedServiceCoordId");

                entity.HasOne(d => d.ManagedServicesTypeNavigation)
                    .WithMany(p => p.ProjectManagedServicesTypeNavigation)
                    .HasForeignKey(d => d.ManagedServicesType)
                    .HasConstraintName("FK_Project_ManagedServicesType");

                entity.HasOne(d => d.ProjectType)
                    .WithMany(p => p.ProjectProjectType)
                    .HasForeignKey(d => d.ProjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ProjectClientNotification>(entity =>
            {
                entity.ToTable("ProjectClientNotification", "project");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.ProjectId);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.SendNcrreportingNotification).HasColumnName("SendNCRReportingNotification");

                entity.HasOne(d => d.CustomerContact)
                    .WithMany(p => p.ProjectClientNotification)
                    .HasForeignKey(d => d.CustomerContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectClientNotification)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ProjectInvoiceAssignmentReference>(entity =>
            {
                entity.ToTable("ProjectInvoiceAssignmentReference", "project");

                entity.HasIndex(e => e.AssignmentReferenceTypeId);

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.ProjectId)
                    .HasName("IX_ProjectInvoiceAssignmentRefence_ProjectId");

                entity.HasIndex(e => new { e.ProjectId, e.AssignmentReferenceTypeId })
                    .HasName("IX_ProjectAssignmentReference_ProjectReference")
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.AssignmentReferenceType)
                    .WithMany(p => p.ProjectInvoiceAssignmentReference)
                    .HasForeignKey(d => d.AssignmentReferenceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectInvoiceAssignmentRefence_Data_AssignmentReferenceTypeId");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectInvoiceAssignmentReference)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectInvoiceAssignmentRefence_Project_ProjectId");
            });

            modelBuilder.Entity<ProjectInvoiceAttachment>(entity =>
            {
                entity.ToTable("ProjectInvoiceAttachment", "project");

                entity.HasIndex(e => e.DocumentTypeId);

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.ProjectId);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.ProjectInvoiceAttachment)
                    .HasForeignKey(d => d.DocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectInvoiceAttachment)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ProjectMessage>(entity =>
            {
                entity.ToTable("ProjectMessage", "project");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => new { e.ProjectId, e.MessageTypeId });

                entity.Property(e => e.Identifier).HasMaxLength(100);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDefaultMessage).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.ProjectMessage)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectMessage)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ProjectMessageType>(entity =>
            {
                entity.ToTable("ProjectMessageType", "project");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ProjectNote>(entity =>
            {
                entity.ToTable("ProjectNote", "project");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.ProjectId);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(4000);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectNote)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshToken", "auth");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccessToken)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Application)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RequestedIp)
                    .IsRequired()
                    .HasColumnName("RequestedIP")
                    .HasMaxLength(100);

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<ResourceSearch>(entity =>
            {
                entity.ToTable("ResourceSearch", "search");

                entity.HasIndex(e => e.CategoryId);

                entity.HasIndex(e => e.CompanyId);

                entity.HasIndex(e => e.CustomerId);

                entity.HasIndex(e => e.ServiceId);

                entity.HasIndex(e => e.SubCategoryId);

                entity.Property(e => e.ActionStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AssignedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AssignedTo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AssignedToOm)
                    .HasColumnName("AssignedToOM")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DispositionType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SearchType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SerilizationType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ResourceSearch)
                    .HasForeignKey(d => d.CategoryId);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.ResourceSearch)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ResourceSearch)
                    .HasForeignKey(d => d.CustomerId);

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ResourceSearch)
                    .HasForeignKey(d => d.ServiceId);

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.ResourceSearch)
                    .HasForeignKey(d => d.SubCategoryId);
            });

            modelBuilder.Entity<ResourceSearchNote>(entity =>
            {
                entity.ToTable("ResourceSearchNote", "search");

                entity.HasIndex(e => e.ResourceSearchId);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(4000);

                entity.HasOne(d => d.ResourceSearch)
                    .WithMany(p => p.ResourceSearchNote)
                    .HasForeignKey(d => d.ResourceSearchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResourceSearchNote_ResourceSearch");
            });

            modelBuilder.Entity<RevenueAccrual>(entity =>
            {
                entity.ToTable("RevenueAccrual", "finance");

                entity.Property(e => e.AdditionalCompany1).HasMaxLength(60);

                entity.Property(e => e.AdditionalCompany2).HasMaxLength(60);

                entity.Property(e => e.Businessunit).HasMaxLength(50);

                entity.Property(e => e.ChargeReference).HasMaxLength(50);

                entity.Property(e => e.ContractHoldingCompany).HasMaxLength(60);

                entity.Property(e => e.ContractHoldingCoordinator).HasMaxLength(50);

                entity.Property(e => e.CostCentre).HasMaxLength(100);

                entity.Property(e => e.Currency).HasMaxLength(3);

                entity.Property(e => e.CurrentInvoicingStatus).HasMaxLength(1);

                entity.Property(e => e.Customer).HasMaxLength(60);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Division).HasMaxLength(30);

                entity.Property(e => e.DivisionRef).HasMaxLength(15);

                entity.Property(e => e.ExchangeRate).HasColumnType("decimal(16, 6)");

                entity.Property(e => e.HostCompany).HasMaxLength(60);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.MiassignmentNo).HasColumnName("MIAssignmentNo");

                entity.Property(e => e.MicontractNumber)
                    .HasColumnName("MIContractNumber")
                    .HasMaxLength(12);

                entity.Property(e => e.MiprojectNumber).HasColumnName("MIProjectNumber");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.NativeCurrency).HasMaxLength(3);

                entity.Property(e => e.NetExpenseTotal).HasColumnType("decimal(38, 6)");

                entity.Property(e => e.NetFeeTotal).HasColumnType("decimal(38, 6)");

                entity.Property(e => e.Office).HasMaxLength(30);

                entity.Property(e => e.OperatingCompany).HasMaxLength(60);

                entity.Property(e => e.OperatingCoordinator).HasMaxLength(50);

                entity.Property(e => e.ParentCompany).HasMaxLength(60);

                entity.Property(e => e.Ptype)
                    .HasColumnName("PType")
                    .HasMaxLength(1);

                entity.Property(e => e.Rate).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Reportnumber).HasMaxLength(65);

                entity.Property(e => e.Suppliernumber).HasMaxLength(150);

                entity.Property(e => e.Type).HasMaxLength(15);

                entity.Property(e => e.Units).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.VisitDatePeriod).HasMaxLength(30);

                entity.Property(e => e.Visitstatus).HasMaxLength(28);

                entity.HasOne(d => d.Token)
                    .WithMany(p => p.RevenueAccrual)
                    .HasForeignKey(d => d.TokenId)
                    .HasConstraintName("FK_RevenueAccrual_RevenueAccrual_TokenId");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role", "security");

                entity.HasIndex(e => new { e.ApplicationId, e.Name })
                    .HasName("IX_Role_Name_ApplicationId")
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.Role)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_ApplicationId");
            });

            modelBuilder.Entity<RoleActivity>(entity =>
            {
                entity.ToTable("RoleActivity", "security");

                entity.HasIndex(e => e.ModuleId);

                entity.HasIndex(e => new { e.RoleId, e.ActivityId, e.ModuleId })
                    .HasName("IX_RoleActivity_RoleId_ActivityId")
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.RoleActivity)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleActivity_RoleActivity_ActivityId");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.RoleActivity)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleActivity)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleActivity_RoleId");
            });

            modelBuilder.Entity<SqlauditLogDetail>(entity =>
            {
                entity.ToTable("SQLAuditLogDetail", "audit");

                entity.HasIndex(e => e.SqlAuditLogId)
                    .HasName("IX_audit_SqlAuditLogId");

                entity.HasOne(d => d.SqlAuditLog)
                    .WithMany(p => p.SqlauditLogDetail)
                    .HasForeignKey(d => d.SqlAuditLogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SQLAuditLogDetail_SQLAuditLog_SQLAuditLogId");

                entity.HasOne(d => d.SqlAuditSubModule)
                    .WithMany(p => p.SqlauditLogDetail)
                    .HasForeignKey(d => d.SqlAuditSubModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SQLAuditLogDetail_SQLAuditModule_SubModuleId");
            });

            modelBuilder.Entity<SqlauditLogEvent>(entity =>
            {
                entity.ToTable("SQLAuditLogEvent", "audit");

                entity.HasIndex(e => e.ActionOn)
                    .HasName("IX_audit_ActionOn");

                entity.HasIndex(e => e.SearchReference)
                    .HasName("IX_audit_SearchReference");

                entity.HasIndex(e => new { e.ActionOn, e.SearchReference })
                    .HasName("IX_audit_ActionOn_SearchReference");

                entity.Property(e => e.ActionBy)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ActionOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ActionType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.SearchReference)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.SqlAuditModule)
                    .WithMany(p => p.SqlauditLogEvent)
                    .HasForeignKey(d => d.SqlAuditModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SQLAuditLog_SQLAuditModule_SqlAuditModuleId");
            });

            modelBuilder.Entity<SqlauditModule>(entity =>
            {
                entity.ToTable("SQLAuditModule", "audit");

                entity.HasIndex(e => e.ModuleName)
                    .HasName("IX_audit_ModuleName");

                entity.Property(e => e.ModuleDescription).HasMaxLength(500);

                entity.Property(e => e.ModuleName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Supplier", "supplier");

                entity.HasIndex(e => e.Address);

                entity.HasIndex(e => e.CityId)
                    .HasName("IX_Supplier_City");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.SupplierName);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PostalCode).HasMaxLength(15);

                entity.Property(e => e.SupplierName)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Supplier)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Supplier_City");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Supplier)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_Supplier_CountryId");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.Supplier)
                    .HasForeignKey(d => d.CountyId)
                    .HasConstraintName("FK_Supplier_CountyId");
            });

            modelBuilder.Entity<SupplierContact>(entity =>
            {
                entity.ToTable("SupplierContact", "supplier");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.SupplierId)
                    .HasName("IX_Supplier_SupplierId");

                entity.Property(e => e.EmailId).HasMaxLength(60);

                entity.Property(e => e.FaxNumber).HasMaxLength(60);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.MobileNumber).HasMaxLength(60);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.OtherContactDetails).HasMaxLength(150);

                entity.Property(e => e.SupplierContactName)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.TelephoneNumber).HasMaxLength(60);

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SupplierContact)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupplierAddress_SupplierId");
            });

            modelBuilder.Entity<SupplierNote>(entity =>
            {
                entity.ToTable("SupplierNote", "supplier");

                entity.HasIndex(e => e.SupplierId)
                    .HasName("IX_Supplier_SupplierId");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(4000);

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SupplierNote)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupplierNote_SupplierId");
            });

            modelBuilder.Entity<SupplierPurchaseOrder>(entity =>
            {
                entity.ToTable("SupplierPurchaseOrder", "supplier");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.Status);

                entity.HasIndex(e => e.SupplierId)
                    .HasName("IX_SupplierPurchaseOrder_MainSupplierId");

                entity.HasIndex(e => new { e.Id, e.Status })
                    .HasName("IX_SupplierPO_Status");

                entity.Property(e => e.BudgetHoursUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.BudgetHoursUnitWarning).HasDefaultValueSql("((0))");

                entity.Property(e => e.BudgetValue).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CompletionDate).HasColumnType("datetime");

                entity.Property(e => e.DeliveryDate).HasColumnType("datetime");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.MaterialDescription)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.SupplierPonumber)
                    .IsRequired()
                    .HasColumnName("SupplierPONumber")
                    .HasMaxLength(150);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.SupplierPurchaseOrder)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupplierPurchaseOrder_ProjectId");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SupplierPurchaseOrder)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupplierPurchaseOrder_SupplierId");
            });

            modelBuilder.Entity<SupplierPurchaseOrderNote>(entity =>
            {
                entity.ToTable("SupplierPurchaseOrderNote", "supplier");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.SupplierPurchaseOrderId);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.SupplierPurchaseOrder)
                    .WithMany(p => p.SupplierPurchaseOrderNote)
                    .HasForeignKey(d => d.SupplierPurchaseOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupplierPurchaseOrderNote_SupplierPurchaseOrderId");
            });

            modelBuilder.Entity<SupplierPurchaseOrderSubSupplier>(entity =>
            {
                entity.ToTable("SupplierPurchaseOrderSubSupplier", "supplier");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.SupplierId)
                    .HasName("IX_SubSupplierPurchaseOrder_SubSupplierId");

                entity.HasIndex(e => e.SupplierPurchaseOrderId)
                    .HasName("IX_SubSupplierPurchaseOrder_SupplierPurchaseOrderId");

                entity.HasIndex(e => new { e.Id, e.SupplierId })
                    .HasName("IX_SupplierPurchaseOrderSubSupplier");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SupplierPurchaseOrderSubSupplier)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubSupplierPurchaseOrder_Supplier");

                entity.HasOne(d => d.SupplierPurchaseOrder)
                    .WithMany(p => p.SupplierPurchaseOrderSubSupplier)
                    .HasForeignKey(d => d.SupplierPurchaseOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubSupplierPurchaseOrder_SupplierPurchaseOrderId");
            });

            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.ToTable("SystemSetting", "common");

                entity.Property(e => e.KeyName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.KeyValue).IsRequired();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task", "common");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Moduletype)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaskRefCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TaskType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.AssignedBy)
                    .WithMany(p => p.TaskAssignedBy)
                    .HasForeignKey(d => d.AssignedById)
                    .HasConstraintName("FK_commonTask_Data_AssignedById");

                entity.HasOne(d => d.AssignedTo)
                    .WithMany(p => p.TaskAssignedTo)
                    .HasForeignKey(d => d.AssignedToId)
                    .HasConstraintName("FK_commonTask_Data_AssignedToId");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.CompanyId);
            });

            modelBuilder.Entity<TaxonomyBusinessUnit>(entity =>
            {
                entity.ToTable("TaxonomyBusinessUnit", "master");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TaxonomyBusinessUnitCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaxonomyBusinessUnit_CategoryId");

                entity.HasOne(d => d.ProjectType)
                    .WithMany(p => p.TaxonomyBusinessUnitProjectType)
                    .HasForeignKey(d => d.ProjectTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaxonomyBusinessUnit_ProjectTypeId");
            });

            modelBuilder.Entity<TaxonomyService>(entity =>
            {
                entity.ToTable("TaxonomyService", "master");

                entity.HasIndex(e => e.TaxonomySubCategoryId);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.TaxonomyServiceName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.TaxonomySubCategory)
                    .WithMany(p => p.TaxonomyService)
                    .HasForeignKey(d => d.TaxonomySubCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaxonomyService_TaxonomySubCategory");
            });

            modelBuilder.Entity<TaxonomySubCategory>(entity =>
            {
                entity.ToTable("TaxonomySubCategory", "master");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.TaxonomySubCategoryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.TaxonomyCategory)
                    .WithMany(p => p.TaxonomySubCategory)
                    .HasForeignKey(d => d.TaxonomyCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaxonomySubCategory_Data_CategoryId");
            });

            modelBuilder.Entity<TechnicalSpecialist>(entity =>
            {
                entity.ToTable("TechnicalSpecialist", "techspecialist");

                entity.HasIndex(e => e.EmploymentTypeId);

                entity.HasIndex(e => e.FirstName);

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.LastName);

                entity.HasIndex(e => e.LogInName);

                entity.HasIndex(e => e.Pin)
                    .HasName("UQ__Technica__C570590334A23F6D")
                    .IsUnique();

                entity.HasIndex(e => e.ProfileStatusId);

                entity.HasIndex(e => new { e.FirstName, e.LastName });

                entity.HasIndex(e => new { e.Id, e.Salutation, e.FirstName, e.MiddleName, e.LastName, e.DateOfBirth, e.DrivingLicenseNumber, e.PassportNumber, e.ModeOfCommunication, e.SubDivisionId, e.StartDate, e.EndDate, e.ProfileStatusId, e.EmploymentTypeId, e.IsReviewAndModerationProcess, e.TaxReference, e.CompanyPayrollId, e.PayrollReference, e.PayrollNote, e.ProfessionalAfiliation, e.ProfessionalSummary, e.BusinessInformationComment, e.ProfileActionId, e.IsActive, e.ModifiedBy, e.LastModification, e.UpdateCount, e.DrivingLicenseExpiryDate, e.PassportExpiryDate, e.PassportCountryOriginId, e.IsEreportingQualified, e.CreatedBy, e.LogInName, e.HomePageComment, e.CompanyId })
                    .HasName("IX_TechnicalSpecialist_CompanyID");

                entity.Property(e => e.ApprovalStatus).HasMaxLength(1);

                entity.Property(e => e.AssignedByUser).HasMaxLength(50);

                entity.Property(e => e.AssignedToUser).HasMaxLength(50);

                entity.Property(e => e.BusinessInformationComment).HasMaxLength(4000);

                entity.Property(e => e.ContactComment).HasMaxLength(1200);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.DrivingLicenseExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.DrivingLicenseNumber).HasMaxLength(100);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName).HasMaxLength(30);

                entity.Property(e => e.HomePageComment).HasMaxLength(2000);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsEreportingQualified).HasColumnName("IsEReportingQualified");

                entity.Property(e => e.IsReviewAndModerationProcess).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsTsCredSent).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.LastName).HasMaxLength(30);

                entity.Property(e => e.LogInName).HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(100);

                entity.Property(e => e.ModeOfCommunication).HasMaxLength(20);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PassportExpiryDate)
                    .HasColumnName("passportExpiryDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.PassportNumber).HasMaxLength(100);

                entity.Property(e => e.PayrollNote).HasMaxLength(2000);

                entity.Property(e => e.PayrollReference).HasMaxLength(15);

                entity.Property(e => e.Pin).HasComputedColumnSql("([Id])");

                entity.Property(e => e.ProfessionalAfiliation).HasMaxLength(100);

                entity.Property(e => e.ProfessionalSummary).HasMaxLength(4000);

                entity.Property(e => e.Salutation).HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TaxReference).HasMaxLength(30);

                entity.Property(e => e.Tqmcomment)
                    .HasColumnName("TQMComment")
                    .HasMaxLength(4000);

                entity.Property(e => e.Userid).HasColumnName("USERID");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TechnicalSpecialist)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CompanyPayroll)
                    .WithMany(p => p.TechnicalSpecialist)
                    .HasForeignKey(d => d.CompanyPayrollId)
                    .HasConstraintName("FK_TechnicalSpecialist_Data_CompanyPayrollId");

                entity.HasOne(d => d.EmploymentType)
                    .WithMany(p => p.TechnicalSpecialistEmploymentType)
                    .HasForeignKey(d => d.EmploymentTypeId);

                entity.HasOne(d => d.PassportCountryOrigin)
                    .WithMany(p => p.TechnicalSpecialist)
                    .HasForeignKey(d => d.PassportCountryOriginId)
                    .HasConstraintName("FK_TechnicalSpecialist_Data_PassportCountryOriginId");

                entity.HasOne(d => d.PendingWith)
                    .WithMany(p => p.TechnicalSpecialistPendingWith)
                    .HasForeignKey(d => d.PendingWithId);

                entity.HasOne(d => d.ProfileAction)
                    .WithMany(p => p.TechnicalSpecialistProfileAction)
                    .HasForeignKey(d => d.ProfileActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ProfileStatus)
                    .WithMany(p => p.TechnicalSpecialistProfileStatus)
                    .HasForeignKey(d => d.ProfileStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SubDivision)
                    .WithMany(p => p.TechnicalSpecialistSubDivision)
                    .HasForeignKey(d => d.SubDivisionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TechnicalSpecialist_Data_SubdivisionId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TechnicalSpecialistUser)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK_TechnicalSpecialist_User_UserId");
            });

            modelBuilder.Entity<TechnicalSpecialistCalendar>(entity =>
            {
                entity.ToTable("TechnicalSpecialistCalendar", "techspecialist");

                entity.HasIndex(e => e.CalendarRefCode);

                entity.HasIndex(e => e.CalendarStatus);

                entity.HasIndex(e => e.CalendarType);

                entity.HasIndex(e => e.CompanyId);

                entity.HasIndex(e => e.EndDateTime);

                entity.HasIndex(e => e.IsActive);

                entity.HasIndex(e => e.StartDateTime);

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.Property(e => e.CalendarStatus).HasMaxLength(50);

                entity.Property(e => e.CalendarType)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistCalendar)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistCertificationAndTraining>(entity =>
            {
                entity.ToTable("TechnicalSpecialistCertificationAndTraining", "techspecialist");

                entity.HasIndex(e => e.CertificationAndTrainingId);

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.Property(e => e.CertificationAndTrainingRefId).HasMaxLength(255);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Duration).HasMaxLength(300);

                entity.Property(e => e.EffeciveDate).HasColumnType("datetime");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.IsIlearn)
                    .HasColumnName("isILearn")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.RecordType)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.VerificationDate).HasColumnType("datetime");

                entity.Property(e => e.VerificationStatus).HasMaxLength(25);

                entity.Property(e => e.VerificationType).HasMaxLength(50);

                entity.HasOne(d => d.CertificationAndTraining)
                    .WithMany(p => p.TechnicalSpecialistCertificationAndTraining)
                    .HasForeignKey(d => d.CertificationAndTrainingId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistCertificationAndTraining)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TechnicalSpecialistCertificationAndTraining_TechnicalSpecialist_TSId");

                entity.HasOne(d => d.VerifiedBy)
                    .WithMany(p => p.TechnicalSpecialistCertificationAndTraining)
                    .HasForeignKey(d => d.VerifiedById)
                    .HasConstraintName("FK_TechnicalSpecialistCertificationAndTraining_User");
            });

            modelBuilder.Entity<TechnicalSpecialistCodeAndStandard>(entity =>
            {
                entity.ToTable("TechnicalSpecialistCodeAndStandard", "techspecialist");

                entity.HasIndex(e => e.CodeStandardId)
                    .HasName("IX_ TechnicalSpecialistCodeAndStandard_CodeStandardId");

                entity.HasIndex(e => e.TechnicalSpecialistId)
                    .HasName("IX_ TechnicalSpecialistCodeAndStandard_TechnicalSpecialistId");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.CodeStandard)
                    .WithMany(p => p.TechnicalSpecialistCodeAndStandard)
                    .HasForeignKey(d => d.CodeStandardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TechnicalSpecialistCodeAndStandard_TechnicalSpecialist_CodeStandardId");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistCodeAndStandard)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistCommodityEquipmentKnowledge>(entity =>
            {
                entity.ToTable("TechnicalSpecialistCommodityEquipmentKnowledge", "techspecialist");

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.HasIndex(e => new { e.EquipmentKnowledgeId, e.CommodityId, e.TechnicalSpecialistId })
                    .HasName("IX_TechnicalSpecialistCommodityEquipmentKnowledge_EKId_CID_TSId")
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Commodity)
                    .WithMany(p => p.TechnicalSpecialistCommodityEquipmentKnowledgeCommodity)
                    .HasForeignKey(d => d.CommodityId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EquipmentKnowledge)
                    .WithMany(p => p.TechnicalSpecialistCommodityEquipmentKnowledgeEquipmentKnowledge)
                    .HasForeignKey(d => d.EquipmentKnowledgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TechnicalSpecialistCommodityEquipmentKnowledge_Data_EquipmentId");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistCommodityEquipmentKnowledge)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistComputerElectronicKnowledge>(entity =>
            {
                entity.ToTable("TechnicalSpecialistComputerElectronicKnowledge", "techspecialist");

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.HasIndex(e => new { e.ComputerKnowledgeId, e.TechnicalSpecialistId })
                    .HasName("IX_TechnicalSpecialistComputerElectronicKnowledge_CKId_TSId");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.ComputerKnowledge)
                    .WithMany(p => p.TechnicalSpecialistComputerElectronicKnowledge)
                    .HasForeignKey(d => d.ComputerKnowledgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistComputerElectronicKnowledge)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistContact>(entity =>
            {
                entity.ToTable("TechnicalSpecialistContact", "techspecialist");

                entity.HasIndex(e => e.Address);

                entity.HasIndex(e => e.CityId);

                entity.HasIndex(e => e.ContactType);

                entity.HasIndex(e => e.CountryId);

                entity.HasIndex(e => e.CountyId);

                entity.HasIndex(e => e.IsGeoCordinateSync);

                entity.HasIndex(e => e.PostalCode);

                entity.HasIndex(e => e.TechnicalSpecialistId)
                    .HasName("IX_TechnicalSpecialistContact_TSId");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.ContactType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.EmailAddress).HasMaxLength(500);

                entity.Property(e => e.EmergencyContactName).HasMaxLength(250);

                entity.Property(e => e.FaxNumber).HasMaxLength(60);

                entity.Property(e => e.IsGeoCordinateSync).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.MobileNumber).HasMaxLength(60);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PostalCode).HasMaxLength(15);

                entity.Property(e => e.TelephoneNumber).HasMaxLength(100);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TechnicalSpecialistContact)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_TechnicalSpecialistContact_Data_CityId");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TechnicalSpecialistContact)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_TechnicalSpecialistContact_Data_CountryId");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.TechnicalSpecialistContact)
                    .HasForeignKey(d => d.CountyId)
                    .HasConstraintName("FK_TechnicalSpecialistContact_Data_CountyId");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistContact)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistCustomerApproval>(entity =>
            {
                entity.ToTable("TechnicalSpecialistCustomerApproval", "techspecialist");

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.Property(e => e.Comments).HasMaxLength(4000);

                entity.Property(e => e.CustomerSapId).HasMaxLength(50);

                entity.Property(e => e.DateFrom).HasColumnType("datetime");

                entity.Property(e => e.DateTo).HasColumnType("datetime");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.CustomerCommodityCodes)
                    .WithMany(p => p.TechnicalSpecialistCustomerApproval)
                    .HasForeignKey(d => d.CustomerCommodityCodesId);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TechnicalSpecialistCustomerApproval)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_TechnicalSpecialistCustomerApproval_master_CustomerId");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistCustomerApproval)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistCustomers>(entity =>
            {
                entity.ToTable("TechnicalSpecialistCustomers", "master");

                entity.HasIndex(e => e.Code)
                    .HasName("IX_Customer1_Code")
                    .IsUnique();

                entity.Property(e => e.Code).HasMaxLength(7);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<TechnicalSpecialistEducationalQualification>(entity =>
            {
                entity.ToTable("TechnicalSpecialistEducationalQualification", "techspecialist");

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.DateFrom).HasColumnType("datetime");

                entity.Property(e => e.DateTo).HasColumnType("datetime");

                entity.Property(e => e.Institution).HasMaxLength(100);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Place).HasMaxLength(100);

                entity.Property(e => e.Qualification).HasMaxLength(100);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TechnicalSpecialistEducationalQualification)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_TechnicalSpecialistEducationalQualification_Data_CityID");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TechnicalSpecialistEducationalQualification)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_TechnicalSpecialistEducationalQualification_Data_CountryID");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.TechnicalSpecialistEducationalQualification)
                    .HasForeignKey(d => d.CountyId)
                    .HasConstraintName("FK_TechnicalSpecialistEducationalQualification_Data_CountyID");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistEducationalQualification)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistLanguageCapability>(entity =>
            {
                entity.ToTable("TechnicalSpecialistLanguageCapability", "techspecialist");

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.HasIndex(e => new { e.LanguageId, e.TechnicalSpecialistId })
                    .HasName("IX_TechnicalSpecialistLanguageCapability_LId_TSId")
                    .IsUnique();

                entity.Property(e => e.ComprehensionCapabilityLevel)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.SpeakingCapabilityLevel)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.WritingCapabilityLevel)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.TechnicalSpecialistLanguageCapability)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TechnicalSpecialistLanguageCapability_Data_LanguageID");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistLanguageCapability)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistNote>(entity =>
            {
                entity.ToTable("TechnicalSpecialistNote", "techspecialist");

                entity.HasIndex(e => e.RecordRefId);

                entity.HasIndex(e => e.RecordType);

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(4000);

                entity.Property(e => e.RecordType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistNote)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistPayRate>(entity =>
            {
                entity.ToTable("TechnicalSpecialistPayRate", "techspecialist");

                entity.HasIndex(e => e.ExpenseTypeId);

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.PayScheduleId);

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Rate).HasColumnType("decimal(12, 4)");

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.HasOne(d => d.ExpenseType)
                    .WithMany(p => p.TechnicalSpecialistPayRate)
                    .HasForeignKey(d => d.ExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PaySchedule)
                    .WithMany(p => p.TechnicalSpecialistPayRate)
                    .HasForeignKey(d => d.PayScheduleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TechnicalSpecialistPayRate_TechnicalSpecialistPaySchedule_PaySchdId");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistPayRate)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TechnicalSpecialistPayRate_TechnicalSpecialist_TSId");
            });

            modelBuilder.Entity<TechnicalSpecialistPaySchedule>(entity =>
            {
                entity.ToTable("TechnicalSpecialistPaySchedule", "techspecialist");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.HasIndex(e => new { e.TechnicalSpecialistId, e.PayScheduleName, e.PayCurrency })
                    .HasName("UK_TSPaySchedule")
                    .IsUnique();

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PayCurrency).HasMaxLength(5);

                entity.Property(e => e.PayScheduleName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PayScheduleNote).HasMaxLength(200);

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistPaySchedule)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistStamp>(entity =>
            {
                entity.ToTable("TechnicalSpecialistStamp", "techspecialist");

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.Property(e => e.IssuedDate).HasColumnType("datetime");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ReturnDate).HasColumnType("datetime");

                entity.Property(e => e.StampNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TechnicalSpecialistStamp)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistStamp)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistTaxonomy>(entity =>
            {
                entity.ToTable("TechnicalSpecialistTaxonomy", "techspecialist");

                entity.HasIndex(e => e.ApprovalStatus);

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.TaxonomyServicesId);

                entity.HasIndex(e => e.TaxonomyStatus);

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.Property(e => e.ApprovalStatus)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ApprovedBy).HasMaxLength(50);

                entity.Property(e => e.Comments).HasMaxLength(4000);

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.Interview).HasMaxLength(50);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.TaxonomyStatus).HasMaxLength(40);

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.HasOne(d => d.TaxonomyCategory)
                    .WithMany(p => p.TechnicalSpecialistTaxonomy)
                    .HasForeignKey(d => d.TaxonomyCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TechnicalSpecialist_Data_TaxonomyCategoryId");

                entity.HasOne(d => d.TaxonomyServices)
                    .WithMany(p => p.TechnicalSpecialistTaxonomy)
                    .HasForeignKey(d => d.TaxonomyServicesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TechnicalSpecialist_TaxonomyService_TaxonomyServicesId");

                entity.HasOne(d => d.TaxonomySubCategory)
                    .WithMany(p => p.TechnicalSpecialistTaxonomy)
                    .HasForeignKey(d => d.TaxonomySubCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TechnicalSpecialist_TaxonomySubCategory_TaxonomySubCategoryId");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistTaxonomy)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistTaxonomyHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId)
                    .HasName("PK__Technica__4D7B4ABD8A9EAF37");

                entity.ToTable("TechnicalSpecialistTaxonomyHistory", "techspecialist");

                entity.Property(e => e.ApprovalStatus).HasMaxLength(100);

                entity.Property(e => e.ApprovedBy).HasMaxLength(50);

                entity.Property(e => e.Comments).HasMaxLength(4000);

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.Interview).HasMaxLength(100);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ToDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TechnicalSpecialistTimeOffRequest>(entity =>
            {
                entity.ToTable("TechnicalSpecialistTimeOffRequest", "techspecialist");

                entity.HasIndex(e => e.FromDate);

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.HasIndex(e => e.ToDate);

                entity.Property(e => e.ApprovalStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ApprovedBy).HasMaxLength(50);

                entity.Property(e => e.Comments).HasMaxLength(4000);

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.RequestedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RequestedOn).HasColumnType("datetime");

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.HasOne(d => d.LeaveType)
                    .WithMany(p => p.TechnicalSpecialistTimeOffRequest)
                    .HasForeignKey(d => d.LeaveTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TechnicalSpecialistTimeOffRequest_Data_LeaveCategoryTypeId");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistTimeOffRequest)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistTrainingAndCompetency>(entity =>
            {
                entity.ToTable("TechnicalSpecialistTrainingAndCompetency", "techspecialist");

                entity.HasIndex(e => e.IsIlearn);

                entity.HasIndex(e => e.RecordType);

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.Property(e => e.Competency).HasMaxLength(255);

                entity.Property(e => e.Duration).HasMaxLength(50);

                entity.Property(e => e.Expiry).HasColumnType("datetime");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.RecordType).HasMaxLength(50);

                entity.Property(e => e.Score).HasMaxLength(200);

                entity.Property(e => e.TrainingDate).HasColumnType("datetime");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistTrainingAndCompetency)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TechnicalSpecialistTrainingAndCompetency_TechnicalSpecialist");
            });

            modelBuilder.Entity<TechnicalSpecialistTrainingAndCompetencyType>(entity =>
            {
                entity.ToTable("TechnicalSpecialistTrainingAndCompetencyType", "techspecialist");

                entity.HasIndex(e => e.TechnicalSpecialistTrainingAndCompetencyId);

                entity.HasOne(d => d.TechnicalSpecialistTrainingAndCompetency)
                    .WithMany(p => p.TechnicalSpecialistTrainingAndCompetencyType)
                    .HasForeignKey(d => d.TechnicalSpecialistTrainingAndCompetencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TechnicalSpecTrainingAndCompetencyType_TechnicalSpecialistTrainingAndCompetencyId");

                entity.HasOne(d => d.TrainingOrCompetencyData)
                    .WithMany(p => p.TechnicalSpecialistTrainingAndCompetencyType)
                    .HasForeignKey(d => d.TrainingOrCompetencyDataId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TechnicalSpecialistWorkHistory>(entity =>
            {
                entity.ToTable("TechnicalSpecialistWorkHistory", "techspecialist");

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.Property(e => e.ClientName).HasMaxLength(100);

                entity.Property(e => e.DateFrom).HasColumnType("datetime");

                entity.Property(e => e.DateTo).HasColumnType("datetime");

                entity.Property(e => e.JobDescription).HasMaxLength(4000);

                entity.Property(e => e.JobResponsibility).HasMaxLength(200);

                entity.Property(e => e.JobTitle).HasMaxLength(100);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ProjectName).HasMaxLength(100);

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TechnicalSpecialistWorkHistory)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TimeOffRequestCategory>(entity =>
            {
                entity.ToTable("TimeOffRequestCategory", "master");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.EmploymentType)
                    .WithMany(p => p.TimeOffRequestCategoryEmploymentType)
                    .HasForeignKey(d => d.EmploymentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_TimeOffRequestCategory_Data_EmploymentTypeId");

                entity.HasOne(d => d.LeaveCategoryType)
                    .WithMany(p => p.TimeOffRequestCategoryLeaveCategoryType)
                    .HasForeignKey(d => d.LeaveCategoryTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_TimeOffRequestCategory_Data_LeaveCategoryTypeId");
            });

            modelBuilder.Entity<Timesheet>(entity =>
            {
                entity.ToTable("Timesheet", "timesheet");

                entity.HasIndex(e => e.AssignmentId);

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_Timesheet_EvoID");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.TimesheetNumber);

                entity.HasIndex(e => new { e.AssignmentId, e.FromDate, e.Id })
                    .HasName("IX_Timesheet_Id")
                    .IsUnique();

                entity.HasIndex(e => new { e.AssignmentId, e.TimesheetStatus, e.FromDate })
                    .HasName("IX_Timesheet_FromDate");

                entity.HasIndex(e => new { e.Id, e.TimesheetStatus, e.IsApprovedByContractCompany })
                    .HasName("IX_Timesheet_IsApprovedByContractHoldingCompany");

                entity.Property(e => e.ClientReviewStatus).HasMaxLength(1);

                entity.Property(e => e.DatePeriod).HasMaxLength(30);

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.ExpectedCompleteDate).HasColumnType("datetime");

                entity.Property(e => e.ExtranetUpdate).HasMaxLength(50);

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedFrom).HasMaxLength(500);

                entity.Property(e => e.IsApprovedByContractCompany).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModeOfCreation).HasMaxLength(50);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.NextTimesheetDate).HasColumnType("datetime");

                entity.Property(e => e.NextTimesheetStatus).HasMaxLength(1);

                entity.Property(e => e.NextVisitDateTo).HasColumnType("datetime");

                entity.Property(e => e.NotificationReference).HasMaxLength(65);

                entity.Property(e => e.ReasonForRejection).HasMaxLength(1000);

                entity.Property(e => e.Reference1).HasMaxLength(50);

                entity.Property(e => e.Reference2).HasMaxLength(50);

                entity.Property(e => e.Reference3).HasMaxLength(50);

                entity.Property(e => e.ResultofInvestigation).HasMaxLength(500);

                entity.Property(e => e.ReviewedBy).HasMaxLength(50);

                entity.Property(e => e.ReviewedDate).HasColumnType("datetime");

                entity.Property(e => e.SendToCustomer).HasColumnType("datetime");

                entity.Property(e => e.SummaryOfReport).HasMaxLength(400);

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.Property(e => e.TimesheetDescription).HasMaxLength(130);

                entity.Property(e => e.TimesheetReject).HasMaxLength(10);

                entity.Property(e => e.TimesheetStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.Property(e => e.VisitCreationOrigin).HasMaxLength(10);

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.Timesheet)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_timesheet.Timesheet_Assignment_AssignmentId");
            });

            modelBuilder.Entity<TimesheetHistory>(entity =>
            {
                entity.ToTable("TimesheetHistory", "timesheet");

                entity.HasIndex(e => e.HistoryItemId);

                entity.HasIndex(e => e.TimesheetId);

                entity.Property(e => e.ChangedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.Property(e => e.TimesheetHistoryDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.HistoryItem)
                    .WithMany(p => p.TimesheetHistory)
                    .HasForeignKey(d => d.HistoryItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimesheetHistory_HistoryItem");

                entity.HasOne(d => d.Timesheet)
                    .WithMany(p => p.TimesheetHistory)
                    .HasForeignKey(d => d.TimesheetId)
                    .HasConstraintName("FK_TimesheetHistory_Timesheet");
            });

            modelBuilder.Entity<TimesheetInterCompanyDiscount>(entity =>
            {
                entity.ToTable("TimesheetInterCompanyDiscount", "timesheet");

                entity.HasIndex(e => new { e.CompanyId, e.DiscountType, e.TimesheetId })
                    .HasName("IX_TimesheetInterCompanyDiscount_CompanyId_TimesheetId");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.DiscountType)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("(N'I')");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TimesheetInterCompanyDiscount)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Timesheet)
                    .WithMany(p => p.TimesheetInterCompanyDiscount)
                    .HasForeignKey(d => d.TimesheetId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TimesheetNote>(entity =>
            {
                entity.ToTable("TimesheetNote", "timesheet");

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_TimesheetNote_EvoID");

                entity.HasIndex(e => e.LastModification);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.HasOne(d => d.Timesheet)
                    .WithMany(p => p.TimesheetNote)
                    .HasForeignKey(d => d.TimesheetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimesheetNote_Timesheet");
            });

            modelBuilder.Entity<TimesheetReference>(entity =>
            {
                entity.ToTable("TimesheetReference", "timesheet");

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_TimesheetReference_EvoID");

                entity.HasIndex(e => new { e.TimesheetId, e.AssignmentReferenceTypeId })
                    .IsUnique();

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ReferenceValue)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.HasOne(d => d.AssignmentReferenceType)
                    .WithMany(p => p.TimesheetReference)
                    .HasForeignKey(d => d.AssignmentReferenceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimesheetReference_Data_assignmentReferenceTypeId");

                entity.HasOne(d => d.Timesheet)
                    .WithMany(p => p.TimesheetReference)
                    .HasForeignKey(d => d.TimesheetId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TimesheetTechnicalSpecialist>(entity =>
            {
                entity.ToTable("TimesheetTechnicalSpecialist", "timesheet");

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_TimesheetTechnicalSpecialist_EvoID");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.TechnicalSpecialistId);

                entity.HasIndex(e => new { e.Id, e.TimesheetId })
                    .HasName("IX_TimesheetTechnicalSpecialist_TimesheetId");

                entity.HasIndex(e => new { e.TechnicalSpecialistId, e.TimesheetId })
                    .HasName("IX_TimesheetTechnicalSpecialist_TimesheetId_TechnicalSpecialistId")
                    .IsUnique();

                entity.HasIndex(e => new { e.TimesheetId, e.Id })
                    .HasName("IX_TimesheetTechnicalSpecialist_Id")
                    .IsUnique();

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.TimesheetTechnicalSpecialist)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Timesheet)
                    .WithMany(p => p.TimesheetTechnicalSpecialist)
                    .HasForeignKey(d => d.TimesheetId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TimesheetTechnicalSpecialistAccountItemConsumable>(entity =>
            {
                entity.ToTable("TimesheetTechnicalSpecialistAccountItemConsumable", "timesheet");

                entity.HasIndex(e => e.AssignmentId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemConsumable  _AssignmentId");

                entity.HasIndex(e => e.ChargeExpenseTypeId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemConsumable  _ChargeExpenseTypeId");

                entity.HasIndex(e => e.ChargeRateId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemConsumable_ContractRate");

                entity.HasIndex(e => e.ContractId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemConsumable_Contract");

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemConsumable_EvoID");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.PayExpenseTypeId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemConsumable  _PayExpenseTypeId");

                entity.HasIndex(e => e.PayRateId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemConsumable  _PayRateId");

                entity.HasIndex(e => e.PayrollPeriodId);

                entity.HasIndex(e => e.ProjectId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemConsumable  _ProjectId");

                entity.HasIndex(e => e.TimesheetId);

                entity.HasIndex(e => e.TimesheetTechnicalSpecialistId);

                entity.HasIndex(e => e.UnpaidReasonId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemConsumable  _UnpaidReasonId");

                entity.HasIndex(e => e.UnpaidStatusId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemConsumable  _UnpaidStatusId");

                entity.HasIndex(e => new { e.ExpenceDate, e.AssignmentId })
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemConsumable_AssignmentId_ExpenseDate");

                entity.Property(e => e.ChargeDescription).HasMaxLength(100);

                entity.Property(e => e.ChargeRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.ChargeRateCurrency).HasMaxLength(50);

                entity.Property(e => e.ChargeTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CostofSalesStatus).HasMaxLength(1);

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.ExpenceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoicingStatus).HasMaxLength(1);

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModeOfCreation).HasMaxLength(5);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PayRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.PayRateCurrency)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PayRateDescription).HasMaxLength(100);

                entity.Property(e => e.PayTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChargeExpenseType)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumableChargeExpenseType)
                    .HasForeignKey(d => d.ChargeExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimesheetTechnicalSpecialistAccountItemConsumable_Data_ChargeExpenceTypeId");

                entity.HasOne(d => d.ChargeRateNavigation)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.ChargeRateId);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CreditNote)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumableCreditNote)
                    .HasForeignKey(d => d.CreditNoteId);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumableInvoice)
                    .HasForeignKey(d => d.InvoiceId);

                entity.HasOne(d => d.PayExpenseType)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumablePayExpenseType)
                    .HasForeignKey(d => d.PayExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PayRateNavigation)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.PayRateId);

                entity.HasOne(d => d.PayrollPeriod)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.PayrollPeriodId)
                    .HasConstraintName("FK_TimesheetTechnicalSpecialistAccountItemConsumable_Company_PayrollPeriod");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumableSalesTax)
                    .HasForeignKey(d => d.SalesTaxId);

                entity.HasOne(d => d.Timesheet)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.TimesheetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimesheetTechnicalSpecialistAccountItemConsumable_timesheet_TimesheetId");

                entity.HasOne(d => d.TimesheetTechnicalSpecialist)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.TimesheetTechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.UnpaidReason)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.UnpaidReasonId);

                entity.HasOne(d => d.UnpaidStatus)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumableUnpaidStatus)
                    .HasForeignKey(d => d.UnpaidStatusId);

                entity.HasOne(d => d.WithholdingTax)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemConsumableWithholdingTax)
                    .HasForeignKey(d => d.WithholdingTaxId);
            });

            modelBuilder.Entity<TimesheetTechnicalSpecialistAccountItemExpense>(entity =>
            {
                entity.ToTable("TimesheetTechnicalSpecialistAccountItemExpense", "timesheet");

                entity.HasIndex(e => e.AssignmentId);

                entity.HasIndex(e => e.ChargeRateId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemExpense_ContractRate");

                entity.HasIndex(e => e.ContractId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemExpense_Contract");

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemExpense_EvoID");

                entity.HasIndex(e => e.ExpenseChargeTypeId)
                    .HasName("IX_TimesheetTechSpecAccItmExpense_ExpenseChargeTypeId");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.PayRateId);

                entity.HasIndex(e => e.PayrollPeriodId);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.TimesheetId);

                entity.HasIndex(e => e.TimesheetTechnicalSpeciallistId);

                entity.HasIndex(e => e.UnpaidReasonId);

                entity.HasIndex(e => e.UnpaidStatusId);

                entity.HasIndex(e => new { e.ExpenseDate, e.AssignmentId })
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemExpense_AssignmentId_ExpenseDate");

                entity.Property(e => e.ChargeExchangeRate).HasColumnType("decimal(16, 6)");

                entity.Property(e => e.ChargeRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.ChargeRateCurrency).HasMaxLength(50);

                entity.Property(e => e.ChargeTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CostofSalesStatus).HasMaxLength(1);

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.ExpenceCurrency).HasMaxLength(50);

                entity.Property(e => e.ExpenseDate).HasColumnType("datetime");

                entity.Property(e => e.ExpenseDescription).HasMaxLength(50);

                entity.Property(e => e.InvoicingStatus).HasMaxLength(1);

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModeOfCreation).HasMaxLength(5);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PayExchangeRate).HasColumnType("decimal(16, 6)");

                entity.Property(e => e.PayRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.PayRateCurrency).HasMaxLength(100);

                entity.Property(e => e.PayRateTax).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.PayTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChargeRateNavigation)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.ChargeRateId);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CreditNote)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpenseCreditNote)
                    .HasForeignKey(d => d.CreditNoteId);

                entity.HasOne(d => d.ExpenseChargeType)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpenseExpenseChargeType)
                    .HasForeignKey(d => d.ExpenseChargeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimesheetTechnicalSpecialistAccountItemExpense_Data_ExpenceChargeTypeId");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpenseInvoice)
                    .HasForeignKey(d => d.InvoiceId);

                entity.HasOne(d => d.PayRateNavigation)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.PayRateId);

                entity.HasOne(d => d.PayrollPeriod)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.PayrollPeriodId)
                    .HasConstraintName("FK_TimesheetTechnicalSpecialistAccountItemExpense_Company_PayrollPeriod");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpenseSalesTax)
                    .HasForeignKey(d => d.SalesTaxId);

                entity.HasOne(d => d.Timesheet)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.TimesheetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimesheetTechnicalSpecialistAccountItemExpense_timesheet_TimesheetId");

                entity.HasOne(d => d.TimesheetTechnicalSpeciallist)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.TimesheetTechnicalSpeciallistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimesheetTechnicalSpecialistAccountItemExpense_TimesheetTechnicalSpecialist_TimesheetTechncalSpecialistId");

                entity.HasOne(d => d.UnpaidReason)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.UnpaidReasonId);

                entity.HasOne(d => d.UnpaidStatus)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpenseUnpaidStatus)
                    .HasForeignKey(d => d.UnpaidStatusId);

                entity.HasOne(d => d.WithholdingTax)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemExpenseWithholdingTax)
                    .HasForeignKey(d => d.WithholdingTaxId);
            });

            modelBuilder.Entity<TimesheetTechnicalSpecialistAccountItemTime>(entity =>
            {
                entity.ToTable("TimesheetTechnicalSpecialistAccountItemTime", "timesheet");

                entity.HasIndex(e => e.AssignmentId);

                entity.HasIndex(e => e.ChargeRateId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemTime_ContractRate");

                entity.HasIndex(e => e.ContractId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemTime_Contract");

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemTime_EvoID");

                entity.HasIndex(e => e.ExpenseChargeTypeId)
                    .HasName("IX_TimesheetTechSpecAccItmTime");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.PayRateId);

                entity.HasIndex(e => e.PayrollPeriodId);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.TimesheetId);

                entity.HasIndex(e => e.TimesheetTechnicalSpeciallistId)
                    .HasName("IX_TimesheetTechSpecAccItmTime_TimesheetTechnicalSpeciallistId");

                entity.HasIndex(e => e.UnpaidReasonId);

                entity.HasIndex(e => e.UnpaidStatusId);

                entity.Property(e => e.ChargeRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.ChargeRateCurrency).HasMaxLength(50);

                entity.Property(e => e.ChargeRateDescription).HasMaxLength(100);

                entity.Property(e => e.ChargeReportUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.ChargeTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.ChargeTravelUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.ChargeWaitUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.ChargeWorkUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CostofSalesStatus).HasMaxLength(1);

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.ExpenseDate).HasColumnType("datetime");

                entity.Property(e => e.ExpenseDescription).HasMaxLength(50);

                entity.Property(e => e.InvoicingStatus).HasMaxLength(1);

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModeOfCreation).HasMaxLength(5);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PayRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.PayRateCurrency).HasMaxLength(50);

                entity.Property(e => e.PayRateDescription).HasMaxLength(100);

                entity.Property(e => e.PayTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.Property(e => e.TimeDescription).HasMaxLength(50);

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChargeRateNavigation)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.ChargeRateId);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CreditNote)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTimeCreditNote)
                    .HasForeignKey(d => d.CreditNoteId);

                entity.HasOne(d => d.ExpenseChargeType)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTimeExpenseChargeType)
                    .HasForeignKey(d => d.ExpenseChargeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTimeInvoice)
                    .HasForeignKey(d => d.InvoiceId);

                entity.HasOne(d => d.PayRateNavigation)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.PayRateId);

                entity.HasOne(d => d.PayrollPeriod)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.PayrollPeriodId)
                    .HasConstraintName("FK_TimesheetTechnicalSpecialistAccountItemTime_Company_PayrollPeriod");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTimeSalesTax)
                    .HasForeignKey(d => d.SalesTaxId);

                entity.HasOne(d => d.Timesheet)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.TimesheetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimesheetTechnicalSpecialistAccountItemTime_timesheet_TimesheetId");

                entity.HasOne(d => d.TimesheetTechnicalSpeciallist)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.TimesheetTechnicalSpeciallistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimesheetTechnicalSpecialistAccountItemTime_TimesheetTechnicalSpecialist_TimesheetTechnicalSpecialistId");

                entity.HasOne(d => d.UnpaidReason)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.UnpaidReasonId);

                entity.HasOne(d => d.UnpaidStatus)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTimeUnpaidStatus)
                    .HasForeignKey(d => d.UnpaidStatusId);

                entity.HasOne(d => d.WithholdingTax)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTimeWithholdingTax)
                    .HasForeignKey(d => d.WithholdingTaxId);
            });

            modelBuilder.Entity<TimesheetTechnicalSpecialistAccountItemTravel>(entity =>
            {
                entity.ToTable("TimesheetTechnicalSpecialistAccountItemTravel", "timesheet");

                entity.HasIndex(e => e.AssignmentId);

                entity.HasIndex(e => e.ChargeExpenseTypeId);

                entity.HasIndex(e => e.ChargeRateId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemTravel_ContractRate");

                entity.HasIndex(e => e.ContractId)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemTravel_Contract");

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemTravel_EvoID");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.PayExpenseTypeId);

                entity.HasIndex(e => e.PayRateId);

                entity.HasIndex(e => e.PayrollPeriodId);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.TimesheetId);

                entity.HasIndex(e => e.TimesheetTechnicalSpecialistId);

                entity.HasIndex(e => e.UnpaidReasonId);

                entity.HasIndex(e => e.UnpaidStatusId);

                entity.HasIndex(e => new { e.ExpenceDate, e.AssignmentId })
                    .HasName("IX_TimesheetTechnicalSpecialistAccountItemTravel_Assignment_ExenseDate");

                entity.Property(e => e.ChargeRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.ChargeRateCurrency).HasMaxLength(50);

                entity.Property(e => e.ChargeTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CostofSalesStatus).HasMaxLength(1);

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.ExpenceDate).HasColumnType("datetime");

                entity.Property(e => e.ExpenseDescription).HasMaxLength(50);

                entity.Property(e => e.InvoicingStatus).HasMaxLength(1);

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModeOfCreation).HasMaxLength(5);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PayRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.PayRateCurrency)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PayTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChargeExpenseType)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravelChargeExpenseType)
                    .HasForeignKey(d => d.ChargeExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimesheetTechnicalSpecialistAccountItemTravel_Data_ChargeExpenceTypeId");

                entity.HasOne(d => d.ChargeRateNavigation)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.ChargeRateId);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CreditNote)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravelCreditNote)
                    .HasForeignKey(d => d.CreditNoteId);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravelInvoice)
                    .HasForeignKey(d => d.InvoiceId);

                entity.HasOne(d => d.PayExpenseType)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravelPayExpenseType)
                    .HasForeignKey(d => d.PayExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PayRateNavigation)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.PayRateId);

                entity.HasOne(d => d.PayrollPeriod)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.PayrollPeriodId)
                    .HasConstraintName("FK_TimesheetTechnicalSpecialistAccountItemTravel_Company_PayrollPeriod");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravelSalesTax)
                    .HasForeignKey(d => d.SalesTaxId);

                entity.HasOne(d => d.Timesheet)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.TimesheetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimesheetTechnicalSpecialistAccountItemTravel_timesheet_TimesheetId");

                entity.HasOne(d => d.TimesheetTechnicalSpecialist)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.TimesheetTechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.UnpaidReason)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.UnpaidReasonId);

                entity.HasOne(d => d.UnpaidStatus)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravelUnpaidStatus)
                    .HasForeignKey(d => d.UnpaidStatusId);

                entity.HasOne(d => d.WithholdingTax)
                    .WithMany(p => p.TimesheetTechnicalSpecialistAccountItemTravelWithholdingTax)
                    .HasForeignKey(d => d.WithholdingTaxId);
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("Token", "finance");

                entity.Property(e => e.CutOffDate).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.IsOpcompany).HasColumnName("IsOPCompany");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.Logger).HasMaxLength(50);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Token1)
                    .IsRequired()
                    .HasColumnName("Token")
                    .HasMaxLength(15);

                entity.Property(e => e.TokenFor).HasMaxLength(100);

                entity.Property(e => e.TokenStatus)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('INITIATED')");
            });

            modelBuilder.Entity<TokenMessage>(entity =>
            {
                entity.ToTable("TokenMessage", "finance");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Period).HasColumnType("datetime");

                entity.HasOne(d => d.Token)
                    .WithMany(p => p.TokenMessage)
                    .HasForeignKey(d => d.TokenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TokenMessage_Token");
            });

            modelBuilder.Entity<UnpaidStatusReason>(entity =>
            {
                entity.ToTable("UnpaidStatusReason", "master");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Resason)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.HasOne(d => d.UnpaidStatus)
                    .WithMany(p => p.UnpaidStatusReason)
                    .HasForeignKey(d => d.UnpaidStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnpaidStatus_UnpaidReason_LNK_Data_UnpaidStatusId");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "security");

                entity.HasIndex(e => e.IsActive);

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.Name);

                entity.HasIndex(e => new { e.SamaccountName, e.ApplicationId })
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.ApplicationId, e.Name, e.SamaccountName, e.Email, e.EmailConfirmed, e.PasswordHash, e.SecurityStamp, e.PhoneNumber, e.PhoneNumberConfirmed, e.LockoutEnabled, e.LockoutEndDateUtc, e.AccessFailedCount, e.CompanyOfficeId, e.IsActive, e.Culture, e.LastModification, e.UpdateCount, e.ModifiedBy, e.AuthenticationMode, e.IsPasswordNeedToBeChange, e.SecurityQuestion1, e.SecurityQuestion1Answer, e.Comments, e.IsErepTrained, e.ExtranetAccessLevel, e.IsShowNewVisit, e.CompanyId })
                    .HasName("IX_User_Company");

                entity.Property(e => e.AuthenticationMode).HasMaxLength(2);

                entity.Property(e => e.Comments).HasMaxLength(255);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Culture).HasMaxLength(10);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ExtranetAccessLevel).HasMaxLength(1);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsErepTrained).HasColumnName("IsERepTrained");

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.LockoutEndDateUtc).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHash).HasColumnType("nvarchar(max)");

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.SamaccountName)
                    .IsRequired()
                    .HasColumnName("SAMAccountName")
                    .HasMaxLength(50);

                entity.Property(e => e.SecurityQuestion1).HasMaxLength(255);

                entity.Property(e => e.SecurityQuestion1Answer).HasMaxLength(255);

                entity.Property(e => e.SecurityStamp).HasColumnType("nvarchar(max)");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.CompanyId);

                entity.HasOne(d => d.CompanyOffice)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.CompanyOfficeId);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole", "security");

                entity.HasIndex(e => e.UserId);

                entity.HasIndex(e => new { e.RoleId, e.UserId, e.CompanyId })
                    .HasName("IX_UserRole_UID_RID_CID")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.UserId, e.RoleId, e.LastModification, e.UpdateCount, e.ModifiedBy, e.ApplicationId, e.CompanyId })
                    .HasName("IX_USerRole_CompanyId");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_UserRole_ApplicationId");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_UserRole_CompanyId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_RoleId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_UserId");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("UserType", "security");

                entity.HasIndex(e => e.CompanyId);

                entity.HasIndex(e => e.UserId);

                entity.HasIndex(e => new { e.UserId, e.CompanyId });

                entity.HasIndex(e => new { e.UserId, e.UserTypeName });

                entity.HasIndex(e => new { e.UserTypeName, e.CompanyId });

                entity.HasIndex(e => new { e.UserId, e.CompanyId, e.UserTypeName })
                    .HasName("IX_UserType_UserTypeName");

                entity.HasIndex(e => new { e.UserId, e.UserTypeName, e.CompanyId });

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.UserTypeName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.UserType)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserType_CompanyId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserType)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserType_UserId");
            });

            modelBuilder.Entity<Visit>(entity =>
            {
                entity.ToTable("Visit", "visit");

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_Visit_EvoID");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.ToDate);

                entity.HasIndex(e => e.VisitNumber);

                entity.HasIndex(e => new { e.AssignmentId, e.VisitStatus, e.FromDate })
                    .HasName("IX_Visit_FromDate");

                entity.HasIndex(e => new { e.Id, e.FromDate, e.AssignmentId })
                    .HasName("IX_Visit_AssignmentId");

                entity.HasIndex(e => new { e.Id, e.VisitStatus, e.IsApprovedByContractCompany })
                    .HasName("IX_Visit_IsApprovedByContractHoldingCompany");

                entity.Property(e => e.ClientReviewStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.DatePeriod).HasMaxLength(30);

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.ExpectedCompleteDate).HasColumnType("datetime");

                entity.Property(e => e.ExtranetUpdate).HasMaxLength(50);

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.InsertedFrom).HasMaxLength(500);

                entity.Property(e => e.IsApprovedByContractCompany).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModeOfCreation).HasMaxLength(50);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.NextVisitDate).HasColumnType("datetime");

                entity.Property(e => e.NextVisitDateTo).HasColumnType("datetime");

                entity.Property(e => e.NextVisitStatus).HasMaxLength(1);

                entity.Property(e => e.NotificationReference).HasMaxLength(25);

                entity.Property(e => e.Reference1).HasMaxLength(50);

                entity.Property(e => e.Reference2).HasMaxLength(50);

                entity.Property(e => e.Reference3).HasMaxLength(50);

                entity.Property(e => e.RejectionReason).HasMaxLength(1000);

                entity.Property(e => e.ReportNumber).HasMaxLength(65);

                entity.Property(e => e.ReportSentToCustomerDate).HasColumnType("datetime");

                entity.Property(e => e.ResultofInvestigation).HasMaxLength(500);

                entity.Property(e => e.ReviewedBy).HasMaxLength(50);

                entity.Property(e => e.ReviewedDate).HasColumnType("datetime");

                entity.Property(e => e.SummaryOfReport).HasMaxLength(400);

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.Property(e => e.VisitCreationOrigin).HasMaxLength(10);

                entity.Property(e => e.VisitReject).HasMaxLength(10);

                entity.Property(e => e.VisitStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.Visit)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Visit)
                    .HasForeignKey(d => d.SupplierId);
            });

            modelBuilder.Entity<VisitHistory>(entity =>
            {
                entity.ToTable("VisitHistory", "visit");

                entity.HasIndex(e => e.HistoryItemId);

                entity.HasIndex(e => e.VisitId);

                entity.Property(e => e.ChangedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.Property(e => e.VisitHistoryDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.HistoryItem)
                    .WithMany(p => p.VisitHistory)
                    .HasForeignKey(d => d.HistoryItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VisitHistory_HistoryItem");

                entity.HasOne(d => d.Visit)
                    .WithMany(p => p.VisitHistory)
                    .HasForeignKey(d => d.VisitId)
                    .HasConstraintName("FK_VisitHistory_Visit");
            });

            modelBuilder.Entity<VisitInterCompanyDiscount>(entity =>
            {
                entity.ToTable("VisitInterCompanyDiscount", "visit");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => new { e.CompanyId, e.DiscountType, e.VisitId })
                    .HasName("IX_VisitInterCompanyDiscount_CompanyId_VisitId");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.DiscountType)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Percentage).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("(N'I')");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.VisitInterCompanyDiscount)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Visit)
                    .WithMany(p => p.VisitInterCompanyDiscount)
                    .HasForeignKey(d => d.VisitId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<VisitNote>(entity =>
            {
                entity.ToTable("VisitNote", "visit");

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_VisitNote_EvoID");

                entity.HasIndex(e => e.LastModification)
                    .HasName("IX_VisitNote_LastModification");

                entity.HasIndex(e => e.VisitId);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.LastModification)
                    .HasColumnName("lastModification")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.HasOne(d => d.Visit)
                    .WithMany(p => p.VisitNote)
                    .HasForeignKey(d => d.VisitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VisitNote_Visit");
            });

            modelBuilder.Entity<VisitReference>(entity =>
            {
                entity.ToTable("VisitReference", "visit");

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_VisitReference_EvoID");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => new { e.VisitId, e.AssignmentReferenceTypeId })
                    .IsUnique();

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ReferenceValue)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.HasOne(d => d.AssignmentReferenceType)
                    .WithMany(p => p.VisitReference)
                    .HasForeignKey(d => d.AssignmentReferenceTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Visit)
                    .WithMany(p => p.VisitReference)
                    .HasForeignKey(d => d.VisitId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<VisitRejectedCount>(entity =>
            {
                entity.ToTable("VisitRejectedCount", "visit");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.Property(e => e.VisitStatus)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VisitSupplierPerformance>(entity =>
            {
                entity.ToTable("VisitSupplierPerformance", "visit");

                entity.HasIndex(e => new { e.PerformanceType, e.VisitId })
                    .HasName("IX_VisitSupplierPerformance_VisitId_PerformanceType");

                entity.Property(e => e.LastModification).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.NcrcloseOutDate)
                    .HasColumnName("NCRCloseOutDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.PerformanceType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Score)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Visit)
                    .WithMany(p => p.VisitSupplierPerformance)
                    .HasForeignKey(d => d.VisitId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<VisitTechnicalSpecialist>(entity =>
            {
                entity.ToTable("VisitTechnicalSpecialist", "visit");

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_VisitTechnicalSpecialist_EvoID");

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.VisitId)
                    .HasName("IX_VisitTechnicalSpecialist_Visit");

                entity.HasIndex(e => new { e.VisitId, e.TechnicalSpecialistId })
                    .HasName("IX_VisitTechnicalSpecialist_VisitId_TechnicalSpecialListId")
                    .IsUnique();

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.HasOne(d => d.TechnicalSpecialist)
                    .WithMany(p => p.VisitTechnicalSpecialist)
                    .HasForeignKey(d => d.TechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Visit)
                    .WithMany(p => p.VisitTechnicalSpecialist)
                    .HasForeignKey(d => d.VisitId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<VisitTechnicalSpecialistAccountItemConsumable>(entity =>
            {
                entity.ToTable("VisitTechnicalSpecialistAccountItemConsumable", "visit");

                entity.HasIndex(e => e.AssignmentId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemConsumable _AssignmentId");

                entity.HasIndex(e => e.ChargeExpenseTypeId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemConsumable _ChargeExpenseTypeId");

                entity.HasIndex(e => e.ChargeRateId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemConsumable_ContractRate");

                entity.HasIndex(e => e.ContractId);

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemConsumable_EvoID");

                entity.HasIndex(e => e.PayExpenseTypeId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemConsumable _PayExpenseTypeId");

                entity.HasIndex(e => e.PayRateId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemConsumable _PayRateId");

                entity.HasIndex(e => e.PayrollPeriodId);

                entity.HasIndex(e => e.ProjectId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemConsumable _ProjectId");

                entity.HasIndex(e => e.UnpaidReasonId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemConsumable _UnpaidReasonId");

                entity.HasIndex(e => e.UnpaidStatusId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemConsumable _UnpaidStatusId");

                entity.HasIndex(e => new { e.ExpenceDate, e.AssignmentId })
                    .HasName("IX_VisitTechnicalSpecialistAccountItemConsumable_AssignmentId_Expense");

                entity.HasIndex(e => new { e.ProjectId, e.AssignmentId, e.VisitId })
                    .HasName("IX_VisitTechnicalSpecialistAccountItemConsumable_VisitId");

                entity.Property(e => e.ChargeDescription).HasMaxLength(100);

                entity.Property(e => e.ChargeRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.ChargeRateCurrency).HasMaxLength(50);

                entity.Property(e => e.ChargeTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CostofSalesStatus).HasMaxLength(1);

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.ExpenceDate).HasColumnType("datetime");

                entity.Property(e => e.InvoicingStatus).HasMaxLength(1);

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModeOfCreation).HasMaxLength(5);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PayRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.PayRateCurrency)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PayRateDescription).HasMaxLength(100);

                entity.Property(e => e.PayTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChargeExpenseType)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumableChargeExpenseType)
                    .HasForeignKey(d => d.ChargeExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChargeRateNavigation)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.ChargeRateId);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CreditNote)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumableCreditNote)
                    .HasForeignKey(d => d.CreditNoteId);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumableInvoice)
                    .HasForeignKey(d => d.InvoiceId);

                entity.HasOne(d => d.PayExpenseType)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumablePayExpenseType)
                    .HasForeignKey(d => d.PayExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PayRateNavigation)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.PayRateId);

                entity.HasOne(d => d.PayrollPeriod)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.PayrollPeriodId)
                    .HasConstraintName("FK_VisitTechnicalSpecialistAccountItemConsumable_Company_PayrollPeriod");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumableSalesTax)
                    .HasForeignKey(d => d.SalesTaxId);

                entity.HasOne(d => d.UnpaidReason)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.UnpaidReasonId);

                entity.HasOne(d => d.UnpaidStatus)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumableUnpaidStatus)
                    .HasForeignKey(d => d.UnpaidStatusId);

                entity.HasOne(d => d.Visit)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.VisitId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.VisitTechnicalSpecialist)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumable)
                    .HasForeignKey(d => d.VisitTechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.WithholdingTax)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemConsumableWithholdingTax)
                    .HasForeignKey(d => d.WithholdingTaxId);
            });

            modelBuilder.Entity<VisitTechnicalSpecialistAccountItemExpense>(entity =>
            {
                entity.ToTable("VisitTechnicalSpecialistAccountItemExpense", "visit");

                entity.HasIndex(e => e.AssignmentId);

                entity.HasIndex(e => e.ChargeRateId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemExpense_ContractRate");

                entity.HasIndex(e => e.ContractId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemExpense_Contract");

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemExpense_EvoID");

                entity.HasIndex(e => e.ExpenseChargeTypeId);

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.PayRateId);

                entity.HasIndex(e => e.PayrollPeriodId);

                entity.HasIndex(e => e.ProjectId);

                entity.HasIndex(e => e.UnpaidReasonId);

                entity.HasIndex(e => e.UnpaidStatusId);

                entity.HasIndex(e => e.VisitId);

                entity.HasIndex(e => e.VisitTechnicalSpeciallistId)
                    .HasName("IX_VisitTechSpecAccItmExpense_VisitTechnicalSpeciallistId");

                entity.HasIndex(e => new { e.ExpenseDate, e.AssignmentId })
                    .HasName("IX_VisitTechnicalSpecialistAccountItemExpense_AssignmentId_Expense");

                entity.Property(e => e.ChargeExchangeRate).HasColumnType("decimal(16, 6)");

                entity.Property(e => e.ChargeRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.ChargeRateCurrency).HasMaxLength(50);

                entity.Property(e => e.ChargeTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CostofSalesStatus).HasMaxLength(1);

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.ExpenceCurrency).HasMaxLength(50);

                entity.Property(e => e.ExpenseDate).HasColumnType("datetime");

                entity.Property(e => e.ExpenseDescription).HasMaxLength(50);

                entity.Property(e => e.InvoicingStatus).HasMaxLength(1);

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModeOfCreation).HasMaxLength(5);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PayExchangeRate).HasColumnType("decimal(16, 6)");

                entity.Property(e => e.PayRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.PayRateCurrency).HasMaxLength(100);

                entity.Property(e => e.PayRateTax).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.PayTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChargeRateNavigation)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.ChargeRateId);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CreditNote)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpenseCreditNote)
                    .HasForeignKey(d => d.CreditNoteId);

                entity.HasOne(d => d.ExpenseChargeType)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpenseExpenseChargeType)
                    .HasForeignKey(d => d.ExpenseChargeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpenseInvoice)
                    .HasForeignKey(d => d.InvoiceId);

                entity.HasOne(d => d.PayRateNavigation)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.PayRateId);

                entity.HasOne(d => d.PayrollPeriod)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.PayrollPeriodId)
                    .HasConstraintName("FK_VisitTechnicalSpecialistAccountItemExpense_Company_PayrollPeriod");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpenseSalesTax)
                    .HasForeignKey(d => d.SalesTaxId);

                entity.HasOne(d => d.UnpaidReason)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.UnpaidReasonId);

                entity.HasOne(d => d.UnpaidStatus)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpenseUnpaidStatus)
                    .HasForeignKey(d => d.UnpaidStatusId);

                entity.HasOne(d => d.Visit)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.VisitId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.VisitTechnicalSpeciallist)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpense)
                    .HasForeignKey(d => d.VisitTechnicalSpeciallistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VisitTechnicalSpecialistAccountItemExpense_VisitTechnicalSpecialist_VisitTechnicalSpecialistId");

                entity.HasOne(d => d.WithholdingTax)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemExpenseWithholdingTax)
                    .HasForeignKey(d => d.WithholdingTaxId);
            });

            modelBuilder.Entity<VisitTechnicalSpecialistAccountItemTime>(entity =>
            {
                entity.ToTable("VisitTechnicalSpecialistAccountItemTime", "visit");

                entity.HasIndex(e => e.AssignmentId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemTime _AssignmentId");

                entity.HasIndex(e => e.ChargeRateId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemTime_ContractRate");

                entity.HasIndex(e => e.ContractId);

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemTime_EvoID");

                entity.HasIndex(e => e.ExpenseChargeTypeId);

                entity.HasIndex(e => e.LastModification);

                entity.HasIndex(e => e.PayRateId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemTime _PayRateId");

                entity.HasIndex(e => e.PayrollPeriodId);

                entity.HasIndex(e => e.ProjectId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemTime _ProjectId");

                entity.HasIndex(e => e.UnpaidReasonId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemTime _UnpaidReasonId");

                entity.HasIndex(e => e.UnpaidStatusId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemTime _UnpaidStatusId");

                entity.HasIndex(e => e.VisitId);

                entity.HasIndex(e => e.VisitTechnicalSpeciallistId)
                    .HasName("IX_VisitYechSpecAccItem_VisitTechnicalSpeciallistId");

                entity.Property(e => e.ChargeRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.ChargeRateCurrency).HasMaxLength(50);

                entity.Property(e => e.ChargeRateDescription).HasMaxLength(100);

                entity.Property(e => e.ChargeReportUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.ChargeTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.ChargeTravelUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.ChargeWaitUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.ChargeWorkUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CostofSalesStatus).HasMaxLength(1);

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.ExpenseDate).HasColumnType("datetime");

                entity.Property(e => e.ExpenseDescription).HasMaxLength(50);

                entity.Property(e => e.InvoicingStatus).HasMaxLength(1);

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModeOfCreation).HasMaxLength(5);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PayRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.PayRateCurrency).HasMaxLength(50);

                entity.Property(e => e.PayRateDescription).HasMaxLength(100);

                entity.Property(e => e.PayTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.Property(e => e.TimeDescription).HasMaxLength(50);

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChargeRateNavigation)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.ChargeRateId);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CreditNote)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTimeCreditNote)
                    .HasForeignKey(d => d.CreditNoteId);

                entity.HasOne(d => d.ExpenseChargeType)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTimeExpenseChargeType)
                    .HasForeignKey(d => d.ExpenseChargeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTimeInvoice)
                    .HasForeignKey(d => d.InvoiceId);

                entity.HasOne(d => d.PayRateNavigation)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.PayRateId);

                entity.HasOne(d => d.PayrollPeriod)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.PayrollPeriodId)
                    .HasConstraintName("FK_VisitTechnicalSpecialistAccountItemTime_Company_PayrollPeriod");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTimeSalesTax)
                    .HasForeignKey(d => d.SalesTaxId);

                entity.HasOne(d => d.UnpaidReason)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.UnpaidReasonId);

                entity.HasOne(d => d.UnpaidStatus)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTimeUnpaidStatus)
                    .HasForeignKey(d => d.UnpaidStatusId);

                entity.HasOne(d => d.Visit)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.VisitId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.VisitTechnicalSpeciallist)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTime)
                    .HasForeignKey(d => d.VisitTechnicalSpeciallistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VisitTechnicalSpecialistAccountItemTime_VisitTechnicalSpecialist_VisitTechnicalSpecialistId");

                entity.HasOne(d => d.WithholdingTax)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTimeWithholdingTax)
                    .HasForeignKey(d => d.WithholdingTaxId);
            });

            modelBuilder.Entity<VisitTechnicalSpecialistAccountItemTravel>(entity =>
            {
                entity.ToTable("VisitTechnicalSpecialistAccountItemTravel", "visit");

                entity.HasIndex(e => e.AssignmentId)
                    .HasName("IX_ VisitTechnicalSpecialistAccountItemTravel_AssignmentId");

                entity.HasIndex(e => e.ChargeExpenseTypeId)
                    .HasName("IX_ VisitTechnicalSpecialistAccountItemTravel_ChargeExpenseTypeId");

                entity.HasIndex(e => e.ChargeRateId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemTravel_ContractRate");

                entity.HasIndex(e => e.ContractId)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemTravel_Contract");

                entity.HasIndex(e => e.Evoid)
                    .HasName("IX_VisitTechnicalSpecialistAccountItemTravel_EvoID");

                entity.HasIndex(e => e.PayExpenseTypeId)
                    .HasName("IX_ VisitTechnicalSpecialistAccountItemTravel_PayExpenseTypeId");

                entity.HasIndex(e => e.PayRateId)
                    .HasName("IX_ VisitTechnicalSpecialistAccountItemTravel_PayRateId");

                entity.HasIndex(e => e.PayrollPeriodId);

                entity.HasIndex(e => e.ProjectId)
                    .HasName("IX_ VisitTechnicalSpecialistAccountItemTravel_ProjectId");

                entity.HasIndex(e => e.UnpaidReasonId)
                    .HasName("IX_ VisitTechnicalSpecialistAccountItemTravel_UnpaidReasonId");

                entity.HasIndex(e => e.UnpaidStatusId)
                    .HasName("IX_ VisitTechnicalSpecialistAccountItemTravel_UnpaidStatusId");

                entity.HasIndex(e => e.VisitId);

                entity.HasIndex(e => e.VisitTechnicalSpecialistId)
                    .HasName("IX_VisitTechSpecAccItmTravel_VisitTechnicalSpecialistId");

                entity.HasIndex(e => new { e.ExpenceDate, e.AssignmentId })
                    .HasName("IX_VisitTechnicalSpecialistAccountItemTravel_AssignmentId_Expense");

                entity.Property(e => e.ChargeRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.ChargeRateCurrency).HasMaxLength(50);

                entity.Property(e => e.ChargeTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.CostofSalesStatus).HasMaxLength(1);

                entity.Property(e => e.Evoid).HasColumnName("EVOID");

                entity.Property(e => e.ExpenceDate).HasColumnType("datetime");

                entity.Property(e => e.ExpenseDescription).HasMaxLength(50);

                entity.Property(e => e.InvoicingStatus).HasMaxLength(1);

                entity.Property(e => e.LastModification)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ModeOfCreation).HasMaxLength(5);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.PayRate).HasColumnType("decimal(16, 4)");

                entity.Property(e => e.PayRateCurrency)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PayTotalUnit).HasColumnType("decimal(16, 2)");

                entity.Property(e => e.SyncFlag)
                    .HasColumnName("syncFlag")
                    .HasMaxLength(4)
                    .HasDefaultValueSql("('I')");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChargeExpenseType)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravelChargeExpenseType)
                    .HasForeignKey(d => d.ChargeExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ChargeRateNavigation)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.ChargeRateId);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CreditNote)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravelCreditNote)
                    .HasForeignKey(d => d.CreditNoteId);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravelInvoice)
                    .HasForeignKey(d => d.InvoiceId);

                entity.HasOne(d => d.PayExpenseType)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravelPayExpenseType)
                    .HasForeignKey(d => d.PayExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PayRateNavigation)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.PayRateId);

                entity.HasOne(d => d.PayrollPeriod)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.PayrollPeriodId)
                    .HasConstraintName("FK_VisitTechnicalSpecialistAccountItemTravel_Company_PayrollPeriod");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SalesTax)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravelSalesTax)
                    .HasForeignKey(d => d.SalesTaxId);

                entity.HasOne(d => d.UnpaidReason)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.UnpaidReasonId);

                entity.HasOne(d => d.UnpaidStatus)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravelUnpaidStatus)
                    .HasForeignKey(d => d.UnpaidStatusId);

                entity.HasOne(d => d.Visit)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.VisitId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.VisitTechnicalSpecialist)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravel)
                    .HasForeignKey(d => d.VisitTechnicalSpecialistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.WithholdingTax)
                    .WithMany(p => p.VisitTechnicalSpecialistAccountItemTravelWithholdingTax)
                    .HasForeignKey(d => d.WithholdingTaxId);
            });
        }
    }
}
