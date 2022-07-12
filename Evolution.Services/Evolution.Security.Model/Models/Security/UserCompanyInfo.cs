namespace Evolution.Security.Domain.Models.Security
{
    public class UserCompanyInfo
    {
        public int? CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string UserLogonName { get; set; }
    }
}