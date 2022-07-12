using Evolution.AuthorizationService.Interfaces;
using Evolution.AuthorizationService.Models.Tokens;
using Evolution.Common.Helpers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Evolution.AuthorizationService.Services
{
    public class JwtHandler : IJwtHandler
    {
        //private readonly TokenOption _options;        
        //private readonly SecurityKey _securityKey;
        //private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthClientRepository _clientRpository;

        public JwtHandler(IAuthClientRepository clientRpository)//, IOptions<TokenOption> options, IPasswordHasher<User> passwordHasher)
        {
            _clientRpository = clientRpository;
            //_options = options.Value;
            //_passwordHasher = passwordHasher;
        }

        public JsonWebToken Create(TokenOption tokenOption,ref long refreshTokenExprAsServerLocal)
        {
            var accessTokenGenrationTime =(long) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            var accessTokenExp = (long)(DateTime.UtcNow.AddMinutes(Convert.ToInt32(tokenOption.AccessTokenExpMins)) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            var refreshTokenExp = refreshTokenExprAsServerLocal=(long)(DateTime.UtcNow.AddMinutes(Convert.ToInt32(tokenOption.RefreshTokenExpMins)) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;             
            var userTimeZone = DateTimeHelper.ValidateTimeZoneAndReturnDefault(string.Empty);
            var payload = new JwtPayload
            {
                {JwtRegisteredClaimNames.Sub, tokenOption.Subject},
                {JwtRegisteredClaimNames.Iss, tokenOption.TokenIssuer},
                {JwtRegisteredClaimNames.Iat, accessTokenGenrationTime},
                {JwtRegisteredClaimNames.Exp,accessTokenExp },
                {JwtRegisteredClaimNames.UniqueName, tokenOption.UniqueName},
                {JwtRegisteredClaimNames.Aud, tokenOption.Audience},
                {"ccode", tokenOption.DefualtCompCode},
                {"cid", tokenOption.DefaultCompanyId},
                {"uculture", tokenOption.DefaultLangCulture},
                {"utype", tokenOption.UserType},
                {"uapp", tokenOption.Application},
                {"TokenTimeZone", TimeZoneInfo.Local.Id},
            };
            
            var jwtHeader = new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOption.SecretKey)), SecurityAlgorithms.HmacSha256));
            var jwt = new JwtSecurityToken(jwtHeader, payload);            
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JsonWebToken
            {
                AccessToken = token,
                AccessTokenExpires = accessTokenExp,
                RefreshToken = this.GetRefreshToken(Guid.NewGuid()),
                RefreshTokenExpires = refreshTokenExp
            };
        }

        private string GetRefreshToken(Guid input)
        {
            byte[] source = input.ToByteArray();

            var encoder = new SHA256Managed();
            byte[] encoded = encoder.ComputeHash(source);

            return Convert.ToBase64String(encoded);
        }        
    }
}
