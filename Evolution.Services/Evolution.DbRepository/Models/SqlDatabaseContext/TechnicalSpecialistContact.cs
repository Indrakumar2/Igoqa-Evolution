using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TechnicalSpecialistContact
    {
        public int Id { get; set; }
        public int TechnicalSpecialistId { get; set; }
        public string Address { get; set; }
        public int? CountryId { get; set; }
        public int? CountyId { get; set; }
        public int? CityId { get; set; }
        public string PostalCode { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string MobileNumber { get; set; }
        public string EmergencyContactName { get; set; }
        public string ContactType { get; set; }
        public int? DisplayOrder { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public bool? IsGeoCordinateSync { get; set; }

        public virtual City City { get; set; }
        public virtual Country Country { get; set; }
        public virtual County County { get; set; }
        public virtual TechnicalSpecialist TechnicalSpecialist { get; set; }
    }
}
