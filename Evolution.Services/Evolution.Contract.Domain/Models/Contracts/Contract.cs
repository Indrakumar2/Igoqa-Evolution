using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;

namespace Evolution.Contract.Domain.Models.Contracts
{
    public class Contract : BaseContract
    {
        private decimal _contractRemainingBudgetValue = 0;
        private decimal _contractRemainingBudgetHours = 0;
        private decimal _contractBudgetValueWarningPercentage = 0;
        private decimal _contractBudgetHoursWarningPercentage = 0;
        [AuditNameAttribute("Budget Monetary Value ")]
        public decimal ContractBudgetMonetaryValue { get; set; }

        [AuditNameAttribute("Budget Monetary Currency ")]
        public string ContractBudgetMonetaryCurrency { get; set; }

        [AuditNameAttribute("Budget Monetary Warning")]
        public int? ContractBudgetMonetaryWarning { get; set; }

        [AuditNameAttribute("Budget Hours ")]
        public decimal ContractBudgetHours { get; set; }

        [AuditNameAttribute("Budget Hours Warning ")]
        public int? ContractBudgetHoursWarning { get; set; }


        public decimal ContractInvoicedToDate { get; set; }


        public decimal ContractUninvoicedToDate { get; set; }


        public decimal ContractHoursInvoicedToDate { get; set; }


        public decimal ContractHoursUninvoicedToDate { get; set; }


        public decimal ContractRemainingBudgetValue
        {
            get
            {
                return _contractRemainingBudgetValue = (ContractBudgetMonetaryValue - Math.Round(ContractInvoicedToDate, 2) - Math.Round(ContractUninvoicedToDate, 2));
            }
        }

        public decimal ContractRemainingBudgetHours
        {
            get
            {
                return _contractRemainingBudgetHours = (ContractBudgetHours - Math.Round(ContractHoursInvoicedToDate, 2) - Math.Round(ContractHoursUninvoicedToDate, 2));
            }
        }


        public decimal ContractBudgetValueWarningPercentage
        {
            get
            {
                var value = ContractBudgetMonetaryValue > 0 ? ((Math.Round(ContractInvoicedToDate, 2) + Math.Round(ContractUninvoicedToDate, 2)) / ContractBudgetMonetaryValue) * 100 : 0;
                return _contractBudgetValueWarningPercentage = value > 0 && value >= ContractBudgetMonetaryWarning ? Math.Round(value, 2) : 0;
            }
        }


        public decimal ContractBudgetHourWarningPercentage
        {
            get
            {
                var value = ContractBudgetHours > 0 ? ((Math.Round(ContractHoursInvoicedToDate, 2) + Math.Round(ContractHoursUninvoicedToDate, 2)) / ContractBudgetHours) * 100 : 0;
                return _contractBudgetHoursWarningPercentage = value > 0 && value >= ContractBudgetHoursWarning ? Math.Round(value, 2) : 0;
            }
        }

        public bool? IsParentContract { get; set; }

        [AuditNameAttribute("Parent Contract Invoice Used")]
        public bool? IsParentContractInvoiceUsed { get; set; }


        public bool? IsChildContract { get; set; }

        [AuditNameAttribute(" Parent Company Code")]
        public string ParentCompanyCode { get; set; }

        [AuditNameAttribute(" Parent Company Office")]
        public string ParentCompanyOffice { get; set; }

        [AuditNameAttribute("Parent Contract Discount")]
        public decimal? ParentContractDiscount { get; set; }

        [AuditNameAttribute("Parent Contract Holder")]
        public string ParentContractHolder { get; set; }

        [AuditNameAttribute("Created Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? CreatedDate { get; set; }

        public bool? IsFrameworkContract { get; set; }

        [AuditNameAttribute("Framework Company Code")]
        public string FrameworkCompanyCode { get; set; }

        [AuditNameAttribute("Framework Company Office Name")]
        public string FrameworkCompanyOfficeName { get; set; }


        public bool? IsRelatedFrameworkContract { get; set; }

        [AuditNameAttribute(" Framework Contract Number")]
        public string FrameworkContractNumber { get; set; }

        [AuditNameAttribute("Framework Contract Holder")]
        public string FrameworkContractHolder { get; set; }

        [AuditNameAttribute("In iConnect?")]
        public bool? IsCRM { get; set; }


        [AuditNameAttribute("iConnect Reason")]
        public string ContractCRMReason { get; set; }

        [AuditNameAttribute("Contract Client Reporting Requirement")]
        public string ContractClientReportingRequirement { get; set; }

        [AuditNameAttribute("Assignment Instruction Notes")]
        public string ContractOperationalNote { get; set; }

        [AuditNameAttribute("Contract Invoice Payment Terms")]
        public string ContractInvoicePaymentTerms { get; set; }

        [AuditNameAttribute("Contract Customer Contact")]
        public string ContractCustomerContact { get; set; }

        [AuditNameAttribute("Contract Customer Contact Address")]
        public string ContractCustomerContactAddress { get; set; }

        [AuditNameAttribute("Contract Customer Invoice Contact")]
        public string ContractCustomerInvoiceContact { get; set; }

        [AuditNameAttribute("Contract Customer Invoice Address")]
        public string ContractCustomerInvoiceAddress { get; set; }

        [AuditNameAttribute("Contract Invoice Remittance Identifier")]
        public string ContractInvoiceRemittanceIdentifier { get; set; }

        [AuditNameAttribute("Contract Sales Tax")]
        public string ContractSalesTax { get; set; }

        [AuditNameAttribute("Contract Withholding Tax")]
        public string ContractWithHoldingTax { get; set; }

        [AuditNameAttribute("Invoicing Currency")]
        public string ContractInvoicingCurrency { get; set; }

        [AuditNameAttribute("Invoice Grouping")]
        public string ContractInvoiceGrouping { get; set; }

        [AuditNameAttribute("Invoice Footer Identifier")]
        public string ContractInvoiceFooterIdentifier { get; set; }

        [AuditNameAttribute("Invoice Instruction Notes")]
        public string ContractInvoiceInstructionNotes { get; set; }

        [AuditNameAttribute("Invoice Free Text")]
        public string ContractInvoiceFreeText { get; set; }

        [AuditNameAttribute("Conflict Of Interest")]
        public string ContractConflictOfInterest { get; set; }

        [AuditNameAttribute("Is Fixed Exchange Rate Used")]
        public bool? IsFixedExchangeRateUsed { get; set; }

        [AuditNameAttribute("Remittance Text")]
        public bool? IsRemittanceText { get; set; }

        [AuditNameAttribute("Contract Invoicing Company Code")]
        public string ContractInvoicingCompanyCode { get; set; }

        [AuditNameAttribute("Contract Invoicing Company Name")]
        public string ContractInvoicingCompanyName { get; set; }
        public ParentContractInvoicingDetails ParentContractInvoicingDetails { get; set; }
        public List<string> ContractExpense { get; set; }
        public List<string> ContractRef { get; set; }
        public List<string> ContractCurrency { get; set; }
        public IList<ContractExchangeRate> ContractExchange { get; set; }
        public string ContractSaveOrigin { get; set; }
    }

    public class ContractWithId : Contract
    {
        public int ContractId { get; set; }
        public string MergedContractData
        {
            get
            {
                return ContractNumber + "|" + CustomerContractNumber;
            }
        }
        public int? Evolution1Id { get; set; }
        public int? MasterDataTypeId { get; set; }
    }

    public class ParentContractInvoicingDetails
    {
        [AuditNameAttribute("Parent Contract Sales Tax")]
        public string ParentContractSalesTax { get; set; }

        [AuditNameAttribute("Parent Contract Withholding Tax")]
        public string ParentContractWithHoldingTax { get; set; }

        [AuditNameAttribute("Parent Contract Invoice Footer Identifier")]
        public string ParentContractInvoiceFooterIdentifier { get; set; }

        [AuditNameAttribute("Parent Contract Invoice Remittance Identifier")]
        public string ParentContractInvoiceRemittanceIdentifier { get; set; }

        [AuditNameAttribute("Parent Contract Invoice Used")]
        public bool? IsParentContractInvoiceUsed { get; set; }
    }
}
