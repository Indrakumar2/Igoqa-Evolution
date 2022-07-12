using Evolution.Common.Models.Base;

namespace Evolution.Company.Domain.Models.Companies
{
    public class CompanyQualification : BaseModel
    {
        [AuditNameAttribute("Company Qualification Id")]
        public int? CompanyQualificationId { get; set; }

        [AuditNameAttribute("Company Code")]
        public string CompanyCode { get; set; }

        [AuditNameAttribute("Qualification")]
        public string Qualification { get; set; }
        
    }
}
