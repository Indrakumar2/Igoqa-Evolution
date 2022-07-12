using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Enums
{
    public enum MongoSyncStatusType
    {
        [Display(Name = "In Progress")]
        In_Progress,

        [Display(Name = "Failed–Retryable")]
        Failed_Retryable,

        [Display(Name = "Failed–Non-Retryable")]
        Failed_Non_Retryable,

        [Display(Name = "Yet To Process")]
        Yet_To_Process,

        [Display(Name = "Synced")]
        Synced,

    }
}
