using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyOffice
    {
        public CompanyOffice()
        {
            ContractCompanyOffice = new HashSet<Contract>();
            ContractFrameworkCompanyOffice = new HashSet<Contract>();
            Project = new HashSet<Project>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string OfficeName { get; set; }
        public string AccountRef { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public string PostalCode { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public int? CountryId { get; set; }
        public int? CountyId { get; set; }

        public virtual City City { get; set; }
        public virtual Company Company { get; set; }
        public virtual Country Country { get; set; }
        public virtual County County { get; set; }
        public virtual ICollection<Contract> ContractCompanyOffice { get; set; }
        public virtual ICollection<Contract> ContractFrameworkCompanyOffice { get; set; }
        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
