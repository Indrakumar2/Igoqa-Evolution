using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistTaxonomy
    {
        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public int TaxonomyCategoryId { get; set; }
        public int TaxonomySubCategoryId { get; set; }
        public int TaxonomyServicesId { get; set; }
        public string ApprovalStatus { get; set; }
        public string Interview { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DisplayOrder { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string Comments { get; set; }
        public string TaxonomyStatus { get; set; }
        public string ApprovedBy { get; set; }

        public virtual Data TaxonomyCategory { get; set; }
        public virtual TaxonomyService TaxonomyServices { get; set; }
        public virtual TaxonomySubCategory TaxonomySubCategory { get; set; }
        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
