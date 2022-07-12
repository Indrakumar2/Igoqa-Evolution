using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class City
    {
        public City()
        {
            Assignment = new HashSet<Assignment>();
            CompanyOffice = new HashSet<CompanyOffice>();
            CustomerAddress = new HashSet<CustomerAddress>();
            Supplier = new HashSet<Supplier>();
            TechnicalSpecialistContact = new HashSet<TechnicalSpecialistContact>();
            TechnicalSpecialistEducationalQualification = new HashSet<TechnicalSpecialistEducationalQualification>();
        }

        public int Id { get; set; }
        public int CountyId { get; set; }
        public string Name { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual County County { get; set; }
        public virtual ICollection<Assignment> Assignment { get; set; }
        public virtual ICollection<CompanyOffice> CompanyOffice { get; set; }
        public virtual ICollection<CustomerAddress> CustomerAddress { get; set; }
        public virtual ICollection<Supplier> Supplier { get; set; }
        public virtual ICollection<TechnicalSpecialistContact> TechnicalSpecialistContact { get; set; }
        public virtual ICollection<TechnicalSpecialistEducationalQualification> TechnicalSpecialistEducationalQualification { get; set; }
    }
}
