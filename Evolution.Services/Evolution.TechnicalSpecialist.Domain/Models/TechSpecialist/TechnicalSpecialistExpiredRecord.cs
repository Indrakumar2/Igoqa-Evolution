using System;

namespace Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist
{
    public class TechnicalSpecialistExpiredRecord  
    {
        public int TechnicalSpecialistId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int CompanyId { get; set; }
        public string DocumentType { get; set; }
        public string Email { get; set; }
    }
}
