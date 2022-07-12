using System;

namespace Evolution.AuthorizationService.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string DefaultCompanyCode { get; set; }
        
        public string Username { get; set; }

        public string UserType { get; set; }

        public string AuthenticationType { get; set; }

        public string PasswordHash{ get; set; }

        public string Application { get; set; }
        
        public int? AccessFailedCount { get; set; }

        public string DisplayName { get; set; }

        public bool? IsActive { get; set; }

        public string Culture { get; set; }

        public bool IsRoleAssigned { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public int DefaultCompanyId { get; set; }

    }
}
