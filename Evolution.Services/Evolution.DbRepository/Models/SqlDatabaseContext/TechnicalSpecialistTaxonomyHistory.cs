using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistTaxonomyHistory
    {
        public int HistoryId { get; set; }
        public int? TaxonomyId { get; set; }
        public int? TechnicalSpecialistId { get; set; }
        public int? TaxonomyCategoryId { get; set; }
        public int? TaxonomySubCategoryId { get; set; }
        public int? TaxonomyServicesId { get; set; }
        public string ApprovalStatus { get; set; }
        public string Interview { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DisplayOrder { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string Comments { get; set; }
        public string ModifiedBy { get; set; }
        public string ApprovedBy { get; set; }
    }
}
