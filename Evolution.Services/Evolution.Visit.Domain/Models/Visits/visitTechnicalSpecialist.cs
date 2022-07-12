using Evolution.Common.Models.Base;
using System.Collections.Generic;

namespace Evolution.Visit.Domain.Models.Visits
{
    public class VisitTechnicalSpecialist : BaseModel
    {
        [AuditNameAttribute("Visit Technical Specialist Id")]
        public long? VisitTechnicalSpecialistId { get; set; }
        [AuditNameAttribute("Visit Id")]
        public long VisitId { get; set; }
        /// <summary>
        ///Define the Technical specialist
        /// </summary>
        /// 
        [AuditNameAttribute("Technical Specialist Name")]
        public string TechnicalSpecialistName { get; set; }
        /// <summary>
        ///Defines the gross margin
        /// </summary>
        /// 
        public decimal GrossMargin { get; set; }
        [AuditNameAttribute("ePin")]
        public int Pin { get; set; }
        public string LoginName { get; set; }
        public string FullName { get; set; }
    }

    public class VisitTechnicalSpecialistGrossMargin
    {
        public VisitTechnicalSpecialistGrossMargin()
        {
            VisitTechnicalSpecialists = new List<VisitTechnicalSpecialist>();
        }
        [AuditNameAttribute("Visit Account Gross Margin")]
        public decimal VisitAccountGrossMargin { get; set; }
       
        public IList<VisitTechnicalSpecialist> VisitTechnicalSpecialists { get; set; }
    }

    public  class TechnicalSpecialist
    {
        [AuditNameAttribute("Visit Id")]
        public long VisitId { get; set; }
        [AuditNameAttribute("Full Name")]
        public string FullName
        {
            get
            {
                return string.Format("{0}, {1} ({2})", LastName, FirstName, Pin);
            }
            private set { }
        }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }
        [AuditNameAttribute("ePin")]
        public int Pin { get; set; }
        [AuditNameAttribute("Technical Specialist Name")]
        public string TechnicalSpecialistName { get; set; }

        public string LoginName { get; set; }
    }
}