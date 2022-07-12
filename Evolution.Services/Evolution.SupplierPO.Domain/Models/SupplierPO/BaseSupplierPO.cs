using Evolution.Common.Models.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Evolution.SupplierPO.Domain.Models.SupplierPO
{
    public class BaseSupplierPO : BaseModel
    {
        [AuditNameAttribute("Supplier PO Id")]
        public int? SupplierPOId { get; set; }

        [AuditNameAttribute("Supplier PO Number")]
        public string SupplierPONumber { get; set; }

        [AuditNameAttribute("Project Number ")]
        public int SupplierPOProjectNumber { get; set; }

        [AuditNameAttribute("Customer Code ")]
        public string SupplierPOCustomerCode { get; set; }

        [AuditNameAttribute("Customer Name")]
        public string SupplierPOCustomerName { get; set; }

        [AuditNameAttribute("Customer Project Number")]
        public string SupplierPOCustomerProjectNumber { get; set; }

        [AuditNameAttribute("Customer Project Name")]
        public string SupplierPOCustomerProjectName { get; set; }

        [AuditNameAttribute("Contract Number ")]
        public string SupplierPOContractNumber { get; set; }

        [AuditNameAttribute("Delivery Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? SupplierPODeliveryDate { get; set; }

        [AuditNameAttribute("Completed Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? SupplierPOCompletedDate { get; set; }

        [AuditNameAttribute("Main Supplier Id ")]
        public int SupplierPOMainSupplierId { get; set; } // ITK D-744

        [AuditNameAttribute("Main Supplier Name")]
        public string SupplierPOMainSupplierName { get; set; }

        [AuditNameAttribute("Sub-Supplier Name")]
        public String SupplierPOSubSupplierName { get; set; }

        [AuditNameAttribute("Material Description")]
        public string SupplierPOMaterialDescription { get; set; }

        [AuditNameAttribute("Status")]
        public string SupplierPOStatus { get; set; }

        [AuditNameAttribute("Company Name")]
        public string SupplierPOCompanyName { get; set; }
        
        [AuditNameAttribute("Company Code ")]
        public string SupplierPOCompanyCode { get; set; }

        public int SupplierPOCompanyId { get; set; }

        // D - 619
        [AuditNameAttribute("Is SupplierPO Company Active")]
        public bool IsSupplierPOCompanyActive { get; set; }

        public int? SupplierPOSubSupplierId { get; set; } //Changes for IGO - D905

        public int? SupplierPOCustomerId { get; set; }

        public int ContractCompanyId { get; set; }

    }

    public class SupplierPOSearch : BaseSupplierPO
    {
        [AuditNameAttribute("Supplier PO Ids")]
        public IList<int> SupplierPOIds { get; set; }
        [AuditNameAttribute("Search Document Type")]
        public string SearchDocumentType { get; set; }
        [AuditNameAttribute("Document Search Text")]
        public string DocumentSearchText { get; set; }
    }

    public class HeaderList
    {
        public string Label { get; set; }
        public string Key { get; set; }
    }

    public class Result
    {
        public IList<HeaderList> Header { get; set; }
        public IList<BaseSupplierPO> BaseSupplierPO { get; set; }
    }
}
