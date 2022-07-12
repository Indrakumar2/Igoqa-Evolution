using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistCodeAndStandard
    {
        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public int CodeStandardId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }

        public virtual Data CodeStandard { get; set; }
        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
