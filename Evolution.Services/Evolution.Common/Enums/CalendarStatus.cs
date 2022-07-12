using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Enums
{
    public enum CalendarStatus
    {
        [Display(Name = "PTO")]
        PTO = 1,
        [Display(Name = "Confirmed")]
        Confirmed = 2,
        [Display(Name = "Tentative")]
        Tentative = 3,
        [Display(Name = "TBA")]
        TBA=4,
        [Display(Name = "Pre-Assignment")]
        PREASGMNT = 5, 
        [Display(Name = "Available")]
        Available = 100
    }
}
