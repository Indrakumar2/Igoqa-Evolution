using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TaxonomySubCategory
    {
        public TaxonomySubCategory()
        {
            ResourceSearch = new HashSet<ResourceSearch>();
            TaxonomyService = new HashSet<TaxonomyService>();
            TechnicalSpecialistTaxonomy = new HashSet<TechnicalSpecialistTaxonomy>();
        }

        public int Id { get; set; }
        public int TaxonomyCategoryId { get; set; }
        public string TaxonomySubCategoryName { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Data TaxonomyCategory { get; set; }
        public virtual ICollection<ResourceSearch> ResourceSearch { get; set; }
        public virtual ICollection<TaxonomyService> TaxonomyService { get; set; }
        public virtual ICollection<TechnicalSpecialistTaxonomy> TechnicalSpecialistTaxonomy { get; set; }
    }
}
