using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Enums
{
    public enum ModuleCodeType
    {
        None,
        [Display(Name = "Assignment")]
        ASGMNT,
        CNT,
        COMP,
        CUST,
        PRJ,
        SUP,
        SUPPO,
        TS,
        [Display(Name = "Visit")]
        VST,
        RSEARCH,
        [Display(Name = "Timesheet")]
        TIME,
        AUTH,
        ATTACH
    }
}
