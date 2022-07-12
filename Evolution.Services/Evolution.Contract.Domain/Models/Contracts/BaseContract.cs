using Evolution.Common.Models.Base;
using System;

namespace Evolution.Contract.Domain.Models.Contracts
{
    public class BaseContract : BaseModel
    {
       
        [AuditNameAttribute(" Contract Number")]
        public string ContractNumber { get; set; }

        [AuditNameAttribute("  Contract Holding Company Code")]
        public string ContractHoldingCompanyCode { get; set; }

        [AuditNameAttribute(" Contract Holding Company Name")]
        public string ContractHoldingCompanyName { get; set; }

        [AuditNameAttribute("Is Contract Holding Company Active")]
        public bool IsContractHoldingCompanyActive { get; set; }

        [AuditNameAttribute("  Contract Customer Code")]
        public string ContractCustomerCode { get; set; }

        [AuditNameAttribute(" Contract Customer Name")]
        public string ContractCustomerName { get; set; }

        [AuditNameAttribute("Customer Contract Number")]
        public string CustomerContractNumber { get; set; }

        [AuditNameAttribute("Parent Contract Id")]
        public int? ParentContractId { get; set; }

        [AuditNameAttribute("Parent Contract Number")]
        public string ParentContractNumber { get; set; }

        [AuditNameAttribute("Contract Start Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? ContractStartDate { get; set; }

        [AuditNameAttribute("Contract End Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? ContractEndDate { get; set; }

        [AuditNameAttribute("Contract Type")]
        public string ContractType { get; set; }

        [AuditNameAttribute("Contract Status")]
        public string ContractStatus { get; set; }

        [AuditNameAttribute("iConnect Opp Ref")]
        public Int64? ContractCRMReference { get; set; }

        //private decimal? _contractCRMReference = null;
    }
}
