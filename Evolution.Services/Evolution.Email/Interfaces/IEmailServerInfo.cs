namespace Evolution.Email.Interfaces
{
    public interface IEmailServerInfo
    {
        string EmailServerName { get; set; }

        int EmailServerPort { get; set; }

        string EmailUsername { get; set; }

        string EmailUserPassword { get; set; }

        bool IsEmailUseSslWrappedConnection { get; set; }

        bool IsSandBoxEnvironment { get; set; }

        string SandBoxEnvirnonmentTOEmail { get; set; }

        string SandBoxEnvirnonmentCCEmail { get; set; }

        bool IsAuthenticationRequired { get; set; }
    }
}
