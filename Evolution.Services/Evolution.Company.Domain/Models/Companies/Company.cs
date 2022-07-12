using Evolution.Common.Models.Base;
using System.Collections.Generic;

namespace Evolution.Company.Domain.Models.Companies
{
    public class Company : BaseModel
    {
        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Company Name")]
        public string CompanyName { get; set; }

        [AuditNameAttribute("Invoice Company Name ")]
        public string InvoiceName { get; set; }

        [AuditNameAttribute("Is Active")]
        public bool? IsActive { get; set; }

        [AuditNameAttribute("Currency")]
        public string Currency { get; set; }

        [AuditNameAttribute("Sales Tax Description")]
        public string SalesTaxDescription { get; set; }

        [AuditNameAttribute("Withholding Tax Description")]
        public string WithholdingTaxDescription { get; set; }

        [AuditNameAttribute("Inter Company Expense Account Ref")]
        public string InterCompanyExpenseAccRef { get; set; }

        [AuditNameAttribute("Inter Company Rate Account Ref ")]
        public string InterCompanyRateAccRef { get; set; }

        [AuditNameAttribute("Inter Company Royalty Account Ref")]
        public string InterCompanyRoyaltyAccRef { get; set; }

        [AuditNameAttribute("Company MIIWA Id")]
        public int? CompanyMiiwaid { get; set; }

        public int? OperatingCountry { get; set; }//IGO QC D-906

        [AuditNameAttribute("Operating Country ")]
        public string OperatingCountryName { get; set; }

        [AuditNameAttribute("Company MIIWA Ref")]
        public int? CompanyMiiwaref { get; set; }

        [AuditNameAttribute("Is Use ICTMS")]
        public bool? IsUseIctms { get; set; }

        [AuditNameAttribute("Is Full Use")]
        public bool? IsFullUse { get; set; }

        [AuditNameAttribute("Gfs COA")]
        public string GfsCoa { get; set; }

        [AuditNameAttribute("Gfs BU")]
        public string GfsBu { get; set; }

        [AuditNameAttribute("Region")]
        public string Region { get; set; }

        [AuditNameAttribute(" Is COS Email Override Allow")]
        public bool? IsCOSEmailOverrideAllow { get; set; }

        [AuditNameAttribute("  Avg TS Hourly Cost")]
        public decimal? AvgTSHourlyCost { get; set; }

        [AuditNameAttribute("VAT/TAX Reg No")]
        public string VatTaxRegNo { get; set; }

        [AuditNameAttribute(" EUV at Prefix")]
        public string EUVatPrefix { get; set; }

        [AuditNameAttribute("IA Region")]
        public string IARegion { get; set; }

        [AuditNameAttribute("Cognos Number")]
        public string CognosNumber { get; set; }

        [AuditNameAttribute("Logo ")]
        public string LogoText { get; set; }

        [AuditNameAttribute("Resource Outside Distance ")]
        public int ResourceOutsideDistance { get; set; }

        // CR560
        [AuditNameAttribute("VAT Regulation Text (Within EC) ")]
        public string VATRegulationTextWithinEC { get; set; }

        // CR560
        [AuditNameAttribute("VAT Regulation Text (Outside EC) ")]
        public string VATRegulationTextOutsideEC { get; set; }

        public bool IsFromRefresh { get; set; }
    }

    public class CompanySearch : Company
    {
        [AuditNameAttribute("Company Codes")]
        public IList<string> CompanyCodes { get; set; }

        [AuditNameAttribute("Search Document Type")]
        public string SearchDocumentType { get; set; }

        [AuditNameAttribute("Document Search Text")]
        public string DocumentSearchText { get; set; }
    }
}