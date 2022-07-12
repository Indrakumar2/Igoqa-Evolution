namespace Evolution.Email.Interfaces
{
    public interface IEmailConfiguration
    {
        IEmailServerInfo IncommingServer { get; set; }

        IEmailServerInfo OutgoingServer { get; set; }
    }
}
