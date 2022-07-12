using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Task
    {
        public int Id { get; set; }
        public string Moduletype { get; set; }
        public string TaskType { get; set; }
        public string TaskRefCode { get; set; }
        public int? AssignedById { get; set; }
        public int? AssignedToId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public string Description { get; set; }
        public int? CompanyId { get; set; }

        public virtual User AssignedBy { get; set; }
        public virtual User AssignedTo { get; set; }
        public virtual Company Company { get; set; }
    }
}
