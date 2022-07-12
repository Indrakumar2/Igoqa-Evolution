namespace Evolution.AuthorizationService.Models.Tokens
{
    public class RefreshToken
    {
        public string Token { get; set; }

        public string Username { get; set; }

        public string RequestedIp { get; set; }

        public long TokenExpiretime { get; set; }

        public string Application { get; set; }

        public string AccessToken { get; set; }
    }
}
