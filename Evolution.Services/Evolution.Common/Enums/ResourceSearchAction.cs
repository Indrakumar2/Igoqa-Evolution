using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Enums
{
    public enum  ResourceSearchAction
    {
        [Display(Name = "Save Search”")]
        SS,
        [Display(Name = "Create/Update Pre-Assignment")]
        CUP,
        [Display(Name = "Search Disposition")]
        SD,
        [Display(Name = "Won")]
        W,
        [Display(Name = "Lost")]
        L,
        [Display(Name = "Assigne Resource")]
        AR,
        [Display(Name = "Assigned Pre-Assignment")]
        APA,
        [Display(Name = "Override Preferred Resource")]
        OPR,
        [Display(Name = "Potential Lost Opportunity")]
        PLO,
        [Display(Name = "Approve/Reject Resource")]
        ARR,
        [Display(Name = "No match in GRM")]
        NMG,
        [Display(Name = "Save Search and Send to PC")]
        SSSPC,

    }
}
