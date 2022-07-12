namespace Evolution.AuthorizationService.Models.Tokens
{
    public class TokenOption
    {
        public string SecretKey { get; set; }

        public int? AccessTokenExpMins { get; set; }

        public int? RefreshTokenExpMins { get; set; }
        
        public string TokenIssuer { get; set; }

        public string Audience { get; set; }

        public string Subject { get; set; }

        public string UniqueName { get; set; }

        public string DefualtCompCode { get; set; }

        public string DefaultLangCulture { get; set; }

        public string UserType { get; set; }

        public string Application { get; set; }

        public int DefaultCompanyId { get; set; }
    }
}
