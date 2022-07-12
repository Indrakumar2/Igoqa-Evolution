using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Enums
{
    public enum NoteType
    {
        [Display(Name = "Internal Training Details ")]//need to check
        ITD,
        [Display(Name = "Competency Details")]//need to check
        CD,
        [Display(Name = "Certificate Details")]
        CED,
        [Display(Name = "Training Details ")]
        TD,
        [Display(Name = "PayRoll Notes ")]
        PN,
        [Display(Name = "Technical Specilist Notes")]
        TS
 
    }
}
