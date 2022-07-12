using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Security.Domain.Interfaces.Security
{
    public interface IRefreshTokenService
    {
        bool isValidToken(string token, string userName);
    }
}
