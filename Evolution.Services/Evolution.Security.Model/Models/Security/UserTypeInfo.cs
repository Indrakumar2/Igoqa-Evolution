using Evolution.Common.Models.Base;

namespace Evolution.Security.Domain.Models.Security
{
    public class UserTypeInfo : BaseModel
    {
        [AuditNameAttribute("Logon Name")]
        public string UserLogonName { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("User Type")]
        public string UserType { get; set; }

        [AuditNameAttribute("Active")]
        public bool IsActive { get; set; }

        public string UserName { get; set; } //Added for ITK Defect 908(Ref ALM Document 14-05-2020)
    }
}
