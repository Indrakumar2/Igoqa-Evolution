using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Enums
{
    public enum DraftType
    {
        [Display(Name = "Task")]
        Task,

        [Display(Name = "Create Profile")]
        CreateProfile,

        [Display(Name = "Edit Profile")]
        TS_EditProfile,

        [Display(Name = "Edit Profile")]
        RCRM_EditProfile,

        [Display(Name = "Edit Profile")]
        TM_EditProfile,

        [Display(Name = "Profile Change History")]
        ProfileChangeHistory
    }
}
