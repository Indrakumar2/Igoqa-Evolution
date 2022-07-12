using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Enums
{
    public enum CalendarType
    {
      [Display(Name = "PreAssignment”")]
        PRE,
        [Display(Name = "PTO")]
        PTO,
        [Display(Name = "Assignment")]
        ASSIGNMENT,
        [Display(Name = "Visit")]
        VISIT,
        [Display(Name = "Timesheet")]
        TIMESHEET
    }
}
