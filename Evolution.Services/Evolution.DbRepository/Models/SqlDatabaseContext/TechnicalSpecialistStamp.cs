using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistStamp
    {
        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public bool IsSoftStamp { get; set; }
        public int CountryId { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int? DisplayOrder { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string StampNumber { get; set; }

        public virtual Data Country { get; set; }
        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
