using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Enums
{
    public enum FileExtensionType
    {        
        [Display(Name = "Text")]
        TXT,
        [Display(Name = "Word")]
        DOCX,
        [Display(Name = "Legacy Word")]
        DOC,
        [Display(Name = "Excel")]
        XLSX,
        [Display(Name = "Legacy Excel")]
        XLS,
        [Display(Name = "PDF")]
        PDF,
        [Display(Name = "Outlook Email")]
        MSG
    }
}
