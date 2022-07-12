using System.ComponentModel.DataAnnotations;

namespace Evolution.Admin.Domain.Enums
{
    public enum MICoordinatorStatus
    {
        [Display(Name = "InActive")]
        InActive=1,

        [Display(Name = "DO NOT USE")]
        DONOTUSE =2
    }
}
