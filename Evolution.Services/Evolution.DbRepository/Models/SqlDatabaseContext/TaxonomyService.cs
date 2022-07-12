using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TaxonomyService
    {
        public TaxonomyService()
        {
            AssignmentTaxonomy = new HashSet<AssignmentTaxonomy>();
            ResourceSearch = new HashSet<ResourceSearch>();
            TechnicalSpecialistTaxonomy = new HashSet<TechnicalSpecialistTaxonomy>();
        }

        public int Id { get; set; }
        public int TaxonomySubCategoryId { get; set; }
        public string TaxonomyServiceName { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual TaxonomySubCategory TaxonomySubCategory { get; set; }
        public virtual ICollection<AssignmentTaxonomy> AssignmentTaxonomy { get; set; }
        public virtual ICollection<ResourceSearch> ResourceSearch { get; set; }
        public virtual ICollection<TechnicalSpecialistTaxonomy> TechnicalSpecialistTaxonomy { get; set; }
    }
}
