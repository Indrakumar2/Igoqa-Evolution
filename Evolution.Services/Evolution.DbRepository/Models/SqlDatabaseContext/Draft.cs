using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class Draft
    {
        public int Id { get; set; }
        public string Moduletype { get; set; }
        public string Description { get; set; }
        public string SerilizableObject { get; set; }
        public string SerilizationType { get; set; }
        public string DraftId { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedTo { get; set; }
        public string ModifiedBy { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedTo { get; set; }
        public DateTime? AssignedOn { get; set; }
        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }
    }
}
