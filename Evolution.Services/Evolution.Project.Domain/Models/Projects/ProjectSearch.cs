using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;

namespace Evolution.Project.Domain.Models.Projects
{
    public class ProjectSearch : BaseModel
    {
        [AuditNameAttribute("Contract Number")]
        public string ContractNumber { get; set; }

        [AuditNameAttribute("Customer Contract Number")]
        public string CustomerContractNumber { get; set; }

        [AuditNameAttribute(" Project Number")]
        public int? ProjectNumber { get; set; }

        public IList<int> ProjectNumbers { get; set; }

        [AuditNameAttribute("Customer Project Number")]
        public string CustomerProjectNumber { get; set; }

        [AuditNameAttribute(" Customer Project Name")]
        public string CustomerProjectName { get; set; }

        [AuditNameAttribute(" Contract Holding CompanyCode")]
        public string ContractHoldingCompanyCode { get; set; }

        [AuditNameAttribute("Contract Holding Company Name")]
        public string ContractHoldingCompanyName { get; set; }

        // ITK D - 619
        [AuditNameAttribute("Is Contract Holding Company Active")]
        public bool IsContractHoldingCompanyActive { get; set; }

        [AuditNameAttribute(" Contract Customer Code")]
        public string ContractCustomerCode { get; set; }

        [AuditNameAttribute(" Contract Customer Name")]
        public string ContractCustomerName { get; set; }

        [AuditNameAttribute(" Project Start Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? ProjectStartDate { get; set; }

        [AuditNameAttribute(" Project End Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? ProjectEndDate { get; set; }
        
        [AuditNameAttribute(" Contract Type")]
        public string ContractType { get; set; }

        [AuditNameAttribute(" Company Office")]
        public string CompanyOffice { get; set; }

        [AuditNameAttribute(" Company Division")]
        public string CompanyDivision { get; set; }

        [AuditNameAttribute(" Project Status")]
        public string ProjectStatus { get; set; }

        [AuditNameAttribute(" Project Type")]
        public string ProjectType { get; set; }

        [AuditNameAttribute(" Project Coordinator Name")]
        public string ProjectCoordinatorName { get; set; }

        [AuditNameAttribute(" Project Coordinator Code")]
        public string ProjectCoordinatorCode { get; set; }

        public string ProjectCoordinatorEmail { get; set; }

        [AuditNameAttribute(" WorkFlowTypeIn")]
        public string WorkFlowTypeIn { get; set; }
       
        [AuditNameAttribute("eReport Project Mapped")]
        public bool IsEReportProjectMapped { get; set; }

        [AuditNameAttribute(" Search Document Type")]
        public string SearchDocumentType { get; set; }

        [AuditNameAttribute(" Document Search Text")]
        public string DocumentSearchText { get; set; }

        public int ContractCustomerId { get; set; }
        public int ProjectTypeId { get; set; }

    }
}
