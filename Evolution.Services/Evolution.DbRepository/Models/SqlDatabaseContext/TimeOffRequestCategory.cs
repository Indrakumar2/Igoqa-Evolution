using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class TimeOffRequestCategory
    {
        public int Id { get; set; }
        public int EmploymentTypeId { get; set; }
        public int LeaveCategoryTypeId { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Data EmploymentType { get; set; }
        public virtual Data LeaveCategoryType { get; set; }
    }
}
