using Evolution.Common.Models.Documents;
using Evolution.Document.Domain.Models.Document;
using System.Collections.Generic;
using DBModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.Company.Domain.Models.Companies
{
    public class CompanyDetail
    {
        public Company CompanyInfo { get; set; }

        public IList<CompanyAddress> CompanyOffices { get; set; }

        public IList<CompanyDivision> CompanyDivisions { get; set; }

        public IList<CompanyDivisionCostCenter> CompanyDivisionCostCenters { get; set; }

        public CompanyInvoice CompanyInvoiceInfo { get; set; }

        public CompanyEmailTemplate CompanyEmailTemplates { get; set; }

        public IList<CompanyPayroll> CompanyPayrolls { get; set; }

        public IList<CompanyPayrollPeriod> CompanyPayrollPeriods { get; set; }

        public IList<CompanyExpectedMargin> CompanyExpectedMargins { get; set; }

        public IList<CompanyTax> CompanyTaxes { get; set; }

        public IList<CompanyQualification> CompanyQualifications { get; set; }

        public IList<CompanyNote> CompanyNotes { get; set; }

        public IList<ModuleDocument> CompanyDocuments { get; set; }

        public IList<DBModel.SqlauditModule> dbModule = null;
    }
}
