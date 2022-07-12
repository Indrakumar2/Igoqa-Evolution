using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class OverrideResource
    {
        public int Id { get; set; }
        public int ResourceSearchId { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ResourceSearch ResourceSearch { get; set; }
        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
