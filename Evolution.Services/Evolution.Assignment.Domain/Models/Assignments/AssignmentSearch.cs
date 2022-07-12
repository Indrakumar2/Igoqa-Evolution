using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentSearch : BaseModel
    {
        // TBC: if combination of project Number & Assignment  umber used than not needed

        [AuditNameAttribute("Assignment Id ")]
        public int? AssignmentId { get; set; }

        public string AssignmentFormattedNumber { get; set; }

        [AuditNameAttribute("Assignment Number ")]
        public int? AssignmentNumber { get; set; }

        [AuditNameAttribute("Company Address Id ")]
        public int? AssignmentCompanyAddressId { get; set; }

        [AuditNameAttribute("Company Address  ")]
        public string AssignmentCompanyAddress { get; set; }

        [AuditNameAttribute("Assignment Type ")]
        public string AssignmentType { get; set; }

        [AuditNameAttribute("Assignment LifecycleId ")]
        public int? AssignmentLifecycleId { get; set; }

        [AuditNameAttribute("Assignment Lifecycle ")]
        public string AssignmentLifecycle { get; set; }

        [AuditNameAttribute("Customer Code")]
        public string AssignmentCustomerCode { get; set; }

        [AuditNameAttribute("Customer Name")]
        public string AssignmentCustomerName { get; set; }

        [AuditNameAttribute("Contract Holding Company Id")]
        public int? AssignmentContractHoldingCompanyId { get; set; }

        [AuditNameAttribute("Contract Holding Company Code")]
        public string AssignmentContractHoldingCompanyCode { get; set; }

        [AuditNameAttribute("Contract Holding Company")]
        public string AssignmentContractHoldingCompany { get; set; }

        // ITK D - 619
        [AuditNameAttribute("Is Contract Holding Company Active")]
        public bool IsContractHoldingCompanyActive { get; set; }

        [AuditNameAttribute("Contract NumberId")]
        public int? AssignmentContractId { get; set; }

        [AuditNameAttribute("Contract Number")]
        public string AssignmentContractNumber { get; set; }

        [AuditNameAttribute("Project Id")]
        public int? AssignmentProjectId { get; set; }

        [AuditNameAttribute("Project Number")]
        public int? AssignmentProjectNumber { get; set; }

        [AuditNameAttribute("Customer Project Number")]
        public string AssignmentCustomerProjectNumber { get; set; }

        [AuditNameAttribute("Customer Project Name")]
        public string AssignmentCustomerProjectName { get; set; }

        [AuditNameAttribute("Supplier PO Id ")]
        public int? AssignmentSupplierPurchaseOrderId { get; set; }

        [AuditNameAttribute("Supplier PO Number")]
        public string AssignmentSupplierPurchaseOrderNumber { get; set; }

        [AuditNameAttribute("Supplier PO Material Description ")]
        public string MaterialDescription { get; set; }

        public IList<TechnicalSpecialist> TechSpecialists { get; set; }

        [AuditNameAttribute("Resource Name ")]
        public string TechnicalSpecialistName { get; set; }

        [AuditNameAttribute("Assignment Status")]
        public string AssignmentStatus { get; set; }

        [AuditNameAttribute("Is Assignment Completed ")]
        public bool? IsAssignmentCompleted { get; set; }

        [AuditNameAttribute("Contract Holding Company Coordinator Id ")]
        public int? AssignmentContractHoldingCompanyCoordinatorId { get; set; }

        [AuditNameAttribute("Contract Holding Company Coordinator ")]
        public string AssignmentContractHoldingCompanyCoordinator { get; set; }

        [AuditNameAttribute("Contract Holding Company Coordinator Sam Account Name ")]
        public string AssignmentContractHoldingCompanyCoordinatorSamAcctName { get; set; }

        [AuditNameAttribute("Contract Holding Company Coordinator Code ")]
        public string AssignmentContractHoldingCompanyCoordinatorCode { get; set; }

        [AuditNameAttribute("Operating Company Coordinator Id")]
        public int? AssignmentOperatingCompanyCoordinatorId { get; set; }

        [AuditNameAttribute("Operating Company Coordinator ")]
        public string AssignmentOperatingCompanyCoordinator { get; set; }
        
        [AuditNameAttribute("Operating Company Coordinator Code ")]
        public string AssignmentOperatingCompanyCoordinatorCode { get; set; }

        [AuditNameAttribute("Operating Company Id")]
        public int? AssignmentOperatingCompanyId { get; set; }

        [AuditNameAttribute("Operating Company Code")]
        public string AssignmentOperatingCompanyCode { get; set; }

        [AuditNameAttribute("Operating Company")]
        public string AssignmentOperatingCompany { get; set; }

        // ITK D - 619
        [AuditNameAttribute("Is Operating Company Active")]
        public bool IsOperatingCompanyActive { get; set; }

        [AuditNameAttribute("Host Company Id")]
        public int? AssignmentHostCompanyId { get; set; }

        [AuditNameAttribute("Host Company Code")]
        public string AssignmentHostCompanyCode { get; set; }

        [AuditNameAttribute("Host Company")]
        public string AssignmentHostCompany { get; set; }

        [AuditNameAttribute("Assignment Reference")]
        public string AssignmentReference { get; set; }

        [AuditNameAttribute("Customer Assignment ContactId")]
        public int? AssignmentCustomerAssigmentContactId { get; set; }

        [AuditNameAttribute("Customer Assignment Contact")]
        public string AssignmentCustomerAssigmentContact { get; set; }

        [AuditNameAttribute("Assignment Main Supplier Name ")]
        public string AssignmentSupplierName { get; set; }

        [AuditNameAttribute("Main Supplier Id ")]
        public int? AssignmentSupplierId { get; set; }

        [AuditNameAttribute("Is Inactive Assignment Only")]
        public bool IsInactiveAssignmentOnly { get; set; }

        [AuditNameAttribute("WorkFlow Type In")]
        public string WorkFlowTypeIn { get; set; }

        [AuditNameAttribute("Search Document Type")]
        public string SearchDocumentType { get; set; }

        [AuditNameAttribute("Document Search Text")]
        public string DocumentSearchText { get; set; }
       
        public IList<string> AssignmentIDs { get; set; }
        
        public DateTime? AssignmentLastVisitDate { get; set; }

        public DateTime? AssignmentFirstVisitDate { get; set; }

        public DateTime? AssignmentLastVisitFromDate { get; set; }
        
        public int? AssignmentPercentageCompleted { get; set; }

        [AuditNameAttribute("Expected Complete Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? AssignmentExpectedCompleteDate { get; set; }

        // ITK D - 689
        [AuditNameAttribute("Is Only View Assignment")]
        public bool IsOnlyViewAssignment { get; set; }

        // ITK D - 689
        [AuditNameAttribute("Logged In Company Code")]
        public string LoggedInCompanyCode { get; set; }

        public bool isVisitTimesheet { get; set; }

        [AuditNameAttribute("Is Internal Assignment")]
        public bool IsInternalAssignment { get; set; }
    }

    public class AssignmentEditSearch :BaseModel
    {
        [AuditNameAttribute("Assignment Id ")]
        public int? AssignmentId { get; set; }

        [AuditNameAttribute("Assignment Number ")]
        public int? AssignmentNumber { get; set; }

        [AuditNameAttribute("Customer Name")]
        public string AssignmentCustomerName { get; set; }

        [AuditNameAttribute("Contract Holding Company")]
        public string AssignmentContractHoldingCompany { get; set; }

        [AuditNameAttribute("Contract Holding Company Code")]
        public string AssignmentContractHoldingCompanyCode { get; set; }

        // ITK D - 619
        [AuditNameAttribute("Is Contract Holding Company Active")]
        public bool IsContractHoldingCompanyActive { get; set; }

        [AuditNameAttribute("Project Number")]
        public int? AssignmentProjectNumber { get; set; }

        [AuditNameAttribute("Supplier PO Id ")]
        public int? AssignmentSupplierPurchaseOrderId { get; set; }

        [AuditNameAttribute("Supplier PO Number")]
        public string AssignmentSupplierPurchaseOrderNumber { get; set; }

        [AuditNameAttribute("Supplier PO Material Description ")]
        public string MaterialDescription { get; set; }

        public IList<TechnicalSpecialist> TechSpecialists { get; set; }

        [AuditNameAttribute("Assignment Status")]
        public string AssignmentStatus { get; set; }

        [AuditNameAttribute("Is Assignment Completed ")]
        public bool? IsAssignmentCompleted { get; set; }

        [AuditNameAttribute("Operating Company Code")]
        public string AssignmentOperatingCompanyCode { get; set; }

        [AuditNameAttribute("Operating Company")]
        public string AssignmentOperatingCompany { get; set; }

        public string AssignmentContractHoldingCompanyCoordinator { get; set; }

        public string AssignmentOperatingCompanyCoordinator { get; set; }

        // ITK D - 619
        [AuditNameAttribute("Is Operating Company Active")]
        public bool IsOperatingCompanyActive { get; set; }

        [AuditNameAttribute("Assignment Reference")]
        public string AssignmentReference { get; set; }

         [AuditNameAttribute("Search Document Type")]
        public string SearchDocumentType { get; set; }

        [AuditNameAttribute("Document Search Text")]
        public string DocumentSearchText { get; set; }

        public IList<int> AssignmentIDs { get; set; }

        public DateTime? AssignmentLastVisitDate { get; set; }

        public DateTime? AssignmentFirstVisitDate { get; set; }

        public DateTime? AssignmentLastVisitFromDate { get; set; }

        public int? AssignmentPercentageCompleted { get; set; }

        [AuditNameAttribute("Expected Complete Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? AssignmentExpectedCompleteDate { get; set; }

        // ITK D - 689
        [AuditNameAttribute("Is Only View Assignment")]
        public bool IsOnlyViewAssignment { get; set; }

        // ITK D - 689
        [AuditNameAttribute("Logged In Company Code")]
        public string LoggedInCompanyCode { get; set; }

        public bool isVisitTimesheet { get; set; }

        public string TechnicalSpecialistName { get; set; }

        [AuditNameAttribute("WorkFlow Type In")]
        public string WorkFlowTypeIn { get; set; }

        public int AssignmentContractHoldingCompanyId { get; set; }

        public int AssignmentOperatingCompanyId { get; set; }

        public int AssignmentContractHoldingCompanyCoordinatorId { get; set; }

        public int AssignmentOperatingCompanyCoordinatorId { get; set; }

        public int? AssignmentCustomerId { get; set; }

        public int LoggedInCompanyId { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int ServiceId { get; set; }
        [AuditNameAttribute("Is Internal Assignment")]
        public bool? IsInternalAssignment { get; set; }

    }
}
