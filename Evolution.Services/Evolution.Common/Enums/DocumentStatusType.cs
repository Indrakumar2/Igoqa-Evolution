using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Enums
{
    public enum DocumentStatusType
    {
        [Display(Name ="Created")]
        CR,
        [Display(Name = "Inprogress")]
        IN,
        [Display(Name = "Completed")]
        C,
        [Display(Name = "Cancel")]
        CL,
        [Display(Name = "Failed")]
        F,
        [Display(Name = "Approved")]
        APP,
        [Display(Name = "Rejected")]
        R,
        [Display(Name = "CompletedDraft")]
        CD,
        [Display(Name = "Extracted")]
        EXT,
    }

    public enum DocumentType
    {
        TS_EducationQualification,
        TS_InternalTraining,
        TS_Competency,
        TS_CustomerApproval,
        TS_Certificate,
        TS_Training,
        TS_Stamp,
        TS_ProfessionalAfiliation,
        TS_CertVerification,
        TS_TrainingVerification
    }
}
