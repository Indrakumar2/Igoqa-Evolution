using Evolution.Common.Models.Base;

namespace Evolution.Assignment.Domain.Models.Assignments
{
    public class AssignmentTaxonomy : BaseModel
    {
        [AuditNameAttribute("Assignment Taxonomy Id")]
        public int? AssignmentTaxonomyId { get; set; }

        [AuditNameAttribute("Assignment Id")]
        public int? AssignmentId { get; set; }

        [AuditNameAttribute("Taxonomy Category")]
        public string TaxonomyCategory { get; set; }

        [AuditNameAttribute("Taxonomy Sub Category")]
        public string TaxonomySubCategory { get; set; }
        
        [AuditNameAttribute("Taxonomy Service")]
        public string TaxonomyService { get; set; }

        [AuditNameAttribute("Taxonomy Service Id")]
        public int TaxonomyServiceId { get; set; }

        [AuditNameAttribute("Taxonomy Sub Category Id")]
        public int TaxonomySubCategoryId { get; set; }

        [AuditNameAttribute("Taxonomy Category Id")]
        public int TaxonomyCategoryId { get; set; }

    }
}
