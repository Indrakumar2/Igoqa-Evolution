namespace Evolution.AuthorizationService.Interfaces
{
    public interface IActiveDirectoryService
    {
        bool ConnectToLDAPServer();

        bool ValidateLogin(string username, string password, out bool isDisabledAdAccount, bool validatePswd = false);
    }
}
