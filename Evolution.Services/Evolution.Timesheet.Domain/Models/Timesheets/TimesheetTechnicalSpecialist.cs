using Evolution.Common.Models.Base;
using System.Collections;
using System.Collections.Generic;

namespace Evolution.Timesheet.Domain.Models.Timesheets
{
    public class TimesheetTechnicalSpecialist : BaseModel
    {
        [AuditNameAttribute("Timesheet Technical Specialist Id")]
        public long? TimesheetTechnicalSpecialistId { get; set; }
        [AuditNameAttribute("Timesheet Id")]
        public long? TimesheetId { get; set; }
        [AuditNameAttribute("Technical Specialist Name")]
        public string TechnicalSpecialistName { get; set; }
        [AuditNameAttribute("ePin")]
        public int Pin { get; set; }
        public decimal GrossMargin { get; set; }
        public string LoginName { get; set; }
    }

    public class TimesheetTechnicalSpecialistGrossMargin
    {
       
        public TimesheetTechnicalSpecialistGrossMargin()
        {
            TimesheetTechnicalSpecialists = new List<TimesheetTechnicalSpecialist>();
        }
        [AuditNameAttribute("Account Gross Margin")]
        public decimal TimesheetAccountGrossMargin { get; set; }
      
        public IList<TimesheetTechnicalSpecialist> TimesheetTechnicalSpecialists { get; set; }
    }

    public class TechnicalSpecialist
    {
        [AuditNameAttribute("Timesheet Id")]
        public long? TimesheetId { get; set; }
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
      
        public string LastName { get; set; }
    
        public int? Pin { get; set; }

        public string ProfileStatus { get; set; }

        public string LoginName { get; set; }
    }
}