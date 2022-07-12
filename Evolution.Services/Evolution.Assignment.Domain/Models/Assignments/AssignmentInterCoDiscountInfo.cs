using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentInterCoDiscountInfo : BaseModel
    {
        [AuditNameAttribute("Assignment Id")]
        public int? AssignmentId { get; set; }

        [AuditNameAttribute("Parent Contract Holding Company Name")]
        public string ParentContractHoldingCompanyName { get; set; }

        [AuditNameAttribute("Parent Contract Holding Company Code")]
        public string ParentContractHoldingCompanyCode { get; set; }

        [AuditNameAttribute("Parent Contract Holding Company Discount")]
        public decimal? ParentContractHoldingCompanyDiscount { get; set; }

        [AuditNameAttribute("Parent Contract Holding Company Description")]
        public string ParentContractHoldingCompanyDescription { get; set; }

        [AuditNameAttribute("Contract Holding Company ")]
        public string AssignmentContractHoldingCompanyName { get; set; }

        [AuditNameAttribute("Contract Holding Company Code")]
        public string AssignmentContractHoldingCompanyCode { get; set; }
        
        [AuditNameAttribute("Contract Holding Company Discount")]
        public decimal? AssignmentContractHoldingCompanyDiscount { get; set; }

        [AuditNameAttribute("Contract Holding Company Description")]
        public string AssignmentContractHoldingCompanyDescription { get; set; }

        [AuditNameAttribute("Operating Company Code")]
        public string AssignmentOperatingCompanyCode { get; set; }

        [AuditNameAttribute("Operating Company ")]
        public string AssignmentOperatingCompanyName { get; set; }

        [AuditNameAttribute("Operating Company Discount")]
        public decimal? AssignmentOperatingCompanyDiscount { get; set; }

        [AuditNameAttribute("Additional Intercompany1 ")]
        public string AssignmentAdditionalIntercompany1_Name { get; set; }

        [AuditNameAttribute("Additional Intercompany1 Code")]
        public string AssignmentAdditionalIntercompany1_Code { get; set; }

        [AuditNameAttribute("Additional Intercompany1 Discount")]
        public decimal? AssignmentAdditionalIntercompany1_Discount { get; set; }

        [AuditNameAttribute("Additional Intercompany1 Description")]
        public string AssignmentAdditionalIntercompany1_Description { get; set; }

        [AuditNameAttribute("Additional Intercompany2 ")]
        public string AssignmentAdditionalIntercompany2_Name { get; set; }

        [AuditNameAttribute("Additional Intercompany2 Code")]
        public string AssignmentAdditionalIntercompany2_Code { get; set; }

        [AuditNameAttribute("Additional Intercompany2 Discount")]
        public decimal? AssignmentAdditionalIntercompany2_Discount { get; set; }

        [AuditNameAttribute("Additional Inter Company2 Description ")]
        public string AssignmentAdditionalIntercompany2_Description { get; set; }

        [AuditNameAttribute("Host Company ")]
        public string AssignmentHostcompanyName { get; set; }

        [AuditNameAttribute("Host Company Code")]
        public string AssignmentHostcompanyCode { get; set; }

        [AuditNameAttribute("Host Company Discount")]
        public decimal? AssignmentHostcompanyDiscount { get; set; }
        
        [AuditNameAttribute("Host Company Description")]
        public string AssignmentHostcompanyDescription { get; set; }

        [AuditNameAttribute("Amendment Reason")]
        public string AmendmentReason { get; set; }
    }
}
