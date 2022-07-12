namespace Evolution.AuthorizationService.Models.Tokens
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public long AccessTokenExpires { get; set; }

        public long RefreshTokenExpires { get; set; }

        public bool isAuthTokenAlreadyExistsForUser { get; set; }
    }
}