using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyPayroll
    {
        public CompanyPayroll()
        {
            CompanyPayrollPeriod = new HashSet<CompanyPayrollPeriod>();
            TechnicalSpecialist = new HashSet<TechnicalSpecialist>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Currency { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }
        public string ExportPrefix { get; set; }
        public string Name { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<CompanyPayrollPeriod> CompanyPayrollPeriod { get; set; }
        public virtual ICollection<TechnicalSpecialist> TechnicalSpecialist { get; set; }
    }
}
