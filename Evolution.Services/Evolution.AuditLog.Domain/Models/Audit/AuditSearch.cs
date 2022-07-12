using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Base;

namespace Evolution.AuditLog.Domain.Models.Audit
{
    public class AuditSearch : BaseModel
    {
        public int? AuditSearchId { get; set; }
        public string Module { get; set; }
        public int ModuleId { get; set; }
        public string SearchName { get; set; }
        public string DispalyName { get; set; }
    }
}