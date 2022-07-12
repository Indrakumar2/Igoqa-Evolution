using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Enums
{
    public enum CompCertTrainingType
    {
        [Display(Name = "Certificate")]
        Ce,
        [Display(Name = "Training")]
        Tr,
        [Display(Name = "Competency")]
        Co,
        [Display(Name = "InternalTraining")]
        IT
    }
}