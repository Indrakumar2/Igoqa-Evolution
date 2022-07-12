using Evolution.Common.Models.Base;

namespace Evolution.Security.Domain.Models.Security
{
    public class ActivityInfo : BaseModel
    {
        public int ActivityId { get; set; }

        [AuditNameAttribute("Application Name")]
        public string ApplicationName { get; set; }

        [AuditNameAttribute("System Role Code")]
        public string ActivityCode { get; set; }

        [AuditNameAttribute("System Role")]
        public string ActivityName { get; set; }

        [AuditNameAttribute("System Role Description")]
        public string Description { get; set; }

        [AuditNameAttribute("Module Id")]
        public string ModuleId { get; set; }
    }
}
