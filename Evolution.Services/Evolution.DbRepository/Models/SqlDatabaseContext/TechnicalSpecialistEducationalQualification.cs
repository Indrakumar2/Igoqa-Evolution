using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistEducationalQualification
    {
        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public string Qualification { get; set; }
        public string Institution { get; set; }
        public decimal? Percentage { get; set; }
        public string Address { get; set; }
        public int? CountryId { get; set; }
        public int? CountyId { get; set; }
        public int? CityId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? DisplayOrder { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string Place { get; set; }

        public virtual City City { get; set; }
        public virtual Country Country { get; set; }
        public virtual County County { get; set; }
        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
