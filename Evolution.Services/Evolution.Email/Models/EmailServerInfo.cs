using Evolution.Email.Interfaces;
using System.ComponentModel;

namespace Evolution.Email.Models
{
    public class EmailServerInfo : IEmailServerInfo
    {
        public string EmailServerName { get; set; }

        public int EmailServerPort { get; set; }

        public string EmailUsername { get; set; }

        public string EmailUserPassword { get; set; }

        [DefaultValue(true)]
        public bool IsEmailUseSslWrappedConnection { get; set; }

        public bool IsSandBoxEnvironment { get; set; }

        public string SandBoxEnvirnonmentTOEmail { get; set; }

        public string SandBoxEnvirnonmentCCEmail { get; set; }

        public int SyncIntervalInMinutes { get; set; } = 5;

        public bool IsAuthenticationRequired { get; set; }
    }
}
