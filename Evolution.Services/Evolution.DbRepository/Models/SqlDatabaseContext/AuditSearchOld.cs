using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class AuditSearchOld
    {
        public int Id { get; set; }
        public int? ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string SearchName { get; set; }
        public string DisplayName { get; set; }
        public DateTime? LastModification { get; set; }
        public byte? UpdateCount { get; set; }
        public string ModifiedBy { get; set; }

        public virtual SqlauditModule Module { get; set; }
        public virtual SqlauditModuleOld ModuleNavigation { get; set; }
    }
}
