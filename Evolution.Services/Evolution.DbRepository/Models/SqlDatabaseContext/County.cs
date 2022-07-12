using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class County
    {
        public County()
        {
            Assignment = new HashSet<Assignment>();
            City = new HashSet<City>();
            CompanyOffice = new HashSet<CompanyOffice>();
            Supplier = new HashSet<Supplier>();
            TechnicalSpecialistContact = new HashSet<TechnicalSpecialistContact>();
            TechnicalSpecialistEducationalQualification = new HashSet<TechnicalSpecialistEducationalQualification>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Assignment> Assignment { get; set; }
        public virtual ICollection<City> City { get; set; }
        public virtual ICollection<CompanyOffice> CompanyOffice { get; set; }
        public virtual ICollection<Supplier> Supplier { get; set; }
        public virtual ICollection<TechnicalSpecialistContact> TechnicalSpecialistContact { get; set; }
        public virtual ICollection<TechnicalSpecialistEducationalQualification> TechnicalSpecialistEducationalQualification { get; set; }
    }
}
