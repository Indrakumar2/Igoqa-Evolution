using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Enums
{
    public enum ResourceProfileStatus
    {
        [Display(Name = "Active")]
        Active = 3919,

        [Display(Name = "DNP TMR")]
        DNP_TMR = 3918,

        [Display(Name = "Inactive")]
        Inactive = 3916,

        [Display(Name = "Pending TMR")]
        Pending_TMR = 3915,

        [Display(Name = "Pre-qualification")]
        Pre_qualification = 3914,

        [Display(Name = "Suspended")]
        Suspended = 3917

    }
}
