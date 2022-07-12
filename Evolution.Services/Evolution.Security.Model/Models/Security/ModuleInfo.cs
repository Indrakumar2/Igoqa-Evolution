using Evolution.Common.Models.Base;

namespace Evolution.Security.Domain.Models.Security
{
    public class ModuleInfo : BaseModel
    {
        public int ModuleId { get; set; }

        //public int? ApplicationId { get; set; }

        [AuditNameAttribute("Application Name")]
        public string ApplicationName { get; set; }

        [AuditNameAttribute("Module")]
        public string ModuleName { get; set; }

        [AuditNameAttribute("Module Description")]
        public string Description { get; set; }
    }
}
