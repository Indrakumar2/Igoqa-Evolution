using Evolution.AuthorizationService.Models.Tokens;

namespace Evolution.AuthorizationService.Interfaces
{
    public interface IJwtHandler
    {
        JsonWebToken Create(TokenOption tokenOption,ref long refreshTokenExprAsServerLocal);
    }
}
