using Evolution.Common.Models.Base;
using Evolution.Common.Extensions;
using System;

namespace Evolution.Document.Domain.Models.Document
{
    public class ModuleDocument : BaseModel
    {
        public long? Id { get; set; }

        [AuditNameAttribute("Document Name")]
        public string DocumentName { get; set; }

        [AuditNameAttribute("Document Type")]
        public string DocumentType { get; set; }

        [AuditNameAttribute("Document Size")]
        public long? DocumentSize { get; set; }

        [AuditNameAttribute("Is Visible To TS")]
        public bool? IsVisibleToTS { get; set; }

        [AuditNameAttribute("Is Visible To Customer")]
        public bool? IsVisibleToCustomer { get; set; }

        [AuditNameAttribute("Is Visible Out Of Company")]
        public bool? IsVisibleOutOfCompany { get; set; }

        [AuditNameAttribute("Status")]
        public string Status { get; set; }

        public string DocumentUniqueName { get; set; }

        [AuditNameAttribute("Module Code")]
        public string ModuleCode { get; set; }

        [AuditNameAttribute("Module Ref Code")]
        public string ModuleRefCode { get; set; }

        public string SubModuleRefCode { get; set; }

        [AuditNameAttribute("Created Date")]
        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public int? DisplayOrder { get; set; }

        [AuditNameAttribute("Comments")]
        public string Comments { get; set; }

        [AuditNameAttribute("Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        [AuditNameAttribute("IsDeleted")]
        public bool? IsDeleted { get; set; } //Sanity defect 148 fix

        public bool CanDelete
        {
            get
            {
                DateTime dt = DateTime.Now;
                int currentYear = dt.Year;
                int createdDateYear = CreatedOn.HasValue ? CreatedOn.Value.Year : 0;
                int currentQuarter = dt.GetQuarter();
                int createdDateQuarter = CreatedOn.HasValue ? CreatedOn.Value.GetQuarter() : 0;
                int previousQuarter = CreatedOn.HasValue ? CreatedOn.Value.GetPreviousQuarter() : 0;
                if (currentYear == createdDateYear && (currentQuarter == createdDateQuarter))
                    return true;
                else
                {
                    if (previousQuarter > currentQuarter)
                    {
                        int previousQuarterYear = currentYear - 1;
                        if (createdDateYear == previousQuarterYear || currentYear == createdDateYear)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        if (previousQuarter < currentQuarter && currentYear == createdDateYear)
                            return true;
                        return false;
                    }
                }
            }
        }

        public bool? IsForApproval { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public string ApprovedBy { get; set; }

        public string CoordinatorName { get; set; }

        public string DocumentTitle { get; set; }

        public string FilePath { get; set; }

        public string CompanyCode { get; set; }

    }
}
