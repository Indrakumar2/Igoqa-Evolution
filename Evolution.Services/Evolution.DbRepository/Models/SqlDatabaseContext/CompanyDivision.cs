using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyDivision
    {
        public CompanyDivision()
        {
            CompanyDivisionCostCenter = new HashSet<CompanyDivisionCostCenter>();
            Project = new HashSet<Project>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int DivisionId { get; set; }
        public string AccountReference { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Company Company { get; set; }
        public virtual Data Division { get; set; }
        public virtual ICollection<CompanyDivisionCostCenter> CompanyDivisionCostCenter { get; set; }
        public virtual ICollection<Project> Project { get; set; }
    }
}
