
using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Enums
{
    public enum SecurityPermission
    {
        [Display(Name = "New")]
        N00001,
        [Display(Name = "Modify")]
        M00001,
        [Display(Name = "Delete")]
        D00001,
        [Display(Name = "View")]
        V00001,
        [Display(Name = "ViewGlobal")]
        A00001,
        [Display(Name = "View Level3")]
        S00007
    }
}
