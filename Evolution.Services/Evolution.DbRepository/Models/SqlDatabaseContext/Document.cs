using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Document
    {
        public Document()
        {
            DocumentLibrary = new HashSet<DocumentLibrary>();
        }

        public long Id { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public bool? IsVisibleToCustomer { get; set; }
        public bool? IsVisibleToTechSpecialist { get; set; }
        public bool? IsVisibleToOutsideOfCompany { get; set; }
        public string Status { get; set; }
        public long? Size { get; set; }
        public string DocumentUniqueName { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleRefCode { get; set; }
        public string SubModuleRefCode { get; set; }
        public DateTime? LastModification { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Comments { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsForApproval { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string ApprovedBy { get; set; }
        public int? CoordinatorId { get; set; }
        public int? UserId { get; set; }
        public string FilePath { get; set; }
        public string DocumentTitle { get; set; }
        public long? Evoid { get; set; }

        public virtual User Coordinator { get; set; }
        public virtual ICollection<DocumentLibrary> DocumentLibrary { get; set; }
    }
}
