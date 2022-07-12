using System.ComponentModel.DataAnnotations;

namespace Evolution.Contract.Domain.Enums
{
    public enum ContractType
    {
        [Display(Name = "Standard Contract")]
        STD,
        [Display(Name = "Parent Contract")]
        PAR,
        [Display(Name = "Child Contract")]
        CHD,
        [Display(Name = "Framework Contract")]
        FRW,
        [Display(Name = "Related Framework Contract")]
        RFW,
        [Display(Name = "Related Framework Contract")]
        IRF
    }
    
}
