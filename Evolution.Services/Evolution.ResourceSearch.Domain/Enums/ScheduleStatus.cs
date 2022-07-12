using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Evolution.ResourceSearch.Domain.Enums
{
    public enum ScheduleStatus
    {
        [Display(Name = "Available")]
        Available=1,

        [Display(Name = "Tentative")]
        Tentative =2,

        [Display(Name = "TBA")]
        TBA = 3,

        [Display(Name = "PTO")]
        PTO = 4,

        [Display(Name = "Confirmed")]
        Confirmed = 5,

    }
}
