using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyExpectedMargin
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public decimal MinimumMargin { get; set; }
        public int MarginTypeId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Company Company { get; set; }
        public virtual Data MarginType { get; set; }
    }
}
