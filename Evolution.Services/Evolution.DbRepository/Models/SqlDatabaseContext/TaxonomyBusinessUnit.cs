using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TaxonomyBusinessUnit
    {
        public int Id { get; set; }
        public int ProjectTypeId { get; set; }
        public int CategoryId { get; set; }
        public DateTime? LastModification { get; set; }
        public string ModifiedBy { get; set; }
        public byte? UpdateCount { get; set; }

        public virtual Data Category { get; set; }
        public virtual Data ProjectType { get; set; }
    }
}
