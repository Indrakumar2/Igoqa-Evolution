using System;
using System.Collections.Generic;
using System.Text;
using DomainModel = Evolution.Timesheet.Domain.Models.Timesheets;

namespace Evolution.Timesheet.Domain.Models.Timesheets
{
    public class TimesheetValidationData
    {
        public List<DomainModel.BaseTimesheet> TimesheetAssignmentDates {get;set;}
        public IList<DomainModel.TimesheetTechnicalSpecialist> TechnicalSpecialists { get; set; }
    }
}