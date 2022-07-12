using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Company.Domain.Models.Companies
{
    public class CompanyInvoice : BaseModel
    {
        //public int? CompanyInvoiceId { get; set; }
        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Invoice Summarry Text")]
        public string InvoiceSummarryText { get; set; }

        [AuditNameAttribute("Invoice Inter Company Description")]
        public string InvoiceInterCompDescription { get; set; }

        [AuditNameAttribute("Invoice Inter Company Draft Text")]
        public string InvoiceInterCompDraftText { get; set; }

        [AuditNameAttribute("Invoice Inter Company Text")]
        public string InvoiceInterCompText { get; set; }

        [AuditNameAttribute("Invoice Description Text")]
        public string InvoiceDescriptionText { get; set; }

        [AuditNameAttribute("Invoice Draft Text ")]
        public string InvoiceDraftText { get; set; }

        [AuditNameAttribute("Invoice Header")]
        public string InvoiceHeader { get; set; }

        [AuditNameAttribute(" Tech Specialist Extranet Comment")]
        public string TechSpecialistExtranetComment { get; set; }

        [AuditNameAttribute(" Customer Extranet Comment")]
        public string CustomerExtranetComment { get; set; }

        [AuditNameAttribute(" Reverse Charge Disclaimer")]
        public string ReverseChargeDisclaimer { get; set; }

        [AuditNameAttribute("Invoice Remittances")]
        public IList<CompanyMessage> InvoiceRemittances { get; set; }
        
        [AuditNameAttribute("Invoice Footers")]
        public IList<CompanyMessage> InvoiceFooters { get; set; }
    }

    public class CompanyPartialInvoice : BaseModel
    {
        //public int? CompanyInvoiceId { get; set; }
        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Invoice Summarry Text")]
        public string InvoiceSummarryText { get; set; }

        [AuditNameAttribute("Invoice Inter Company Description")]
        public string InvoiceInterCompDescription { get; set; }

        [AuditNameAttribute("Invoice Inter Company Draft Text")]
        public string InvoiceInterCompDraftText { get; set; }

        [AuditNameAttribute("Invoice Inter Company Text")]
        public string InvoiceInterCompText { get; set; }

        [AuditNameAttribute("Invoice Description Text")]
        public string InvoiceDescriptionText { get; set; }

        [AuditNameAttribute("Invoice Draft Text")]
        public string InvoiceDraftText { get; set; }

        [AuditNameAttribute("Invoice Header")]
        public string InvoiceHeader { get; set; }

        [AuditNameAttribute(" Tech Specialist Extranet Comment")]
        public string TechSpecialistExtranetComment { get; set; }

        [AuditNameAttribute(" Customer Extranet Comment")]
        public string CustomerExtranetComment { get; set; }

        [AuditNameAttribute(" Reverse Charge Disclaimer")]
        public string ReverseChargeDisclaimer { get; set; }
    }
}
