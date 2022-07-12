using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Evolution.Admin.Domain.Enums
{
    public enum UserType
    {
        [Display(Name = "MICoordinator")]
        MICoordinator = 1,
    }
}
