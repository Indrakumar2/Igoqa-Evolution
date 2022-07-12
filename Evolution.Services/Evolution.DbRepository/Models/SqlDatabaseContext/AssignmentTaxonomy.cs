using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class AssignmentTaxonomy
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int TaxonomyServiceId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Assignment Assignment { get; set; }
        public virtual TaxonomyService TaxonomyService { get; set; }
    }
}
