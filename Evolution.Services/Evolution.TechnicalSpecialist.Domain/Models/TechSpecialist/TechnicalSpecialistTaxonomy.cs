using Evolution.Common.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistTaxonomyInfo : BaseTechnicalSpecialistModel
    {
        [AuditNameAttribute("Technical Specialist Taxonomy Info Id")]
        public int Id { get; set; }
        [AuditNameAttribute("ePin")]
        public int Epin { get; set; }
        [AuditNameAttribute("Taxonomy Category")]
        public string TaxonomyCategoryName { get; set; }
        [AuditNameAttribute("Taxonomy Sub Category")]
        public string TaxonomySubCategoryName { get; set; }
        [AuditNameAttribute("Taxonomy Services")]
        public string TaxonomyServices { get; set; }
        [AuditNameAttribute("Approval Status")]
        public string ApprovalStatus { get; set; }
        [AuditNameAttribute("Interview")]
        public string Interview { get; set; }
        [AuditNameAttribute("Comments")]
        public string Comments { get; set; }
        [AuditNameAttribute("From Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? FromDate { get; set; }
        [AuditNameAttribute("To Date", "dd-MMM-yyyy", AuditNameformatDataType.DateTime)]
        public DateTime? ToDate { get; set; }
        //[AuditNameAttribute("Display Order")]
        public int DisplayOrder { get; set; }
        [AuditNameAttribute("Taxonomy Status")]
        public string TaxonomyStatus { get; set; }
        [AuditNameAttribute("ApprovedBy")]
        public string ApprovedBy { get; set; }

    }
}
