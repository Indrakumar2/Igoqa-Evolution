namespace Evolution.Security.Domain.Models.Security
{
    public class UserPermissionInfo
    {
        public string Module { get; set; }

        public string Activity { get; set; }

        public bool HasPermission { get; set; }
    }

}
