using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class CompanyDivisionCostCenter
    {
        public CompanyDivisionCostCenter()
        {
            Project = new HashSet<Project>();
        }

        public int Id { get; set; }
        public int CompanyDivisionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual CompanyDivision CompanyDivision { get; set; }
        public virtual ICollection<Project> Project { get; set; }
    }
}
