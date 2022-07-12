using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ResourceSearch
    {
        public ResourceSearch()
        {
            OverrideResource = new HashSet<OverrideResource>();
            ResourceSearchNote = new HashSet<ResourceSearchNote>();
        }

        public int Id { get; set; }
        public string ActionStatus { get; set; }
        public string SearchType { get; set; }
        public string SerilizableObject { get; set; }
        public string SerilizationType { get; set; }
        public string DispositionType { get; set; }
        public int CompanyId { get; set; }
        public int? CustomerId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? ServiceId { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedTo { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }
        public string AssignedToOm { get; set; }
        public int? AssignmentId { get; set; }

        public virtual Data Category { get; set; }
        public virtual Company Company { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual TaxonomyService Service { get; set; }
        public virtual TaxonomySubCategory SubCategory { get; set; }
        public virtual ICollection<OverrideResource> OverrideResource { get; set; }
        public virtual ICollection<ResourceSearchNote> ResourceSearchNote { get; set; }
    }
}
