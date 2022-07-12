using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class ApplicationMenu
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int ModuleId { get; set; }
        public string MenuName { get; set; }
        public int? ParentMenuId { get; set; }
        public string ActivitiesCode { get; set; }
        public bool? IsActive { get; set; }

        public virtual Application Application { get; set; }
        public virtual Module Module { get; set; }
    }
}
