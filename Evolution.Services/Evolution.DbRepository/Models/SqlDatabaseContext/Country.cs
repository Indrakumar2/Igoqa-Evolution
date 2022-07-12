using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Country
    {
        public Country()
        {
            Assignment = new HashSet<Assignment>();
            Company = new HashSet<Company>();
            CompanyOffice = new HashSet<CompanyOffice>();
            County = new HashSet<County>();
            Supplier = new HashSet<Supplier>();
            TechnicalSpecialist = new HashSet<TechnicalSpecialist>();
            TechnicalSpecialistContact = new HashSet<TechnicalSpecialistContact>();
            TechnicalSpecialistEducationalQualification = new HashSet<TechnicalSpecialistEducationalQualification>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? RegonId { get; set; }
        public bool? IsEumember { get; set; }
        public string Euvatprefix { get; set; }
        public bool? IsGccmember { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Data Regon { get; set; }
        public virtual ICollection<Assignment> Assignment { get; set; }
        public virtual ICollection<Company> Company { get; set; }
        public virtual ICollection<CompanyOffice> CompanyOffice { get; set; }
        public virtual ICollection<County> County { get; set; }
        public virtual ICollection<Supplier> Supplier { get; set; }
        public virtual ICollection<TechnicalSpecialist> TechnicalSpecialist { get; set; }
        public virtual ICollection<TechnicalSpecialistContact> TechnicalSpecialistContact { get; set; }
        public virtual ICollection<TechnicalSpecialistEducationalQualification> TechnicalSpecialistEducationalQualification { get; set; }
    }
}
